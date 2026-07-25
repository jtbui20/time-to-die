using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private SpawnerSchedule spawnSchedule;
    [SerializeField] private List<Waypoint> spawners;
    [SerializeField] private float spawnDelay = 1f;
    public static EnemyManager Instance;

    private List<FreeEnemy> enemies = new();
    [SerializeField] private Coroutine[] spawnCoroutines;

    public List<FreeEnemy> EnemyList { get { return enemies; } }

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
        var waypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
        foreach (var waypoint in waypoints)
        {
            if (waypoint.IsSpawnPoint)
            {
                spawners.Add(waypoint);
            }
        }
        spawnCoroutines = new Coroutine[spawners.Count];

        GameClock clock = GameClock.Instance;
        if (clock != null) { GameClock.Instance.OnTick += Tick; }
    }

    public void SetupSpawner(SpawnerSchedule spawns)
    {
        spawnSchedule = spawns;
    }

    public void Tick(int turnNumber)
    {
        foreach (var enemy in enemies)
        {
            enemy.MoveForTurn();
        }

        SpawnWave(turnNumber);
    }

    public void Add(FreeEnemy enemy)
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
        }
    }

    public void ProcessStep()
    {
        foreach (var enemy in enemies)
        {
            Debug.Log($"{enemy} is moving");
            enemy.MoveForTurn();
        }
    }

    public void SpawnWave(int waveNumber)
    {
        Debug.Log($"Spawning wave {waveNumber}");
        if (spawnSchedule == null) { return; }

        Debug.Log($"A");
        for (int i = 0; i < spawners.Count; i++)
        {
            Debug.Log($"B");
            Queue<SpawnTiming> spawnQueue = new();
            foreach (var spawn in spawnSchedule.Schedule)
            {
                Debug.Log($"C");
                if (spawn.TurnNumber == waveNumber && spawn.SpawnPointIndex == i)
                {
                    spawnQueue.Enqueue(spawn);
                }
            }

            if (spawnQueue.Count > 0)
            {
                Debug.Log($"D");
                Debug.Log($"D : {spawnCoroutines[i]}, {spawnCoroutines[i] == null}");
                if (spawnCoroutines[i] == null)
                {
                    Debug.Log($"E");
                    spawnCoroutines[i] = StartCoroutine(SpawnWithDelay(spawnDelay, spawnQueue));
                }
            }
        }
    }

    private IEnumerator SpawnWithDelay(float delay, Queue<SpawnTiming> queue)
    {
        Debug.Log($"Spawning on delay");
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
                Add(enemy);
                Debug.Log($"Spawned {enemyView.gameObject.name}");
                yield return new WaitForSeconds(delay);
                enemy.MoveForTurn();
            }
        }

        spawnCoroutines[spawnerIndex] = null;
    }
}