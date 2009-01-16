using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Email;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Security;
using Subtext.Framework.Web;
using Subtext.Web.Controls.Captcha;

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

namespace Subtext.Web.UI.Controls
{
	public  class Contact : BaseControl
	{
		protected ValidationSummary ValidationSummary1;
		protected Label lblMessage;
		protected Button btnSend;
		protected RequiredFieldValidator RequiredFieldValidator1;
		protected TextBox tbMessage;
		protected TextBox tbSubject;
		protected RegularExpressionValidator RegularExpressionValidator1;
		protected RequiredFieldValidator RequiredFieldValidator2;
		protected TextBox tbEmail;
		protected TextBox tbName;
		protected InvisibleCaptcha invisibleCaptchaValidator;
		protected CaptchaControl captcha;

		/// <summary>
		/// Initializes the control.  Sets up the send button's 
		/// click event handler.
		/// </summary>
		/// <param name="e"></param>
		override protected void OnInit(EventArgs e)
		{
			this.btnSend.Click += new EventHandler(this.btnSend_Click);

			EnsureEmailRequired();
			//Captcha should not be given to admin.
			if (!SecurityHelper.IsAdmin)
			{
				int btnIndex = Controls.IndexOf(this.btnSend);
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
			foreach(Control control in this.Controls)
			{
				RequiredFieldValidator validator = control as RequiredFieldValidator;
				if (validator == null)
					continue;

				if (validator.ControlToValidate == tbEmail.ID)
					return;
			}
			RequiredFieldValidator emailRequiredValidator = new RequiredFieldValidator();
			emailRequiredValidator.ControlToValidate = tbEmail.ID;
			emailRequiredValidator.ErrorMessage = "* Please enter your email address";
			emailRequiredValidator.Display = ValidatorDisplay.Dynamic;
			Controls.AddAt(Controls.IndexOf(tbEmail) + 1, emailRequiredValidator);
		}


		private void btnSend_Click(object sender, EventArgs e)
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
				string fromEmail = tbEmail.Text;
				
				string subject = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} (via {1})", tbSubject.Text, 
				                               info.Title);

				string sendersIpAddress = HttpHelper.GetUserIpAddress(Context).ToString();

				// \n by itself has issues with qmail (unix via openSmtp), \r\n should work on unix + wintel
				string body = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Mail from {0}:\r\n\r\nSender: {1}\r\nEmail: {2}\r\nIP Address: {3}\r\n=====================================\r\n{4}", 
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
                catch(Exception) {
					lblMessage.Text = "Your message could not be sent, most likely due to a problem with the mail server.";
				}
			}
		}

		private void CreateCommentWithContactMessage()
		{
			FeedbackItem contactMessage = new FeedbackItem(FeedbackType.None);

			contactMessage.Author = tbName.Text;
			contactMessage.Email = tbEmail.Text;
			contactMessage.Body = tbMessage.Text;
			contactMessage.Title = "CONTACT: " + tbSubject.Text;
			contactMessage.IpAddress = HttpHelper.GetUserIpAddress(Context);

			try
			{
				FeedbackItem.Create(contactMessage, new CommentFilter(HttpContext.Current.Cache));
                var emailService = new EmailService(EmailProvider.Instance(), new EmbeddedTemplateEngine(), SubtextContext);
                emailService.EmailCommentToBlogAuthor(contactMessage);
				lblMessage.Text = "Your message was sent.";
			}
			catch (BaseCommentException exc)
			{
				lblMessage.Text = exc.Message;
			}

			tbName.Text = string.Empty;
			tbEmail.Text = string.Empty;
			tbSubject.Text = string.Empty;
			tbMessage.Text = string.Empty;
		}

		static bool SendContactMessageToFeedback
		{
			get
			{
                string contactSetting = System.Configuration.ConfigurationManager.AppSettings["ContactToFeedback"];
				if(contactSetting != null)
					try
					{
						return bool.Parse(contactSetting);
					}
					catch(FormatException)
					{
						
					}
				return false;
			}
		}
	}
}

