using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Cysharp.Threading.Tasks;

public class EnemyView : UnitView
{
    public FreeEnemy Enemy { get; private set; }
    public override IDamageable Source { get { return Enemy; } }

    private NavMeshAgent agent;
    private bool isMoving = false;

    public void Init(FreeEnemy enemy)
    {
        if (enemy == null) { return; }

        Enemy = enemy;

        base.Init(enemy);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(Enemy.Position);
    }

    protected override void UpdatePosition()
    {
        if (agent == null) { return; }

        if (!isMoving)
        {
            MoveEnemy().Forget();
        }
    }

    private async UniTask MoveEnemy()
    {
        isMoving = true;

        AgentPath path = Enemy.AgentPath;

        foreach (Vector3 destination in path.DestinationPoints)
        {
            agent.SetDestination(destination);

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                await UniTask.Yield();
            }
        }

        isMoving = false;
    }

    protected override void UpdateView()
    {
        // health view goes here
    }
}