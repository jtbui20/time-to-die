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
    private float currentDamageMult;
    private float currentRangeMult;

    public int Range { get { return range; } }
    public int ChainDistance { get { return chainDistance; } }
    public int ChainTick { get { return chainTick; } }
    public int Damage { get { return damage; } }
    public BombType BombType { get { return bombType; } }
    public float CurrentDamageMult { get { return currentDamageMult; } }
    public float DamageMult { get { return bombDef.ChainDamageMult;}}
    public float CurrentRangeMult { get { return currentRangeMult; } }
    public float RangeMult { get { return bombDef.ChainRangeMult; } }

    public FreeBomb(IUnitDefinition unit) : base(unit)
    {
        bombDef = unit as BombDefinition;
        if (bombDef == null)
        {
            Debug.LogError($"Unit \"{this}\" attempted to initialise with null definition \"{unit}\"");
            return; 
        }
        currentDamageMult = 0f;
        currentRangeMult = 0f;

        AdjustStatus();
    }

    public FreeBomb(FreeBomb original) : this(original.bombDef)
    {
        // Clone over any upgrades or states not originating from BombDefinition

        //health = original.health;
        currentDamageMult = original.CurrentDamageMult;
        currentRangeMult = original.CurrentRangeMult;
    }

    public void ApplyChainScaling(FreeBomb chainBomb)
    {
        float combinedDamage = chainBomb.CurrentDamageMult + currentDamageMult;
        currentDamageMult = (1f + combinedDamage) * (1f + chainBomb.DamageMult) - 1f;

        float combinedRange = chainBomb.CurrentRangeMult + currentRangeMult;
        currentRangeMult = (1f + combinedRange) * (1f + chainBomb.RangeMult) - 1f;
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

    public override void Cleanup()
    {
        BombManager.Instance.Remove(this);
        base.Cleanup();
    }

    public List<IDamageable> GetDamageableInExplosionRadius(int destructibleMask)
    {
        List<IDamageable> targets = new();

        Collider[] hitColliders = Physics.OverlapSphere(position, range * (1f + CurrentRangeMult), destructibleMask, QueryTriggerInteraction.Collide);
        
        //DebugExplosion(position, range * (1f + CurrentRangeMult));

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

    private void DebugExplosion(Vector3 position, float radius)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = Vector3.one * radius * 2f;

        GameObject.Destroy(sphere, 3f);
    }

    /// <summary>
    /// This doesn't check the component, we expect it to have it
    /// </summary>
    /// <param name="destructibleMask"></param>
    /// <returns></returns>
    public RaycastHit[] GetDamageableInexplosionRadiusRaycast(int destructibleMask)
    {
        RaycastHit[] results = new RaycastHit[10];
        var size = Physics.SphereCastNonAlloc(position, range, Vector3.up, results, 0f, destructibleMask);
        return results;
    }
}