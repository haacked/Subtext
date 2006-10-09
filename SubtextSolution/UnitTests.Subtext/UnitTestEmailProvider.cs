using System;
using Subtext.Extensibility.Providers;

namespace UnitTests.Subtext
{
	public class UnitTestEmailProvider : EmailProvider
	{
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
			return true;
		}
	}
}
