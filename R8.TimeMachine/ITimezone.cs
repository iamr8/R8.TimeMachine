using System;
using System.Globalization;
using NodaTime;

namespace R8.TimeMachine
{
    public interface ITimezone
    {
        /// <summary>
        ///     Gets the IANA timezone identifier.
        /// </summary>
        string IanaId { get; }

        /// <summary>
        ///     Gets the first day of the week.
        /// </summary>
        DayOfWeek FirstDayOfWeek { get; }

        /// <summary>
        ///     Returns the calendar system of the timezone.
        /// </summary>
        CalendarSystem Calendar { get; }

        /// <summary>
        ///     Returns the culture info of the timezone.
        /// </summary>
        CultureInfo Culture { get; }
    }
}