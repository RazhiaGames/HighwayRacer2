//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Manages all selectable player cars as a scriptable object.
/// </summary>
[System.Serializable]
public class HR_PlayerCars : ScriptableObject {

    private static HR_PlayerCars instance;
    public static HR_PlayerCars Instance {
        get {
            if (instance == null)
                instance = Resources.Load("HR_PlayerCars") as HR_PlayerCars;
            return instance;
        }
    }

    /// <summary>
    /// The last car added.
    /// </summary>
    [Space(10f)]
    public Cars lastAdd;

    /// <summary>
    /// Represents a car with its details.
    /// </summary>
    [System.Serializable]
    public class Cars {
        /// <summary>
        /// Name of the vehicle.
        /// </summary>
        public string vehicleName = "";

        /// <summary>
        /// The car GameObject.
        /// </summary>
        public GameObject playerCar;

        /// <summary>
        /// Whether the car is unlocked.
        /// </summary>
        public bool unlocked = false;

        /// <summary>
        /// Price of the car.
        /// </summary>
        public int price = 25000;
    }

    /// <summary>
    /// Array of all cars.
    /// </summary>
    public Cars[] cars;

}
