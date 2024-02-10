using System.Globalization;
using NodaTime;

namespace R8.TimeMachine.Tests.Resolvers
{
    public class TurkeyTimezone : ILocalTimezoneResolver
    {
        public string IanaId => "Europe/Istanbul";

        public CalendarSystem GetCalendarSystem()
        {
            return CalendarSystem.Gregorian;
        }

        public CultureInfo GetCultureInfo()
        {
            return CultureInfo.GetCultureInfo("tr-TR");
        }
    }
}