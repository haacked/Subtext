#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a collection of <see cref="Entry">Entry</see> Components.
	/// </summary>
	[Serializable]
	public class EntryDay: EntryCollection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntryDay">EntryDay</see> class.
		/// </summary>
		public EntryDay() 
		{
		}

		public EntryDay(DateTime dt)
		{
			BlogDay = dt;
		}

		private DateTime _blogday;
		public DateTime BlogDay
		{
			get{return _blogday;}
			set{_blogday = value;}
		}


		private string _link;
		public string Link
		{
			get{return _link;}
			set{_link = value;}
		}
	}
}

