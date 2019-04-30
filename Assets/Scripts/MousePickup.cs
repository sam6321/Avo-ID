using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePickup : MonoBehaviour
{
    int clickableMask = 0;

    void Start()
    {
        clickableMask = LayerMask.GetMask("MouseInteractable");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, clickableMask))
            {
                hit.collider.gameObject.SendMessageUpwards("OnClick", SendMessageOptions.RequireReceiver);
            }
        }
    }

}
