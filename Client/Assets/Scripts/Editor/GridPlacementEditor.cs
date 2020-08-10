using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridPlacement))]
public class GridPlacementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridPlacement grid = (GridPlacement)target;
        if (GUILayout.Button("Set"))
        {
            grid.PlaceObjects();
        }
    }
}