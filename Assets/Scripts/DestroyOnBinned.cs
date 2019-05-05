using System.Collections;
using UnityEngine;

public class DestroyOnBinned : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 10.0f;

    public void OnHitTarget(PathTraverser traverser, PathNode current, PathNode next)
    {
        if(current.CompareTag("Bin"))
        {
            // Also remove this item from the avo spawner, because otherwise another won't spawn until this one has poofed
            AvoSpawnerTag tag = GetComponent<AvoSpawnerTag>();
            if(tag)
            {
                tag.Spawner.OnSpawnedItemDestroyed(tag);
            }
            PerformDestroy(despawnTime);
        }
    }

    public void PerformDestroy(float waitTime)
    {
        StartCoroutine(Despawn(waitTime));
    }

    IEnumerator Despawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("fade");
    }

    public void OnFadeComplete()
    {
        Destroy(gameObject);
    }
}
