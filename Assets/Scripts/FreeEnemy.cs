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

    public float Speed { get { return speed; } }
    public AgentPath AgentPath { get { return agentPath; } }

    public FreeEnemy(IUnitDefinition unit, Waypoint? waypoint) : base(unit)
    {
        enemyDef = unit as EnemyDefinition;
        if (enemyDef == null)
        {
            Debug.LogError($"Unit \"{this}\" attempted to initialise with null definition \"{unit}\"");
            return; 
        }

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
        if (nextWaypoint == null) { return; }
        
        agentPath = NavMeshUtility.CalculateMoveForTurn(speed, position, nextWaypoint);
        if (agentPath.DestinationPoints.Count > 0)
        {
            nextWaypoint = agentPath.NextWaypoint;
        }
        base.Position = agentPath.DestinationPoints[^1];
    }

    public void ClearPathPoints()
    {
        agentPath.DestinationPoints.Clear();
    }
}