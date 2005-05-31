using System;
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
		/// Returns an instance of this provider loaded from the specified section name.
		/// </summary>
		/// <returns></returns>
		protected static ProviderBase Instance(string sectionName) 
		{
			// Use the cache because the reflection used later is expensive
			Cache cache = HttpRuntime.Cache;
			Type type = null;
			string cacheKey = null;

			ProviderConfiguration config = (ProviderConfiguration)ConfigurationSettings.GetConfig(sectionName);

			// Read the configuration specific information
			// for this provider
			ProviderInfo providerInfo = (ProviderInfo)config.Providers[config.DefaultProvider];

			// In the cache?
			cacheKey = sectionName + "::" + config.DefaultProvider;
			if ( cache[cacheKey] == null ) 
			{
				// The assembly should be in \bin or GAC, so we simply need
				// to get an instance of the type
				try 
				{
					type = Type.GetType( providerInfo.Type );

					// Insert the type into the cache
					// Provider Types must have a default no parameter constructor
					Type[] paramTypes = new Type[0];
					cache.Insert( cacheKey, type.GetConstructor(paramTypes) );
				} 
				catch (Exception e) 
				{
					throw new Exception("Unable to load provider", e);
				}
			}

			ConstructorInfo constructor = cache[cacheKey] as ConstructorInfo;
			ProviderBase provider = (ProviderBase)(constructor.Invoke(new object[] {}));
			provider.Initialize(providerInfo.Name, providerInfo.Attributes);
			return provider;
		}
	}
}
