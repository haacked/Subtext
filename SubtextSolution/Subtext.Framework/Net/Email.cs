using System;
using Subtext.Extensibility.Providers;

namespace Subtext.Framework.Net
{
	/// <summary>
	/// Email API entry point. Use this to send emails.
	/// </summary>
	public static class Email
	{
		private static EmailProvider provider;
		private static GenericProviderCollection<EmailProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<EmailProvider>("Email", out provider);

		/// <summary>
		/// Returns the currently configured Email Provider.
		/// </summary>
		/// <returns></returns>
		public static EmailProvider Provider
		{
			get
			{
				return provider;
			}
		}	

		/// <summary>
		/// Returns all the configured Email Providers.
		/// </summary>
		public static GenericProviderCollection<EmailProvider> Providers
		{
			get
			{
				return providers;
			}
		}

		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static bool Send(string to, string from, string subject, string message)
		{
			return Provider.Send(to, from, subject, message);
		}
	}
}
