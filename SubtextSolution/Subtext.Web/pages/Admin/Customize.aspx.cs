#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.Pages
{
    public partial class Customize : AdminOptionsPage
    {
        bool containsTags;
        private int pageIndex = 0;

        protected bool ContainsTags
        {
            get { return containsTags; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if(null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
            {
                pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
            }

            base.OnLoad(e);
        }

        protected override void BindLocalUI()
        {
            Blog blog = Config.CurrentBlog;
            IPagedCollection<MetaTag> tags = MetaTags.GetMetaTagsForBlog(blog, pageIndex, resultsPager.PageSize);

            containsTags = tags.Count > 0;

            // we want to databind either way so we can alter the DOM via JavaScript and AJAX requests.
            MetatagRepeater.DataSource = tags;
            MetatagRepeater.DataBind();

            resultsPager.ItemCount = tags.MaxItems;
            resultsPager.PageSize = Preferences.ListingItemCount;
            resultsPager.PageIndex = pageIndex;

            base.BindLocalUI();
        }

        protected static MetaTag EvalTag(object dataItem)
        {
            return (MetaTag)dataItem;
        }

        protected static string EvalName(object dataItem)
        {
            var tag = dataItem as MetaTag;

            return tag == null ? string.Empty : tag.Name;
        }

        protected static string EvalContent(object dataItem)
        {
            var tag = dataItem as MetaTag;

            return tag == null ? string.Empty : tag.Content;
        }

        protected static string EvalHttpEquiv(object dataItem)
        {
            var tag = dataItem as MetaTag;

            return tag == null ? string.Empty : tag.HttpEquiv;
        }
    }
}