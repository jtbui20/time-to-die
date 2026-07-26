using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DefaultNamespace.CustomAnimations;
using DefaultNamespace.Data;
using DefaultNamespace.VFX;

public class BombManager : MonoBehaviour
{
    [SerializeField] private LayerMask destructibleMask;
    [SerializeField] private float explodeDelay = 0.5f;
    public static BombManager Instance;
    private List<FreeBomb> bombs = new();
    [SerializeField] private List<FreeBomb> explodeQueue = new();
    public List<FreeBomb> Bombs { get { return bombs; } }
    public BombBindings bombPrefabBindings;
    
    [SerializeField]
    private AnimBombMoveConfig bombMoveConfig;

    private Coroutine explosionRoutine;

    public AnimationQueue animationQueue;

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
        foreach (var bomb in bombs)
        {
            bomb.ChangeHealth(Random.Range(0, 5));
        }
    }

    public void SpawnBomb(FreeBomb bomb, Vector3 targetPosition)
    {
        var GameObject = bombPrefabBindings.GetPrefab(bomb.BombType);
        var bombObject = Instantiate(GameObject, targetPosition, Quaternion.identity);
        BombView bombView = bombObject.GetComponent<BombView>();

        var clonedBomb = new FreeBomb(bomb);

        bombView.Init(clonedBomb);
        bombs.Add(clonedBomb);
    }

    public void Add(FreeBomb bomb)
    {
        bombs.Add(bomb);
    }

    public void Tick(int turnNumber)
    {
        CountdownBombs();
    }

    public void CountdownBombs()
    {
        foreach (FreeBomb bomb in bombs)
        {
            bomb.ChangeHealth(-1);
            if (bomb.Health <= 0)
            {
                explodeQueue.Add(bomb);
            }
        }
    }

    public void GenerateBombActionQueue()
    {
        foreach (FreeBomb bomb in explodeQueue)
        {
            GenerateExplodeAction(bomb);
        }
        
        explodeQueue.Clear();
    }

    public UniTask WaitForBombsToComplete()
    {
        return animationQueue.GetCompletionToken();
    }

    public void GenerateExplodeAction(FreeBomb currentBomb, int currentChain = 0, bool isHead = false)
    {
        var ExplodeAction = new AnimBombExplode(currentBomb, 0.2f);
        ExplodeAction.OnComplete += () =>
        {
            CalculateNextExplosions(currentBomb, currentChain + 1);
        };
        
        animationQueue.RemoveDuplicateExplode(currentBomb);
        if (isHead)
        {
            animationQueue.EnqueueHead(ExplodeAction);
        }
        else
        {
            animationQueue.Enqueue(ExplodeAction);
        }
    }
    
    public void GenerateMoveAction(FreeBomb currentBomb, Vector3 targetPosition)
    {
        var MoveAction = new AnimBombMove(currentBomb, targetPosition, 0.2f, bombMoveConfig);
        animationQueue.EnqueueHead(MoveAction);
    }

    public void CalculateNextExplosions(FreeBomb currentBomb, int currentChain)
    {
        List<IDamageable> targets = currentBomb.GetDamageableInExplosionRadius(destructibleMask);

        foreach (IDamageable target in targets)
        {
            if (target == null)
            {
                return;
            }

            if (target is FreeBomb chainedBomb)
            {
                var direction = (chainedBomb.Position - currentBomb.Position);
                direction.y = 0f;
                direction.Normalize();
                Vector3 newPos = chainedBomb.Position + direction * currentBomb.ChainDistance;
                
                // Substitute with damage formula
                chainedBomb.TakeDamage(currentBomb.ChainTick);

                if (chainedBomb.Health <= 0)
                {
                    GenerateExplodeAction(chainedBomb, currentChain + 1, true);
                }
                
                GenerateMoveAction(chainedBomb, newPos);
            }
            else
            {
                target.TakeDamage(currentBomb.Damage);
            }
        }
        currentBomb.Cleanup();
    }
}