//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Traffic car controller.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HR_TrafficCar : MonoBehaviour {

    /// <summary>
    /// Reference to the lane manager.
    /// </summary>
    private HR_TrafficManager trafficManager;
    public HR_TrafficManager TrafficManager {

        get {

            if (trafficManager == null)
                trafficManager = HR_TrafficManager.Instance;

            return trafficManager;

        }

    }

    /// <summary>
    /// Property to get the Rigidbody component.
    /// </summary>
    private Rigidbody Rigid {
        get {
            if (rigid == null)
                rigid = GetComponent<Rigidbody>();

            return rigid;
        }
    }
    private Rigidbody rigid;

    /// <summary>
    /// Spawn height of the car.
    /// </summary>
    public float spawnHeight = 0f;

    /// <summary>
    /// Trigger for detecting collisions.
    /// </summary>
    [HideInInspector]
    public BoxCollider triggerCollider;

    /// <summary>
    /// Has the car crashed?
    /// </summary>
    [HideInInspector]
    public bool crashed = false;

    /// <summary>
    /// Enum representing the direction of changing lanes.
    /// </summary>
    public enum ChangingLines { Straight, Right, Left }

    /// <summary>
    /// Current direction of lane change.
    /// </summary>
    [HideInInspector]
    public ChangingLines changingLines = ChangingLines.Straight;

    /// <summary>
    /// Current lane of the car.
    /// </summary>
    public HR_Lane currentLane;

    /// <summary>
    /// Is the car driving in the opposite direction?
    /// </summary>
    public bool oppositeDirection = false;

    /// <summary>
    /// Maximum speed of the car.
    /// </summary>
    public float maximumSpeed = 10f;

    private float _maximumSpeed = 10f;

    /// <summary>
    /// Desired speed of the car.
    /// </summary>
    private float desiredSpeed;

    /// <summary>
    /// Distance to the next car.
    /// </summary>
    private float distance = 0f;

    /// <summary>
    /// Steering angle of the car.
    /// </summary>
    private Quaternion steeringAngle = Quaternion.identity;

    /// <summary>
    /// Path direction of the car.
    /// </summary>
    private Vector3 pathDirection = Vector3.forward;

    [Space(10)]
    /// <summary>
    /// Wheel models of the car.
    /// </summary>
    public Transform[] wheelModels;

    /// <summary>
    /// Wheel rotation angle.
    /// </summary>
    private float wheelRotation = 0f;

    private bool headlightsOn = false;
    private bool brakingOn = false;

    /// <summary>
    /// Enum representing the state of the signals.
    /// </summary>
    private enum SignalsOn { Off, Right, Left, All }

    /// <summary>
    /// Current state of the signals.
    /// </summary>
    private SignalsOn signalsOn = SignalsOn.Off;

    private float signalTimer = 0f;
    private float spawnProtection = 0f;

    [Space(10)]
    /// <summary>
    /// Layer mask for collision detection.
    /// </summary>
    public LayerMask collisionLayer = 1;

    /// <summary>
    /// Layer mask for other detections.
    /// </summary>
    public LayerMask detectionLayer = 1;

    [Space(10)]
    /// <summary>
    /// Engine sound clip.
    /// </summary>
    public AudioClip engineSound;

    private AudioSource engineSoundSource;

    [Space(10)]
    /// <summary>
    /// Array of headlights.
    /// </summary>
    public Light[] headLights;

    /// <summary>
    /// Array of brake lights.
    /// </summary>
    public Light[] brakeLights;

    /// <summary>
    /// Array of signal lights.
    /// </summary>
    public Light[] signalLights;

    private Vector3 closestWaypoint = Vector3.zero;

    private float interval = .25f;
    private float nextTime = 0f;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        // Getting all lights and setting them to vertex mode.
        Light[] allLights = GetComponentsInChildren<Light>();

        foreach (Light l in allLights)
            l.renderMode = LightRenderMode.ForceVertex;

        distance = 20f;

        CreateTriggerVolume();

        // Creating engine sound.
        engineSoundSource = HR_CreateAudioSource.NewAudioSource(gameObject, "Engine Sound", 2f, 5f, 1f, engineSound, true, true, false);
        engineSoundSource.pitch = 1.5f;

        _maximumSpeed = maximumSpeed;

        // Setting layer of the children gameobjects.
        foreach (Transform t in transform) {

            if (t.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
                t.gameObject.layer = LayerMask.NameToLayer(HR_Settings.Instance.trafficCarsLayer);

        }

    }

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    private void Start() {

        // Changing lines randomly.
        InvokeRepeating(nameof(ChangeLines), Random.Range(15, 45), Random.Range(15, 45));

    }

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    public void OnEnable() {

        Rigid.linearVelocity = Vector3.zero;
        Rigid.angularVelocity = Vector3.zero;
        Rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

        crashed = false;
        spawnProtection = 0f;
        maximumSpeed = Random.Range(_maximumSpeed * .85f, _maximumSpeed);
        distance = 20f;
        steeringAngle = transform.rotation;

        oppositeDirection = Vector3.Dot(transform.forward, Vector3.forward) < 0;
        GetClosestPointOnPath();

        signalsOn = SignalsOn.Off;
        changingLines = ChangingLines.Straight;

        // Enabling headlights if it's night.
        headlightsOn = TrafficManager.GameplayManager != null && (TrafficManager.GameplayManager.dayOrNight == HR_GamePlayManager.DayOrNight.Night);

    }

    /// <summary>
    /// Creates a trigger volume for the car.
    /// </summary>
    private void CreateTriggerVolume() {

        Bounds bounds = HR_GetBounds.GetBounds(transform);

        // Creating trigger for detecting front vehicles.
        GameObject triggerColliderGO = new GameObject("HR_TriggerVolume");
        triggerColliderGO.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        triggerColliderGO.transform.position = bounds.center;
        triggerColliderGO.transform.rotation = transform.rotation;
        triggerColliderGO.transform.SetParent(transform, true);
        triggerColliderGO.transform.localScale = transform.localScale;

        BoxCollider boxCollider = triggerColliderGO.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = bounds.size;
        boxCollider.center = bounds.center;

        triggerCollider = triggerColliderGO.GetComponent<BoxCollider>();
        triggerCollider.size = new Vector3(bounds.size.x * 1.05f, bounds.size.y, bounds.size.z + (bounds.size.z * 3f));
        triggerCollider.center = new Vector3(bounds.center.x, 0f, bounds.center.z + (triggerCollider.size.z / 2f) - (bounds.size.z / 3f));

    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update() {

        // Spawn protection to prevent crashes too soon.
        spawnProtection += Time.deltaTime;

        if (spawnProtection > 1f)
            spawnProtection = 1f;

        Lights();
        Wheels();

    }

    private void GetClosestPointOnPath() {

        closestWaypoint = TrafficManager.LaneManager.FindClosestPointOnLane(currentLane, transform.position + (transform.forward * 1f), out pathDirection);

        if (oppositeDirection)
            pathDirection = -pathDirection;

    }

    /// <summary>
    /// Handles the car's navigation on the road.
    /// </summary>
    private void Navigation() {

        if (!TrafficManager.LaneManager) {

            changingLines = ChangingLines.Straight;
            desiredSpeed = 0f;
            brakingOn = true;
            pathDirection = Vector3.forward;
            return;

        }

        if (Time.time >= nextTime) {

            nextTime += interval;
            GetClosestPointOnPath();

        }

        if (closestWaypoint == Vector3.zero) {

            changingLines = ChangingLines.Straight;
            desiredSpeed = 0f;
            brakingOn = true;
            pathDirection = Vector3.forward;
            return;

        }

        Quaternion targetRotation = Quaternion.LookRotation(pathDirection);
        steeringAngle = Quaternion.Slerp(steeringAngle, targetRotation, Time.fixedDeltaTime * 5f);
        //steeringAngle = targetRotation;

        // Adjusting desired speed according to distance to the next car.
        desiredSpeed = crashed ? Mathf.Lerp(desiredSpeed, 0f, Time.fixedDeltaTime) : Mathf.InverseLerp(10f, 30f, distance) * maximumSpeed;

        brakingOn = distance < 20;

        if (!crashed)
            transform.rotation = steeringAngle;

        // Setting linear and angular velocity of the car.
        Rigid.linearVelocity = Vector3.Slerp(Rigid.linearVelocity, transform.forward * (desiredSpeed / 3.6f), Time.fixedDeltaTime * 3f);
        Rigid.angularVelocity = Vector3.Slerp(Rigid.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);

        if (!crashed) {

            Vector3 offsetDirection = new Vector3(closestWaypoint.x, 0f, 0f) - (new Vector3(transform.position.x, 0f, 0f));
            //offsetDirection = offsetDirection.normalized;

            if (offsetDirection.x < -1f)
                offsetDirection.x = -1f;
            if (offsetDirection.x > 1f)
                offsetDirection.x = 1f;

            offsetDirection.y = 0f;
            offsetDirection.z = 0f;

            changingLines = offsetDirection.magnitude > .35f ? (offsetDirection.x > 0f ? ChangingLines.Right : ChangingLines.Left) : ChangingLines.Straight;

            //if (offsetDirection.magnitude > .5f)
                Rigid.AddForce(offsetDirection * 0.005f * Mathf.Clamp(Rigid.linearVelocity.magnitude, 0f, 20f), ForceMode.VelocityChange);

        }

    }

    /// <summary>
    /// Checks if the car is facing backward.
    /// </summary>
    /// <returns>True if facing backward, false otherwise.</returns>
    private bool IsFacingBackward() {

        float dotProduct = Vector3.Dot(transform.forward, Vector3.zero);
        return dotProduct < 0;

    }

    /// <summary>
    /// Handles the car's lights.
    /// </summary>
    private void Lights() {

        if (!crashed) {

            signalsOn = SignalsOn.Off;

            if (changingLines == ChangingLines.Right)
                signalsOn = SignalsOn.Right;

            if (changingLines == ChangingLines.Left)
                signalsOn = SignalsOn.Left;

        } else {

            signalsOn = SignalsOn.All;

        }

        signalTimer += Time.deltaTime;

        for (int i = 0; i < signalLights.Length; i++) {

            signalLights[i].intensity = signalsOn switch {
                SignalsOn.Off => 0f,
                SignalsOn.Left when signalLights[i].transform.localPosition.x < 0f => signalTimer >= .5f ? 0f : 1f,
                SignalsOn.Right when signalLights[i].transform.localPosition.x > 0f => signalTimer >= .5f ? 0f : 1f,
                SignalsOn.All => signalTimer >= .5f ? 0f : 1f,
                _ => signalLights[i].intensity
            };

            if (signalTimer >= 1f)
                signalTimer = 0f;

        }

        for (int i = 0; i < headLights.Length; i++)
            headLights[i].intensity = headlightsOn ? 1f : 0f;

        for (int i = 0; i < brakeLights.Length; i++)
            brakeLights[i].intensity = brakingOn ? 1f : (headlightsOn ? .6f : 0f);

    }

    /// <summary>
    /// Handles the rotation of the wheels.
    /// </summary>
    private void Wheels() {

        for (int i = 0; i < wheelModels.Length; i++) {

            wheelRotation += desiredSpeed * 20 * Time.deltaTime;
            wheelModels[i].transform.localRotation = Quaternion.Euler(wheelRotation, 0f, 0f);

        }

    }

    /// <summary>
    /// Called every fixed framerate frame.
    /// </summary>
    private void FixedUpdate() {

        Navigation();

    }

    /// <summary>
    /// Called when another collider stays in this trigger.
    /// </summary>
    /// <param name="col">The other collider involved in this collision.</param>
    private void OnTriggerStay(Collider col) {

        if ((detectionLayer.value & (1 << col.transform.gameObject.layer)) > 0) {

            if (!IsFacingBackward(col.transform))
                distance = Vector3.Distance(transform.position, col.transform.position);

        }

    }

    /// <summary>
    /// Called when another collider exits this trigger.
    /// </summary>
    /// <param name="col">The other collider involved in this collision.</param>
    private void OnTriggerExit(Collider col) {

        if ((detectionLayer.value & (1 << col.transform.gameObject.layer)) > 0)
            distance = 20f;

    }

    /// <summary>
    /// Called when a collision occurs.
    /// </summary>
    /// <param name="col">The collision data associated with this collision.</param>
    private void OnCollisionEnter(Collision col) {

        if ((collisionLayer.value & (1 << col.transform.gameObject.layer)) > 0) {

            if (col.impulse.magnitude < 1000f || crashed || spawnProtection < .5f)
                return;

            crashed = true;
            signalsOn = SignalsOn.All;

        }

    }

    /// <summary>
    /// Checks if the target transform is facing backward.
    /// </summary>
    /// <param name="target">The target transform.</param>
    /// <returns>True if facing backward, false otherwise.</returns>
    private bool IsFacingBackward(Transform target) {

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToTarget);
        return dotProduct < 0;

    }

    /// <summary>
    /// Changes the lane of the car.
    /// </summary>
    private void ChangeLines() {

        if (changingLines == ChangingLines.Left || changingLines == ChangingLines.Right)
            return;

        if (!HR_LaneManager.Instance)
            return;

        int maxLanes = HR_LaneManager.Instance.lanes.Length;
        int randomNumber = Random.Range(0, maxLanes);

        while (HR_LaneManager.Instance.lanes[randomNumber].lane.leftSide != oppositeDirection)
            randomNumber = Random.Range(0, maxLanes);

        currentLane = HR_LaneManager.Instance.lanes[randomNumber].lane;

    }

    /// <summary>
    /// Sets the Rigidbody settings for the car.
    /// </summary>
    private void SetRigidbodySettings() {

        Rigidbody rigidbody = Rigid;

        if (!rigidbody)
            gameObject.AddComponent<Rigidbody>();

        Rigid.interpolation = RigidbodyInterpolation.Interpolate;
        Rigid.mass = 1500f;
        Rigid.linearDamping = 1f;
        Rigid.angularDamping = 4f;
        Rigid.maxAngularVelocity = 2.5f;
        Rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

    }

    /// <summary>
    /// Resets the Rigidbody settings for the car.
    /// </summary>
    private void Reset() {

        SetRigidbodySettings();

    }

}
