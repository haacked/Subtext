#region Disclaimer/Info

// Calendar created 11/07/2005 by:
//*Simone Chiaretta (http://blogs.ugidotnet.org/piyo/)
//
//With inspiration and guidance from:
// DottextBlogCalendar created on 2/14/2004 by:
//*Scott Willeke (http://blogs.pingpoet.com/overflow)
//*Scott Mitchell (http://scottonwriting.net/sowblog/posts/708.aspx)
//*Scott Watermasysk (http://scottwater.com/blog/archive/2004/02/13/CalendarControl.aspx)

#endregion

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Properties;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for StaticPage.
    /// </summary>
    public class StaticPage : BaseControl
    {
        protected Repeater CatList;

        private void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var lcc = new List<LinkCategory>();
            lcc.AddRange(Repository.GetCategories(CategoryType.LinkCollection, ActiveFilter.ActiveOnly));
            CatList.DataSource = lcc;
            CatList.DataBind();
        }

        protected void CategoryCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var linkcat = (LinkCategory)e.Item.DataItem;
                if (linkcat != null)
                {
                    var title = (Literal)e.Item.FindControl("Title");
                    if (title != null)
                    {
                        title.Text = linkcat.Title;
                    }

                    var linkList = (Repeater)e.Item.FindControl("LinkList");
                    linkList.DataSource = linkcat.Links;
                    linkList.DataBind();
                }
            }
        }

        protected void LinkCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var link = (Link)e.Item.DataItem;
                if (link != null)
                {
                    var linkControl = (HyperLink)e.Item.FindControl("Link");
                    linkControl.NavigateUrl = link.Url;
                    linkControl.Text = link.Title;
                    if (link.NewWindow)
                    {
                        linkControl.Target = "_blank";
                    }

                    if (link.HasRss)
                    {
                        var rssLink = (HyperLink)e.Item.FindControl("RssLink");
                        if (rssLink != null)
                        {
                            rssLink.NavigateUrl = link.Rss;
                            rssLink.Visible = true;
                            rssLink.ToolTip = string.Format(Resources.LinkPage_Subscribe, link.Title);
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