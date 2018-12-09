// Copyright S. F. P. Griffin 2018, License: GNU LESSER GENERAL PUBLIC LICENSE, Version 3, 29 June 2007.

namespace SFPG.DateTimeExtensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTimePair And(this DateTime one, DateTime two)
        {
            return new DateTimePair(one, two);
        }
    }
}
