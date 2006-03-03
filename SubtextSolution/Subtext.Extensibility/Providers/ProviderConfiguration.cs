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
using System.Xml;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Contains information about the providers specified in the configuration file.
	/// </summary>
	public class ProviderConfiguration
	{
		ProviderCollection _providers = new ProviderCollection();

		/// <summary>
		/// Loads information about configured providers from the 
		/// Configuration Section.
		/// </summary>
		/// <param name="node">Node.</param>
		public virtual void LoadValuesFromConfigurationXml(XmlNode node)
		{
			string defaultProviderName = null;
			// Get the default provider
			if(node.Attributes["defaultProvider"] != null)
			{
				defaultProviderName = node.Attributes["defaultProvider"].Value;
			}

			// Read child nodes
			foreach (XmlNode child in node.ChildNodes) 
			{
				if (child.Name == "providers")
					GetProviders(child);
			}

			if((defaultProviderName == null || defaultProviderName.Length == 0) && _providers.Count > 0)
			{
				defaultProviderName = _providers[0].Name;
			}
			_providers.DefaultProvider = _providers[defaultProviderName];
		}

		void GetProviders(XmlNode node) 
		{
			foreach (XmlNode provider in node.ChildNodes) 
			{
				switch (provider.Name) 
				{
					case "add" :
						_providers.Add(new ProviderInfo(provider.Attributes) );
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
		/// Returns a collection of <see cref="ProviderInfo"/> instances configured 
		/// within the config file.
		/// </summary>
		/// <value></value>
		public ProviderCollection Providers
		{
			get
			{
				return _providers;
			}
		}
	}
}
