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

    [SerializeField]
    private List<Labels> appliedLabels = new List<Labels>();

    public List<Labels> AppliedLabels { get { return appliedLabels; } }
}
