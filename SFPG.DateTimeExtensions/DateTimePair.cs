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

        private const int HoursInMeridian = 12;

        private DateTime One { get; set; }
        private DateTime Two { get; set; }
        private DateTime Earliest => One < Two ? One : Two;
        private DateTime Latest => One < Two ? Two : One;
        private bool AmStart => !PmStart;
        private bool PmStart => Earliest.Hour > 12;
        private bool AmEnd => Latest.Hour < 12;
        private bool PmEnd => !AmEnd;

        private long TicksInMeridian => TimeSpan.TicksPerHour * HoursInMeridian;

        public IfBlock<DateTimePair, string> AreSameDay => new IfBlock<DateTimePair, string>(One.Date == Two.Date, this);
        public IfBlock<DateTimePair, string> AreNotSameDay => new IfBlock<DateTimePair, string>(One.Date != Two.Date, this);

        public string CalculateAverageMeridian()
        {
            var meridianAfterEarliest = new DateTime(Earliest.Year, Earliest.Month, Earliest.Day, PmStart ? 0 : 12, 0, 0);
            var meridianBeforeLatest = new DateTime(Latest.Year, Latest.Month, Latest.Day, AmEnd ? 0 : 12, 0, 0);
            var earliestToMeridian = meridianAfterEarliest - Earliest;
            var meridianToLatest = Latest - meridianBeforeLatest;
            var am = GetAmTotal(earliestToMeridian, meridianToLatest); 
            var pm = GetPmTotal(earliestToMeridian, meridianToLatest);
            return am >= pm ? "AM" : "PM";
        }

        private long GetAmTotal(TimeSpan earliestToMeridian, TimeSpan meridianToLatest)
        {
            var moreThanOneDividingMeridianPeriod = (Latest - Earliest).TotalHours >= HoursInMeridian;
            var startsAndEndsInPm = PmStart && PmEnd;
            const long PmBoundaryOffset = 1; // because distance from am time to 12:00 needs to not include 12:00 itself as 12:00 itself is PM.
            // sum time spent in the AM between: earliest and meridian after it, and, between: latest and meridian before it, and,
            // add extra period of AM time, if both times start in the PM, and, are more than 12 hours apart (unbalanced dividing meridian periods).
            var am = (PmStart ? 0 : earliestToMeridian.Ticks -  PmBoundaryOffset) +
                     (AmEnd ? meridianToLatest.Ticks : 0) +
                     (moreThanOneDividingMeridianPeriod && startsAndEndsInPm ? TicksInMeridian: 0); //offset for unbalanced, dividing, AM period.
            return am;
        }

        private long GetPmTotal(TimeSpan earliestToMeridian, TimeSpan meridianToLatest)
        {
            var moreThanOneDividingMeridianPeriod = (Latest - Earliest).TotalHours >= HoursInMeridian;
            var startsAndEndsInAm = AmStart && AmEnd; 
            // sum time spent in the PM between: earliest and meridian after it, and, between: latest and meridian before it, and,
            // add extra period of PM time, if both times start in the AM, and, are more than 12 hours apart (unbalanced dividing meridian periods).
            var pm = (PmStart ? earliestToMeridian.Ticks : 0) +
                     (AmEnd ? 0 : meridianToLatest.Ticks) +
                     (moreThanOneDividingMeridianPeriod && startsAndEndsInAm ? TicksInMeridian : 0); // offset for unbalanced, diving, PM period.
            return pm;
        }

        private bool IsEven(int number) => number % 2 == 0;
    }
}
