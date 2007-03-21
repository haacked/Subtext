using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Subtext.Framework.Util.TimeZoneUtil
{
	public class UTCTimeZone : TimeZone
	{
		public override string DaylightName
		{
			get
			{
				return "UTC";
			}
		}

		public override DaylightTime GetDaylightChanges(int year)
		{
			return new DaylightTime(DateTime.MinValue, DateTime.MinValue, TimeSpan.FromTicks(0));
		}

		public override TimeSpan GetUtcOffset(DateTime time)
		{
			return TimeSpan.FromTicks(0);
		}

		public override bool IsDaylightSavingTime(DateTime time)
		{
			return false;
		}

		public override string StandardName
		{
			get
			{
				return "UTC";
			}
		}

		public override DateTime ToLocalTime(DateTime time)
		{
			return time;
		}

		public override DateTime ToUniversalTime(DateTime time)
		{
			return time;
		}
	}
}
