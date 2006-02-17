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
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Caching;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// <see href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnaspnet/html/asp02182004.asp"/> Part 1 and 
	/// <see href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnaspnet/html/asp04212004.asp"/> Part 2.
	/// </summary>
	public abstract class ProviderBase 
	{
		static Hashtable _providers = new Hashtable();

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public abstract void Initialize(string name, NameValueCollection configValue);

		/// <summary>
		/// Returns the friendly name of the provider when the provider is initialized.
		/// </summary>
		/// <value></value>
		public abstract string Name { get; }

		/// <summary>
		/// Gets the providers installed using the section name.
		/// </summary>
		/// <param name="sectionName">Name of the section.</param>
		/// <returns></returns>
		protected static ProviderCollection GetProviders(string sectionName)
		{
			ProviderConfiguration config = (ProviderConfiguration)ConfigurationSettings.GetConfig(sectionName);
			return config.Providers;
		}

		/// <summary>
		/// Instantiates and returns the default instance of the provider loaded from 
		/// the specified section name.
		/// </summary>
		/// <returns></returns>
		protected static ProviderBase Instance(string sectionName) 
		{
			//In the original configuration, the Cache is used to cache 
			//constructor info.  Not sure why we can't just store a 
			//static instance of the fully loaded provider.
			ProviderBase storedProvider = _providers[sectionName] as ProviderBase;
			
			//This check is now thread safe.
			if(storedProvider != null)
			{
				return storedProvider;
			}

			// Use the cache because the reflection used later is expensive
			ProviderConfiguration config = (ProviderConfiguration)ConfigurationSettings.GetConfig(sectionName);

			ProviderBase provider = Instance(sectionName, config.Providers.DefaultProvider);

			// It's possible that at this point, another thread has already 
			// added the correct provider to this hashtable.  That we might be 
			// overwriting that instance is not a big problem as a provider doesn't 
			// have changing state and this one will be essentially a copy of the 
			// existing one.  Also, it's a rare thing to occur.  So we don't lock 
			// for maximum throughput.
			_providers[sectionName] = provider;
			return provider;
		}

		/// <summary>
		/// Instantiates and returns an instance of this provider loaded from 
		/// the specified section name matching the specified <see cref="ProviderInfo"/>.
		/// </summary>
		/// <param name="sectionName">Name of the section.</param>
		/// <param name="providerInfo">Provider info.</param>
		/// <returns></returns>
		public static ProviderBase Instance(string sectionName, ProviderInfo providerInfo)
		{
			Cache cache = HttpRuntime.Cache;

			// Is the actual provider In the cache?
			string cacheKey = sectionName + "::";
	
			if(providerInfo != null)
				cacheKey += providerInfo.Name;
	
			if (cache[cacheKey] == null) 
			{
				// The assembly should be in \bin or GAC, so we simply need
				// to get an instance of the type
				try 
				{
					Type type = Type.GetType(providerInfo.Type);

					// Insert the type into the cache
					// Provider Types must have a default no parameter constructor
					Type[] paramTypes = new Type[0];
					cache.Insert(cacheKey, type.GetConstructor(paramTypes));
				} 
				catch (Exception e) 
				{
					throw new Exception("Unable to load provider type '" + providerInfo.Type + "'", e);
				}
			}
	
			ConstructorInfo constructor = cache[cacheKey] as ConstructorInfo;
			ProviderBase provider = (ProviderBase)(constructor.Invoke(new object[] {}));
			provider.Initialize(providerInfo.Name, providerInfo.Attributes);
			return provider;
		}

		/// <summary>
		/// Determines whether the specified setting value (an attribute value 
		/// in the "add" element of a provider section in web.config) is actually 
		/// the name of an AppSetting variable.
		/// </summary>
		/// <param name="settingValue">Setting value.</param>
		/// <returns>
		/// 	<c>true</c> if the setting value is a pointer to app settings; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPointerToAppSettings(string settingValue)
		{
			if(settingValue == null)
				return false;

			return settingValue.StartsWith("${") && settingValue.EndsWith("}");
		}

		/// <summary>
		/// Gets the setting value for the specfied setting name and configValue dictionary.
		/// </summary>
		/// <param name="settingKey">Setting Name.</param>
		/// <param name="configValue">Config value.</param>
		/// <returns></returns>
		public string GetSettingValue(string settingKey, System.Collections.Specialized.NameValueCollection configValue)
		{
			if(settingKey == null)
				throw new ArgumentNullException("settingKey", "The setting key is null. The provider may not be configured correctly.");
			
			if(configValue == null)
				throw new ArgumentNullException("configValue", "The config values collection is null. The provider for the setting '" + settingKey + "' may not be configured correctly.");

			const int StartDelimiterLength = 2;
			const int EndDelimiterLength = 1;
			string settingValue = configValue[settingKey];
		
			if(IsPointerToAppSettings(settingValue))
			{
				string appKeyName = settingValue.Substring(StartDelimiterLength, settingValue.Length - (StartDelimiterLength + EndDelimiterLength));
				settingValue = System.Configuration.ConfigurationSettings.AppSettings[appKeyName];
			}
			return settingValue;
		}
	}
}
