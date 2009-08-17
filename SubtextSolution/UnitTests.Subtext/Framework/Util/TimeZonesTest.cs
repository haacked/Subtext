using System;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
	[TestFixture]
	public class TimeZonesTest
	{
        public const string PacificTimeZoneId = "Pacific Standard Time";
        const string NewZealandZoneId = "New Zealand Standard Time";
        const string CentralEuropeZoneId = "Central Europe Standard Time";
        public const string HawaiiTimeZoneId = "Hawaiian Standard Time";

        [Test]
        public void CanGetTimeZones() 
        {
            // arrange, act
            var timeZones = TimeZones.GetTimeZones();
            foreach (var timeZone in timeZones)
            {
                Console.WriteLine(timeZone.Id.GetHashCode() + "\t" + timeZone.StandardName);
            }

            // assert
            Assert.Greater(timeZones.Count, 10);
        }

        
        [Test]
		public void GenerateUpdateScript()
		{
			string sql = string.Empty;
			string sqlFormat = "UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneId = '{0}' WHERE TimeZone = {1}" + Environment.NewLine + "GO" + Environment.NewLine;
			foreach(var timezone in TimeZones.GetTimeZones())
			{
				sql += String.Format(sqlFormat, timezone.Id, timezone.Id.GetHashCode());
			}
			Console.Write(sql);
		}
		
		[Test]//, Ignore("Only run this when we need to regen this file. Better to make this a build step.")]
		public void WriteTimeZonesToFile()
		{
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            foreach (var timeZone in timeZones) 
            {
                Console.WriteLine(timeZone.ToSerializedString());
            }

            //Type tzcType = timeZones.GetType();
            //XmlSerializer ser = new XmlSerializer(tzcType);
            //using (StreamWriter writer = new StreamWriter("c:\\WindowsTimeZoneCollection.xml", false, Encoding.UTF8))
            //{
            //    ser.Serialize(writer, timeZones);
            //}
		}
	}
}
