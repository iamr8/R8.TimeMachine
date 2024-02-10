using System.Globalization;
using NodaTime;

namespace R8.TimeMachine.Tests.Resolvers
{
    public class UKTimezone : ILocalTimezoneResolver
    {
        public string IanaId => "Europe/London";

        public CalendarSystem GetCalendarSystem()
        {
            return CalendarSystem.Gregorian;
        }

        public CultureInfo GetCultureInfo()
        {
            return CultureInfo.GetCultureInfo("en-GB");
        }
    }
}