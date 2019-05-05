using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [System.Serializable]
    public class FloatRange
    {
        public float min;
        public float max;

        public float Random()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    [System.Serializable]
    public class RollingAvocadoSpawns
    {
        public Vector3 position;
        public Vector3 forceDirection;
    }

    [SerializeField]
    private GameObject rollingAvocadoPrefab;

    [SerializeField]
    private FloatRange rollingAvocadoSpawnRange;

    [SerializeField]
    private RollingAvocadoSpawns[] rollingAvocadoSpawns;

    private float nextRollingAvocadoSpawn = 10.0f;

    // Update is called once per frame
    void Update()
    {
        RollingAvocado();
    }

    void RollingAvocado()
    {
        if(Time.time >= nextRollingAvocadoSpawn)
        {
            RollingAvocadoSpawns spawn = Utils.RandomElement(rollingAvocadoSpawns);
            GameObject rollingAvo = Instantiate(rollingAvocadoPrefab, spawn.position, Quaternion.identity);
            rollingAvo.GetComponent<ConstantForce>().force = spawn.forceDirection;

            nextRollingAvocadoSpawn = Time.time + rollingAvocadoSpawnRange.Random();
        }
    }
}
