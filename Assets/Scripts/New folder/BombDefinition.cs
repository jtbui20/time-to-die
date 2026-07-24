using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BombDefinition", menuName = "Scriptable Objects/Bomb/BombDefinition")]
public class BombDefinition : ScriptableObject, IUnitDefinition
{
    [SerializeField] private BombType bombType;
    [SerializeField] private int health;
    [SerializeField] private int range;
    [SerializeField] private int chainDistance;
    [SerializeField] private int chainTick;
    [SerializeField] private int damage;
    [SerializeField] private string description;

    public BombType BombType { get { return bombType; } }
    public int Health { get { return health; } }
    public int Range { get { return range; } }
    public int ChainDistance { get { return chainDistance; } }
    public int ChainTick { get { return chainTick; } }
    public int Damage { get { return damage; } }
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

public enum BombType
{
    Standard
}