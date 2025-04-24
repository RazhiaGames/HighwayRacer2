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
/// Fixes the floating origin when the player gets too far away from the origin. Repositions all important and necessary gameobjects to the 0 point.
/// </summary>
public class HR_FixFloatingOrigin : MonoBehaviour {

    /// <summary>
    /// Necessary gameobjects.
    /// </summary>
    private List<GameObject> targetGameObjects = new List<GameObject>();

    /// <summary>
    /// Target Z limit.
    /// </summary>
    public float zLimit = 2000f;

    /// <summary>
    /// Resets the position of the important game objects back to the origin.
    /// </summary>
    private void ResetBack() {

        if (targetGameObjects == null)
            targetGameObjects = new List<GameObject>();
        else
            targetGameObjects.Clear();

        if (HR_TrafficManager.Instance)
            targetGameObjects.Add(HR_TrafficManager.Instance.spawnedTrafficCarsContainer);

        if (HR_CurvedRoadManager.Instance)
            targetGameObjects.Add(HR_CurvedRoadManager.Instance.spawnedRoadsContainer);

        if (HR_LaneManager.Instance)
            targetGameObjects.Add(HR_LaneManager.Instance.gameObject);

        if (RCCP_SceneManager.Instance.activePlayerVehicle)
            targetGameObjects.Add(RCCP_SceneManager.Instance.activePlayerVehicle.gameObject);

        if (FindFirstObjectByType<HR_Camera>())
            targetGameObjects.Add(FindFirstObjectByType<HR_Camera>().gameObject);

        // Creating parent gameobject, adding necessary gameobjects, repositioning them, and lastly destroy the parent.
        GameObject parentGameObject = new GameObject("Parent");

        for (int i = 0; i < targetGameObjects.Count; i++)
            targetGameObjects[i].transform.SetParent(parentGameObject.transform, true);

        parentGameObject.transform.position -= Vector3.forward * zLimit;

        for (int i = 0; i < targetGameObjects.Count; i++)
            targetGameObjects[i].transform.SetParent(null);

        HR_GamePlayManager.Instance.player.distance -= zLimit / 1000f;

        Destroy(parentGameObject);

    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        // If no player vehicle found, return.
        if (!RCCP_SceneManager.Instance.activePlayerVehicle)
            return;

        // Getting distance.
        float distance = RCCP_SceneManager.Instance.activePlayerVehicle.transform.position.z;

        // If distance exceeds the limits, reset.
        if (distance >= zLimit)
            ResetBack();

    }

}
