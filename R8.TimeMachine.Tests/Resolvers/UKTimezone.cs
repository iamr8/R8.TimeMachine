using System;
using System.Globalization;
using NodaTime;

namespace R8.TimeMachine.Tests.Resolvers
{
    public class UKTimezone : LocalTimezoneMap
    {
        public override string IanaId => "Europe/London";
        public override DayOfWeek FirstDayOfWeek => DayOfWeek.Monday;
        public override CalendarSystem Calendar => CalendarSystem.Gregorian;
        public override CultureInfo Culture => CultureInfo.GetCultureInfo("en-GB");
    }
}