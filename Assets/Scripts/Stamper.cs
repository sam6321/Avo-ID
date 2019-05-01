using System;
using System.Collections.Generic;
using UnityEngine;

public class Stamper : MonoBehaviour
{
    [SerializeField]
    private Avocado.Labels[] labels;

    [SerializeField]
    private int labelIndex = 0;

    private Animator animator;

    private class TraverserQueueItem
    {
        public PathTraverser traverser;
        public PathNode nextNode;
    }

    private Queue<TraverserQueueItem> queue = new Queue<TraverserQueueItem>();
    private TraverserQueueItem currentItem = null;
    private bool isStamping = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Stamp(PathTraverser traverser, PathNode node, PathNode nextNode)
    {
        // Stop moving where you are now
        traverser.Target = null;

        // Assign the appropriate stamp to this avocado
        Avocado avo = traverser.GetComponent<Avocado>();
        avo.AppliedLabels.Add(labels[labelIndex]);

        TraverserQueueItem item = new TraverserQueueItem()
        {
            traverser = traverser,
            nextNode = nextNode
        };

        if (isStamping)
        {
            // Stamping something else right now, enqueue this for stamping
            queue.Enqueue(item);
        }
        else
        {
            // Set to current and go
            StampInternal(item);
        }
    }

    // Triggered when the stamper has begun moving up and the traverser can move forward
    public void OnStampComplete()
    {
        currentItem.traverser.Target = currentItem.nextNode;
        try
        {
            StampInternal(queue.Dequeue());
        }
        catch (InvalidOperationException e)
        {
            // Queue empty
            isStamping = false;
        }
    }

    private void StampInternal(TraverserQueueItem item)
    {
        currentItem = item;
        isStamping = true;
        animator.SetTrigger("stamp");
    }

    private void OnClick()
    {
        labelIndex = (labelIndex + 1) % labels.Length;
    }
}
