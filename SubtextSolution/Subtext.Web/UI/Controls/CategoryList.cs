using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// <summary>
	///	Code behind for the category list control.
	/// </summary>
	public  class CategoryList : BaseControl
	{
		protected Repeater CatList;

        private IList<LinkCategory> lcc;
        public IList<LinkCategory> LinkCategories
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
				Controls.Clear();
				Visible = false;
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
                    
					/*if (FriendlyUrlSettings.Settings.Enabled)
						Link.NavigateUrl = string.Format("/category/{0}.aspx", FriendlyUrlSettings.TransformString(link.Title.Replace(" ", FriendlyUrlSettings.Settings.SeparatingCharacter), FriendlyUrlSettings.Settings.TextTransformation));*/

					if(Link.Attributes["title"] == null || Link.Attributes["title"].Length == 0)
					{
						Link.Attributes["title"] = "";
					}
					Link.Text = link.Title;

					if (link.NewWindow)
					{
						if (!String.IsNullOrEmpty(Link.Attributes["rel"]))
						{
							Link.Attributes["rel"] += " ";
						}
						Link.Attributes["rel"] += "external ";
					}
                    
                    Link.Attributes["rel"] += link.Relation;

					HyperLink RssLink = (HyperLink)e.Item.FindControl("RssLink");
					if (RssLink != null)
					{
						if(link.HasRss)
						{
							RssLink.NavigateUrl = link.Rss;
							RssLink.Visible = true;
							if(RssLink.ToolTip == null || RssLink.ToolTip.Length == 0)
							{
								RssLink.ToolTip = string.Format(CultureInfo.InvariantCulture, "Subscribe to {0}", link.Title);
							}
						}
						else
						{
							RssLink.Visible = false;
						}
					}
				}
			}
		}
	}
}


