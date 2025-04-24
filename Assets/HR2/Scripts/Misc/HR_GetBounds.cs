//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Provides utility functions to get the bounds of objects and their children, excluding particles and trails.
/// </summary>
public class HR_GetBounds : MonoBehaviour {

    /// <summary>
    /// Gets the center of the bounds of an object, including all child renderers, but excluding particles and trails.
    /// </summary>
    /// <param name="obj">The transform of the object.</param>
    /// <returns>The center of the bounds.</returns>
    public static Vector3 GetBoundsCenter(Transform obj) {

        var renderers = obj.GetComponentsInChildren<Renderer>();

        Bounds bounds = new Bounds();
        bool initBounds = false;
        foreach (Renderer r in renderers) {

            if (!((r is TrailRenderer) || (r is ParticleSystemRenderer))) {

                if (!initBounds) {

                    initBounds = true;
                    bounds = r.bounds;

                } else {

                    bounds.Encapsulate(r.bounds);

                }

            }

        }

        Vector3 center = bounds.center;
        return center;

    }

    /// <summary>
    /// Gets the size of the bounds of an object, including all child renderers, but excluding particles and trails.
    /// </summary>
    /// <param name="obj">The transform of the object.</param>
    /// <returns>The size of the bounds.</returns>
    public static Vector3 GetBoundsSize(Transform obj) {

        Vector3 size = Vector3.zero;

        // Get the Renderer component from the GameObject
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        // Initialize the combined bounds with the bounds of the first renderer
        Bounds combinedBounds = new Bounds();

        if (renderers != null && renderers.Length >= 1) {

            foreach (Renderer r in renderers) {

                if (!((r is TrailRenderer) || (r is ParticleSystemRenderer)))
                    combinedBounds.Encapsulate(r.bounds);

            }

        }

        size = combinedBounds.size;
        return size;

    }

    /// <summary>
    /// Gets the bounds of an object, including all child renderers, but excluding particles and trails.
    /// </summary>
    /// <param name="obj">The transform of the object.</param>
    /// <returns>The bounds of the object.</returns>
    public static Bounds GetBounds(Transform obj) {

        // Get the Renderer component from the GameObject
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        // Initialize the combined bounds with the bounds of the first renderer
        Bounds combinedBounds = new Bounds();

        if (renderers != null && renderers.Length >= 1) {

            foreach (Renderer r in renderers) {

                if (!((r is TrailRenderer) || (r is ParticleSystemRenderer) || (r.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))))
                    combinedBounds.Encapsulate(r.bounds);

            }

        }

        return combinedBounds;

    }

}
