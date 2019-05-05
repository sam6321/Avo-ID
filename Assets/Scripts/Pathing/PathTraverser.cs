using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathTraverser : MonoBehaviour
{
    [System.Serializable]
    public class OnHitTargetEvent : UnityEvent<PathTraverser, PathNode, PathNode> { }

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float onTargetDistance = 2.0f;

    [SerializeField]
    private PathNode target;

    [SerializeField]
    private OnHitTargetEvent onHitTarget;

    private new Rigidbody rigidbody = null;
    private List<PathNode> potentialTargets = new List<PathNode>();

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    // If these nodes appear, always choose them even when disabled.
    private List<PathNode> overridePath = new List<PathNode>();
    public List<PathNode> OverridePath { get => overridePath; }

    public PathNode Target
    {
        get { return target; }
        set
        {
            target = value;
            if(target == null && rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
            }
        }
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

    Vector3 CalcVelocity()
    {
        if (target)
        {
            return (target.transform.position - rigidbody.position).normalized * moveSpeed;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if(target)
        {
            // Calc current velocity toward target and new position that we should be at the end of this frame.
            Vector3 velocity = CalcVelocity();
            Vector3 newPosition = rigidbody.position + velocity * Time.fixedDeltaTime;

            // Check if this object will move over the target this frame
            float distanceToTarget = Vector3.Distance(rigidbody.position, target.transform.position);
            float distanceToNewPosition = Vector3.Distance(rigidbody.position, newPosition);

            if(distanceToTarget < distanceToNewPosition || distanceToTarget < onTargetDistance)
            {
                // Going to move past the target at our current velocity. So pretend we hit it then begin moving to the 
                // new target
                PathNode[] potentialTargets;
                if(Target.Next.Any(node => overridePath.Contains(node))) {
                    // If any override nodes are in the next path, always choose them.
                    potentialTargets = Target.Next.Where(node => overridePath.Contains(node)).ToArray();
                }
                else
                {
                    // No override path
                    potentialTargets = Target.Next.Where(next => next.NodeEnabled).ToArray();
                }

                PathNode currentTarget = target;
                PathNode nextTarget = potentialTargets.Length > 0 ? Utils.RandomElement(potentialTargets) : null;

                target = nextTarget;

                currentTarget.OnTraverserHitTarget.Invoke(this, currentTarget, nextTarget);
                onHitTarget.Invoke(this, currentTarget, nextTarget);

                // Recalc velocity with new target
                velocity = CalcVelocity();
            }

            if(target)
            {
                rigidbody.position += velocity * Time.fixedDeltaTime;
            }
        }
    }
}
