using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathGraph))]
public class PathGraphEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathGraph graph = target as PathGraph;
        if (GUILayout.Button("Create Node"))
        {
            graph.CreateRoot();
        }
    }
}
