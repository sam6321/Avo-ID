using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingAvocado : MonoBehaviour
{
    // Update is called once per frame
    private float startTime = 0.0f;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if(Time.time > startTime + 30.0f)
        {
            Destroy(gameObject);
        }
    }
}
