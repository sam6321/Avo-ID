using System;
using System.Collections.Generic;
using UnityEngine;

class PathGraph : MonoBehaviour
{
    private List<PathNode> nodes = new List<PathNode>();
    private PathNode root = null;

    void Start()
    {
        root = Create(0, 0, 0);

        PathNode node1 = Create(1, 1, 1);
        PathNode node2 = Create(0, 1, 0);
        PathNode node3 = Create(1, 0, 0);
        PathNode node4 = Create(6, 3, 0);

        root.SetLink(node1);
        node1.SetLink(node2);
        node2.SetLink(node3);
        node2.SetLink(node4);
        node4.SetLink(root);
    }

    private PathNode Create(Vector3 position)
    {
        PathNode node = new PathNode() { Position = position };
        nodes.Add(node);
        return node;
    }

    private PathNode Create(float x, float y, float z)
    {
        return Create(new Vector3(x, y, z));
    }

    private void OnDrawGizmos()
    {
        HashSet<PathNode> seen = new HashSet<PathNode>();
        DrawPathNodeGizmo(root, seen);
    }

    private void DrawPathNodeGizmo(PathNode node, HashSet<PathNode> seen)
    {
        if(node != null)
        {
            Gizmos.DrawSphere(node.Position, 0.1f);
            foreach(PathNodeEntry entry in node.Links)
            {
                if (entry.Enabled)
                {
                    Gizmos.DrawLine(node.Position, entry.Node.Position);
                    DrawArrowGizmo(node.Position, entry.Node.Position);
                    if (!seen.Contains(entry.Node))
                    {
                        seen.Add(entry.Node);
                        DrawPathNodeGizmo(entry.Node, seen);
                    }
                }
            }
        }
    }

    Vector3 lineUp = new Vector3(0, 0.1f, -0.1f);
    Vector3 lineDown = new Vector3(0, -0.1f, -0.1f);
    Vector3 lineLeft = new Vector3(0.1f, 0, -0.1f);
    Vector3 lineRight = new Vector3(-0.1f, 0, -0.1f);
    private void DrawArrowGizmo(Vector3 from, Vector3 to)
    {
        Gizmos.matrix = Matrix4x4.TRS((from + to) * 0.5f, Quaternion.LookRotation((to - from).normalized), Vector3.one);
        Gizmos.DrawLine(Vector3.zero, lineUp);
        Gizmos.DrawLine(Vector3.zero, lineDown);
        Gizmos.DrawLine(Vector3.zero, lineLeft);
        Gizmos.DrawLine(Vector3.zero, lineRight);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
