using System;
using System.Collections.Generic;

namespace Poker.Model
{
    public class Card
    {
        public string Name { get; set; }
        public string Suit { get; private set; }
        public int DenoteValue { get; private set; }
        public float Score { get; private set; }
        private string Denote { get; set; }
        private float SuitValue { get; set; }

        private enum Suits { C = 1, D = 2, H = 3, S = 4 }; //Clubs, Diamonds, Hearts, Spades

        private readonly Dictionary<string, int> _cardValues = new Dictionary<string, int>()
        {
            {"1", 1}, {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5},
            {"6", 6}, {"7", 7}, {"8", 8},  {"9", 9}, {"10", 10},
            {"J", 11}, {"Q", 12}, {"K", 13}, {"A", 14},
        };

        public Card(string name)
        {
            Name = name; //AC
            Denote = name.Substring(0, name.Length - 1); //A
            Suit = name.Remove(0, name.Length - 1); //C

            DenoteValue = _cardValues[Denote]; //A = 14
            SuitValue = (float) ((int)Enum.Parse(typeof(Suits), Suit) * 0.01); //C = 0.01
            Score = DenoteValue + SuitValue; //14 + 0.01
        }
    }
}