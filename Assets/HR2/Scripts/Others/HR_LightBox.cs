//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Manages a fake transparent box for emissive lighting.
/// </summary>
public class HR_LightBox : MonoBehaviour {

    /// <summary>
    /// Reference to the Light component.
    /// </summary>
    private Light _light;

    /// <summary>
    /// Reference to the MeshRenderer component.
    /// </summary>
    private MeshRenderer boxRenderer;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        _light = GetComponent<Light>();
        boxRenderer = GetComponentInChildren<MeshRenderer>();

    }

    /// <summary>
    /// Called once per frame to update the color and emission of the box material based on the light's intensity.
    /// </summary>
    private void Update() {

        if (!_light || !boxRenderer)
            return;

        boxRenderer.material.SetColor("_BaseColor", _light.color * _light.intensity);
        boxRenderer.material.SetColor("_EmissionColor", _light.color * _light.intensity);

    }

}
