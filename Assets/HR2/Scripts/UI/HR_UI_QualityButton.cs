//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles quality settings changes through UI buttons.
/// </summary>
public class HR_UI_QualityButton : MonoBehaviour {

    /// <summary>
    /// Enum for graphics quality levels
    /// </summary>
    public enum GraphicsLevel { Low, Medium, High }

    /// <summary>
    /// Selected graphics quality level
    /// </summary>
    public GraphicsLevel _graphicsLevel;

    /// <summary>
    /// Reference to the Button component
    /// </summary>
    private Button button;

    /// <summary>
    /// Default color of the button
    /// </summary>
    private Color defButtonColor;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    void Awake() {

        button = GetComponent<Button>();
        defButtonColor = button.image.color;

    }

    /// <summary>
    /// Event handler for button click events
    /// </summary>
    public void OnClick() {

        switch (_graphicsLevel) {

            case GraphicsLevel.Low:
                QualitySettings.SetQualityLevel(0);
                break;
            case GraphicsLevel.Medium:
                QualitySettings.SetQualityLevel(1);
                break;
            case GraphicsLevel.High:
                QualitySettings.SetQualityLevel(2);
                break;

        }

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        button.image.color = defButtonColor;
        Color activeColor = new Color(.667f, 1f, 0f);

        if (QualitySettings.GetQualityLevel() == 0 && _graphicsLevel == GraphicsLevel.Low)
            button.image.color = activeColor;

        if (QualitySettings.GetQualityLevel() == 1 && _graphicsLevel == GraphicsLevel.Medium)
            button.image.color = activeColor;

        if (QualitySettings.GetQualityLevel() == 2 && _graphicsLevel == GraphicsLevel.High)
            button.image.color = activeColor;

    }

}
