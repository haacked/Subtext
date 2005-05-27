using System;
using System.Xml;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Creates an XML element, containg configuration for the log4net repository
	/// </summary>
	public interface IConfigurationFactory
	{
		XmlElement CreateConfiguration(IConfigurationDeclaration configurationDeclaration);
	}
}
