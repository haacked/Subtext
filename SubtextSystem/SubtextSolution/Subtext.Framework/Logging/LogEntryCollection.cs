using System;
using System.Collections;
using Subtext.Framework.Components;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Summary description for LogEntryCollection.
	/// </summary>
	public class LogEntryCollection : CollectionBase, IPagedResults
	{
		public void Add(LogEntry logEntry)
		{
			InnerList.Add(logEntry);
		}

		public LogEntry this[int index]
		{
			get
			{
				return (LogEntry)InnerList[index];
			}
		}

		public void CopyTo(LogEntry[] logEntries, int index)
		{
			InnerList.CopyTo(logEntries, index);
		}

		#region IPagedResults Members

		public int MaxItems
		{
			get
			{
				return InnerList.Count;
			}
			set
			{
			}
		}

		#endregion
	}
}
