using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.TimeZones;

namespace R8.TimeMachine
{
    /// <summary>
    ///     Represents a timezone.
    /// </summary>
    [DebuggerDisplay("{" + nameof(IanaId) + "} ({" + nameof(Id) + "})")]
    public class LocalTimezone : IEquatable<LocalTimezone>, IComparable<LocalTimezone>
    {
        /// <summary>
        ///     A collection of <see cref="ILocalTimezoneResolver" /> that can be used to resolve timezone information.
        /// </summary>
        public static readonly LocalTimezoneResolverCollection Resolvers;

        private static readonly object SharedSyncRoot = new object();

        private static readonly CachedLocalTimezoneCollection Cached;

        private static LocalTimezone? _current;
        private static readonly AsyncLocal<LocalTimezone?> CurrentLocal = new AsyncLocal<LocalTimezone?>();
        private readonly int _dayOfWeekAdjustment;
        private readonly object _syncRoot = new object();

        internal readonly DateTimeZone Zone;
        private DayOfWeek[]? _cachedDaysOfWeek;
        private Offset? _cachedOffset;

        static LocalTimezone()
        {
            lock (SharedSyncRoot)
            {
                Resolvers ??= new LocalTimezoneResolverCollection();
                Cached ??= new CachedLocalTimezoneCollection();
                _current ??= CurrentLocal.Value ?? CreateFrom(GetDefaultZone().Id);
            }
        }

        private LocalTimezone(string? ianaId)
        {
            if (ianaId == null)
                throw new ArgumentNullException(nameof(ianaId));

            lock (_syncRoot)
            {
                if (Cached.TryGetValue(ianaId, out var timezone))
                {
                    IanaId = ianaId;
                    Id = timezone.Id;
                    Zone = timezone.Zone;
                    Culture = timezone.Culture;
                    Calendar = timezone.Calendar;
                    _cachedOffset = timezone._cachedOffset;

                    FirstDayOfWeek = timezone.FirstDayOfWeek;
                    _dayOfWeekAdjustment = timezone._dayOfWeekAdjustment;
                }
                else
                {
                    IanaId = ianaId;
                    Zone = DateTimeZoneProviders.Tzdb[ianaId];
                    Id = TzdbDateTimeZoneSource.Default.TzdbToWindowsIds.FirstOrDefault(x => x.Key.Equals(ianaId)).Value;

                    if (Resolvers.TryGetValue(ianaId, out var resolver))
                    {
                        Culture = resolver.GetCultureInfo();
                        Calendar = resolver.GetCalendarSystem();
                    }
                    else
                    {
                        var zonedClock = SystemClock.Instance.InZone(Zone);
                        Calendar = zonedClock.Calendar;
                        Culture = CultureInfo.InvariantCulture;
                    }

                    FirstDayOfWeek = Culture.DateTimeFormat.FirstDayOfWeek;
                    _dayOfWeekAdjustment = Math.Abs(0 - (int)FirstDayOfWeek);

                    Cached.Add(this);
                }
            }
        }

        /// <summary>
        ///     Gets the ID of the timezone
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Gets the IANA ID of the timezone
        /// </summary>
        public string IanaId { get; }

        /// <summary>
        ///     Gets the culture info of the timezone
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        ///     Gets the calendar system of the timezone
        /// </summary>
        public CalendarSystem Calendar { get; }

        /// <summary>
        ///     Gets the first day of week for the timezone
        /// </summary>
        public DayOfWeek FirstDayOfWeek { get; }

        /// <summary>
        ///     Gets or sets the current timezone.
        /// </summary>
        /// <remarks>If a scope is started using <see cref="StartScope" />, this property will return the timezone of the scope.</remarks>
        public static LocalTimezone Current
        {
            get
            {
                lock (SharedSyncRoot)
                {
                    if (CurrentLocal.Value != null)
                        return CurrentLocal.Value;
                    if (_current != null)
                        return _current;

                    var ianaId = GetDefaultZone().Id;
                    _current = CreateFrom(ianaId);
                    return _current;
                }
            }

            set
            {
                lock (SharedSyncRoot)
                {
                    _current = value;
                }
            }
        }

        /// <summary>
        ///     Gets a <see cref="LocalTimezone" /> object representing the UTC timezone.
        /// </summary>
        public static LocalTimezone Utc => CreateFrom("UTC");

        public int CompareTo(LocalTimezone? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var offset = GetOffset();
            var otherOffset = other.GetOffset();
            return offset.CompareTo(otherOffset);
        }

        public bool Equals(LocalTimezone? other)
        {
            if (other is null)
                return false;
            return IanaId.Equals(other.IanaId, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Starts a new scope for the current timezone. This will set the current timezone to the specified timezone. This method is useful when you want to change the timezone for a specific scope (ASP.NET Core).
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="timezone" /> is null.</exception>
        /// <remarks>Don't forget to call <see cref="EndScope" /> to end the scope, when you are done with the timezone. Otherwise, the timezone will remain the same.</remarks>
        public static void StartScope(LocalTimezone timezone)
        {
            if (timezone == null)
                throw new ArgumentNullException(nameof(timezone));

            lock (SharedSyncRoot)
            {
                CurrentLocal.Value = timezone;
            }
        }

        /// <summary>
        ///     Ends the current timezone scope. This will set the current timezone to default.
        /// </summary>
        public static void EndScope()
        {
            lock (SharedSyncRoot)
            {
                CurrentLocal.Value = null;
            }
        }

        /// <summary>
        ///     Returns days of the week according to timezone and its calendar.
        /// </summary>
        /// <returns>An array of <see cref="DayOfWeek" /> objects</returns>
        public DayOfWeek[] GetDaysOfWeek()
        {
            lock (_syncRoot)
            {
                if (_cachedDaysOfWeek != null)
                    return _cachedDaysOfWeek;

                var daysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToArray();
                if (_dayOfWeekAdjustment == 0)
                    return daysOfWeek;

                Span<DayOfWeek> orderedArray = stackalloc DayOfWeek[7];
                for (var i = 0; i < daysOfWeek.Length; i++)
                {
                    var adjustment = (i + _dayOfWeekAdjustment - 1) % 7;
                    var index = adjustment >= 6
                        ? 7 - adjustment - 1
                        : adjustment + 1;
                    orderedArray[i] = daysOfWeek[index];
                }

                _cachedDaysOfWeek = orderedArray.ToArray();
                return _cachedDaysOfWeek;
            }
        }

        /// <summary>
        ///     Returns a <see cref="ZonedDateTime" /> object from a <see cref="DateTime" />.
        /// </summary>
        /// <param name="ticks">A value representing the number of ticks</param>
        /// <param name="isUtc">A value indicating whether the <paramref name="ticks" /> is in UTC or not</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public ZonedDateTime GetZonedDateTime(long ticks, bool isUtc)
        {
            ZonedDateTime zoned;
            if (isUtc)
            {
                var instant = Instant.FromUnixTimeTicks(ticks);
                zoned = instant.InZone(Zone);
            }
            else
            {
                var dateTime = new DateTime(ticks, DateTimeKind.Unspecified);
                var local = LocalDateTime.FromDateTime(dateTime);
                zoned = local.InZoneLeniently(Zone);
            }

            var output = zoned.WithCalendar(Calendar);
            return output;
        }

        /// <summary>
        ///     Returns a <see cref="ZonedDateTime" /> object from a <see cref="DateTime" />.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime" /> object</param>
        /// <returns>A <see cref="ZonedDateTime" /> object</returns>
        public ZonedDateTime GetZonedDateTime(DateTime dateTime)
        {
            ZonedDateTime zoned;
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                var instant = Instant.FromDateTimeUtc(dateTime);
                zoned = instant.InZone(Zone);
            }
            else
            {
                var local = LocalDateTime.FromDateTime(dateTime);
                zoned = local.InZoneLeniently(Zone);
            }

            var output = zoned.WithCalendar(Calendar);
            return output;
        }

        /// <summary>
        ///     Returns the offset of the timezone.
        /// </summary>
        /// <returns>An <see cref="Offset" /> object</returns>
        public Offset GetOffset()
        {
            lock (_syncRoot)
            {
                if (_cachedOffset != null)
                    return _cachedOffset.Value;

                var instant = SystemClock.Instance.GetCurrentInstant();
                _cachedOffset = Zone.GetUtcOffset(instant);
                return _cachedOffset.Value;
            }
        }

        private static DateTimeZone GetDefaultZone()
        {
            return DateTimeZoneProviders.Tzdb.GetSystemDefault();
        }

        /// <summary>
        ///     Creates a <see cref="LocalTimezone" /> object from a IANA ID.
        /// </summary>
        /// <param name="ianaId">A valid IANA ID.</param>
        /// <returns>A <see cref="LocalTimezone" /> object.</returns>
        public static LocalTimezone CreateFrom(string? ianaId)
        {
            return new LocalTimezone(ianaId);
        }

        public static implicit operator LocalTimezone(string? ianaId)
        {
            return CreateFrom(ianaId);
        }

        public static implicit operator string(LocalTimezone timezone)
        {
            return timezone.IanaId;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("GMT");
            sb.Append(GetOffset().ToString("m", CultureInfo.CurrentCulture));
            return sb.ToString();
        }

        public static int CompareTo(LocalTimezone? left, LocalTimezone? right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left is null) return -1;
            if (right is null) return 1;
            return left.CompareTo(right);
        }

        public static bool Equal(LocalTimezone? left, LocalTimezone? right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left is null || right is null)
                return false;
            return left.Equals(right);
        }

        public override bool Equals(object? obj)
        {
            return obj is LocalTimezone other && Equals(other);
        }

        public override int GetHashCode()
        {
            return IanaId.GetHashCode();
        }

        public static bool operator ==(LocalTimezone? left, LocalTimezone? right)
        {
            return Equal(left, right);
        }

        public static bool operator !=(LocalTimezone? left, LocalTimezone? right)
        {
            return !Equal(left, right);
        }

        public static bool operator <(LocalTimezone? left, LocalTimezone? right)
        {
            return CompareTo(left, right) < 0;
        }

        public static bool operator >(LocalTimezone? left, LocalTimezone? right)
        {
            return CompareTo(left, right) > 0;
        }

        public static bool operator <=(LocalTimezone? left, LocalTimezone? right)
        {
            return CompareTo(left, right) <= 0;
        }

        public static bool operator >=(LocalTimezone? left, LocalTimezone? right)
        {
            return CompareTo(left, right) >= 0;
        }
    }
}