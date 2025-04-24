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
/// Manages the engine smoke effect based on the player's damage.
/// </summary>
public class HR_EngineSmoke : MonoBehaviour {

    /// <summary>
    /// Reference to the HR_Player script.
    /// </summary>
    private HR_Player playerHandler;

    /// <summary>
    /// Reference to the ParticleSystem component.
    /// </summary>
    private ParticleSystem particles;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Start() {

        playerHandler = GetComponentInParent<HR_Player>();
        particles = GetComponent<ParticleSystem>();

    }

    /// <summary>
    /// Called once per frame to update the emission state of the particle system based on the player's damage.
    /// </summary>
    private void Update() {

        if (!playerHandler || !particles)
            return;

        ParticleSystem.EmissionModule em = particles.emission;

        if (playerHandler.damage >= 60f) {

            if (!em.enabled)
                em.enabled = true;

        } else {

            if (em.enabled)
                em.enabled = false;

        }

    }
}
