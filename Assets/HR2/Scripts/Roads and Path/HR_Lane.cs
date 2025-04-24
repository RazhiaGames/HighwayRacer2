//----------------------------------------------
//                   Highway Racer
//
// Copyright � 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using AshVP;
using UnityEngine;

/// <summary>
/// Manages lanes for the path the vehicles follow.
/// </summary>
public class HR_Lane : MonoBehaviour
{

    /// <summary>
    /// List of points that make up the lane.
    /// </summary>
    public List<Transform> points = new List<Transform>();

    /// <summary>
    /// Initial distance value.
    /// </summary>
    public float initialDistance = -1f;

    /// <summary>
    /// Indicates if the lane is on the left side.
    /// </summary>
    public bool leftSide = false;


    /// <summary>
    /// Updates the waypoints of the lane based on the path manager's path points.
    /// </summary>
    public void CreateWaypoints(List<Transform> pathPoints)
    {

        for (int i = 0; i < pathPoints.Count; i++)
        {

            if (pathPoints[i] != null)
            {

                if (initialDistance == -1)
                    initialDistance = transform.position.x;

                GameObject newWP = new GameObject("Waypoint_" + i.ToString());
                newWP.transform.SetParent(transform);
                newWP.transform.position = pathPoints[i].position;
                newWP.transform.rotation = pathPoints[i].rotation;
                Vector3 direction = Quaternion.Euler(newWP.transform.eulerAngles) * Vector3.right;
                newWP.transform.position += direction * initialDistance;
                points.Add(newWP.transform);

            }

        }

    }



    /// <summary>
    /// Creates waypoints for the lane based on the path manager's path points.
    /// </summary>
    public void UpdateWaypoints()
    {

        HR_PathManager pathManager = HR_PathManager.Instance;

        for (int i = 0; i < pathManager.pathPoints.Count; i++)
        {

            if (pathManager.pathPoints[i] != null)
            {

                points[i].position = pathManager.pathPoints[i].position;
                points[i].rotation = pathManager.pathPoints[i].rotation;
                Vector3 direction = Quaternion.Euler(points[i].transform.eulerAngles) * Vector3.right;
                points[i].transform.position += direction * initialDistance;

            }

        }

    }
  
    /// <summary>
    /// Finds the closest point on the lane to the target position.
    /// </summary>
    /// <param name="targetPosition">The target position.</param>
    /// <param name="pathDirection">The direction of the path.</param>
    /// <returns>The closest point on the lane.</returns>
    public Vector3 FindClosestPointOnPath(Vector3 targetPosition, out Vector3 pathDirection)
    {

        Vector3 closestPoint = points[0].position;

        pathDirection = points[1].position - points[0].position; // Initial direction

        float minDistance = Vector3.Distance(targetPosition, closestPoint);

        for (int i = 0; i < points.Count - 1; i++)
        {

            Vector3 pointA = points[i].position;
            Vector3 pointB = points[i + 1].position;
            Vector3 projectedPoint = ProjectPointOnLineSegment(pointA, pointB, targetPosition);

            float distance = Vector3.Distance(targetPosition, projectedPoint);

            if (distance < minDistance)
            {

                closestPoint = projectedPoint;
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
    private Vector3 ProjectPointOnLineSegment(Vector3 pointA, Vector3 pointB, Vector3 point)
    {

        Vector3 AB = pointB - pointA;
        float t = Vector3.Dot(point - pointA, AB) / Vector3.Dot(AB, AB);
        t = Mathf.Clamp01(t);
        return pointA + t * AB;

    }

    /// <summary>
    /// Draws gizmos in the editor for visualization.
    /// </summary>
    private void OnDrawGizmos()
    {

        for (int i = 0; i < points.Count; i++)
        {

            Color defColor = Gizmos.color;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(points[i].position, .4f);

            if (i != points.Count - 1)
                Gizmos.DrawLine(points[i].position, points[i + 1].position);

            Gizmos.color = defColor;

        }

    }

}