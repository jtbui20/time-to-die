using UnityEngine;
using System.Collections.Generic;

public class WaypointRandom : MonoBehaviour, IWaypointSelector
{
    [SerializeField] private List<Waypoint> waypoints = new();

    public Waypoint? Next()
    {
        if (waypoints.Count == 0) { return null; }

        int rand = Random.Range(0, waypoints.Count);
        return waypoints[rand];
    }
}