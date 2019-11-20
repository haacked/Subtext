using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
    [TestClass]
    public class TimeZonesTest
    {
        const string CentralEuropeZoneId = "Central Europe Standard Time";
        public const string HawaiiTimeZoneId = "Hawaiian Standard Time";
        const string NewZealandZoneId = "New Zealand Standard Time";
        public const string PacificTimeZoneId = "Pacific Standard Time";

        [TestMethod]
        public void CanGetTimeZones()
        {
            // arrange, act
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZones.GetTimeZones();
            foreach(TimeZoneInfo timeZone in timeZones)
            {
                Console.WriteLine(timeZone.Id.GetHashCode() + "\t" + timeZone.StandardName);
            }

            // assert
            Assert.IsTrue(timeZones.Count > 10);
        }


        [TestMethod]
        public void GenerateUpdateScript()
        {
            string sql = string.Empty;
            string sqlFormat =
                "UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = '{0}' WHERE TimeZone = {1}" +
                Environment.NewLine + "GO" + Environment.NewLine;
            foreach(TimeZoneInfo timezone in TimeZones.GetTimeZones())
            {
                sql += String.Format(sqlFormat, timezone.Id, timezone.Id.GetHashCode());
            }
            Console.Write(sql);
        }

        //[TestMethod]
        //[Ignore("Only run this when we need to regen this file. Better to make this a build step.")]
        //public void WriteTimeZonesToFile()
        //{
        //    ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
        //    foreach(TimeZoneInfo timeZone in timeZones)
        //    {
        //        Console.WriteLine(timeZone.ToSerializedString());
        //    }

        //    Type tzcType = timeZones.GetType();
        //    XmlSerializer ser = new XmlSerializer(tzcType);
        //    using(StreamWriter writer = new StreamWriter("c:\\WindowsTimeZoneCollection.xml", false, Encoding.UTF8))
        //    {
        //        ser.Serialize(writer, timeZones);
        //    }
        //}
    }
}