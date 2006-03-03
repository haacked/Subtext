using System;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace blowery.Web.HttpCompress
{
	/// <summary>
	/// This class acts as a factory for the configuration settings.
	/// </summary>
	public sealed class SectionHandler : IConfigurationSectionHandler
	{
    /// <summary>
    /// Create a new config section handler.  This is of type <see cref="Settings"/>
    /// </summary>
    object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode configSection) {
      Settings settings;
      if(parent == null)
        settings = Settings.Default;
      else
        settings = (Settings)parent;
      settings.AddSettings(configSection);
      return settings;
    }
	}

}
