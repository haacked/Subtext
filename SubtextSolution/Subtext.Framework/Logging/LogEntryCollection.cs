#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;
using Subtext.Framework.Components;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Summary description for LogEntryCollection.
	/// </summary>
	public class LogEntryCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogEntryCollection"/> class.
		/// </summary>
		public LogEntryCollection() : base()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogEntryCollection"/> class.
		/// </summary>
		/// <param name="entries">The entries.</param>
		public LogEntryCollection(LogEntry[] entries)
		{
			this.AddRange(entries);
		}

		/// <summary>
		/// Adds the specified log entry.
		/// </summary>
		/// <param name="logEntry">The log entry.</param>
		public void Add(LogEntry logEntry)
		{
			InnerList.Add(logEntry);
		}

		/// <summary>
		/// Copies the elements of the specified <see cref="Entry">Entry</see> array to the end of the collection.
		/// </summary>
		/// <param name="entries">An array of type <see cref="Entry">Entry</see> containing the Components to add to the collection.</param>
		public void AddRange(LogEntry[] entries) 
		{
			for (int i = 0;	(i < entries.Length); i = (i + 1)) 
			{
				this.Add(entries[i]);
			}
		}

		/// <summary>
		/// Adds the contents of another <see cref="EntryCollection">EntryCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="entryCollection">A <see cref="EntryCollection">EntryCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(LogEntryCollection entryCollection) 
		{
			for (int i = 0;	(i < entryCollection.Count); i = (i +	1))	
			{
				this.Add((LogEntry)entryCollection.List[i]);
			}
		}

		/// <summary>
		/// Gets the <see cref="LogEntry"/> at the specified index.
		/// </summary>
		/// <value></value>
		public LogEntry this[int index]
		{
			get
			{
				return (LogEntry)InnerList[index];
			}
		}

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="logEntries">The log entries.</param>
		/// <param name="index">The index.</param>
		public void CopyTo(LogEntry[] logEntries, int index)
		{
			InnerList.CopyTo(logEntries, index);
		}

		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="EntryCollection">EntryCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="EntryCollection">EntryCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(LogEntry value) 
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Returns the index of the specified log Entry.
		/// </summary>
		/// <param name="logEntry">The log entry.</param>
		/// <returns></returns>
		public int IndexOf(LogEntry logEntry) 
		{
			return this.List.IndexOf(logEntry);
		}
		
		/// <summary>
		/// Inserts the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="logEntry">The log entry.</param>
		public void Insert(int index, LogEntry logEntry)	
		{
			List.Insert(index, logEntry);
		}
		
		/// <summary>
		/// Removes the specified log entry.
		/// </summary>
		/// <param name="logEntry">The log entry.</param>
		public void Remove(LogEntry logEntry) 
		{
			List.Remove(logEntry);
		}
	}
}
