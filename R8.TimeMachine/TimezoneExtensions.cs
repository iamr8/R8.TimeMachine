using System;

namespace R8.TimeMachine
{
    public static class TimezoneExtensions
    {
        /// <summary>
        ///     Returns a <see cref="LocalTimezone" /> from the specified <see cref="ITimezone" />.
        /// </summary>
        /// <param name="options">A <see cref="LocalTimezoneMap" /> object.</param>
        /// <returns>A <see cref="LocalTimezone" /> object.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="options" /> is null.</exception>
        public static LocalTimezone? GetLocalTimezone<T>(this T? options) where T : LocalTimezoneMap
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            return LocalTimezone.Create(options.IanaId);
        }

        /// <summary>
        ///     Returns a <see cref="TimezoneDateTime" /> from the specified <see cref="DateTime" /> according to the specified <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime" /> object.</param>
        /// <param name="timeZone">A <see cref="LocalTimezone" /> object.</param>
        /// <returns>A <see cref="TimezoneDateTime" /> object.</returns>
        public static TimezoneDateTime ToTimezoneDateTime(this DateTime dateTime, LocalTimezone timeZone)
        {
            return new TimezoneDateTime(dateTime, timeZone);
        }

        /// <summary>
        ///     Returns a <see cref="TimezoneDateTime" /> from the specified <see cref="DateTime" /> according to the specified <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime" /> object.</param>
        /// <returns>A <see cref="TimezoneDateTime" /> object.</returns>
        public static TimezoneDateTime ToTimezoneDateTime(this DateTime dateTime)
        {
            return dateTime.ToTimezoneDateTime(LocalTimezone.Current);
        }
    }
}