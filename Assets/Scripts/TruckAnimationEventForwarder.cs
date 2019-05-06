using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAnimationEventForwarder : MonoBehaviour
{
    [SerializeField]
    private GameLogic logic;

    [SerializeField]
    private AudioClip driveAwaySound;

    [SerializeField]
    private AudioClip returnSound;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void OnDriveAwayBegin()
    {
        source.PlayOneShot(driveAwaySound);
    }

    public void OnDriveAwayComplete()
    {
        logic.OnTruckDriveAwayComplete();
    }

    public void OnReturnBegin()
    {
        source.PlayOneShot(returnSound);
    }

    public void OnReturnComplete()
    {
        logic.OnTruckReturnComplete();
    }
}
