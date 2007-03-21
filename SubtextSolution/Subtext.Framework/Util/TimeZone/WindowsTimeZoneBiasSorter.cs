using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Subtext.Framework.Util.TimeZoneUtil
{
	class WindowsTimeZoneBiasSorter : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((WindowsTimeZone)x).BaseBias.CompareTo(((WindowsTimeZone)y).BaseBias);
		}
	}
}
