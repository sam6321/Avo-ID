using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StamperUI : MonoBehaviour
{
    [SerializeField]
    private GameObject textPrefab;

    private GameObject selectedText = null;
    private List<GameObject> labelTexts = new List<GameObject>();

    public void SetLabels(Avocado.Labels[] labels)
    {
        foreach(GameObject label in labelTexts)
        {
            Destroy(label);
        }
        labelTexts.Clear();

        foreach(Avocado.Labels label in labels)
        {
            GameObject newText = Instantiate(textPrefab, transform);
            newText.GetComponent<Image>().enabled = false;
            newText.GetComponentInChildren<Text>().text = label.ToString();
            labelTexts.Add(newText);
        }
    }

    public void SetSelectedIndex(int index)
    {
        if (selectedText)
        {
            selectedText.GetComponent<Image>().enabled = false;
        }

        selectedText = labelTexts[index];
        selectedText.GetComponent<Image>().enabled = true;
    }
}
