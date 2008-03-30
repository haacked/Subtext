#region Timezone code from DasBlog
/*This code was contributed by the DasBlog team */
#endregion

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Subtext.Framework.Util
{
    
	/// <summary>
	/// This structure is the equivalent of the Windows SYSTEMTIME structure.
	/// </summary>
	public struct WindowsSystemTime
	{
		public ushort year;
		public ushort month;
		public ushort dayOfWeek;
		public ushort day;
		public ushort hour;
		public ushort minute;
		public ushort second;
		public ushort milliseconds;

		public int InitializeFromByteArray( byte[] buffer, int offset )
		{
			int pos = offset;
			year = BitConverter.ToUInt16( buffer, pos ); pos += 2;
			month = BitConverter.ToUInt16( buffer, pos ); pos += 2;
			dayOfWeek = BitConverter.ToUInt16( buffer, pos ); pos += 2;
			day = BitConverter.ToUInt16( buffer, pos ); pos += 2;
			hour = BitConverter.ToUInt16( buffer, pos ); pos += 2;
			minute = BitConverter.ToUInt16( buffer, pos ); pos += 2;
			second = BitConverter.ToUInt16( buffer, pos ); pos += 2;
			milliseconds = BitConverter.ToUInt16( buffer, pos ); pos += 2;
			return pos;
		}
	}

	/// <summary>
	/// This structure is the binary equivalent of the TZI information found
	/// in the Windows registry for each key underneath 
	/// "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones"
	/// </summary>
	public struct WindowsTZI
	{
		/// <summary>
		/// Base Bias from GMT, in seconds. Subtracting this value from the local time results in GMT.
		/// For Western Europe, this value is -60, for US Eastern Time this value is +300.
		/// </summary>
		public int bias;
		/// <summary>
		/// Standard time bias as an offset to the base bias. This is usually 0.
		/// </summary>
		public int standardBias;
		/// <summary>
		/// Daylight savings time bias as an offset to the base bias. Subtracting this value from
		/// the local time results in the standard time. For Western Europe, this value is -60,
		/// for US Eastern time ít's also -60 and for India Standard Time it's 0.
		/// </summary>
		public int daylightBias;
		/// <summary>
		/// This is WindowsSystemTime structure (equivalent to the unmanaged SYSTEMTIME) structure,
		/// but stores NOT a valid date. Instead, the following rules apply:
		/// If "month" is 0, there is no daylight savings time for this zone. The dayOfWeek value
		/// indicates the day of the week on which daylight savings time is effective. The day value
		/// indicates, starting at 1 (with a maximum value of 5, saying "last") the occurrence of 
		/// that weekday within the month on which the daylight savings time is effective.
		/// </summary>
		public WindowsSystemTime standardDate;
		/// <summary>
		/// This is WindowsSystemTime structure (equivalent to the unmanaged SYSTEMTIME) structure,
		/// but stores NOT a valid date. Instead, the following rules apply:
		/// If "month" is 0, there is no daylight savings time for this zone. The dayOfWeek value
		/// indicates the day of the week on which daylight savings time reverts back to standard time. 
		/// The day value indicates, starting at 1 (with a maximum value of 5, saying "last") the 
		/// occurrence of that weekday within the month on which the daylight savings time reverts back.
		/// </summary>
		public WindowsSystemTime daylightDate;

		/// <summary>
		/// Initializes the structure from a byte array.
		/// </summary>
		/// <param name="buffer">Byte buffer</param>
		/// <param name="offset">Offset intro the buffer at which to start</param>
		/// <returns>Position at which the parsing left off</returns>
		public int InitializeFromByteArray( byte[] buffer, int offset )
		{
			int pos = offset;
			standardDate.month = 0;
			daylightDate.month = 0;
			bias = BitConverter.ToInt32(buffer,pos);pos+=4;
			standardBias = BitConverter.ToInt32(buffer,pos);pos+=4;
			daylightBias = BitConverter.ToInt32(buffer,pos);pos+=4;
			pos = standardDate.InitializeFromByteArray( buffer, pos );
			pos = daylightDate.InitializeFromByteArray( buffer, pos );
			return pos;
		}
	}

    

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
		
		internal WindowsTimeZone(string displayName, string daylightZoneName, string standardZoneName, int zoneIndex, byte[] tzi )
		{
			this.winTZI.bias = 0;
			this.displayName = displayName.Trim();
			this.daylightZoneName = daylightZoneName.Trim();
			this.standardZoneName = standardZoneName.Trim();
			this.zoneIndex = zoneIndex;

			this.winTZI.InitializeFromByteArray( tzi, 0 );
			this.baseBias = TimeSpan.FromMinutes( winTZI.bias*-1 );
			this.standardBias = TimeSpan.FromMinutes( winTZI.standardBias*-1 );
			this.daylightBias = TimeSpan.FromMinutes( winTZI.daylightBias*-1 );
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
		public string DisplayName { get { return displayName; } set { this.displayName = value;}}
		public string DaylightZoneName { get { return daylightZoneName; } set { this.daylightZoneName = value;}}
		public string StandardZoneName { get { return standardZoneName; } set { this.standardZoneName = value;}}
		public int ZoneIndex { get { return zoneIndex; } set { this.zoneIndex = value;}}
		public WindowsTZI WinTZI { get { return winTZI; } 
			set
			{
				this.winTZI = value;
				this.baseBias = TimeSpan.FromMinutes( winTZI.bias*-1 );
				this.standardBias = TimeSpan.FromMinutes( winTZI.standardBias*-1 );
				this.daylightBias = TimeSpan.FromMinutes( winTZI.daylightBias*-1 );
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
				lock(daylightChangesLock)
				{
					DateTime current;
					DateTime start = DateTime.MinValue;
					DateTime end = DateTime.MinValue;
					TimeSpan delta = TimeSpan.FromTicks(0);
            
					if ( winTZI.daylightDate.month != 0 && winTZI.standardDate.month != 0 )
					{
						// day count is a value from 1 to 5, indicating the day 
						// in the month on which the switch occurs
						int dayCount = winTZI.daylightDate.day;
						current = start = new DateTime( year, winTZI.daylightDate.month, 1, winTZI.daylightDate.hour,winTZI.daylightDate.minute,winTZI.daylightDate.second );
						while ( current.Month == winTZI.daylightDate.month )
						{
							if ( Convert.ToUInt16(current.DayOfWeek) == winTZI.daylightDate.dayOfWeek )
							{
								start = current;
								--dayCount;
								if ( dayCount == 0 )
								{
									break;
								}
							}
							current = current.AddDays(1);                    
						}

						// day count is a value from 1 to 5, indicating the day 
						// in the month on which the switch occurs
						dayCount = winTZI.standardDate.day;
						current = end = new DateTime( year, winTZI.standardDate.month, 1, winTZI.standardDate.hour,winTZI.standardDate.minute,winTZI.standardDate.second );
						while ( current.Month == winTZI.standardDate.month )
						{
							if ( Convert.ToUInt16(current.DayOfWeek) == winTZI.standardDate.dayOfWeek )
							{
								end = current;
								--dayCount;
								if ( dayCount == 0 )
								{
									break;
								}
							}
							current = current.AddDays(1);                    
						}
						delta = daylightBias - standardBias;
					}
					retVal = new DaylightTime(start,end,delta);
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
			if ( IsDaylightSavingTime( time ) )
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
			if(time.Kind == DateTimeKind.Local)
				return time;

			bool isAmbiguousLocalDst;
			long offset = this.GetUtcOffsetFromUniversalTime(time, out isAmbiguousLocalDst);
			long newTimeInTicks = time.Ticks + offset;

			if (newTimeInTicks > 0x2bca2875f4373fff)
			{
				return new DateTime(0x2bca2875f4373fff, DateTimeKind.Local);
			}
			if(newTimeInTicks < 0)
			{
				return new DateTime(0, DateTimeKind.Local);
			}

			return new DateTime(newTimeInTicks, DateTimeKind.Local);
		}

		public override DateTime ToUniversalTime(DateTime time)
		{
			long newTimeInTicks = time.Ticks - GetUtcOffset( time ).Ticks;
			return new DateTime(newTimeInTicks, DateTimeKind.Utc);
		}


		public string FormatAdjustedUniversalTime( DateTime date )
		{
			TimeSpan utcOffset = GetUtcOffset( date );
			DateTime adjustedTime = date + utcOffset;
			string adjustedDateString;
                
			if ( IsDaylightSavingTime( adjustedTime ) )
			{
				adjustedDateString = String.Format( "{0} ({1}, UTC{2}{3:00}:{4:00})",adjustedTime.ToString("U"), DaylightName, Math.Sign(utcOffset.Ticks)<0?"-":"+", Math.Abs(utcOffset.Hours), Math.Abs(utcOffset.Minutes%60) );
			}
			else
			{
				adjustedDateString = String.Format( "{0} ({1}, UTC{2}{3:00}:{4:00})",adjustedTime.ToString("U"), StandardName, Math.Sign(utcOffset.Ticks)<0?"-":"+", Math.Abs(utcOffset.Hours), Math.Abs(utcOffset.Minutes%60) );
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

			using (StreamReader rs = new StreamReader(tzcType.Assembly.GetManifestResourceStream(tzcType.Namespace + ".WindowsTimeZoneCollection.xml")))
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


	class WindowsTimeZoneBiasSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((WindowsTimeZone)x).BaseBias.CompareTo(((WindowsTimeZone)y).BaseBias);
		}
	}

	/// <summary>
	/// A collection of elements of type WindowsTimeZone
	/// </summary>
	[Serializable]
	public class WindowsTimeZoneCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the WindowsTimeZoneCollection class.
		/// </summary>
		public WindowsTimeZoneCollection()
		{
			// empty
		}

		/// <summary>
		/// Adds an instance of type WindowsTimeZone to the end of this WindowsTimeZoneCollection.
		/// </summary>
		/// <param name="value">
		/// The WindowsTimeZone to be added to the end of this WindowsTimeZoneCollection.
		/// </param>
		public virtual void Add(WindowsTimeZone value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic WindowsTimeZone value is in this WindowsTimeZoneCollection.
		/// </summary>
		/// <param name="value">
		/// The WindowsTimeZone value to locate in this WindowsTimeZoneCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this WindowsTimeZoneCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(WindowsTimeZone value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this WindowsTimeZoneCollection
		/// </summary>
		/// <param name="value">
		/// The WindowsTimeZone value to locate in the WindowsTimeZoneCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(WindowsTimeZone value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Gets or sets the WindowsTimeZone at the given index in this WindowsTimeZoneCollection.
		/// </summary>
		public virtual WindowsTimeZone this[int index]
		{
			get
			{
				return (WindowsTimeZone) this.List[index];
			}
		}

		/// <summary>
		/// Gets the WindowsTimeZone by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public virtual WindowsTimeZone GetById(int id)
		{
			foreach( WindowsTimeZone wtz in this )
			{
				if ( wtz.Id == id )
				{
					return wtz;
				}
			}
			return null;
		}

		/// <summary>
		/// Type-specific enumeration class, used by WindowsTimeZoneCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: IEnumerator
		{
			private IEnumerator wrapped;

			public Enumerator(WindowsTimeZoneCollection collection)
			{
				this.wrapped = ((CollectionBase)collection).GetEnumerator();
			}

			public WindowsTimeZone Current
			{
				get
				{
					return (WindowsTimeZone) (this.wrapped.Current);
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return (this.wrapped.Current);
				}
			}

			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this WindowsTimeZoneCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		public void SortByTimeZoneBias()
		{
			this.InnerList.Sort( new WindowsTimeZoneBiasSorter());
		}
	}


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
			return new DaylightTime(DateTime.MinValue,DateTime.MinValue, TimeSpan.FromTicks(0));
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
