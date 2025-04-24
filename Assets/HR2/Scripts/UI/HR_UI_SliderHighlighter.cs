//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI slider highlighter that animates the slider when the mouse cursor enters or exits.
/// </summary>
public class HR_UI_SliderHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    /// <summary>
    /// The image component of the slider.
    /// </summary>
    public Image slider;

    /// <summary>
    /// The default position of the slider.
    /// </summary>
    public Vector3 defaultPosition = Vector3.zero;

    /// <summary>
    /// Indicates if the slider is currently being highlighted.
    /// </summary>
    public bool highlighting = false;

    /// <summary>
    /// Indicates if the slider is currently animating.
    /// </summary>
    public bool animating = false;

    /// <summary>
    /// Indicates if the slider has already been animated once.
    /// </summary>
    public bool animatedOnce = false;

    /// <summary>
    /// Initializes the slider and its default position.
    /// </summary>
    private void Awake() {

        if (!slider) {

            Debug.LogError("Image is not selected on this component named " + transform.name + "!");
            enabled = false;
            return;

        }

        // Getting default local position.
        defaultPosition = slider.rectTransform.localPosition;

    }

    /// <summary>
    /// Resets highlighting and animation states when the object is enabled.
    /// </summary>
    private void OnEnable() {

        highlighting = false;
        animating = false;
        animatedOnce = false;
        slider.gameObject.SetActive(false);

    }

    /// <summary>
    /// Resets highlighting and animation states when the object is disabled.
    /// </summary>
    private void OnDisable() {

        highlighting = false;
        animating = false;
        animatedOnce = false;
        slider.gameObject.SetActive(false);

    }

    /// <summary>
    /// Handles the pointer enter event to start highlighting.
    /// </summary>
    /// <param name="eventData">Event data for the pointer enter.</param>
    public void OnPointerEnter(PointerEventData eventData) {

        highlighting = true;

    }

    /// <summary>
    /// Handles the pointer exit event to stop highlighting.
    /// </summary>
    /// <param name="eventData">Event data for the pointer exit.</param>
    public void OnPointerExit(PointerEventData eventData) {

        highlighting = false;
        animatedOnce = false;

    }

    /// <summary>
    /// Updates the highlighting animation.
    /// </summary>
    private void Update() {

        if (!animating && highlighting && !animatedOnce)
            StartCoroutine(Highlight());

    }

    /// <summary>
    /// Animates the slider from left to right.
    /// </summary>
    /// <returns>Coroutine for the highlight animation.</returns>
    private IEnumerator Highlight() {

        animating = true;
        animatedOnce = true;

        slider.gameObject.SetActive(true);
        slider.rectTransform.localPosition = defaultPosition;
        slider.rectTransform.localPosition += Vector3.right * -300f;

        float time = 1f;

        while (time > 0) {

            time -= Time.deltaTime;
            slider.rectTransform.Translate(Vector3.right * (Time.unscaledDeltaTime) * 4500f);
            yield return null;

        }

        slider.gameObject.SetActive(false);

        animating = false;

    }

}
