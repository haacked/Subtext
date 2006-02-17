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
using Subtext.Extensibility;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for EntryStatsView.
	/// </summary>
	public class EntryStatsView : Entry
	{
		/// <summary>
		/// Creates a new <see cref="EntryStatsView"/> instance.
		/// </summary>
		public EntryStatsView() : base(PostType.None)
		{
		}

		private int _webCount;
		public int WebCount
		{
			get {return this._webCount;}
			set {this._webCount = value;}
		}

		private int _aggCount;
		public int AggCount
		{
			get {return this._aggCount;}
			set {this._aggCount = value;}
		}

		private DateTime _webLastUpdated;
		public DateTime WebLastUpdated
		{
			get {return this._webLastUpdated;}
			set {this._webLastUpdated = value;}
		}

		private DateTime _aggLastUpdated;
		public DateTime AggLastUpdated
		{
			get {return this._aggLastUpdated;}
			set {this._aggLastUpdated = value;}
		}
	}
}
