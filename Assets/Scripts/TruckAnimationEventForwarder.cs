using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimationEventForwarder : MonoBehaviour
{
    [SerializeField]
    private GameLogic logic;

    public void OnDriveAwayComplete()
    {
        logic.OnTruckDriveAwayComplete();
    }

    public void OnReturnComplete()
    {
        logic.OnTruckReturnComplete();
    }
}
