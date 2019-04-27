using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathNodeEntry
{
    private PathNode node;
    private bool enabled;

    public PathNodeEntry(PathNode node, bool enabled)
    {
        this.node = node;
        this.enabled = enabled;
    }

    public PathNode Node
    {
        get { return node; }
    }

    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }
}

[Serializable]
public class PathNode
{
    private static long nextId = 0;

    private long id = nextId++;
    private List<PathNodeEntry> links = new List<PathNodeEntry>();
    public Vector3 Position { get; set; }
    public IReadOnlyList<PathNodeEntry> Links { get { return links; } }

    public void SetLink(PathNode next, bool enabled=true)
    {
        PathNodeEntry link = GetLink(next);
        if(link == null)
        {
            link = new PathNodeEntry(next, enabled);
            links.Add(link);
        }
        else
        {
            link.Enabled = enabled;
        }
    }

    public bool RemoveLink(PathNode next)
    {
        int index = links.FindIndex(entry => entry.Node == next);
        if(index >= 0)
        {
            links.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasLink(PathNode next)
    {
        return GetLink(next) != null;
    }

    private PathNodeEntry GetLink(PathNode next)
    {
        return links.Find(entry => entry.Node == next);
    }
}
