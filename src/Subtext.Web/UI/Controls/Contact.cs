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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Email;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Security;
using Subtext.Framework.Services;
using Subtext.Framework.Web;
using Subtext.Web.Controls.Captcha;

namespace Subtext.Web.UI.Controls
{
    public class Contact : BaseControl
    {
        protected Button btnSend;
        protected CaptchaControl captcha;
        protected InvisibleCaptcha invisibleCaptchaValidator;
        protected Label lblMessage;
        protected RegularExpressionValidator RegularExpressionValidator1;
        protected RequiredFieldValidator RequiredFieldValidator1;
        protected RequiredFieldValidator RequiredFieldValidator2;
        protected TextBox tbEmail;
        protected TextBox tbMessage;
        protected TextBox tbName;
        protected TextBox tbSubject;
        protected ValidationSummary ValidationSummary1;

        public static bool SendContactMessageToFeedback
        {
            get
            {
                string contactSetting = ConfigurationManager.AppSettings["ContactToFeedback"];
                if (contactSetting != null)
                {
                    bool useFeedback;
                    bool.TryParse(contactSetting, out useFeedback);
                    return useFeedback;
                }
                return false;
            }
        }

        // If you've ever turned on ContactToFeedback in the past, but later turn it off, 
        // you may still want to show the existing messages.
        public static bool ShowContactMessages
        {
            get
            {
                if (SendContactMessageToFeedback)
                {
                    return true;
                }
                string contactSetting = ConfigurationManager.AppSettings["ShowContactToFeedback"];
                if (contactSetting != null)
                {
                    bool showFeedback;
                    bool.TryParse(contactSetting, out showFeedback);
                    return showFeedback;
                }
                return false;
            }
        }

        /// <summary>
        /// Initializes the control.  Sets up the send button's 
        /// click event handler.
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            btnSend.Click += OnSendButtonClick;

            EnsureEmailRequired();
            //Captcha should not be given to admin.
            if (User.IsAdministrator())
            {
                int btnIndex = Controls.IndexOf(btnSend);
                AddCaptchaIfNecessary(ref captcha, ref invisibleCaptchaValidator, btnIndex);
            }
            else
            {
                RemoveCaptcha();
            }
            base.OnInit(e);
        }

        private void EnsureEmailRequired()
        {
            foreach (Control control in Controls)
            {
                var validator = control as RequiredFieldValidator;
                if (validator == null)
                {
                    continue;
                }

                if (validator.ControlToValidate == tbEmail.ID)
                {
                    return;
                }
            }
            var emailRequiredValidator = new RequiredFieldValidator
            {
                ControlToValidate = tbEmail.ID,
                ErrorMessage = "* Please enter your email address",
                Display = ValidatorDisplay.Dynamic
            };
            Controls.AddAt(Controls.IndexOf(tbEmail) + 1, emailRequiredValidator);
        }


        private void OnSendButtonClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var contactMessage = new FeedbackItem(FeedbackType.ContactPage)
                {
                    Author = tbName.Text,
                    Email = tbEmail.Text,
                    Body = tbMessage.Text,
                    Title = string.Format("CONTACT: {0}", tbSubject.Text),
                    IpAddress = HttpHelper.GetUserIpAddress(SubtextContext.HttpContext)
                };


                if (SendContactMessageToFeedback || String.IsNullOrEmpty(Blog.Email))
                {
                    CreateCommentWithContactMessage(contactMessage);
                }
                else
                {
                    try
                    {
                        var emailService = new EmailService(EmailProvider.Instance(), new EmbeddedTemplateEngine(),
                                                        SubtextContext);
                        emailService.EmailCommentToBlogAuthor(contactMessage);
                    }
                    catch (Exception)
                    {
                        lblMessage.Text =
                            "Your message could not be sent, most likely due to a problem with the mail server.";
                        return;
                    }
                }
                if (!Context.User.IsAdministrator())
                {
                    lblMessage.Text = "Your message was sent.";
                }
                else
                {
                    lblMessage.Text = "Message submitted successfully, but no email was sent because you&#8217;re logged in as administrator.";
                }
                SwapWithLabel(tbName);
                SwapWithLabel(tbEmail);
                SwapWithLabel(tbMessage);
                SwapWithLabel(tbSubject);
                if (captcha != null)
                {
                    Controls.Remove(captcha);
                }
                Controls.Remove(btnSend);
            }
        }

        private void SwapWithLabel(TextBox textBox)
        {
            int index = Controls.IndexOf(textBox);
            Controls.RemoveAt(index);
            Controls.AddAt(index, new LiteralControl(HttpUtility.HtmlEncode(textBox.Text)));
            textBox.Visible = true;
        }

        private void CreateCommentWithContactMessage(FeedbackItem contactMessage)
        {
            try
            {
                ICommentSpamService feedbackService = null;
                if (Blog.FeedbackSpamServiceEnabled)
                {
                    feedbackService = new AkismetSpamService(Blog.FeedbackSpamServiceKey, Blog, null, Url);
                }
                var commentService = new CommentService(SubtextContext,
                                                        new CommentFilter(SubtextContext, feedbackService));
                commentService.Create(contactMessage, true/*runFilters*/);
            }
            catch (BaseCommentException exc)
            {
                lblMessage.Text = exc.Message;
            }
        }
    }
}