using System.Linq;
using NUnit.Framework;
using Poker.Extensions;
using Poker.Model;


namespace Poker
{
    [TestFixture]
    public class PokerHandTest
    {
        [TestCase("3C 4D 6C 9H 10H", "3S 4S 6S 9S 2H", "Batman wins - HighCard: 10H")]
        [TestCase("2C 2S 6D 6H 9H", "3C 3S 7D 7H 9D", "Superman wins - TwoPairs: 7H")]
        [TestCase("3C 4C 5C 6C 7C", "2C 2S 2D 2H 5D", "Batman wins - StraightFlush")]
        [TestCase("6D 9H 2S 6H 2C", "3C 5D 7H 3S 7D", "Superman wins - TwoPairs: 7H")]
        [TestCase("7D 9H 3S 7H 3C", "3C 9D 7S 3S 7C", "Tie")]
        [TestCase("2H 3D 5S 9C KD", "2D 3H 5C 9S KH", "Tie")]
        public void GetWinner(string batman, string superman, string expected)
        {
            var answer = PokerHand.Play(batman,superman);
            Assert.AreEqual(expected, answer);
        }

        [TestCase("JC KC AC QC 10C", "10C JC QC KC AC")]
        [TestCase("KS JC 6C 9D JH", "6C 9D JC JH KS")]
        public void Should_Return_Sorted_Cards(string cards, string expected)
        {
            var splitCards = cards.ToCardArray();
            var answer = splitCards.Sort().ToNameString();
            Assert.AreEqual(expected, answer);
        }

        [TestCase("10C JC QC KC AC", PokerRankingOrder.RoyalFlush)]
        [TestCase("10D JD QD KD AD", PokerRankingOrder.RoyalFlush)]
        [TestCase("10H JH QH KH AH", PokerRankingOrder.RoyalFlush)]
        [TestCase("10S JS QS KS AS", PokerRankingOrder.RoyalFlush)]
        [TestCase("9H 10H JH QH KH", PokerRankingOrder.StraightFlush)]
        [TestCase("2S 3S 4S 5S 6S", PokerRankingOrder.StraightFlush)]
        [TestCase("3C 3S 3D 3H 5D", PokerRankingOrder.FourOfAKind)]
        [TestCase("KC KS KD KH AD", PokerRankingOrder.FourOfAKind)]
        [TestCase("3C 3S 3D 5H 5D", PokerRankingOrder.FullHouse)]
        [TestCase("KC KS KD AH AD", PokerRankingOrder.FullHouse)]
        [TestCase("3C 7C 8C JC KC", PokerRankingOrder.Flush)]
        [TestCase("2S 4S 7S 10S QS", PokerRankingOrder.Flush)]
        [TestCase("9C 10D JH QH KS", PokerRankingOrder.Straight)]
        [TestCase("2S 3D 4S 5H 6S", PokerRankingOrder.Straight)]
        [TestCase("9C 9D 9H QH KS", PokerRankingOrder.ThreeOfAKind)]
        [TestCase("3S 3D 3S 5H 6S", PokerRankingOrder.ThreeOfAKind)]
        [TestCase("9C 9D QC QH KS", PokerRankingOrder.TwoPairs)]
        [TestCase("3S 3D 5S 5H 6S", PokerRankingOrder.TwoPairs)]
        [TestCase("9C 9D JC QH KS", PokerRankingOrder.Pair)]
        [TestCase("2S 3D 5S 5H 6S", PokerRankingOrder.Pair)]
        [TestCase("6C 9D JC QH KS", PokerRankingOrder.HighCard)]
        [TestCase("4S 5D 6S 7H AS", PokerRankingOrder.HighCard)]
        public void Should_Get_CorrectPattern(string cards, PokerRankingOrder expected)
        {
            var answer = PokerHand.GetPattern(cards);
            Assert.AreEqual(expected, answer.RankingOrder);
        }
    }
}
