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
    // TimeZoneInfo is sealed. :(
    public class TimeZoneWrapper : ITimeZone
    {
        readonly Func<DateTime> _utcNowFactory;

        public TimeZoneWrapper(TimeZoneInfo timeZone)
            : this(timeZone, TimeZoneInfo.Local, () => DateTime.UtcNow)
        {
        }

        public TimeZoneWrapper(TimeZoneInfo timeZone, TimeZoneInfo serverTimeZone, Func<DateTime> utcNowFactory)
        {
            TimeZoneInfo = timeZone;
            ServerTimeZoneInfo = serverTimeZone;
            _utcNowFactory = utcNowFactory;
        }

        protected TimeZoneInfo TimeZoneInfo { get; private set; }

        protected TimeZoneInfo ServerTimeZoneInfo { get; private set; }

        public DateTime UtcNow
        {
            get { return _utcNowFactory(); }
        }

        public DateTime Now
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(UtcNow, TimeZoneInfo); }
        }

        public DateTime ServerNow
        {
            get { return TimeZoneInfo.ConvertTimeFromUtc(UtcNow, ServerTimeZoneInfo); }
        }

        public DateTime ToUtc(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo, TimeZoneInfo.Utc);
        }

        public DateTime FromUtc(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Unspecified);
            }
            return FromTimeZone(dateTime, TimeZoneInfo.Utc);
        }

        public DateTime FromTimeZone(DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, TimeZoneInfo);
        }

        public DateTime ToServerDateTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo, ServerTimeZoneInfo);
        }

        public bool IsInPast(DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            return FromTimeZone(dateTime, sourceTimeZone) < Now;
        }

        public bool IsInFuture(DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            return FromTimeZone(dateTime, sourceTimeZone) > Now;
        }

        public int Offset
        {
            get
            {
                return TimeZoneInfo.BaseUtcOffset.Hours;
            }
        }
    }
}