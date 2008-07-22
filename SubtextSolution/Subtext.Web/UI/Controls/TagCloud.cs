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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Subtext.Framework.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
    public class TagCloud : BaseControl
    {
        private IEnumerable<Tag> tags;
        public IEnumerable<Tag> TagItems
        {
            get { return tags; }
            set { tags = value; }
        }

        private int itemCount = 0;
        
		[DefaultValue(0)]
        public int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
        }

        protected virtual void Tags_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Tag tag = (Tag)e.Item.DataItem;
                HyperLink tagLink = e.Item.FindControl("TagUrl") as HyperLink;
				if(tagLink != null)
					tagLink.NavigateUrl = string.Format("{0}Tags/{1}/default.aspx", Config.CurrentBlog.RootUrl, tag.TagName);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            tags = Cacher.GetTopTags(ItemCount, CacheDuration.Short);
            int tagCount = 0;
            foreach (Tag t in tags)
                tagCount++;

            if (tagCount == 0)
            {
                this.Visible = false;
            }
            else
            {

                Repeater tagRepeater = this.FindControl("Tags") as Repeater;
                if (tagRepeater != null)
                {
                    tagRepeater.DataSource = TagItems;
                    tagRepeater.DataBind();

                }

                HyperLink hlDefault = ControlHelper.FindControlRecursively(this, "DefaultTagLink") as HyperLink;
                if (hlDefault != null)
                {
                    hlDefault.NavigateUrl = string.Format("{0}Tags/default.aspx", Config.CurrentBlog.RootUrl);
                }
            }
        }
    }
}
