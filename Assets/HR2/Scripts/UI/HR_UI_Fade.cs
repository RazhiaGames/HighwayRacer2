//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_UI_Fade : MonoBehaviour {

    /// <summary>
    /// Reference to the CanvasGroup component
    /// </summary>
    public CanvasGroup canvasGroup;

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        // Start the fade animation
        StartCoroutine(Fade());

    }

    /// <summary>
    /// Coroutine to handle the fade animation
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator Fade() {

        canvasGroup.alpha = 0f;
        float timer = 1f;

        while (timer > 0) {

            timer -= Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime * 5f);
            yield return null;

        }

        canvasGroup.alpha = 1f;

    }

    /// <summary>
    /// Called when the object becomes disabled or inactive
    /// </summary>
    private void OnDisable() {

        // Stop all coroutines
        StopAllCoroutines();

    }

}
