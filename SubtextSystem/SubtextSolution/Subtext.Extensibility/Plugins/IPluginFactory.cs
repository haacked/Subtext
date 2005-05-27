using System;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Summary description for IPluginFactory.
	/// </summary>
	public interface IPluginFactory : IPlugin
	{
		IPluginIdentifierCollection PluginIdentifiers {get;}
		IPlugin CreatePluginInstance(IPluginIdentifier pluginId, IPluginContext context);
	}
}
