using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TwoWayJunction : MonoBehaviour
{
    [SerializeField]
    private PathNode openNode;

    [SerializeField]
    private PathNode closedNode;

    [SerializeField]
    private bool closed = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        SetState(closed);
    }

    private void SetState(bool closed)
    {
        openNode.NodeEnabled = !closed;
        closedNode.NodeEnabled = closed;

        animator.SetBool("closed", closed);

        this.closed = closed;
    }

    void OnMouseOver()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            SetState(!closed);
        }
    }

}
