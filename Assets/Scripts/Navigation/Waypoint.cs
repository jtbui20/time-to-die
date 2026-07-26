using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private bool isSpawnPoint;
    private IWaypointSelector waypointSelector;

    public bool IsSpawnPoint { get { return isSpawnPoint; } }

    private void Awake()
    {
        waypointSelector = GetComponent<IWaypointSelector>();
    }

    public Waypoint? Next()
    {
        if (waypointSelector == null) { waypointSelector = GetComponent<IWaypointSelector>(); }
        return waypointSelector.Next();
    }
}