using System;
using System.Collections.Generic;

namespace GameFramework.Cards
{
    /// <summary>
    /// A card manager that uses a three pile system, with a deck, hand, and discard pile.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreePileTemplateCardManager<T> : CardManager<T> where T : class
    {
        // TODO: REWORK SENDCARDS TO ALLOW PILE SPECIFIC BEHAVIOUR
        //       IMPLEMENT PILE SPECIFIC BEHAVIOURS

        #region CONFIGURATION
        /// <summary>
        /// Determines if the discard pile should be returned to the deck after the deck is empty when trying to draw, and allow further drawing
        /// </summary>
        public bool PullExcessive = false;

        /// <summary>
        /// Determines if the deck should be shuffled on certain actions.
        /// </summary>
        public bool ShouldDeckShuffleAfterChange = false;

        #endregion

        public enum CardLocation
        {
            Deck,
            Hand,
            Discard
        }

        public CardPile<T> PileDeck => GetPile(CardLocation.Deck);
        public CardPile<T> PileHand => GetPile(CardLocation.Hand);
        public CardPile<T> PileDiscard => GetPile(CardLocation.Discard);


        public ThreePileTemplateCardManager()
        {
            // Generate piles based on enum
            GeneratePiles(System.Enum.GetNames(typeof(CardLocation)));
        }

        public override void Reset()
        {
            base.Reset();
            GeneratePiles(System.Enum.GetNames(typeof(CardLocation)));
            InvokeUpdate();
        }

        public CardPile<T> GetPile(CardLocation location) => GetPile(location.ToString());

        public void LoadDeck(List<T> Deck)
        {
            foreach (T card in Deck) PileDeck.InsertCard(card);

            InvokeUpdate();
        }

        CardSelection<T> SendCards(CardLocation source, CardLocation target, int quantity = -1)
        {
            CardSelection<T> output = SendCards(source.ToString(), target.ToString(), quantity);

            InvokeUpdate();

            return output;
        }

        /// <summary>
        /// [DEPRECATED] Use source.DrawSpecificCard().MoveTo(target) instead
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="cards"></param>
        /// <returns></returns>
        CardSelection<T> SendCards(CardLocation source, CardLocation target, T[] cards)
        {
            CardSelection<T> output = SendCards(source.ToString(), target.ToString(), cards);
            InvokeUpdate();

            return output;
        }

        #region General Commands

        /// <summary>
        /// Take a card from the deck and add to hand
        /// </summary>
        /// <param name="quantity"></param>
        public CardSelection<T> Draw(int quantity = 1)
        {
            List<T> selection = new List<T>();
            for (int i = 0; i < quantity; i++)
            {
                if (PileDeck.Count == 0)
                {
                    if (PullExcessive) RecycleAll();
                    else break;
                }

                CardSelection<T> draw = PileDeck.DrawFromTopPile();

                selection.Add(draw.selection[0]);
                draw.MoveTo(PileHand);
            }

            InvokeUpdate();

            return new CardSelection<T>(PileDeck, selection.ToArray(), readOnly: true);
        }

        /// <summary>
        /// Take a card from the hand and add to discard
        /// </summary>
        /// <param name="card"></param>
        public CardSelection<T> Discard(T card)
        {
            CardSelection<T> output = PileHand
                .DrawSpecificCard(card)
                .MoveTo(PileDiscard);

            InvokeUpdate();

            return output;
        }

        /// <summary>
        /// Take a set of cards from the hand and add to discard
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>

        public CardSelection<T> Discard(CardSelection<T> cards)
        {
            CardSelection<T> output = PileHand.DrawSpecificCard(cards).MoveTo(PileDiscard);

            InvokeUpdate();

            return output;
        }

        /// <summary>
        /// Takes a specific card from the deck and adds to hand
        /// </summary>
        /// <param name="card"></param>
        public CardSelection<T> Search(T card)
        {
            var selection = PileDeck
                    .DrawSpecificCard(card)
                    .MoveTo(PileHand);

            if (ShouldDeckShuffleAfterChange) PileDeck.Shuffle();

            InvokeUpdate();

            return selection;
        }

        public void DiscardAllHand()
        {
            SendCards(CardLocation.Hand, CardLocation.Discard);

            InvokeUpdate();
        }

        /// <summary>
        /// Take a card from the discard and add to deck
        /// </summary>
        /// <param name="card"></param>
        public CardSelection<T> Recycle(T card)
        {
            var selection = PileDiscard
                    .DrawSpecificCard(card)
                    .MoveTo(PileDeck);

            if (ShouldDeckShuffleAfterChange) PileDeck.Shuffle();

            InvokeUpdate();

            return selection;
        }

        public CardSelection<T> Recycle(T[] cards)
        {
            var selection = PileDiscard
                    .DrawSpecificCard(cards)
                    .MoveTo(PileDeck);

            if (ShouldDeckShuffleAfterChange) PileDeck.Shuffle();

            InvokeUpdate();

            return selection;
        }

        /// <summary>
        /// Send all cards from the discard pile to the deck
        /// </summary>
        public void RecycleAll()
        {
            SendCards(CardLocation.Discard, CardLocation.Deck);
            if (ShouldDeckShuffleAfterChange) PileDeck.Shuffle();

            InvokeUpdate();
        }

        public void Mill(int quantity = 1)
        {
            SendCards(CardLocation.Deck, CardLocation.Discard, quantity);
            InvokeUpdate();
        }

        /// <summary>
        /// Creates a new card and adds directly to hand
        /// </summary>
        /// <param name="card"></param>
        public void GenerateToPile(string pileName, T card)
        {
            GetPile(pileName).InsertCard(card);
            InvokeUpdate();
        }

        public CardSelection<T> ReturnToDeck(T card)
        {
            var selection = PileHand
                    .DrawSpecificCard(card)
                    .MoveTo(PileDeck);

            if (ShouldDeckShuffleAfterChange) PileDeck.Shuffle();

            InvokeUpdate();
            return selection;
        }

        public void ReturnEverythingToDeck()
        {
            RecycleAll();
            SendCards(CardLocation.Hand, CardLocation.Deck);
            InvokeUpdate();
        }

        /// <summary>
        /// Take a card from the hand and add to deck. Just so we can bundle InvokeUpdate()
        /// </summary>
        /// <param name="source"></param>
        /// <param name="card"></param>
        public CardSelection<T> TakeFromPile(string source, T card)
        {
            var output = GetPile(source).DrawSpecificCard(card);
            InvokeUpdate();
            return output;
        }

        #endregion
    }
}