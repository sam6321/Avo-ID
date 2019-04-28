using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathNode))]
public class PathNodeEditor : Editor
{
    static readonly Vector3 positionMask = new Vector3(1, 0, 1);

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathNode node = target as PathNode;
        if (GUILayout.Button("Create Node"))
        {
            PathNode next = node.Graph.CreateNode();
            node.AddNext(next);
            next.transform.position = node.transform.position + Vector3.Scale(Random.onUnitSphere, positionMask);
            Selection.activeGameObject = next.gameObject;
        }
    }
}
