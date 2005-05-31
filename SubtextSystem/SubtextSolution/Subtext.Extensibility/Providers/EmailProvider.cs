using System;
using Subtext.Extensibility.Providers;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provides a class used to handle email.
	/// </summary>
	public abstract class EmailProvider : ProviderBase
	{
		/// <summary>
		/// Returns the configured concrete instance of a <see cref="EmailProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static EmailProvider Instance()
		{
			return (EmailProvider)ProviderBase.Instance("Email");
		}

		#region EmailProvider Methods
		public abstract string AdminEmail
		{
			get;
			set;
		}

		public abstract string SmtpServer
		{
			get;
			set;
		}

		public abstract string Password
		{
			get;
			set;
		}
		
		public abstract string UserName
		{
			get;
			set;
		}

		public abstract bool Send(string to, string from, string subject, string message);
		#endregion
	}
}
