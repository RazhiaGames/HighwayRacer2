//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages various game events for Highway Racer.
/// </summary>
public class HR_Events {

    /// <summary>
    /// Delegate for when the countdown starts.
    /// </summary>
    public delegate void onCountDownStarted();
    public static event onCountDownStarted OnCountDownStarted;

    /// <summary>
    /// Delegate for when the race starts.
    /// </summary>
    public delegate void onRaceStarted();
    public static event onRaceStarted OnRaceStarted;

    /// <summary>
    /// Delegate for when a player is spawned.
    /// </summary>
    public delegate void onPlayerSpawned(HR_Player player);
    public static event onPlayerSpawned OnPlayerSpawned;

    /// <summary>
    /// Delegate for when a player dies.
    /// </summary>
    public delegate void onPlayerDied(HR_Player player, int[] scores);
    public static event onPlayerDied OnPlayerDied;

    /// <summary>
    /// Delegate for when the game is paused.
    /// </summary>
    public delegate void onPaused();
    public static event onPaused OnPaused;

    /// <summary>
    /// Delegate for when the game is resumed.
    /// </summary>
    public delegate void onResumed();
    public static event onResumed OnResumed;

    /// <summary>
    /// When player vehicle changed in the main menu.
    /// </summary>
    public static event onVehicleChanged OnVehicleChanged;
    public delegate void onVehicleChanged(int carIndex);

    /// <summary>
    /// Delegate for when options change
    /// </summary>
    public delegate void OptionsChanged();
    public static event OptionsChanged OnOptionsChanged;

    /// <summary>
    /// Triggers the OnCountDownStarted event.
    /// </summary>
    public static void Event_OnCountDownStarted() {

        if (OnCountDownStarted != null)
            OnCountDownStarted();

    }

    /// <summary>
    /// Triggers the OnRaceStarted event.
    /// </summary>
    public static void Event_OnRaceStarted() {

        if (OnRaceStarted != null)
            OnRaceStarted();

    }

    /// <summary>
    /// Triggers the OnPlayerSpawned event.
    /// </summary>
    public static void Event_OnPlayerSpawned(HR_Player player) {

        if (OnPlayerSpawned != null)
            OnPlayerSpawned(player);

    }

    /// <summary>
    /// Triggers the OnPlayerDied event.
    /// </summary>
    public static void Event_OnPlayerDied(HR_Player player, int[] scores) {

        if (OnPlayerDied != null)
            OnPlayerDied(player, scores);

    }

    /// <summary>
    /// Triggers the OnPaused event.
    /// </summary>
    public static void Event_OnPaused() {

        if (OnPaused != null)
            OnPaused();

    }

    /// <summary>
    /// Triggers the OnResumed event.
    /// </summary>
    public static void Event_OnResumed() {

        if (OnResumed != null)
            OnResumed();

    }

    /// <summary>
    /// Triggers the audio changed event.
    /// </summary>
    public static void Event_OnOptionsChanged() {

        if (OnOptionsChanged != null)
            OnOptionsChanged();

    }

    /// <summary>
    /// Triggers the player vehicle changed event.
    /// </summary>
    public static void Event_OnVehicleChanged(int carIndex) {

        if (OnVehicleChanged != null)
            OnVehicleChanged(carIndex);

    }

}
