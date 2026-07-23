using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemyData enemyData;

    protected Resource health;

    public EnemyData Data { get { return enemyData; } }

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