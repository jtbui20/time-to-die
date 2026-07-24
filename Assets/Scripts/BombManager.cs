using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;

public class BombManager : MonoBehaviour
{
    [SerializeField] private LayerMask destructibleMask;
    [SerializeField] private float explodeDelay = 0.5f;
    public static BombManager Instance;
    private List<FreeBomb> bombs = new();
    [SerializeField] private List<FreeBomb> explodeQueue = new();
    public int CountInQueue => explodeQueue.Count;

    public List<FreeBomb> Bombs
    {
        get { return bombs; }
    }

    public GameObject BombPrebuiltPrefab;

    private Coroutine explosionRoutine;

    private ActionQueueManager actionQueue;

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

        actionQueue = GetComponent<ActionQueueManager>();
    }

    private void Start()
    {
        foreach (var bomb in bombs)
        {
            bomb.ChangeHealth(Random.Range(0, 5));
        }
    }

    public void Add(FreeBomb bomb)
    {
        bombs.Add(bomb);
    }

    public void CountdownBombs()
    {
        foreach (FreeBomb bomb in bombs)
        {
            bomb.ChangeHealth(-1);
        }
    }

    // We "pre generate" based on the data that we have
    public void PrepareQueue()
    {
        
        foreach (var freeBomb in bombs)
        {
            if (freeBomb.Health <= 0)
            {
                actionQueue.EnqueueTask(new ActionQueueRequest(
                    ActionQueueType.Explode, 
                    async (cancellationToken) =>
                {
                    ProcessSingleExplosion(freeBomb);
                    await UniTask.Delay((int)(explodeDelay * 1000), cancellationToken: cancellationToken);
                }));

                // We can try do some simulations here to
                // increase number of bombs to get exactly the combo count
            }
        }
    }

    private void ProcessSingleExplosion(FreeBomb bomb)
    {
        List<IDamageable> additionalTargets = bomb.GetExplodeHits(destructibleMask);

        foreach (IDamageable target in additionalTargets)
        {
            if (target is FreeBomb newBomb)
            {
                // KB
                Vector3 direction = newBomb.Position - bomb.Position;
                direction.y = 0f;
                direction.Normalize();
                Vector3 newPos = newBomb.Position + direction * bomb.ChainDistance;
                newBomb.Position = newPos;

                // Tick
                newBomb.TakeDamage(bomb.ChainTick);
                if (newBomb.Health <= 0)
                {
                    if (explodeQueue.Contains(newBomb))
                    {
                        explodeQueue.Remove(newBomb);
                    }

                    explodeQueue.Insert(0, newBomb);
                }
            }
            else
            {
                target.TakeDamage(bomb.Damage);
            }
        }

        bomb.Cleanup();
    }

    // The action sequencer will manage all this for us
    private void QueueAllCurrentExplosions()
    {
        foreach (FreeBomb bomb in explodeQueue)
        {
            // I need to actually enqueue this task to a choreographer instead, might be the action queue
            actionQueue.EnqueueTask(new ActionQueueRequest(
                ActionQueueType.Explode,
                async (cancellationToken) =>
                {
                    ProcessSingleExplosion(bomb);
                    await UniTask.Delay((int)(explodeDelay * 1000), cancellationToken: cancellationToken);
                }));
        }
    }
}