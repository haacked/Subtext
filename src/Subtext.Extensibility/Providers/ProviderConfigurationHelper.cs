#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using Subtext.Extensibility.Properties;

namespace Subtext.Extensibility.Providers
{
    public static class ProviderConfigurationHelper
    {
        /// <summary>
        /// Helper method for populating a provider collection 
        /// from a Provider section handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GenericProviderCollection<T> LoadProviderCollection<T>(string sectionName, out T provider)
            where T : ProviderBase
        {
            // Get a reference to the provider section
            var section = (ProviderSectionHandler)WebConfigurationManager.GetSection(sectionName);

            // Load registered providers and point _provider
            // to the default provider
            var providers = new GenericProviderCollection<T>();
            ProvidersHelper.InstantiateProviders(section.Providers, providers, typeof(T));

            provider = providers[section.DefaultProvider];
            if(provider == null)
            {
                throw new ProviderException(string.Format(Resources.Provider_UnableToLoadDefaultProvider, sectionName));
            }

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
            if(settingKey == null)
            {
                throw new ArgumentNullException("settingKey");
            }

            if(configValue == null)
            {
                throw new ArgumentNullException("configValue");
            }

            string settingValue = configValue[settingKey];

            if(settingKey == "connectionStringName")
            {
                if(String.IsNullOrEmpty(settingValue))
                {
                    settingValue = ConfigurationManager.AppSettings["connectionStringName"];
                }
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[settingValue];
                if(setting == null)
                {
                    throw new ArgumentException(Resources.InvalidOperationException_ConnectionStringNotFound,
                                                "settingKey");
                }

                return setting.ConnectionString;
            }
            return null;
        }
    }
}