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
using Subtext.Framework.Util;

namespace Subtext.Framework.Components
{
    [Serializable]
    public class ServerTimeZoneInfo
    {
        public ServerTimeZoneInfo()
        {
        }

        public ServerTimeZoneInfo(string timeZoneText)
            : this(TimeZones.GetTimeZones().GetById(timeZoneText), TimeZoneInfo.Local, DateTime.Now, DateTime.UtcNow)
        {
        }

        public ServerTimeZoneInfo(TimeZoneInfo timeZone, TimeZoneInfo localTimeZone, DateTime now, DateTime utcNow)
        {
            ServerTimeZone = string.Format(CultureInfo.InvariantCulture, "{0} ({1})",
                                           localTimeZone.StandardName,
                                           localTimeZone.GetUtcOffset(now));
            ServerTime = now.ToString("yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture);
            ServerUtcTime = utcNow.ToString("yyyy/MM/dd hh:mm tt", CultureInfo.InvariantCulture);
            CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZone).ToString("yyyy/MM/dd hh:mm tt",
                                                                                     CultureInfo.InvariantCulture);
        }

        public string ServerTimeZone { get; set; }

        public string ServerTime { get; set; }

        public string ServerUtcTime { get; set; }

        public string CurrentTime { get; set; }
    }
}