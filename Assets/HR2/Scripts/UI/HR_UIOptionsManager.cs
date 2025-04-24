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
/// Options manager that handles quality, gameplay, and controller settings.
/// </summary>
public class HR_UIOptionsManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_UIOptionsManager instance;
    public static HR_UIOptionsManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<HR_UIOptionsManager>(true);
            }

            return instance;
        }
    }
    #endregion

    /// <summary>
    /// UI Image for touch controls
    /// </summary>
    public Image touch;

    /// <summary>
    /// UI Image for tilt controls
    /// </summary>
    public Image tilt;

    /// <summary>
    /// UI Image for steering wheel controls
    /// </summary>
    public Image steeringWheel;

    [Space()]

    /// <summary>
    /// UI Image for low graphics quality
    /// </summary>
    public Image low;

    /// <summary>
    /// UI Image for medium graphics quality
    /// </summary>
    public Image med;

    /// <summary>
    /// UI Image for high graphics quality
    /// </summary>
    public Image high;

    /// <summary>
    /// UI Image for ultra graphics quality
    /// </summary>
    public Image ultra;

    [Space()]

    /// <summary>
    /// UI Image for shadows option
    /// </summary>
    public Image shadows;

    /// <summary>
    /// UI Text for shadows option
    /// </summary>
    public GameObject shadowsOnText;

    /// <summary>
    /// UI Text for shadows option
    /// </summary>
    public GameObject shadowsOffText;

    /// <summary>
    /// UI Image for processing option
    /// </summary>
    public Image postProcessing;

    /// <summary>
    /// UI Text for post processing option
    /// </summary>
    public GameObject postProcessingOnText;

    /// <summary>
    /// UI Text for post processing option
    /// </summary>
    public GameObject postProcessingOffText;

    [Space()]

    /// <summary>
    /// UI Slider for draw distance
    /// </summary>
    public Slider drawDistance;

    /// <summary>
    /// UI Slider for master volume
    /// </summary>
    public Slider masterVolume;

    /// <summary>
    /// UI Slider for music volume
    /// </summary>
    public Slider musicVolume;

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    public void OnEnable() {

        int currentMobileControllerIndex = HR_API.GetControllerType();

        if (touch && tilt && steeringWheel) {

            //if (RCCP_Settings.Instance.mobileControllerEnabled) {

                if (currentMobileControllerIndex == 0)
                    EnableTargetControllerImage(touch);

                if (currentMobileControllerIndex == 1)
                    EnableTargetControllerImage(tilt);

                if (currentMobileControllerIndex == 2)
                    EnableTargetControllerImage(steeringWheel);

            //}

        }

        int currentQualityIndex = HR_API.GetQuality();

        if (currentQualityIndex == 0)
            EnableTargetQualityImage(low);

        if (currentQualityIndex == 1)
            EnableTargetQualityImage(med);

        if (currentQualityIndex == 2)
            EnableTargetQualityImage(high);

        if (currentQualityIndex == 3)
            EnableTargetQualityImage(ultra);

        drawDistance.value = HR_API.GetDrawDistance();
        masterVolume.value = HR_API.GetAudioVolume();
        musicVolume.value = HR_API.GetMusicVolume();

        shadows.gameObject.SetActive(HR_API.GetShadows());
        shadowsOnText.SetActive(shadows.gameObject.activeSelf);
        shadowsOffText.SetActive(!shadowsOnText.gameObject.activeSelf);

        postProcessing.gameObject.SetActive(HR_API.GetPP());
        postProcessingOnText.SetActive(postProcessing.gameObject.activeSelf);
        postProcessingOffText.SetActive(!postProcessingOnText.gameObject.activeSelf);

    }

    /// <summary>
    /// Set the controller type based on the selected button
    /// </summary>
    /// <param name="button">Selected button</param>
    public void SetControllerType(Button button) {

        switch (button.name) {

            case "Touchscreen":
                HR_API.SetControllerType(0);
                EnableTargetControllerImage(touch);
                break;

            case "Accelerometer":
                HR_API.SetControllerType(1);
                EnableTargetControllerImage(tilt);
                break;

            case "SteeringWheel":
                HR_API.SetControllerType(2);
                EnableTargetControllerImage(steeringWheel);
                break;

            case "Joystick":
                HR_API.SetControllerType(3);
                break;

        }

    }

    /// <summary>
    /// Set the master volume based on the slider value
    /// </summary>
    /// <param name="slider">Slider for master volume</param>
    public void SetMasterVolume(Slider slider) {

        HR_API.SetAudioVolume(slider.value);

    }

    /// <summary>
    /// Set the music volume based on the slider value
    /// </summary>
    /// <param name="slider">Slider for music volume</param>
    public void SetMusicVolume(Slider slider) {

        HR_API.SetMusicVolume(slider.value);

    }

    /// <summary>
    /// Set the quality level based on the selected button
    /// </summary>
    /// <param name="button">Selected button</param>
    public void SetQuality(Button button) {

        switch (button.name) {

            case "Low":
                HR_API.SetQuality(0);
                EnableTargetQualityImage(low);
                break;
            case "Med":
                HR_API.SetQuality(1);
                EnableTargetQualityImage(med);
                break;
            case "High":
                HR_API.SetQuality(2);
                EnableTargetQualityImage(high);
                break;
            case "Ultra":
                HR_API.SetQuality(3);
                EnableTargetQualityImage(ultra);
                break;

        }

    }

    /// <summary>
    /// Set the shadows on / off.
    /// </summary>
    public void SetShadows() {

        HR_API.SetShadows();

        shadows.gameObject.SetActive(HR_API.GetShadows());
        shadowsOnText.SetActive(shadows.gameObject.activeSelf);
        shadowsOffText.SetActive(!shadowsOnText.gameObject.activeSelf);

    }

    /// <summary>
    /// Set the post processing on / off.
    /// </summary>
    public void SetPP() {

        HR_API.SetPP();

        postProcessing.gameObject.SetActive(HR_API.GetPP());
        postProcessingOnText.SetActive(postProcessing.gameObject.activeSelf);
        postProcessingOffText.SetActive(!postProcessingOnText.gameObject.activeSelf);

    }

    /// <summary>
    /// Set the draw distance based on the slider value
    /// </summary>
    /// <param name="slider">Slider for draw distance</param>
    public void SetDrawDistance(Slider slider) {

        HR_API.SetDrawDistance(slider.value);

    }

    private void EnableTargetControllerImage(Image targetImage) {

        touch.gameObject.SetActive(false);
        tilt.gameObject.SetActive(false);
        steeringWheel.gameObject.SetActive(false);

        targetImage.gameObject.SetActive(true);

    }

    private void EnableTargetQualityImage(Image targetImage) {

        low.gameObject.SetActive(false);
        med.gameObject.SetActive(false);
        high.gameObject.SetActive(false);
        ultra.gameObject.SetActive(false);

        targetImage.gameObject.SetActive(true);

    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame() {

        Application.Quit();

    }

}
