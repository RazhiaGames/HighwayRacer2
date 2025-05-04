//----------------------------------------------
//                   Highway Racer
//
// Copyright � 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the direction of the path for the vehicles.
/// </summary>
public class HR_PathDirection : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_PathDirection instance;
    public static HR_PathDirection Instance {

        get {

            if (instance == null)
                instance = FindFirstObjectByType<HR_PathDirection>();

            if (instance == null)
                instance = new GameObject("HR_PathDirection").AddComponent<HR_PathDirection>();

            return instance;

        }

    }
    #endregion

    /// <summary>
    /// Called once per frame to update the forward direction of the transform to match the path angle.
    /// </summary>
    private void Update() {

        transform.forward = HR_PathManager.Instance.GetPathAngle();

    }

}
