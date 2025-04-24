//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Provides extension methods for checking the bounds of transforms.
/// </summary>
public static class HR_BoundsExtension {

    /// <summary>
    /// Checks if the target bounds are within the specified bounds.
    /// </summary>
    /// <param name="t">The transform to check.</param>
    /// <param name="bounds">The bounds to check within.</param>
    /// <param name="target">The target bounds to check.</param>
    /// <returns>True if the target bounds are within the specified bounds, otherwise false.</returns>
    public static bool ContainBounds(Transform t, Bounds bounds, Bounds target) {

        if (bounds.Contains(target.ClosestPoint(t.position)))
            return true;

        return false;

    }

}
