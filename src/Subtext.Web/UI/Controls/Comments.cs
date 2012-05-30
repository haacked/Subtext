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
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Logging;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Web.Controls;
using Subtext.Web.UI.ViewModels;
using Image = System.Web.UI.WebControls.Image;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Codebehind for the control that displays comments/trackbacks/pingbacks.
    /// </summary>
    public class Comments : BaseControl, ICommentControl
    {
        const string Anchortag = "<a name=\"{0}\"></a>";
        const string Linktag = "<a title=\"permalink: {0}\" href=\"{1}\">#</a>";
        static readonly ILog Log = new Log();

        private CommentViewModel _comment;
        protected Repeater CommentList;
        private GravatarService _gravatarService;
        protected Literal NoCommentMessage;

        public bool IsEditEnabled
        {
            get { return Request.IsAuthenticated && User.IsAdministrator(); }
        }

        /// <summary>
        /// If the currecnt comment was written by the author, 
        /// writes the specified css class
        /// </summary>
        /// <returns></returns>
        protected string AuthorCssClass
        {
            get { return Comment.IsBlogAuthor ? " author" : ""; }
        }

        public CommentViewModel Comment
        {
            get { return _comment; }
        }

        private Entry RealEntry
        {
            get
            {
                if (_entry == null)
                {
                    _entry = Cacher.GetEntryFromRequest(true, SubtextContext);
                    if (_entry == null)
                    {
                        HttpHelper.SetFileNotFoundResponse();
                    }
                }
                return _entry;
            }
        }

        Entry _entry;

        public string EditUrl(CommentViewModel feedback)
        {
            string url = AdminUrl.FeedbackEdit(feedback.Id);

            return VirtualPathUtility.ToAbsolute(StringHelper.LeftBefore(url, "?")) + "?" +
                   url.RightAfter("?");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _gravatarService = new GravatarService(ConfigurationManager.AppSettings);

            if (Blog.CommentsEnabled)
            {
                BindFeedback(true);
            }
            else
            {
                Visible = false;
            }
        }

        internal void BindFeedback(bool fromCache)
        {
            Entry entry = RealEntry;

            if (entry != null && entry.AllowComments)
            {
                BindFeedback(entry, fromCache);
            }
            else
            {
                Visible = false;
            }
        }

        // Customizes the display row for each comment.
        protected void CommentsCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var feedbackItem = (FeedbackItem)e.Item.DataItem;
                _comment = new CommentViewModel(feedbackItem, SubtextContext);
                if (feedbackItem != null)
                {
                    var title = (Literal)(e.Item.FindControl("Title"));
                    if (title != null)
                    {
                        // we should probably change skin format to dynamically wire up to 
                        // skin located title and permalinks at some point
                        title.Text = string.Format(CultureInfo.InvariantCulture, "{2}&nbsp;{0}{1}",
                                                   Anchor(feedbackItem.Id),
                                                   feedbackItem.Title,
                                                   Link(feedbackItem.Title, Url.FeedbackUrl(feedbackItem)));
                    }

                    //Shows the name of the commenter with a link if provided.
                    var namelink = (HyperLink)e.Item.FindControl("NameLink");
                    if (namelink != null)
                    {
                        if (feedbackItem.SourceUrl != null)
                        {
                            namelink.NavigateUrl = feedbackItem.SourceUrl.ToString();
                            ControlHelper.SetTitleIfNone(namelink, feedbackItem.SourceUrl.ToString());
                        }

                        if (feedbackItem.FeedbackType == FeedbackType.Comment)
                        {
                            namelink.Text = feedbackItem.Author;
                            ControlHelper.SetTitleIfNone(namelink, feedbackItem.Author);
                        }
                        else if (feedbackItem.FeedbackType == FeedbackType.PingTrack)
                        {
                            namelink.Text = !String.IsNullOrEmpty(feedbackItem.Author)
                                                ? feedbackItem.Author
                                                : "Pingback/TrackBack";
                            ControlHelper.SetTitleIfNone(namelink, "PingBack/TrackBack");
                        }

                        if (feedbackItem.IsBlogAuthor)
                        {
                            HtmlHelper.AppendCssClass(namelink, "author");
                        }
                    }

                    var postDate = (Literal)(e.Item.FindControl("PostDate"));
                    if (postDate != null)
                    {
                        var dateCreated = feedbackItem.DateCreated;

                        postDate.Text = dateCreated.ToShortDateString() + " " +
                                        dateCreated.ToShortTimeString();
                    }

                    var post = e.Item.FindControl("PostText") as Literal;
                    if (post != null)
                    {
                        if (!String.IsNullOrEmpty(feedbackItem.Body))
                        {
                            post.Text = feedbackItem.Body;
                            if (feedbackItem.Body.Length == 0 && feedbackItem.FeedbackType == FeedbackType.PingTrack)
                            {
                                post.Text = "Pingback / Trackback";
                            }
                        }
                    }

                    if (_gravatarService.Enabled)
                    {
                        var gravatarImage = e.Item.FindControl("GravatarImg") as Image;
                        if (gravatarImage != null)
                        {
                            string ip;
                            if (feedbackItem.IpAddress != null)
                            {
                                ip = feedbackItem.IpAddress.ToString();
                            }
                            else
                            {
                                ip = string.Format("{0} {1}", DateTime.UtcNow.Millisecond, DateTime.UtcNow.Second);
                            }

                            string gravatarUrl = gravatarUrl = _gravatarService.GenerateUrl(feedbackItem.Email);
                            gravatarImage.Attributes.Remove("PlaceHolderImage");
                            gravatarImage.ImageUrl = gravatarUrl;
                            gravatarImage.Visible = true;
                        }
                    }

                    if (Request.IsAuthenticated && User.IsAdministrator())
                    {
                        var editCommentTextLink = (HyperLink)(e.Item.FindControl("EditCommentTextLink"));
                        if (editCommentTextLink != null)
                        {
                            editCommentTextLink.NavigateUrl = AdminUrl.FeedbackEdit(feedbackItem.Id);
                            if (String.IsNullOrEmpty(editCommentTextLink.Text))
                            {
                                editCommentTextLink.Text = "Edit Comment " +
                                                           feedbackItem.Id.ToString(CultureInfo.InstalledUICulture);
                            }
                            ControlHelper.SetTitleIfNone(editCommentTextLink, "Click to edit this entry.");
                        }
                        var editCommentImgLink = (HyperLink)(e.Item.FindControl("EditCommentImgLink"));
                        if (editCommentImgLink != null)
                        {
                            editCommentImgLink.NavigateUrl = AdminUrl.FeedbackEdit(feedbackItem.Id);
                            if (String.IsNullOrEmpty(editCommentImgLink.ImageUrl))
                            {
                                editCommentImgLink.ImageUrl = Url.EditIconUrl();
                            }
                            ControlHelper.SetTitleIfNone(editCommentImgLink,
                                                         "Click to edit comment " +
                                                         feedbackItem.Id.ToString(CultureInfo.InstalledUICulture));
                        }
                    }
                }
            }
        }

        private static string Link(string title, string link)
        {
            if (link == null)
            {
                return string.Empty;
            }

            return string.Format(Linktag, title, link);
        }

        // GC: xhmtl format wreaking havoc in non-xhtml pages in non-IE, changed to non nullable format

        private static string Anchor(int id)
        {
            return string.Format(Anchortag, id);
        }

        internal void BindFeedback(Entry entry, bool fromCache)
        {
            try
            {
                CommentList.DataSource = fromCache ? Cacher.GetFeedback(entry, SubtextContext) : Repository.GetFeedbackForEntry(entry);
                CommentList.DataBind();

                if (CommentList.Items.Count == 0)
                {
                    if (entry.CommentingClosed)
                    {
                        Controls.Clear();
                    }
                    else
                    {
                        CommentList.Visible = false;
                        NoCommentMessage.Text = "No comments posted yet.";
                    }
                }
                else
                {
                    CommentList.Visible = true;
                    NoCommentMessage.Text = string.Empty;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
                Visible = false;
            }
        }

        public void InvalidateFeedbackCache()
        {
            Cacher.InvalidateFeedback(RealEntry, SubtextContext);
        }

        [Obsolete("This will get removed in the next version")]
        protected void RemoveComment_ItemCommand(object sender, EventArgs e)
        {
        }
    }
}