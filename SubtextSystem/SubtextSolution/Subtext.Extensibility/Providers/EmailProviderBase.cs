using System;
using System.Collections.Specialized;


namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Abstract base class that classes that implement <see cref="EmailProvider"/> 
	/// may optionally use.  This class encapsulates some common behavior among 
	/// email providers.
	/// </summary>
	public abstract class EmailProviderBase : EmailProvider
	{
		string _name;
		string _smtpServer = "localhost";
		string _password;
		string _userName;
		string _adminEmail;

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
			_name = name;
			_adminEmail = configValue["adminEmail"];
			_smtpServer = configValue["smtpServer"];
			_password = configValue["password"];
			_userName = configValue["username"];
		}

		/// <summary>
		/// Gets or sets the admin email.  This is the email address that 
		/// emails sent to a blog owner appears to be from.  It represents 
		/// the system and might not be a real address.
		/// </summary>
		/// <value></value>
		public override string AdminEmail
		{
			get
			{
				return _adminEmail;
			}
			set
			{
				_adminEmail = value;
			}
		}

		/// <summary>
		/// Gets or sets the SMTP server.  If not specified, 
		/// defaults to "localhost";
		/// </summary>
		/// <value></value>
		public override string SmtpServer
		{
			get 
			{
				if(_smtpServer == null || _smtpServer.Length == 0)
					_smtpServer = "localhost";
				return _smtpServer;
			}
			set
			{
				_smtpServer = value;
			}
		}

		/// <summary>
		/// Gets or sets the password used for SMTP servers that 
		/// require authentication.
		/// </summary>
		/// <value></value>
		public override string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the user for smpt servers that require authentication.
		/// </summary>
		/// <value></value>
		public override string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				_userName = value;
			}
		}

		
		/// <summary>
		/// Returns the friendly name of the provider when the provider is initialized.
		/// </summary>
		/// <value></value>
		public override string Name
		{
			get
			{
				return _name;
			}
		}
	}
}
