using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PathTraverser : MonoBehaviour
{
    [System.Serializable]
    public class OnHitTargetEvent : UnityEvent<PathNode, PathNode> { }

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float onTargetDistance = 2.0f;

    [SerializeField]
    private PathNode target;

    [SerializeField]
    private OnHitTargetEvent onHitTarget;

    private new Rigidbody rigidbody;

    public PathNode Target
    {
        get { return target; }
        set { target = value; }
    }

    public OnHitTargetEvent OnHitTarget
    {
        get { return onHitTarget; }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (onHitTarget == null)
        {
            onHitTarget = new OnHitTargetEvent();
        }
    }

    void FixedUpdate()
    {
        if(target)
        {
            float remaining = Vector3.Distance(rigidbody.position, Target.transform.position);
            if (remaining < onTargetDistance)
            {
                PathNode[] potentialTargets = Target.Next.Where(next => next.NodeEnabled).ToArray();
                PathNode nextTarget = potentialTargets.Length > 0 ? Utils.RandomElement(potentialTargets) : null;
                
                onHitTarget.Invoke(target, nextTarget);

                target = nextTarget;
            }

            if(target)
            {
                rigidbody.velocity = (target.transform.position - rigidbody.position).normalized * Mathf.Min(moveSpeed, remaining * 8.0f);
            }
        }
    }
}
