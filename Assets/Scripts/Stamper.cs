﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Stamper : MonoBehaviour
{
    [SerializeField]
    private Avocado.Labels[] labels;

    [SerializeField]
    private int labelIndex = 0;

    private Animator animator;
    private StamperUI popup;

    private bool isStamping = false;

    [SerializeField]
    List<AudioClip> stampingSounds;

    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        popup = GetComponent<UIPopup>().GetPopupComponent<StamperUI>();
        popup.SetLabels(labels);
        popup.SetSelectedIndex(labelIndex);
        audioSource = GetComponent<AudioSource>();
    }

    public void Stamp(PathTraverser traverser, PathNode node, PathNode nextNode)
    {
        // If this object doesn't have an avocado, then it's a fruit that should've been binned.
        // Still stamp it, but don't add any labels
        Avocado avocado = traverser.GetComponent<Avocado>();
        if(avocado)
        {
            // Assign the appropriate stamp to this avocado
            avocado.AddLabel(labels[labelIndex]);
        }

        if (!isStamping)
        {
            StampInternal();
        }
    }

    // Triggered when the stamper has begun moving up and the traverser can move forward
    public void OnStampComplete()
    {
        isStamping = false;
    }

    private void StampInternal()
    {
        isStamping = true;
        Utils.PlayRandomSound(audioSource, stampingSounds);
        animator.SetTrigger("stamp");
    }

    private void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            labelIndex = (labelIndex + 1) % labels.Length;
            popup.SetSelectedIndex(labelIndex);
        }
    }
}
