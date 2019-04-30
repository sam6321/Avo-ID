using System.Collections;
using UnityEngine;

public class AvoSpawner : MonoBehaviour
{
    [SerializeField]
    private PathNode spawnNode;

    [SerializeField]
    private GameObject avoPrefab;

    [SerializeField]
    private float spawnRate = 2.0f;

    [SerializeField]
    private Stamper stamper1;

    private float lastSpawn = 0.0f;

    private bool canSpawn = true;

    void Update()
    {
        if(Time.time >= lastSpawn + spawnRate && canSpawn)
        {
            GameObject newAvo = Instantiate(avoPrefab, spawnNode.transform.position, Quaternion.identity);
            PathTraverser traverser = newAvo.GetComponent<PathTraverser>();
            Rigidbody rigidbody = newAvo.GetComponent<Rigidbody>();
            traverser.Target = spawnNode;
            traverser.OnHitTarget.AddListener((lastNode, nextNode) =>
            {
                switch(lastNode.gameObject.name)
                {
                    case "Node_Bin":
                        rigidbody.velocity = Vector3.zero;
                        rigidbody.useGravity = true;
                        break;

                    case "Node_Truck":
                        Destroy(newAvo);
                        break;

                    case "Node_Stamper1":
                        stamper1.Stamp(traverser, nextNode);
                        break;

                    default:
                        break;
                }
            });

            lastSpawn = Time.time;
        }
    }
}
