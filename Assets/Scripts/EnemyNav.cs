using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
public class EnemyNav: MonoBehaviour
{
    [SerializeField] private NavAgentConfig navAgentConfig;
    private Enemy enemyController;
    private NavMeshAgent agent;
    [SerializeField] private Waypoint waypoint;
    [SerializeField] private Vector3 destination;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<Enemy>();

        agent.speed = enemyController.Data.Speed;

    }

    public void Initialise(Waypoint dest)
    {
        SetNextDestination(dest);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, destination) <= navAgentConfig.ArrivalDistance)
        {
            SetNextDestination(waypoint.Next());
        }
    }

    private void SetNextDestination(Waypoint dest)
    {
        if (dest != null)
        {
            waypoint = dest;
            float x = navAgentConfig.DestinationVariance.x;
            float y = navAgentConfig.DestinationVariance.y;
            Vector3 variance = new Vector3(Random.Range(-x, x), 0, Random.Range(-y, y));
            destination = dest.transform.position + variance;
            agent.destination = destination;
        }
    }
}