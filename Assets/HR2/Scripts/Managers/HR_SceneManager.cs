//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages and observes all major systems such as gameplay, missions, markers, spawn positions, and minimap.
/// </summary>
public class HR_SceneManager : HR_Singleton<HR_SceneManager> {

    public enum LevelType { MainMenu, Gameplay }
    public LevelType levelType = LevelType.Gameplay;

    #region GAMEPLAYCOMPONENTS

    /// <summary>
    /// Gameplay manager.
    /// </summary>
    public HR_GamePlayManager GameplayManager {
        get {
            if (gameplayManager == null)
                gameplayManager = FindObjectOfType<HR_GamePlayManager>();
            return gameplayManager;
        }
    }

    /// <summary>
    /// Curved road manager.
    /// </summary>
    public HR_CurvedRoadManager CurvedRoadManager {
        get {
            if (curvedRoadManager == null)
                curvedRoadManager = FindObjectOfType<HR_CurvedRoadManager>();
            return curvedRoadManager;
        }
    }

    /// <summary>
    /// Path manager.
    /// </summary>
    public HR_PathManager PathManager {
        get {
            if (pathManager == null)
                pathManager = FindObjectOfType<HR_PathManager>();
            return pathManager;
        }
    }

    /// <summary>
    /// Traffic manager.
    /// </summary>
    public HR_TrafficManager TrafficManager {
        get {
            if (trafficManager == null)
                trafficManager = FindObjectOfType<HR_TrafficManager>();
            return trafficManager;
        }
    }

    /// <summary>
    /// Lane manager.
    /// </summary>
    public HR_LaneManager LaneManager {
        get {
            if (laneManager == null)
                laneManager = FindObjectOfType<HR_LaneManager>();
            return laneManager;
        }
    }

    /// <summary>
    /// Player camera.
    /// </summary>
    public HR_Camera PlayerCamera {
        get {
            if (playerCamera == null)
                playerCamera = FindObjectOfType<HR_Camera>();
            return playerCamera;
        }
    }

    /// <summary>
    /// UI Gameplay panel.
    /// </summary>
    public HR_UI_GameplayPanel GameplayPanel {
        get {
            if (gameplayPanel == null)
                gameplayPanel = FindObjectOfType<HR_UI_GameplayPanel>();
            return gameplayPanel;
        }
    }

    /// <summary>
    /// UI Gameover panel.
    /// </summary>
    public HR_UI_GameOverPanel GameoverPanel {
        get {
            if (gameoverPanel == null)
                gameoverPanel = FindObjectOfType<HR_UI_GameOverPanel>();
            return gameoverPanel;
        }
    }

    /// <summary>
    /// Event system.
    /// </summary>
    public EventSystem Event {
        get {
            if (eventSystem == null)
                eventSystem = FindObjectOfType<EventSystem>();
            return eventSystem;
        }
    }

    #endregion

    #region MAINMENUCOMPONENTS

    /// <summary>
    /// Mainmenu manager.
    /// </summary>
    public HR_MainMenuManager MainMenuManager {
        get {
            if (mainmenuManager == null)
                mainmenuManager = FindObjectOfType<HR_MainMenuManager>();
            return mainmenuManager;
        }
    }

    /// <summary>
    /// Mainmenu UI panel.
    /// </summary>
    public HR_UI_MainmenuPanel MainMenuPanel {
        get {
            if (mainmenuPanel == null)
                mainmenuPanel = FindObjectOfType<HR_UI_MainmenuPanel>();
            return mainmenuPanel;
        }
    }

    /// <summary>
    /// Showroom camera.
    /// </summary>
    public HR_Camera_Showroom ShowroomCamera {
        get {
            if (showroomCamera == null)
                showroomCamera = FindObjectOfType<HR_Camera_Showroom>();
            return showroomCamera;
        }
    }

    #endregion

    #region RCCPCOMPONENTS

    /// <summary>
    /// RCCP Scene Manager.
    /// </summary>
    public RCCP_SceneManager RCCPSceneManager {
        get {
            if (rccpSceneManager == null)
                rccpSceneManager = FindObjectOfType<RCCP_SceneManager>();
            return rccpSceneManager;
        }
    }

    #endregion

    #region GET
    private HR_GamePlayManager gameplayManager;
    private HR_CurvedRoadManager curvedRoadManager;
    private HR_PathManager pathManager;
    private HR_TrafficManager trafficManager;
    private HR_LaneManager laneManager;
    private HR_Camera playerCamera;
    private HR_UI_GameplayPanel gameplayPanel;
    private HR_UI_GameOverPanel gameoverPanel;
    private EventSystem eventSystem;

    private HR_MainMenuManager mainmenuManager;
    private HR_UI_MainmenuPanel mainmenuPanel;
    private HR_Camera_Showroom showroomCamera;

    private RCCP_SceneManager rccpSceneManager;
    #endregion

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {
        // Getting and checking all components.
        GetAllComponents();
    }

    /// <summary>
    /// Gets all major manager components.
    /// </summary>
    public void GetAllComponents() {

        switch (levelType) {

            case LevelType.Gameplay:

                HR_GamePlayManager _gameplayManager = GameplayManager;
                RCCP_SceneManager _rccpSceneManager = RCCPSceneManager;
                HR_CurvedRoadManager _curvedRoadManager = CurvedRoadManager;
                HR_PathManager _pathManager = PathManager;
                HR_TrafficManager _trafficManager = TrafficManager;
                HR_LaneManager _laneManager = LaneManager;
                HR_Camera _playerCamera = PlayerCamera;

                break;

            case LevelType.MainMenu:

                HR_MainMenuManager _mainmenuManager = MainMenuManager;

                break;

        }

    }

}
