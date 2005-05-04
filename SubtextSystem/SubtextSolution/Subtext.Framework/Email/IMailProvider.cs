using System;

namespace Subtext.Framework.Email
{
	/// <summary>
	/// Summary description for IMailProvider.
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
