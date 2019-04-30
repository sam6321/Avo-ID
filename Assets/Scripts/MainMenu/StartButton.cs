using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuButton
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("MainScene");
    }
}
