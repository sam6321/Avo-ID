using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class PathNode : MonoBehaviour
{
    [System.Serializable]
    public class OnTraverserHitTargetEvent : UnityEvent<PathTraverser, PathNode, PathNode> { };

    [SerializeField]
    public PathGraph Graph;

    [SerializeField]
    private bool nodeEnabled = true;

    [SerializeField]
    private List<PathNode> nextNodes = new List<PathNode>();

    [SerializeField]
    private List<PathNode> previousNodes = new List<PathNode>();

    [SerializeField]
    private OnTraverserHitTargetEvent onTraverserHitTarget = new OnTraverserHitTargetEvent();

    public IReadOnlyCollection<PathNode> Next { get { return nextNodes; } }
    public IReadOnlyCollection<PathNode> Previous { get { return previousNodes; } }
    
    public bool NodeEnabled
    {
        get { return nodeEnabled; }
        set { nodeEnabled = value; }
    }

    public OnTraverserHitTargetEvent OnTraverserHitTarget
    {
        get { return onTraverserHitTarget; }
    }

    void Start()
    {
        // Ensure all of our previous nodes have a link to us as a next node,
        // and all our next nodes have a link to us as previous.
        // Also ensure we have a reference to our parent node graph container.
        EnsureValid();
    }

    void OnDestroy()
    {
        EnsureValid();

        foreach (PathNode next in Next.ToArray())
        {
            next.RemovePrevious(this);
        }

        foreach(PathNode previous in Previous.ToArray())
        {
            previous.RemoveNext(this);
        }

        if (Graph)
        {
            Graph.RemoveRoot(this);
        }
    }

    public bool AddNext(PathNode next)
    {
        if(HasNext(next))
        {
            return false;
        }
        else
        {
            nextNodes.Add(next);
            next.AddPrevious(this);
            return true;
        }
    }

    public bool RemoveNext(PathNode next)
    {
        if(nextNodes.Remove(next))
        {
            next.RemovePrevious(this);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasNext(PathNode next)
    {
        return nextNodes.Contains(next);
    }

    public bool AddPrevious(PathNode previous)
    {
        if(HasPrevious(previous))
        {
            return false;
        }
        else
        {
            previousNodes.Add(previous);
            previous.AddNext(this);
            return true;
        }
    }

    public bool RemovePrevious(PathNode previous)
    {
        if(previousNodes.Remove(previous))
        {
            previous.RemoveNext(this);
            if(Graph)
            {
                Graph.MaybeOrphan(this);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasPrevious(PathNode previous)
    {
        return previousNodes.Contains(previous);
    }

    private void EnsureValid()
    {
        Graph = GetComponentInParent<PathGraph>();

        foreach(PathNode node in Next)
        {
            node.AddPrevious(this);
        }

        foreach(PathNode node in Previous)
        {
            node.AddNext(this);
        }
    }

    #region Gizmos

    static readonly Vector3 lineUp = new Vector3(0, 0.1f, -0.1f);
    static readonly Vector3 lineDown = new Vector3(0, -0.1f, -0.1f);
    static readonly Vector3 lineLeft = new Vector3(0.1f, 0, -0.1f);
    static readonly Vector3 lineRight = new Vector3(-0.1f, 0, -0.1f);
    static readonly Color reduceColour = new Color(0.5f, 0.5f, 0.5f);

    private bool GetAncestorDisabled(PathNode node)
    {
        foreach(PathNode previous in node.Previous)
        {
            if(previous.NodeEnabled)
            {
                return GetAncestorDisabled(previous);
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        bool ancestorDisabled = GetAncestorDisabled(this);

        Gizmos.color = Previous.Count == 0 ? Color.yellow : Color.green;

        if(!NodeEnabled)
        {
            Gizmos.color = Color.red;
        }
        else if(ancestorDisabled)
        {
            Gizmos.color = Color.red + Color.green * 0.5f; // orange
        }

        Gizmos.DrawSphere(transform.position, 0.1f);
        foreach (PathNode next in Next)
        {
            Gizmos.color = Color.white;
            if (ancestorDisabled || !next.NodeEnabled || !NodeEnabled)
            {
                Gizmos.color *= reduceColour;
            }

            Vector3 from = transform.position;
            Vector3 to = next.transform.position;
            Gizmos.DrawLine(from, to);
            Gizmos.matrix = Matrix4x4.TRS((from + to) * 0.5f, Quaternion.LookRotation((to - from).normalized), Vector3.one);
            Gizmos.DrawLine(Vector3.zero, lineUp);
            Gizmos.DrawLine(Vector3.zero, lineDown);
            Gizmos.DrawLine(Vector3.zero, lineLeft);
            Gizmos.DrawLine(Vector3.zero, lineRight);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

    #endregion
}
