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
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for ArchivePostPage.
    /// </summary>
    public class ArchivePostPage : BaseControl
    {
        protected Repeater CatList;
        protected Repeater DateItemList;

        private void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var lcc = new List<LinkCategory>();
            lcc.AddRange(Repository.GetCategories(CategoryType.PostCollection, ActiveFilter.None));
            lcc.Add(Repository.Links(CategoryType.PostCollection, SubtextContext.Blog, Url));
            CatList.DataSource = lcc;
            CatList.DataBind();

            LinkCategory monthCat = Repository.ArchiveMonth(Url, Blog);
            DateItemList.DataSource = monthCat.Links;
            DateItemList.DataBind();
        }

        protected void CategoryCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var linkcat = (LinkCategory)e.Item.DataItem;
                if (linkcat != null)
                {
                    if (linkcat.Id != 0)
                    {
                        var description = (Label)e.Item.FindControl("Description");
                        if (description != null)
                        {
                            description.Text = linkcat.Description;
                        }

                        var catlink = (HyperLink)e.Item.FindControl("CatLink");
                        if (catlink != null)
                        {
                            catlink.NavigateUrl = Url.CategoryUrl(linkcat);
                            catlink.Text = linkcat.Title;
                            ControlHelper.SetTitleIfNone(catlink, linkcat.CategoryType + " Category.");
                        }
                    }
                }
            }
        }

        protected void DateItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var link = (Link)e.Item.DataItem;
                if (link != null)
                {
                    var datelink = (HyperLink)e.Item.FindControl("DateLink");
                    if (datelink != null)
                    {
                        datelink.NavigateUrl = link.Url;
                        datelink.Text = link.Title;
                        ControlHelper.SetTitleIfNone(datelink, "Posts for the month.");
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