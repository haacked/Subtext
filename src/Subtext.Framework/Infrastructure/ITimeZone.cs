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

namespace Subtext.Infrastructure
{
    public interface ITimeZone
    {
        /// <summary>
        /// This is the current date and time in this time zone
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// This is the current UTC date and time
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// This is the current date and time based on the server's time zone.
        /// </summary>
        DateTime ServerNow { get; }

        /// <summary>
        /// Converts the specified DateTime instance to UTC
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        DateTime ToUtc(DateTime dateTime);

        /// <summary>
        /// Converts the specified UTC DateTime to this timezone.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        DateTime FromUtc(DateTime dateTime);

        /// <summary>
        /// Converts the specified DateTime instance to the server's (local) time zone
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        DateTime ToServerDateTime(DateTime dateTime);

        /// <summary>
        /// Converts the specified DateTime instance to this time zone from the specified time zone.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="sourceTimeZone"></param>
        /// <returns></returns>
        DateTime FromTimeZone(DateTime dateTime, TimeZoneInfo sourceTimeZone);

        /// <summary>
        /// Returns true if the specified date is in the past, otherwise false.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="sourceTimeZone"></param>
        /// <returns></returns>
        bool IsInPast(DateTime dateTime, TimeZoneInfo sourceTimeZone);

        /// <summary>
        /// Returns true if the specified date is in the future, otherwise false.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="sourceTimeZone"></param>
        /// <returns></returns>
        bool IsInFuture(DateTime dateTime, TimeZoneInfo sourceTimeZone);

        /// <summary>
        /// TimeZone Offset from UTC
        /// </summary>
        int Offset { get; }
    }
}