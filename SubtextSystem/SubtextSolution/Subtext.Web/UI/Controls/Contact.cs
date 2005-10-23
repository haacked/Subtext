using System;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
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

		/// <summary>
		/// Initializes the control.  Sets up the send button's 
		/// click event handler.
		/// </summary>
		/// <param name="e"></param>
		override protected void OnInit(EventArgs e)
		{
			this.btnSend.Click += new EventHandler(this.btnSend_Click);
			base.OnInit(e);
		}
		

		private void btnSend_Click(object sender, EventArgs e)
		{
			if(Page.IsValid)
			{
				if(SendContactMessageToFeedback)
				{
					Entry entry = new Entry(PostType.Comment);
					entry.Author = tbName.Text;
					entry.Email = tbEmail.Text;
					entry.Body = tbMessage.Text;
					entry.TitleUrl = "contact.aspx";
					entry.Title = "CONTACT: " + tbSubject.Text;
					entry.SourceName = Subtext.Framework.Util.Globals.GetUserIpAddress(Context);
					Entries.InsertComment(entry);
					
					lblMessage.Text = "Your message was sent.";
					tbName.Text = "";
					tbEmail.Text = "";
					tbSubject.Text = "";
					tbMessage.Text = "";
					return;
				}

				EmailProvider email = EmailProvider.Instance();
				BlogInfo info = Config.CurrentBlog;
				string toEmail = info.Email;
				string fromEmail = tbEmail.Text;
				
				string Subject = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} (via {1})", tbSubject.Text, 
				                               info.Title);

				string sendersIpAddress = Framework.Util.Globals.GetUserIpAddress(Context);

				// \n by itself has issues with qmail (unix via openSmtp), \r\n should work on unix + wintel
				string Body = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Mail from {0}:\r\n\r\nSender: {1}\r\nEmail: {2}\r\nIP Address: {3}\r\n=====================================\r\n{4}", 
				                            info.Title,
					tbName.Text,
					tbEmail.Text,
					sendersIpAddress,
					tbMessage.Text);				

				if(email.Send(toEmail,fromEmail,Subject,Body))
				{
					lblMessage.Text = "Your message was sent.";
					tbName.Text = "";
					tbEmail.Text = "";
					tbSubject.Text = "";
					tbMessage.Text = "";
				}
				else
				{
					lblMessage.Text = "Your message could not be sent, most likely due to a problem with the mail server.";
				}
			}
		}

		bool SendContactMessageToFeedback
		{
			get
			{
				string contactSetting = System.Configuration.ConfigurationSettings.AppSettings["ContactToFeedback"];
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

