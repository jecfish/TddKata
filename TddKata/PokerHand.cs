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
                player.Key.Hand = player.Value.ToCardArray();
                var rankPatter = GetPattern(player.Key.Hand);
                player.Key.Rank = rankPatter.RankingOrder;
                player.Key.WinningCards = rankPatter.GetWinningCards();
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

        public static IPokerRank GetPattern(Card[] cards)
        {
            var patternArr = new IPokerRank[]
            {
                new RankRoyalFlush(cards), 
                new RankStraightFlush(cards), 
                new RankFourOfAKind(cards), 
                new RankFullHouse(cards),
                new RankFlush(cards),
                new RankStraight(cards), 
                new RankThreeOfAKind(cards),
                new RankTwoPairs(cards), 
                new RankPairs(cards), 
                new RankHighCard(cards) 
            };

            foreach (var pattern in patternArr.Where(pattern => pattern.Get()))
            {
                return pattern;
            }

            throw new Exception("No pattern found.");
        }

        public static IPokerRank GetPattern(string cards)
        {
            var result = GetPattern(cards.ToCardArray());
            return result;
        }
    }
}
