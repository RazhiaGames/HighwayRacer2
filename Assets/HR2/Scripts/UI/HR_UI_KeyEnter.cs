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
/// Handles key press events to trigger button clicks.
/// </summary>
[DisallowMultipleComponent]
public class HR_UI_KeyEnter : MonoBehaviour {

    /// <summary>
    /// The input name to listen for
    /// </summary>
    public string inputName = "Submit";

    /// <summary>
    /// Reference to the Button component
    /// </summary>
    private Button button;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Start() {

        button = GetComponent<Button>();

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        if (Input.GetButtonDown(inputName))
            button.onClick.Invoke();

    }

}
