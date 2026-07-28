using UnityEngine;

public class EnemyPrebuilt : EnemyView
{
    [SerializeField] private EnemyDefinition enemyDef;
    [SerializeField] private Waypoint waypoint;
    public void Start()
    {
        if (enemyDef != null)
        {
            FreeEnemy enemy = new FreeEnemy(enemyDef, waypoint);
            base.Init(enemy);

            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.Add(enemy);
            }
        }
    }
}