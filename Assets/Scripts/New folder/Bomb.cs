using System;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using System.Numerics;

[Serializable]
public class Bomb : Unit
{
    private BombType bombType;
    private int range;
    private int chainDistance;
    private int chainTick;
    private int damage;
    private BombDefinition bombDef;

    public int Range { get { return range; } }
    public int ChainDistance { get { return chainDistance; } }
    public int ChainTick { get { return chainTick; } }
    public int Damage { get { return damage; } }

    public Bomb(IUnitDefinition unit) : base(unit)
    {
        bombDef = unit as BombDefinition;
        if (bombDef == null)
        {
            Debug.LogError($"Unit \"{this}\" attempted to initialise with null definition \"{unit}\"");
            return; 
        }


        AdjustStatus();
    }

    protected override void AdjustStatus()
    {
        bombType = bombDef.BombType;
        range = bombDef.Range;
        chainDistance = bombDef.ChainDistance;
        chainTick = bombDef.ChainTick;
        damage = bombDef.Damage;
        base.AdjustStatus();
    }

    private List<Bomb> Explode()
    {
        List<Bomb> targets = new();

        /*
        foreach (Bomb bomb in BombManager.Instance.Bombs)
        {
            if (Helper.FlattenedDistance(Position, bomb.Position));
        }
        */

        return targets;
    }

}