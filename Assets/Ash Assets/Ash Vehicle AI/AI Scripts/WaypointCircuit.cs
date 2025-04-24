using System;
using System.Collections;
using System.Collections.Generic; // List kullanımı için ekledik
using UnityEngine;
using UnityEditor;

namespace AshVP
{
    public class WaypointCircuit : MonoBehaviour
    {
        public WaypointList waypointList = new WaypointList();
        [SerializeField] private bool smoothRoute = true;
        private int numPoints;
        private Vector3[] points;
        private float[] distances;

        [Range(100,500)]
        public float editorVisualisationSubsteps = 100;
        public float Length { get; private set; }

        public Transform[] Waypoints
        {
            get { return waypointList.items.ToArray(); } // List'ten array'e çevirdik
        }

        private int p0n, p1n, p2n, p3n;
        private float i;
        private Vector3 P0, P1, P2, P3;

        private void Awake()
        {
           
        }

        public void CreateNumPoints()
        {
            if (Waypoints.Length > 1)
            {
                CachePositionsAndDistances();
            }
            numPoints = Waypoints.Length;
        }

        public RoutePoint GetRoutePoint(float dist)
        {
            Vector3 p1 = GetRoutePosition(dist);
            Vector3 p2 = GetRoutePosition(dist + 0.1f);
            return new RoutePoint(p1, (p2 - p1).normalized);
        }

        public Vector3 GetRoutePosition(float dist)
        {
            int point = 0;
            if (Length == 0) Length = distances[distances.Length - 1];
            dist = Mathf.Repeat(dist, Length);
            while (distances[point] < dist) ++point;

            p1n = ((point - 1) + numPoints) % numPoints;
            p2n = point;
            i = Mathf.InverseLerp(distances[p1n], distances[p2n], dist);

            if (smoothRoute)
            {
                p0n = ((point - 2) + numPoints) % numPoints;
                p3n = (point + 1) % numPoints;
                p2n = p2n % numPoints;

                P0 = points[p0n];
                P1 = points[p1n];
                P2 = points[p2n];
                P3 = points[p3n];

                return CatmullRom(P0, P1, P2, P3, i);
            }
            else
            {
                return Vector3.Lerp(points[p1n], points[p2n], i);
            }
        }

        private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
        {
            return 0.5f * ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i + (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
        }

        public int ClosestWaypointNum(Transform from)
        {
            Transform closestWaypoint = Waypoints[0];
            var smallestDistance = Vector3.Distance(from.position, closestWaypoint.position);

            foreach (var waypoint in Waypoints)
            {
                var distance = Vector3.Distance(from.position, waypoint.position);
                if (distance < smallestDistance)
                {
                    closestWaypoint = waypoint;
                    smallestDistance = distance;
                }
            }
            return closestWaypoint.GetSiblingIndex();
        }

        public void AddWaypoint(Transform waypoint)
        {
            if (waypoint == null) return;
            waypointList.items.Add(waypoint);
            CachePositionsAndDistances();
        }

        public void RemoveWaypoint(Transform waypoint)
        {
            if (waypoint == null || !waypointList.items.Contains(waypoint)) return;
            waypointList.items.Remove(waypoint);
            CachePositionsAndDistances();
        }

        public void AddWaypointsFromChildren()
        {
            waypointList.items.Clear();
            foreach (Transform child in transform)
            {
                waypointList.items.Add(child);
            }
            transform.name = "WaypointCircuit";
        }

        private void CachePositionsAndDistances()
        {
            points = new Vector3[Waypoints.Length + 1];
            distances = new float[Waypoints.Length + 1];

            float accumulateDistance = 0;
            for (int i = 0; i < points.Length; ++i)
            {
                var t1 = Waypoints[i % Waypoints.Length];
                var t2 = Waypoints[(i + 1) % Waypoints.Length];
                if (t1 != null && t2 != null)
                {
                    Vector3 p1 = t1.position;
                    Vector3 p2 = t2.position;
                    points[i] = p1;
                    distances[i] = accumulateDistance;
                    accumulateDistance += (p1 - p2).magnitude;
                }
            }
        }

        private void OnDrawGizmos()
        {
            DrawGizmos(false);
        }

        private void OnDrawGizmosSelected()
        {
            DrawGizmos(true);
        }

        private void DrawGizmos(bool selected)
        {
            waypointList.circuit = this;
            if (Waypoints.Length > 1)
            {
                numPoints = Waypoints.Length;
                CachePositionsAndDistances();
                Length = distances[distances.Length - 1];

                Gizmos.color = Color.yellow;
                Vector3 prev = Waypoints[0].position;
                if (smoothRoute)
                {
                    for (float dist = 0; dist < Length; dist += Length / editorVisualisationSubsteps)
                    {
                        Vector3 next = GetRoutePosition(dist + 1);
                        Gizmos.DrawLine(prev, next);
                        prev = next;
                    }
                    Gizmos.DrawLine(prev, Waypoints[0].position);
                }
                else
                {
                    for (int n = 0; n < Waypoints.Length; ++n)
                    {
                        Vector3 next = Waypoints[(n + 1) % Waypoints.Length].position;
                        Gizmos.DrawLine(prev, next);
                        prev = next;
                    }
                }
            }
            foreach (Transform waypoint in Waypoints)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(waypoint.position, 1f);
            }
        }

        [Serializable]
        public class WaypointList
        {
            public WaypointCircuit circuit;
            public List<Transform> items = new List<Transform>(); // Dizi yerine List kullanıldı
        }

        public struct RoutePoint
        {
            public Vector3 position;
            public Vector3 direction;

            public RoutePoint(Vector3 position, Vector3 direction)
            {
                this.position = position;
                this.direction = direction;
            }
        }
    }
}
