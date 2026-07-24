using UnityEngine;

public class EnemyPrebuilt : EnemyView
{
    [SerializeField] private EnemyDefinition enemyDef;
    public void Start()
    {
        if (enemyDef != null)
        {
            FreeEnemy enemy = new FreeEnemy(enemyDef);
            base.Init(enemy);

            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.Add(enemy);
            }
        }
    }
}