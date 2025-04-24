//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class HR_UI_GameplayPanel : MonoBehaviour {

    /// <summary>
    /// Reference to the player instance
    /// </summary>
    private HR_Player player;

    /// <summary>
    /// Content game object
    /// </summary>
    public GameObject content;

    /// <summary>
    /// UI Text components for displaying gameplay information
    /// </summary>
    public TextMeshProUGUI score;
    public TextMeshProUGUI timeLeft;
    public TextMeshProUGUI combo;

    public TextMeshProUGUI speed;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI highSpeed;
    public TextMeshProUGUI oppositeDirection;

    public Slider damageSlider;
    public Slider bombSlider;

    /// <summary>
    /// UI elements for combo display
    /// </summary>
    private Image comboMImage;
    private Vector2 comboDefPos;

    /// <summary>
    /// UI elements for high speed display
    /// </summary>
    private Image highSpeedImage;
    private Vector2 highSpeedDefPos;

    /// <summary>
    /// UI elements for opposite direction display
    /// </summary>
    private Image oppositeDirectionImage;
    private Vector2 oppositeDirectionDefPos;

    /// <summary>
    /// UI elements for time attack mode
    /// </summary>
    private Image timeAttackImage;

    /// <summary>
    /// UI elements for bomb mode
    /// </summary>
    private RectTransform bombRect;
    private Vector2 bombDefPos;

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Awake() {

        if (combo) {

            comboMImage = combo.GetComponentInParent<Image>();
            comboDefPos = comboMImage.rectTransform.anchoredPosition;

        }

        if (highSpeed) {

            highSpeedImage = highSpeed.GetComponentInParent<Image>();
            highSpeedDefPos = highSpeedImage.rectTransform.anchoredPosition;

        }

        if (oppositeDirection) {

            oppositeDirectionImage = oppositeDirection.GetComponentInParent<Image>();
            oppositeDirectionDefPos = oppositeDirectionImage.rectTransform.anchoredPosition;

        }

        if (timeLeft)
            timeAttackImage = timeLeft.GetComponentInParent<Image>();

        if (bombSlider) {

            bombRect = bombSlider.GetComponent<RectTransform>();
            bombDefPos = bombRect.anchoredPosition;

        }

    }

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        HR_Events.OnPlayerSpawned += HR_PlayerHandler_OnPlayerSpawned;
        HR_Events.OnPlayerDied += HR_PlayerHandler_OnPlayerDied;

    }

    /// <summary>
    /// Event handler for the OnPlayerSpawned event
    /// </summary>
    /// <param name="_player">The player instance</param>
    private void HR_PlayerHandler_OnPlayerSpawned(HR_Player _player) {

        player = _player;
        content.SetActive(true);

    }

    /// <summary>
    /// Event handler for the OnPlayerDied event
    /// </summary>
    /// <param name="_player">The player instance</param>
    /// <param name="scores">Array of scores</param>
    private void HR_PlayerHandler_OnPlayerDied(HR_Player _player, int[] scores) {

        player = null;
        content.SetActive(false);

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        if (!player)
            return;

        if (combo) {

            if (player.combo > 1)
                comboMImage.rectTransform.anchoredPosition = Vector2.Lerp(comboMImage.rectTransform.anchoredPosition, comboDefPos, Time.deltaTime * 5f);
            else
                comboMImage.rectTransform.anchoredPosition = Vector2.Lerp(comboMImage.rectTransform.anchoredPosition, new Vector2(comboDefPos.x - 500, comboDefPos.y), Time.deltaTime * 5f);

        }

        if (highSpeed) {

            if (player.highSpeedCurrent > .1f)
                highSpeedImage.rectTransform.anchoredPosition = Vector2.Lerp(highSpeedImage.rectTransform.anchoredPosition, highSpeedDefPos, Time.deltaTime * 5f);
            else
                highSpeedImage.rectTransform.anchoredPosition = Vector2.Lerp(highSpeedImage.rectTransform.anchoredPosition, new Vector2(highSpeedDefPos.x + 500, highSpeedDefPos.y), Time.deltaTime * 5f);

        }

        if (oppositeDirection) {

            if (player.opposideDirectionCurrent > .1f)
                oppositeDirectionImage.rectTransform.anchoredPosition = Vector2.Lerp(oppositeDirectionImage.rectTransform.anchoredPosition, oppositeDirectionDefPos, Time.deltaTime * 5f);
            else
                oppositeDirectionImage.rectTransform.anchoredPosition = Vector2.Lerp(oppositeDirectionImage.rectTransform.anchoredPosition, new Vector2(oppositeDirectionDefPos.x - 500, oppositeDirectionDefPos.y), Time.deltaTime * 5f);

        }

        if (timeLeft) {

            if (HR_GamePlayManager.Instance.mode == HR_GamePlayManager.Mode.TimeAttack) {

                if (!timeLeft.gameObject.activeSelf)
                    timeAttackImage.gameObject.SetActive(true);

            } else {

                if (timeLeft.gameObject.activeSelf)
                    timeAttackImage.gameObject.SetActive(false);

            }

        }

        if (damageSlider) {

            damageSlider.value = player.damage;

        }

        if (bombSlider) {

            if (HR_GamePlayManager.Instance.mode == HR_GamePlayManager.Mode.Bomb) {

                if (!bombSlider.gameObject.activeSelf)
                    bombSlider.gameObject.SetActive(true);

            } else {

                if (bombSlider.gameObject.activeSelf)
                    bombSlider.gameObject.SetActive(false);

            }

            if (player.bombTriggered)
                bombRect.anchoredPosition = Vector2.Lerp(bombRect.anchoredPosition, bombDefPos, Time.deltaTime * 5f);
            else
                bombRect.anchoredPosition = Vector2.Lerp(bombRect.anchoredPosition, new Vector2(bombDefPos.x - 500, bombDefPos.y), Time.deltaTime * 5f);

        }

    }

    /// <summary>
    /// LateUpdate method called once per frame after Update
    /// </summary>
    private void LateUpdate() {

        if (!player)
            return;

        if (score)
            score.text = player.score.ToString("F0");

        if (speed)
            speed.text = player.speed.ToString("F0");

        if (distance)
            distance.text = (player.distance).ToString("F2");

        if (highSpeed)
            highSpeed.text = player.highSpeedCurrent.ToString("F1");

        if (oppositeDirection)
            oppositeDirection.text = player.opposideDirectionCurrent.ToString("F1");

        if (timeLeft)
            timeLeft.text = player.timeLeft.ToString("F1");

        if (combo)
            combo.text = player.combo.ToString();

        if (bombSlider && HR_GamePlayManager.Instance.mode == HR_GamePlayManager.Mode.Bomb)
            bombSlider.value = player.bombHealth / 100f;

    }

    /// <summary>
    /// Called when the object becomes disabled or inactive
    /// </summary>
    private void OnDisable() {

        HR_Events.OnPlayerSpawned -= HR_PlayerHandler_OnPlayerSpawned;
        HR_Events.OnPlayerDied -= HR_PlayerHandler_OnPlayerDied;

    }

}
