using UnityEngine;

public class BombPrebuilt : BombView
{
    [SerializeField] private BombDefinition bombDef;
    public void Start()
    {
        if (bombDef != null)
        {
            FreeBomb bomb = new FreeBomb(bombDef);
            BombManager.Instance.Add(bomb);
            base.Init(bomb);
        }
    }
}