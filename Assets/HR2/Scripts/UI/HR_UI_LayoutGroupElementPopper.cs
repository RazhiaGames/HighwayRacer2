//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI layout group element popper. Adjusts the element's scale for popup effect. Must be attached to the layout group.
/// </summary>
public class HR_UI_LayoutGroupElementPopper : MonoBehaviour {

    /// <summary>
    /// Indicates if the elements have been initialized.
    /// </summary>
    private bool initialized = false;

    /// <summary>
    /// All elements in the layout group.
    /// </summary>
    private Transform[] elements;

    /// <summary>
    /// Default scales of the elements.
    /// </summary>
    private Vector3[] defaultScale;

    /// <summary>
    /// Initializes elements and starts the popup effect when the object is enabled.
    /// </summary>
    private void OnEnable() {

        if (initialized) {

            if (elements.Length > 0) {

                for (int i = 0; i < elements.Length; i++) {

                    if (elements[i] != null) {

                        elements[i].localScale = defaultScale[i];
                        StartCoroutine(Pop(elements[i], i, defaultScale[i]));

                    }

                }

            }

            return;

        }

        initialized = true;

        elements = transform.GetComponentsInChildren<Transform>(false);
        defaultScale = new Vector3[elements.Length];

        if (elements.Length < 1)
            return;

        for (int i = 0; i < elements.Length; i++) {

            if (elements[i] != null) {

                if (defaultScale[i] == Vector3.zero)
                    defaultScale[i] = elements[i].localScale;

                StartCoroutine(Pop(elements[i], i, defaultScale[i]));

            }

        }

    }

    /// <summary>
    /// Popup effect for each element in the layout group.
    /// </summary>
    /// <param name="element">The element to apply the popup effect to.</param>
    /// <param name="waitForSeconds">The delay before applying the popup effect.</param>
    /// <param name="scale">The default scale of the element.</param>
    /// <returns>Coroutine for the popup effect.</returns>
    public IEnumerator Pop(Transform element, float waitForSeconds, Vector3 scale) {

        yield return new WaitForSeconds((float)waitForSeconds / 25f);

        element.localScale *= 1.1f;
        float time = 1.5f;

        while (time > 0f) {

            time -= Time.deltaTime;
            element.localScale = Vector3.Lerp(element.localScale, scale, Time.unscaledDeltaTime * 5f);

            yield return null;

        }

        element.localScale = scale;

    }

    /// <summary>
    /// Resets elements to their default scales when the object is disabled.
    /// </summary>
    private void OnDisable() {

        if (initialized) {

            if (elements.Length > 0) {

                for (int i = 0; i < elements.Length; i++) {

                    if (elements[i] != null)
                        elements[i].localScale = defaultScale[i];

                }

            }

        }

    }

}
