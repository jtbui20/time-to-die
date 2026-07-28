using System;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

[System.Serializable]
public class FreeEnemy : FreeUnit
{
    [SerializeField] private float speed;

    private EnemyDefinition enemyDef;
    private Waypoint nextWaypoint;
    private AgentPath agentPath = new();
    private int maxHealth;

    public float Speed { get { return speed; } }
    public int MaxHealth { get { return maxHealth; } }
    public AgentPath AgentPath { get { return agentPath; } }

    public FreeEnemy(IUnitDefinition unit, Waypoint? waypoint) : base(unit)
    {
        enemyDef = unit as EnemyDefinition;
        if (enemyDef == null)
        {
            Debug.LogError($"Unit \"{this}\" attempted to initialise with null definition \"{unit}\"");
            return; 
        }
        maxHealth = health;
        nextWaypoint = waypoint;
        AdjustStatus();
    }

    protected override void AdjustStatus()
    {
        speed = enemyDef.Speed;
        base.AdjustStatus();
    }

    public void MoveForTurn()
    {
        if (nextWaypoint == null) 
        {
            var enemyManger = EnemyManager.Instance;
            if (enemyManger != null)
            {
                enemyManger.Escape(this);
                enemyManger.Remove(this); 
            }
            return; 
        }
        
        agentPath = NavMeshUtility.CalculateMoveForTurn(speed, position, nextWaypoint);
        if (agentPath.DestinationPoints.Count > 0)
        {
            nextWaypoint = agentPath.NextWaypoint;
            base.Position = agentPath.DestinationPoints[^1];
        }
    }

    public void ClearPathPoints()
    {
        agentPath.DestinationPoints.Clear();
    }

    public override void Cleanup()
    {
        EnemyManager.Instance.Remove(this);
        Debug.Log($"Cleaning up {GetHashCode()}");

        base.Cleanup();

        // any destroys
    }
}