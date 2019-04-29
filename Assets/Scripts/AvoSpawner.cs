using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoSpawner : MonoBehaviour
{
    [SerializeField]
    private PathNode spawnNode;

    [SerializeField]
    private GameObject avoPrefab;

    [SerializeField]
    private float spawnRate = 2.0f;

    private float lastSpawn = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= lastSpawn + spawnRate)
        {
            GameObject newAvo = Instantiate(avoPrefab, spawnNode.transform.position, Quaternion.identity);
            PathTraverser traverser = newAvo.GetComponent<PathTraverser>();
            traverser.Target = spawnNode;

            lastSpawn = Time.time;
        }
    }
}
