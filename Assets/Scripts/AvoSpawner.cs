using UnityEngine;
using System.Collections.Generic;

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

    [SerializeField]
    private int maxSpawned = 1;

    private float lastSpawn = 0.0f;
    private List<AvoSpawnerTag> spawnedItems = new List<AvoSpawnerTag>();

    public int MaxSpawned { get => maxSpawned; set => maxSpawned = value; }
    public float MovementSpeed { get; set; } = 1.0f;
    public float SpawnRate { get => spawnRate; set => spawnRate = value; }

    void Start()
    {
        lastSpawn = -spawnRate;
        spawnableObjects = new WeightedRandom<GameObject>(spawnableObjectSelection);
    }

    void Update()
    {
        if((Time.time >= lastSpawn + spawnRate && spawnedItems.Count < maxSpawned) || spawnedItems.Count == 0)
        {
            // Pick a weighted random object to spawn
            GameObject newObject = Instantiate(spawnableObjects.GetItem(), spawnNode.transform.position, Quaternion.identity);
            PathTraverser traverser = newObject.GetComponent<PathTraverser>();
            traverser.Target = spawnNode;
            traverser.MoveSpeed = MovementSpeed;

            AvoSpawnerTag tag = newObject.AddComponent<AvoSpawnerTag>();
            tag.Spawner = this;
            spawnedItems.Add(tag);

            lastSpawn = Time.time;
        }
    }
    
    public void ResetLastSpawned()
    {
        lastSpawn = Time.time - spawnRate;
    }

    public void DestroyAllSpawnedItems()
    {
        foreach(AvoSpawnerTag tag in spawnedItems)
        {
            DestroyOnBinned dob = tag.GetComponent<DestroyOnBinned>();
            if(dob)
            {
                // Destroy immediately
                dob.PerformDestroy(0);
            }
            else
            {
                Destroy(dob.gameObject);
            }
        }

        spawnedItems.Clear();
    }

    public void OnSpawnedItemDestroyed(AvoSpawnerTag item)
    {
        int index = spawnedItems.IndexOf(item);
        if(index >= 0)
        {
            spawnedItems.RemoveAt(index);
        }
    }
}
