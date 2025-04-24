//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HR_UI_CountAnimation : MonoBehaviour {

    /// <summary>
    /// Reference to the Text component
    /// </summary>
    private TextMeshProUGUI text;

    /// <summary>
    /// Original text value
    /// </summary>
    public string originalText;

    /// <summary>
    /// Values for animation
    /// </summary>
    public float originalValue = 0f;
    public float targetValue = 0f;

    /// <summary>
    /// Flags for animation control
    /// </summary>
    public bool actNow = false;
    public bool endedAnimation = false;

    /// <summary>
    /// Reference to the AudioSource component
    /// </summary>
    private AudioSource countingAudioSource;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Awake() {

        text = GetComponent<TextMeshProUGUI>();
        originalText = text.text;

    }

    /// <summary>
    /// Method to get the number from the text
    /// </summary>
    public void GetNumber() {

        originalValue = float.Parse(text.text, System.Globalization.NumberStyles.Number);
        text.text = "0";

    }

    /// <summary>
    /// Method to start the counting animation
    /// </summary>
    public void Count() {

        if (Camera.main) {

            if (GameObject.Find(HR_Settings.Instance.countingPointsAudioClip.name))
                countingAudioSource = GameObject.Find(HR_Settings.Instance.countingPointsAudioClip.name).GetComponent<AudioSource>();
            else
                countingAudioSource = HR_CreateAudioSource.NewAudioSource(Camera.main.gameObject, HR_Settings.Instance.countingPointsAudioClip.name, 0f, 0f, 1f, HR_Settings.Instance.countingPointsAudioClip, true, true, true);

        }

        countingAudioSource.ignoreListenerPause = true;
        countingAudioSource.ignoreListenerVolume = false;

        actNow = true;

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        if (!actNow || endedAnimation)
            return;

        if (countingAudioSource && !countingAudioSource.isPlaying)
            countingAudioSource.Play();

        targetValue = Mathf.MoveTowards(targetValue, originalValue, Time.unscaledDeltaTime * 3000f);

        text.text = targetValue.ToString("F0");

        if ((originalValue - targetValue) <= .05f) {

            if (countingAudioSource && countingAudioSource.isPlaying)
                countingAudioSource.Stop();

            text.text = originalValue.ToString("F0");

            if (GetComponentInParent<HR_UI_ButtonSlideAnimation>())
                GetComponentInParent<HR_UI_ButtonSlideAnimation>().endedAnimation = true;

            endedAnimation = true;

        }

        if (endedAnimation)
            enabled = false;

    }

}
