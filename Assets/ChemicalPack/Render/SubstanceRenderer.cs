using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubstanceRenderer : MonoBehaviour
{
    private BaseContainer baseContainer;

    public float scaleY;
    public float filled;

    private void Start()
    {
        baseContainer = GetComponent < BaseContainer > ();
        baseContainer.OnBaseVolumeChanged += BaseContainer_OnBaseVolumeChanged;
    }

    private void BaseContainer_OnBaseVolumeChanged(float filled)
    {
        this.filled = filled;
    }

}
