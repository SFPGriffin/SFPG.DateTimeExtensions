// Copyright S. F. P. Griffin 2018, License: GNU LESSER GENERAL PUBLIC LICENSE, Version 3, 29 June 2007.

namespace SFPG.DateTimeExtensions
{
    using System;

    public class DateTimePair
    {
        public DateTimePair(DateTime one, DateTime two)
        {
            One = one;
            Two = two;
        }

        private DateTime One { get; set; }
        private DateTime Two { get; set; }
        private DateTime Earliest => One < Two ? One : Two;
        private DateTime Latest => One < Two ? Two : One;
        private bool AmStart => !PmStart;
        private bool PmStart => Earliest.Hour > 12;
        private bool AmEnd => Latest.Hour < 12;
        private bool PmEnd => !AmEnd;

        private const int TwelveHours = 12;

        private long TicksInHalfday => TimeSpan.TicksPerHour * TwelveHours;

        public IfBlock<DateTimePair, string> AreSameDay => new IfBlock<DateTimePair, string>(One.Date == Two.Date, this);
        public IfBlock<DateTimePair, string> AreNotSameDay => new IfBlock<DateTimePair, string>(One.Date != Two.Date, this);

        public string GetAverageOfAmOrPm()
        {
            var TwelveOfClockAfterEarliest = new DateTime(Earliest.Year, Earliest.Month, Earliest.Day, PmStart ? 0 : 12, 0, 0);
            var TwelveofClockBeforeLatest = new DateTime(Latest.Year, Latest.Month, Latest.Day, AmEnd ? 0 : 12, 0, 0);
            var earliestToTwelveOfClock = TwelveOfClockAfterEarliest - Earliest;
            var twelveOfClockToLatest = Latest - TwelveofClockBeforeLatest;
            var am = GetAmTotal(earliestToTwelveOfClock, twelveOfClockToLatest); 
            var pm = GetPmTotal(earliestToTwelveOfClock, twelveOfClockToLatest);
            return am >= pm ? "AM" : "PM";
        }

        private long GetAmTotal(TimeSpan earliestToTwelveOfClock, TimeSpan twelveOfClockToLatest)
        {
            var moreThanOneDividingHalfday = (Latest - Earliest).TotalHours >= TwelveHours;
            var startsAndEndsInPm = PmStart && PmEnd;
            const long PmBoundaryOffset = 1; // because distance from am time to 12:00 needs to not include 12:00 itself as 12:00 itself is PM.
            // sum time spent in the AM between: earliest and the twelve of the clock after it, and, between: latest and the twelve of the clock before it, and,
            // add extra period of AM time, if both times start in the PM, and, are more than 12 hours apart (unbalanced dividing half days).
            var am = (PmStart ? 0 : earliestToTwelveOfClock.Ticks -  PmBoundaryOffset) +
                     (AmEnd ? twelveOfClockToLatest.Ticks : 0) +
                     (moreThanOneDividingHalfday && startsAndEndsInPm ? TicksInHalfday: 0); //offset for unbalanced, dividing, AM period.
            return am;
        }

        private long GetPmTotal(TimeSpan earliestToTwelveOfClock, TimeSpan twelveOfClockToLatest)
        {
            var moreThanOneDividingHalfDay = (Latest - Earliest).TotalHours >= TwelveHours;
            var startsAndEndsInAm = AmStart && AmEnd; 
            // sum time spent in the PM between: earliest and the twelve of the clock after it, and, between: latest and the twelve of the clock before it, and,
            // add extra period of PM time, if both times start in the AM, and, are more than 12 hours apart (unbalanced dividing half days).
            var pm = (PmStart ? earliestToTwelveOfClock.Ticks : 0) +
                     (AmEnd ? 0 : twelveOfClockToLatest.Ticks) +
                     (moreThanOneDividingHalfDay && startsAndEndsInAm ? TicksInHalfday : 0); // offset for unbalanced, diving, PM period.
            return pm;
        }

        private bool IsEven(int number) => number % 2 == 0;
    }
}
