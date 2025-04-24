//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
[DisallowMultipleComponent]
public class HR_UI_ButtonSound : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Reference to the AudioSource component
    /// </summary>
    private AudioSource clickSound;

    private Button button;

    public AudioClip audioClip;

    private void Awake() {

        button = GetComponent<Button>();

    }

    /// <summary>
    /// Event handler for pointer click events
    /// </summary>
    /// <param name="data">Pointer event data</param>
    public void OnPointerClick(PointerEventData data) {

        if (!button)
            return;

        if (!button.interactable)
            return;

        if (Camera.main) {

            // Create and configure a new AudioSource for the button click sound
            clickSound = HR_CreateAudioSource.NewAudioSource(Camera.main.gameObject, audioClip.name, 0f, 0f, 1f, audioClip, false, true, true);
            clickSound.ignoreListenerPause = true;
            clickSound.ignoreListenerVolume = false;

        }

    }

    private void Reset() {

        audioClip = HR_Settings.Instance.buttonClickAudioClip;

    }

}
