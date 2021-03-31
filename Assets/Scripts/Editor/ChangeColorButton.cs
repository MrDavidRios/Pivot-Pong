using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColorSchemeChange))]
public class ChangeColorButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ColorSchemeChange colorSchemeScript = (ColorSchemeChange)target;

        if (GUILayout.Button("Change Color Scheme"))
        {
            colorSchemeScript.ChangeColorScheme(1);
        }
    }
}