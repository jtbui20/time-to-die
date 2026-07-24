using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombManager : MonoBehaviour
{
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

        List<FreeBomb> additionalTargets = currentBomb.Explode();

        foreach (FreeBomb target in additionalTargets)
        {
            target.ChangeHealth(-currentBomb.ChainTick);
            if (target.Health <= 0)
            {
                if (explodeQueue.Contains(target))
                {
                    explodeQueue.Remove(target);
                }
                explodeQueue.Insert(0, target);
            }
        }

        currentBomb.Cleanup();

        yield return new WaitForSeconds(explodeDelay);
    }

    explosionRoutine = null;
}

}