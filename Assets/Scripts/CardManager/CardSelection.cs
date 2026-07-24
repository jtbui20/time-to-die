using System;

namespace GameFramework.Cards
{
    public class CardSelection<T> where T : class
    {
        readonly CardPile<T> source;
        public T[] selection { get; private set; }
        
        public bool ReadOnly { get; private set; }

        public CardSelection(CardPile<T> source, T[] selection, bool readOnly = false)
        {
            this.source = source;
            this.selection = selection;
        }

        public CardSelection<T> MoveTo(CardPile<T> target)
        {
            if (ReadOnly) throw new Exception("This selection is read-only");
            // Ensure the target is available
            if (source.Parent.GetPile(target.PileName) == null)
            {
                throw new ArgumentException("Pile is not registered in related Card Manager");
            }
            foreach (T card in selection)
            {
                target.InsertCard(card);
            }

            return this;
        }

        public CardSelection<T> RemoveFromSource()
        {
            if (ReadOnly) throw new Exception("This selection is read-only");
            foreach (T card in selection)
            {
                source.DrawSpecificCard(card);
            }

            return this;
        }

        public static implicit operator T[](CardSelection<T> selection) => selection.selection;
    }
}
