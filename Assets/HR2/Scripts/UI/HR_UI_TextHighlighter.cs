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
using TMPro;

/// <summary>
/// UI text highlighter on mouse cursor enters/exits.
/// </summary>
public class HR_UI_TextHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    /// <summary>
    /// Text component to be highlighted.
    /// </summary>
    public TextMeshProUGUI text;

    /// <summary>
    /// Default color of the text.
    /// </summary>
    public Color defaultColor = Color.white;

    /// <summary>
    /// Highlighted color of the text.
    /// </summary>
    public Color highlightColor = Color.black;

    /// <summary>
    /// Indicates if the text is currently being highlighted.
    /// </summary>
    public bool highlighting = false;

    /// <summary>
    /// Initializes the default color of the text.
    /// </summary>
    private void Awake() {

        defaultColor = text.color;

    }

    /// <summary>
    /// Resets the text highlighting state when the object is enabled.
    /// </summary>
    private void OnEnable() {

        highlighting = false;
        text.color = defaultColor;

    }

    /// <summary>
    /// Resets the text highlighting state when the object is disabled.
    /// </summary>
    private void OnDisable() {

        highlighting = false;
        text.color = defaultColor;

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

    }

    /// <summary>
    /// Updates the text color based on the highlighting state.
    /// </summary>
    private void Update() {

        if (highlighting)
            text.color = Color.Lerp(text.color, highlightColor, Time.unscaledDeltaTime * 25f);
        else
            text.color = Color.Lerp(text.color, defaultColor, Time.unscaledDeltaTime * 25f);

    }

    /// <summary>
    /// Resets the text component reference.
    /// </summary>
    private void Reset() {

        text = GetComponentInChildren<TextMeshProUGUI>(true);

        if (!text)
            Debug.LogError("Text component couldn't be found for this " + transform.name + "! CCDS_UI_TextHighlighter needs a target text to work with. Please select the text component...");

        defaultColor = text.color;
        highlightColor = defaultColor;

    }

}
