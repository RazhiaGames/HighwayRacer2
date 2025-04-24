//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AshVP;
using Random = UnityEngine.Random;

/// <summary>
/// Manages the traffic cars in the game.
/// </summary>
public class HR_TrafficManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_TrafficManager instance;
    public static HR_TrafficManager Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<HR_TrafficManager>();
            return instance;
        }
    }
    #endregion

    // Getting an Instance of HR_GamePlayHandler.
    #region HR_GamePlayHandler Instance

    private HR_GamePlayManager _GameplayManager;
    public HR_GamePlayManager GameplayManager {

        get {

            if (_GameplayManager == null)
                _GameplayManager = HR_GamePlayManager.Instance;

            return _GameplayManager;

        }

    }

    #endregion

    /// <summary>
    /// Reference to the lane manager.
    /// </summary>
    private HR_LaneManager laneManager;

    /// <summary>
    /// Property to get the lane manager instance.
    /// </summary>
    public HR_LaneManager LaneManager {
        get {
            if (!laneManager)
                laneManager = HR_LaneManager.Instance;

            return laneManager;
        }
    }

    /// <summary>
    /// Array of traffic cars.
    /// </summary>
    public TrafficCars[] trafficCars;

    /// <summary>
    /// Class representing traffic cars and their spawn amounts.
    /// </summary>
    [System.Serializable]
    public class TrafficCars {
        /// <summary>
        /// The traffic car prefab.
        /// </summary>
        public AiCarContrtoller trafficCar;

        /// <summary>
        /// The amount of this traffic car to spawn.
        /// </summary>
        public int amount = 1;
    }

    [Space()]

    /// <summary>
    /// Minimum distance for spawning traffic cars.
    /// </summary>
    public float minDistance = 300f;

    /// <summary>
    /// Maximum distance for spawning traffic cars.
    /// </summary>
    public float maxDistance = 600f;

    public float realignThreshold = 100f;

    /// <summary>
    /// List of spawned traffic cars.
    /// </summary>
    private List<AiCarContrtoller> spawnedTrafficCars = new List<AiCarContrtoller>();

    /// <summary>
    /// Container for the spawned traffic cars.
    /// </summary>
    [HideInInspector] public GameObject spawnedTrafficCarsContainer;

   


    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Start() {

        CreateTraffic();

    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        if (HR_GamePlayManager.Instance.gameStarted)
            AnimateTraffic();

    }

    /// <summary>
    /// Spawns all traffic cars.
    /// </summary>
    private void CreateTraffic() {

        // Creating container for the spawned traffic cars.
        spawnedTrafficCarsContainer = new GameObject("HR_TrafficContainer");

        for (int i = 0; i < trafficCars.Length; i++) {

            for (int k = 0; k < trafficCars[i].amount; k++) {

                GameObject go;

                go = Instantiate(trafficCars[i].trafficCar.gameObject, Vector3.zero,Quaternion.identity);
                spawnedTrafficCars.Add(go.GetComponent<AiCarContrtoller>());
                go.SetActive(false);
                go.transform.SetParent(spawnedTrafficCarsContainer.transform, true);
                go.transform.position -= Vector3.forward * 100f;

            }

        }

        Invoke(nameof(Populate), .1f);

    }

    /// <summary>
    /// Animates the traffic cars.
    /// </summary>
    private void Populate() {

        // If there is no camera, return.
        if (!Camera.main)
            return;

        // If traffic car is below the camera or too far away, realign.
        for (int i = 0; i < spawnedTrafficCars.Count; i++) {

            //if (Camera.main.transform.position.z > (spawnedTrafficCars[i].transform.position.z + 100) || Camera.main.transform.position.z < (spawnedTrafficCars[i].transform.position.z - maxDistance))
            ReAlignTraffic(spawnedTrafficCars[i], true,true);

        }

    }

    /// <summary>
    /// Animates the traffic cars.
    /// </summary>
    private void AnimateTraffic() {

        // If there is no camera, return.
        if (!Camera.main)
            return;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camPos = Camera.main.transform.position;

        // If traffic car is below the camera or too far away, realign.
        for (int i = 0; i < spawnedTrafficCars.Count; i++) {

            AiCarContrtoller car = spawnedTrafficCars[i];
            Vector3 toCar = car.transform.position - camPos;

            float distanceInViewDirection = Vector3.Dot(toCar, camForward);
            if (distanceInViewDirection < -realignThreshold || distanceInViewDirection > maxDistance)
            {
                ReAlignTraffic(car, false, false);
            }

            /*if (Camera.main.transform.position.z > (spawnedTrafficCars[i].transform.position.z + realignThreshold) || Camera.main.transform.position.z < (spawnedTrafficCars[i].transform.position.z - maxDistance))
                ReAlignTraffic(spawnedTrafficCars[i], false,false);*/   
        }

    }



    /// <summary>
    /// Realigns the traffic car.
    /// </summary>
    /// <param name="realignableObject">The traffic car to realign.</param>
    public void ReAlignTraffic(AiCarContrtoller realignableObject, bool ignoreDistanceToRef, bool firstTimeCreating)
    {
        
        HR_LaneManager.Lane[] lanes = HR_LaneManager.Instance.lanes;
        HR_LaneManager.Lane randomLane = lanes[Random.Range(0, lanes.Length)];
        float distance = Random.Range(minDistance, maxDistance);

        if (ignoreDistanceToRef)
            distance = Random.Range(60f, minDistance);

        Vector3 pos = randomLane.lane.FindClosestPointOnPath(Camera.main.transform.position + (Camera.main.transform.forward * distance), out Vector3 dir);


        if (firstTimeCreating)
            pos += Vector3.up;

        realignableObject.currentLane = randomLane.lane;
        realignableObject.transform.position = pos;
        realignableObject.transform.rotation = Quaternion.identity;

        switch (HR_GamePlayManager.Instance.mode)
        {
            case HR_GamePlayManager.Mode.OneWay:
                realignableObject.transform.forward = dir;
                realignableObject.GetComponent<AiCarContrtoller>().oppositeDirection = false;
                break;

            case HR_GamePlayManager.Mode.TwoWay:
                realignableObject.transform.forward = randomLane.lane.leftSide ? -dir : dir;
                realignableObject.GetComponent<AiCarContrtoller>().oppositeDirection = randomLane.lane.leftSide;
                break;

            case HR_GamePlayManager.Mode.TimeAttack:
            case HR_GamePlayManager.Mode.Bomb:
                realignableObject.transform.forward = dir;
                break;
        }

        if (!firstTimeCreating)
            realignableObject.RealignCar();

        realignableObject.transform.position += realignableObject.transform.up * realignableObject.spawnHeight;
        realignableObject.gameObject.SetActive(true);
        if (CheckIfClipping(realignableObject.triggerCollider))
        {
            realignableObject.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Checks if the new aligned car is clipping with another traffic car.
    /// </summary>
    /// <param name="trafficCarBound">The bounding box of the traffic car.</param>
    /// <returns>True if clipping, false otherwise.</returns>
    private bool CheckIfClipping(BoxCollider trafficCarBound) {

        for (int i = 0; i < spawnedTrafficCars.Count; i++) {

            if (!trafficCarBound.transform.IsChildOf(spawnedTrafficCars[i].transform) && spawnedTrafficCars[i].gameObject.activeSelf) {

                if (HR_BoundsExtension.ContainBounds(trafficCarBound.transform, trafficCarBound.bounds, spawnedTrafficCars[i].triggerCollider.bounds))
                    return true;

            }

        }

        return false;

    }

}
