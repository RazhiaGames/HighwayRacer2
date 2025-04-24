//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles fading of a UI CanvasGroup when the object is enabled or disabled.
/// </summary>
public class HR_UI_CanvasGroupFader : MonoBehaviour {

    /// <summary>
    /// Reference to the CanvasGroup component.
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// Initializes the CanvasGroup and starts the fade-in effect when the object is enabled.
    /// </summary>
    private void OnEnable() {

        // Get the CanvasGroup component.
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        // If CanvasGroup is not found, exit the method.
        if (!canvasGroup)
            return;

        // Set the alpha of the CanvasGroup to 0 (fully transparent).
        canvasGroup.alpha = 0f;

        // Start the fade-in coroutine.
        StartCoroutine(Fade());

    }

    /// <summary>
    /// Coroutine to fade the alpha value of the CanvasGroup from 0 to 1 over time.
    /// </summary>
    /// <returns>IEnumerator for the coroutine.</returns>
    public IEnumerator Fade() {

        // Duration for the fade effect.
        float time = 1f;

        // Gradually increase the alpha value to 1 (fully opaque).
        while (time > 0f) {

            time -= Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.unscaledDeltaTime * 5f);
            yield return null;

        }

        // Ensure the alpha value is set to 1 after fading.
        canvasGroup.alpha = 1f;

    }

    /// <summary>
    /// Resets the alpha value of the CanvasGroup to 0 when the object is disabled.
    /// </summary>
    private void OnDisable() {

        // Get the CanvasGroup component.
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        // If CanvasGroup is not found, exit the method.
        if (!canvasGroup)
            return;

        // Set the alpha of the CanvasGroup to 0 (fully transparent).
        canvasGroup.alpha = 0f;

    }

}
