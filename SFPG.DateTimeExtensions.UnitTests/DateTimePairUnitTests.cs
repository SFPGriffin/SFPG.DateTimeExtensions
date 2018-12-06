// Copyright S. F. P. Griffin 2018, License: GNU GENERAL PUBLIC LICENSE, Version 3, 29 June 2007.

namespace SFPG.DateTimeExtensions.UnitTests
{
    using System; 
    using FluentAssertions;
    using Xunit;

    public class DateTimePairUnitTests
    {
        [Theory]
        [InlineData(5, 11, 00, 00, 05, 13, 00, 00, "PM")]
        [InlineData(5, 11, 00, 00, 05, 14, 00, 00, "PM")]
        [InlineData(5, 06, 00, 00, 05, 14, 00, 00, "AM")]
        [InlineData(5, 11, 58, 00, 05, 12, 3, 00, "PM")]
        [InlineData(5, 11, 58, 00, 05, 12, 1, 00, "AM")]
        [InlineData(5, 11, 59, 58, 05, 12, 0, 03, "PM")]
        [InlineData(5, 11, 59, 58, 05, 12, 0, 01, "AM")]
        [InlineData(7, 22, 00, 00, 05, 9, 0, 00, "PM")]
        [InlineData(7, 11, 00, 01, 05, 11, 00, 0, "AM")]
        [InlineData(7, 11, 00, 00, 05, 11, 01, 00, "PM")] 
        [InlineData(5, 23, 00, 00, 30, 03, 00, 00, "AM")]
        [InlineData(5, 11, 00, 00, 6, 01, 00, 00, "PM")]
        public void CalculatesAverageMeridian_WorksWithSameMonthAndYear(
            int dayOne, int hourOne, int minuteOne, int secondOne,
            int dayTwo, int hourTwo, int minuteTwo, int secondTwo,
            string expected)
        {
            var dateOne = new DateTime(2018, 11, dayOne, hourOne, minuteOne, secondOne);
            var dateTwo = new DateTime(2018, 11, dayTwo, hourTwo, minuteTwo, secondTwo);
            var pair = new DateTimePair(dateOne, dateTwo);

            pair.CalculateAverageMeridian().Should().Be(expected);
        }

        [Theory]
        [InlineData(5, 11, 00, 00, 05, 14, 00, 00, "PM")]
        [InlineData(5, 06, 00, 00, 05, 14, 00, 00, "AM")]
        public void CalculatesAverageMeridian_WorksDifferentMonths(
            int dayOne, int hourOne, int minuteOne, int secondOne,
            int dayTwo, int hourTwo, int minuteTwo, int secondTwo,
            string expected)
        {
            var dateOne = new DateTime(2018, 11, dayOne, hourOne, minuteOne, secondOne);
            var dateTwo = new DateTime(2018, 12, dayTwo, hourTwo, minuteTwo, secondTwo);
            var pair = new DateTimePair(dateOne, dateTwo);

            pair.CalculateAverageMeridian().Should().Be(expected);
        }

        [Theory]
        [InlineData(5, 11, 00, 00, 05, 14, 00, 00, "PM")]
        [InlineData(5, 06, 00, 00, 05, 14, 00, 00, "AM")]
        public void CalculatesAverageMeridian_WorksDifferentYears(
            int dayOne, int hourOne, int minuteOne, int secondOne,
            int dayTwo, int hourTwo, int minuteTwo, int secondTwo,
            string expected)
        {
            var dateOne = new DateTime(2018, 11, dayOne, hourOne, minuteOne, secondOne);
            var dateTwo = new DateTime(2019, 11, dayTwo, hourTwo, minuteTwo, secondTwo);
            var pair = new DateTimePair(dateOne, dateTwo);

            pair.CalculateAverageMeridian().Should().Be(expected);
        }

        [Theory]
        [InlineData(5, 11, 00, 00, 05, 14, 00, 00, "PM")]
        [InlineData(5, 06, 00, 00, 05, 14, 00, 00, "AM")]
        public void CalculatesAverageMeridian_WorksDifferentMonthAndYear(
            int dayOne, int hourOne, int minuteOne, int secondOne,
            int dayTwo, int hourTwo, int minuteTwo, int secondTwo,
            string expected)
        {
            var dateOne = new DateTime(2018, 11, dayOne, hourOne, minuteOne, secondOne);
            var dateTwo = new DateTime(2019, 03, dayTwo, hourTwo, minuteTwo, secondTwo);
            var pair = new DateTimePair(dateOne, dateTwo);

            pair.CalculateAverageMeridian().Should().Be(expected);
        }
    }
}
