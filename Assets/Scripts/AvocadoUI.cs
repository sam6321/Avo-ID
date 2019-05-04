using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvocadoUI : MonoBehaviour
{
    [SerializeField]
    private GameObject textPrefab;

    [SerializeField]
    private Color correctColour;

    [SerializeField]
    private Color wrongColour;

    // Lookup for all the correct labels for this avocado
    private Dictionary<Avocado.Labels, GameObject> requiredLabelLookup = new Dictionary<Avocado.Labels, GameObject>();
    private int incorrectLabelCount = 0;
    private int correctLabelCount = 0;
    private int requiredLabelCount = 0;

    public int IncorrectLabelCount { get => incorrectLabelCount; }

    public int CorrectLabelCount { get => correctLabelCount; }

    public int RequiredLabelCount { get => requiredLabelCount; }

    public void AddLabel(Avocado.Labels label)
    {
        if (requiredLabelLookup.TryGetValue(label, out GameObject labelObject))
        {
            // This is a label we wanted. Ignore duplicates for now?
            Image image = labelObject.GetComponent<Image>();
            if(!image.enabled)
            {
                correctLabelCount++;
                image.enabled = true;
            }
        }
        else
        {
            // This is not a label we wanted
            CreateLabel(label, wrongColour, highlightEnabled: true);
            incorrectLabelCount++;
        }
    }

    public void SetRequiredLabels(Avocado.Labels[] labels)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        incorrectLabelCount = 0;
        correctLabelCount = 0;
        requiredLabelCount = labels.Length;
        requiredLabelLookup.Clear();
        foreach(Avocado.Labels label in labels)
        {
            requiredLabelLookup.Add(label, CreateLabel(label, correctColour));
        }
    }

    private GameObject CreateLabel(Avocado.Labels label, Color startColour, bool highlightEnabled=false)
    {
        GameObject newLabel = Instantiate(textPrefab, transform);
        newLabel.GetComponentInChildren<Text>().text = label.ToString();
        Image image = newLabel.GetComponent<Image>();
        image.color = startColour;
        image.enabled = highlightEnabled;
        return newLabel;
    }
}
