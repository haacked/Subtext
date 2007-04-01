using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Subtext.Framework.Util.TimeZoneUtil
{
	/// <summary>
	/// This is a specialization of the abstract TimeZone class
	/// that implements time zones based in the time zone information
	/// found in Windows.
	/// </summary>
	[Serializable]
	public class WindowsTimeZone : TimeZone
	{
		private string displayName;
		private string daylightZoneName;
		private string standardZoneName;
		private int zoneIndex;
		private TimeSpan baseBias;
		private TimeSpan standardBias;
		private TimeSpan daylightBias;
		private WindowsTZI winTZI;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowsTimeZone"/> class.
		/// </summary>
		public WindowsTimeZone()
		{
		}

		internal WindowsTimeZone(string displayName, string daylightZoneName, string standardZoneName, int zoneIndex, byte[] tzi)
		{
			this.winTZI.bias = 0;
			this.displayName = displayName.Trim();
			this.daylightZoneName = daylightZoneName.Trim();
			this.standardZoneName = standardZoneName.Trim();
			this.zoneIndex = zoneIndex;

			this.winTZI.InitializeFromByteArray(tzi, 0);
			this.baseBias = TimeSpan.FromMinutes(winTZI.bias * -1);
			this.standardBias = TimeSpan.FromMinutes(winTZI.standardBias * -1);
			this.daylightBias = TimeSpan.FromMinutes(winTZI.daylightBias * -1);
		}

		/// <summary>
		/// Gets the id.
		/// </summary>
		/// <value>The id.</value>
		public int Id
		{
			get
			{
				return this.StandardName.GetHashCode();
			}
		}

		/// <summary>
		/// Gets the current time in this timezone.
		/// </summary>
		/// <value>The now.</value>
		public DateTime Now
		{
			get
			{
				return ToLocalTime(DateTime.UtcNow);
			}
		}

		/// <summary>
		/// A friendly name for the timezone.
		/// </summary>
		/// <value>The name of the display.</value>
		public string DisplayName { get { return displayName; } set { this.displayName = value; } }
		public string DaylightZoneName { get { return daylightZoneName; } set { this.daylightZoneName = value; } }
		public string StandardZoneName { get { return standardZoneName; } set { this.standardZoneName = value; } }
		public int ZoneIndex { get { return zoneIndex; } set { this.zoneIndex = value; } }
		public WindowsTZI WinTZI
		{
			get { return winTZI; }
			set
			{
				this.winTZI = value;
				this.baseBias = TimeSpan.FromMinutes(winTZI.bias * -1);
				this.standardBias = TimeSpan.FromMinutes(winTZI.standardBias * -1);
				this.daylightBias = TimeSpan.FromMinutes(winTZI.daylightBias * -1);
			}
		}

		public TimeSpan BaseBias { get { return baseBias; } }
		public TimeSpan StandardBias { get { return standardBias; } }
		public TimeSpan DaylightBias { get { return daylightBias; } }

		public override string DaylightName
		{
			get
			{
				return daylightZoneName;
			}
		}

		public override string ToString()
		{
			return displayName;
		}


		/// <summary>
		/// Gets a timezone the by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public static WindowsTimeZone GetById(int id)
		{
			return TimeZones.GetById(id);
		}

		private static object daylightChangesLock = new object();
		private static Hashtable daylightChanges = new Hashtable();

		public override DaylightTime GetDaylightChanges(int year)
		{
			//Check cache...
			//This was broken.
			DaylightTime retVal = daylightChanges[this.zoneIndex + ":" + year] as DaylightTime;

			if (retVal == null)
			{
				lock (daylightChangesLock)
				{
					DateTime start = DateTime.MinValue;
					DateTime end = DateTime.MinValue;
					TimeSpan delta = TimeSpan.FromTicks(0);

					if (winTZI.daylightDate.month != 0 && winTZI.standardDate.month != 0)
					{
						// day count is a value from 1 to 5, indicating the day 
						// in the month on which the switch occurs
						int dayCount = winTZI.daylightDate.day;
						DateTime current = start = new DateTime(year, winTZI.daylightDate.month, 1, winTZI.daylightDate.hour, winTZI.daylightDate.minute, winTZI.daylightDate.second);
						while (current.Month == winTZI.daylightDate.month)
						{
							if (Convert.ToUInt16(current.DayOfWeek, NumberFormatInfo.InvariantInfo) == winTZI.daylightDate.dayOfWeek)
							{
								start = current;
								--dayCount;
								if (dayCount == 0)
								{
									break;
								}
							}
							current = current.AddDays(1);
						}

						// day count is a value from 1 to 5, indicating the day 
						// in the month on which the switch occurs
						dayCount = winTZI.standardDate.day;
						current = end = new DateTime(year, winTZI.standardDate.month, 1, winTZI.standardDate.hour, winTZI.standardDate.minute, winTZI.standardDate.second);
						while (current.Month == winTZI.standardDate.month)
						{
							if (Convert.ToUInt16(current.DayOfWeek, NumberFormatInfo.InvariantInfo) == winTZI.standardDate.dayOfWeek)
							{
								end = current;
								--dayCount;
								if (dayCount == 0)
								{
									break;
								}
							}
							current = current.AddDays(1);
						}
						delta = daylightBias - standardBias;
					}
					retVal = new DaylightTime(start, end, delta);
					if (daylightChanges.ContainsKey(this.zoneIndex + ":" + year) == false)
					{
						daylightChanges.Add(this.zoneIndex + ":" + year, retVal);
					}
				}
			}
			return retVal;
		}

		public override TimeSpan GetUtcOffset(DateTime time)
		{
			if (IsDaylightSavingTime(time))
			{
				return baseBias + daylightBias;
			}
			else
			{
				return baseBias + standardBias;
			}
		}

		public override string StandardName
		{
			get
			{
				return standardZoneName;
			}
		}

		/// <summary>
		/// Returns the local time (for this timezone) that corresponds to a specified coordinated universal time (UTC).
		/// </summary>
		/// <param name="time">A UTC time.</param>
		/// <returns>
		/// A <see cref="T:System.DateTime"></see> instance whose value is the local time that corresponds to time.
		/// </returns>
		public override DateTime ToLocalTime(DateTime time)
		{
			if (time.Kind == DateTimeKind.Local)
				return time;

			bool isAmbiguousLocalDst;
			long offset = this.GetUtcOffsetFromUniversalTime(time, out isAmbiguousLocalDst);
			long newTimeInTicks = time.Ticks + offset;

			if (newTimeInTicks > 0x2bca2875f4373fff)
			{
				return new DateTime(0x2bca2875f4373fff, DateTimeKind.Local);
			}
			if (newTimeInTicks < 0)
			{
				return new DateTime(0, DateTimeKind.Local);
			}

			return new DateTime(newTimeInTicks, DateTimeKind.Local);
		}

		public override DateTime ToUniversalTime(DateTime time)
		{
			long newTimeInTicks = time.Ticks - GetUtcOffset(time).Ticks;
			return new DateTime(newTimeInTicks, DateTimeKind.Utc);
		}


		public string FormatAdjustedUniversalTime(DateTime date)
		{
			TimeSpan utcOffset = GetUtcOffset(date);
			DateTime adjustedTime = date + utcOffset;
			string adjustedDateString;

			if (IsDaylightSavingTime(adjustedTime))
			{
				adjustedDateString = String.Format("{0} ({1}, UTC{2}{3:00}:{4:00})", adjustedTime.ToString("U"), DaylightName, Math.Sign(utcOffset.Ticks) < 0 ? "-" : "+", Math.Abs(utcOffset.Hours), Math.Abs(utcOffset.Minutes % 60));
			}
			else
			{
				adjustedDateString = String.Format("{0} ({1}, UTC{2}{3:00}:{4:00})", adjustedTime.ToString("U"), StandardName, Math.Sign(utcOffset.Ticks) < 0 ? "-" : "+", Math.Abs(utcOffset.Hours), Math.Abs(utcOffset.Minutes % 60));
			}
			return adjustedDateString;
		}


		private static WindowsTimeZoneCollection timeZones;

		static WindowsTimeZone()
		{
			timeZones = LoadTimeZonesFromXml();
		}

		public static WindowsTimeZoneCollection TimeZones
		{
			get
			{
				return timeZones;
			}
		}

		private static WindowsTimeZoneCollection LoadTimeZonesFromXml()
		{
			WindowsTimeZoneCollection tzs;
			Type tzcType = typeof(WindowsTimeZoneCollection);
			XmlSerializer ser = new XmlSerializer(tzcType);

			using (StreamReader rs = new StreamReader(tzcType.Assembly.GetManifestResourceStream("Subtext.Framework.Util.TimeZone.WindowsTimeZoneCollection.xml")))
			{
				tzs = ser.Deserialize(rs) as WindowsTimeZoneCollection;
			}

			return tzs;
		}

		//This code ported via Reflector, hence the weird variable names.
		internal long GetUtcOffsetFromUniversalTime(DateTime time, out bool isAmbiguousLocalDst)
		{
			TimeSpan utcOffset = this.baseBias;
			DaylightTime daylightSavingsChanges = this.GetDaylightChanges(time.Year);
			isAmbiguousLocalDst = false;
			if ((daylightSavingsChanges != null) && (daylightSavingsChanges.Delta.Ticks != 0))
			{
				DateTime time4;
				DateTime time5;
				DateTime daylightSavingsStart = daylightSavingsChanges.Start - utcOffset;
				DateTime daylightSavingsEnd = (daylightSavingsChanges.End - utcOffset) - daylightSavingsChanges.Delta;
				if (daylightSavingsChanges.Delta.Ticks > 0)
				{
					time4 = daylightSavingsEnd - daylightSavingsChanges.Delta;
					time5 = daylightSavingsEnd;
				}
				else
				{
					time4 = daylightSavingsStart;
					time5 = daylightSavingsStart - daylightSavingsChanges.Delta;
				}
				bool flag1;
				if (daylightSavingsStart > daylightSavingsEnd)
				{
					flag1 = (time < daylightSavingsEnd) || (time >= daylightSavingsStart);
				}
				else
				{
					flag1 = (time >= daylightSavingsStart) && (time < daylightSavingsEnd);
				}
				if (flag1)
				{
					utcOffset += daylightSavingsChanges.Delta;
					if ((time >= time4) && (time < time5))
					{
						isAmbiguousLocalDst = true;
					}
				}
			}
			return utcOffset.Ticks;
		}
	}
}
