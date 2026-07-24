using System;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

[System.Serializable]
public class FreeBomb : FreeUnit
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

    public FreeBomb(IUnitDefinition unit) : base(unit)
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

    public List<IDamageable> Explode(int destructibleMask)
    {
        List<IDamageable> targets = new();

        Collider[] hitColliders = Physics.OverlapSphere(position, range, destructibleMask);
        foreach (Collider collider in hitColliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (damageable.Source == this) { continue; }
                targets.Add(damageable.Source);
            }
        }

        /*
        foreach (FreeBomb bomb in BombManager.Instance.Bombs)
        {
            if (bomb == this) { continue; }

            
            if (Helper.FlattenedDistance(Position, bomb.Position) <= (float)range)
            {
                targets.Add(bomb);
            }
        }
        */

        return targets;
    }
}