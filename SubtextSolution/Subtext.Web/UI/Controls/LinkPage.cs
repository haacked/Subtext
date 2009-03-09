using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Components;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///		Summary description for LinkPage.
	/// </summary>
	public class LinkPage : Subtext.Web.UI.Controls.BaseControl
	{
		protected System.Web.UI.WebControls.DataList CatList;

		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
            List<LinkCategory> lcc = new List<LinkCategory>();
			lcc.AddRange(Repository.GetActiveCategories());
			CatList.DataSource = lcc;
			CatList.DataBind();
		}

		protected void CategoryCreated(object sender,  DataListItemEventArgs e)
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
					ControlHelper.SetTitleIfNone(Link, link.Title);
					if(link.NewWindow)
					{
						if(!String.IsNullOrEmpty(Link.Attributes["rel"]))
						{
							Link.Attributes["rel"] += " ";
						}
                        Link.Attributes["rel"] += "external ";
					}
                    Link.Attributes["rel"] += link.Relation;
					if(link.HasRss)
					{
						HyperLink RssLink = (HyperLink)e.Item.FindControl("RssLink");
						if(RssLink != null)
						{
							RssLink.NavigateUrl = link.Rss;
							RssLink.Visible = true;
							RssLink.ToolTip = string.Format("Subscribe to {0}", link.Title);
							ControlHelper.SetTitleIfNone(RssLink, RssLink.ToolTip);
						}
					}
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
