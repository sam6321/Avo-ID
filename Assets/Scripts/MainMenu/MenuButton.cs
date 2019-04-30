using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    Sprite offSprite;

    [SerializeField]
    Sprite onSprite;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnMouseOver()
    {
        if(spriteRenderer.sprite != onSprite)
        {
            spriteRenderer.sprite = onSprite;
        }
    }

    void OnMouseExit()
    {
        if(spriteRenderer.sprite != offSprite)
        {
            spriteRenderer.sprite = offSprite;
        }
    }
}
