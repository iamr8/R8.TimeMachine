using System;

namespace R8.TimeMachine
{
    public class TimeMachine : ITimeMachine
    {
        public TimezoneDateTime Now(LocalTimezone? timezone = null)
        {
            var tz = timezone ?? LocalTimezone.Current;
            var tzdt = UtcNow().ToTimezoneDateTime(tz);
            return tzdt;
        }

        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}