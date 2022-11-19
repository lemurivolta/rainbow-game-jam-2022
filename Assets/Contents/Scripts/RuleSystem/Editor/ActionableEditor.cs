using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Actionable))]
public class ActionableEditor : Editor
{
    private Actionable actionable;
    private void OnEnable()
    {
        actionable = (Actionable)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Perform Action"))
        {
            actionable.PerformAction();
        }
    }
}