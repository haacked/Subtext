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
using System.Net;
using System.Net.Mail;
using System.Text;
using log4net;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Threading;
using System.Threading;

namespace Subtext.Framework.Email
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
		/// <param name="toStr"></param>
		/// <param name="fromStr"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public override void Send(string to, string from, string subject, string message)
		{
            ThreadPool.QueueUserWorkItem(callback => SendAsync(to, from, subject, message));
		}

        private void SendAsync(string toStr, string fromStr, string subject, string message) {
            try
            {
                MailAddress from = new MailAddress(fromStr);
                MailAddress to = new MailAddress(toStr);

                MailMessage em = new MailMessage(from, to);
                em.BodyEncoding = Encoding.UTF8;
                em.Subject = subject;
                em.Body = message;

                SmtpClient client = new SmtpClient(this.SmtpServer);
                client.Port = this.Port;
                client.EnableSsl = this.SslEnabled;

                if (this.UserName != null && this.Password != null)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(this.UserName, this.Password);
                }

                client.Send(em);
            }
            catch (Exception e)
            {
                log.Error("Could not send email.", e);
                //Swallow as this was on an async thread.
            }
            
        }
        
        delegate void SendEmailDelegate(string to, string from, string subject, string body);
	}
}


