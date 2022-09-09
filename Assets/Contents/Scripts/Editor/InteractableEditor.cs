using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{

    public override void OnInspectorGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            var properties = new SerializedProperty[] {
                serializedObject.FindProperty("CanCloeInteract"),
                serializedObject.FindProperty("CanYelenaInteract"),
                serializedObject.FindProperty("CanSarahInteract"),
                serializedObject.FindProperty("CanMarielleInteract")
            };

            if (GUILayout.Button("All characters"))
            {
                foreach (var property in properties)
                {
                    property.boolValue = true;
                }
                serializedObject.ApplyModifiedProperties();
            }
            if (GUILayout.Button("No characters"))
            {
                foreach (var property in properties)
                {
                    property.boolValue = false;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }

        base.OnInspectorGUI();
    }
}
