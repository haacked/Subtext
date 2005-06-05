using System;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;

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
	///		Summary description for CategoryDisplayByColumn.
	/// </summary>
	public  class CategoryList : BaseControl
	{
		protected System.Web.UI.WebControls.Repeater CatList;

		private LinkCategoryCollection lcc;
		public LinkCategoryCollection LinkCategories
		{
			get{return lcc;}
			set{lcc = value;}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(LinkCategories != null)
			{
				CatList.DataSource = LinkCategories;
				CatList.DataBind();
			}
			else
			{
				this.Controls.Clear();
				this.Visible = false;
			}

		}

		protected void CategoryCreated(object sender,  RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LinkCategory linkcat = (LinkCategory)e.Item.DataItem;
				if(linkcat != null)
				{
					Literal title = (Literal)e.Item.FindControl("Title");
					if(title != null)
					{
						title.Text = linkcat.Title;
					}

					Repeater LinkList = (Repeater)e.Item.FindControl("LinkList");
					LinkList.DataSource = linkcat.Links;
					LinkList.DataBind();
				}
			}
		}

		protected void LinkCreated(object sender,  RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Link link = (Link)e.Item.DataItem;
				if(link != null)
				{
					HyperLink Link = (HyperLink)e.Item.FindControl("Link");
					Link.NavigateUrl = link.Url;
					Link.Text = link.Title;
					if(link.NewWindow)
					{
						Link.Target = "_blank";
					}

					if(link.HasRss)
					{
						HyperLink RssLink = (HyperLink)e.Item.FindControl("RssLink");
						if(RssLink != null)
						{
							RssLink.NavigateUrl = link.Rss;
							RssLink.Visible = true;
							RssLink.ToolTip = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Subscribe to {0}",link.Title);
						}
					}
				}
			}
		}
	}
}

