using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private Vector2 offset = new Vector2(0, 0);

    [SerializeField]
    private bool showOnMouseOver = false;

    [SerializeField]
    private List<GameObject> overlayElements;

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
        Show();
    }

    private void Show()
    {
        if (showOnMouseOver && !instance.activeSelf)
        {
            if(overlayElements.Where(overlay => overlay.activeSelf).Count() == 0)
            {
                instance.SetActive(true);
            }
        }
    }

    private void OnMouseOver()
    {
        if (showOnMouseOver)
        {
            Show();
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
