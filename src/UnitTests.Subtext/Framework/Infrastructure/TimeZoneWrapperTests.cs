using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Util;
using Subtext.Infrastructure;

namespace UnitTests.Subtext.Framework.Infrastructure
{
    [TestClass]
    public class TimeZoneWrapperTests
    {
        [TestMethod]
        public void Now_ReturnsTimeInLocalTimeZone()
        {
            // arrange
            DateTime utcNow = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt",
                                                  CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            TimeZoneInfo timeZone = TimeZones.GetTimeZones().GetById("Mountain Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(timeZone, TimeZoneInfo.Local, () => utcNow);

            // act
            DateTime now = timeZoneWrapper.Now;

            // assert
            DateTime expected = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            Assert.AreEqual(expected, now);
        }

        [TestMethod]
        public void UtcNow_ReturnsSpecifiedUtcNow()
        {
            // arrange
            DateTime expectedUtcNow = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt",
                                                          CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var timeZoneWrapper = new TimeZoneWrapper(TimeZoneInfo.Local, TimeZoneInfo.Local, () => expectedUtcNow);

            // act
            DateTime utcNow = timeZoneWrapper.UtcNow;

            // assert
            Assert.AreEqual(expectedUtcNow, utcNow);
        }

        [TestMethod]
        public void ServerNow_ReturnsLocalTimeOnServer()
        {
            // arrange
            DateTime utcNow = DateTime.ParseExact("2009/08/15 11:00 PM", "yyyy/MM/dd hh:mm tt",
                                                  CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            TimeZoneInfo blogTimeZone = TimeZones.GetTimeZones().GetById("Korea Standard Time");
            TimeZoneInfo serverTimeZone = TimeZones.GetTimeZones().GetById("Mountain Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, serverTimeZone, () => utcNow);

            // act
            DateTime serverNow = timeZoneWrapper.ServerNow;

            // assert
            DateTime expected = DateTime.ParseExact("2009/08/15 05:00 PM", "yyyy/MM/dd hh:mm tt",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            Assert.AreEqual(expected, serverNow);
        }

        [TestMethod]
        public void ToUtc_ConvertsSpecifiedTimeInTimeZone_ToUtcTime()
        {
            // arrange
            TimeZoneInfo blogTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => DateTime.UtcNow);
            DateTime expectedUtcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt",
                                                               CultureInfo.InvariantCulture,
                                                               DateTimeStyles.AdjustToUniversal);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(expectedUtcDateTime, blogTimeZone);

            // act
            DateTime utc = timeZoneWrapper.ToUtc(tokyoDateTime);

            // assert
            Assert.AreEqual(expectedUtcDateTime, utc);
        }

        [TestMethod]
        public void ToServerDateTime_ConvertsSpecifiedTimeInTimeZone_ToServerTimeZone()
        {
            // arrange
            TimeZoneInfo blogTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            TimeZoneInfo serverTimeZone = TimeZones.GetTimeZones().GetById("Iran Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, serverTimeZone, () => DateTime.UtcNow);
            DateTime expectedUtcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt",
                                                               CultureInfo.InvariantCulture,
                                                               DateTimeStyles.AdjustToUniversal);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(expectedUtcDateTime, blogTimeZone);
            DateTime iranDateTime = TimeZoneInfo.ConvertTimeFromUtc(expectedUtcDateTime, serverTimeZone);

            // act
            DateTime serverDateTime = timeZoneWrapper.ToServerDateTime(tokyoDateTime);

            // assert
            Assert.AreEqual(iranDateTime, serverDateTime);
        }

        [TestMethod]
        public void FromUtc_ConvertsSpecifiedTime_ToBlogTimeZoneFromUtc()
        {
            // arrange
            TimeZoneInfo blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => DateTime.UtcNow);
            DateTime utcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt",
                                                       CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

            // act
            DateTime fijiDateTime = timeZoneWrapper.FromUtc(utcDateTime); // To Fiji Time Zone

            // assert
            DateTime expected = DateTime.ParseExact("2009/08/16 06:18 AM", "yyyy/MM/dd hh:mm tt",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            Assert.AreEqual(expected, fijiDateTime);
        }

        [TestMethod]
        public void FromUtc_ConvertsSpecifiedLocalTime_ToBlogTimeZoneFromUtc()
        {
            // arrange
            var time = new DateTime(2009, 08, 15, 18, 18, 0, DateTimeKind.Local);
            TimeZoneInfo blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => DateTime.UtcNow);
            
            // act
            var fijiDateTime = timeZoneWrapper.FromUtc(time); // To Fiji Time Zone

            // assert
            DateTime expected = DateTime.ParseExact("2009/08/16 06:18 AM", "yyyy/MM/dd hh:mm tt",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            Assert.AreEqual(expected, fijiDateTime);
        }

        [TestMethod]
        public void FromTimeZone_ConvertsSpecifiedTime_ToBlogTimeZoneFromSpecifiedTimeZone()
        {
            // arrange
            TimeZoneInfo blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            TimeZoneInfo tokyoTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => DateTime.UtcNow);
            DateTime utcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt",
                                                       CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tokyoTimeZone);

            // act
            DateTime fijiDateTime = timeZoneWrapper.FromTimeZone(tokyoDateTime, tokyoTimeZone); // To Fiji Time Zone

            // assert
            DateTime expected = DateTime.ParseExact("2009/08/16 06:18 AM", "yyyy/MM/dd hh:mm tt",
                                                    CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            Assert.AreEqual(expected, fijiDateTime);
        }

        [TestMethod]
        public void IsInPast_WithDateInPast_ReturnsTrue()
        {
            // arrange
            TimeZoneInfo blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            TimeZoneInfo tokyoTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            DateTime utcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt",
                                                       CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            var timeZoneWrapper = new TimeZoneWrapper(blogTimeZone, TimeZoneInfo.Local, () => utcDateTime);
            DateTime tokyoDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tokyoTimeZone).AddHours(-1);

            // act
            bool isInPast = timeZoneWrapper.IsInPast(tokyoDateTime, tokyoTimeZone);
            bool isInFuture = timeZoneWrapper.IsInFuture(tokyoDateTime, tokyoTimeZone);

            // assert
            Assert.IsTrue(isInPast);
            Assert.IsFalse(isInFuture);
        }

        [TestMethod]
        public void IsInPast_WithDateInFuture_ReturnsFalse()
        {
            // arrange
            TimeZoneInfo blogTimeZone = TimeZones.GetTimeZones().GetById("Fiji Standard Time");
            TimeZoneInfo tokyoTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            DateTime utcDateTime = DateTime.ParseExact("2009/08/15 06:18 PM", "yyyy/MM/dd hh:mm tt",
                                                       CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
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