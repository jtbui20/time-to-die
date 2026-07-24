using UnityEngine;

public class Enemy1 : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemyData enemyData;

    protected Resource health;
    protected EnemyNav navAgent;

    public EnemyData Data { get { return enemyData; } }
    public Vector3 Position { get { return transform.position; } }
    public Vector3 Velocity { get { return navAgent.Velocity; } }

    protected void Awake()
    {
        Initialise();
    }
    protected void Initialise()
    {
        if (enemyData != null)
        {
            health = ScriptableObject.CreateInstance<Resource>();
            health.Initialise(enemyData.Health, 0, enemyData.Health);
        }
        else
        {
            Debug.LogError($"{this.name} has not been assigned data values!");
            gameObject.SetActive(false);
        }

        navAgent = GetComponent<EnemyNav>();
    }

    public void TakeDamage(float damage)
    {
        health.ChangeValue(-damage);
    }

    protected void CheckHealth()
    {
        if (health.Value > health.Min) { return; }

        Destroy(gameObject);
    }
}