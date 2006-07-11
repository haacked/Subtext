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
using System.Configuration.Provider;
using System.Web.Configuration;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Provides a class used to handle email.
	/// </summary>
    public abstract class EmailProvider : System.Configuration.Provider.ProviderBase
	{

        private static EmailProvider _provider = null;
        private static GenericProviderCollection<EmailProvider> _providers = null;
        private static object _lock = new object();


        public static EmailProvider Instance()
        {
            LoadProviders();
            return _provider;
        }

        private static void LoadProviders()
        {
            // Avoid claiming lock if providers are already loaded
            if (_provider == null)
            {
                lock (_lock)
                {
                    // Do this again to make sure _provider is still null
                    if (_provider == null)
                    {
                        // Get a reference to the <EmailProvider> section
                        EmailProviderSectionHandler section = (EmailProviderSectionHandler)
                            WebConfigurationManager.GetSection
                            ("EmailProvider");

                        // Load registered providers and point _provider
                        // to the default provider
                        _providers = new GenericProviderCollection<EmailProvider>();
                        ProvidersHelper.InstantiateProviders
                            (section.Providers, _providers,
                            typeof(EmailProvider));
                        _provider = _providers[section.DefaultProvider];

                        if (_provider == null)
                            throw new ProviderException
                                ("Unable to load default EmailProvider");
                    }
                }
            }
        }


		#region EmailProvider Methods
		/// <summary>
		/// Gets or sets the admin email.  This is the email address that 
		/// emails sent to a blog owner appears to be from.  It represents 
		/// the system and might not be a real address.
		/// </summary>
		/// <value></value>
		public abstract string AdminEmail
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the SMTP server.
		/// </summary>
		/// <value></value>
		public abstract string SmtpServer
		{
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets the SMTP server port.
		/// </summary>
		/// <value></value>
		public abstract int Port
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the password for SMTP servers that 
		/// require authentication.
		/// </summary>
		/// <value></value>
		public abstract string Password
		{
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets the name of the user for SMPT servers that 
		/// require authentication.
		/// </summary>
		/// <value></value>
		public abstract string UserName
		{
			get;
			set;
		}

		public abstract bool Send(string to, string from, string subject, string message);
		#endregion
	}
}
