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

    [SerializeField]
    private AudioClip openSound;

    [SerializeField]
    private AudioClip closeSound;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SetState(closed);
    }

    private void SetState(bool closed)
    {
        audioSource.PlayOneShot(closed ? closeSound : openSound);
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
