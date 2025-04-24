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
/// Manages all the lanes for the path vehicles follow.
/// </summary>
[DefaultExecutionOrder(-10)]
public class HR_LaneManager : MonoBehaviour
{

    #region SINGLETON PATTERN
    private static HR_LaneManager instance;
    public static HR_LaneManager Instance
    {

        get
        {

            if (instance == null)
                instance = FindObjectOfType<HR_LaneManager>();

            return instance;

        }

    }
    #endregion

    /// <summary>
    /// Class representing a lane with its properties.
    /// </summary>
    [System.Serializable]
    public class Lane
    {
        /// <summary>
        /// The HR_Lane object representing the lane.
        /// </summary>
        public HR_Lane lane;

    }

    /// <summary>
    /// Array of lanes.
    /// </summary>
    public Lane[] lanes;

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {

        HR_CurvedRoadManager.OnAllRoadsAligned += HR_CurvedRoadManager_OnAllRoadsAligned;
        HR_CurvedRoadManager.OnRoadAligned += HR_CurvedRoadManager_OnRoadAligned;

    }

    private void HR_CurvedRoadManager_OnRoadAligned(HR_CurvedRoad road)
    {

        UpdateWaypoints();

    }

    private void HR_CurvedRoadManager_OnAllRoadsAligned(List<HR_CurvedRoad> allRoads)
    {

        CreateWaypoints();

    }

    /// <summary>
    /// Called when the object becomes disabled.
    /// </summary>
    private void OnDisable()
    {

        HR_CurvedRoadManager.OnAllRoadsAligned -= HR_CurvedRoadManager_OnAllRoadsAligned;
        HR_CurvedRoadManager.OnRoadAligned -= HR_CurvedRoadManager_OnRoadAligned;

    }

    /// <summary>
    /// Finds and assigns all lanes in the children of this game object.
    /// </summary>
    [ContextMenu("Get All Lanes")]
    public void GetLanes()
    {

        HR_Lane[] foundLanes = GetComponentsInChildren<HR_Lane>(true);
        lanes = new Lane[foundLanes.Length];

        for (int i = 0; i < lanes.Length; i++)
        {

            lanes[i] = new Lane();
            lanes[i].lane = foundLanes[i];

        }

    }

    /// <summary>
    /// Creates waypoints for all lanes.
    /// </summary>
    public void CreateWaypoints()
    {

        HR_PathManager pathManager = HR_PathManager.Instance;
        List<Transform> pathPoints = pathManager.pathPoints;

        if (pathPoints == null || (pathPoints != null && pathPoints.Count < 1))
            return;

        for (int i = 0; i < lanes.Length; i++)
        {

            if (lanes[i] != null)
                lanes[i].lane.CreateWaypoints(pathPoints);

        }

    }

    /// <summary>
    /// Updates the waypoints for all lanes.
    /// </summary>
    public void UpdateWaypoints()
    {

        for (int i = 0; i < lanes.Length; i++)
        {

            if (lanes[i] != null)
                lanes[i].lane.UpdateWaypoints();

        }

    }

    /// <summary>
    /// Gets the index of the specified lane.
    /// </summary>
    /// <param name="lane">The lane to get the index of.</param>
    /// <returns>The index of the lane.</returns>
    public int GetLaneIndex(Lane lane)
    {

        for (int i = 0; i < lanes.Length; i++)
        {

            if (Equals(lane, lanes[i]))
                return i;

        }

        return 0;

    }

    public LaneSelectionResult GetAvailableLane(HR_Lane currentLane, Transform carTransform)
    {
        int laneIndex = -1;
        bool leftClear;
        bool rightClear;

        for (int i = 0; i < lanes.Length; i++)
        {
            if (lanes[i].lane == currentLane)
            {
                laneIndex = i;
                break;
            }
        }

        if (laneIndex == -1)
        {
            Debug.LogError("GetAvailableLane: Current lane not found!");
            return new LaneSelectionResult { lane = currentLane, direction = 0 };
        }

        switch (HR_GamePlayManager.Instance.mode)
        {
            case HR_GamePlayManager.Mode.OneWay:

                leftClear = laneIndex > 0 && !IsLaneOccupied(carTransform, -1);
                rightClear = laneIndex < lanes.Length - 1 && !IsLaneOccupied(carTransform, 1);

                if (laneIndex == 0 && rightClear)
                    return new LaneSelectionResult { lane = lanes[1].lane, direction = 1 };

                if (laneIndex == lanes.Length - 1 && leftClear)
                    return new LaneSelectionResult { lane = lanes[laneIndex - 1].lane, direction = -1 };

                if (leftClear && rightClear)
                {
                    int moveDirection = Random.Range(0, 2) == 0 ? -1 : 1;
                    return new LaneSelectionResult
                    {
                        lane = lanes[laneIndex + moveDirection].lane,
                        direction = moveDirection
                    };
                }

                if (leftClear)
                    return new LaneSelectionResult { lane = lanes[laneIndex - 1].lane, direction = -1 };

                if (rightClear)
                    return new LaneSelectionResult { lane = lanes[laneIndex + 1].lane, direction = 1 };

                break;

            case HR_GamePlayManager.Mode.TwoWay:
                leftClear = laneIndex == 3 && !IsLaneOccupied(carTransform, -1);
                rightClear = laneIndex == 2 && !IsLaneOccupied(carTransform, 1);

                if (leftClear)
                    return new LaneSelectionResult { lane = lanes[2].lane, direction = -1 };

                if (rightClear)
                    return new LaneSelectionResult { lane = lanes[3].lane, direction = 1 };

                break;
        }

        return new LaneSelectionResult { lane = currentLane, direction = 0 };
    }
    private bool IsLaneOccupied(Transform carTransform, int direction)
    {
        Vector3 carSize = carTransform.GetComponent<BoxCollider>().size;
        Vector3 rayOrigin = carTransform.position;
        Vector3 rayDirection = carTransform.right * direction;

        Vector3 boxHalfExtents = new Vector3(1.3f, 0.5f, carSize.z + 6); // Dar ama uzun kutu (sağ-sol kontrolü için)
        float rayLength = 3f;

        // Burada rotasyonu doğru ayarlıyoruz:
        Quaternion boxRotation = carTransform.rotation;

        RaycastHit hit;
        bool isOccupied = Physics.BoxCast(rayOrigin, boxHalfExtents, rayDirection, out hit, boxRotation, rayLength, LayerMask.GetMask(Settings.TRAFFIC_CAR_LAYER));

        Debug.DrawRay(rayOrigin, rayDirection * rayLength, isOccupied ? Color.green : Color.red, 0.5f);

        return isOccupied;
    }




    /// <summary>
    /// Finds the closest point on the specified lane to the target position.
    /// </summary>
    /// <param name="lane">The lane to search.</param>
    /// <param name="targetPosition">The target position.</param>
    /// <param name="pathDirection">The direction of the path.</param>
    /// <returns>The closest point on the lane.</returns>
    public Vector3 FindClosestPointOnLane(HR_Lane lane, Vector3 targetPosition, out Vector3 pathDirection)
    {

        HR_Lane foundLane = null;

        for (int i = 0; i < lanes.Length; i++)
        {

            if (Equals(lanes[i].lane, lane))
            {

                foundLane = lanes[i].lane;
                break;

            }

        }

        if (!foundLane)
        {

            pathDirection = Vector3.forward;
            return Vector3.zero;

        }

        List<Transform> pathPoints = foundLane.points;

        if (pathPoints == null || (pathPoints != null && pathPoints.Count < 2))
        {

            pathDirection = Vector3.forward;
            return Vector3.zero;

        }

        Vector3 closestPoint = pathPoints[0].position;

        pathDirection = pathPoints[1].position - pathPoints[0].position; // Initial direction

        float minDistance = Vector3.Distance(targetPosition, closestPoint);

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {

            Vector3 pointA = pathPoints[i].position;
            Vector3 pointB = pathPoints[i + 1].position;
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
    public Vector3 ProjectPointOnLineSegment(Vector3 pointA, Vector3 pointB, Vector3 point)
    {

        Vector3 AB = pointB - pointA;
        float t = Vector3.Dot(point - pointA, AB) / Vector3.Dot(AB, AB);
        t = Mathf.Clamp01(t);
        return pointA + t * AB;

    }

    /// <summary>
    /// Finds the lane by given point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public HR_Lane FindLaneByPoint(Transform point)
    {

        HR_Lane closestPoint = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < lanes.Length; i++)
        {

            List<Transform> allPoints = lanes[i].lane.points;

            if (allPoints != null && allPoints.Count >= 2)
            {

                for (int k = 0; k < allPoints.Count; k++)
                {

                    if (Vector3.Distance(allPoints[k].position, point.position) < closestDistance)
                    {

                        closestDistance = Vector3.Distance(allPoints[k].position, point.position);
                        closestPoint = lanes[i].lane;

                    }

                }

            }

        }

        return closestPoint;

    }

    /// <summary>
    /// Resets the lanes.
    /// </summary>
    private void Reset()
    {

        HR_Lane[] allLanes = GetComponentsInChildren<HR_Lane>(true);

        for (int i = 0; i < allLanes.Length; i++)
            DestroyImmediate(allLanes[i].gameObject);

        lanes = new Lane[4];

        for (int i = 0; i < lanes.Length; i++)
        {

            HR_Lane newLane = new GameObject("Lane_" + (i + 1).ToString()).AddComponent<HR_Lane>();
            newLane.transform.SetParent(transform);
            newLane.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            float xPos = Mathf.Lerp(-4.8f, 4.8f, (float)i / 3f);
            newLane.transform.position += Vector3.right * xPos;

            lanes[i] = new Lane();
            lanes[i].lane = newLane;

            if (newLane.transform.localPosition.x < 0)
                lanes[i].lane.leftSide = true;

        }

    }

}
public struct LaneSelectionResult
{
    public HR_Lane lane;
    public int direction; // -1 = Sol, +1 = Sağ, 0 = Aynı şerit
}

