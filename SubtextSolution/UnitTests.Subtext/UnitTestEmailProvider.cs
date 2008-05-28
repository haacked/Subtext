using System;
using Subtext.Extensibility.Providers;

namespace UnitTests.Subtext
{
	public class UnitTestEmailProvider : EmailProvider
	{
		EmailProvider internalProvider;
		
		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public override bool Send(string to, string from, string subject, string message)
		{
			To = to;
			From = from;
			Subject = subject;
			Message = message;
			
			if (internalProvider == null)
				return true;
			
			return internalProvider.Send(to, from, subject, message);
		}

        public void SetInternalProvider(EmailProvider provider)
		{
			internalProvider = provider;
		}

		public string To;
		public string From;
		public string Subject;
		public string Message;
        public string ReplyTo;
	}
}
