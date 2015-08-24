using Poker.Model;

namespace Poker.Interfaces
{
    public interface IPokerRank
    {
        Card[] Cards { get; }
        bool Get();
        Card[] GetWinningCards();
    }
}