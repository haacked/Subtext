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
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Docuverse.Identicon;
using log4net;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Format;
using Subtext.Framework.Logging;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Web.Controls;
using Image = System.Web.UI.WebControls.Image;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///	Codebehind for the control that displays comments/trackbacks/pingbacks.
    /// </summary>
    public class Comments : BaseControl, ICommentControl
    {
        static readonly ILog log = new Log();

        protected Repeater CommentList;
        protected Literal NoCommentMessage;
        private FeedbackItem comment;
        private GravatarService gravatarService = null; 

        public FeedbackItem Comment
        {
            get 
            { 
                return comment; 
            }
        }

        public bool IsEditEnabled
        {
            get { return Request.IsAuthenticated && SecurityHelper.IsAdmin; }
        }

        public string EditUrl(FeedbackItem feedback)
        {
            string url = AdminUrl.FeedbackEdit(feedback.Id);

            return VirtualPathUtility.ToAbsolute(StringHelper.LeftBefore(url, "?")) + "?" + StringHelper.RightAfter(url, "?");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.gravatarService = new GravatarService(ConfigurationManager.AppSettings);

            if (Blog.CommentsEnabled)
            {
                BindFeedback(true);
            }
            else
            {
                Visible = false;
            }
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

        internal void BindFeedback(bool fromCache)
        {
            Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short, true, SubtextContext);

            if (entry != null && entry.AllowComments)
            {
                BindFeedback(entry, fromCache);
            }
            else
            {
                Visible = false;
            }
        }

        protected void RemoveComment_ItemCommand(Object Sender, RepeaterCommandEventArgs e)
        {
            int feedbackId = Int32.Parse(e.CommandName);
            FeedbackItem feedback = FeedbackItem.Get(feedbackId);
            if (feedback != null)
            {
                FeedbackItem.Delete(feedback, null);
                Cacher.ClearCommentCache(feedback.EntryId, SubtextContext);
                BindFeedback(false);
            }
            //Response.Redirect(string.Format(CultureInfo.InvariantCulture, "{0}?Pending=true", Request.Path));
        }

        // Customizes the display row for each comment.
        protected void CommentsCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FeedbackItem feedbackItem = (FeedbackItem)e.Item.DataItem;
                this.comment = feedbackItem;
                if (feedbackItem != null)
                {
                    Literal title = (Literal)(e.Item.FindControl("Title"));
                    if (title != null)
                    {
                        // we should probably change skin format to dynamically wire up to 
                        // skin located title and permalinks at some point
                        title.Text = string.Format(CultureInfo.InvariantCulture, "{2}&nbsp;{0}{1}", Anchor(feedbackItem.Id),
                            feedbackItem.Title, Link(feedbackItem.Title, Url.FeedbackUrl(feedbackItem)));
                    }

                    //Shows the name of the commenter with a link if provided.
                    HyperLink namelink = (HyperLink)e.Item.FindControl("NameLink");
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
                            namelink.Text = !String.IsNullOrEmpty(feedbackItem.Author) ? feedbackItem.Author : "Pingback/TrackBack";
                            ControlHelper.SetTitleIfNone(namelink, "PingBack/TrackBack");
                        }

                        if (feedbackItem.IsBlogAuthor)
                            HtmlHelper.AppendCssClass(namelink, "author");
                    }

                    Literal PostDate = (Literal)(e.Item.FindControl("PostDate"));
                    if (PostDate != null)
                    {
                        PostDate.Text = feedbackItem.DateCreated.ToShortDateString() + " " + feedbackItem.DateCreated.ToShortTimeString();
                    }

                    Literal Post = e.Item.FindControl("PostText") as Literal;
                    if (Post != null)
                    {
                        if (feedbackItem.Body.Length > 0)
                        {
                            Post.Text = feedbackItem.Body;
                            if (feedbackItem.Body.Length == 0 && feedbackItem.FeedbackType == FeedbackType.PingTrack)
                            {
                                Post.Text = "Pingback / Trackback";
                            }
                        }
                    }

                    if (gravatarService.Enabled)
                    {
                        Image gravatarImage = e.Item.FindControl("GravatarImg") as Image;
                        if (gravatarImage != null)
                        {
                            //This allows per-skin configuration of the default gravatar image.
                            string defaultGravatarImage = gravatarImage.Attributes["PlaceHolderImage"];

                            string ip;
                            if (feedbackItem.IpAddress != null)
                                ip = feedbackItem.IpAddress.ToString();
                            else
                                ip = DateTime.Now.Millisecond + " " + DateTime.Now.Second;

                            //This allows a host-wide setting of the default gravatar image.
                            string gravatarUrl = null;
                            if (!String.IsNullOrEmpty(feedbackItem.Email))
                            {
                                if(!String.IsNullOrEmpty(defaultGravatarImage)) 
                                {
                                    string host = Request.Url.Host;
                                    string scheme = Request.Url.Scheme;
                                    string port = Request.Url.Port == 80 ? string.Empty : ":" + Request.Url.Port;
                                    string defaultImagePath = HttpHelper.ExpandTildePath(defaultGravatarImage);
                                    defaultGravatarImage = string.Format("{0}://{1}{2}{3}", scheme, host, port, defaultImagePath);
                                    defaultGravatarImage = HttpUtility.UrlEncode(defaultGravatarImage);
                                }
                                gravatarUrl = gravatarService.GenerateUrl(feedbackItem.Email, defaultGravatarImage);
                            }
                            if (!String.IsNullOrEmpty(gravatarUrl))
                            {
                                gravatarImage.Attributes.Remove("PlaceHolderImage");
                                if (gravatarUrl.Length != 0)
                                {
                                    gravatarImage.ImageUrl = gravatarUrl;
                                    gravatarImage.Visible = true;
                                }
                            }
                            else
                            {
                                string identiconUrl;
                                string identiconSizeSetting = ConfigurationManager.AppSettings["IdenticonSize"];

                                int identiconSize = 40;
                                if (!String.IsNullOrEmpty(identiconSizeSetting))
                                {
                                    int.TryParse(identiconSizeSetting, out identiconSize);
                                }
                                identiconUrl = string.Format("~/images/IdenticonHandler.ashx?size={0}&code={1}"
                                    , identiconSize
                                    , IdenticonUtil.Code(ip));

                                gravatarImage.ImageUrl = identiconUrl;
                                gravatarImage.Visible = true;
                            }
                        }
                    }

                    if (Request.IsAuthenticated && SecurityHelper.IsAdmin)
                    {
                        HyperLink editCommentTextLink = (HyperLink)(e.Item.FindControl("EditCommentTextLink"));
                        if (editCommentTextLink != null)
                        {
                            editCommentTextLink.NavigateUrl = AdminUrl.FeedbackEdit(feedbackItem.Id);
                            if (String.IsNullOrEmpty(editCommentTextLink.Text))
                            {
                                editCommentTextLink.Text = "Edit Comment " + feedbackItem.Id.ToString(CultureInfo.InstalledUICulture);
                            }
                            ControlHelper.SetTitleIfNone(editCommentTextLink, "Click to edit this entry.");
                        }
                        HyperLink editCommentImgLink = (HyperLink)(e.Item.FindControl("EditCommentImgLink"));
                        if (editCommentImgLink != null)
                        {
                            editCommentImgLink.NavigateUrl = AdminUrl.FeedbackEdit(feedbackItem.Id);
                            if (String.IsNullOrEmpty(editCommentImgLink.ImageUrl))
                            {
                                editCommentImgLink.ImageUrl = Url.EditIconUrl();
                            }
                            ControlHelper.SetTitleIfNone(editCommentImgLink, "Click to edit comment " + feedbackItem.Id.ToString(CultureInfo.InstalledUICulture));
                        }
                        LinkButton editlink = (LinkButton)(e.Item.FindControl("EditLink"));
                        if (editlink != null)
                        {
                            //editlink.CommandName = "Remove";
                            editlink.Text = "Remove Comment " + feedbackItem.Id.ToString(CultureInfo.InvariantCulture);
                            editlink.CommandName = feedbackItem.Id.ToString(CultureInfo.InvariantCulture);
                            editlink.Attributes.Add("onclick", "return confirm(\"Are you sure you want to delete comment " + feedbackItem.Id.ToString(CultureInfo.InvariantCulture) + "?\");");
                            editlink.Visible = true;
                            editlink.CommandArgument = feedbackItem.Id.ToString(CultureInfo.InvariantCulture);

                            ControlHelper.SetTitleIfNone(editlink, "Click to remove this entry.");
                        }
                    }
                }
            }
        }

        const string linktag = "<a title=\"permalink: {0}\" href=\"{1}\">#</a>";
        private static string Link(string title, string link)
        {
            if (link == null)
                return string.Empty;

            return string.Format(linktag, title, link);
        }

        // GC: xhmtl format wreaking havoc in non-xhtml pages in non-IE, changed to non nullable format
        const string anchortag = "<a name=\"{0}\"></a>";
        private static string Anchor(int id)
        {
            return string.Format(anchortag, id);
        }

        internal void BindFeedback(Entry entry, bool fromCache)
        {
            try
            {
                CommentList.DataSource = Cacher.GetFeedback(entry, CacheDuration.Short, fromCache, SubtextContext);
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
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                Visible = false;
            }
        }
    }
}


