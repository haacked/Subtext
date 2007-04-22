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
using System.Net.Mail;
using System.Text;
using log4net;
using Subtext.Extensibility.Providers;
using System.Net;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Default implementation of the <see cref="EmailProvider"/>.  This uses 
	/// the new (introduced in .NET 2.0) System.Net.SmtpClient class which uses SMTP.
	/// </summary>
	public class SystemMailProvider : EmailProvider
	{
		private readonly static ILog log = new Logging.Log();

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="toAddress">The address to send the email to</param>
		/// <param name="fromAddress">The address to send the email from</param>
		/// <param name="replyTo">The email address to use in the ReplyTo header</param>
		/// <param name="subject">The subject of the email</param>
		/// <param name="message">The email contents</param>
		/// <returns>True if the email has sent, otherwise false</returns>
		public override bool Send(string to, string from, string subject, string message)
		{
			return ( Send(to, from, null, subject, message) );
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
			try
			{
				MailAddress fromAddress = new MailAddress(from);
				MailAddress toAddress = new MailAddress(to);

				MailMessage em = new MailMessage(fromAddress, toAddress);
                if (null != replyTo && replyTo.Length > 0)
                {
                    em.Headers.Add("Reply-To", replyTo);
                }

				em.BodyEncoding = Encoding.UTF8;
				em.Subject = subject;
				em.Body = message;

				SmtpClient client = new SmtpClient(this.SmtpServer);
				client.Port = this.Port;

				if (this.UserName != null && this.Password != null)
				{
					client.UseDefaultCredentials = false;
					client.Credentials = new NetworkCredential(this.UserName, this.Password);
				}

				client.Send(em);
				return true;
			}
			catch (SmtpException exc)
			{
				//TODO: One reason an email could be rejected is that the email server 
				//		might reject the send (from) email address.
				//      We should probably throw an exception instead of returning 
				//		true or false. But we'll have to define each exception ourselves.
				log.Error("Could not send email.", exc);
				return false;
			}
		}

	}
}