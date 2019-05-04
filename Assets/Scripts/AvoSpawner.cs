using UnityEngine;

public class AvoSpawner : MonoBehaviour
{
    [System.Serializable]
    public class WeightedRandomGameObject : WeightedRandomEntry<GameObject> { }

    [SerializeField]
    private PathNode spawnNode;

    [SerializeField]
    private WeightedRandomGameObject[] spawnableObjectSelection;
    private WeightedRandom<GameObject> spawnableObjects;

    [SerializeField]
    private float spawnRate = 2.0f;

    private float lastSpawn = 0.0f;

    void Start()
    {
        lastSpawn = -spawnRate;
        spawnableObjects = new WeightedRandom<GameObject>(spawnableObjectSelection);
    }

    void Update()
    {
        if(Time.time >= lastSpawn + spawnRate)
        {
            // Pick a weighted random object to spawn
            GameObject newAvo = Instantiate(spawnableObjects.GetItem(), spawnNode.transform.position, Quaternion.identity);
            PathTraverser traverser = newAvo.GetComponent<PathTraverser>();
            traverser.Target = spawnNode;
            lastSpawn = Time.time;
        }
    }
}
