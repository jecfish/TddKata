using Poker.Model;

namespace Poker.Interfaces
{
    public interface IPokerRank
    {
        PokerRankingOrder RankingOrder {get;}
        Card[] Cards { get; }
        bool Get();
        Card[] GetWinningCards();
}
}