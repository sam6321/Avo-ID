using System.Collections;
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
        Bumpy,
        Smooth,
        Cut,
        Uncut,
        Damaged,
        Undamaged
    }

    private AvocadoUI popup;

    [SerializeField]
    private List<Labels> appliedLabels = new List<Labels>();

    void Start()
    {
        popup = GetComponent<UIPopup>().GetPopupComponent<AvocadoUI>();
    }

    public void AddLabel(Labels label)
    {
        appliedLabels.Add(label);
        popup.AddLabel(label);
    }
}
