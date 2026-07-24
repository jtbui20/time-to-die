using UnityEngine;

[RequireComponent(typeof(Waypoint))]
public class AgentSpawner: MonoBehaviour
{
    private EnemyManager1 enemyManager;
    private Waypoint waypoint;
    [SerializeField] private GameObject agentPrefab;

    private void Start()
    {
        enemyManager = EnemyManager1.Instance;
        waypoint = GetComponent<Waypoint>();
        if (agentPrefab != null)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject agent = GameObject.Instantiate(agentPrefab);
        EnemyNav navController = agent.GetComponent<EnemyNav>();
        navController.Initialise(waypoint.Next());

        if (enemyManager != null)
        {
            enemyManager.Add(agent.GetComponent<Enemy1>());
        }
    }
}