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
	public class DotNetOpenMailProvider : EmailProviderBase
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
			email.TextPart = new TextAttachment(message);

			SmtpServer smtpServer = new SmtpServer(SmtpServer);
			
			//Authentication.
			if(this.UserName != null && this.Password != null)
			{
				smtpServer.SmtpAuthToken = new SmtpAuthToken(UserName, Password);
			}

			try
			{
				return email.Send(smtpServer);
			}
			catch(Exception e)
			{
				Log.Error("Error occurred while sending an email.", e);
			}
			return false;
		}
	}
}
