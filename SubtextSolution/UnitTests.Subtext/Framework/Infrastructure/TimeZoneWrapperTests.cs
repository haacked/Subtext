using System;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework.Infrastructure;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Infrastructure
{
    [TestFixture]
    public class TimeZoneWrapperTests
    {
        [Test]
        public void Now_ReturnsTimeInLocalTimeZone()
        {
            // arrange
            DateTime utcNow = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var timeZone = TimeZones.GetTimeZones().GetById("Mountain Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(timeZone, TimeZoneInfo.Local, () => utcNow);

            // act
            var now = timeZoneWrapper.Now;

            // assert
            DateTime expected = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            Assert.AreEqual(expected, now);
        }

        [Test]
        public void UtcNow_ReturnsSpecifiedUtcNow()
        {
            // arrange
            DateTime expectedUtcNow = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var timeZoneWrapper = new TimeZoneWrapper(TimeZoneInfo.Local, TimeZoneInfo.Local, () => expectedUtcNow);

            // act
            var utcNow = timeZoneWrapper.UtcNow;

            // assert
            Assert.AreEqual(expectedUtcNow, utcNow);
        }

        [Test]
        public void ServerNow_ReturnsLocalTimeOnServer()
        {
            // arrange
            var utcNow = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var blogTimeZone = TimeZones.GetTimeZones().GetById("Korea Standard Time");
            var serverTimeZone = TimeZones.GetTimeZones().GetById("Mountain Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, serverTimeZone, () => utcNow);
            
            // act
            var serverNow = timeZoneWrapper.ServerNow;

            // assert
            DateTime expected = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            Assert.AreEqual(expected, serverNow);
        }

        [Test]
        public void ToUtc_ConvertsSpecifiedTimeInTimeZone_ToUtcTime()
        {
            // arrange
            var blogTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => DateTime.UtcNow);
            DateTime expectedUtcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(expectedUtcDateTime, blogTimeZone);

            // act
            var utc = timeZoneWrapper.ToUtc(tokyoDateTime);

            // assert
            Assert.AreEqual(expectedUtcDateTime, utc);
        }

        [Test]
        public void ToServerDateTime_ConvertsSpecifiedTimeInTimeZone_ToServerTimeZone()
        {
            // arrange
            var blogTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            var serverTimeZone = TimeZones.GetTimeZones().GetById("Iran Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, serverTimeZone, () => DateTime.UtcNow);
            DateTime expectedUtcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(expectedUtcDateTime, blogTimeZone);
            DateTime iranDateTime = TimeZoneInfo.ConvertTimeFromUtc(expectedUtcDateTime, serverTimeZone);

            // act
            var serverDateTime = timeZoneWrapper.ToServerDateTime(tokyoDateTime);

            // assert
            Assert.AreEqual(iranDateTime, serverDateTime);
        }

        [Test]
        public void FromUtc_ConvertsSpecifiedTime_ToBlogTimeZoneFromUtc()
        {
            // arrange
            var blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            var utcTimeZone = TimeZoneInfo.Utc;
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => DateTime.UtcNow);
            DateTime utcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

            // act
            var fijiDateTime = timeZoneWrapper.FromUtc(utcDateTime); // To Fiji Time Zone

            // assert
            var expected = DateTime.ParseExact("2009/08/16 06:18 AM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            Assert.AreEqual(expected, fijiDateTime);
        }

        [Test]
        public void FromTimeZone_ConvertsSpecifiedTime_ToBlogTimeZoneFromSpecifiedTimeZone()
        {
            // arrange
            var blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            var tokyoTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => DateTime.UtcNow);
            DateTime utcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tokyoTimeZone);

            // act
            var fijiDateTime = timeZoneWrapper.FromTimeZone(tokyoDateTime, tokyoTimeZone); // To Fiji Time Zone

            // assert
            var expected = DateTime.ParseExact("2009/08/16 06:18 AM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            Assert.AreEqual(expected, fijiDateTime);
        }

        [Test]
        public void IsInPast_WithDateInPast_ReturnsTrue()
        {
            // arrange
            var blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            var tokyoTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            DateTime utcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => utcDateTime);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tokyoTimeZone).AddHours(-1);

            // act
            bool isInPast = timeZoneWrapper.IsInPast(tokyoDateTime, tokyoTimeZone);
            bool isInFuture = timeZoneWrapper.IsInFuture(tokyoDateTime, tokyoTimeZone);

            // assert
            Assert.IsTrue(isInPast);
            Assert.IsFalse(isInFuture);
        }

        [Test]
        public void IsInPast_WithDateInFuture_ReturnsFalse()
        {
            // arrange
            var blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            var tokyoTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            DateTime utcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => utcDateTime);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tokyoTimeZone).AddHours(1);

            // act
            bool isInPast = timeZoneWrapper.IsInPast(tokyoDateTime, tokyoTimeZone);
            bool isInFuture = timeZoneWrapper.IsInFuture(tokyoDateTime, tokyoTimeZone);

            // assert
            Assert.IsFalse(isInPast);
            Assert.IsTrue(isInFuture);
        }
    }
}
