using System.Globalization;
using NodaTime;

namespace R8.TimeMachine
{
    public interface ILocalTimezoneResolver
    {
        /// <summary>
        ///     Gets the IANA timezone identifier.
        /// </summary>
        string IanaId { get; }

        /// <summary>
        ///     Returns the calendar system of the timezone.
        /// </summary>
        /// <returns></returns>
        CalendarSystem GetCalendarSystem();

        /// <summary>
        ///     Returns the culture info of the timezone.
        /// </summary>
        /// <returns></returns>
        CultureInfo GetCultureInfo();
    }
}