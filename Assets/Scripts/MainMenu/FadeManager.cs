using System;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private Action onFadeCompleteAction;

    public void StartFade(Action onFadeComplete=null)
    {
        Debug.Log("Fading");
        onFadeCompleteAction = onFadeComplete;
        animator.SetBool("Fade", true);
    }

    public void OnFadeComplete()
    {
        Debug.Log("Faded");
        animator.SetBool("Fade", false);
        if (onFadeCompleteAction != null)
        {
            onFadeCompleteAction();
        }
    }
}
