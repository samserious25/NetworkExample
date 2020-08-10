using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WordMaker))]
public class GlossaryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WordMaker glossary = (WordMaker)target;

        if (GUILayout.Button("Get word"))
        {
            glossary.GetWord();
        }

        if (GUILayout.Button("Clear"))
        {
            glossary.DisableBoxes();
        }
    }
}