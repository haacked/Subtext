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
		/// Returns an instance of this provider loaded from the specified section name.
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
			
			// It's possible that at this point, another thread has already 
			// added the correct provider to this hashtable.  That we might be 
			// overwriting that instance is not a big problem as a provider doesn't 
			// have changing state and this one will be essentially a copy of the 
			// existing one.  Also, it's a rare thing to occur.  So we don't lock 
			// for maximum throughput.
			_providers[sectionName] = provider;
			return provider;
		}
	}
}
