using System.Collections.Generic;
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

public static class IListExtensions {
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts) {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i) {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
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

public readonly struct GridCoords
{
    public readonly int X;
    public readonly int Y;

    public GridCoords(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int ToIndex(int width)
    {
        return X + Y * width;
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(X,Y);
    }

    public static GridCoords FromVector2Int(Vector2Int v)
    {
        return new GridCoords(v.x, v.y);
    }

    public GridCoords OffsetFrom(GridCoords coords)
    {
        return new GridCoords(X+coords.X, Y+coords.Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

public struct TileData
{
    public byte TerrainID;
    public bool IsOccupied;
}