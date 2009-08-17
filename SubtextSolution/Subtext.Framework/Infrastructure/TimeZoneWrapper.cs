#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subtext.Framework.Infrastructure
{
    // TimeZoneInfo is sealed. :(
    public class TimeZoneWrapper : ITimeZone
    {
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
        Func<DateTime> _utcNowFactory;

        public DateTime UtcNow
        {
            get 
            {
                return _utcNowFactory();
            }
        }

        protected TimeZoneInfo TimeZoneInfo
        {
            get;
            private set;
        }

        protected TimeZoneInfo ServerTimeZoneInfo
        {
            get;
            private set;
        }

        public DateTime Now
        {
            get
            {
                return TimeZoneInfo.ConvertTimeFromUtc(UtcNow, TimeZoneInfo);
            }
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
    }
}
