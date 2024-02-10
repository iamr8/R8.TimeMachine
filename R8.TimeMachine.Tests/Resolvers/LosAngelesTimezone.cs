using System.Globalization;
using NodaTime;

namespace R8.TimeMachine.Tests.Resolvers
{
    public class LosAngelesTimezone : ILocalTimezoneResolver
    {
        public string IanaId => "America/Los_Angeles";

        public CalendarSystem GetCalendarSystem()
        {
            return CalendarSystem.Gregorian;
        }

        public CultureInfo GetCultureInfo()
        {
            return CultureInfo.GetCultureInfo("en-US");
        }
    }
}