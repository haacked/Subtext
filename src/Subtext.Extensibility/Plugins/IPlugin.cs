#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

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
