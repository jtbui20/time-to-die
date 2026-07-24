using UnityEngine;
using TMPro;

public class EnemyView : UnitView
{
    public FreeEnemy Enemy { get; private set; }
    public override IDamageable Source { get { return Enemy; } }

    public void Init(FreeEnemy enemy)
    {
        if (enemy == null) { return; }

        Enemy = enemy;

        base.Init(enemy);
    }

    protected override void UpdateView()
    {
        // health view goes here
    }
}