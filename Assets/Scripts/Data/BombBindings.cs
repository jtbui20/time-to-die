using UnityEngine;

namespace DefaultNamespace.Data
{
    [CreateAssetMenu(fileName = "BombBindings", menuName = "Game/BombBindings", order = 0)]
    public class BombBindings : ScriptableObject
    {
        public GameObject BasicBomb;
        public GameObject ChainBomb;
        public GameObject Molotov;
        public GameObject IceBomb;
        public GameObject TNT;
        public GameObject Instant;

        public GameObject GetPrefab(BombType type)
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