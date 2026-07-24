using System;
using static System.Math;
using UnityEngine;

public class Enemy : Unit
{
    private int speed;
    private EnemyDefinition enemyDef;

    public int Speed { get { return speed; } }
    public Enemy(IUnitDefinition unit) : base(unit)
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