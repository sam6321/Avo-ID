using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MenuButton
{
    [SerializeField]
    AudioClip mouseDownAudio;

    void OnMouseDown()
    {
        audioSource.PlayOneShot(mouseDownAudio);
        Application.Quit();
    }
}
