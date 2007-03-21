using System;
using System.Collections.Generic;
using System.Text;

namespace Subtext.Framework.Util.TimeZoneUtil
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

		public int InitializeFromByteArray(byte[] buffer, int offset)
		{
			int pos = offset;
			year = BitConverter.ToUInt16(buffer, pos); pos += 2;
			month = BitConverter.ToUInt16(buffer, pos); pos += 2;
			dayOfWeek = BitConverter.ToUInt16(buffer, pos); pos += 2;
			day = BitConverter.ToUInt16(buffer, pos); pos += 2;
			hour = BitConverter.ToUInt16(buffer, pos); pos += 2;
			minute = BitConverter.ToUInt16(buffer, pos); pos += 2;
			second = BitConverter.ToUInt16(buffer, pos); pos += 2;
			milliseconds = BitConverter.ToUInt16(buffer, pos); pos += 2;
			return pos;
		}
	}
}
