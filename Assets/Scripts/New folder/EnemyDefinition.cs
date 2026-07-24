using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "Scriptable Objects/Enemy/EnemyDefinition")]
public class EnemyDefinition : ScriptableObject, IUnitDefinition
{
    [SerializeField] private int health;
    [SerializeField] private int speed;
    [SerializeField] private string description;

    public int Health { get { return health; } }
    public int Speed { get { return speed; } }
    public string Description { get { return description; } }

    public event Action OnRebuild;

    private void OnEnable()
    {
        Rebuild();
    }

    private void OnValidate()
    {
        Rebuild();
    }

    private void Rebuild()
    {
        OnRebuild?.Invoke();
    }

}