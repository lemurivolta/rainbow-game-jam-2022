using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.Assertions;
using System.Collections.Generic;

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

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Contents/Fonts/Utilities/BalloonTextExtractor.uxml");
        VisualElement content = visualTree.Instantiate();
        root.Add(content);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Contents/Fonts/Utilities/BalloonTextExtractor.uss");
        root.styleSheets.Add(styleSheet);

        // bind events
        charactersLabel = root.Query<TextField>("characters").First();
        Assert.IsNotNull(charactersLabel, "could not find characters label");
        statusLabel = root.Query<Label>("status").First();
        Assert.IsNotNull(statusLabel, "could not find status label");
        root.Query<Button>("findButton").First().RegisterCallback<ClickEvent>((_) => FindCharacters());
    }

    private void FindCharacters()
    {
        statusLabel.text = "Computing...";
        HashSet<char> set = new();
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
                            }
                        }
                    }
                }
            }
        }
        var chars = new List<char>(set);
        chars.Sort();
        charactersLabel.value = string.Join("", chars);
        statusLabel.text = "";
    }
}