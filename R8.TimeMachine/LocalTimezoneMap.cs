using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using NodaTime;

namespace R8.TimeMachine
{
    /// <summary>
    ///     A class representing a local timezone options.
    /// </summary>
    public abstract class LocalTimezoneMap : ITimezone
    {
        private static readonly Lazy<DayOfWeek[]> UnorderedDaysOfWeek = new Lazy<DayOfWeek[]>(() => Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToArray(), LazyThreadSafetyMode.ExecutionAndPublication);

        internal DayOfWeek[] OrderedDaysOfWeek
        {
            get
            {
                var daysOfWeek = UnorderedDaysOfWeek.Value;
                if (!(daysOfWeek is { Length: 7 }))
                    throw new InvalidOperationException("Days of week are not valid.");

                var dayOfWeekAdjustment = Math.Abs(0 - (int)FirstDayOfWeek);
                if (dayOfWeekAdjustment == 0)
                    return daysOfWeek;

                Span<DayOfWeek> orderedArray = stackalloc DayOfWeek[7];
                for (var i = 0; i < daysOfWeek.Length; i++)
                {
                    var adjustment = (i + dayOfWeekAdjustment - 1) % 7;
                    var index = adjustment >= 6
                        ? 7 - adjustment - 1
                        : adjustment + 1;
                    orderedArray[i] = daysOfWeek[index];
                }

                return orderedArray.ToArray();
            }
        }

        public abstract string IanaId { get; }
        public abstract DayOfWeek FirstDayOfWeek { get; }
        public abstract CalendarSystem Calendar { get; }
        public abstract CultureInfo Culture { get; }
    }
}