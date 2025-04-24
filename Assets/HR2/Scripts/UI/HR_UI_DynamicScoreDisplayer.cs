//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HR_UI_DynamicScoreDisplayer : MonoBehaviour {

    #region SINGLETON PATTERN
    /// <summary>
    /// Singleton instance
    /// </summary>
    private static HR_UI_DynamicScoreDisplayer instance;

    /// <summary>
    /// Static property to get the singleton instance
    /// </summary>
    public static HR_UI_DynamicScoreDisplayer Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<HR_UI_DynamicScoreDisplayer>();
            }

            return instance;
        }
    }
    #endregion

    private HR_GamePlayManager gameplayManager;
    public HR_GamePlayManager GameplayManager {

        get {

            if (!gameplayManager)
                gameplayManager = HR_GamePlayManager.Instance;

            return gameplayManager;

        }

    }

    /// <summary>
    /// Reference to the main score Text component
    /// </summary>
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// Lifetime for displaying the score
    /// </summary>
    private float lifeTime = 1f;

    /// <summary>
    /// Timer for the score display
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// Default position for the score Text components
    /// </summary>
    private float offset = 0f;

    /// <summary>
    /// Enum for the side of the screen
    /// </summary>
    public enum Side { Left, Right, Center }

    /// <summary>
    /// Reference to the near miss AudioSource component
    /// </summary>
    private AudioSource nearMissSound;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Start() {

        timer = 0f;

    }

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        // Subscribe to the OnNearMiss event
        HR_Player.OnNearMiss += HR_PlayerHandler_OnNearMiss;

    }

    private void Track() {

        Camera mCamera = Camera.main;

        if (!GameplayManager || !mCamera)
            return;

        HR_Player player = GameplayManager.player;

        if (!player)
            return;

        // Convert the target object's world position to screen position
        Vector3 pos = mCamera.WorldToScreenPoint(player.transform.position) + (Vector3.right * offset);
        pos.z = 0f;

        scoreText.transform.position = Vector3.Lerp(scoreText.transform.position, pos, Time.deltaTime * 3f);

    }

    /// <summary>
    /// Event handler for the OnNearMiss event
    /// </summary>
    /// <param name="player">The player instance</param>
    /// <param name="score">The score value</param>
    /// <param name="side">The side of the screen</param>
    private void HR_PlayerHandler_OnNearMiss(HR_Player player, int score, Side side) {

        switch (side) {

            case Side.Left:
                offset = -185f;
                DisplayScore(score);
                break;

            case Side.Right:
                offset = 185f;
                DisplayScore(score);
                break;

            case Side.Center:
                offset = 0f;
                DisplayScore(score);
                break;

        }

    }

    /// <summary>
    /// Method to display the score
    /// </summary>
    /// <param name="score">The score value</param>
    /// <param name="offset">The offset for the score position</param>
    public void DisplayScore(int score) {

        scoreText.text = "+" + score.ToString();

        timer = lifeTime;
        nearMissSound = HR_CreateAudioSource.NewAudioSource(gameObject, HR_Settings.Instance.nearMissAudioClip.name, 0f, 0f, 1f, HR_Settings.Instance.nearMissAudioClip, false, true, true);
        nearMissSound.ignoreListenerPause = true;

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        if (timer > 0)
            timer -= Time.deltaTime;

        timer = Mathf.Clamp(timer, 0f, lifeTime);

        if (timer > 0)
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 1f);
        else
            scoreText.color = Color.Lerp(scoreText.color, new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 0f), Time.deltaTime * 10f);

    }

    private void LateUpdate() {

        Track();

    }

    /// <summary>
    /// Called when the object becomes disabled or inactive
    /// </summary>
    private void OnDisable() {

        // Unsubscribe from the OnNearMiss event
        HR_Player.OnNearMiss -= HR_PlayerHandler_OnNearMiss;

    }

}
