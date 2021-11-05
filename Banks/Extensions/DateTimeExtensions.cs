using System;

namespace Banks.Extensions
{
    internal static class DateTimeExtensions
    {
        public static DateTime GetYearMonthAndFirstDay(this DateTime dateTime)
            => new DateTime(dateTime.Year, dateTime.Month, 1);
    }
}