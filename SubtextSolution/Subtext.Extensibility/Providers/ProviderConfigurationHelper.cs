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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;

namespace Subtext.Extensibility.Providers
{
    public class ProviderConfigurationHelper
    {
        /// <summary>
        /// Determines whether the specified setting value (an attribute value 
        /// in the "add" element of a provider section in web.config) is actually 
        /// the name of an AppSetting variable.
        /// </summary>
        /// <param name="settingValue">Setting value.</param>
        /// <returns>
        /// 	<c>true</c> if the setting value is a pointer to app settings; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPointerToAppSettings(string settingValue)
        {
            if (settingValue == null)
                return false;

            return settingValue.StartsWith("${") && settingValue.EndsWith("}");
        }

        /// <summary>
        /// Gets the setting value for the specfied setting name and configValue dictionary.
        /// </summary>
        /// <param name="settingKey">Setting Name.</param>
        /// <param name="configValue">Config value.</param>
        /// <returns></returns>
        public static string GetSettingValue(string settingKey, System.Collections.Specialized.NameValueCollection configValue)
        {
            if (settingKey == null)
                throw new ArgumentNullException("settingKey", "The setting key is null. The provider may not be configured correctly.");

            if (configValue == null)
                throw new ArgumentNullException("configValue", "The config values collection is null. The provider for the setting '" + settingKey + "' may not be configured correctly.");

            string settingValue = configValue[settingKey];

            if (settingKey == "connectionStringName")
            {
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[settingValue];
                if (setting == null)
                    throw new ArgumentException("The Connection String '" + settingValue + "' was not found in the ConnectionStrings section.", "settingKey");

                return setting.ConnectionString;
            }

            const int StartDelimiterLength = 2;
            const int EndDelimiterLength = 1;
            if (IsPointerToAppSettings(settingValue))
            {
                string appKeyName = settingValue.Substring(StartDelimiterLength, settingValue.Length - (StartDelimiterLength + EndDelimiterLength));
                settingValue = System.Configuration.ConfigurationManager.AppSettings[appKeyName];
            }

            if (ConfigurationManager.ConnectionStrings[settingValue] != null)
            {
                settingValue = ConfigurationManager.ConnectionStrings[settingValue].ConnectionString;
            }

            return settingValue;
        }
	
    }
}
