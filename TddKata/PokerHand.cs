using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Poker.Extensions;
using Poker.Interfaces;
using Poker.Model;

namespace Poker
{
    public class PokerHand
    {
        public static string Play(string batman, string superman)
        {
            //TODO: Refactor this method, so ugly
            var players = new Dictionary<Player, string>()
            {
                { new Player("Batman"), batman }, 
                { new Player("Superman"), superman }, 
            };

            foreach (var player in players)
            {
                Card[] winningCards;
                player.Key.Hand = player.Value.ToCardArray();
                player.Key.Rank = GetPattern(player.Key.Hand, out winningCards);
                player.Key.WinningCards = winningCards;
            }

            var highestRank = players.Keys.Max(x => x.Rank);
            var winners = players.Keys.Where(x => x.Rank == highestRank).ToArray();
            string result;

            if (winners.Count() == 1)
            {
                result = string.Format("{0} wins - {1}", winners.First().Name, winners.First().Rank);
            }
            else if (winners.First().WinningCards == null)
            {
                result = "Tie";
            }
            else
            {
                IPokerTieBreaker tieBreaker = new PokerTieBreaker();
                Card finalCard;
                var final = tieBreaker.Get(winners
                    .Select((w, index) => new{ index, w.WinningCards})
                    .ToDictionary(t => t.index, t => t.WinningCards), out finalCard);

                result = final.Count() > 1
                    ? "Tie"
                    : string.Format("{0} wins - {1}: {2}",
                        winners[final[0]].Name,
                        winners[final[0]].Rank,
                        finalCard.Name);
            }

            return result;
        }

        public static PokerRankingOrder GetPattern(Card[] cards, out Card[] winningCards)
        {
            var patternArr = new Dictionary<PokerRankingOrder, IPokerRank>()
            {
                { PokerRankingOrder.RoyalFlush,  new RankRoyalFlush(cards) },
                { PokerRankingOrder.StraightFlush, new RankStraightFlush(cards) },
                { PokerRankingOrder.FourOfAKind, new RankFourOfAKind(cards) },
                { PokerRankingOrder.FullHouse, new RankFullHouse(cards) },
                { PokerRankingOrder.Flush, new RankFlush(cards) },
                { PokerRankingOrder.Straight, new RankStraight(cards) },
                { PokerRankingOrder.ThreeOfAKind, new RankThreeOfAKind(cards) },
                { PokerRankingOrder.TwoPairs, new RankTwoPairs(cards) },
                { PokerRankingOrder.Pair, new RankPairs(cards) },
                { PokerRankingOrder.HighCard, new RankHighCard(cards) },
            };

            foreach (var pattern in patternArr.Where(pattern => pattern.Value.Get()))
            {
                winningCards = pattern.Value.GetWinningCards();
                return pattern.Key;
            }

            throw new Exception("No pattern found.");
        }

        public static PokerRankingOrder GetPattern(string cards, out Card[] winningCards)
        {
            var result = GetPattern(cards.ToCardArray(), out winningCards);
            return result;
        }
    }
}
