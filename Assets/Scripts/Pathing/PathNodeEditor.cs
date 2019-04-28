using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathNode))]
public class PathNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathNode node = target as PathNode;
        if (GUILayout.Button("Create Node"))
        {
            PathNode next = node.Graph.CreateNode();
            node.AddNext(next);
        }
    }
}
