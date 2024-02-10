using System.Globalization;
using NodaTime;

namespace R8.TimeMachine.Tests.Resolvers
{
    public class IranTimezone : ILocalTimezoneResolver
    {
        public string IanaId => "Asia/Tehran";

        public CalendarSystem GetCalendarSystem()
        {
            return CalendarSystem.PersianSimple;
        }

        public CultureInfo GetCultureInfo()
        {
            return CultureInfo.GetCultureInfo("fa-IR");
        }
    }
}