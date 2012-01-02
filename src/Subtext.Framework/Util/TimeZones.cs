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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Subtext.Framework.Util
{
    public static class TimeZones
    {
        private static readonly ReadOnlyCollection<TimeZoneInfo> _timeZones = ReadTimeZonesFromEmbeddedResource();

        private static ReadOnlyCollection<TimeZoneInfo> ReadTimeZonesFromEmbeddedResource()
        {
            Type timeZoneType = typeof(TimeZones);
            var timeZones = new List<TimeZoneInfo>();
            using (var reader = new StreamReader(timeZoneType.Assembly.GetManifestResourceStream(timeZoneType.FullName + ".txt")))
            {
                while (!reader.EndOfStream)
                {
                    timeZones.Add(TimeZoneInfo.FromSerializedString(reader.ReadLine()));
                }
            }
            return new ReadOnlyCollection<TimeZoneInfo>(timeZones);
        }

        public static ReadOnlyCollection<TimeZoneInfo> GetTimeZones()
        {
            return _timeZones;
        }

        public static TimeZoneInfo GetById(this ReadOnlyCollection<TimeZoneInfo> timeZones, string timeZoneId)
        {
            return (from timeZone in timeZones
                    where timeZone.Id == timeZoneId
                    select timeZone).FirstOrDefault();
        }

        public static DateTime AsUtc(this DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
        }
    }
}