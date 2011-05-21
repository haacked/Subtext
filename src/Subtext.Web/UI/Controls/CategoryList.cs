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

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Code behind for the category list control.
    /// </summary>
    public class CategoryList : BaseControl
    {
        protected Repeater CatList;

        public ICollection<LinkCategory> LinkCategories { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (LinkCategories != null)
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

                    /*if (FriendlyUrlSettings.Settings.Enabled)
						Link.NavigateUrl = string.Format("/category/{0}.aspx", FriendlyUrlSettings.TransformString(link.Title.Replace(" ", FriendlyUrlSettings.Settings.SeparatingCharacter), FriendlyUrlSettings.Settings.TextTransformation));*/

                    if (string.IsNullOrEmpty(linkControl.Attributes["title"]))
                    {
                        linkControl.Attributes["title"] = "";
                    }
                    linkControl.Text = link.Title;

                    if (link.NewWindow)
                    {
                        if (!String.IsNullOrEmpty(linkControl.Attributes["rel"]))
                        {
                            linkControl.Attributes["rel"] += " ";
                        }
                        linkControl.Attributes["rel"] += "external ";
                    }

                    linkControl.Attributes["rel"] += link.Relation;

                    var rssLink = (HyperLink)e.Item.FindControl("RssLink");
                    if (rssLink != null)
                    {
                        if (link.HasRss)
                        {
                            rssLink.NavigateUrl = link.Rss;
                            rssLink.Visible = true;
                            if (string.IsNullOrEmpty(rssLink.ToolTip))
                            {
                                rssLink.ToolTip = string.Format(CultureInfo.InvariantCulture, "Subscribe to {0}",
                                                                link.Title);
                            }
                        }
                        else
                        {
                            rssLink.Visible = false;
                        }
                    }
                }
            }
        }
    }
}