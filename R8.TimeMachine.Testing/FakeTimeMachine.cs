using System;
using System.Reflection;

namespace R8.TimeMachine.Testing
{
    public class FakeTimeMachine : ITimeMachine
    {
        private TimezoneDateTime _currentTimezoneDateTime;
        private DateTime _currentUtcTime;

        public LocalTimezone? CurrentTimezone { get; set; } = LocalTimezone.Utc;

        public TimezoneDateTime CurrentTimezoneDateTime
        {
            private get => _currentTimezoneDateTime;
            set
            {
                if (CurrentTimezone == null)
                    throw new AmbiguousMatchException($"'{nameof(CurrentTimezone)}' should not be null");

                _currentTimezoneDateTime = value;
                _currentUtcTime = _currentTimezoneDateTime.ToDateTimeUtc();
            }
        }

        public DateTime CurrentUtcTime
        {
            private get => _currentUtcTime;
            set
            {
                if (value.Kind != DateTimeKind.Utc)
                    throw new AmbiguousMatchException($"'{nameof(CurrentUtcTime)}' should be kind of {DateTimeKind.Utc}");

                if (CurrentTimezone == null)
                    throw new AmbiguousMatchException($"'{nameof(CurrentTimezone)}' should not be null");

                _currentUtcTime = value;
                _currentTimezoneDateTime = _currentUtcTime.ToTimezoneDateTime(CurrentTimezone);
            }
        }

        public TimezoneDateTime Now(LocalTimezone? timezone = null)
        {
            return CurrentTimezoneDateTime;
        }

        public DateTime UtcNow()
        {
            return CurrentUtcTime;
        }

        public TimezoneDateTime Now(string? timezone = "Asia/Tehran")
        {
            return CurrentTimezoneDateTime;
        }

        public TimezoneDateTime RealNow(string ianaId = "Asia/Tehran")
        {
            return DateTime.UtcNow.ToTimezoneDateTime(ianaId);
        }

        public DateTime RealUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}