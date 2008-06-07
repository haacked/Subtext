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

using System.Collections.Generic;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using System;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Web.Admin.Pages
{
    public partial class Customize : AdminOptionsPage
    {
        private int pageIndex = 0;

        protected override void OnLoad(System.EventArgs e)
        {
            if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
                this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

            base.OnLoad(e);
        }

        protected bool ContainsTags
        {
            get
            {
                return this.containsTags;
            }
        }
        bool containsTags;

        protected override void BindLocalUI()
        {
            BlogInfo blog = Config.CurrentBlog;
            IPagedCollection<MetaTag> tags = MetaTags.GetMetaTagsForBlog(blog, this.pageIndex, this.resultsPager.PageSize);

            this.containsTags = tags.Count > 0;

            // we want to databind either way so we can alter the DOM via JavaScript and AJAX requests.
            MetatagRepeater.DataSource = tags;
            MetatagRepeater.DataBind();

            resultsPager.ItemCount = tags.MaxItems;
            resultsPager.PageSize = Preferences.ListingItemCount;
            resultsPager.PageIndex = this.pageIndex;

            base.BindLocalUI();
        }

        protected static MetaTag EvalTag(object dataItem)
        {
            return (MetaTag) dataItem;
        }

        protected static string EvalName(object dataItem)
        {
            MetaTag tag = dataItem as MetaTag;

            return tag == null ? string.Empty : tag.Name;
        }

        protected static string EvalContent(object dataItem)
        {
            MetaTag tag = dataItem as MetaTag;

            return tag == null ? string.Empty : tag.Content;
        }

        protected static string EvalHttpEquiv(object dataItem)
        {
            MetaTag tag = dataItem as MetaTag;

            return tag == null ? string.Empty : tag.HttpEquiv;
        }
    }
}
