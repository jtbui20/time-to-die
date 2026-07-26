using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private SpawnerSchedule spawnSchedule;
    [SerializeField] private List<Waypoint> spawners;
    [SerializeField] private float spawnDelay = 1f;
    public static EnemyManager Instance;
    public event Action OnEnemyCountChanged;
    public event Action OnEnemyEscape;
    public event Action OnEnemiesDefeated;

    private List<FreeEnemy> enemies = new();
    private Coroutine[] spawnCoroutines;
    private int finalWave = -1;
    private int currentWave = 0;
    [SerializeField] private int enemyCount;

    public List<FreeEnemy> EnemyList { get { return enemies; } }
    public int EnemyCount { get { return enemyCount; } }

    private void Awake()
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

    private void Start()
    {
        GameClock clock = GameClock.Instance;
        if (clock != null) { GameClock.Instance.OnTick += Tick; }
    }

    public void SetupSpawner(SpawnerSchedule spawns, List<Waypoint> waypoints)
    {
        spawnSchedule = spawns;
        int highestWave = 0;
        foreach (var spawn in spawns.Schedule)
        {
            if (spawn.TurnNumber > highestWave)
            {
                highestWave = spawn.TurnNumber;
            }
        }
        finalWave = highestWave;
        
        foreach (var waypoint in waypoints)
        {
            if (waypoint.IsSpawnPoint)
            {
                spawners.Add(waypoint);
            }
        }
        spawnCoroutines = new Coroutine[spawners.Count];
    }

    public void Tick(int turnNumber)
    {
        foreach (var enemy in enemies)
        {
            enemy.MoveForTurn();
        }
        currentWave = turnNumber;
        SpawnWave(turnNumber);
    }

    public void Add(FreeEnemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            enemyCount++;
            OnEnemyCountChanged?.Invoke();
        }
    }

    private void AddWithoutCount(FreeEnemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void Remove(FreeEnemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            enemyCount--;
            OnEnemyCountChanged?.Invoke();
        }

        if (currentWave >= finalWave && enemies.Count <= 0)
        {
            // all enemies dead
            OnEnemiesDefeated?.Invoke();
        }
    }

    public void Escape(FreeEnemy enemy)
    {
        // trigger lose life
        OnEnemyEscape?.Invoke();
        enemy.Cleanup();
    }

    public void ProcessStep()
    {
        foreach (var enemy in enemies)
        {
            enemy.MoveForTurn();
        }
    }

    public void ProcessDeathChains()
    {
        for (var i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].Health <= 0)
            {
                enemies[i].Cleanup();
            }
        }
    }

    public void SpawnWave(int waveNumber)
    {
        currentWave = waveNumber;
        if (spawnSchedule == null) { return; }

        for (int i = 0; i < spawners.Count; i++)
        {
            Queue<SpawnTiming> spawnQueue = new();
            foreach (var spawn in spawnSchedule.Schedule)
            {
                if (spawn.TurnNumber == waveNumber && spawn.SpawnPointIndex == i)
                {
                    spawnQueue.Enqueue(spawn);
                }
            }

            if (spawnQueue.Count > 0)
            {
                if (spawnCoroutines[i] == null)
                {
                    int newEnemies = 0;
                    foreach (var group in spawnQueue)
                    {
                        newEnemies += group.EnemyCount;
                    }
                    enemyCount += newEnemies;
                    OnEnemyCountChanged?.Invoke();
                    spawnCoroutines[i] = StartCoroutine(SpawnWithDelay(spawnDelay, spawnQueue));
                }
            }
        }
    }

    private IEnumerator SpawnWithDelay(float delay, Queue<SpawnTiming> queue)
    {
        int spawnerIndex = queue.Peek().SpawnPointIndex;
        while (queue.Count > 0)
        {
            var spawns = queue.Dequeue();
            for (int i = 0; i < spawns.EnemyCount; i++)
            {
                var enemy = new FreeEnemy(spawns.Enemy, spawners[spawnerIndex]);
                var enemyView = GameObject.Instantiate(spawns.Enemy.EnemyPrefab).GetComponent<EnemyView>();

                enemyView.Init(enemy);
                enemy.Position = spawners[spawnerIndex].gameObject.transform.position;
                AddWithoutCount(enemy);
                yield return new WaitForSeconds(delay);
                enemy.MoveForTurn();
            }
        }

        spawnCoroutines[spawnerIndex] = null;
    }
}