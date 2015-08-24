namespace Poker.Model
{
    public class Player
    {
        public string Name { get; private set; }
        public Card[] Hand { get; set; }
        public PokerRankingOrder Rank { get; set; }
        public Card[] WinningCards { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public Player(string name, Card[] hand)
        {
            Name = name;
            Hand = hand;
        }
    }
}