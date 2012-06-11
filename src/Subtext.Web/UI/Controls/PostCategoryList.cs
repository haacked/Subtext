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
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for PostCategoryList.
    /// </summary>
    public class PostCategoryList : BaseControl
    {
        protected Repeater CatList;

        public ICollection<LinkCategory> LinkCategories { get; set; }

        public bool ShowEmpty { get; set; }

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

        protected override void OnPreRender(EventArgs e)
        {
            if (CatList.Items.Count == 0)
            {
                Visible = ShowEmpty;
            }
            base.OnPreRender(e);
        }

        protected void CategoryCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var linkcat = (LinkCategory)e.Item.DataItem;
                if (linkcat != null)
                {
                    var linkControl = (HyperLink)e.Item.FindControl("Link");
                    linkControl.NavigateUrl = Url.CategoryUrl(linkcat);
                    if (string.IsNullOrEmpty(linkControl.Attributes["title"]))
                    {
                        linkControl.Attributes["title"] = "";
                    }
                    linkControl.Text = linkcat.Title;
                }
            }
        }
    }
}