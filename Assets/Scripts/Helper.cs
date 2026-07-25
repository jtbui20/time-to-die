using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

public static class NavMeshUtility
{
    public static float GetPathLength(NavMeshPath path)
    {
        float length = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return length;
    }

    public static Vector3 GetPointOnPath(NavMeshPath path, float distance)
    {
        if (path.corners.Length == 0) { return Vector3.zero; }

        Vector3 current = path.corners[0];
        for (int i = 1; i < path.corners.Length; i++)
        {
            Vector3 next = path.corners[i];
            float segment = Vector3.Distance(current, next);
            if (distance <= segment)
            {
                return Vector3.MoveTowards(current, next, distance);
            }

            distance -= segment;
            current = next;
        }
        return path.corners[^1];
    }

    public static bool CalculatePath(Vector3 start, Vector3 end, NavMeshPath path)
    {
        // might need to use area mask later
        int NavmeshArea = NavMesh.AllAreas;
        return NavMesh.CalculatePath(start, end, NavmeshArea, path);
    }

    public static AgentPath CalculateMoveForTurn(float speed, Vector3 start, Waypoint end)
    {
        AgentPath agentPath = new() { NextWaypoint = end };
        float remainingDistance = speed;
        NavMeshPath path = new();

        // End point is not vector3 because it needs to be extended if unit is too close to destination
        Waypoint currentWaypoint = end; 
        Vector3 currentStart = start;
        Vector3 currentEnd = currentWaypoint.transform.position;
        
        
        while (remainingDistance > 0f && currentWaypoint != null)
        {
            path.ClearCorners();
            if (!CalculatePath(currentStart, currentEnd, path)) { break; } 

            float pathLength = GetPathLength(path);
            if (remainingDistance <= pathLength)
            {
                agentPath.DestinationPoints.Add(GetPointOnPath(path, remainingDistance));
                remainingDistance = 0f;
            }
            else
            {
                agentPath.DestinationPoints.Add(currentEnd);
                remainingDistance -= pathLength;
                currentWaypoint = currentWaypoint.Next();
                agentPath.NextWaypoint = currentWaypoint;

                if (currentWaypoint == null) 
                {
                    break; 
                }
            
                currentStart = currentEnd;
                currentEnd = currentWaypoint.transform.position;
            }
        }
        return agentPath;
    }
}

public class AgentPath
{
    public List<Vector3> DestinationPoints = new();
    public Waypoint NextWaypoint = null;
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