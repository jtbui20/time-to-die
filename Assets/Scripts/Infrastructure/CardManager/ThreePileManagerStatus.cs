namespace GameFramework.Cards
{
    public readonly struct ThreePileManagerStatus<T> where T : class
    {
        public int DeckCount { get; }
        public int HandCount { get; }
        public int DiscardCount { get; }

        public T[] Deck { get; }

        public T[] Hand { get; }

        public T[] Discard { get; }

        public ThreePileManagerStatus(ThreePileTemplateCardManager<T> manager)
        {
            DeckCount = manager.PileDeck.Count;
            HandCount = manager.PileHand.Count;
            DiscardCount = manager.PileDiscard.Count;
            Deck = manager.PileDeck.ViewPile();
            Hand = manager.PileHand.ViewPile();
            Discard = manager.PileDiscard.ViewPile();
        }
    }
}