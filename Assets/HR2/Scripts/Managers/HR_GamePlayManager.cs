//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the gameplay, including spawning player vehicles, setting volume, setting mods, and listening to player events.
/// </summary>
public class HR_GamePlayManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_GamePlayManager instance;
    public static HR_GamePlayManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<HR_GamePlayManager>();
            }

            return instance;
        }
    }
    #endregion

    /// <summary>
    /// Is the game started?
    /// </summary>
    public bool gameStarted = false;

    /// <summary>
    /// Player object.
    /// </summary>
    public HR_Player player;

    /// <summary>
    /// Enum for the time of the scene: Day or Night.
    /// </summary>
    public enum DayOrNight { Day, Night }

    /// <summary>
    /// Time of the scene.
    /// </summary>
    [Header("Time Of The Scene")]
    public DayOrNight dayOrNight = DayOrNight.Day;

    /// <summary>
    /// Enum for the current mode: OneWay, TwoWay, TimeAttack, Bomb.
    /// </summary>
    public enum Mode { OneWay, TwoWay, TimeAttack, Bomb }

    /// <summary>
    /// Current mode of the game.
    /// </summary>
    [Header("Current Mode")]
    public Mode mode = Mode.OneWay;

    /// <summary>
    /// Spawn location of the cars.
    /// </summary>
    [Header("Spawn Location Of The Cars")]
    public Transform spawnLocation;

    /// <summary>
    /// Index of the selected car.
    /// </summary>
    private int selectedCarIndex = 0;

    /// <summary>
    /// Index of the selected mode.
    /// </summary>
    private int selectedModeIndex = 0;

    /// <summary>
    /// Is the game paused?
    /// </summary>
    private bool paused = false;

    /// <summary>
    /// Minimum speed at start.
    /// </summary>
    private readonly float minimumSpeedAtStart = 60f;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        // Setting time scale, volume, unpause, and target frame rate.
        Time.timeScale = 1f;
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.pause = false;

        if (HR_UIOptionsManager.Instance)
            HR_UIOptionsManager.Instance.OnEnable();

    }

    /// <summary>
    /// Called on start.
    /// </summary>
    private void Start() {

        // Make sure time scale is 1. We are setting volume to 0, we'll be increasing it smoothly in the update method.
        Time.timeScale = 1f;
        AudioListener.volume = 0f;
        AudioListener.pause = false;
        gameStarted = false;

        // Getting selected player car index and mode index.
        selectedCarIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex");
        selectedModeIndex = PlayerPrefs.GetInt("SelectedModeIndex");

        // Setting proper mode.
        switch (selectedModeIndex) {

            case 0:
                mode = Mode.OneWay;
                break;
            case 1:
                mode = Mode.TwoWay;
                break;
            case 2:
                mode = Mode.TimeAttack;
                break;
            case 3:
                mode = Mode.Bomb;
                break;

        }

        SpawnPlayer();     // Spawning the player vehicle.

        StartCoroutine(StartRaceDelayed());     //  Starting the race with a delay.
        StartCoroutine(AdjustAudioOnStart());       //  Adjusting the audiovolume at start.

    }

    /// <summary>
    /// Adjusting the audiovolume at start.
    /// </summary>
    private IEnumerator AdjustAudioOnStart() {

        float timer = 2f;

        while (timer > 0) {

            timer -= Time.deltaTime;

            // Adjusting volume smoothly.
            float targetVolume = HR_API.GetAudioVolume();
            AudioListener.volume = Mathf.MoveTowards(AudioListener.volume, targetVolume, Time.deltaTime * 3f);

            yield return null;

        }

        AudioListener.volume = HR_API.GetAudioVolume();

    }

    /// <summary>
    /// Spawns the player car.
    /// </summary>
    private void SpawnPlayer() {

        player = (RCCP.SpawnRCC(HR_PlayerCars.Instance.cars[selectedCarIndex].playerCar.GetComponent<RCCP_CarController>(), spawnLocation.position, spawnLocation.rotation, true, false, true)).GetComponent<HR_Player>();
        player.canCrash = true;
        player.Rigid.linearVelocity = new Vector3(0f, 0f, minimumSpeedAtStart / 3.6f);
        StartCoroutine(CheckDayTime());

        LoadCustomization();
        ApplyCustomization();

        // Listening event when player spawned.
        HR_Events.Event_OnPlayerSpawned(player);

    }

    /// <summary>
    /// Countdown before the game.
    /// </summary>
    /// <returns>Enumerator for coroutine.</returns>
    public IEnumerator StartRaceDelayed() {

        HR_Events.Event_OnCountDownStarted();

        yield return new WaitForSeconds(4);

        gameStarted = true;
        RCCP.SetControl(player.GetComponent<RCCP_CarController>(), true);

        HR_Events.Event_OnRaceStarted();

    }

    /// <summary>
    /// Checks the time and adjusts lights.
    /// </summary>
    /// <returns>Enumerator for coroutine.</returns>
    private IEnumerator CheckDayTime() {

        if (player.GetComponent<RCCP_CarController>().Lights == null)
            yield break;

        yield return new WaitForFixedUpdate();

        if (dayOrNight == DayOrNight.Night)
            player.GetComponent<RCCP_CarController>().Lights.lowBeamHeadlights = true;
        else
            player.GetComponent<RCCP_CarController>().Lights.lowBeamHeadlights = false;

    }

    /// <summary>
    /// Handles player crash.
    /// </summary>
    /// <param name="player">The crashed player.</param>
    /// <param name="scores">The scores of the player.</param>
    public void CrashedPlayer(HR_Player player, int[] scores) {

        gameStarted = false;

        HR_Events.Event_OnPlayerDied(player, scores);

        StartCoroutine(FinishRaceDelayed(1f));

    }

    /// <summary>
    /// Finishes the race after a delay and saves the high score.
    /// </summary>
    /// <param name="delayTime">The delay time in seconds.</param>
    /// <returns>Enumerator for coroutine.</returns>
    public IEnumerator FinishRaceDelayed(float delayTime) {

        yield return new WaitForSecondsRealtime(delayTime);
        FinishRace();

    }

    /// <summary>
    /// Finishes the race and saves the high score instantly.
    /// </summary>
    public void FinishRace() {

        switch (mode) {

            case Mode.OneWay:
                PlayerPrefs.SetInt("bestScoreOneWay", (int)player.score);
                break;
            case Mode.TwoWay:
                PlayerPrefs.SetInt("bestScoreTwoWay", (int)player.score);
                break;
            case Mode.TimeAttack:
                PlayerPrefs.SetInt("bestScoreTimeAttack", (int)player.score);
                break;
            case Mode.Bomb:
                PlayerPrefs.SetInt("bestScoreBomb", (int)player.score);
                break;

        }

    }

    /// <summary>
    /// Navigates to the main menu.
    /// </summary>
    public void MainMenu() {

        HR_API.MainMenu();

    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void RestartGame() {

        HR_API.RestartGame();

    }

    /// <summary>
    /// Pauses or resumes the game.
    /// </summary>
    public void Paused() {

        paused = !paused;

        if (paused)
            HR_Events.Event_OnPaused();
        else
            HR_Events.Event_OnResumed();

    }

    /// <summary>
    /// Saves the current loadout.
    /// </summary>
    public void SaveCustomization() {

        HR_Player currentVehicle = player;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Save();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    /// <summary>
    /// Loads the latest loadout.
    /// </summary>
    public void LoadCustomization() {

        HR_Player currentVehicle = player;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Load();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    /// <summary>
    /// Applies the loaded loadout.
    /// </summary>
    public void ApplyCustomization() {

        HR_Player currentVehicle = player;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Initialize();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

    }

    /// <summary>
    /// Resets the spawn location.
    /// </summary>
    private void Reset() {

        if (spawnLocation == null) {

            GameObject spawnLocationGO = GameObject.Find("HR_SpawnLocation");

            if (spawnLocationGO)
                spawnLocation = spawnLocationGO.transform;

            if (spawnLocation)
                return;

            spawnLocation = new GameObject("HR_SpawnLocation").transform;
            spawnLocation.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            spawnLocation.position += Vector3.up * 1f;
            spawnLocation.position += Vector3.forward * 10f;
            spawnLocation.position += Vector3.right * 1.8f;

        }

    }

}
