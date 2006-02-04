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
using System.Web.Mail;
using Subtext.Extensibility.Providers;

namespace Subtext.Framework.Email
{
	/// <summary>
	/// Default implementation of the <see cref="EmailProvider"/>.  This uses 
	/// classes in the System.Web.Mail namespace which have dependencies on 
	/// CDONTS.
	/// </summary>
	public class SystemMailProvider : EmailProviderBase
	{
		public override bool Send(string to, string from, string subject, string message)
		{
			try
			{
				MailMessage em = new MailMessage();
				em.To = to;
				em.From = from;
				em.Subject = subject;
				em.Body = message;

				// Found out how to send authenticated email via System.Web.Mail 
				// at http://SystemWebMail.com (fact 3.8)
				if(this.UserName != null && this.Password != null)
				{
					em.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");	//basic authentication
					em.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", this.UserName); //set your username here
					em.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", this.Password);	//set your password here
				}

				SmtpMail.SmtpServer = this.SmtpServer;
				SmtpMail.Send(em);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}

