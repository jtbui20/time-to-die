using UnityEngine;

public class BombPrebuilt : BombView
{
    [SerializeField] private BombDefinition bombDef;
    public void Start()
    {
        if (bombDef != null)
        {
            FreeBomb bomb = new FreeBomb(bombDef);
            base.Init(bomb);

            if (BombManager.Instance != null)
            {
                BombManager.Instance.Add(bomb);
            }
        }
    }
}