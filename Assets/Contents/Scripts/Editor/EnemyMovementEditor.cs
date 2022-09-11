using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyMovement)), CanEditMultipleObjects]
public class EnemyMovementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var pathProperty = serializedObject.FindProperty("Path");
        var path = (Transform)pathProperty.objectReferenceValue;

        if (GUILayout.Button("Print path world positions"))
        {
            for (var i = 0; i < path.childCount; i++)
            {
                var child = path.GetChild(i);
                Debug.Log($"{child.name}: ({child.position.x}, {child.position.y})");
            }
        }

        base.OnInspectorGUI();
    }
}
