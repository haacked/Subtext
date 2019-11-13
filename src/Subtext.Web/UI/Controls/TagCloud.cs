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
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
    [PartialCaching(120, null, null, "Blogger", true)]
    public class TagCloud : BaseControl
    {
        public IEnumerable<Tag> TagItems { get; set; }

        [DefaultValue(0)]
        public int ItemCount { get; set; }

        protected virtual void Tags_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var tag = (Tag)e.Item.DataItem;
                var tagLink = e.Item.FindControl("TagUrl") as HyperLink;
                if (tagLink != null)
                {
                    tagLink.NavigateUrl = Url.TagUrl(tag.TagName);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            TagItems = Cacher.GetTopTags(ItemCount, SubtextContext);
            int tagCount = TagItems.Count();

            if (tagCount == 0)
            {
                Visible = false;
            }
            else
            {
                var tagRepeater = FindControl("Tags") as Repeater;
                if (tagRepeater != null)
                {
                    tagRepeater.DataSource = TagItems;
                    tagRepeater.DataBind();
                }

                var defaultTagLink = ControlHelper.FindControlRecursively(this, "DefaultTagLink") as HyperLink;
                if (defaultTagLink != null)
                {
                    defaultTagLink.NavigateUrl = Url.TagCloudUrl();
                }
            }
        }
    }
}