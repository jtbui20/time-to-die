using UnityEngine;

namespace DefaultNamespace.Data
{
    [CreateAssetMenu(fileName = "BombSpriteBindings", menuName = "Game/Bomb Sprite Bindings", order = 0)]
    public class BombSpriteBindings : ScriptableObject
    {
        public Sprite BasicBomb;
        public Sprite ChainBomb;
        public Sprite Molotov;
        public Sprite IceBomb;
        public Sprite TNT;
        public Sprite Instant;
        
        public Sprite GetPrefab(BombType type)
        {
            switch (type)
            {
                case BombType.Standard:
                    return BasicBomb;
                case BombType.Chain:
                    return ChainBomb;
                case BombType.Molotov:
                    return Molotov;
                case BombType.Ice:
                    return IceBomb;
                case BombType.TNT:
                    return TNT;
                case BombType.Instant:
                    return Instant;
                default:
                    return null;
            }
        }
    }
}