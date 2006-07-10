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
using Subtext.Extensibility.Providers;
using System.Net;

namespace Subtext.Framework.Email
{
	/// <summary>
	/// Default implementation of the <see cref="EmailProvider"/>.  This uses 
	/// the new (introduced in .NET 2.0) System.Net.SmtpClient class which uses SMTP.
	/// </summary>
	public class SystemMailProvider : EmailProviderBase
	{
		public override bool Send(string toStr, string fromStr, string subject, string message)
		{
			try
			{   
                MailAddress from = new MailAddress(fromStr);
                MailAddress to = new MailAddress(toStr);

                MailMessage em = new MailMessage(from, to);

				em.Subject = subject;
				em.Body = message;

                SmtpClient client = new SmtpClient(this.SmtpServer);

				if(this.UserName != null && this.Password != null)
				{
                    NetworkCredential basicAuthCredential = new NetworkCredential(this.UserName, this.Password);
                    client.UseDefaultCredentials = false;
                    client.Credentials = basicAuthCredential;
				}
                
				client.Send(em);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}

