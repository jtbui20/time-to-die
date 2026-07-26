using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BombDefinition", menuName = "Scriptable Objects/Bomb/BombDefinition")]
public class BombDefinition : ScriptableObject, IUnitDefinition
{
    [SerializeField] private BombType bombType;
    [SerializeField] private int health = 3;
    [SerializeField] private int range = 5;
    [SerializeField] private int chainDistance = 5;
    [SerializeField] private int chainTick = 1;
    [SerializeField] private float chainDamageMult = 1f;
    [SerializeField] private float chainRangeMult = 0.2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private string description;

    public BombType BombType { get { return bombType; } }
    public int Health { get { return health; } }
    public int Range { get { return range; } }
    public int ChainDistance { get { return chainDistance; } }
    public int ChainTick { get { return chainTick; } }
    public float ChainDamageMult { get { return chainDamageMult; } }
    public float ChainRangeMult { get { return chainRangeMult; } }
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
    Standard,
    Ice,
    Chain,
    Instant,
    Molotov,
    TNT
}