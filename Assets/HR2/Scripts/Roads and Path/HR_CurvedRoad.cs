//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the curved roads in the game.
/// </summary>
public class HR_CurvedRoad : MonoBehaviour {

    /// <summary>
    /// Array of all bones to be controlled.
    /// </summary>
    public Transform[] bones;

    /// <summary>
    /// Determines if curves should be randomized.
    /// </summary>
    public bool useRandomizedCurves = true;

    /// <summary>
    /// Minimum angle for the randomized curves.
    /// </summary>
    [Min(0f)] public float minimumCurveAngle = 30f;

    /// <summary>
    /// Maximum angle for the randomized curves.
    /// </summary>
    [Min(0f)] public float maximumCurveAngle = 90f;

    /// <summary>
    /// The end point of the road.
    /// </summary>
    public Transform endPoint;

    /// <summary>
    /// The animation curve used for the road.
    /// </summary>
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(.5f, 1f), new Keyframe(1f, 0f));

    private float randomCurveInput = 1f;

    /// <summary>
    /// Vector3 representing the curve vector.
    /// </summary>
    public Vector3 curveVector = Vector3.one;

    private Vector3 randomVector = Vector3.one;

    /// <summary>
    /// Initial local positions of bones.
    /// </summary>
    private Vector3[] initialPositions;

    /// <summary>
    /// Initial local rotations of bones.
    /// </summary>
    private Quaternion[] initialRotations;

    /// <summary>
    /// Width of the road.
    /// </summary>
    [Min(1f)] public float roadWidth = 5.5f;

    /// <summary>
    /// Distance multiplier for waypoints of the road.
    /// </summary>
    [Min(1f)] public float waypointDistance = 1f;

    [System.Serializable]
    public class SkinnedColliders {

        public SkinnedMeshRenderer skinnedMeshRenderer;
        public MeshCollider meshCollider;
        [HideInInspector] public Mesh bakedMesh;

    }

    public SkinnedColliders[] skinnedColliders;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void Awake() {

        // Store initial local positions and rotations of bones
        initialPositions = new Vector3[bones.Length];
        initialRotations = new Quaternion[bones.Length];

        for (int i = 0; i < bones.Length; i++) {

            initialPositions[i] = bones[i].localPosition;
            initialRotations[i] = bones[i].localRotation;

        }

    }

    /// <summary>
    /// Randomizes the curve.
    /// </summary>
    public void RandomizeCurve() {

        // Randomize a Vector3 within a specific range for each component
        randomVector = new Vector3(

            Random.Range(-curveVector.x, curveVector.x),
            Random.Range(-curveVector.y, curveVector.y),
            Random.Range(-curveVector.z, curveVector.z)

        );

        randomCurveInput = Random.Range(minimumCurveAngle, maximumCurveAngle);

        int random = Random.Range(0, 2);

        if (random == 1)
            randomCurveInput *= -1f;

        UpdateEverything();

    }

    /// <summary>
    /// Updates all bones and colliders.
    /// </summary>
    public void UpdateEverything() {

        // Apply the offset and rotation change to all bones
        for (int i = 0; i < bones.Length; i++) {

            bones[i].localPosition = initialPositions[i];
            bones[i].localRotation = initialRotations[i];

            bones[i].localPosition += randomVector * randomCurveInput * curve.Evaluate(i / ((float)bones.Length - 1));

            if (i > 0) {

                bones[i].LookAt(bones[i - 1]);
                bones[i].Rotate(Vector3.up, 180f);

            }

        }

        UpdateColliders();

    }

    /// <summary>
    /// Updates the colliders of the road.
    /// </summary>
    private void UpdateColliders() {

        for (int i = 0; i < skinnedColliders.Length; i++) {

            skinnedColliders[i].bakedMesh = new Mesh();
            skinnedColliders[i].skinnedMeshRenderer.BakeMesh(skinnedColliders[i].bakedMesh);
            skinnedColliders[i].meshCollider.sharedMesh = null;
            skinnedColliders[i].meshCollider.sharedMesh = skinnedColliders[i].bakedMesh;

        }

    }

    /// <summary>
    /// Sorts the given path points by their Z position.
    /// </summary>
    /// <param name="pathPoints">The list of path points to sort.</param>
    /// <returns>The sorted list of path points.</returns>
    private List<Transform> SortByZPosition(List<Transform> pathPoints) {

        pathPoints = pathPoints.Where(item => item != null).ToList();
        pathPoints.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        return pathPoints;

    }

    /// <summary>
    /// Sets the end position of the road.
    /// </summary>
    [ContextMenu("Set EndPosition")]
    public void SetEndPosition() {

        endPoint = transform.Find("EndPoint");

        if (!endPoint) {

            endPoint = new GameObject("EndPoint").transform;
            endPoint.SetParent(transform, false);

        }

        endPoint.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Vector3 bounds = HR_GetBounds.GetBoundsSize(transform);
        endPoint.transform.position += Vector3.forward * bounds.z;

    }

    /// <summary>
    /// Finds and sets the bones for the road.
    /// </summary>
    [ContextMenu("Find Bones")]
    public void FindBones() {

        List<Transform> foundBones = new List<Transform>();

        foreach (Transform item in transform) {

            if (item.name.Contains("Bone"))
                foundBones.Add(item);

        }

        bones = foundBones.ToArray();

    }

    /// <summary>
    /// Checks and sorts the order of the bones.
    /// </summary>
    [ContextMenu("Check Bones Order")]
    public void CheckBonesOrder() {

        List<Transform> bonesOrder = bones.ToList();
        bonesOrder = SortByZPosition(bonesOrder);
        bones = bonesOrder.ToArray();

    }

    /// <summary>
    /// Draws gizmos in the editor for visualization.
    /// </summary>
    private void OnDrawGizmos() {

        Gizmos.color = Color.magenta;

        if (endPoint)
            Gizmos.DrawSphere(endPoint.position, .6f);

        for (int i = 0; i < bones.Length; i++) {

            Gizmos.color = Color.green;

            if (bones[i])
                Gizmos.DrawSphere(bones[i].position, .6f);

            if (i < bones.Length - 1 && bones[i] && bones[i + 1])
                Gizmos.DrawLine(bones[i].position, bones[i + 1].position);

        }

    }

    /// <summary>
    /// Resets the end position of the road.
    /// </summary>
    private void Reset() {

        SetEndPosition();

    }

}
