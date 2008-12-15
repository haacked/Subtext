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
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Web.Controls;
using Subtext.Framework;
using Subtext.Extensibility;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Displays the titles of the most recent entries on the skin.
    /// </summary>
	public class RecentPosts : BaseControl, IEntryControl
    {
        private const int DefaultRecentPostCount = 5;
        private ICollection<Entry> posts;
        protected Repeater postList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentPosts"/> class.
        /// </summary>
        public RecentPosts()
        {
            // number of posts to show, use default if not set by user
            // use recentcomments settings here - avoid schema additions & 
            // likely most people would be happy with the same settings for both controls anyway
            int postCount = Config.CurrentBlog.NumberOfRecentComments > 0 ? Config.CurrentBlog.NumberOfRecentComments : DefaultRecentPostCount;
            posts = Entries.GetRecentPosts(postCount, PostType.BlogPost, PostConfig.IsActive, true);
        }

        /// <summary>
        /// Binds the posts <see cref="List{T}"/> to the post list repeater.
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/>
        /// event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (posts != null)
            {
                postList.DataSource = posts;
                postList.DataBind();
            }
            else
            {
                this.Controls.Clear();
                this.Visible = false;
            }

        }

    	public Entry Entry
    	{
    		get { return this.currentEntry; }
    	}

    	private Entry currentEntry;

        protected void PostCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Entry post = (Entry)e.Item.DataItem;
            	this.currentEntry = post;
                HyperLink lnkPost = (HyperLink)e.Item.FindControl("Link");
                if (lnkPost != null)
                {
                    // display whole title, (up to 255 chars), no truncation
                    lnkPost.Text = HtmlHelper.RemoveHtml(post.Title);
                    lnkPost.NavigateUrl = post.Url;
                    ControlHelper.SetTitleIfNone(lnkPost, "Blog Entry.");
                }
            }
        }
    }
}
