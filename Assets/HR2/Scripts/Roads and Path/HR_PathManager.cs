//----------------------------------------------
//                   Highway Racer
//
// Copyright � 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the path points and calculates the closest path points to the player.
/// </summary>
[DefaultExecutionOrder(-20)]
public class HR_PathManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_PathManager instance;
    public static HR_PathManager Instance {

        get {

            if (instance == null)
                instance = FindObjectOfType<HR_PathManager>();

            if (instance == null)
                instance = new GameObject("HR_PathManager").AddComponent<HR_PathManager>();

            return instance;

        }

    }
    #endregion

    /// <summary>
    /// Reference to the player.
    /// </summary>
    public HR_Player player;

    /// <summary>
    /// List of all path points.
    /// </summary>
    public List<Transform> pathPoints = new List<Transform>();

    /// <summary>
    /// List of closest path points to the player.
    /// </summary>
    public List<Transform> closestPathPointsToPlayer = new List<Transform>();

    /// <summary>
    /// Closest path point to the player.
    /// </summary>
    public Transform closestPathPointToPlayer;

    private float interval = .5f;
    private float nextTime = 0f;

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable() {

        HR_CurvedRoadManager.OnAllRoadsAligned += HR_CurvedRoadManager_OnAllRoadsAligned;
        HR_CurvedRoadManager.OnRoadAligned += HR_CurvedRoadManager_OnRoadAligned;
        HR_Events.OnPlayerSpawned += HR_GamePlayHandler_OnPlayerSpawned;

    }

    private void HR_CurvedRoadManager_OnRoadAligned(HR_CurvedRoad road) {

        ProcessPath();
    }

    private void HR_CurvedRoadManager_OnAllRoadsAligned(List<HR_CurvedRoad> allRoads) {

        for (int i = 0; i < allRoads.Count; i++)
            AddPath(allRoads[i]);

    }

    /// <summary>
    /// Called when the object becomes disabled.
    /// </summary>
    private void OnDisable() {

        HR_CurvedRoadManager.OnAllRoadsAligned -= HR_CurvedRoadManager_OnAllRoadsAligned;
        HR_CurvedRoadManager.OnRoadAligned -= HR_CurvedRoadManager_OnRoadAligned;
        HR_Events.OnPlayerSpawned -= HR_GamePlayHandler_OnPlayerSpawned;

    }

    /// <summary>
    /// Called once per frame to update the closest path point to the player.
    /// </summary>
    private void Update() {

        if (Time.time >= nextTime) {

            nextTime += interval;
            ProcessPath();

        }

    }

    public void ProcessPath() {

        if (pathPoints != null && pathPoints.Count > 2)
            CheckPathPoints();

        if (player)
            closestPathPointToPlayer = FindClosestPointOnPathWithTransform(player.transform.position, out Vector3 direction);

        transform.forward = GetPathAngle();

    }

    /// <summary>
    /// Adds the path points from the specified curved road to the path manager.
    /// </summary>
    /// <param name="curvedRoad">The curved road containing path points.</param>
    public void AddPath(HR_CurvedRoad curvedRoad) {

        for (int i = 0; i < curvedRoad.bones.Length; i++) {

            if (curvedRoad.bones[i] != null) {

                if (!pathPoints.Contains(curvedRoad.bones[i]))
                    pathPoints.Add(curvedRoad.bones[i]);

            }

        }

    }

    /// <summary>
    /// Gets the angle of the path based on the closest path points.
    /// </summary>
    /// <returns>The angle of the path.</returns>
    public Vector3 GetPathAngle()
    {
        Transform first = GetFirstClosestPoint();
        Transform middle = GetNextClosestPoint(first);

        if (!first || !middle)
            return Vector3.zero;

        return (middle.position - first.position).normalized;
    }

    
    private Transform GetNextClosestPoint(Transform current)
    {
        int index = closestPathPointsToPlayer.IndexOf(current);

        // Get the next in list if possible
        if (index >= 0 && index < closestPathPointsToPlayer.Count - 1)
            return closestPathPointsToPlayer[index + 4];

        return null;
    }
    /// <summary>
    /// Handles the player spawned event.
    /// </summary>
    /// <param name="spawnedPlayer">The spawned player.</param>
    private void HR_GamePlayHandler_OnPlayerSpawned(HR_Player spawnedPlayer) {

        player = spawnedPlayer;

    }

    /// <summary>
    /// Finds the closest point on the path to the target position.
    /// </summary>
    /// <param name="targetPosition">The target position.</param>
    /// <param name="pathDirection">The direction of the path.</param>
    /// <returns>The closest point on the path.</returns>
    public Vector3 FindClosestPointOnPath(Vector3 targetPosition, out Vector3 pathDirection) {

        if (pathPoints == null || (pathPoints != null && pathPoints.Count < 2)) {

            pathDirection = Vector3.forward;
            return Vector3.zero;

        }

        Vector3 closestPoint = pathPoints[0].position;

        pathDirection = pathPoints[1].position - pathPoints[0].position; // Initial direction

        float minDistance = Vector3.Distance(targetPosition, closestPoint);

        for (int i = 0; i < pathPoints.Count - 1; i++) {

            Vector3 pointA = pathPoints[i].position;
            Vector3 pointB = pathPoints[i + 1].position;
            Vector3 projectedPoint = ProjectPointOnLineSegment(pointA, pointB, targetPosition);

            float distance = Vector3.Distance(targetPosition, projectedPoint);

            if (distance < minDistance) {

                closestPoint = projectedPoint;
                pathDirection = pointB - pointA; // Update direction based on the segment
                minDistance = distance;

            }

        }

        return closestPoint;

    }

    /// <summary>
    /// Finds the closest point on the path to the target position and returns its transform.
    /// </summary>
    /// <param name="targetPosition">The target position.</param>
    /// <param name="pathDirection">The direction of the path.</param>
    /// <returns>The transform of the closest point on the path.</returns>
    public Transform FindClosestPointOnPathWithTransform(Vector3 targetPosition, out Vector3 pathDirection) {

        if (pathPoints == null || (pathPoints != null && pathPoints.Count < 2)) {

            pathDirection = Vector3.forward;
            return null;

        }

        Transform closestPoint = pathPoints[0];

        pathDirection = pathPoints[1].position - pathPoints[0].position; // Initial direction

        float minDistance = Vector3.Distance(targetPosition, closestPoint.position);

        for (int i = 0; i < pathPoints.Count - 1; i++) {

            Vector3 pointA = pathPoints[i].position;
            Vector3 pointB = pathPoints[i + 1].position;
            Vector3 projectedPoint = ProjectPointOnLineSegment(pointA, pointB, targetPosition);

            float distance = Vector3.Distance(targetPosition, projectedPoint);

            if (distance < minDistance) {

                closestPoint = pathPoints[i];
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
    public Vector3 ProjectPointOnLineSegment(Vector3 pointA, Vector3 pointB, Vector3 point) {

        Vector3 AB = pointB - pointA;
        float t = Vector3.Dot(point - pointA, AB) / Vector3.Dot(AB, AB);
        t = Mathf.Clamp01(t);
        return pointA + t * AB;

    }

    /// <summary>
    /// Gets the first closest point to the player.
    /// </summary>
    /// <returns>The first closest point.</returns>
    public Transform GetFirstClosestPoint() {

        Transform first = null;

        for (int i = 0; i < closestPathPointsToPlayer.Count; i++) {

            if (closestPathPointsToPlayer[i] != null) {

                first = closestPathPointsToPlayer[i];
                break;

            }

        }

        return first;

    }

    /// <summary>
    /// Gets the middle closest point to the specified target point.
    /// </summary>
    /// <param name="target">The target point.</param>
    /// <returns>The middle closest point.</returns>
    public Transform GetMiddleClosestPoint(Transform target) {

        Transform next = target;

        for (int i = 0; i < closestPathPointsToPlayer.Count; i++) {

            if (closestPathPointsToPlayer[i] != null) {

                if (Equals(closestPathPointsToPlayer[i], target)) {

                    next = closestPathPointsToPlayer[(int)(((closestPathPointsToPlayer.Count - 1) - i) / 3f)];
                    break;

                }

            }

        }

        return next;

    }

    /// <summary>
    /// Gets the last closest point to the player.
    /// </summary>
    /// <returns>The last closest point.</returns>
    public Transform GetLastClosestPoint() {

        Transform last = null;

        for (int i = closestPathPointsToPlayer.Count - 1; i > 0; i--) {

            if (closestPathPointsToPlayer[i] != null) {

                last = closestPathPointsToPlayer[i];
                break;

            }

        }

        return last;

    }

    /// <summary>
    /// Checks and updates the closest path points to the player.
    /// </summary>
    private void CheckPathPoints() {

        if (!player)
            return;

        for (int i = 0; i < pathPoints.Count; i++) {

            if (Vector3.Distance(pathPoints[i].position, player.transform.position) < 125f) {

                if (!closestPathPointsToPlayer.Contains(pathPoints[i]))
                    closestPathPointsToPlayer.Add(pathPoints[i]);

                if (IsInFront(player.gameObject, pathPoints[i].gameObject) && !closestPathPointsToPlayer.Contains(pathPoints[i]))
                    closestPathPointsToPlayer.Add(pathPoints[i]);

            } else {

                if (closestPathPointsToPlayer.Contains(pathPoints[i]))
                    closestPathPointsToPlayer.Remove(pathPoints[i]);

            }

            if (!IsInFront(player.gameObject, pathPoints[i].gameObject) && closestPathPointsToPlayer.Contains(pathPoints[i]))
                closestPathPointsToPlayer.Remove(pathPoints[i]);

        }

        //SortByZPosition();
    }

    /// <summary>
    /// Finds the road that contains the specified point.
    /// </summary>
    /// <param name="point">The point to search for.</param>
    /// <returns>The curved road containing the point.</returns>
    public HR_CurvedRoad FindRoadByTransform(Transform point) {

        HR_CurvedRoadManager curvedRoadsInstance = HR_CurvedRoadManager.Instance;

        for (int i = 0; i < curvedRoadsInstance.spawnedRoads.Count; i++) {

            if (point.IsChildOf(curvedRoadsInstance.spawnedRoads[i].transform))
                return curvedRoadsInstance.spawnedRoads[i];

        }

        return null;

    }

    /// <summary>
    /// Finds the road that contains the closest path point to the player.
    /// </summary>
    /// <returns>The curved road containing the closest path point to the player.</returns>
    public HR_CurvedRoad FindRoadByPlayer() {

        if (!closestPathPointToPlayer)
            return null;

        HR_CurvedRoadManager curvedRoadsInstance = HR_CurvedRoadManager.Instance;

        if (!curvedRoadsInstance)
            return null;

        if (curvedRoadsInstance.spawnedRoads == null || (curvedRoadsInstance.spawnedRoads != null && curvedRoadsInstance.spawnedRoads.Count < 1))
            return null;

        for (int i = 0; i < curvedRoadsInstance.spawnedRoads.Count; i++) {

            if (curvedRoadsInstance.spawnedRoads[i] != null) {

                if (closestPathPointToPlayer.IsChildOf(curvedRoadsInstance.spawnedRoads[i].transform))
                    return curvedRoadsInstance.spawnedRoads[i];

            }

        }

        return null;

    }

    /// <summary>
    /// Checks if the target object is in front of the other object.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="other">The other object.</param>
    /// <returns>True if the target is in front of the other object, false otherwise.</returns>
    private bool IsInFront(GameObject target, GameObject other) {

        if (Vector3.Distance(target.transform.position, other.transform.position) < 20f)
            return true;

        Vector3 directionToOther = (other.transform.position - target.transform.position).normalized;
        float dotProduct = Vector3.Dot(target.transform.forward, directionToOther);

        return dotProduct > 0;

    }

    /// <summary>
    /// Sorts the path points based on the dominant axis (Z or X).
    /// </summary>
    private void SortByPathDirection()
    {

        if (pathPoints == null || pathPoints.Count < 2)
            return;

        // İlk ve son waypoint arasındaki farkı al
        Vector3 firstPos = pathPoints[0].position;
        Vector3 lastPos = pathPoints[pathPoints.Count - 1].position;

        float deltaX = Mathf.Abs(lastPos.x - firstPos.x);
        float deltaZ = Mathf.Abs(lastPos.z - firstPos.z);

        // Yolun hangi eksende daha fazla ilerlediğini belirle
        if (deltaZ >= deltaX)
        {
            // Z ekseninde ilerleme daha fazla, Z eksenine göre sırala
            pathPoints.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        }
        else
        {
            // X ekseninde ilerleme daha fazla, X eksenine göre sırala
            pathPoints.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        }
    }
    /// <summary>
    /// Sorts the path points by their Z position.
    /// </summary>
    private void SortByZPosition()
    {

        pathPoints = pathPoints.Where(item => item != null).ToList();
        pathPoints.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));

    }

}
