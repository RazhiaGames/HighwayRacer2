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

/// <summary>
/// Handles the horn functionality for mobile controllers.
/// </summary>
public class HR_UI_Horn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    /// <summary>
    /// Indicates whether the horn is being pressed
    /// </summary>
    bool isPressing = false;

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        if (!RCCP_Settings.Instance.mobileControllerEnabled) {

            gameObject.SetActive(false);
            return;

        }

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        if (!RCCP_SceneManager.Instance.activePlayerVehicle)
            return;

        if (!RCCP_SceneManager.Instance.activePlayerVehicle.Lights)
            return;

        if (isPressing)
            RCCP_SceneManager.Instance.activePlayerVehicle.Lights.highBeamHeadlights = true;
        else
            RCCP_SceneManager.Instance.activePlayerVehicle.Lights.highBeamHeadlights = false;

    }

    /// <summary>
    /// Event handler for pointer down events
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerDown(PointerEventData eventData) {

        isPressing = true;

    }

    /// <summary>
    /// Event handler for pointer up events
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerUp(PointerEventData eventData) {

        isPressing = false;

    }

}
