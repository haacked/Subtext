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
using Subtext.Framework.Components;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Base class for SubText specific events
	/// </summary>
	public class EntryEventArgs : SubtextEventArgs
	{

		private Entry _entry;
		/// <summary>
		/// The Entry being manipulated
		/// </summary>
		public Entry Entry
		{
			get { return _entry; }
		}


		public EntryEventArgs(Entry entry, ObjectState state): base(state)
		{
			_entry = entry;
		}

		public EntryEventArgs(Entry entry) : this(entry, ObjectState.None) { }

	}
}
