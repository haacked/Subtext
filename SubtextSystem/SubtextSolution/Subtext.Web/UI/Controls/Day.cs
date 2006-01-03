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
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Displays all entries for a given day.
	/// </summary>
	public class Day : EntryList
	{
		protected System.Web.UI.WebControls.Repeater DayList;
		protected System.Web.UI.WebControls.HyperLink ImageLink;
		protected System.Web.UI.WebControls.Literal  DateTitle;

		private EntryDay bpd;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Day"/> class and sets 
		/// the DescriptionOnly property to false.
		/// </summary>
		public Day() : base()
		{
			this.DescriptionOnly = false;	
		}

		/// <summary>
		/// Sets the current day.
		/// </summary>
		/// <value>The current day.</value>
		public EntryDay CurrentDay
		{
			get{return bpd;}
			set{bpd = value;}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(bpd != null)
			{
				ImageLink.NavigateUrl = Subtext.Framework.Configuration.Config.CurrentBlog.UrlFormats.DayUrl(bpd.BlogDay);
				DateTitle.Text = bpd.BlogDay.ToLongDateString();
				DayList.DataSource = bpd;
				DayList.DataBind();
			}
			else
			{
				this.Visible = false;
			}
		}
	}
}

