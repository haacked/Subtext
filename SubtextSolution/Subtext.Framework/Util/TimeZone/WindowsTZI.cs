using System;
using System.Collections.Generic;
using System.Text;

namespace Subtext.Framework.Util.TimeZoneUtil
{
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
		public int InitializeFromByteArray(byte[] buffer, int offset)
		{
			int pos = offset;
			standardDate.month = 0;
			daylightDate.month = 0;
			bias = BitConverter.ToInt32(buffer, pos); pos += 4;
			standardBias = BitConverter.ToInt32(buffer, pos); pos += 4;
			daylightBias = BitConverter.ToInt32(buffer, pos); pos += 4;
			pos = standardDate.InitializeFromByteArray(buffer, pos);
			pos = daylightDate.InitializeFromByteArray(buffer, pos);
			return pos;
		}
	}
}
