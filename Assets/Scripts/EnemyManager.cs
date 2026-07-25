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
    private Coroutine[] spawnCoroutines;

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

        spawnCoroutines = new Coroutine[spawners.Count];
    }

    private void Start()
    {
        GameClock clock = GameClock.Instance;
        if (clock != null) { GameClock.Instance.OnTick += Tick; }
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

    public void SpawnWave(int waveNumber)
    {
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
                Add(enemy);

                yield return new WaitForSeconds(delay);
                enemy.MoveForTurn();
            }
        }

        spawnCoroutines[spawnerIndex] = null;
    }
}