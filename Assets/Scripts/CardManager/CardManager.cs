using System.Collections.Generic;

namespace GameFramework.Cards
{
    public class CardManager<T> where T : class
    {
        List<CardPile<T>> Piles;

        protected virtual void InvokeUpdate() { }

        protected void GeneratePiles(string[] pileNames)
        {
            Piles = new List<CardPile<T>>();
            foreach (string pileName in pileNames)
            {
                if (Piles.Find(pile => pile.PileName == pileName) != null) continue;
                Piles.Add(new CardPile<T>(this, pileName));
            }

            InvokeUpdate();
        }

        protected CardSelection<T> SendCards(string source, string target, int quantity = -1)
        {
            CardPile<T> sourcePile = GetPile(source);
            CardPile<T> targetPile = GetPile(target);

            if (quantity == -1) quantity = sourcePile.Count;

            CardSelection<T> selection = sourcePile.DrawFromTopPile(quantity);

            selection.MoveTo(targetPile);

            return selection;
        }

        protected CardSelection<T> SendCards(string source, string target, T[] cards)
        {
            CardPile<T> sourcePile = GetPile(source);
            CardPile<T> targetPile = GetPile(target);

            CardSelection<T> selection = sourcePile.QueryCard(cards);

            selection.MoveTo(targetPile);

            return selection;
        }

        public CardPile<T> GetPile(string pileName)
        {
            return Piles.Find(pile => pile.PileName == pileName);
        }

        public virtual void Reset()
        {
            foreach (CardPile<T> pile in Piles)
            {
                pile.ClearPile();
            }

            Piles.Clear();
        }
    }
}