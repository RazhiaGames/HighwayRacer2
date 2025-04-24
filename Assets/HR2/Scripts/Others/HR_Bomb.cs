//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Bomb with timer and SFX.
/// </summary>
public class HR_Bomb : MonoBehaviour {

    // Player.
    private HR_Player player;

    /// <summary>
    /// Property to get the player component.
    /// </summary>
    private HR_Player Player {

        get {

            if (player == null)
                player = GetComponentInParent<HR_Player>();

            return player;

        }

    }

    private Light bombLight;        // Light component.
    private float bombTimer = 0f;   // Timer for the bomb.

    private AudioSource bombTimerAudioSource;       // AudioSource for the bomb timer sound effect.
    private AudioClip BombTimerAudiclip { get { return HR_Settings.Instance.bombTimerAudioClip; } }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        // If game mode is bomb, enable the bomb, otherwise disable it.
        if (HR_GamePlayManager.Instance) {

            if (HR_GamePlayManager.Instance.mode == HR_GamePlayManager.Mode.Bomb)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);

        } else {

            gameObject.SetActive(false);
            return;

        }

        // Getting player handler and creating light with SFX.
        bombTimerAudioSource = HR_CreateAudioSource.NewAudioSource(gameObject, "Bomb Timer AudioSource", 0f, 0f, .25f, BombTimerAudiclip, false, false, false);
        bombLight = GetComponentInChildren<Light>();
        bombLight.enabled = true;
        bombLight.intensity = 0f;

    }

    /// <summary>
    /// Called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate() {

        // If no player found, return.
        if (!Player)
            return;

        // If bomb is not triggered, return.
        if (!Player.bombTriggered)
            return;

        // Adjusting signal light timer.
        bombTimer += Time.fixedDeltaTime * Mathf.Lerp(5f, 1f, Player.bombHealth / 100f);

        // Adjusting the intensity of the bomb light.
        if (bombTimer >= .5f)
            bombLight.intensity = Mathf.Lerp(bombLight.intensity, 0f, Time.fixedDeltaTime * 50f);
        else
            bombLight.intensity = Mathf.Lerp(bombLight.intensity, .1f, Time.fixedDeltaTime * 50f);

        // Playing the bomb timer sound effect at regular intervals.
        if (bombTimer >= 1f) {

            bombTimer = 0f;
            bombTimerAudioSource.Play();

        }

    }

}
