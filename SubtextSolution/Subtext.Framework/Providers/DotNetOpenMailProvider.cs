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
using DotNetOpenMail;
using DotNetOpenMail.SmtpAuth;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Logging;
using System.Globalization;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Implements the <see cref="EmailProvider"/> using an open source library, 
	/// <see href="http://dotnetopenmail.sourceforge.net/">DotNetOpenMail</see>. 
	/// </summary>
	public class DotNetOpenMailProvider : EmailProvider
	{
		static Log Log = new Log();

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="toAddress">The address to send the email to</param>
		/// <param name="fromAddress">The address to send the email from</param>
		/// <param name="subject">The subject of the email</param>
		/// <param name="message">The email contents</param>
		/// <returns>True if the email has sent, otherwise false</returns>
		public override bool Send(string to, string from, string subject, string message)
		{
			return (Send(to, from, null, subject, message));
		}

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="toAddress">The address to send the email to</param>
		/// <param name="fromAddress">The address to send the email from</param>
		/// <param name="replyTo">The email address to use in the ReplyTo header</param>
		/// <param name="subject">The subject of the email</param>
		/// <param name="message">The email contents</param>
		/// <returns>True if the email has sent, otherwise false</returns>
		public override bool Send(string to, string from, string replyTo, string subject, string message)
		{
			EmailMessage email = new EmailMessage();
			email.FromAddress = new EmailAddress(from);
			email.AddToAddress(new EmailAddress(to));
			email.Subject = subject;
			email.BodyText = message;

            if (null != replyTo && replyTo.Length > 0)
            {
                email.AddCustomHeader("Reply-To", replyTo);
            }

			SmtpServer smtpServer = new SmtpServer(SmtpServer, Port);

			//Authentication.
			if (this.UserName != null && this.Password != null)
			{
				smtpServer.SmtpAuthToken = new SmtpAuthToken(UserName, Password);
			}

			try
			{
				return email.Send(smtpServer);
			}
			//Mail Exception is thrown when there are network or connection errors
			catch (MailException mailEx)
			{
				string msg = String.Format(CultureInfo.CurrentUICulture, "Connection or network error sending email from {0} to {1}", from, to);
				Log.Error(msg, mailEx);
			}
			//SmtpException is thrown for all SMTP exceptions
			catch (SmtpException smtpEx)
			{
				string msg = String.Format(CultureInfo.CurrentUICulture, "Error sending email from {0} to {1}", from, to);
				Log.Error(msg, smtpEx);
			}
			return false;
		}
	}
}
