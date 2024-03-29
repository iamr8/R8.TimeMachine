using System;
using System.Globalization;
using FluentAssertions;
using NodaTime;
using R8.TimeMachine.Tests.Resolvers;
using Xunit;

namespace R8.TimeMachine.Tests
{
    public class TimezoneTests
    {
        public TimezoneTests()
        {
            LocalTimezone.Mappings.Clear();
            LocalTimezone.Mappings.Add(new IranTimezone());
            LocalTimezone.Mappings.Add(new TurkeyTimezone());
            LocalTimezone.Mappings.Add(new UKTimezone());
            LocalTimezone.Mappings.Add(new LosAngelesTimezone());
        }

        [Fact]
        public void should_return_current_timezone_set_by_scope()
        {
            var systemTimezone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            LocalTimezone.Current.IanaId.Should().Be(systemTimezone.Id);

            LocalTimezone.StartScope("Europe/Istanbul");

            LocalTimezone.Current.IanaId.Should().Be("Europe/Istanbul");

            LocalTimezone.EndScope();

            LocalTimezone.Current.IanaId.Should().Be(systemTimezone.Id);
        }

        [Fact]
        public void should_return_utc_timezone()
        {
            var timezone = LocalTimezone.Utc;
            var currentTimezone = DateTimeZone.Utc;

            timezone.IanaId.Should().Be(currentTimezone.Id);
        }

        [Fact]
        public void should_return_list_of_resolvers()
        {
            // Act
            var resolvers = LocalTimezone.Mappings;

            // Assert
            resolvers.Should().HaveCount(4);
            resolvers.Should().ContainSingle(x => x.IanaId == "Asia/Tehran");
            resolvers.Should().ContainSingle(x => x.IanaId == "Europe/Istanbul");
            resolvers.Should().ContainSingle(x => x.IanaId == "Europe/London");
            resolvers.Should().ContainSingle(x => x.IanaId == "America/Los_Angeles");
        }

        [Fact]
        public void should_return_timezone_from_resolver()
        {
            // Act
            var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone();

            // Assert
            timezone.Should().NotBeNull();
            timezone.IanaId.Should().Be("Asia/Tehran");
            timezone.Id.Should().BeOneOf("Iran Standard Time", "Iran Daylight Time");
            timezone.ToString().Should().Be("GMT+03:30");
            timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("fa-IR"));
            timezone.Calendar.Should().Be(CalendarSystem.PersianSimple);
            timezone.FirstDayOfWeek.Should().Be(DayOfWeek.Saturday);

            var daysOfWeek = timezone.Map.OrderedDaysOfWeek;
            daysOfWeek[0].Should().Be(DayOfWeek.Saturday);
            daysOfWeek[1].Should().Be(DayOfWeek.Sunday);
            daysOfWeek[2].Should().Be(DayOfWeek.Monday);
            daysOfWeek[3].Should().Be(DayOfWeek.Tuesday);
            daysOfWeek[4].Should().Be(DayOfWeek.Wednesday);
            daysOfWeek[5].Should().Be(DayOfWeek.Thursday);
            daysOfWeek[6].Should().Be(DayOfWeek.Friday);
        }

        [Fact]
        public void should_return_timezone_of_iran()
        {
            // Act
            var timezone = LocalTimezone.Create("Asia/Tehran");

            // Assert
            timezone.IanaId.Should().Be("Asia/Tehran");
            timezone.Id.Should().BeOneOf("Iran Standard Time", "Iran Daylight Time");
            timezone.ToString().Should().Be("GMT+03:30");
            timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("fa-IR"));
            timezone.Calendar.Should().Be(CalendarSystem.PersianSimple);
            timezone.FirstDayOfWeek.Should().Be(DayOfWeek.Saturday);

            var daysOfWeek = timezone.Map.OrderedDaysOfWeek;
            daysOfWeek[0].Should().Be(DayOfWeek.Saturday);
            daysOfWeek[1].Should().Be(DayOfWeek.Sunday);
            daysOfWeek[2].Should().Be(DayOfWeek.Monday);
            daysOfWeek[3].Should().Be(DayOfWeek.Tuesday);
            daysOfWeek[4].Should().Be(DayOfWeek.Wednesday);
            daysOfWeek[5].Should().Be(DayOfWeek.Thursday);
            daysOfWeek[6].Should().Be(DayOfWeek.Friday);
        }

        [Fact]
        public void should_return_timezone_of_turkey()
        {
            // Act
            var timezone = LocalTimezone.Create("Europe/Istanbul");

            // Assert
            timezone.IanaId.Should().Be("Europe/Istanbul");
            timezone.Id.Should().BeOneOf("Turkey Standard Time", "Turkey Daylight Time");
            timezone.ToString().Should().Be("GMT+03:00");
            timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("tr-TR"));
            timezone.Calendar.Should().Be(CalendarSystem.Gregorian);
            timezone.FirstDayOfWeek.Should().Be(DayOfWeek.Monday);

            var daysOfWeek = timezone.Map.OrderedDaysOfWeek;
            daysOfWeek[0].Should().Be(DayOfWeek.Monday);
            daysOfWeek[1].Should().Be(DayOfWeek.Tuesday);
            daysOfWeek[2].Should().Be(DayOfWeek.Wednesday);
            daysOfWeek[3].Should().Be(DayOfWeek.Thursday);
            daysOfWeek[4].Should().Be(DayOfWeek.Friday);
            daysOfWeek[5].Should().Be(DayOfWeek.Saturday);
            daysOfWeek[6].Should().Be(DayOfWeek.Sunday);
        }

        [Fact]
        public void should_return_timezone_of_los_angeles()
        {
            // Act
            var timezone = LocalTimezone.Create("America/Los_Angeles");

            // Assert
            timezone.IanaId.Should().Be("America/Los_Angeles");
            timezone.Id.Should().BeOneOf("Pacific Standard Time", "Pacific Daylight Time");
            timezone.ToString().Should().Be("GMT-08:00");
            timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("en-US"));
            timezone.Calendar.Should().Be(CalendarSystem.Gregorian);
            timezone.FirstDayOfWeek.Should().Be(DayOfWeek.Sunday);

            var daysOfWeek = timezone.Map.OrderedDaysOfWeek;
            daysOfWeek[0].Should().Be(DayOfWeek.Sunday);
            daysOfWeek[1].Should().Be(DayOfWeek.Monday);
            daysOfWeek[2].Should().Be(DayOfWeek.Tuesday);
            daysOfWeek[3].Should().Be(DayOfWeek.Wednesday);
            daysOfWeek[4].Should().Be(DayOfWeek.Thursday);
            daysOfWeek[5].Should().Be(DayOfWeek.Friday);
            daysOfWeek[6].Should().Be(DayOfWeek.Saturday);
        }

        [Fact]
        public void should_return_timezone_of_uk()
        {
            // Act
            var timezone = LocalTimezone.Create("Europe/London");

            // Assert
            timezone.IanaId.Should().Be("Europe/London");
            timezone.Id.Should().BeOneOf("GMT Standard Time", "GMT Daylight Time");
            timezone.ToString().Should().Be("GMT+00:00");
            timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("en-GB"));
            timezone.Calendar.Should().Be(CalendarSystem.Gregorian);
            timezone.FirstDayOfWeek.Should().Be(DayOfWeek.Monday);

            var daysOfWeek = timezone.Map.OrderedDaysOfWeek;
            daysOfWeek[0].Should().Be(DayOfWeek.Monday);
            daysOfWeek[1].Should().Be(DayOfWeek.Tuesday);
            daysOfWeek[2].Should().Be(DayOfWeek.Wednesday);
            daysOfWeek[3].Should().Be(DayOfWeek.Thursday);
            daysOfWeek[4].Should().Be(DayOfWeek.Friday);
            daysOfWeek[5].Should().Be(DayOfWeek.Saturday);
            daysOfWeek[6].Should().Be(DayOfWeek.Sunday);
        }

        [Fact]
        public void should_return_cached_timezone()
        {
            // Act
            var timezone = LocalTimezone.Create("Asia/Tehran");

            // Assert
            timezone.IanaId.Should().Be("Asia/Tehran");
            timezone.Id.Should().BeOneOf("Iran Standard Time", "Iran Daylight Time");
            timezone.ToString().Should().Be("GMT+03:30");
            timezone.Culture.Should().Be(CultureInfo.GetCultureInfo("fa-IR"));
            timezone.Calendar.Should().Be(CalendarSystem.PersianSimple);
            timezone.FirstDayOfWeek.Should().Be(DayOfWeek.Saturday);

            var daysOfWeek = timezone.Map.OrderedDaysOfWeek;
            daysOfWeek[0].Should().Be(DayOfWeek.Saturday);
            daysOfWeek[1].Should().Be(DayOfWeek.Sunday);
            daysOfWeek[2].Should().Be(DayOfWeek.Monday);
            daysOfWeek[3].Should().Be(DayOfWeek.Tuesday);
            daysOfWeek[4].Should().Be(DayOfWeek.Wednesday);
            daysOfWeek[5].Should().Be(DayOfWeek.Thursday);
            daysOfWeek[6].Should().Be(DayOfWeek.Friday);
        }

        [Fact]
        public void should_return_timezone_without_resolver()
        {
            // Act
            var timezone = LocalTimezone.Create("Europe/Paris");

            // Assert
            timezone.IanaId.Should().Be("Europe/Paris");
            timezone.Id.Should().BeOneOf("Romance Standard Time", "Central European Daylight Time");
            timezone.ToString().Should().Be("GMT+01:00");
            timezone.Culture.Should().Be(CultureInfo.InvariantCulture);
            timezone.Calendar.Should().Be(CalendarSystem.Iso);
            timezone.FirstDayOfWeek.Should().Be(DayOfWeek.Sunday);
        }

        [Fact]
        public void should_be_equal()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Asia/Tehran");

            timezone.Should().Be(timezone2);
        }

        [Fact]
        public void should_not_be_equal()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Europe/Istanbul");

            timezone.Should().NotBe(timezone2);
        }

        [Fact]
        public void should_not_be_equal_with_null()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");

            timezone.Should().NotBeNull();
        }

        [Fact]
        public void should_be_equal_by_operator()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Asia/Tehran");

            (timezone == timezone2).Should().BeTrue();
        }

        [Fact]
        public void should_not_be_equal_by_operator()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Europe/Istanbul");

            (timezone != timezone2).Should().BeTrue();
        }

        [Fact]
        public void should_be_casted_to_string()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            ((string)timezone).Should().Be("Asia/Tehran");
        }

        [Fact]
        public void should_be_casted_to_timezone()
        {
            var timezone = (LocalTimezone)"Asia/Tehran";
            timezone.IanaId.Should().Be("Asia/Tehran");
        }

        [Fact]
        public void should_return_zoned_date_time_from_utc_datetime_according_to_timezone()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var dateTime = new DateTime(2021, 1, 1, 12, 0, 5, DateTimeKind.Utc);
            var zonedDateTime = timezone.GetZonedDateTime(dateTime);

            zonedDateTime.Year.Should().Be(1399);
            zonedDateTime.Month.Should().Be(10);
            zonedDateTime.Day.Should().Be(12);
            zonedDateTime.Hour.Should().Be(15);
            zonedDateTime.Minute.Should().Be(30);
            zonedDateTime.Second.Should().Be(5);
        }

        [Fact]
        public void should_return_zoned_date_time_from_local_datetime_according_to_timezone()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var dateTime = new DateTime(2021, 1, 1, 12, 0, 0, DateTimeKind.Local);
            var zonedDateTime = timezone.GetZonedDateTime(dateTime);

            zonedDateTime.Year.Should().Be(1399);
            zonedDateTime.Month.Should().Be(10);
            zonedDateTime.Day.Should().Be(12);
            zonedDateTime.Hour.Should().Be(12);
            zonedDateTime.Minute.Should().Be(0);
            zonedDateTime.Second.Should().Be(0);
        }

        [Fact]
        public void should_return_offset_from_timezone()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var offset = timezone.Offset;

            offset.Should().Be(Offset.FromHoursAndMinutes(3, 30));
        }

        [Fact]
        public void should_be_comparable_greater_than()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Europe/Istanbul");

            (timezone > timezone2).Should().BeTrue();
        }

        [Fact]
        public void should_be_comparable_greater_than_or_equal()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Asia/Tehran");

            (timezone >= timezone2).Should().BeTrue();
        }

        [Fact]
        public void should_be_comparable_less_than()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Europe/Istanbul");

            (timezone2 < timezone).Should().BeTrue();
        }

        [Fact]
        public void should_be_comparable_less_than_or_equal()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Asia/Tehran");

            (timezone2 <= timezone).Should().BeTrue();
        }

        [Fact]
        public void should_be_comparable_hash_code()
        {
            var timezone = LocalTimezone.Create("Asia/Tehran");
            var timezone2 = LocalTimezone.Create("Asia/Tehran");

            timezone2.GetHashCode().Should().Be(timezone.GetHashCode());
        }
    }
}