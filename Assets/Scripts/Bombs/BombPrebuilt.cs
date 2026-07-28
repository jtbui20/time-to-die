using UnityEngine;

public class BombPrebuilt : BombView
{
    [SerializeField] private BombDefinition bombDef;
    private FreeBomb targetReference;
    public void Start()
    {
        if (targetReference != null)
        {
            base.Init(targetReference);
        } else if (bombDef != null)
        {
            FreeBomb bomb = new FreeBomb(bombDef);
            base.Init(bomb);

            if (BombManager.Instance != null)
            {
                BombManager.Instance.Add(bomb);
            }
        }
    }

    public void ExternalSetup(FreeBomb bomb)
    {
        targetReference = bomb;
    }
}