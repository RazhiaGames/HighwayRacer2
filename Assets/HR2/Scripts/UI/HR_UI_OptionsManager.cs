//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class HR_UI_OptionsManager : MonoBehaviour {

    public GameObject pausedMenu;
    public GameObject pausedButtons;
    public GameObject optionsMenu;
    public GameObject restartButton;

    private void OnEnable() {

        HR_Events.OnPaused += OnPaused;
        HR_Events.OnResumed += OnResumed;

        restartButton.SetActive(true);

    }

    public void ResumeGame() {

        HR_GamePlayManager.Instance.Paused();

    }

    public void RestartGame() {

        HR_API.RestartGame();

    }

    public void MainMenu() {

        HR_API.MainMenu();

    }

    public void OptionsMenu(bool open) {

        optionsMenu.SetActive(open);

        if (open)
            pausedButtons.SetActive(false);
        else
            pausedButtons.SetActive(true);

    }

    private void OnPaused() {

        pausedMenu.SetActive(true);
        pausedButtons.SetActive(true);

        AudioListener.pause = true;
        Time.timeScale = 0;

    }

    public void OnResumed() {

        pausedMenu.SetActive(false);
        pausedButtons.SetActive(false);

        AudioListener.pause = false;
        Time.timeScale = 1;

    }

    public void ChangeCamera() {

        if (FindObjectOfType<HR_Camera>())
            FindObjectOfType<HR_Camera>().ChangeCameraMode();

    }

    private void OnDisable() {

        HR_Events.OnPaused -= OnPaused;
        HR_Events.OnResumed -= OnResumed;

    }

}