using System.Collections.Generic;
using GameFramework.Cards;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameplayPlayerInstance : MonoBehaviour
    {
        public ThreePileTemplateCardManager<FreeBomb> BombDeck;

        [SerializeField] private PlayerData _playerDataReference;
        public int MaxHandSize = 5;

        public void Awake()
        {
            BombDeck = new ThreePileTemplateCardManager<FreeBomb>();
        }

        public void Inject(PlayerData playerData)
        {
            _playerDataReference = playerData;
        }

        public void InitializePlayer()
        {
            // Create bombs and load them into the deck

            List<FreeBomb> bombs = new List<FreeBomb>();
            
            foreach(BombDefinition bombDef in _playerDataReference.BombBagReference)
            {
                FreeBomb bomb = new FreeBomb(bombDef);
                bombs.Add(bomb);
            }
            
            BombDeck.LoadDeck(bombs);
            Debug.Log($"Player initialized with {bombs.Count} bombs.");
        }
    }
}