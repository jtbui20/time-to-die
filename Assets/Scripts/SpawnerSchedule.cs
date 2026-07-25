using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnerSchedule", menuName = "Scriptable Objects/SpawnerSchedule")]
public class SpawnerSchedule : ScriptableObject
{
    [SerializeField] private List<SpawnTiming> schedule = new();
    
    public List<SpawnTiming> Schedule { get { return schedule; } }
}

[System.Serializable]
public struct SpawnTiming
{
    public int TurnNumber;
    public int SpawnPointIndex;
    public EnemyDefinition Enemy;
    public int EnemyCount;
}