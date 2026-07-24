using System;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

[System.Serializable]
public class FreeEnemy : FreeUnit
{
    [SerializeField] private int speed;

    private EnemyDefinition enemyDef;

    public int Speed { get { return speed; } }

    public FreeEnemy(IUnitDefinition unit) : base(unit)
    {
        enemyDef = unit as EnemyDefinition;
        if (enemyDef == null)
        {
            Debug.LogError($"Unit \"{this}\" attempted to initialise with null definition \"{unit}\"");
            return; 
        }

        AdjustStatus();
    }

    protected override void AdjustStatus()
    {
        speed = enemyDef.Speed;
        base.AdjustStatus();
    }
}