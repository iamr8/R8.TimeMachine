using System;
using NodaTime;
using NodaTime.Extensions;

namespace R8.TimeMachine
{
    /// <summary>
    ///     Represents an instant in time, typically expressed as a date and time of day according to a particular calendar and time zone.
    /// </summary>
    public readonly struct TimezoneDateTime : IComparable, IComparable<TimezoneDateTime>, IEquatable<TimezoneDateTime>, IFormattable
    {
        private readonly ZonedDateTime _zonedDateTime;

        private TimezoneDateTime(DateTime dateTime, LocalTimezone timezone)
        {
            _zonedDateTime = timezone.GetZonedDateTime(dateTime);

            Ticks = dateTime.Ticks;
            Year = _zonedDateTime.Year;
            Month = _zonedDateTime.Month;
            Day = _zonedDateTime.Day;
            Hour = _zonedDateTime.Hour;
            Minute = _zonedDateTime.Minute;
            Second = _zonedDateTime.Second;
            DayOfWeek = _zonedDateTime.DayOfWeek.ToDayOfWeek();
        }

        private TimezoneDateTime(ZonedDateTime zonedDateTime)
        {
            _zonedDateTime = zonedDateTime;

            var f = _zonedDateTime.ToDateTimeUtc();
            Ticks = f.Ticks;
            Year = _zonedDateTime.Year;
            Month = _zonedDateTime.Month;
            Day = _zonedDateTime.Day;
            Hour = _zonedDateTime.Hour;
            Minute = _zonedDateTime.Minute;
            Second = _zonedDateTime.Second;
            DayOfWeek = _zonedDateTime.DayOfWeek.ToDayOfWeek();
        }

        private TimezoneDateTime(int localYear, int localMonth, int localDay, int localHour, int localMinute, int localSecond, LocalTimezone timezone)
        {
            var dateTime = new LocalDateTime(localYear, localMonth, localDay, localHour, localMinute, localSecond, timezone.Calendar);
            var zone = dateTime.InZoneLeniently(timezone.Zone);
            _zonedDateTime = zone.WithCalendar(timezone.Calendar);

            var f = _zonedDateTime.ToDateTimeUtc();
            Ticks = f.Ticks;
            Year = localYear;
            Month = localMonth;
            Day = localDay;
            Hour = localHour;
            Minute = localMinute;
            Second = localSecond;
            DayOfWeek = _zonedDateTime.DayOfWeek.ToDayOfWeek();
        }

        /// <inheritdoc cref="DateTime.Ticks" />
        public long Ticks { get; }

        /// <inheritdoc cref="DateTime.Year" />
        public int Year { get; }

        /// <inheritdoc cref="DateTime.Month" />
        public int Month { get; }

        /// <inheritdoc cref="DateTime.Day" />
        public int Day { get; }

        /// <inheritdoc cref="DateTime.Hour" />
        public int Hour { get; }

        /// <inheritdoc cref="DateTime.Minute" />
        public int Minute { get; }

        /// <inheritdoc cref="DateTime.Second" />
        public int Second { get; }

        /// <inheritdoc cref="DateTime.DayOfWeek" />
        public DayOfWeek DayOfWeek { get; }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> instance with given local year, month, day, hour, minute, second and timezone.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="timeZone" /> is null.</exception>
        public static TimezoneDateTime Parse(int localYear, int localMonth, int localDay, int localHour, int localMinute, int localSecond, LocalTimezone timeZone)
        {
            return new TimezoneDateTime(localYear, localMonth, localDay, localHour, localMinute, localSecond, timeZone);
        }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> instance with given local year, month, day and timezone.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="timeZone" /> is null.</exception>
        public static TimezoneDateTime Parse(int localYear, int localMonth, int localDay, LocalTimezone timeZone)
        {
            return new TimezoneDateTime(localYear, localMonth, localDay, 0, 0, 0, timeZone);
        }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> instance from given <see cref="DateTime" /> and <see cref="LocalTimezone" />.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="timezone" /> is null.</exception>
        public static TimezoneDateTime FromDateTime(DateTime dateTime, LocalTimezone timezone)
        {
            return new TimezoneDateTime(dateTime, timezone);
        }

        /// <inheritdoc cref="ZonedDateTime.ToDateTimeUtc" />
        public DateTime ToDateTimeUtc()
        {
            return new DateTime(Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        ///     Returns the timezone of this instance.
        /// </summary>
        /// <returns>A <see cref="LocalTimezone" /> object</returns>
        /// <exception cref="InvalidOperationException">Thrown when timezone is not available.</exception>
        public LocalTimezone GetTimezone()
        {
            return LocalTimezone.CreateFrom(_zonedDateTime.Zone.Id);
        }

        /// <summary>
        ///     Returns the number of days in the month and year of the specified date.
        /// </summary>
        /// <returns>An integer from 28 through 31.</returns>
        public int GetDaysInMonth()
        {
            return _zonedDateTime.Calendar.GetDaysInMonth(Year, Month);
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the hour value set to 00:00. For instance, if the current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing "2019-01-01 12:00:00".
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetStartOfHour()
        {
            return new TimezoneDateTime(Year, Month, Day, Hour, 0, 0, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the hour value set to 23:59. For instance, if the current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing "2019-01-01 23:59:59".
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetEndOfHour()
        {
            return new TimezoneDateTime(Year, Month, Day, Hour, 59, 59, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the minute value set to 00. For instance, if the current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing "2019-01-01 12:33:00".
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetStartOfMinute()
        {
            return new TimezoneDateTime(Year, Month, Day, Hour, Minute, 0, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the minute value set to 59. For instance, if the current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing "2019-01-01 12:33:59".
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetEndOfMinute()
        {
            return new TimezoneDateTime(Year, Month, Day, Hour, Minute, 59, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the time value set to 00:00:00. For instance, if the current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing "2019-01-01 00:00:00".
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        /// <remarks>This method works similar to <c>DateTime.Date</c></remarks>
        public TimezoneDateTime GetStartOfDay()
        {
            return new TimezoneDateTime(Year, Month, Day, 0, 0, 0, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object with the same date as this instance, and the time value set to 23:59:59. For instance, if the current instance represents the date "2019-01-01 12:33:25", this method will return a new instance representing "2019-01-01 23:59:59".
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetEndOfDay()
        {
            return new TimezoneDateTime(Year, Month, Day, 23, 59, 59, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and the day value set to 1. For instance, if the current instance represents the date "2019-01-15 12:33:25", this method will return a new instance representing "2019-01-01 00:00:00".
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetStartOfMonth()
        {
            return new TimezoneDateTime(Year, Month, 1, 0, 0, 0, GetTimezone());
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and the day value set to the last day of the month. For instance, if the current instance represents the date "2019-01-15 12:33:25", this method will return a new instance representing "2019-01-31 23:59:59".
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetEndOfMonth()
        {
            return new TimezoneDateTime(Year, Month, GetDaysInMonth(), 23, 59, 59, GetTimezone());
        }

        private int GetDayInWeek()
        {
            return (7 + (DayOfWeek - GetTimezone().FirstDayOfWeek)) % 7;
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and represents the first moment of the current week.
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetStartOfWeek()
        {
            var zonedDateTime = GetInitialMomentOfWeek();
            return new TimezoneDateTime(zonedDateTime);
        }

        private ZonedDateTime GetInitialMomentOfWeek()
        {
            var dayInWeek = GetDayInWeek();
            var zone = GetTimezone();
            var zonedDateTime = _zonedDateTime.Plus(TimeSpan.FromDays(-dayInWeek).ToDuration()).Date.AtStartOfDayInZone(zone.Zone);
            return zonedDateTime;
        }

        /// <summary>
        ///     Returns a new object similar to this instance, and represents the last moment of the current week.
        /// </summary>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime GetEndOfWeek()
        {
            var zonedDateTime = GetInitialMomentOfWeek().Plus(Duration.FromDays(6)).PlusHours(23).PlusMinutes(59).PlusSeconds(59);
            return new TimezoneDateTime(zonedDateTime);
        }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
        /// <remarks>This API has slow performance.</remarks>
        public TimezoneDateTime AddYears(int value)
        {
            var timezone = GetTimezone();
            var zonedDateTime = _zonedDateTime
                .LocalDateTime
                .PlusYears(value)
                .InZoneLeniently(timezone.Zone)
                .WithCalendar(timezone.Calendar);
            return new TimezoneDateTime(zonedDateTime);
        }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="value">A number of months. The value parameter can be negative or positive.</param>
        public TimezoneDateTime AddMonths(int value)
        {
            var timezone = GetTimezone();
            var zonedDateTime = _zonedDateTime
                .LocalDateTime
                .PlusMonths(value)
                .InZoneLeniently(timezone.Zone)
                .WithCalendar(timezone.Calendar);
            return new TimezoneDateTime(zonedDateTime);
        }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="value">A number of days. The value parameter can be negative or positive.</param>
        public TimezoneDateTime AddDays(int value)
        {
            var zonedDateTime = _zonedDateTime.Plus(TimeSpan.FromDays(value).ToDuration());
            return new TimezoneDateTime(zonedDateTime);
        }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of hours to the value of this instance.
        /// </summary>
        /// <param name="value">A number of hours. The value parameter can be negative or positive.</param>
        public TimezoneDateTime AddHours(int value)
        {
            var zonedDateTime = _zonedDateTime.Plus(TimeSpan.FromHours(value).ToDuration());
            return new TimezoneDateTime(zonedDateTime);
        }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of minutes to the value of this instance.
        /// </summary>
        /// <param name="value">A number of minutes. The value parameter can be negative or positive.</param>
        public TimezoneDateTime AddMinutes(int value)
        {
            var zonedDateTime = _zonedDateTime.Plus(TimeSpan.FromMinutes(value).ToDuration());
            return new TimezoneDateTime(zonedDateTime);
        }

        /// <summary>
        ///     Returns a new <see cref="TimezoneDateTime" /> that adds the specified number of seconds to the value of this instance.
        /// </summary>
        /// <param name="value">A number of seconds. The value parameter can be negative or positive.</param>
        public TimezoneDateTime AddSeconds(int value)
        {
            var zonedDateTime = _zonedDateTime.Plus(TimeSpan.FromSeconds(value).ToDuration());
            return new TimezoneDateTime(zonedDateTime);
        }

        /// <summary>
        ///     Returns a new object with the specified timezone.
        /// </summary>
        /// <param name="timezone">A timezone to be compared with the current data</param>
        /// <returns>A <see cref="TimezoneDateTime" /> object</returns>
        public TimezoneDateTime WithTimezone(LocalTimezone timezone)
        {
            var dateTime = _zonedDateTime.ToDateTimeUtc();
            return new TimezoneDateTime(dateTime, timezone);
        }

        public override string ToString()
        {
            return ToString("G");
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";

            var dateTime = _zonedDateTime.ToDateTimeUnspecified();
            return dateTime.ToString(format, GetTimezone().Culture);
        }

        /// <summary>
        ///     Converts the value of the current <see cref="TimezoneDateTime" /> object to its equivalent string representation using the formatting conventions of the current culture.
        /// </summary>
        /// <param name="format">A standard or custom date and time format string.</param>
        /// <returns>A string representation of the value of the current <see cref="TimezoneDateTime" /> object.</returns>
        public string ToString(string? format)
        {
            return ToString(format, GetTimezone().Culture);
        }

        public static int Compare(TimezoneDateTime left, TimezoneDateTime right)
        {
            var ticks1 = left.Ticks;
            var ticks2 = right.Ticks;

            if (ticks1 > ticks2)
                return 1;
            if (ticks1 < ticks2)
                return -1;
            return 0;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null)
                return 1;
            if (obj is TimezoneDateTime tz)
                return Compare(this, tz);
            throw new ArgumentException("Argument must be TimezoneDateTime");
        }

        public int CompareTo(TimezoneDateTime value)
        {
            return Compare(this, value);
        }

        public static bool operator >(TimezoneDateTime a, TimezoneDateTime b)
        {
            return a.CompareTo(b) > 0;
        }

        public TimezoneDateTime Add(TimeSpan timeSpan)
        {
            var zoned = _zonedDateTime.Plus(timeSpan.ToDuration());
            return new TimezoneDateTime(zoned);
        }

        public TimezoneDateTime Subtract(TimeSpan timeSpan)
        {
            var zoned = _zonedDateTime.Minus(timeSpan.ToDuration());
            return new TimezoneDateTime(zoned);
        }

        public TimeSpan Subtract(TimezoneDateTime timezoneDateTime)
        {
            return TimeSpan.FromTicks(Ticks).Subtract(TimeSpan.FromTicks(timezoneDateTime.Ticks));
        }

        public static TimeSpan operator -(TimezoneDateTime a, TimezoneDateTime b)
        {
            return a.Subtract(b);
        }

        public static TimezoneDateTime operator +(TimezoneDateTime d, TimeSpan t)
        {
            return d.Add(t);
        }

        public static bool operator <(TimezoneDateTime a, TimezoneDateTime b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >=(TimezoneDateTime a, TimezoneDateTime b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <=(TimezoneDateTime a, TimezoneDateTime b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool Equals(TimezoneDateTime left, TimezoneDateTime right)
        {
            return left.Ticks == right.Ticks;
        }

        public static bool operator ==(TimezoneDateTime a, TimezoneDateTime b)
        {
            return Equals(a, b);
        }

        public bool Equals(TimezoneDateTime other)
        {
            return Equals(this, other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is TimezoneDateTime tz)
                return Equals(this, tz);
            return false;
        }

        public static bool operator !=(TimezoneDateTime a, TimezoneDateTime b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }
    }
}