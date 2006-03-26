using System;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;

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

		protected void CategoryCreated(object sender, RepeaterItemEventArgs e)
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

		protected void LinkCreated(object sender, RepeaterItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				Link link = (Link)e.Item.DataItem;
				if(link != null)
				{
					HyperLink Link = (HyperLink)e.Item.FindControl("Link");
					Link.NavigateUrl = link.Url;
					if(Link.Attributes["title"] == null || Link.Attributes["title"].Length == 0)
					{
						Link.Attributes["title"] = "Category Link";
					}
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
							if(RssLink.Attributes["title"] == null || RssLink.Attributes["title"].Length == 0)
							{
								RssLink.Attributes["title"] = "Click to Subscribe";
							}
							RssLink.ToolTip = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Subscribe to {0}",link.Title);
						}
					}
				}
			}
		}
	}
}

