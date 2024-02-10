using System;

namespace R8.TimeMachine
{
    public static class TimezoneExtensions
    {
        /// <summary>
        ///     Returns a <see cref="LocalTimezone" /> from the specified <see cref="ILocalTimezoneResolver" />.
        /// </summary>
        /// <param name="resolver">A <see cref="LocalTimezoneResolverCollection" /> object.</param>
        /// <returns>A <see cref="LocalTimezone" /> object.</returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="resolver" /> is null.</exception>
        public static LocalTimezone? GetLocalTimezone(this ILocalTimezoneResolver resolver)
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver));
            return LocalTimezone.CreateFrom(resolver.IanaId);
        }

        /// <summary>
        ///     Returns a <see cref="TimezoneDateTime" /> from the specified <see cref="DateTime" /> according to the specified <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime" /> object.</param>
        /// <param name="timeZone">A <see cref="LocalTimezone" /> object.</param>
        /// <returns>A <see cref="TimezoneDateTime" /> object.</returns>
        public static TimezoneDateTime ToTimezoneDateTime(this DateTime dateTime, LocalTimezone timeZone)
        {
            return TimezoneDateTime.FromDateTime(dateTime, timeZone);
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