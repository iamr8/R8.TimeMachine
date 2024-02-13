using System;
using System.Globalization;
using NodaTime;

namespace R8.TimeMachine.Tests.Resolvers
{
    public class LosAngelesTimezone : LocalTimezoneMap
    {
        public override string IanaId => "America/Los_Angeles";
        public override DayOfWeek FirstDayOfWeek => DayOfWeek.Sunday;
        public override CalendarSystem Calendar => CalendarSystem.Gregorian;
        public override CultureInfo Culture => CultureInfo.GetCultureInfo("en-US");
    }
}