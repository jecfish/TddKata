using System.Collections.Generic;
using System.Linq;
using Poker.Interfaces;
using Poker.Model;

namespace Poker
{
    /* No rank of suits: Implementation based on http://www.pokerhands.com/poker_hand_tie_rules.html */
    public class PokerTieBreaker : IPokerTieBreaker
    {
        protected virtual int[] GetMaxValue(Dictionary<int, Card> cards)
        {
            var max = cards.Max(x => x.Value.DenoteValue);

            var indexWithMax = cards.Where(c => c.Value.DenoteValue == max)
                .Select(c => c.Key).ToArray();

            return indexWithMax;
        }
        
        public virtual int[] Get(Dictionary<int, Card[]> hands, out Card winningCard)// key = player, value = card
        {
            var loop = hands.First().Value.Length; //assuming each hand length are the same

            for (var i=0; i < loop; i++)
            {
                var winners = GetMaxValue(hands.ToDictionary(t => t.Key, t => t.Value.First()));
                if (winners.Length == 1)
                {
                    winningCard = hands[winners[0]][i];
                    return winners;
                }
                hands = hands.Where(x => winners.Contains(x.Key)).ToDictionary(t => t.Key, t => t.Value);
                loop --;
            }

            winningCard = null;
            return hands.Keys.ToArray();
        }
    }
}