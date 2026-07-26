using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace DefaultNamespace.Data
{
    [CreateAssetMenu(fileName = "NavData", menuName = "Game Data/Navigation Data")]
    public class NavigationScriptableObject : ScriptableObject
    {
        public GameObject NavigationPrefab;
        public List<Waypoint> Waypoints;
        public NavMeshData NavMeshData;

        private void OnValidate()
        {
            GatherWaypoints();
        }

        private void OnEnable()
        {
            GatherWaypoints();
        }

        private void GatherWaypoints()
        {
            Waypoints.Clear();
            var waypoints = NavigationPrefab.GetComponentsInChildren<Waypoint>();
            foreach (var waypoint in waypoints)
            {
                Waypoints.Add(waypoint);
            }
        }
    }
}