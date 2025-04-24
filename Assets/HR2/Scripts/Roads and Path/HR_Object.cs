//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents an object in the game that can find and set the closest point on a path.
/// </summary>
public class HR_Object : MonoBehaviour {

    /// <summary>
    /// Target bone.
    /// </summary>
    public Transform targetBone;

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable() {

        if (targetBone != null) {

            // Store the original scale
            Vector3 originalScale = transform.localScale;

            if (transform.parent != targetBone)
                transform.SetParent(targetBone, true);

            // Restore the original scale
            //transform.localScale = originalScale;

        }

    }

    /// <summary>
    /// Finds the closest point on the path and sets it as the target point.
    /// </summary>
    [ContextMenu("Find Closest Point On Path")]
    public void FindClosest() {

        targetBone = FindClosestPointOnPathWithTransform(transform.position, out Vector3 direction);
        OnEnable();

    }

    /// <summary>
    /// Finds the closest point on the path to the target position.
    /// </summary>
    /// <param name="targetPosition">The target position.</param>
    /// <param name="pathDirection">The direction of the path.</param>
    /// <returns>The closest point on the path.</returns>
    public Transform FindClosestPointOnPathWithTransform(Vector3 targetPosition, out Vector3 pathDirection) {

        HR_CurvedRoad[] roads = FindObjectsOfType<HR_CurvedRoad>(false);
        List<Transform> points = new List<Transform>();

        for (int i = 0; i < roads.Length; i++) {

            for (int k = 0; k < roads[i].bones.Length; k++)
                points.Add(roads[i].bones[k]);

        }

        if (points == null || (points != null && points.Count < 2)) {

            pathDirection = Vector3.forward;
            return null;

        }

        points = SortByZPosition(points);

        Transform closestPoint = points[0];

        pathDirection = points[1].position - points[0].position; // Initial direction

        float minDistance = Vector3.Distance(targetPosition, closestPoint.position);

        for (int i = 0; i < points.Count - 1; i++) {

            Vector3 pointA = points[i].position;
            Vector3 pointB = points[i + 1].position;
            Vector3 projectedPoint = ProjectPointOnLineSegment(pointA, pointB, targetPosition);

            float distance = Vector3.Distance(targetPosition, projectedPoint);

            if (distance < minDistance) {

                closestPoint = points[i];
                pathDirection = pointB - pointA; // Update direction based on the segment
                minDistance = distance;

            }

        }

        return closestPoint;

    }

    /// <summary>
    /// Projects a point onto a line segment.
    /// </summary>
    /// <param name="pointA">The start point of the line segment.</param>
    /// <param name="pointB">The end point of the line segment.</param>
    /// <param name="point">The point to project.</param>
    /// <returns>The projected point on the line segment.</returns>
    private Vector3 ProjectPointOnLineSegment(Vector3 pointA, Vector3 pointB, Vector3 point) {

        Vector3 AB = pointB - pointA;
        float t = Vector3.Dot(point - pointA, AB) / Vector3.Dot(AB, AB);
        t = Mathf.Clamp01(t);
        return pointA + t * AB;

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

}
