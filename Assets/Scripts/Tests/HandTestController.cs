using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.UI;
using GameFramework.Cards;
using UnityEngine;

namespace Tests
{
    public class HandTestController : MonoBehaviour
    {
        public List<BombDefinition> InitialDeck;
        
        public ThreePileTemplateCardManager<FreeBomb> BombDeck;

        public RackController rack;

        private void Start()
        {
            SetupDeck();
        }

        void SetupDeck()
        {
            BombDeck = new ThreePileTemplateCardManager<FreeBomb>();
            BombDeck.PullExcessive = true;
            BombDeck.ShouldDeckShuffleAfterChange = true;
            List<FreeBomb> bombs = new List<FreeBomb>();
            
            foreach (BombDefinition bombDef in InitialDeck)
            {
                bombs.Add(new FreeBomb(bombDef));
            }
            BombDeck.LoadDeck(bombs);
            
            BombDeck.PileDeck.Shuffle();
        }

        public void DrawHand()
        {
            int drawCount = 5 - BombDeck.PileHand.Count;
            
            BombDeck.Draw(drawCount);
            
            rack.LoadInNewBombs(BombDeck.PileHand.ViewPile().ToList());
        }


        public void DiscardHand()
        {
            BombDeck.DiscardAllHand();
            rack.HandleDiscard();
        }
    }
}