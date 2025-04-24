//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Get, add, consume currency. Get unlocked vehicles, and unlocked parts string list.
/// </summary>
public class HR_API {

    /// <summary>
    /// Delegate for when player currency changes.
    /// </summary>
    public delegate void onPlayerMoneyChanged();

    /// <summary>
    /// Event triggered when player currency changes.
    /// </summary>
    public static event onPlayerMoneyChanged OnPlayerMoneyChanged;

    /// <summary>
    /// Delegate for when player name changes.
    /// </summary>
    public delegate void onPlayerNameChanged();

    /// <summary>
    /// Event triggered when player name changes.
    /// </summary>
    public static event onPlayerNameChanged OnPlayerNameChanged;

    /// <summary>
    /// Is this first gameplay?
    /// </summary>
    /// <returns></returns>
    public static bool IsFirstGameplay() {

        if (GetTotalPlayedTime() <= 1f)
            return true;
        else
            return false;

    }

    /// <summary>
    /// Sets the player name.
    /// </summary>
    /// <param name="newPlayerName"></param>
    public static void SetPlayerName(string newPlayerName) {

        PlayerPrefs.SetString("PlayerName", newPlayerName);
        SetTotalPlayedTime();

        if (OnPlayerNameChanged != null)
            OnPlayerNameChanged();

    }

    /// <summary>
    /// Gets the player name.
    /// </summary>
    /// <returns></returns>
    public static string GetPlayerName() {

        return PlayerPrefs.GetString("PlayerName", "New Player");

    }

    /// <summary>
    /// Gets the current currency as an integer.
    /// </summary>
    /// <returns>The current currency.</returns>
    public static int GetCurrency() {

        return PlayerPrefs.GetInt("Currency", 0);

    }

    /// <summary>
    /// Consumes the specified amount of currency.
    /// </summary>
    /// <param name="consume">The amount of currency to consume.</param>
    public static void ConsumeCurrency(int consume) {

        int current = GetCurrency();

        PlayerPrefs.SetInt("Currency", current - consume);

        if (OnPlayerMoneyChanged != null)
            OnPlayerMoneyChanged();

    }

    /// <summary>
    /// Adds the specified amount of currency.
    /// </summary>
    /// <param name="add">The amount of currency to add.</param>
    public static void AddCurrency(int add) {

        int current = GetCurrency();

        PlayerPrefs.SetInt("Currency", current + add);

        if (OnPlayerMoneyChanged != null)
            OnPlayerMoneyChanged();

    }

    /// <summary>
    /// Gets the indexes of unlocked vehicles.
    /// </summary>
    /// <returns>An array of indexes of unlocked vehicles.</returns>
    public static int[] UnlockedVehicles() {

        List<int> unlockeds = new List<int>();

        for (int i = 0; i < HR_PlayerCars.Instance.cars.Length; i++) {

            if (PlayerPrefs.HasKey(HR_PlayerCars.Instance.cars[i].playerCar.name + "Owned"))
                unlockeds.Add(i);

        }

        return unlockeds.ToArray();

    }

    /// <summary>
    /// Unlocks the specified vehicle.
    /// </summary>
    /// <param name="index">The index of the vehicle to unlock.</param>
    public static void UnlockVehicle(int index) {

        PlayerPrefs.SetInt(HR_PlayerCars.Instance.cars[index].playerCar.name + "Owned", 1);

    }

    /// <summary>
    /// Locks the specified vehicle.
    /// </summary>
    /// <param name="index">The index of the vehicle to lock.</param>
    public static void LockVehicle(int index) {

        PlayerPrefs.DeleteKey(HR_PlayerCars.Instance.cars[index].playerCar.name + "Owned");

    }

    /// <summary>
    /// Unlocks the specified vehicle.
    /// </summary>
    /// <param name="index">The index of the vehicle to unlock.</param>
    public static void UnlockAllVehicles() {

        for (int i = 0; i < HR_PlayerCars.Instance.cars.Length; i++) {

            if (HR_PlayerCars.Instance.cars[i] != null)
                PlayerPrefs.SetInt(HR_PlayerCars.Instance.cars[i].playerCar.name + "Owned", 1);

        }

    }

    /// <summary>
    /// Checks if the specified vehicle is owned.
    /// </summary>
    /// <param name="index">The index of the vehicle.</param>
    /// <returns>True if the vehicle is owned, otherwise false.</returns>
    public static bool OwnedVehicle(int index) {

        if (PlayerPrefs.HasKey(HR_PlayerCars.Instance.cars[index].playerCar.name + "Owned"))
            return true;
        else
            return false;

    }

    /// <summary>
    /// Saves the high scores.
    /// </summary>
    /// <param name="scores">An array of scores to save.</param>
    public static void SaveHighScores(int[] scores) {

        PlayerPrefs.SetInt("bestScoreOneWay", scores[0]);
        PlayerPrefs.SetInt("bestScoreTwoWay", scores[1]);
        PlayerPrefs.SetInt("bestScoreTimeAttack", scores[2]);
        PlayerPrefs.SetInt("bestScoreBomb", scores[3]);

    }

    /// <summary>
    /// Loads the high scores.
    /// </summary>
    /// <returns>An array of high scores.</returns>
    public static int[] GetHighScores() {

        int[] scores = new int[4];

        scores[0] = PlayerPrefs.GetInt("bestScoreOneWay");
        scores[1] = PlayerPrefs.GetInt("bestScoreTwoWay");
        scores[2] = PlayerPrefs.GetInt("bestScoreTimeAttack");
        scores[3] = PlayerPrefs.GetInt("bestScoreBomb");

        return scores;

    }

    /// <summary>
    /// Navigates to the main menu.
    /// </summary>
    public static void MainMenu() {

        SceneManager.LoadSceneAsync(0);
        SetTotalPlayedTime();

    }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public static void RestartGame() {

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SetTotalPlayedTime();

    }

    /// <summary>
    /// Sets the volume of the audiolistener.
    /// </summary>
    /// <param name="volume"></param>
    public static void SetAudioVolume(float volume) {

        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("AudioVolume", volume);
        HR_Events.Event_OnOptionsChanged();

        //Load();
        //CCDS_SaveGameManager.saveData.audioVolume = volume;
        //Save();

    }

    /// <summary>
    /// Sets the music volume.
    /// </summary>
    /// <param name="volume"></param>
    public static void SetMusicVolume(float volume) {

        PlayerPrefs.SetFloat("MusicVolume", volume);
        HR_Events.Event_OnOptionsChanged();

        //Load();
        //CCDS_SaveGameManager.saveData.musicVolume = volume;
        //Save();

    }

    /// <summary>
    /// Get volume of the audiolistener.
    /// </summary>
    /// <returns></returns>
    public static float GetAudioVolume() {

        //Load();
        //return CCDS_SaveGameManager.saveData.audioVolume;

        return PlayerPrefs.GetFloat("AudioVolume", HR_Settings.Instance.defaultAudioVolume);

    }

    /// <summary>
    /// Get volume of the music.
    /// </summary>
    /// <returns></returns>
    public static float GetMusicVolume() {

        //Load();
        //return CCDS_SaveGameManager.saveData.musicVolume;

        return PlayerPrefs.GetFloat("MusicVolume", HR_Settings.Instance.defaultMusicVolume);

    }

    /// <summary>
    /// Set the maximum draw distance for the main camera.
    /// </summary>
    /// <param name="distance"></param>
    public static void SetDrawDistance(float distance) {

        PlayerPrefs.SetFloat("DrawDistance", distance);
        HR_Events.Event_OnOptionsChanged();

    }

    /// <summary>
    /// Get the maximum draw distance for the main camera.
    /// </summary>
    /// <returns></returns>
    public static float GetDrawDistance() {

        return PlayerPrefs.GetFloat("DrawDistance", HR_Settings.Instance.defaultDrawDistance);

    }

    /// <summary>
    /// Gets the total played time.
    /// </summary>
    /// <returns></returns>
    public static float GetTotalPlayedTime() {

        return PlayerPrefs.GetFloat("TotalPlayedTime", 0f);

    }

    /// <summary>
    /// Sets the total player time.
    /// </summary>
    /// <param name="addTime"></param>
    public static void SetTotalPlayedTime() {

        float lastPlayedTime = Time.timeSinceLevelLoad;
        PlayerPrefs.SetFloat("TotalPlayedTime", GetTotalPlayedTime() + lastPlayedTime);

    }

    /// <summary>
    /// Set the controller type based on the selected button
    /// </summary>
    /// <param name="button">Selected button</param>
    public static void SetControllerType(int controllerIndex) {

        switch (controllerIndex) {

            case 0:
                PlayerPrefs.SetInt("ControllerType", 0);
                RCCP.SetMobileController(RCCP_Settings.MobileController.TouchScreen);
                break;
            case 1:
                PlayerPrefs.SetInt("ControllerType", 1);
                RCCP.SetMobileController(RCCP_Settings.MobileController.Gyro);
                break;
            case 2:
                PlayerPrefs.SetInt("ControllerType", 2);
                RCCP.SetMobileController(RCCP_Settings.MobileController.SteeringWheel);
                break;
            case 3:
                PlayerPrefs.SetInt("ControllerType", 3);
                RCCP.SetMobileController(RCCP_Settings.MobileController.Joystick);
                break;

        }

        HR_Events.Event_OnOptionsChanged();

    }

    /// <summary>
    /// Get the controller type.
    /// </summary>
    public static int GetControllerType() {

        switch (RCCP_Settings.Instance.mobileController) {

            case RCCP_Settings.MobileController.TouchScreen:
                return 0;
            case RCCP_Settings.MobileController.Gyro:
                return 1;
            case RCCP_Settings.MobileController.SteeringWheel:
                return 2;
            case RCCP_Settings.MobileController.Joystick:
                return 3;

        }

        return 0;

    }

    /// <summary>
    /// Set the quality level based on the selected button
    /// </summary>
    /// <param name="button">Selected button</param>
    public static void SetQuality(int qualityIndex) {

        QualitySettings.SetQualityLevel(qualityIndex);

        HR_Events.Event_OnOptionsChanged();

    }

    /// <summary>
    /// Get the quality level.
    /// </summary>
    public static int GetQuality() {

        return QualitySettings.GetQualityLevel();

    }

    /// <summary>
    /// Set the realtime shadows option
    /// </summary>
    public static void SetShadows() {

        PlayerPrefsX.SetBool("Shadows", !PlayerPrefsX.GetBool("Shadows", HR_Settings.Instance.defaultShadows));
        HR_Events.Event_OnOptionsChanged();

    }

    /// <summary>
    /// Get the realtime shadows option
    /// </summary>
    public static bool GetShadows() {

        return PlayerPrefsX.GetBool("Shadows", HR_Settings.Instance.defaultShadows);

    }

    /// <summary>
    /// Set the post processing option
    /// </summary>
    public static void SetPP() {

        PlayerPrefsX.SetBool("PP", !PlayerPrefsX.GetBool("PP", HR_Settings.Instance.defaultPP));
        HR_Events.Event_OnOptionsChanged();

    }

    /// <summary>
    /// Get the post processing option
    /// </summary>
    public static bool GetPP() {

        return PlayerPrefsX.GetBool("PP", HR_Settings.Instance.defaultPP);

    }

    /// <summary>
    /// Resets the game by deleting the save data and reloading the scene again.
    /// </summary>
    public static void ResetGame() {

        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync(HR_Settings.Instance.mainMenuSceneIndex);

    }

}
