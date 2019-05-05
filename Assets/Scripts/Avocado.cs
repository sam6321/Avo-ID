using System.Linq;
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
        Bumpy,
        Smooth,
        Cut,
        Uncut,
        Damaged,
        Undamaged
    }

    private class LabelGroup
    {
        public Labels[] labels;

        public LabelGroup(params Labels[] labels)
        {
            this.labels = labels;
        }

        public Labels RandomLabel()
        {
            return Utils.RandomElement(labels);
        }
    }

    private static LabelGroup[] labelGroups = new LabelGroup[]
    {
        new LabelGroup(Labels.Large, Labels.Small),
        new LabelGroup(Labels.Ripe, Labels.Unripe),
        new LabelGroup(Labels.Bumpy, Labels.Smooth),
        new LabelGroup(Labels.Cut, Labels.Uncut),
        new LabelGroup(Labels.Damaged, Labels.Undamaged),
    };

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
        popup.SetRequiredLabels(requiredLabels);
    }

    public void AddLabel(Labels label)
    {
        popup.AddLabel(label);
    }
}
