using System;
using NUnit.Framework;

namespace RomanNumeral
{
    [TestFixture]
    public class RomanConverterTest
    {
        //ones
        [TestCase(1, "I")]
        [TestCase(4, "IV")]
        [TestCase(5, "V")]
        [TestCase(7, "VII")]
        [TestCase(9, "IX")]
        //tens
        [TestCase(10, "X")]
        [TestCase(40, "XL")]
        [TestCase(50, "L")]
        [TestCase(70, "LXX")]
        [TestCase(90, "XC")]
        //hundreds
        [TestCase(100, "C")]
        [TestCase(400, "CD")]
        [TestCase(500, "D")]
        [TestCase(700, "DCC")]
        [TestCase(900, "CM")]
        //thousands
        [TestCase(1000, "M")]
        [TestCase(2000, "MM")]
        [TestCase(3000, "MMM")]
        //random
        [TestCase(75, "LXXV")]
        [TestCase(268, "CCLXVIII")]
        [TestCase(1230, "MCCXXX")]
        [TestCase(2790, "MMDCCXC")]
        [TestCase(3999, "MMMCMXCIX")]
        public void Should_Get_RomanNumeral_Answer(int number, string expected)
        {
            var answer = RomanConverter.Convert(number);
            Assert.AreEqual(expected, answer);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(8888)]
        public void Should_ThrowError_InvalidNumber(int number)
        {
            var expected = new Exception(RomanConverter.ErrInvalidInput);
            Assert.Throws(Is.TypeOf<Exception>().And.Message.EqualTo(expected.Message), delegate
            {
                RomanConverter.Convert(number);
            });
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(4234)]
        [TestCase(4000)]
        public void Should_ThrowError_Number_LessThan1_MoreThan3999(int number)
        {
            var expected = new ArgumentOutOfRangeException(RomanConverter.ErrOutOfRange);
            Assert.Throws(Is.TypeOf<ArgumentOutOfRangeException>().And.Message.EqualTo(expected.Message), delegate
            {
                RomanConverter.Validate(number);
            });
        }

        [TestCase(6)]
        [TestCase(38)]
        [TestCase(999)]
        [TestCase(2356)]
        public void Should_Return_True_Number_Between_1_To_3999(int number)
        {
            var answer = RomanConverter.Validate(number);
            Assert.IsTrue(answer);
        }
    }
}
