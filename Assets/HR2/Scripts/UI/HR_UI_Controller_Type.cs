//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HR_UI_Controller_Type : MonoBehaviour {

    /// <summary>
    /// Enum for controller types
    /// </summary>
    public ControllerType _controllerType;
    public enum ControllerType { keypad, accelerometer }

    /// <summary>
    /// Reference to the Button component
    /// </summary>
    private Button sprite;
    private Color defCol;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Start() {

        sprite = GetComponent<Button>();
        defCol = sprite.image.color;

        if (!PlayerPrefs.HasKey("ControllerType"))
            PlayerPrefs.SetInt("ControllerType", 0);

        Check();

    }

    /// <summary>
    /// Event handler for button click events
    /// </summary>
    public void OnClick() {

        if (_controllerType == ControllerType.keypad) {

            PlayerPrefs.SetInt("ControllerType", 0);
            RCCP.SetMobileController(RCCP_Settings.MobileController.TouchScreen);

        }

        if (_controllerType == ControllerType.accelerometer) {

            PlayerPrefs.SetInt("ControllerType", 1);
            RCCP.SetMobileController(RCCP_Settings.MobileController.Gyro);

        }

        HR_UI_Controller_Type[] ct = FindObjectsOfType<HR_UI_Controller_Type>();

        foreach (HR_UI_Controller_Type cts in ct)
            cts.Check();

    }

    /// <summary>
    /// Method to check and update the controller type
    /// </summary>
    private void Check() {

        if (PlayerPrefs.GetInt("ControllerType") == 0) {

            if (_controllerType == ControllerType.keypad)
                sprite.image.color = new Color(.667f, 1f, 0f);

            if (_controllerType == ControllerType.accelerometer)
                sprite.image.color = defCol;

        }

        if (PlayerPrefs.GetInt("ControllerType") == 1) {

            if (_controllerType == ControllerType.keypad)
                sprite.image.color = defCol;

            if (_controllerType == ControllerType.accelerometer)
                sprite.image.color = new Color(.667f, 1f, 0f);

        }

    }

}
