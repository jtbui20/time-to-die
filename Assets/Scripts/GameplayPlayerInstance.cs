using System.Collections.Generic;
using GameFramework.Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameplayPlayerInstance : MonoBehaviour
    {
        public ThreePileTemplateCardManager<BombDefinition> BombDeck;

        [SerializeField] private PlayerData _playerDataReference;
        public int MaxHandSize = 5;

        public void Awake()
        {
            BombDeck = new ThreePileTemplateCardManager<BombDefinition>();
        }

        public void Inject(PlayerData playerData)
        {
            _playerDataReference = playerData;
        }

        public void InitializePlayer()
        {
            // Create bombs and load them into the deck
            BombDeck.LoadDeck(_playerDataReference.BombBagReference);
        }
    }
}