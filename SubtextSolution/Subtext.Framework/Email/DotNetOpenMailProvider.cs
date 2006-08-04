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

namespace Subtext.Framework.Email
{
	/// <summary>
	/// Implements the <see cref="EmailProvider"/> using an open source library, 
	/// <see href="http://dotnetopenmail.sourceforge.net/">DotNetOpenMail</see>. 
	/// </summary>
	public class DotNetOpenMailProvider : EmailProvider
	{
		static Log Log = new Log();

		/// <summary>
		/// Sends an email with the specified parameters.
		/// </summary>
		/// <param name="to">Email address of the recipient.</param>
		/// <param name="from">Email address of the sender.</param>
		/// <param name="subject">Subject of the email.</param>
		/// <param name="message">The body of the email Message.</param>
		/// <returns></returns>
		public override bool Send(string to, string from, string subject, string message)
		{
			EmailMessage email = new EmailMessage();
			email.FromAddress = new EmailAddress(from);
			email.AddToAddress(new EmailAddress(to));
			email.Subject = subject;
			email.BodyText = message;

			SmtpServer smtpServer = new SmtpServer(SmtpServer, Port);
			
			//Authentication.
			if(this.UserName != null && this.Password != null)
			{
				smtpServer.SmtpAuthToken = new SmtpAuthToken(UserName, Password);
			}

			try
			{
				return email.Send(smtpServer);
			}
		    //Mail Exception is thrown when there are network or connection errors
			catch(MailException mailEx)
			{
                string msg = String.Format("Connection or network error sending email from {0} to {1}", from, to);
				Log.Error(msg, mailEx);
			}
		    //SmtpException is thrown for all SMTP exceptions
		    catch (SmtpException smtpEx)
		    {
                string msg = String.Format("Error sending email from {0} to {1}", from, to);
		        Log.Error(msg, smtpEx);
		    }
			return false;
		}
	}
}
