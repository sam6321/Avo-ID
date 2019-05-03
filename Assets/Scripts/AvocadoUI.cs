using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvocadoUI : MonoBehaviour
{
    [SerializeField]
    private GameObject textPrefab;

    public void AddLabel(Avocado.Labels label)
    {
        Instantiate(textPrefab, transform).GetComponentInChildren<Text>().text = label.ToString();
    }
}
