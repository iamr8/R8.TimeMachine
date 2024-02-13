using System;
using System.Globalization;
using NodaTime;
using NodaTime.Extensions;

namespace R8.TimeMachine
{
    internal sealed class UtcTimezone : LocalTimezoneMap
    {
        private const string UtcIanaId = "UTC";
        private static readonly ZonedClock? ZonedClock;

        static UtcTimezone()
        {
            var dateTimeZone = DateTimeZoneProviders.Tzdb[UtcIanaId];
            ZonedClock ??= SystemClock.Instance.InZone(dateTimeZone);
        }

        public UtcTimezone()
        {
            IanaId = UtcIanaId;
            Calendar = ZonedClock!.Calendar;
            Culture = CultureInfo.InvariantCulture;
            FirstDayOfWeek = Culture.DateTimeFormat.FirstDayOfWeek;
        }

        public override string IanaId { get; }
        public override DayOfWeek FirstDayOfWeek { get; }
        public override CalendarSystem Calendar { get; }
        public override CultureInfo Culture { get; }
    }
}