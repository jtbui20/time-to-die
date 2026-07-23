using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private IWaypointSelector waypointSelector;

    private void Awake()
    {
        waypointSelector = GetComponent<IWaypointSelector>();
    }

    public Waypoint? Next()
    {
        return waypointSelector.Next();
    }
}