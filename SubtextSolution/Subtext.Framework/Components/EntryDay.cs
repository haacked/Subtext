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
using System.Collections.ObjectModel;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a collection of <see cref="Entry">Entry</see> Components.
	/// </summary>
	[Serializable]
	public class EntryDay : Collection<Entry>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntryDay">EntryDay</see> class.
		/// </summary>
		public EntryDay() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntryDay">EntryDay</see> class.
		/// </summary>
		public EntryDay(DateTime dt)
		{
			BlogDay = dt;
		}

		public DateTime BlogDay
		{
			get;
			set;
		}

		public string Link
		{
			get;
			set;
		}
	}
}

