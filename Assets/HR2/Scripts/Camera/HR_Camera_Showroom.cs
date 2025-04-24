//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Showroom camera used in the main menu.
/// </summary>
public class HR_Camera_Showroom : MonoBehaviour {

    /// <summary>
    /// Camera target, usually the spawn point.
    /// </summary>
    public Transform target;

    /// <summary>
    /// Z distance from the target.
    /// </summary>
    public float distance = 8f;

    [Space]

    /// <summary>
    /// Is auto orbiting enabled?
    /// </summary>
    public bool orbitingNow = true;

    /// <summary>
    /// Auto orbiting speed.
    /// </summary>
    public float orbitSpeed = 5f;

    /// <summary>
    /// Smoothing factor.
    /// </summary>
    public float smoothSpeed = 5f;

    [Space]

    /// <summary>
    /// Minimum Y degree.
    /// </summary>
    public float minY = 5f;

    /// <summary>
    /// Maximum Y degree.
    /// </summary>
    public float maxY = 35f;

    [Space]

    /// <summary>
    /// Drag speed.
    /// </summary>
    public float dragSpeed = 10f;

    /// <summary>
    /// Orbit X value.
    /// </summary>
    public float orbitX = 0f;

    /// <summary>
    /// Orbit Y value.
    /// </summary>
    public float orbitY = 0f;

    private void OnEnable() {

        // Calculate the desired rotation.
        Quaternion desiredRotation = Quaternion.Euler(orbitY, orbitX, 0);

        // Smoothly interpolate the camera's rotation.
        transform.rotation = desiredRotation;

    }

    /// <summary>
    /// Called after all Update functions have been called.
    /// </summary>
    private void LateUpdate() {

        // If there is no target, return.
        if (!target)
            return;

        // If auto orbiting is enabled, increase orbitX slowly with orbitSpeed factor.
        if (orbitingNow)
            orbitX += Time.deltaTime * orbitSpeed;

        // Clamping orbit Y.
        orbitY = ClampAngle(orbitY, minY, maxY);

        // Calculate the desired rotation.
        Quaternion desiredRotation = Quaternion.Euler(orbitY, orbitX, 0);

        // Smoothly interpolate the camera's rotation.
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * smoothSpeed);

        // Maintain a constant distance from the target.
        Vector3 direction = transform.rotation * Vector3.back;
        Vector3 desiredPosition = target.transform.position + direction * distance;

        // Set the camera's position directly to ensure constant distance.
        transform.position = desiredPosition;

    }

    /// <summary>
    /// Clamps the angle between the specified min and max values.
    /// </summary>
    /// <param name="angle">The angle to clamp.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The clamped angle.</returns>
    private float ClampAngle(float angle, float min, float max) {

        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);

    }

    /// <summary>
    /// Toggles the auto rotation state.
    /// </summary>
    /// <param name="state">The state to set.</param>
    public void ToggleAutoRotation(bool state) {

        orbitingNow = state;

    }

    /// <summary>
    /// Called when the object is being dragged.
    /// </summary>
    /// <param name="pointerData">The pointer data.</param>
    public void OnDrag(PointerEventData pointerData) {

        float x = pointerData.delta.x * dragSpeed * .04f;

        if (x > 10f)
            x = 10f;
        if (x < -10f)
            x = -10f;

        // Receiving drag input from UI.
        orbitX += x;
        orbitY -= pointerData.delta.y * dragSpeed * .04f;

    }

    public void Reset() {
        
        HR_MainMenuManager mainMenuManager = FindObjectOfType<HR_MainMenuManager>();

        if (!mainMenuManager)
            return;

        Transform spawnPoint = mainMenuManager.carSpawnLocation;

        if (!spawnPoint)
            return;

        target = spawnPoint;

    }

}
