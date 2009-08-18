﻿using System;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class ServerTimeZoneInfoTests
    {
        [Test]
        public void ctor_WhenServerAndLocalTimeZonesAreSame_ShowsSameTime() { 
            //arrange
            var pacificTimeZone = TimeZones.GetTimeZones().GetById("Pacific Standard Time");
            var serverTimeZone = TimeZones.GetTimeZones().GetById("Pacific Standard Time");
            var now = DateTime.ParseExact("2009/08/11 11:50 PM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture);
            var utcNow = now.ToUniversalTime();

            //act
            var info = new ServerTimeZoneInfo(pacificTimeZone, serverTimeZone, now, utcNow);

            //assert
            Assert.AreEqual("2009/08/11 11:50 PM", info.CurrentTime);
            Assert.AreEqual("2009/08/11 11:50 PM", info.ServerTime);
            Assert.AreEqual("Pacific Standard Time (-07:00:00)", info.ServerTimeZone);
            Assert.AreEqual("2009/08/12 06:50 AM", info.ServerUtcTime);
        }

        [Test]
        public void ctor_WhenServerAndLocalTimeZonesAreDifferent_ShowsDifferentTimes()
        {
            //arrange
            var pacificTimeZone = TimeZones.GetTimeZones().GetById("Tokyo Standard Time");
            var serverTimeZone = TimeZones.GetTimeZones().GetById("Pacific Standard Time");
            var now = DateTime.ParseExact("2009/08/12 12:03 AM", "yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture);
            var utcNow = now.ToUniversalTime();

            //act
            var info = new ServerTimeZoneInfo(pacificTimeZone, serverTimeZone, now, utcNow);

            //assert
            Assert.AreEqual("2009/08/12 04:03 PM", info.CurrentTime);
            Assert.AreEqual("2009/08/12 12:03 AM", info.ServerTime);
            Assert.AreEqual("Pacific Standard Time (-07:00:00)", info.ServerTimeZone);
            Assert.AreEqual("2009/08/12 07:03 AM", info.ServerUtcTime);
        }
    }
}