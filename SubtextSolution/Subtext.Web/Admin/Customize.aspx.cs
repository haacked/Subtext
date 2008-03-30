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

namespace Subtext.Web.Admin.Pages
{
    public partial class Customize : AdminOptionsPage
    {
        protected override void BindLocalUI()
        {
            BlogInfo blog = Config.CurrentBlog;
            IList<MetaTag> tags = MetaTags.GetMetaTagsForBlog(blog);

            if (tags.Count == 0)
            {
                MetatagListWrapper.Attributes.Add("style", "display:none;");
            }
            else
            {
                NoMetatagsMessage.Attributes.Add("style", "display:none;");
            }

            // we want to databind either way so we can alter the DOM via JavaScript and AJAX requests.
            MetatagRepeater.DataSource = tags;
            MetatagRepeater.DataBind();

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
