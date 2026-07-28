using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Cards
{
    public enum CardPilePosition
    {
        Top,
        Bottom
    }

    public class CardPile<T> where T: class
    {

        #region CONFIGURATION
        // Rules
        public bool ruleShouldShuffleOnInsert = false;
        public bool ruleShouldShuffleOnTutor = false;
        public bool ruleHasLimitPileSize = false;

        // Limits
        public int limitPileSize = 0;

        // Flags
        public bool flagThrow = false;
        #endregion

        public string PileName { get; private set; }
        public CardManager<T> Parent { get; private set; }

        readonly Random random = new Random();
        // 0 is the top of the pile, Cards.Count - 1 is the bottom
        readonly List<T> Cards = new List<T>();

        public int Count => Cards.Count;
        public T[] ViewPile() => Cards.ToArray();

        public bool IsEmpty => Cards.Count == 0;
        public int PickRandomIndex => random.Next(0, Cards.Count);
        bool IsIndexOutOfRange(int index) => index < 0 || index >= Cards.Count;

        public CardPile(CardManager<T> parent, string pileName)
        {
            Parent = parent;
            PileName = pileName;
        }

        T Peek(int index)
        {
            if (IsEmpty) return null;
            if (IsIndexOutOfRange(index))
            {
                if (flagThrow) throw new ArgumentOutOfRangeException("Index is out of range");
                return null;
            }
            return Cards[index];
        }

        T[] Peek(int[] indices)
        {
            if (IsEmpty) return null;
            return indices.Select(i => Peek(i)).OfType<T>().ToArray();
        }

        T PeekByCard(T card)
        {
            if (IsEmpty) return default;
            return Cards.Find(c => c.Equals(card));
        }

        T[] PeekByCard(T[] cards)
        {
            if (IsEmpty) return null;
            return Cards.FindAll(c => cards.Contains(c)).OfType<T>().ToArray();
        }

        T[] PeekByName(string name)
        {
            if (IsEmpty) return null;
            return Cards.FindAll(c => c.ToString() == name).OfType<T>().ToArray();
        }

        void Push(T card, int index = 0)
        {
            if (card == null)
            {
                if (flagThrow) throw new ArgumentNullException("There is no card to add to the pile");
                return;
            }
            if (IsEmpty)
            {
                Cards.Add(card);
                return;
            }
            if (ruleHasLimitPileSize && Cards.Count >= limitPileSize)
            {
                if (flagThrow) throw new ArgumentOutOfRangeException("Pile is full");
                return;
            }
            if (IsIndexOutOfRange(index)) {
                if (index == Cards.Count) Cards.Add(card);
                if (flagThrow) throw new ArgumentOutOfRangeException("Index is out of range");
                return;
            }
            Cards.Insert(index, card);
        }

        T Pop(T card)
        {
            if (IsEmpty)
            {
                if (flagThrow) throw new ArgumentNullException("There are no cards in the pile");
                return null;
            }
            if (PeekByCard(card) == null)
            {
                if (flagThrow) throw new ArgumentNullException($"This {card} does not exist in the pile");
                return null;
            }
            Cards.Remove(card);
            return card;
        }

        public CardSelection<T> QueryPile()
        {
            return new CardSelection<T>(this, Cards.ToArray());
        }

        public CardSelection<T> QueryCard(T card)
        {
            T output = PeekByCard(card);
            return new CardSelection<T>(this, output == null ? new T[] { } : new T[] { output });
        }

        public CardSelection<T> QueryCard(T[] cards)
        {
            return new CardSelection<T>(this, PeekByCard(cards));
        }

        public CardSelection<T> QueryCardsFrom(CardPilePosition position, int quantity)
        {
            switch (position)
            {
                case CardPilePosition.Top:
                    return new CardSelection<T>(this, Peek(Enumerable.Range(0, quantity).ToArray()));
                case CardPilePosition.Bottom:
                    return new CardSelection<T>(this, Peek(Enumerable.Range(Cards.Count - quantity, quantity).Reverse().ToArray()));
                default:
                    return new CardSelection<T>(this, Peek(Enumerable.Range(0, quantity).ToArray()));
            }
        }

        public CardSelection<T> QueryRandomCards(int quantity)
        {
            if (IsEmpty) return new CardSelection<T>(this, Enumerable.Empty<T>().ToArray());

            // Select random unique elements from the list
            T[] output = Enumerable.Range(0, quantity)
                .Select(i => random.Next(0, 1 + Cards.Count - quantity))
                .OrderBy(i => i)
                .Select((x, i) => Cards[x + i])
                .ToArray();

            return new CardSelection<T>(this, output);
        }

        public CardSelection<T> QueryCardsByName(string name)
        {
            return new CardSelection<T>(this, PeekByName(name));
        }

        public void InsertCard(T card, int index = 0) {
            Push(card, index);
        }

        public void PlaceCard(T card, CardPilePosition position)
        {
            int index = position == CardPilePosition.Top ? 0 : Cards.Count;
            InsertCard(card, index);
        }

        public CardSelection<T> DrawFromTopPile(int quantity = 1)
        {
            return new CardSelection<T>(this,
                Enumerable.Range(0, quantity)
                .Select(i => Pop(Peek(0))).ToArray()
            );
        }

        public CardSelection<T> DrawSpecificCard(T card)
        {
            T output = Pop(PeekByCard(card));
            return new CardSelection<T>(this, output == null ? new T[] { } : new T[] { output });
        }

        public CardSelection<T> DrawSpecificCard(T[] cards)
        {
            return new CardSelection<T>(this,
                cards.Select(c => Pop(c)).ToArray()
            );
        }

        public void Shuffle()
        {
            int n = Cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(0, n); ;
                (Cards[k], Cards[n]) = (Cards[n], Cards[k]);
            }
        }

        public void Flip()
        {
            Cards.Reverse();
        }

        public void ClearPile()
        {
            Cards.Clear();
        }
    }
}
