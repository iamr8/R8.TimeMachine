using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using NodaTime;
using NodaTime.TimeZones;

namespace R8.TimeMachine
{
    /// <summary>
    ///     Initializes a new instance of <see cref="LocalTimezone" />.
    /// </summary>
    [DebuggerDisplay("{" + nameof(IanaId) + "} ({" + nameof(Id) + "})")]
    public class LocalTimezone : ITimezone, IEquatable<LocalTimezone>, IComparable<LocalTimezone>, IFormattable
    {
        /// <summary>
        ///     A collection of <see cref="ITimezone" /> that can be used to resolve timezone information.
        /// </summary>
        public static readonly LocalTimezoneMapCollection Mappings;

        private static readonly object SyncRoot = new object();
        private static readonly CachedLocalTimezoneCollection Cached;
        private static readonly AsyncLocal<LocalTimezone?> CurrentLocal = new AsyncLocal<LocalTimezone?>();

        private static LocalTimezone? _current;

        internal LocalTimezoneMap Map;
        internal DateTimeZone Zone;

        static LocalTimezone()
        {
            lock (SyncRoot)
            {
                Mappings ??= new LocalTimezoneMapCollection();
                Cached ??= new CachedLocalTimezoneCollection();
                _current ??= CurrentLocal.Value ?? Create(GetDefaultZone().Id);
            }
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="LocalTimezone" />.
        /// </summary>
        private LocalTimezone()
        {
        }

        /// <summary>
        ///     Gets the ID of the timezone
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        ///     Gets or sets the current timezone.
        /// </summary>
        /// <remarks>If a scope is started using <see cref="StartScope" />, this property will return the timezone of the scope.</remarks>
        public static LocalTimezone Current
        {
            get
            {
                lock (SyncRoot)
                {
                    if (CurrentLocal.Value != null)
                        return CurrentLocal.Value;
                    if (_current != null)
                        return _current;

                    var ianaId = GetDefaultZone().Id;
                    _current = Create(ianaId);
                    return _current;
                }
            }

            set
            {
                lock (SyncRoot)
                {
                    _current = value;
                }
            }
        }

        /// <summary>
        ///     Gets a <see cref="LocalTimezone" /> object representing the UTC timezone.
        /// </summary>
        public static LocalTimezone Utc => Create("UTC");

        public int CompareTo(LocalTimezone? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return this.Offset.CompareTo(other.Offset);
        }

        public bool Equals(LocalTimezone? other)
        {
            if (other is null)
                return false;
            return IanaId.Equals(other.IanaId, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Gets the IANA ID of the timezone
        /// </summary>
        public string IanaId { get; private set; }

        /// <summary>
        ///     Gets the culture info of the timezone
        /// </summary>
        public CultureInfo Culture => Map.Culture;

        /// <summary>
        ///     Gets the calendar system of the timezone
        /// </summary>
        public CalendarSystem Calendar => Map.Calendar;

        /// <summary>
        ///     Gets the first day of week for the timezone
        /// </summary>
        public DayOfWeek FirstDayOfWeek => Map.FirstDayOfWeek;

        /// <summary>
        ///     Gets the offset of the timezone
        /// </summary>
        public Offset Offset { get; private set; }

        /// <summary>
        ///     Initializes a new instance of <see cref="LocalTimezone" />.
        /// </summary>
        /// <param name="ianaId">A valid IANA ID.</param>
        /// <returns>A <see cref="LocalTimezone" /> object</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="ianaId" /> is null.</exception>
        public static LocalTimezone Create(string? ianaId)
        {
            if (ianaId == null)
                throw new ArgumentNullException(nameof(ianaId));

            if (Cached.TryGetValue(ianaId, out var cachedTimezone))
            {
                return cachedTimezone!;
            }

            var zone = DateTimeZoneProviders.Tzdb[ianaId];
            var offset = zone.GetUtcOffset(SystemClock.Instance.GetCurrentInstant());

            cachedTimezone = new LocalTimezone
            {
                IanaId = ianaId,
                Id = TzdbDateTimeZoneSource.Default.TzdbToWindowsIds.FirstOrDefault(x => x.Key.Equals(ianaId)).Value,
                Zone = DateTimeZoneProviders.Tzdb[ianaId],
                Offset = offset,
                Map = Mappings.TryGetValue(ianaId, out var resolver) && resolver != null ? resolver : GetUtcMap(),
            };
            Cached.Add(cachedTimezone);
            return cachedTimezone;
        }

        private static LocalTimezoneMap GetUtcMap()
        {
            if (Mappings.TryGetValue("UTC", out var map) && map != null)
                return map;

            return new UtcTimezone();
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

            lock (SyncRoot)
            {
                CurrentLocal.Value = timezone;
            }
        }

        /// <summary>
        ///     Ends the current timezone scope. This will set the current timezone to default.
        /// </summary>
        public static void EndScope()
        {
            lock (SyncRoot)
            {
                CurrentLocal.Value = null;
            }
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

        private static DateTimeZone GetDefaultZone()
        {
            return DateTimeZoneProviders.Tzdb.GetSystemDefault();
        }

        public static implicit operator LocalTimezone(string? ianaId)
        {
            return Create(ianaId);
        }

        public static implicit operator string(LocalTimezone timezone)
        {
            return timezone.IanaId;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("GMT");
            sb.Append(this.Offset.ToString("m", CultureInfo.CurrentCulture));
            return sb.ToString();
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (format == null)
                return ToString();

            if (format.Equals("G", StringComparison.OrdinalIgnoreCase))
                return ToString();

            if (format.Equals("N", StringComparison.OrdinalIgnoreCase))
                return IanaId;

            if (format.Equals("O", StringComparison.OrdinalIgnoreCase))
                return this.Offset.ToString("m", formatProvider ?? this.Culture);

            if (format.Equals("I", StringComparison.OrdinalIgnoreCase))
                return Id;

            if (format.Equals("C", StringComparison.OrdinalIgnoreCase))
                return Culture.Name;

            if (format.Equals("A", StringComparison.OrdinalIgnoreCase))
                return Calendar.Id;

            if (format.Equals("F", StringComparison.OrdinalIgnoreCase))
                return FirstDayOfWeek.ToString();

            throw new FormatException("Invalid format string");
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