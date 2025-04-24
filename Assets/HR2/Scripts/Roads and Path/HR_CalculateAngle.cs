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
/// Calculates forward steering angle.
/// </summary>
public class HR_CalculateAngle {

    /// <summary>
    /// Calculates the angle difference between two forward directions.
    /// </summary>
    /// <param name="transformA">The first transform's rotation.</param>
    /// <param name="transformB">The second transform's rotation.</param>
    /// <param name="steer">The steering factor.</param>
    /// <returns>The angle difference divided by the steering factor.</returns>
    public static float CalculateAngle(Quaternion transformA, Quaternion transformB, float steer) {

        var forwardA = transformA * Vector3.forward;
        var forwardB = transformB * Vector3.forward;

        float angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;

        var angleDiff = Mathf.DeltaAngle(angleA, angleB) / steer;

        return angleDiff;

    }

}
