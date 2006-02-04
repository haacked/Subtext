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
