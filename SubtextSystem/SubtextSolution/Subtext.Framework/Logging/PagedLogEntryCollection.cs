using System;
using Subtext.Framework.Components;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Pageable collection of <see cref="LogEntry"/> instances.
	/// </summary>
	public class PagedLogEntryCollection : LogEntryCollection, IPagedResults
	{
		private int _maxItems;
		
		/// <summary>
		/// Gets or sets the max items this can contain.
		/// </summary>
		/// <value></value>
		public int MaxItems
		{
			get {return this._maxItems;}
			set {this._maxItems = value;}
		}
	}
}
