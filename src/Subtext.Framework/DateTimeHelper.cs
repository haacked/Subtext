#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Globalization;
using System.IO;
using Subtext.Framework.Properties;

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
        public static DateTime ParseUnknownFormatUtc(string dateTime)
        {
            DateTime dt = NullValue.NullDateTime;
            try
            {
                return DateTime.Parse(dateTime, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            }
            catch (FormatException)
            {
            }

            string[] formats = { "r", "s", "u", "yyyyMMddTHHmmss" };

            foreach (string dateFormat in formats)
            {
                try
                {
                    return DateTime.ParseExact(dateTime, dateFormat, CultureInfo.InvariantCulture,
                                               DateTimeStyles.AdjustToUniversal);
                }
                catch (FormatException)
                {
                }
            }
            return dt;
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> instance parsed from the url.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static DateTime DateFromUrl(string url)
        {
            string date = Path.GetFileNameWithoutExtension(url);
            var en = new CultureInfo("en-US");
            switch (date.Length)
            {
                case 8:
                    return DateTime.ParseExact(date, "MMddyyyy", en);
                case 6:
                    return DateTime.ParseExact(date, "MMyyyy", en);
                default:
                    throw new InvalidOperationException(Resources.InvalidOperation_InvalidDateFormat);
            }
        }
    }
}