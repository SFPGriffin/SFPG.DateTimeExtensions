// Copyright S. F. P. Griffin 2018, License: GNU GENERAL PUBLIC LICENSE, Version 3, 29 June 2007.

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

        public string CalculateAverageMeridian()
        {
            var pmStart = Earliest.Hour > 12;
            var amEnd = Latest.Hour < 12;
            var meridianAfterEarliest = new DateTime(Earliest.Year, Earliest.Month, Earliest.Day, pmStart ? 0 : 12, 0, 0);
            var meridianBeforeLatest = new DateTime(Latest.Year, Latest.Month, Latest.Day, amEnd ? 0 : 12, 0, 0);
            var earliestToMeridian = meridianAfterEarliest - Earliest;
            var meridianToLatest = Latest - meridianBeforeLatest;
            var am = (pmStart ? 0 : earliestToMeridian.Ticks) + (amEnd ? meridianToLatest.Ticks : 0); // sum time spent in the AM between: earliest and meridian after it, and, between: latest and meridian before it.
            var pm = (pmStart ? earliestToMeridian.Ticks : 0) + (amEnd ? 0 : meridianToLatest.Ticks); // sum time spent in the PM between: earliest and meridian after it, and, between: latest and meridian before it.
            return am >= pm ? "AM" : "PM";
        }
    }
}
