using System;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// This is the starting point for the plug-in architecture.  
	/// Someone want to run with this?
	/// </summary>
	public interface IPlugin
	{
		/// <summary>
		/// Identifier of the plugin. This value has to be unique. For instance, full type name may be used.
		/// </summary>
		IPluginIdentifier Id {get;}

		/// <summary>
		/// All targets for which this implementation is intended
		/// </summary>
		ITargetIdentifierCollection Targets {get;}

		/// <summary>
		/// Information about plugin implementation
		/// </summary>
		IImplementationInfo Info {get;}
	}
}
