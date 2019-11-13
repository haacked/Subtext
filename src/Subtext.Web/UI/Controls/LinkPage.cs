#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Web.Controls;
using Subtext.Web.Properties;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for LinkPage.
    /// </summary>
    public class LinkPage : BaseControl
    {
        protected DataList CatList;

        private void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var lcc = new List<LinkCategory>();
            lcc.AddRange(Repository.GetActiveCategories());
            CatList.DataSource = lcc;
            CatList.DataBind();
        }

        protected void CategoryCreated(object sender, DataListItemEventArgs e)
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
                    ControlHelper.SetTitleIfNone(linkControl, link.Title);
                    if (link.NewWindow)
                    {
                        if (!String.IsNullOrEmpty(linkControl.Attributes["rel"]))
                        {
                            linkControl.Attributes["rel"] += " ";
                        }
                        linkControl.Attributes["rel"] += "external ";
                    }
                    linkControl.Attributes["rel"] += link.Relation;
                    if (link.HasRss)
                    {
                        var rssLink = (HyperLink)e.Item.FindControl("RssLink");
                        if (rssLink != null)
                        {
                            rssLink.NavigateUrl = link.Rss;
                            rssLink.Visible = true;
                            rssLink.ToolTip = string.Format(CultureInfo.InvariantCulture, Resources.LinkPage_Subscribe,
                                                            link.Title);
                            ControlHelper.SetTitleIfNone(rssLink, rssLink.ToolTip);
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