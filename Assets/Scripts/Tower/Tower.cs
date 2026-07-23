using UnityEngine;

public abstract class Tower: MonoBehaviour
{
    [SerializeField] protected TowerData towerData;
    [SerializeField] protected Transform launcherPoint;

    protected EnemyManager enemyManager;
    [SerializeField] protected Enemy target;
    protected float scanTimer = 0f;
    protected float shootTimer = 0f;

    protected void Start()
    {
        enemyManager = EnemyManager.Instance;

        if (towerData == null) 
        {
            Debug.LogError($"{this.name} was not assigned tower data!"); 
            gameObject.SetActive(false); 
        }

        if (enemyManager == null) 
        {
            Debug.LogError($"{this.name} can not find EnemyManager in scene!"); 
            gameObject.SetActive(false); 
        }
    }

    protected void Update()
    {
        scanTimer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;

        Scan();
        Track();
        Shoot();
    }

    protected void Scan()
    {
        if (scanTimer <= 0f)
        {
            Enemy closestTarget = null;
            float distance = Mathf.Infinity;
            foreach (var enemy in enemyManager.EnemyList)
            {
                Vector3 dist = enemy.Position - transform.position;
                if (dist.sqrMagnitude < distance)
                {
                    closestTarget = enemy;
                    distance = dist.sqrMagnitude;
                }
            }
            if (Vector3.Distance(transform.position, new Vector3(closestTarget.Position.x, transform.position.y, closestTarget.Position.z)) < towerData.Vision)
            {
                target = closestTarget;
            }
            else
            {
                target = null;
            }

            scanTimer = towerData.ScanInterval;
        }
    }

    protected virtual void Track()
    {
        if (target == null) { return; }
        if (Vector3.Distance(transform.position, new Vector3(target.Position.x, transform.position.y, target.Position.z)) > towerData.Vision)
        {
            target = null;
            return;
        }

        transform.LookAt(new Vector3(target.Position.x, transform.position.y, target.Position.z));
    }

    protected virtual bool Shoot()
    {
        if (target == null) { return false; }
        if (shootTimer > 0f) { return false; }
        if (Vector3.Distance(transform.position, new Vector3(target.Position.x, transform.position.y, target.Position.z)) > towerData.Range) { return false; }

        shootTimer = towerData.Cooldown;
        return true;
    }
}