//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_UI_CountDown : MonoBehaviour {

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        // Subscribe to the OnCountDownStarted event
        HR_Events.OnCountDownStarted += HR_GamePlayHandler_OnCountDownStarted;

    }

    /// <summary>
    /// Event handler for the OnCountDownStarted event
    /// </summary>
    private void HR_GamePlayHandler_OnCountDownStarted() {

        // Trigger the "Count" animation
        GetComponent<Animator>().SetTrigger("Count");

    }

    /// <summary>
    /// Called when the object becomes disabled or inactive
    /// </summary>
    private void OnDisable() {

        // Unsubscribe from the OnCountDownStarted event
        HR_Events.OnCountDownStarted -= HR_GamePlayHandler_OnCountDownStarted;

    }

}
