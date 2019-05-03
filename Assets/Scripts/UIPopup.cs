using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private Vector2 offset = new Vector2(0, 0);

    private GameObject instance;

    private void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        instance = Instantiate(prefab, canvas.transform);
        instance.SetActive(false);
    }

    private void Stop()
    {
        Destroy(instance);
    }

    public T GetPopupComponent<T>()
    {
        return instance.GetComponent<T>();
    }

    private void OnMouseEnter()
    {
        instance.SetActive(true);
    }

    private void OnMouseOver()
    {
        (instance.transform as RectTransform).position = (Vector2)Input.mousePosition + offset;
    }

    private void OnMouseExit()
    {
        instance.SetActive(false);
    }
}
