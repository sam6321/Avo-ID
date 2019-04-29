using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PathTraverser : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float onTargetDistance = 2.0f;

    [SerializeField]
    private PathNode target;

    [SerializeField]
    private UnityEvent<PathNode> onHitTarget;

    private new Rigidbody rigidbody;

    public PathNode Target
    {
        get { return target; }
        set { target = value; }
    }

    public UnityEvent<PathNode> OnHitTarget
    {
        get { return onHitTarget; }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(target)
        {
            float remaining = Vector3.Distance(rigidbody.position, Target.transform.position);
            if (remaining < onTargetDistance)
            {
                PathNode[] potentialTargets = Target.Next.Where(next => next.NodeEnabled).ToArray();
                target = potentialTargets.Length > 0 ? Utils.RandomElement(potentialTargets) : null;
                if(!target)
                {
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.useGravity = true;
                }

                onHitTarget.Invoke(target);
            }

            if(target)
            {
                rigidbody.velocity = (target.transform.position - rigidbody.position).normalized * Mathf.Min(moveSpeed, remaining * 8.0f);
            }
        }
    }
}
