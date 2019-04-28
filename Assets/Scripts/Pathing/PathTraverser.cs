using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathTraverser : MonoBehaviour
{
    [SerializeField]
    private PathNode root;

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float onTargetDistance = 2.0f;

    private PathNode target = null;
    private new Rigidbody rigidbody;

    void Start()
    {
        target = root;
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(target)
        {
            float remaining = Vector3.Distance(rigidbody.position, target.transform.position);
            if (remaining < onTargetDistance)
            {
                PathNode[] potentialTargets = target.Next.Where(next => next.NodeEnabled).ToArray();
                target = potentialTargets.Length > 0 ? Utils.RandomElement(potentialTargets) : null;
            }

            if(target)
            {
                rigidbody.velocity = (target.transform.position - rigidbody.position).normalized * Mathf.Min(moveSpeed, remaining * 8.0f);
            }
        }
    }
}
