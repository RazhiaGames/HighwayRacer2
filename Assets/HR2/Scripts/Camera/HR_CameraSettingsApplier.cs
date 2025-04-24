//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Applies quality settings to the camera and other settings.
/// </summary>
public class HR_CameraSettingsApplier : MonoBehaviour {

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable() {

        Check();

        // Listening for the event when options are changed.
        HR_Events.OnOptionsChanged += OptionsManager_OnOptionsChanged;

    }

    /// <summary>
    /// Checks the saved properties and applies them.
    /// </summary>
    public void Check() {

        Camera mCamera = Camera.main;

        if (mCamera) {

            int drawD = (int)HR_API.GetDrawDistance();
            mCamera.farClipPlane = drawD;

        }

        UniversalAdditionalCameraData universalAdditionalCameraData = mCamera.GetComponent<UniversalAdditionalCameraData>();

        if (universalAdditionalCameraData)
            universalAdditionalCameraData.renderShadows = HR_API.GetShadows();

        Volume volume = mCamera.GetComponent<Volume>();

        if (volume)
            volume.enabled = HR_API.GetPP();

    }

    /// <summary>
    /// Handles the options changed event.
    /// </summary>
    public void OptionsManager_OnOptionsChanged() {

        // Checks the saved properties and applies them.
        Check();

    }

    /// <summary>
    /// Called when the behaviour becomes disabled.
    /// </summary>
    private void OnDisable() {

        HR_Events.OnOptionsChanged -= OptionsManager_OnOptionsChanged;

    }

}
