using System.Collections;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 10.0f;

    public void OnHitTarget(PathTraverser traverser, PathNode current, PathNode next)
    {
        if(current.CompareTag("Bin"))
        {
            StartCoroutine(Despawn());
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("fade");
    }

    public void OnFadeComplete()
    {
        Destroy(gameObject);
    }
}
