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
using System.Web.UI.WebControls;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for LastSevenDaysControl.
	/// </summary>
	public class Day : BaseControl
	{
		public Day()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected System.Web.UI.WebControls.Repeater DayList;
		protected System.Web.UI.WebControls.HyperLink ImageLink;
		protected System.Web.UI.WebControls.Literal  DateTitle;

		private EntryDay bpd;
		public EntryDay CurrentDay
		{
			set{bpd = value;}
		}

		const string postdescWithComments = "posted @ <a href=\"{0}\" title = \"permalink\">{1}</a> | <a href=\"{2}#FeedBack\" title = \"comments, pingbacks, trackbacks\">Feedback ({3})</a>";
		const string postdescWithNoComments = "posted @ <a href=\"{0}\" title = \"permalink\">{1}</a>";

		protected void PostCreated(object sender,  RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Entry entry = (Entry)e.Item.DataItem;
				if(entry != null)
				{
					HyperLink hl = (HyperLink)e.Item.FindControl("TitleUrl");
					if(hl != null)
					{
						hl.NavigateUrl = entry.TitleUrl;
						hl.Text = entry.Title;
					}

					Literal PostText = (Literal)e.Item.FindControl("PostText");
					if(PostText != null)
					{
						PostText.Text = entry.Body;
					}

					Literal desc = (Literal)e.Item.FindControl("PostDescription");
					if(desc != null)
					{
						string link = entry.Link;
					}

					Literal PostDesc = (Literal)e.Item.FindControl("PostDesc");
					if(PostDesc != null)
					{
						if(CurrentBlog.EnableComments && entry.AllowComments)
						{
							PostDesc.Text = string.Format(postdescWithComments,entry.Link,entry.DateCreated.ToShortTimeString(),entry.Link,entry.FeedBackCount);
						}
						else
						{
							PostDesc.Text = string.Format(postdescWithNoComments,entry.Link,entry.DateCreated.ToShortTimeString());
						}
					}
					
				}
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
				if(bpd != null)
				{
					//ImageLink.NavigateUrl = Subtext.Framework.Util.Globals.ArchiveUrl(bpd.BlogDay,"MMddyyyy");// bpd.Link;
					ImageLink.NavigateUrl = Subtext.Framework.Configuration.Config.CurrentBlog(Context).UrlFormats.DayUrl(bpd.BlogDay);
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

