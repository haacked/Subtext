using System;
using System.Globalization;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used for helping with date times.
	/// </summary>
	public static class DateTimeHelper
	{
		/// <summary>
		/// Tries to parse the date given in an unknown format. Returns 
		/// NullValue.NullDateTime if it cannot.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns></returns>
		public static DateTime ParseUnknownFormatUTC(string dateTime)
		{
			DateTime dt = NullValue.NullDateTime;
			try
			{
				return DateTime.Parse(dateTime, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
			}
			catch(System.FormatException)
			{
			}

			string[] formats = {"r", "s", "u", "yyyyMMddTHHmmss"};

			foreach(string dateFormat in formats)
			{
				try
				{
					return DateTime.ParseExact(dateTime, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
				}
				catch(System.FormatException)
				{
				}
			}
			return dt;
		}
	}
}
