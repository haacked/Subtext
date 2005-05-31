using System;
using System.Configuration;
using System.Xml;

namespace Subtext.Extensibility.Providers
{
	/// <summary>
	/// Summary description for ProviderSectionHandler.
	/// </summary>
	public abstract class ProviderSectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Creates a configuration section from the specified node.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="context">Context.</param>
		/// <param name="node">Node.</param>
		/// <returns></returns>
		public virtual object Create(object parent, object context, XmlNode node)
		{
			ProviderConfiguration config = CreateProviderConfigurationInstance();
			config.LoadValuesFromConfigurationXml(node);
			return config;
		}

		/// <summary>
		/// Creates the provider configuration instance.
		/// </summary>
		/// <returns></returns>
		public abstract ProviderConfiguration CreateProviderConfigurationInstance(); 
	}
}
