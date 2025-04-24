//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Manages the lens flare effect for a light source.
/// </summary>
[RequireComponent(typeof(LensFlareComponentSRP))]
public class HR_LensFlare : MonoBehaviour {

    /// <summary>
    /// Reference to the Light component.
    /// </summary>
    private Light lightSource;

    /// <summary>
    /// Reference to the LensFlareComponentSRP component.
    /// </summary>
    private LensFlareComponentSRP lensFlare;

    /// <summary>
    /// Brightness of the lens flare.
    /// </summary>
    public float flareBrightness = 1.5f;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Start() {

        lightSource = GetComponent<Light>();
        lensFlare = GetComponent<LensFlareComponentSRP>();

    }

    /// <summary>
    /// Called once per frame to update the lens flare intensity based on the light source's intensity.
    /// </summary>
    void Update() {

        if (!lightSource || !lensFlare)
            return;

        lensFlare.intensity = lightSource.intensity * flareBrightness * .5f;

    }

}
