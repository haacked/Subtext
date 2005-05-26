using System;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// This is the starting point for the plug-in architecture.  
	/// Someone want to run with this?
	/// </summary>
	public interface IPlugin
	{
		IImplementationInfo Info {get;}
	}
}
