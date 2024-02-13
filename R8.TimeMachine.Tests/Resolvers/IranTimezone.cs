using System;
using System.Globalization;
using NodaTime;

namespace R8.TimeMachine.Tests.Resolvers
{
    public class IranTimezone : LocalTimezoneMap
    {
        public override string IanaId => "Asia/Tehran";
        public override DayOfWeek FirstDayOfWeek => DayOfWeek.Saturday;
        public override CalendarSystem Calendar => CalendarSystem.PersianSimple;
        public override CultureInfo Culture => CultureInfo.GetCultureInfo("fa-IR");
    }
}