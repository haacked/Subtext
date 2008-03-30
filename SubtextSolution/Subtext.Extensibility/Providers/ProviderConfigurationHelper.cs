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
using System.Configuration;
using System.Web.Configuration;

namespace Subtext.Extensibility.Providers
{
    public class ProviderConfigurationHelper
    {
		/// <summary>
		/// Helper method for populating a provider collection 
		/// from a Provider section handler.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static GenericProviderCollection<T> LoadProviderCollection<T>(string sectionName, out T provider) where T : ProviderBase
		{
			// Get a reference to the provider section
			ProviderSectionHandler section = (ProviderSectionHandler)WebConfigurationManager.GetSection(sectionName);

			// Load registered providers and point _provider
			// to the default provider
			GenericProviderCollection<T> providers = new GenericProviderCollection<T>();
			ProvidersHelper.InstantiateProviders(section.Providers, providers, typeof(T));

			provider = providers[section.DefaultProvider];
			if (provider == null)
				throw new ProviderException(string.Format("Unable to load default '{0}' provider", sectionName));

			return providers;
		}
    	
		/// <summary>
        /// Gets the setting value for the specfied setting name and configValue dictionary.
        /// </summary>
        /// <param name="settingKey">Setting Name.</param>
        /// <param name="configValue">Config value.</param>
        /// <returns></returns>
        public static string GetConnectionStringSettingValue(string settingKey, NameValueCollection configValue)
        {
            if (settingKey == null)
                throw new ArgumentNullException("settingKey", "The setting key is null. The provider may not be configured correctly.");

            if (configValue == null)
                throw new ArgumentNullException("configValue", "The config values collection is null. The provider for the setting '" + settingKey + "' may not be configured correctly.");

            string settingValue = configValue[settingKey];

            if (settingKey == "connectionStringName")
            {
				if (String.IsNullOrEmpty(settingValue))
					settingValue = ConfigurationManager.AppSettings["connectionStringName"];
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[settingValue];
                if (setting == null)
                    throw new ArgumentException("The Connection String '" + settingValue + "' was not found in the ConnectionStrings section.", "settingKey");

                return setting.ConnectionString;
            }
			return null;
        }
	
    }
}
