using UnityEngine;
using System.Collections.Generic;

public class EnemyManager1 : MonoBehaviour
{
    public static EnemyManager1 Instance;

    private List<Enemy1> enemies = new();

    public List<Enemy1> EnemyList { get { return enemies; } }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Add(Enemy1 enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void Remove(Enemy1 enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}