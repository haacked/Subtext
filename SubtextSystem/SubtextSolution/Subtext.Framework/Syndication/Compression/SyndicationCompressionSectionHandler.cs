using System;
using System.Configuration;

namespace Subtext.Framework.Syndication.Compression
{
	internal class SyndicationCompressionSectionHandler :  IConfigurationSectionHandler	
	{
		object IConfigurationSectionHandler.Create(object parent, object configContext, System.Xml.XmlNode configurationSection) 
		{
			return new SyndicationCompressionSettings(configurationSection);
		}
	}
}
