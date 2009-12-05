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
                if(contactSetting != null)
                {
                    try
                    {
                        return bool.Parse(contactSetting);
                    }
                    catch(FormatException)
                    {
                    }
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
            if(!SecurityHelper.IsAdmin)
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
            foreach(Control control in Controls)
            {
                var validator = control as RequiredFieldValidator;
                if(validator == null)
                {
                    continue;
                }

                if(validator.ControlToValidate == tbEmail.ID)
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
            if(Page.IsValid)
            {
                Blog info = Blog;

                if(SendContactMessageToFeedback || String.IsNullOrEmpty(info.Email))
                {
                    CreateCommentWithContactMessage();
                    return;
                }

                EmailProvider email = EmailProvider.Instance();
                string toEmail = info.Email;
                string fromEmail = email.UseCommentersEmailAsFromAddress
                                       ? tbEmail.Text ?? email.AdminEmail
                                       : email.AdminEmail;

                string subject = string.Format(CultureInfo.InvariantCulture, "{0} (via {1})", tbSubject.Text,
                                               info.Title);

                string sendersIpAddress = HttpHelper.GetUserIpAddress(SubtextContext.HttpContext).ToString();

                // \n by itself has issues with qmail (unix via openSmtp), \r\n should work on unix + wintel
                string body = string.Format(CultureInfo.InvariantCulture,
                                            "Mail from {0}:\r\n\r\nSender: {1}\r\nEmail: {2}\r\nIP Address: {3}\r\n=====================================\r\n{4}",
                                            info.Title,
                                            tbName.Text,
                                            tbEmail.Text,
                                            sendersIpAddress,
                                            tbMessage.Text);

                try
                {
                    email.Send(toEmail, fromEmail, subject, body);
                    lblMessage.Text = "Your message was sent.";
                    tbName.Text = string.Empty;
                    tbEmail.Text = string.Empty;
                    tbSubject.Text = string.Empty;
                    tbMessage.Text = string.Empty;
                }
                catch(Exception)
                {
                    lblMessage.Text =
                        "Your message could not be sent, most likely due to a problem with the mail server.";
                }
            }
        }

        private void CreateCommentWithContactMessage()
        {
            var contactMessage = new FeedbackItem(FeedbackType.None)
            {
                Author = tbName.Text,
                Email = tbEmail.Text,
                Body = tbMessage.Text,
                Title = string.Format("CONTACT: {0}", tbSubject.Text),
                IpAddress = HttpHelper.GetUserIpAddress(SubtextContext.HttpContext)
            };

            try
            {
                ICommentSpamService feedbackService = null;
                if(Blog.FeedbackSpamServiceEnabled)
                {
                    feedbackService = new AkismetSpamService(Blog.FeedbackSpamServiceKey, Blog, null, Url);
                }
                var commentService = new CommentService(SubtextContext,
                                                        new CommentFilter(SubtextContext, feedbackService));
                commentService.Create(contactMessage, true/*runFilters*/);
                var emailService = new EmailService(EmailProvider.Instance(), new EmbeddedTemplateEngine(),
                                                    SubtextContext);
                emailService.EmailCommentToBlogAuthor(contactMessage);
                lblMessage.Text = "Your message was sent.";
            }
            catch(BaseCommentException exc)
            {
                lblMessage.Text = exc.Message;
            }

            tbName.Text = string.Empty;
            tbEmail.Text = string.Empty;
            tbSubject.Text = string.Empty;
            tbMessage.Text = string.Empty;
        }

        //todo: move this to an appropriate place.
    }
}