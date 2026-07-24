using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombManager : MonoBehaviour
{
    [SerializeField] private LayerMask destructibleMask;
    [SerializeField] private float explodeDelay = 0.5f;
    public static BombManager Instance;
    private List<FreeBomb> bombs = new();
    [SerializeField] private List<FreeBomb> explodeQueue = new();
    public List<FreeBomb> Bombs { get { return bombs; } }

    private Coroutine explosionRoutine;

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
        GameClock.Instance.OnTick += Tick;

        foreach (var bomb in bombs)
        {
            bomb.ChangeHealth(Random.Range(0, 5));
        }
    }

    public void Add(FreeBomb bomb)
    {
        bombs.Add(bomb);
    }

    public void Tick()
    {
        List<FreeBomb> additionalTargets;
        explodeQueue.Clear();
        foreach (FreeBomb bomb in bombs)
        {
            bomb.ChangeHealth(-1);
            if (bomb.Health <= 0)
            {
                explodeQueue.Add(bomb);
            }
        }

        if (explodeQueue.Count > 0 && explosionRoutine == null)
        {
            explosionRoutine = StartCoroutine(ProcessExplosions());
        }
    }

    private IEnumerator ProcessExplosions()
{
    while (explodeQueue.Count > 0)
    {
        FreeBomb currentBomb = explodeQueue[0];
        explodeQueue.RemoveAt(0);
        bombs.Remove(currentBomb);
        Debug.Log($"Exploding {currentBomb}");

        List<IDamageable> additionalTargets = currentBomb.Explode(destructibleMask);

        foreach (IDamageable target in additionalTargets)
        {
            if (target is FreeBomb bomb)
            {
                // KB
                Vector3 direction = bomb.Position - currentBomb.Position;
                direction.y = 0f;
                direction.Normalize();
                Vector3 newPos = bomb.Position + direction * currentBomb.ChainDistance;
                bomb.Position = newPos;

                // Tick
                bomb.TakeDamage(currentBomb.ChainTick);
                if (bomb.Health <= 0)
                {
                    if (explodeQueue.Contains(bomb))
                    {
                        explodeQueue.Remove(bomb);
                    }
                    explodeQueue.Insert(0, bomb);
                }
            }
            else
            {
                target.TakeDamage(currentBomb.Damage);
            }
        }

        currentBomb.Cleanup();

        yield return new WaitForSeconds(explodeDelay);
    }

    explosionRoutine = null;
}

}