using System.Collections.Generic;
using Poker.Model;

namespace Poker.Interfaces
{
    public interface IPokerTieBreaker
    {
        int[] Get(Dictionary<int, Card[]> hands, out Card winningCard);
    }
}