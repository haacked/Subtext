using System;
using Subtext.Common.Data;
using Subtext.Framework.Util;

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

namespace Subtext.Web.UI.Controls
{
	using System;

	/// <summary>
	///		Summary description for ArchiveMonth.
	/// </summary>
	public  class ArchiveMonth : Subtext.Web.UI.Controls.BaseControl
	{
		
		protected Subtext.Web.UI.Controls.EntryList Days;

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			DateTime dt = WebPathStripper.GetDateFromRequest(Request.Path,"archive");
			Days.EntryListItems = Cacher.GetMonth(dt,CacheTime.Short,Context);
			Days.EntryListTitle = string.Format("{0} Entries", dt.ToString("MMMM yyyy"));
			Subtext.Web.UI.Globals.SetTitle(string.Format("{0} - {1} Entries",CurrentBlog.Title,dt.ToString("MMMM yyyy")),Context);
		}
	}
}

