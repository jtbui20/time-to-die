using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float Health;
    public float Speed;
    public float Reward;
}
