using System;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Summary description for IPluginFactory.
	/// </summary>
	public interface IPluginFactory : IPlugin
	{
		IPlugin CreatePluginInstance(ITargetIdentifier targetId, IPluginContext context);
	}
}
