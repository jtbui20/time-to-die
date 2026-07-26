using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Cysharp.Threading.Tasks;

public class EnemyView : UnitView
{
    public FreeEnemy Enemy { get; private set; }
    public override IDamageable Source { get { return Enemy; } }
    [SerializeField] private Transform hpBar;
    [SerializeField] private GameObject healthCanvas;

    private NavMeshAgent agent;
    private bool isMoving = false;
    private Camera camera;

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
        camera = Camera.main;
    }

    private void Update()
    {
        if (camera != null && healthCanvas != null)
        {
            healthCanvas.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        }
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
        if (hpBar != null)
        {
            Vector3 scale = hpBar.localScale;
            scale.x = Mathf.Clamp01((float)Enemy.Health/Enemy.MaxHealth);
            hpBar.localScale = scale;
        }
        base.UpdateView();
    }
}