//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_BlurredRoadShader : MonoBehaviour {

    private HR_GamePlayManager gameplayManager;
    private HR_GamePlayManager GameplayManager {

        get {

            if (gameplayManager == null)
                gameplayManager = HR_GamePlayManager.Instance;

            return gameplayManager;

        }

    }

    /// <summary>
    /// The material attached to the MeshRenderer component of this object.
    /// </summary>
    private Material material;

    /// <summary>
    /// The ID of the "_BlurIntensity" property in the shader.
    /// </summary>
    private int nameID = 0;

    // Start is called before the first frame update
    private void Start() {

        //   Get the material.
        material = GetComponent<SkinnedMeshRenderer>().material;

        // Get the shader property ID for "_BlurIntensity"
        nameID = Shader.PropertyToID("_BlurIntensity");

    }

    // Update is called once per frame
    private void Update() {

        if (!GameplayManager)
            return;

        HR_Player player = GameplayManager.player;

        if (!player)
            return;

        float speed = player.speed;
        float intensity = Mathf.Lerp(0f, 1f, speed / 160f);

        material.SetFloat(nameID, intensity);

    }

}
