using System;
using static System.Math;
using UnityEngine;

public abstract class Unit
{
    public event Action OnStatusChanged;
    public event Action OnPositionChanged;

    protected IUnitDefinition unitDef;
    protected int health;
    protected GridCoords position;

    public int Health { get { return health; } }
    public string Description { get { return unitDef != null ? unitDef.Description : ""; } }
    public GridCoords Position { get { return position; } set { position = value; OnPositionChanged?.Invoke(); } }
    public Unit(IUnitDefinition unit)
    {
        if (unit == null)
        {
            Debug.LogError($"Unit \"{this}\" attempted to initialise with null definition \"{unit}\"");
            return; 
        }

        unitDef = unit;
        position = new GridCoords(-1, -1);

        unitDef.OnRebuild += AdjustStatus;

        health = unitDef.Health;
    }

    protected virtual void AdjustStatus()
    {
        OnStatusChanged?.Invoke();
    }

    protected virtual void Cleanup()
    {
        unitDef.OnRebuild -= AdjustStatus;
        unitDef = null;
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