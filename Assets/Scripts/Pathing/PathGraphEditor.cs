﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathGraph))]
public class PathGraphEditor : Editor
{
    static readonly Vector3 positionMask = new Vector3(1, 0, 1);

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathGraph graph = target as PathGraph;
        if (GUILayout.Button("Create Node"))
        {
            PathNode node = graph.CreateRoot();
            node.transform.position = graph.transform.position + Vector3.Scale(Random.onUnitSphere, positionMask);
            Selection.activeGameObject = node.gameObject;
        }
    }
}
