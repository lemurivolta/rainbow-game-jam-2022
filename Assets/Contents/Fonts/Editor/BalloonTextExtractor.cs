using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System;

public class BalloonTextExtractor : EditorWindow
{
    [MenuItem("Rainbow Game Jam/Balloon Text Extractor")]
    public static void ShowExample()
    {
        BalloonTextExtractor wnd = GetWindow<BalloonTextExtractor>();
        wnd.titleContent = new GUIContent("Balloon Text Extractor");
    }

    private TextField charactersLabel;
    private Label statusLabel;
    private VisualElement buttonsContainer;
    private ListView linesListView;

    public void CreateGUI()
    {
        try
        {
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Contents/Fonts/Editor/BalloonTextExtractor.uxml");
            VisualElement content = visualTree.Instantiate();
            root.Add(content);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Contents/Fonts/Editor/BalloonTextExtractor.uss");
            root.styleSheets.Add(styleSheet);

            // bind events
            charactersLabel = root.Query<TextField>("characters").First();
            Assert.IsNotNull(charactersLabel, "could not find characters label");
            statusLabel = root.Query<Label>("status").First();
            Assert.IsNotNull(statusLabel, "could not find status label");
            root.Query<Button>("findButton").First().RegisterCallback<ClickEvent>((_) => FindCharacters());

            // bind makeitem for listviews
            buttonsContainer = root.Query<VisualElement>("buttonsContainer").First();
            Assert.IsNotNull(buttonsContainer);
            linesListView = root.Query<ListView>("linesListView").First();
            Assert.IsNotNull(linesListView);
            linesListView.makeItem = MakeLabel;
            linesListView.bindItem = BindLabel;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private VisualElement MakeButton()
    {
        Debug.Log("make button");
        var button = new Button();
        return button;
    }

    private void BindButton(VisualElement visualElement, int index)
    {
        Debug.Log($"Bind button to {visualElement} at index {index}");
        var button = (Button)visualElement;
        Assert.IsNotNull(button);
        var ch = chars[index];
        button.text = ch.ToString();
        button.clicked += () =>
        {
            ShowLinesFor(ch);
        };
    }

    private char currentlySelectedChar;

    private void ShowLinesFor(char ch)
    {
        currentlySelectedChar = ch;
        linesListView.itemsSource = stringsByCharacter[ch];
        linesListView.Rebuild();
    }

    private VisualElement MakeLabel()
    {
        var label = new Label();
        return label;
    }

    private void BindLabel(VisualElement label, int index)
    {
        var line = stringsByCharacter[currentlySelectedChar][index];
        ((Label)label).text = line;
    }

    private List<char> chars;

    private Dictionary<char, List<string>> stringsByCharacter = new();

    private void FindCharacters()
    {
        statusLabel.text = "Computing...";
        HashSet<char> set = new();
        stringsByCharacter = new();
        var prefabGuids = AssetDatabase.FindAssets("t:prefab");
        foreach (var guid in prefabGuids)
        {
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
            var schermataBarks = obj.GetComponent<SchermataBarks>();
            if (schermataBarks)
            {
                var barksSet = new List<Bark>[] {
                    schermataBarks.StartingBarks,
                    schermataBarks.GameOverBarks,
                    schermataBarks.EndingBarks
                };
                foreach (var barks in barksSet)
                {
                    foreach (var bark in barks)
                    {
                        var lines = new string[] { bark.line_eng, bark.line_ita };
                        foreach (var line in lines)
                        {
                            foreach (var ch in line)
                            {
                                set.Add(ch);
                                List<string> value = null;
                                if (!stringsByCharacter.TryGetValue(ch, out value))
                                {
                                    value = new List<string>();
                                    stringsByCharacter[ch] = value;
                                }
                                Assert.IsNotNull(value);
                                value.Add(line);
                            }
                        }
                    }
                }
            }
        }
        chars = new List<char>(set);
        chars.Sort();
        charactersLabel.value = string.Join("", chars);
        statusLabel.text = "";

        buttonsContainer.Clear();
        int i = 0;
        foreach (var ch in chars)
        {
            var item = MakeButton();
            BindButton(item, i++);
            buttonsContainer.Add(item);
        }
    }
}