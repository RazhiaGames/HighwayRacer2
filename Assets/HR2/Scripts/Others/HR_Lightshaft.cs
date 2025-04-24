//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_Lightshaft : MonoBehaviour {

    /// <summary>
    /// The light source component from the parent object.
    /// </summary>
    private Light lightSource;

    /// <summary>
    /// The material attached to the MeshRenderer component of this object.
    /// </summary>
    private Material material;

    /// <summary>
    /// The ID of the "_EmissionColor" property in the shader.
    /// </summary>
    private int nameID = 0;

    /// <summary>
    /// Determines whether the light shaft should be updated at runtime.
    /// </summary>
    public bool updateAtRuntime = false;

    /// <summary>
    /// Initializes the light source, material, and shader property ID.
    /// </summary>
    private void Awake() {

        // Get the Light component from the parent object
        lightSource = GetComponentInParent<Light>();

        // Get the Material from the MeshRenderer component
        material = GetComponent<MeshRenderer>().material;

        // Enable the "_EMISSION" keyword for the material's shader
        material.EnableKeyword("_EMISSION");

        // Get the shader property ID for "_EmissionColor"
        nameID = Shader.PropertyToID("_EmissionColor");

    }

    /// <summary>
    /// Coroutine that starts after Awake. Sets the emission color based on the light source's color and intensity.
    /// </summary>
    private IEnumerator Start() {

        // Wait until the fixed update cycle before setting the emission color
        yield return new WaitForFixedUpdate();

        // Set the emission color of the material
        material.SetColor(nameID, lightSource.color * lightSource.intensity);

    }

    /// <summary>
    /// Updates the emission color if the light source exists and updateAtRuntime is true.
    /// </summary>
    private void Update() {

        // If the light source does not exist, exit the method
        if (!lightSource)
            return;

        // Update the emission color at runtime, if enabled
        if (updateAtRuntime)
            material.SetColor(nameID, lightSource.color * lightSource.intensity * .2f);

    }

}
