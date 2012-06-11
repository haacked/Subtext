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
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Web.Controls;
using Subtext.Web.UI.ViewModels;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Displays the titles of the most recent entries on the skin.
    /// </summary>
    public class RecentPosts : BaseControl, IEntryControl
    {
        private const int DefaultRecentPostCount = 5;
        private ICollection<Entry> _posts;
        protected Repeater postList;

        public EntryViewModel Entry { get; private set; }

        /// <summary>
        /// Binds the posts <see cref="List{T}"/> to the post list repeater.
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/>
        /// event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            // number of posts to show, use default if not set by user
            // use recentcomments settings here - avoid schema additions & 
            // likely most people would be happy with the same settings for both controls anyway
            int postCount = Config.CurrentBlog.NumberOfRecentComments > 0
                                ? Config.CurrentBlog.NumberOfRecentComments
                                : DefaultRecentPostCount;
            _posts = Repository.GetEntries(postCount, PostType.BlogPost, PostConfig.IsActive, true);

            base.OnLoad(e);

            if (_posts != null)
            {
                postList.DataSource = _posts;
                postList.DataBind();
            }
            else
            {
                Controls.Clear();
                Visible = false;
            }
        }

        protected void PostCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var post = (Entry)e.Item.DataItem;
                Entry = new EntryViewModel(post, SubtextContext);
                var lnkPost = (HyperLink)e.Item.FindControl("Link");
                if (lnkPost != null)
                {
                    // display whole title, (up to 255 chars), no truncation
                    lnkPost.Text = HtmlHelper.RemoveHtml(post.Title);
                    lnkPost.NavigateUrl = Url.EntryUrl(post);
                    ControlHelper.SetTitleIfNone(lnkPost, "Blog Entry.");
                }
            }
        }
    }
}