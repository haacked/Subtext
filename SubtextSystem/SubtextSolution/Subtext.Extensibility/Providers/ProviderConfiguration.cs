using System;
using System.Collections;
using System.Xml;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Contains information about the providers specified in the configuration file.
	/// </summary>
	public class ProviderConfiguration
	{
		string _defaultProvider;
		Hashtable _providers = new Hashtable();

		/// <summary>
		/// Loads information about configured providers from the 
		/// Configuration Section.
		/// </summary>
		/// <param name="node">Node.</param>
		public virtual void LoadValuesFromConfigurationXml(XmlNode node)
		{
			// Get the default provider
			_defaultProvider = node.Attributes["defaultProvider"].Value;

			// Read child nodes
			foreach (XmlNode child in node.ChildNodes) 
			{
				if (child.Name == "providers")
					GetProviders(child);
			}

			if((_defaultProvider == null || _defaultProvider.Length == 0) && _providers.Count > 0)
			{
				_defaultProvider = ((ProviderInfo)_providers[0]).Name;
			}
		}

		void GetProviders(XmlNode node) 
		{
			foreach (XmlNode provider in node.ChildNodes) 
			{
				switch (provider.Name) 
				{
					case "add" :
						_providers.Add(provider.Attributes["name"].Value, new ProviderInfo(provider.Attributes) );
						break;

					case "remove" :
						_providers.Remove(provider.Attributes["name"].Value);
						break;

					case "clear" :
						_providers.Clear();
						break;
				}
			}
		}

		/// <summary>
		/// Gets the default provider.
		/// </summary>
		/// <value></value>
		public string DefaultProvider
		{
			get { return _defaultProvider; }
		}

		/// <summary>
		/// Returns a collection of <see cref="ProviderInfo"/> instances configured 
		/// within the config file.
		/// </summary>
		/// <value></value>
		public Hashtable Providers
		{
			get
			{
				return _providers;
			}
		}
	}
}
