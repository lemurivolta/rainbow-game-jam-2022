using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor.UIElements;
using System.Text;

[CustomEditor(typeof(RuleSystem))]
public class RulesEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var container = new VisualElement()
        {
            style =
            {
                flexDirection = FlexDirection.Column
            }
        };

        var rulesProperty = serializedObject.FindProperty("Rules");
        for (var i = 0; i < rulesProperty.arraySize; i++)
        {
            container.Add(AddRuleToContainer(rulesProperty, i));
        }

        var newRuleContainer = new VisualElement()
        {
            style =
            {
                marginTop = 32
            }
        };
        newRuleContainer.Add(new Button(() =>
        {
            AddRule(container, rulesProperty, new RuleSystem.StateCondition());
        })
        { text = "Add simple rule" });
        newRuleContainer.Add(new Button(() =>
        {
            AddRule(container, rulesProperty, new RuleSystem.MultiStateCondition()
            {
                Kind = RuleSystem.MultiStateCondition.StatesKind.AtLeastOneMustBeOn
            });
        })
        { text = "Add multistate rule" });
        container.Add(newRuleContainer);

        return container;

        static VisualElement AddRuleToContainer(SerializedProperty rulesProperty, int i)
        {
            VisualElement container = new VisualElement();
            var labelContainer = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignContent = Align.Center,
                    marginTop = 14
                }
            };
            labelContainer.Add(new Label($"Rule n. {i + 1}")
            {
                style = {
                    unityFontStyleAndWeight = new StyleEnum<FontStyle>()
                    {
                        value = FontStyle.Bold
                    },
                    fontSize = 14,
                    marginTop = 2
                }
            });
            labelContainer.Add(new Button(() =>
            {
                var x = 0;
                foreach (var child in container.parent.Children())
                {
                    if (child == container)
                    {
                        break;
                    }
                    x++;
                }

                rulesProperty.DeleteArrayElementAtIndex(x);
                rulesProperty.serializedObject.ApplyModifiedProperties();

                container.parent.RemoveAt(x);
            })
            {
                text = "-"
            });
            container.Add(labelContainer);
            var rule = rulesProperty.GetArrayElementAtIndex(i);
            var property = rule.FindPropertyRelative("State");
            var value = property.managedReferenceValue;
            if (value is RuleSystem.StateCondition)
            {
                // add state
                var ruleRowContainer = new VisualElement()
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        alignItems = Align.FlexStart
                    }
                };
                ruleRowContainer.Add(new Label($"Check state:")
                {
                    style =
                    {
                        marginTop = 3
                    }
                });
                var stateProperty = property.FindPropertyRelative("State");
                var stateItem = GetPropertyWithParent(stateProperty, typeof(State));
                ruleRowContainer.Add(stateItem);
                container.Add(ruleRowContainer);
            }
            else if (value is RuleSystem.MultiStateCondition)
            {
                var ruleRowContainer = new VisualElement()
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        alignItems = Align.FlexStart
                    }
                };
                ruleRowContainer.Add(new Label($"Check states:")
                {
                    style =
                    {
                        marginTop = 3
                    }
                });
                var statesProperty = property.FindPropertyRelative("States");
                var verticalContainer = new VisualElement()
                {
                    style = {
                        flexDirection = FlexDirection.Column
                    }
                };

                var kindProp = property.FindPropertyRelative("Kind");
                var enumField = new EnumField()
                {
                    bindingPath = kindProp.propertyPath
                };
                verticalContainer.Add(enumField);
                var stateItems = GetArrayOfPropertiesWithParent(
                    statesProperty, typeof(State), "Add state");
                verticalContainer.Add(stateItems);
                ruleRowContainer.Add(verticalContainer);
                container.Add(ruleRowContainer);
            }
            else
            {
                container.Add(new Label($"have no idea"));
            }
            // add whentrue / whenfalse
            var whenTrueContainer = CreateWhen(
                rule.FindPropertyRelative("WhenTrue"),
                "When true:"
                );
            container.Add(whenTrueContainer);
            var whenFalseContainer = CreateWhen(
                rule.FindPropertyRelative("WhenFalse"),
                "When false:"
                );
            container.Add(whenFalseContainer);

            // summary
            var summary = "When ";
            if (value is RuleSystem.StateCondition)
            {
                try
                {
                    var b = new StringBuilder();
                    var v = (RuleSystem.StateCondition)value;
                    var state = v.State;
                    AddStateDescriptionToBuilder(b, state);
                    summary += b.ToString();
                }
                catch
                {
                    summary += "...";
                }
            }
            else if (value is RuleSystem.MultiStateCondition)
            {
                try
                {
                    var b = new StringBuilder();
                    var v = (RuleSystem.MultiStateCondition)value;
                    var k = 0;
                    foreach (var state in v.States)
                    {
                        AddStateDescriptionToBuilder(b, state);
                        if (++k < v.States.Length)
                        {
                            b.Append(v.Kind == RuleSystem.MultiStateCondition.StatesKind.AllMustBeOn ?
                                " and " : " or ");
                        }
                    }
                    summary += b.ToString();
                }
                catch
                {
                    summary += "...";
                }
            }

            summary += ",\nthen ";
            try
            {
                var b = new StringBuilder();
                var whenTrueProp = rule.FindPropertyRelative("WhenTrue");
                AddWhenDescriptionToBuilder(b, whenTrueProp);
                summary += b.ToString();
            }
            catch
            {
                summary += "...";
            }

            summary += ",\notherwise ";
            try
            {
                var b = new StringBuilder();
                var whenFalseProp = rule.FindPropertyRelative("WhenFalse");
                AddWhenDescriptionToBuilder(b, whenFalseProp);
                summary += b.ToString();
            }
            catch
            {
                summary += "...";
            }

            summary += ".";

            container.Add(new Label(summary)
            {
                style =
                {
                    whiteSpace = WhiteSpace.Normal
                }
            });

            return container;

            static string FromPascalCaseToWords(string n)
            {
                // to implement
                return n.ToLower();
            }

            static void AddStateDescriptionToBuilder(StringBuilder b, State state)
            {
                b.Append(state.gameObject.transform.parent.gameObject.name);
                b.Append(" is ");
                var name = state.name;
                if (name.EndsWith("State"))
                {
                    name = name.Substring(0, name.Length - "State".Length);
                }
                b.Append(FromPascalCaseToWords(name));
            }

            static void AddWhenDescriptionToBuilder(StringBuilder b, SerializedProperty arrayProp)
            {
                for (var n = 0; n < arrayProp.arraySize; n++)
                {
                    var elProp = arrayProp.GetArrayElementAtIndex(n);
                    var o = (MonoBehaviour)elProp.objectReferenceValue;
                    b.Append(FromPascalCaseToWords(o.name));
                    b.Append(" ");
                    b.Append(o.transform.parent.gameObject.name);
                    if (n < arrayProp.arraySize - 1)
                    {
                        b.Append(" and ");
                    }
                }
            }
        }

        static void AddRule(VisualElement container, SerializedProperty rulesProperty, RuleSystem.StateChangeNotifier stateChangeNotifier)
        {
            rulesProperty.InsertArrayElementAtIndex(rulesProperty.arraySize);
            var elementProperty = rulesProperty.GetArrayElementAtIndex(rulesProperty.arraySize - 1);
            elementProperty.FindPropertyRelative("State").managedReferenceValue = stateChangeNotifier;
            elementProperty.FindPropertyRelative("WhenTrue").arraySize = 0;
            elementProperty.FindPropertyRelative("WhenFalse").arraySize = 0;
            rulesProperty.serializedObject.ApplyModifiedProperties();
            container.Insert(
                rulesProperty.arraySize - 1,
                AddRuleToContainer(rulesProperty, rulesProperty.arraySize - 1)
            );
        }
    }

    private static VisualElement CreateWhen(SerializedProperty whenProperty, string label)
    {
        var whenContainer = new VisualElement()
        {
            style = {
                flexDirection = FlexDirection.Row
            }
        };
        whenContainer.Add(new Label(label));
        VisualElement whenItemsContainer =
            GetArrayOfPropertiesWithParent(whenProperty, typeof(Actionable), "Add action");
        whenContainer.Add(whenItemsContainer);
        return whenContainer;
    }

    private static VisualElement GetArrayOfPropertiesWithParent(
        SerializedProperty property, System.Type objectType, string addLabel)
    {
        var itemsContainer = new VisualElement()
        {
            style = {
                flexDirection = FlexDirection.Column
            }
        };
        System.Action<int> removeAt = (int j) =>
        {
            property.DeleteArrayElementAtIndex(j);
            itemsContainer.RemoveAt(j);
            property.serializedObject.ApplyModifiedProperties();
        };
        System.Action<int, int> addAtIndex = (int j, int index) =>
        {
            var prop = property.GetArrayElementAtIndex(j);
            VisualElement itemContainer = GetPropertyWithParent(prop, objectType);
            itemContainer.Add(new Button(() => removeAt(j))
            {
                text = "-",
                style =
                {
                    flexShrink = 0,
                    flexGrow = 0,
                    width = 20,
                }
            });
            if (index < 0)
            {
                itemsContainer.Add(itemContainer);
            }
            else
            {
                itemsContainer.Insert(index, itemContainer);
            }
        };
        for (var j = 0; j < property.arraySize; j++)
        {
            addAtIndex(j, -1);
        }

        itemsContainer.Add(new Button(() =>
        {
            property.InsertArrayElementAtIndex(property.arraySize);
            var elementProperty = property.GetArrayElementAtIndex(property.arraySize - 1);
            elementProperty.objectReferenceValue = null;
            addAtIndex(property.arraySize - 1, property.arraySize - 1);
            property.serializedObject.ApplyModifiedProperties();
        })
        {
            text = addLabel
        });

        return itemsContainer;
    }

    private static VisualElement GetPropertyWithParent(
        SerializedProperty prop, System.Type objectType)
    {
        var itemContainer = new VisualElement()
        {
            style =
                    {
                        flexDirection = FlexDirection.Row,
                        alignItems = Align.Center
                    }
        };
        itemContainer.Add(new ObjectField()
        {
            objectType = objectType,
            allowSceneObjects = true,
            bindingPath = prop.propertyPath,
            style = {
                        flexGrow = 1,
                        flexShrink = 1,
                    }
        });
        var monoBehaviour = (MonoBehaviour)prop?.objectReferenceValue;
        var name = monoBehaviour ? monoBehaviour.gameObject.transform.parent.gameObject.name : null;
        if (name != null)
        {
            itemContainer.Add(new Label(name));
        }

        return itemContainer;
    }
}
