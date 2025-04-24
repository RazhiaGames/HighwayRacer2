//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Changes the scale of the text on text changes.
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class HR_UI_TextPopperOnTextChange : MonoBehaviour {

    /// <summary>
    /// Reference to the TextMeshProUGUI component.
    /// </summary>
    private TextMeshProUGUI text;

    /// <summary>
    /// Stores the old text to detect changes.
    /// </summary>
    public string oldText = "";

    /// <summary>
    /// Timer used to control the pop effect.
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// Default scale of the text.
    /// </summary>
    private Vector3 defaultScale = Vector3.zero;

    /// <summary>
    /// Indicates if the text is currently interacting.
    /// </summary>
    public bool interacting = false;

    /// <summary>
    /// Resets the timer and interaction state when the object is enabled.
    /// </summary>
    private void OnEnable() {

        timer = 0f;
        interacting = false;

    }

    /// <summary>
    /// Resets the timer and interaction state when the object is disabled.
    /// </summary>
    private void OnDisable() {

        timer = 0f;
        interacting = false;

    }

    /// <summary>
    /// Updates the text and applies the pop effect if the text changes.
    /// </summary>
    private void LateUpdate() {

        if (!text)
            text = GetComponent<TextMeshProUGUI>();

        if (!text)
            return;

        if (defaultScale == Vector3.zero)
            defaultScale = text.transform.localScale;

        if (text.text != oldText)
            timer = 1f;

        oldText = text.text;

        if (timer > 0) {

            timer -= Time.unscaledDeltaTime * 3f;

            if (!interacting)
                StartCoroutine(Pop());

        } else {

            timer = 0f;
            interacting = false;
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, defaultScale, Time.unscaledDeltaTime * 5f);

        }

    }

    /// <summary>
    /// Applies the pop effect by changing the scale of the text.
    /// </summary>
    /// <returns>Coroutine for the pop effect.</returns>
    private IEnumerator Pop() {

        interacting = true;

        text.transform.localScale *= 1.2f;
        float time = 1f;

        while (time > 0f) {

            time -= Time.deltaTime;
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, defaultScale, Time.unscaledDeltaTime * 5f);

            yield return null;

        }

        text.transform.localScale = defaultScale;

    }

}
