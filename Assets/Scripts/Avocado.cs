using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Avocado : MonoBehaviour
{
    public enum Labels
    {
        Bin,
        Large,
        Small,
        Ripe,
        Unripe,
        Cut,
        Whole,
    }

    private AvocadoUI popup;

    [SerializeField]
    private Labels[] requiredLabels;

    // I'm aware this isn't fantastic but I cbf moving the counting here instead of the UI behaviour
    public int IncorrectLabelCount { get => popup.IncorrectLabelCount; }

    public int CorrectLabelCount { get => popup.CorrectLabelCount; }

    public int RequiredLabelCount { get => popup.RequiredLabelCount; }

    void Start()
    {
        popup = GetComponent<UIPopup>().GetPopupComponent<AvocadoUI>();

        List<Labels> list = requiredLabels.ToList();
        if(Random.Range(0, 2) == 0)
        {
            list.Add(Labels.Small);
            transform.localScale *= 0.75f;
        }
        else
        {
            list.Add(Labels.Large);
            transform.localScale *= 1.25f;
        }
        requiredLabels = list.ToArray();

        popup.SetRequiredLabels(requiredLabels);
    }

    public void AddLabel(Labels label)
    {
        popup.AddLabel(label);

        if(RequiredLabelCount == CorrectLabelCount)
        {
            GameLogic logic = GameObject.Find("GameManager").GetComponent<GameLogic>();

            // We've got all the labels needed, speed up
            PathTraverser traverser = GetComponent<PathTraverser>();
            traverser.MoveSpeed = 4;
            traverser.OverridePath.AddRange(logic.OverridePath);
            // We also need to get our override path to lead us straight to the truck
        }
    }
}
