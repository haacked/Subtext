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
using System.Net;
using System.Net.Mail;
using System.Text;
using log4net;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Email
{
    /// <summary>
    /// Default implementation of the <see cref="EmailProvider"/>.  This uses 
    /// the new (introduced in .NET 2.0) System.Net.SmtpClient class which uses SMTP.
    /// </summary>
    public class SystemMailProvider : EmailProvider
    {
        private readonly static ILog Log = new Log();

        /// <summary>
        /// Sends an email.
        /// </summary>
        public override void Send(string to, string from, string subject, string message)
        {
            SendAsync(to, from, subject, message);
        }

        private void SendAsync(string toStr, string fromStr, string subject, string message)
        {
            try
            {
                var from = new MailAddress(fromStr);
                var to = new MailAddress(toStr);

                var em = new MailMessage(from, to) {BodyEncoding = Encoding.UTF8, Subject = subject, Body = message, ReplyTo = from};

                var client = new SmtpClient(SmtpServer) {Port = Port, EnableSsl = SslEnabled};

                if(UserName != null && Password != null)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(UserName, Password);
                }

                client.Send(em);
            }
            catch(Exception e)
            {
                Log.Error("Could not send email.", e);
                //Swallow as this was on an async thread.
            }
        }
    }
}