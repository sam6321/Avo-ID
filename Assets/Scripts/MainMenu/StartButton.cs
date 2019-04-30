using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuButton
{
    [SerializeField]
    AudioClip mouseDownAudio;

    void OnMouseDown()
    {
        audioSource.PlayOneShot(mouseDownAudio);
        GameObject.Find("GameManager").GetComponent<FadeManager>().StartFade(() => SceneManager.LoadScene("MainScene"));
        //SceneManager.LoadScene("MainScene");
    }
}
