using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGraph : MonoBehaviour
{
    [SerializeField]
    private List<PathNode> roots = new List<PathNode>();

    public PathNode CreateNode()
    {
        GameObject node = new GameObject("Node", typeof(PathNode));
        node.transform.parent = transform;
        PathNode pathNode = node.GetComponent<PathNode>();
        pathNode.Graph = this;
        return pathNode;
    }

    public PathNode CreateRoot()
    {
        PathNode node = CreateNode();
        roots.Add(node);
        return node;
    }

    public bool RemoveRoot(PathNode item)
    {
        return roots.Remove(item);
    }

    public bool MaybeOrphan(PathNode item)
    {
        if (item.Previous.Count == 0 && !roots.Contains(item))
        {
            roots.Add(item);
            return true;
        }
        else
        {
            return false;
        }
    }
}
