//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the camera functionalities for the game.
/// </summary>
public class HR_Camera : MonoBehaviour {

    /// <summary>
    /// The target the camera should follow.
    /// </summary>
    public HR_Player player;

    /// <summary>
    /// Actual camera component.
    /// </summary>
    private Camera actualCamera;

    /// <summary>
    /// Enum for different camera modes.
    /// </summary>
    public enum CameraMode { Top, TPS, TPS_Fixed }

    [Space()]
    public CameraMode cameraMode = CameraMode.Top;

    /// <summary>
    /// Camera mode index.
    /// </summary>
    public int cameraModeIndex {

        get {

            switch (cameraMode) {

                case CameraMode.Top:
                    return 0;

                case CameraMode.TPS:
                    return 1;

                case CameraMode.TPS_Fixed:
                    return 2;

            }

            return 0;

        }

    }

    /// <summary>
    /// // The height from the target to the camera
    /// </summary>
    public float height_Top = 2.5f;

    /// <summary>
    /// The distance from the target to the camera
    /// </summary>
    public float distance_Top = 8.5f;
    public Quaternion rotation_Top = Quaternion.identity;

    /// <summary>
    /// The height from the target to the camera
    /// </summary>
    [Space()] public float height_TPS = 2.5f;

    /// <summary>
    /// The distance from the target to the camera
    /// </summary>
    public float distance_TPS = 8.5f;
    public Quaternion rotation_TPS = Quaternion.identity;

    /// <summary>
    /// Speed of rotation
    /// </summary>
    [Space()] public float rotationSpeed = 2f;

    /// <summary>
    /// Tilt the camera related to curve angle of the road.
    /// </summary>
    [Space()] public bool tilt = true;
    public float tiltMultiplier = 4f;

    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    private void Awake() {

        actualCamera = GetComponentInChildren<Camera>();

    }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void OnEnable() {

        HR_Events.OnPlayerSpawned += HR_GamePlayHandler_OnPlayerSpawned;
        RCCP_InputManager.OnChangedCamera += ChangeCameraMode;

    }

    /// <summary>
    /// Changes the camera mode.
    /// </summary>
    public void ChangeCameraMode() {

        switch (cameraModeIndex) {

            case 0:

                cameraMode = CameraMode.TPS;
                break;

            case 1:

                cameraMode = CameraMode.TPS_Fixed;
                break;

            case 2:

                cameraMode = CameraMode.Top;
                break;

        }

    }

    /// <summary>
    /// Called when the script instance is being disabled.
    /// </summary>
    private void OnDisable() {

        HR_Events.OnPlayerSpawned -= HR_GamePlayHandler_OnPlayerSpawned;
        RCCP_InputManager.OnChangedCamera -= ChangeCameraMode;

    }

    /// <summary>
    /// Called when the player is spawned.
    /// </summary>
    /// <param name="spawnedPlayer">The spawned player.</param>
    private void HR_GamePlayHandler_OnPlayerSpawned(HR_Player spawnedPlayer) {

        player = spawnedPlayer;

    }

    /// <summary>
    /// Called once per frame, after all Update functions have been called.
    /// </summary>
    private void LateUpdate() {

        if (player == null)
            return;

        if (player.crashed) {

            CrashCamera();
            return;

        }

        // Find the closest point on the path to the target
        Vector3 closestPoint = HR_PathManager.Instance.FindClosestPointOnPath(player.transform.position, out Vector3 pathDirection);

        if (closestPoint == Vector3.zero)
            return;

        switch (cameraMode) {

            case CameraMode.Top:

                // Set the camera's position to the closest point on the path
                targetPosition = closestPoint - pathDirection.normalized * distance_Top;
                targetPosition.y = player.transform.position.y + height_Top;

                // Calculate the target rotation based on the path direction
                targetRotation = Quaternion.LookRotation(pathDirection) * rotation_Top;

                break;

            case CameraMode.TPS:

                // Set the camera's position to the closest point on the path
                targetPosition = closestPoint - pathDirection.normalized * distance_TPS;
                targetPosition.y = player.transform.position.y + height_TPS;

                // Calculate the target rotation based on the path direction
                targetRotation = Quaternion.LookRotation(pathDirection) * rotation_TPS;

                break;

            case CameraMode.TPS_Fixed:

                // Set the camera's position to the closest point on the path
                targetPosition = closestPoint - pathDirection.normalized * distance_TPS;
                targetPosition.y = player.transform.position.y + height_TPS;
                targetPosition.x = player.transform.position.x;

                // Calculate the target rotation based on the path direction
                targetRotation = Quaternion.LookRotation(pathDirection) * rotation_TPS;

                break;

        }

        // Convert the rotations to forward vectors
        Vector3 forwardA = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
        Vector3 forwardB = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f) * Vector3.forward;

        // Calculate the signed angle around the up axis (Y axis)
        float signedAngle = Vector3.SignedAngle(forwardA, forwardB, Vector3.up);

        if (!tilt)
            signedAngle = 0f;

        Quaternion tiltAngle = Quaternion.LookRotation(Vector3.forward) * Quaternion.Euler(0f, 0f, -signedAngle * tiltMultiplier);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
        transform.position = new Vector3(transform.position.x, targetPosition.y, targetPosition.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * tiltAngle, rotationSpeed * Time.deltaTime);

    }

    private void CrashCamera() {

        transform.LookAt(player.transform);
        transform.Rotate(Vector3.forward, -10f);

        float distance = Vector3.Distance(transform.position, player.transform.position);

        actualCamera.fieldOfView = Mathf.Lerp(65f, 5f, Mathf.InverseLerp(-50f, 50f, distance));

    }

}
