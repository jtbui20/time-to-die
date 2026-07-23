using UnityEngine;

public static class Helper
{
    public static Vector2 Vector3to2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector3 Vector2to3(Vector2 v, float y)
    {
        return new Vector3(v.x, y, v.y);
    }

    public static float FlattenedDistance(Vector3 origin, Vector3 target)
    {
        return Vector2.Distance(Vector3to2(origin), Vector3to2(target));
    }
}

public struct ProjectileStats
{
    public float Damage;
    public Vector3 Direction;
    public float Speed;
    public float MinHoming;
    public float MaxHoming;
    public float Lifetime;
    public Transform Target;

    public ProjectileStats(float damage, Vector3 direction, float speed, float minHoming, float maxHoming, float lifetime, Transform target)
    {
        Damage = damage;
        Direction = direction;
        Speed = speed;
        MinHoming = minHoming;
        MaxHoming = maxHoming;
        Lifetime = lifetime;
        Target = target;
    }
}