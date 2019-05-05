using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpOnHold : MonoBehaviour
{
    public void OnHoldDown()
    {
        Time.timeScale = 3.0f;
    }

    public void OnHoldUp()
    {
        Time.timeScale = 1.0f;
    }
}
