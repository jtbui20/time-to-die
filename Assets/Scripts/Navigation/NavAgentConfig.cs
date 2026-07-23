using UnityEngine;

[CreateAssetMenu(fileName = "NavAgentConfig", menuName = "Scriptable Objects/Enemy/NavConfig")]
public class NavAgentConfig : ScriptableObject
{
    public Vector2 DestinationVariance;
    public float ArrivalDistance;

}
