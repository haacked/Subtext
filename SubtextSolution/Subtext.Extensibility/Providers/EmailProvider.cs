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
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provides a class used to handle email.
	/// </summary>
    public abstract class EmailProvider : ProviderBase
	{
        private static EmailProvider provider;
		private static GenericProviderCollection<EmailProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<EmailProvider>("Email", out provider);
        private const int DefaultSmtpPort = 25;
        private string _smtpServer = "localhost";
       
        /// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
			_name = name;
			AdminEmail = configValue["adminEmail"];
			SmtpServer = configValue["smtpServer"];
			Password = configValue["password"];
			UserName = configValue["username"];
			if (configValue["port"] != null) {
                int port;
                if (int.TryParse(configValue["port"], out port)) {
                    Port = port;
                }
                else {
                    Port = DefaultSmtpPort;
                }
			}
			
            if (configValue["sslEnabled"] != null)
			{
                bool sslEnabled;
                if (bool.TryParse(configValue["sslEnabled"], out sslEnabled)) {
                    SslEnabled = sslEnabled;
                }
			}
		}

		/// <summary>
		/// Returns the currently configured Email Provider.
		/// </summary>
		/// <returns></returns>
        public static EmailProvider Instance()
        {
            return provider;
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
		/// Gets or sets the admin email.  This is the email address that 
		/// emails sent to a blog owner appears to be from.  It represents 
		/// the system and might not be a real address.
		/// </summary>
		/// <value></value>
		public string AdminEmail {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the SMTP server.  If not specified, 
		/// defaults to "localhost";
		/// </summary>
		/// <value></value>
		public string SmtpServer {
			get
			{
                if (_smtpServer == null || _smtpServer.Length == 0)
                {
                    _smtpServer = "localhost";
                }
				return _smtpServer;
			}
			set {
				_smtpServer = value;
			}
		}


		/// <summary>
		/// Gets and sets the port.
		/// </summary>
		/// <value>The port.</value>
		public int Port {
			get;
			set;
		}

		/// <summary>
		/// Gets and sets the SSL protocol enable.
		/// </summary>
		/// <value>true or false.</value>
		public bool SslEnabled {
            get;
            set;
		}

		/// <summary>
		/// Gets or sets the password used for SMTP servers that 
		/// require authentication.
		/// </summary>
		/// <value></value>
		public string Password {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name of the user for smpt servers that require authentication.
		/// </summary>
		/// <value></value>
		public string UserName {
			get;
			set;
		}


		/// <summary>
		/// Returns the friendly name of the provider when the provider is initialized.
		/// </summary>
		/// <value></value>
		public override string Name
		{
            get {
                return _name;
            }
		}
        private string _name;
		
		/// <summary>
		/// Sends an email.
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public abstract void Send(string to, string from, string subject, string message);
	}
}
