using System;
using static System.Math;
using UnityEngine;

public abstract class FreeUnit
{
    public event Action OnStatusChanged;
    public event Action OnPositionChanged;
    public event Action OnCleanup;

    protected IUnitDefinition unitDef;
    protected int health;
    protected Vector3 position;

    public int Health { get { return health; } }
    public string Description { get { return unitDef != null ? unitDef.Description : ""; } }
    public Vector3 Position { get { return position; } set { position = value; OnPositionChanged?.Invoke(); } }
    public FreeUnit(IUnitDefinition unit)
    {
        if (unit == null)
        {
            Debug.LogError($"FreeUnit \"{this}\" attempted to initialise with null definition \"{unit}\"");
            return; 
        }

        unitDef = unit;
        position = new Vector3(-1, -1, -1);

        unitDef.OnRebuild += AdjustStatus;

        health = unitDef.Health;
    }

    protected virtual void AdjustStatus()
    {
        OnStatusChanged?.Invoke();
    }

    public virtual void Cleanup()
    {
        if (unitDef != null)
        {
            unitDef.OnRebuild -= AdjustStatus;
            unitDef = null;
        }
        OnCleanup?.Invoke();
    }

    public void ChangeHealth(int value)
    {
        health += value;
        OnStatusChanged?.Invoke();
    }

    public void SetHealth(int value)
    {
        health = value;
        OnStatusChanged?.Invoke();
    }
}