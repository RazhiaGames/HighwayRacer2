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

public class HR_UI_GameOverPanel : MonoBehaviour {

    /// <summary>
    /// Content game object
    /// </summary>
    public GameObject content;

    /// <summary>
    /// UI Texts on scoreboard
    /// </summary>
    [Header("UI Texts On Scoreboard")]
    public TextMeshProUGUI totalScore;
    public TextMeshProUGUI totalMoney;

    public TextMeshProUGUI totalDistance;
    public TextMeshProUGUI totalNearMiss;
    public TextMeshProUGUI totalOverspeed;
    public TextMeshProUGUI totalOppositeDirection;

    public TextMeshProUGUI totalDistanceMoney;
    public TextMeshProUGUI totalNearMissMoney;
    public TextMeshProUGUI totalOverspeedMoney;
    public TextMeshProUGUI totalOppositeDirectionMoney;
    public TextMeshProUGUI winOrLoseText;

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        HR_Events.OnPlayerDied += HR_PlayerHandler_OnPlayerDied;

    }

    /// <summary>
    /// Event handler for the OnPlayerDied event
    /// </summary>
    /// <param name="player">The player instance</param>
    /// <param name="scores">Array of scores</param>
    private void HR_PlayerHandler_OnPlayerDied(HR_Player player, int[] scores) {

        StartCoroutine(DisplayResults(player, scores));

    }

    /// <summary>
    /// Coroutine to display the results
    /// </summary>
    /// <param name="player">The player instance</param>
    /// <param name="scores">Array of scores</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator DisplayResults(HR_Player player, int[] scores) {

        yield return new WaitForSecondsRealtime(1f);

        content.SetActive(true);

        if (totalScore)
            totalScore.text = Mathf.Floor(player.score).ToString("F0");

        if (totalDistance)
            totalDistance.text = (player.distance).ToString("F2");

        if (totalNearMiss)
            totalNearMiss.text = (player.nearMisses).ToString("F0");

        if (totalOverspeed)
            totalOverspeed.text = (player.highSpeedTotal).ToString("F1");

        if (totalOppositeDirection)
            totalOppositeDirection.text = (player.opposideDirectionTotal).ToString("F1");

        if (totalDistanceMoney)
            totalDistanceMoney.text = scores[0].ToString("F0");

        if (totalNearMissMoney)
            totalNearMissMoney.text = scores[1].ToString("F0");

        if (totalOverspeedMoney)
            totalOverspeedMoney.text = scores[2].ToString("F0");

        if (totalOppositeDirectionMoney)
            totalOppositeDirectionMoney.text = scores[3].ToString("F0");

        if (totalMoney)
            totalMoney.text = (scores[0] + scores[1] + scores[2] + scores[3]).ToString();

        gameObject.BroadcastMessage("Animate");
        gameObject.BroadcastMessage("GetNumber");

    }

    /// <summary>
    /// Method to navigate to the main menu
    /// </summary>
    public void MainMenu() {

        HR_API.MainMenu();

    }

    /// <summary>
    /// Method to restart the game
    /// </summary>
    public void Restart() {

        HR_API.RestartGame();

    }

    /// <summary>
    /// Called when the object becomes disabled or inactive
    /// </summary>
    private void OnDisable() {

        HR_Events.OnPlayerDied -= HR_PlayerHandler_OnPlayerDied;

    }

}
