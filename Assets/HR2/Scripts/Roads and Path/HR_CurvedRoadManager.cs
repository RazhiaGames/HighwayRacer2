//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Pooling the road with given amount. Calculates total length of the pool, and translates previous roads to the next position.
/// </summary>
public class HR_CurvedRoadManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_CurvedRoadManager instance;
    public static HR_CurvedRoadManager Instance {

        get {

            if (instance == null)
                instance = FindObjectOfType<HR_CurvedRoadManager>();

            return instance;

        }

    }
    #endregion

    [System.Serializable]
    public class RoadObjects {

        public HR_CurvedRoad road;

    }

    /// <summary>
    /// Array of road objects to be managed.
    /// </summary>
    public RoadObjects[] roads;

    /// <summary>
    /// List of spawned roads.
    /// </summary>
    public List<HR_CurvedRoad> spawnedRoads = new List<HR_CurvedRoad>();

    private HR_CurvedRoad lastRoad;

    /// <summary>
    /// Container for spawned roads.
    /// </summary>
    public GameObject spawnedRoadsContainer;

    /// <summary>
    /// Delegate for the event triggered when all roads are aligned.
    /// </summary>
    /// <param name="allRoads">List of all aligned HR_CurvedRoad objects.</param>
    public delegate void onAllRoadsAligned(List<HR_CurvedRoad> allRoads);

    /// <summary>
    /// Event triggered when all roads are aligned.
    /// </summary>
    public static event onAllRoadsAligned OnAllRoadsAligned;

    /// <summary>
    /// Delegate for the event triggered when a single road is aligned.
    /// </summary>
    /// <param name="road">The aligned HR_CurvedRoad object.</param>
    public delegate void onRoadAligned(HR_CurvedRoad road);

    /// <summary>
    /// Event triggered when a single road is aligned.
    /// </summary>
    public static event onRoadAligned OnRoadAligned;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        for (int i = 0; i < roads.Length; i++) {

            if (roads[i] != null && roads[i].road.gameObject.scene != null)
                roads[i].road.gameObject.SetActive(false);

        }

        // Creating the roads.
        CreateRoads();

    }

    /// <summary>
    /// Creates all roads.
    /// </summary>
    private void CreateRoads() {

        // Creating container for the spawned traffic cars.
        spawnedRoadsContainer = GameObject.Find("HR_CurvedRoads");

        if (!spawnedRoadsContainer)
            spawnedRoadsContainer = new GameObject("HR_CurvedRoads");

        spawnedRoadsContainer.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        for (int k = 0; k < roads.Length; k++) {

            roads[k].road.gameObject.isStatic = false;

            foreach (Transform item in roads[k].road.transform)
                item.gameObject.isStatic = false;

            HR_CurvedRoad newRoad = Instantiate(roads[k].road.gameObject, roads[k].road.transform.position, roads[k].road.transform.rotation).GetComponent<HR_CurvedRoad>();
            spawnedRoads.Add(newRoad);
            newRoad.gameObject.SetActive(true);

            /*if (k != 0)
                newRoad.RandomizeCurve();
*/
            newRoad.transform.SetParent(spawnedRoadsContainer.transform);

        }

        for (int i = 0; i < spawnedRoads.Count; i++) {

            if (i != 0) {

                spawnedRoads[i].transform.position = spawnedRoads[i - 1].endPoint.position;
                spawnedRoads[i].transform.rotation = spawnedRoads[i - 1].endPoint.rotation;

            }

        }

        int totalCreatedRoads = spawnedRoads.Count;
        int minimumRequiredRoads = 3;

        if (totalCreatedRoads < minimumRequiredRoads)
            CreateRoads();

        if (totalCreatedRoads >= minimumRequiredRoads) {

            if (OnAllRoadsAligned != null)
                OnAllRoadsAligned(spawnedRoads);

        }

    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        // Animating the roads.
        AnimateRoads();

    }

    /// <summary>
    /// Animating the roads.
    /// </summary>
    private void AnimateRoads() {

        Camera mainCamera = Camera.main;

        if (!mainCamera)
            return;

        for (int i = 0; i < spawnedRoads.Count; i++) {

            if (IsInFront(spawnedRoads[i].endPoint.gameObject, mainCamera.transform.root.gameObject)) {
                if (!lastRoad) {

                    spawnedRoads[i].transform.position = spawnedRoads[spawnedRoads.Count - 1].endPoint.position;
                    spawnedRoads[i].transform.rotation = spawnedRoads[spawnedRoads.Count - 1].endPoint.rotation;

                } else {

                    spawnedRoads[i].transform.position = lastRoad.endPoint.position;
                    spawnedRoads[i].transform.rotation = lastRoad.endPoint.rotation;

                }

                lastRoad = spawnedRoads[i];
                lastRoad.transform.SetAsLastSibling();

                /*if (i != 0)
                    lastRoad.RandomizeCurve();
*/
                if (OnRoadAligned != null)
                {
                    OnRoadAligned(lastRoad);
                }

            }

        }

    }

    /// <summary>
    /// Checks if the target GameObject is in front of the other GameObject.
    /// </summary>
    /// <param name="target">The target GameObject.</param>
    /// <param name="other">The other GameObject.</param>
    /// <returns>True if the target is in front of the other, otherwise false.</returns>
    private bool IsInFront(GameObject target, GameObject other) {

        if (Vector3.Distance(target.transform.position, other.transform.position) < 100f)
            return false;

        Vector3 directionToOther = (other.transform.position - target.transform.position).normalized;
        float dotProduct = Vector3.Dot(target.transform.forward, directionToOther);

        return dotProduct > 0;

    }

}
