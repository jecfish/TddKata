using System.Collections.Generic;
using System.Linq;
using Poker.Model;

namespace Poker.Extensions
{
    public static class CardExtention
    {
        public static Card[] Sort(this Card[] cards)
        {
            return cards.OrderBy(x => x.Score).ToArray();
        }

        public static Card[] SortDescending(this IEnumerable<Card> cards)
        {
            return cards.OrderByDescending(x => x.Score).ToArray();
        }

        public static int SuitCount(this IEnumerable<Card> cards)
        {
            return cards.Select(x => x.Suit).Distinct().Count();
        }

        public static string ToNameString(this IEnumerable<Card> cards)
        {
            return string.Join(" ", cards.Select(x => x.Name).ToList());
        }

        public static Card[] ToCardArray(this string cardString)
        {
            return cardString.ToCardArray(' ');
        }

        public static Card[] ToCardArray(this string cardString, char seperator)
        {
            return cardString.Split(seperator).Select(x => new Card(x)).ToArray();
        }

        public static Dictionary<int, int> GroupByDenoteValue(this IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.DenoteValue)
                .Select(group => new { DenoteValue = group.Key, Count = group.Count() })
                .ToDictionary(t => t.DenoteValue, t => t.Count);
        }
    }
}