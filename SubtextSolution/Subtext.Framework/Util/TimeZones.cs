#region Timezone code from DasBlog

/*This code was contributed by the DasBlog team */

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
            using(
                var rs =
                    new StreamReader(timeZoneType.Assembly.GetManifestResourceStream(timeZoneType.FullName + ".txt")))
            {
                while(!rs.EndOfStream)
                {
                    timeZones.Add(TimeZoneInfo.FromSerializedString(rs.ReadLine()));
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
    }
}