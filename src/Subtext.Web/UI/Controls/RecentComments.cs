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
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Displays the most recent comments on the skin.
    /// </summary>
    [PartialCaching(120, null, null, "Blogger", true)]
    public class RecentComments : BaseControl
    {
        private const int DefaultRecentPostCount = 5;
        protected Repeater feedList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentComments"/> class.
        /// </summary>
        public RecentComments()
        {
            int commentCount = Config.CurrentBlog.NumberOfRecentComments > 0
                                   ? Config.CurrentBlog.NumberOfRecentComments
                                   : DefaultRecentPostCount;
            ICollection<FeedbackItem> comments = Repository.GetRecentComments(commentCount);
            Comments = (from c in comments where c.EntryId > 0 select c).ToList();
            Gravatar = new GravatarService(ConfigurationManager.AppSettings);
        }

        protected GravatarService Gravatar { get; private set; }

        protected IEnumerable<FeedbackItem> Comments { get; private set; }

        protected FeedbackItem Comment { get; set; }

        protected string SafeCommentBody
        {
            get
            {
                if (Comment != null)
                {
                    string commentBody = HttpUtility.HtmlEncode(HtmlHelper.RemoveHtml(Comment.Body));
                    if (Blog.RecentCommentsLength > 0)
                    {
                        if (commentBody.Length > Blog.RecentCommentsLength)
                        {
                            commentBody = commentBody.Substring(0, Blog.RecentCommentsLength) + "...";
                        }
                    }
                    return commentBody;
                }
                return string.Empty;
            }
        }

        protected string AlternatingCssClass { get; set; }

        /// <summary>
        /// Binds the comments <see cref="List{T}"/> to the comment list repeater.
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/>
        /// event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Comments != null && feedList != null)
            {
                feedList.DataSource = Comments;
                feedList.DataBind();
            }
            else
            {
                Controls.Clear();
                Visible = false;
            }
        }

        public string EditUrl(FeedbackItem feedback)
        {
            string url = AdminUrl.FeedbackEdit(feedback.Id);

            return VirtualPathUtility.ToAbsolute(StringHelper.LeftBefore(url, "?")) + "?" +
                   url.RightAfter("?");
        }

        protected void EntryCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Comment = (FeedbackItem)e.Item.DataItem;

                var title = (HyperLink)e.Item.FindControl("Link");
                if (title != null)
                {
                    title.Text = SafeCommentBody;
                    title.NavigateUrl = Url.FeedbackUrl(Comment);
                    ControlHelper.SetTitleIfNone(title, "Reader Comment.");
                }
                var author = (Literal)e.Item.FindControl("Author");
                if (author != null)
                {
                    author.Text = HttpUtility.HtmlEncode(Comment.Author);
                }
            }
        }

        protected void OnItemBound(object sender, RepeaterItemEventArgs e)
        {
            AlternatingCssClass = e.Item.ItemType != ListItemType.Item ? " alt" : string.Empty;
        }
    }
}