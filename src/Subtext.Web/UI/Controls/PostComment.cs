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
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Email;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Routing;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Web.Properties;
using Subtext.Web.UI.ViewModels;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    ///		Summary description for Comments.
    /// </summary>
    public partial class PostComment : BaseControl, IEntryControl
    {
        private Entry _entry;
        private EntryViewModel _entryViewModel;

        bool IsCommentsRendered
        {
            get { return Blog.CommentsEnabled && Entry != null && Entry.AllowComments; }
        }

        bool IsCommentAllowed
        {
            get { return Blog.CommentsEnabled && Entry != null && Entry.AllowComments && !Entry.CommentingClosed; }
        }

        public EntryViewModel Entry
        {
            get
            {
                if (_entryViewModel == null)
                {
                    _entryViewModel = new EntryViewModel(RealEntry, SubtextContext);
                }
                return _entryViewModel;
            }
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

        /// <summary>
        /// Handles the OnLoad event.  Attempts to prepopulate comment 
        /// fields based on the user's cookie.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //TODO: Make this configurable.
            tbTitle.MaxLength = 128;
            tbEmail.MaxLength = 128;
            tbName.MaxLength = 32;
            tbUrl.MaxLength = 256;
            tbComment.MaxLength = 4000;
            SetValidationGroup();

            if (!IsPostBack)
            {
                if (Entry == null)
                {
                    //Somebody probably is messing with the url.
                    Response.Redirect("~/SystemMessages/FileNotFound.aspx", true);
                    return;
                }

                ResetCommentFields();

                if (Config.CurrentBlog.CoCommentsEnabled)
                {
                    if (coComment == null)
                    {
                        coComment = new SubtextCoComment();
                        var coCommentPlaceHolder = Page.FindControl("coCommentPlaceholder") as PlaceHolder;
                        if (coCommentPlaceHolder != null)
                        {
                            coCommentPlaceHolder.Controls.Add(coComment);
                        }
                    }
                    coComment.PostTitle = RealEntry.Title;
                    coComment.PostUrl = Url.EntryUrl(RealEntry).ToFullyQualifiedUrl(Blog).ToString();
                }
            }

            DataBind();
        }

        void SetValidationGroup()
        {
            foreach (Control control in Controls)
            {
                var validator = control as BaseValidator;
                if (validator != null)
                {
                    validator.ValidationGroup = "SubtextComment";
                    continue;
                }

                var btn = control as Button;
                if (btn != null)
                {
                    btn.ValidationGroup = "SubtextComment";
                    continue;
                }

                var textbox = control as TextBox;
                if (textbox != null)
                {
                    textbox.ValidationGroup = "SubtextComment";
                    continue;
                }
            }
        }

        /// <summary>
        /// Called when an approved comment is added.
        /// </summary>
        protected virtual void OnCommentApproved(FeedbackItem feedback)
        {
            if (feedback.Approved)
            {
                EventHandler<EventArgs> theEvent = CommentApproved;
                if (theEvent != null)
                {
                    theEvent(this, EventArgs.Empty);
                }
            }
        }

        private void RemoveCommentControls()
        {
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                Controls.RemoveAt(i);
            }
        }

        public event EventHandler<EventArgs> CommentApproved;

        private void OnSubmitButtonClick(object sender, EventArgs e)
        {
            Thread.Sleep(5000);
            if (!Page.IsValid)
            {
                return;
            }

            LastDitchValidation();
            try
            {
                Entry currentEntry = RealEntry;
                if (IsCommentAllowed)
                {
                    FeedbackItem feedbackItem = CreateFeedbackInstanceFromFormInput(currentEntry);
                    ICommentSpamService feedbackService = null;
                    if (Blog.FeedbackSpamServiceEnabled)
                    {
                        feedbackService = new AkismetSpamService(Blog.FeedbackSpamServiceKey, Blog, null, Url);
                    }
                    var commentService = new CommentService(SubtextContext,
                                                            new CommentFilter(SubtextContext, feedbackService));
                    commentService.Create(feedbackItem, true/*runFilters*/);
                    var emailService = new EmailService(EmailProvider.Instance(), new EmbeddedTemplateEngine(),
                                                        SubtextContext);
                    emailService.EmailCommentToBlogAuthor(feedbackItem);

                    if (chkRemember == null || chkRemember.Checked)
                    {
                        SetRememberedUserCookie();
                    }

                    DisplayResultMessage(feedbackItem);
                }
            }
            catch (BaseCommentException exception)
            {
                Message.Text = exception.Message;
            }
        }

        private void LastDitchValidation()
        {
            //The validation controls and otherwise should catch everything.
            //This is here to be extra safe.
            //Anything triggering these exceptions is probably malicious.
            if (tbComment.Text.Length > 4000
               || tbTitle.Text.Length > 128
               || tbEmail.Text.Length > 128
               || tbName.Text.Length > 32
               || tbUrl.Text.Length > 256)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_CommentNotValid);
            }
        }

        private void SetRememberedUserCookie()
        {
            var user = new HttpCookie("CommentUser");
            user.Values["Name"] = tbName.Text;
            user.Values["Url"] = tbUrl.Text;
            if (tbEmail != null)
            {
                user.Values["Email"] = tbEmail.Text;
            }
            user.Expires = DateTime.UtcNow.AddDays(30);
            Response.Cookies.Add(user);
        }

        private void DisplayResultMessage(FeedbackItem feedbackItem)
        {
            RemoveCommentControls();
            Message = new Label();

            if (feedbackItem.Approved)
            {
                Message.Text = Resources.PostComment_ThanksForComment;
                Message.CssClass = "success";
                Controls.Add(Message); //This needs to be here for ajax calls.
                Cacher.ClearCommentCache(feedbackItem.EntryId, SubtextContext);
                OnCommentApproved(feedbackItem);
                return;
            }
            if (feedbackItem.NeedsModeratorApproval)
            {
                Message.Text = Resources.PostComment_ThanksForComment + " It will be displayed soon.";
                Message.CssClass = "error moderation";
            }
            else
            {
                Message.Text = "Sorry, but your comment was flagged as spam and will be moderated.";
                Message.CssClass = "error";
            }
            Controls.Add(Message);
        }

        private FeedbackItem CreateFeedbackInstanceFromFormInput(IIdentifiable currentEntry)
        {
            var feedbackItem = new FeedbackItem(FeedbackType.Comment) { Author = tbName.Text };
            if (tbEmail != null)
            {
                feedbackItem.Email = tbEmail.Text;
            }
            feedbackItem.SourceUrl = tbUrl.Text.EnsureUrl();
            feedbackItem.Body = tbComment.Text;
            feedbackItem.Title = tbTitle.Text;
            feedbackItem.EntryId = currentEntry.Id;
            feedbackItem.IpAddress = HttpHelper.GetUserIpAddress(SubtextContext.HttpContext);
            feedbackItem.IsBlogAuthor = User.IsAdministrator();
            return feedbackItem;
        }

        private void ResetCommentFields()
        {
            if (tbComment != null)
            {
                tbComment.Text = string.Empty;
            }

            if (tbEmail != null)
            {
                tbEmail.Text = User.IsAdministrator() ? Blog.Email : string.Empty;
            }

            if (tbName != null)
            {
                tbName.Text = User.IsAdministrator() ? Blog.UserName : string.Empty;
            }

            if (tbTitle != null)
            {
                tbTitle.Text = string.Format("re: {0}", HttpUtility.HtmlDecode(RealEntry.Title));
            }

            if (tbUrl != null)
            {
                tbUrl.Text = User.IsAdministrator() ? Url.BlogUrl().ToFullyQualifiedUrl(Blog).ToString() : string.Empty;
            }

            HttpCookie user = Request.Cookies["CommentUser"];
            if (user != null)
            {
                tbName.Text = user.Values["Name"];
                tbUrl.Text = user.Values["Url"];

                // Remember by default if no-checkbox.
                if (chkRemember != null && chkRemember.Checked)
                {
                    chkRemember.Checked = true;
                }

                //Check to see if email textbox is present
                if (tbEmail != null && user.Values["Email"] != null)
                {
                    tbEmail.Text = user.Values["Email"];
                }
            }

            if (IsCommentsRendered)
            {
                if (RealEntry.CommentingClosed)
                {
                    Controls.Clear();
                    Controls.Add(
                        new LiteralControl(
                            "<div class=\"commentsClosedMessage\"><span style=\"font-style: italic;\">Comments have been closed on this topic.</span></div>"));
                }
                else
                {
                    tbTitle.Text = string.Format("re: {0}", HttpUtility.HtmlDecode(RealEntry.Title));
                }
            }
            else
            {
                Controls.Clear();
            }
        }

        override protected void OnInit(EventArgs e)
        {
            if (btnSubmit != null)
            {
                btnSubmit.Click += OnSubmitButtonClick;
            }

            if (btnCompliantSubmit != null)
            {
                btnCompliantSubmit.Click += OnSubmitButtonClick;
            }

            //Captcha should not be given to admin.
            if (!User.IsAdministrator())
            {
                int btnIndex = Controls.IndexOf(btnSubmit);
                if (btnIndex < 0)
                {
                    btnIndex = Controls.IndexOf(btnCompliantSubmit);
                }

                AddCaptchaIfNecessary(ref captcha, ref invisibleCaptchaValidator, btnIndex);
            }
            else
            {
                RemoveCaptcha();
            }
        }
    }
}