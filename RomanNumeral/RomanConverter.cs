using System;
using System.Linq;

namespace RomanNumeral
{
    public class RomanConverter
    {
        #region Declaration
        //Roman characters
        private const string ROMAN_1 = "I";
        private const string ROMAN_5 = "V";
        private const string ROMAN_10 = "X";
        private const string ROMAN_50 = "L";
        private const string ROMAN_100 = "C";
        private const string ROMAN_500 = "D";
        private const string ROMAN_1000 = "M";

        //Arabic numbers
        private const int ONE = 1;
        private const int FIVE = 5;
        private const int TEN = 10;
        private const int HUNDRED = 100;
        private const int THOUSAND = 1000;

        //Errors
        public const string ErrInvalidInput = "Invalid input";
        public const string ErrOutOfRange = "Number must be between 1 - 3999.";
        #endregion

        public static string Convert(int number)
        {
            try
            {
                Validate(number);

                var result = "";
                var remainder = number;

                while (remainder >= TEN)
                {
                    var divider = remainder >= THOUSAND ? THOUSAND: 
                        remainder >= HUNDRED ? HUNDRED:
                        remainder >= TEN? TEN: ONE;

                    result = string.Concat(result, GetRomanNumeral(remainder / divider * divider));

                    remainder %= divider;
                }

                result = string.Concat(result, GetRomanNumeral(remainder));

                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ErrInvalidInput,ex);
            }
        }

        public static bool Validate(int number)
        {
            const int minValue = 1;
            const int maxValue = 3999;

            if (number < minValue || number > maxValue)
            {
                throw new ArgumentOutOfRangeException(ErrOutOfRange);
            }

            return true;
        }

        #region Private functions - GetRomanNumeral

        private static string GetRomanNumeral(int number)
        {
            string result;

            if (number >= THOUSAND)
            {
                result = GetRomanNumeralThousands(number);
            }
            else if (number >= HUNDRED)
            {
                result = GetRomanNumeralHundreds(number);
            }
            else if (number >= TEN)
            {
                result = GetRomanNumeralTens(number);
            }
            else
            {
                result = GetRomanNumeralOnes(number);
            }

            return result;
        }

        private static string GetRomanNumeral(int multiplier, int number, string first, string fifth, string tenth)
        {
            string result;

            if (number <= 3 * multiplier)
            {
                result = GetRomanNumeralLessOrEqualToThree(multiplier, number, first);
            }
            else if (number == 4 * multiplier)
            {
                result = string.Concat(first, fifth);
            }
            else if (number == 5 * multiplier)
            {
                result = fifth;
            }
            else if (number <= 8 * multiplier)
            {
                result = fifth + string.Concat(Enumerable.Repeat(first, (number - (FIVE * multiplier)) / multiplier));
            }
            else //9
            {
                result = string.Concat(first, tenth);
            }

            return result;
        }

        private static string GetRomanNumeralLessOrEqualToThree(int multiplier, int number, string first)
        {
            string result;

            if (number != 0 && number <= 3 * multiplier)
            {
                result = string.Concat(Enumerable.Repeat(first, (number / multiplier)));
            }
            else
            {
                result = "";
            }

            return result;
        } 

        private static string GetRomanNumeralOnes(int number)
        {
            return GetRomanNumeral(ONE, number, ROMAN_1, ROMAN_5, ROMAN_10);
        }

        private static string GetRomanNumeralTens(int number)
        {
            return GetRomanNumeral(TEN, number, ROMAN_10, ROMAN_50, ROMAN_100);
        }

        private static string GetRomanNumeralHundreds(int number)
        {
            return GetRomanNumeral(HUNDRED, number, ROMAN_100, ROMAN_500, ROMAN_1000);
        }

        private static string GetRomanNumeralThousands(int number)
        {
            return GetRomanNumeralLessOrEqualToThree(THOUSAND, number, ROMAN_1000);
        }

        #endregion
    }
}
