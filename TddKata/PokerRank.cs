using System.Linq;
using Poker.Extensions;
using Poker.Interfaces;
using Poker.Model;

namespace Poker
{
    public abstract class PokerRank : IPokerRank
    {
        public Card[] Cards { get; protected set; }

        protected PokerRank(Card[] cards)
        {
            Cards = cards.Sort();
        }

        public abstract bool Get();


        public abstract Card[] GetWinningCards();
    }

    #region Poker Rank Implementation

    public class RankRoyalFlush : PokerRank
    {

        public override bool Get()
        {
            var valueGroup = Cards.GroupBy(x => x.DenoteValue);

            //e.g. 10C JC QC KC AC
            var result = Cards.SuitCount() == 1 && valueGroup.Select(x => x.Key)
                .SequenceEqual(Enumerable.Range(10, Cards.Count()));

            return result;
        }

        public override Card[] GetWinningCards()
        {
            return null;
        }


        public RankRoyalFlush(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankStraightFlush : PokerRank
    {

        public override bool Get()
        {
            //e.g. 3C,4C,5C,6C,7C
            var result = Cards.SuitCount() == 1 &&
                         Cards.Select(x => x.DenoteValue)
                             .SequenceEqual(Enumerable.Range(Cards.First().DenoteValue, Cards.Count()));
            return result;
        }

        public override Card[] GetWinningCards()
        {
            return new[] { Cards.Last() };
        }

        public RankStraightFlush(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankFourOfAKind : PokerRank
    {

        public override bool Get()
        {
            //e.g. 3C,4C,5C,6C,7C
            var result = Cards.GroupByDenoteValue().Count(x => x.Value == 4) == 1;
            return result;
        }

        public override Card[] GetWinningCards()
        {
            return Cards.Where(x => x.DenoteValue == Cards.GroupByDenoteValue().Last(c => c.Value == 4).Key).ToArray();
        }

        public RankFourOfAKind(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankFullHouse : PokerRank
    {

        public override bool Get()
        {
            var valueGroup = Cards.GroupByDenoteValue();

            //e.g. 3C,3S,3D,5H,5D
            var result = valueGroup.Count(x => x.Value == 3) == 1 &&
                         valueGroup.Count(x => x.Value == 2) == 1;
            return result;
        }

        public override Card[] GetWinningCards()
        {
            var valueGroup = Cards.GroupByDenoteValue();
            return Cards.Where(x => x.DenoteValue == valueGroup.First(c => c.Value == 3).Key).ToArray();
        }

        public RankFullHouse(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankFlush : PokerRank
    {

        public override bool Get()
        {
            //e.g. 3C,3S,3D,5H,5D
            var result = Cards.SuitCount() == 1 &&
                !Cards.Select(x => x.DenoteValue).SequenceEqual(Enumerable.Range(Cards.First().DenoteValue, Cards.Count()));
            return result;
        }

        public override Card[] GetWinningCards()
        {
            return Cards.SortDescending();
        }

        public RankFlush(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankStraight : PokerRank
    {

        public override bool Get()
        {
            //e.g. 3C,4S,5D,6H,7D
            var result = Cards.SuitCount() > 1 &&
                Cards.Select(x => x.DenoteValue).SequenceEqual(Enumerable.Range(Cards.First().DenoteValue, Cards.Count()));
            return result;
        }

        public override Card[] GetWinningCards()
        {
            return Cards.SortDescending();
        }

        public RankStraight(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankThreeOfAKind : PokerRank
    {

        public override bool Get()
        {
            var valueGroup = Cards.GroupByDenoteValue();

            //e.g. 3C,3S,3D,5H,9D
            var result = valueGroup.Count(x => x.Value == 3) == 1 && valueGroup.Count(x => x.Value == 2) == 0;
            return result;
        }

        public override Card[] GetWinningCards()
        {
            var valueGroup = Cards.GroupByDenoteValue();
            return new[] { Cards.Last(x => x.DenoteValue == valueGroup.First(c => c.Value == 3).Key) };
        }

        public RankThreeOfAKind(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankTwoPairs : PokerRank
    {

        public override bool Get()
        {
            var valueGroup = Cards.GroupByDenoteValue();

            //e.g. 3C,3S,5D,5H,9D
            var result = valueGroup.Count(x => x.Value == 2) == 2;
            return result;
        }

        public override Card[] GetWinningCards()
        {
            var valueGroup = Cards.GroupByDenoteValue();
            var result = Cards.Where(x => valueGroup
                .Where(c => c.Value == 2).Select(c => c.Key).Contains(x.DenoteValue))
                .SortDescending().ToArray();
            var odd = Cards.Single(x => !result.Contains(x));

            return new[] { result[0], result[2], odd };
        }

        public RankTwoPairs(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankPairs : PokerRank
    {

        public override bool Get()
        {
            var valueGroup = Cards.GroupByDenoteValue();

            //e.g. 3C,3S,5D,7H,9D
            var result = valueGroup.Count(x => x.Value == 2) == 1 && valueGroup.Count(x => x.Value == 3) == 0;
            return result;
        }

        public override Card[] GetWinningCards()
        {
            var valueGroup = Cards.GroupByDenoteValue();
            var result = Cards.Where(x => x.DenoteValue == valueGroup.First(c => c.Value == 2).Key).ToList();
            var odd = Cards.Where(x => !result.Contains(x)).ToList();
            odd.Add(result.Last());
            odd.Reverse();
            return odd.ToArray();
        }

        public RankPairs(Card[] cards)
            : base(cards)
        {
        }
    }

    public class RankHighCard : PokerRank
    {

        public override bool Get()
        {
            var valueGroup = Cards.GroupByDenoteValue();

            //e.g. 3C,4D,6C,9H,10H 
            var result = valueGroup.Count() == 5 && Cards.SuitCount() > 1 &&
                !Cards.Select(x => x.DenoteValue).SequenceEqual(Enumerable.Range(Cards.First().DenoteValue, Cards.Count()));
            return result;
        }

        public override Card[] GetWinningCards()
        {
            return Cards.SortDescending();
        }

        public RankHighCard(Card[] cards)
            : base(cards)
        {
        }
    }

    #endregion
}