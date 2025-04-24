//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Utility class to manually set the lightmaps of target renderers based on a reference object.
/// </summary>
public class HR_SetLightmapsManually {

    /// <summary>
    /// Aligns the lightmaps of the target object's renderers with those of the reference object's renderers.
    /// </summary>
    /// <param name="referenceMainGameObject">The reference GameObject.</param>
    /// <param name="targetMainGameObject">The target GameObject.</param>
    public static void AlignLightmaps(GameObject referenceMainGameObject, GameObject targetMainGameObject) {

        Renderer[] referenceRenderers;
        Renderer[] targetRenderers;

        referenceRenderers = referenceMainGameObject.GetComponentsInChildren<Renderer>();
        targetRenderers = targetMainGameObject.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < targetRenderers.Length; i++) {

            targetRenderers[i].lightmapIndex = referenceRenderers[i].lightmapIndex;
            targetRenderers[i].lightmapScaleOffset = referenceRenderers[i].lightmapScaleOffset;

        }

    }

}
