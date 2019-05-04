using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private Vector2 offset = new Vector2(0, 0);

    [SerializeField]
    private bool showOnMouseOver = false;

    private GameObject instance;

    private void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        instance = Instantiate(prefab, canvas.transform);
        if(showOnMouseOver)
        {
            instance.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if(!showOnMouseOver)
        {
            (instance.transform as RectTransform).position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

    private void OnDestroy()
    {
        Destroy(instance);
    }

    public T GetPopupComponent<T>()
    {
        return instance.GetComponent<T>();
    }

    private void OnMouseEnter()
    {
        if(showOnMouseOver)
        {
            instance.SetActive(true);
        }
    }

    private void OnMouseOver()
    {
        if (showOnMouseOver)
        {
            (instance.transform as RectTransform).position = (Vector2)Input.mousePosition + offset;
        }
    }

    private void OnMouseExit()
    {
        if (showOnMouseOver)
        {
            instance.SetActive(false);
        }
    }
}
