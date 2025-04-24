//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// General settings for Highway Racer.
/// </summary>
[System.Serializable]
public class HR_Settings : ScriptableObject {

    private static HR_Settings instance;
    public static HR_Settings Instance {

        get {

            if (instance == null)
                instance = Resources.Load("HR_Settings") as HR_Settings;

            return instance;

        }

    }

    /// <summary>
    /// Minimum speed limit for gaining score.
    /// </summary>
    public int minimumSpeedForGainScore = 80;

    /// <summary>
    /// Minimum speed limit for high speed score.
    /// </summary>
    public int minimumSpeedForHighSpeed = 100;

    /// <summary>
    /// Minimum collision impulse for crashes.
    /// </summary>
    public int minimumCollisionForGameOver = 4;

    /// <summary>
    /// Multiplier for distance score.
    /// </summary>
    public int totalDistanceMoneyMP = 360;

    /// <summary>
    /// Multiplier for near miss score.
    /// </summary>
    public int totalNearMissMoneyMP = 30;

    /// <summary>
    /// Multiplier for high speed score.
    /// </summary>
    public int totalOverspeedMoneyMP = 20;

    /// <summary>
    /// Multiplier for opposite direction score.
    /// </summary>
    public int totalOppositeDirectionMP = 30;

    /// <summary>
    /// Initial money when player runs the game for the first time.
    /// </summary>
    public int initialMoney = 10000;

    /// <summary>
    /// All selectable player vehicles.
    /// </summary>
    public GameObject[] selectablePlayerCars;

    /// <summary>
    /// All selectable upgradable wheels.
    /// </summary>
    public GameObject[] upgradableWheels;

    /// <summary>
    /// Explosion prefab used in bomb mode.
    /// </summary>
    public GameObject explosionEffect;

    /// <summary>
    /// Button click sound.
    /// </summary>
    public AudioClip buttonClickAudioClip;

    /// <summary>
    /// Near miss sound.
    /// </summary>
    public AudioClip nearMissAudioClip;

    /// <summary>
    /// Label slide sound.
    /// </summary>
    public AudioClip labelSlideAudioClip;

    /// <summary>
    /// Counting score sound.
    /// </summary>
    public AudioClip countingPointsAudioClip;

    /// <summary>
    /// Bomb timer sound.
    /// </summary>
    public AudioClip bombTimerAudioClip;

    /// <summary>
    /// Normal horn sound.
    /// </summary>
    public AudioClip hornClip;

    /// <summary>
    /// Siren horn sound.
    /// </summary>
    public AudioClip sirenAudioClip;

    /// <summary>
    /// Traffic cars will use this layer.
    /// </summary>
    public string trafficCarsLayer = "HR_TrafficCar";

    /// <summary>
    /// Default audio volume.
    /// </summary>
    public float defaultAudioVolume = 1f;

    /// <summary>
    /// Default music volume.
    /// </summary>
    public float defaultMusicVolume = .65f;

    /// <summary>
    /// Default draw distance.
    /// </summary>
    public float defaultDrawDistance = 350f;

    /// <summary>
    /// Default shadows.
    /// </summary>
    public bool defaultShadows = false;

    /// <summary>
    /// Default post processing.
    /// </summary>
    public bool defaultPP = false;

    /// <summary>
    /// Represents the main menu scene index in the build settings.
    /// </summary>
    public int mainMenuSceneIndex = 0;

    /// <summary>
    /// Reference to the main camera.
    /// </summary>
    [Header("Resources")]
    public HR_Camera gameplayCamera;
    public HR_Camera_Showroom showroomCamera;

    public HR_CurvedRoad road;
    public HR_TrafficCar[] trafficCars;

    public GameObject UI_GameplayPanel;
    public GameObject UI_GameoverPanel;
    public GameObject UI_MainmenuPanel;
    public EventSystem UI_EventSystem;

    public HR_ResetDecal resetDecal;



}
