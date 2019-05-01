using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public void OnHitBin(PathTraverser traverser, PathNode lastNode, PathNode nextNode)
    {
        Rigidbody rigidbody = traverser.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void OnHitTruck(PathTraverser traverser, PathNode lastNode, PathNode nextNode)
    {
        Destroy(traverser.gameObject);
    }
}
