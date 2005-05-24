using System;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Interface for email providers,
	/// </summary>
	public interface IMailProvider
	{
		string AdminEmail
		{
			get;
			set;
		}

		string SmtpServer
		{
			get;
			set;
		}

		string Password
		{
			get;
			set;
		}
		
		string UserName
		{
			get;
			set;
		}

		bool Send(string to, string from, string subject, string message);
	}
}
