using System;
using System.Globalization;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used for helping with date times.
	/// </summary>
	public sealed class DateTimeHelper
	{
		private DateTimeHelper()
		{
		}

		/// <summary>
		/// Tries to parse the date given in an unknown format. Returns 
		/// DateTime.MinValue if it cannot.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns></returns>
		public static DateTime ParseUnknownFormatUTC(string dateTime)
		{
			DateTime dt = DateTime.MinValue;
			try
			{
				dt = DateTime.Parse(dateTime);
			}
			catch(System.FormatException)
			{
			}

			string[] formats = {"r", "s", "u", "yyyyMMddTHHmmss"};

			foreach(string dateFormat in formats)
			{
				try
				{
					dt = DateTime.ParseExact(dateTime, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
					break;
				}
				catch(System.FormatException)
				{
				}
			}
			return dt;
		}
	}
}
