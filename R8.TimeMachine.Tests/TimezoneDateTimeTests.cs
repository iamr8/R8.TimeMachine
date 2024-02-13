using System;
using FluentAssertions;
using R8.TimeMachine.Tests.Resolvers;
using Xunit;

namespace R8.TimeMachine.Tests
{
    public class TimezoneDateTimeTests
    {
        public TimezoneDateTimeTests()
        {
            LocalTimezone.Mappings.Add(new IranTimezone());
            LocalTimezone.Mappings.Add(new TurkeyTimezone());
        }

        [Fact]
        public void should_return_tzdt_from_datetime()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timezone = LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone();


            var result = dateTime.ToTimezoneDateTime(timezone);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);
            result.DayOfWeek.Should().Be(DayOfWeek.Friday);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_from_datetime_according_to_current_timezone()
        {
            LocalTimezone.Current = LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone()!;

            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToTimezoneDateTime();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);
            result.DayOfWeek.Should().Be(DayOfWeek.Friday);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_from_start_of_week()
        {
            var result = new TimezoneDateTime(1402, 11, 24, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetStartOfWeek();

            Assert.Equal(1402, result.Year);
            Assert.Equal(11, result.Month);
            Assert.Equal(21, result.Day);

            Assert.Equal(0, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);
        }

        [Fact]
        public void should_return_tzdt_from_end_of_week()
        {
            var result = new TimezoneDateTime(1402, 11, 21, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetEndOfWeek();

            Assert.Equal(1402, result.Year);
            Assert.Equal(11, result.Month);
            Assert.Equal(27, result.Day);

            Assert.Equal(23, result.Hour);
            Assert.Equal(59, result.Minute);
            Assert.Equal(59, result.Second);
        }

        [Fact]
        public void should_return_tzdt_with_start_of_hour()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetStartOfHour();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_with_end_of_hour()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetEndOfHour();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(59, result.Minute);
            Assert.Equal(59, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_with_start_of_minute()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetStartOfMinute();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_with_end_of_minute()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetEndOfMinute();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(59, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_with_start_of_day()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetStartOfDay();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(0, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_with_end_of_day()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetEndOfDay();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(23, result.Hour);
            Assert.Equal(59, result.Minute);
            Assert.Equal(59, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_with_start_of_month()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetStartOfMonth();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(1, result.Day);

            Assert.Equal(0, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_with_end_of_month()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.GetEndOfMonth();

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(30, result.Day);

            Assert.Equal(23, result.Hour);
            Assert.Equal(59, result.Minute);
            Assert.Equal(59, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_from_local_date_time()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_from_local_date()
        {
            var result = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(0, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_from_datetime2()
        {
            var dateTime = new DateTime(2021, 3, 21, 12, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.Equal(1400, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(1, result.Day);

            Assert.Equal(15, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(31, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_from_datetime3()
        {
            var dateTime = new DateTime(2021, 3, 21, 23, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.Equal(1400, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(2, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(31, result.GetDaysInMonth());
        }

        [Fact]
        public void should_subtract_minute()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMinutes(-1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(29, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_minute()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMinutes(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(31, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_subtract_second()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddSeconds(-1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(29, result.Minute);
            Assert.Equal(59, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_second()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddSeconds(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(1, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_second_resulted_to_next_minute()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 4, 0, 59, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddSeconds(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(4, result.Hour);
            Assert.Equal(1, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_tzdt_with_different_timezone()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.WithTimezone(LocalTimezone.Mappings["Europe/Istanbul"].GetLocalTimezone());

            Assert.Equal(2021, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(1, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);
        }

        [Fact]
        public void should_add_minute_resulted_to_next_hour()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 3, 59, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMinutes(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(4, result.Hour);
            Assert.Equal(0, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_compare_to_other_while_are_equal()
        {
            var result = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.Equal(0, result.CompareTo(result2));
        }

        [Fact]
        public void should_add_month()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMonths(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(11, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_month2()
        {
            var dateTime = new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMonths(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(12, result.Month);
            Assert.Equal(13, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_year()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddYears(2);

            Assert.Equal(1401, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_subtract_year()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddYears(-2);

            Assert.Equal(1397, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_day()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            result = result.AddDays(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(13, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_days_resulted_to_new_month()
        {
            var dateTime = new DateTime(2021, 1, 20, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMonths(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(12, result.Month);
            Assert.Equal(1, result.Day);

            Assert.Equal(3, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_hour()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddHours(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(4, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_subtract_hour()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddHours(-1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(2, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_hour_resulted_to_next_day()
        {
            var result = new TimezoneDateTime(1399, 10, 12, 23, 30, 0, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddHours(1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(13, result.Day);

            Assert.Equal(0, result.Hour);
            Assert.Equal(30, result.Minute);
            Assert.Equal(0, result.Second);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_return_underlying_datetime()
        {
            var result = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            var underlyingDateTime = result.ToDateTimeUtc();
            Assert.Equal(2020, underlyingDateTime.Year);
            Assert.Equal(12, underlyingDateTime.Month);
            Assert.Equal(31, underlyingDateTime.Day);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_subtract_month()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMonths(-1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(9, result.Month);
            Assert.Equal(12, result.Day);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_subtract_twelve_months()
        {
            var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMonths(-12);

            Assert.Equal(1400, result.Year);
            Assert.Equal(11, result.Month);
            Assert.Equal(14, result.Day);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_add_month3()
        {
            var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddMonths(12);

            Assert.Equal(1402, result.Year);
            Assert.Equal(11, result.Month);
            Assert.Equal(14, result.Day);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_subtract_day()
        {
            var dateTime = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddDays(-1);

            Assert.Equal(1399, result.Year);
            Assert.Equal(10, result.Month);
            Assert.Equal(11, result.Day);

            Assert.Equal(DayOfWeek.Saturday, result.GetTimezone().FirstDayOfWeek);

            Assert.Equal(30, result.GetDaysInMonth());
        }

        [Fact]
        public void should_be_equal()
        {
            var result = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.True(result2 == result);
        }

        [Fact]
        public void should_not_be_equal()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.True(result2 != result);
        }

        [Fact]
        public void should_be_greater_than()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.True(result > result2);
        }

        [Fact]
        public void should_be_greater_than_or_equal_to()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.True(result >= result2);
        }

        [Fact]
        public void should_be_less_than_or_equal_to()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.True(result2 <= result);
        }

        [Fact]
        public void should_be_less_than()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.True(result2 < result);
        }

        [Theory]
        [InlineData(null, "1401/11/14 3:30:00")]
        [InlineData("d", "1401/11/14")]
        [InlineData("D", "1401 بهمن 14, جمعه")]
        [InlineData("f", "1401 بهمن 14, جمعه 3:30")]
        [InlineData("F", "1401 بهمن 14, جمعه 3:30:00")]
        [InlineData("g", "1401/11/14 3:30")]
        [InlineData("G", "1401/11/14 3:30:00")]
        [InlineData("m", "14 بهمن")]
        [InlineData("M", "14 بهمن")]
        [InlineData("MMMM", "بهمن")]
        public void should_return_formatted_string(string? format, string expected)
        {
            var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var resultString = result.ToString(format);

            Assert.Equal(expected, resultString);
        }

        [Fact]
        public void should_return_formatted_ordinal_string()
        {
            var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);


            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var resultString = result.ToString();

            Assert.Equal("1401/11/14 3:30:00", resultString);
        }

        [Fact]
        public void should_subtract_two_tzdt_by_operator()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            var result3 = result - result2;

            Assert.Equal(1, result3.Days);
        }

        [Fact]
        public void should_subtract_two_tzdt_by_operator2()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            var result3 = result2 - result;

            Assert.Equal(-1, result3.Days);
        }

        [Fact]
        public void should_subtract_two_tzdt_by_Subtract_method()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            var result3 = result2.Subtract(result);

            Assert.Equal(-1, result3.Days);
        }

        [Fact]
        public void should_subtract_tzdt_and_TimeSpan_by_Subtract_method()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = result.Subtract(TimeSpan.FromDays(1));

            Assert.Equal(1399, result2.Year);
            Assert.Equal(10, result2.Month);
            Assert.Equal(12, result2.Day);
        }

        [Fact]
        public void should_add_tzdt_and_TimeSpan_by_Add_method()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = result.Add(TimeSpan.FromDays(1));

            Assert.Equal(1399, result2.Year);
            Assert.Equal(10, result2.Month);
            Assert.Equal(14, result2.Day);
        }

        [Theory]
        [InlineData(1399, 10, 13, 1, 1399, 10, 14)]
        [InlineData(1399, 10, 30, 1, 1399, 11, 1)]
        public void should_add_tzdt_and_TimeSpan_by_operator(int year, int month, int day, int addingDays, int resultYear, int resultMonth, int resultDay)
        {
            var result = new TimezoneDateTime(year, month, day, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = result + TimeSpan.FromDays(addingDays);

            Assert.Equal(resultYear, result2.Year);
            Assert.Equal(resultMonth, result2.Month);
            Assert.Equal(resultDay, result2.Day);
        }

        [Fact]
        public void should_returns_ticks()
        {
            var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);
            var tzdt = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            dateTime.Ticks.Should().Be(tzdt.Ticks);
        }

        [Fact]
        public void should_be_equal_by_Equal_method()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.True(result2.Equals(result));
        }

        [Fact]
        public void should_not_be_equal_by_Equal_method()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            Assert.False(result2.Equals(result));
        }

        [Fact]
        public void should_subtract_two_tzdt_by_operator3()
        {
            var result = new TimezoneDateTime(1399, 10, 13, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            var result2 = new TimezoneDateTime(1399, 10, 12, LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());

            var result3 = result - result2;

            Assert.Equal(1, result3.Days);
        }

        [Fact]
        public void should_add_days()
        {
            var dateTime = new DateTime(2023, 2, 3, 0, 0, 0, DateTimeKind.Utc);
            var result = dateTime.ToTimezoneDateTime(LocalTimezone.Mappings["Asia/Tehran"].GetLocalTimezone());
            result = result.AddDays(1);
            Assert.Equal(1401, result.Year);
            Assert.Equal(11, result.Month);
            Assert.Equal(15, result.Day);
        }
    }
}