using System;
using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
	[TestFixture]
	public class BlogTimeTests
	{
		/// <summary>
		/// Determines whether this instance a time from the server's timezone to the 
		/// blogger's timezone.
		/// </summary>
		/// <param name="serverTimeZone">The server time zone.</param>
		/// <param name="clientTimeZone">The client time zone.</param>
		/// <param name="serverDateTime">The server date time.</param>
		/// <param name="expectedBloggerDate">The expected blogger date.</param>
		[RowTest]
		[Row(-8, -8, "01/23/2006 16:00", "01/23/2006 16:00")] //no change
		[Row(-7, -8, "01/23/2006 17:00", "01/23/2006 16:00")]
		[Row(-9, -8, "01/23/2006 15:00", "01/23/2006 16:00")]
		public void CanConvertToBloggerTime(int serverTimeZone, int clientTimeZone, string serverDateTime, string expectedBloggerDate)
		{
			DateTime serverDate = DateTime.ParseExact(serverDateTime, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
			Config.Settings.ServerTimeZone = serverTimeZone;
			DateTime blogTime = BlogTime.ConvertToBloggerTime(serverDate, clientTimeZone);

			DateTime expected = DateTime.ParseExact(expectedBloggerDate, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
			Assert.AreEqual(expected, blogTime, "The conversion did not match up");
			
		}

		/// <summary>
		/// Determines whether this instance can convert a datetime from the blogger's 
		/// perspective to the server's timezone.
		/// </summary>
		/// <param name="bloggerTimeZone">The server time zone.</param>
		/// <param name="serverTimeZone">The client time zone.</param>
		/// <param name="bloggerDate">The server date time.</param>
		/// <param name="expectedServerDate">The expected blogger date.</param>
		[RowTest]
		[Row(-8, -8, "01/23/2006 16:00", "01/23/2006 16:00")] //No change
		[Row(-7, -8, "01/23/2006 16:00", "01/23/2006 15:00")]
		[Row(-9, -8, "01/23/2006 17:00", "01/23/2006 18:00")]
		public void CanConvertToServerTime(int bloggerTimeZone, int serverTimeZone, string bloggerDate, string expectedServerDate)
		{
			DateTime bloggerDateTime = DateTime.ParseExact(bloggerDate, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
			Config.Settings.ServerTimeZone = serverTimeZone;
			DateTime serverTime = BlogTime.ConvertToServerTime(bloggerDateTime, bloggerTimeZone);

			DateTime expected = DateTime.ParseExact(expectedServerDate, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
			Assert.AreEqual(expected, serverTime, "The conversion did not match up");
		}
	}
}
