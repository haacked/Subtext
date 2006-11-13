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


namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Interface that must be implemented by all SubText Plugins 
	/// </summary>
	public interface IPlugin
	{
		/// <summary>
		/// Identifier of the plugin. This value has to be unique, so this interface provides a Guid.
		/// </summary>
		Guid Id {get;}

		/// <summary>
		/// Information about plugin implementation
		/// </summary>
		IImplementationInfo Info {get;}
		
		/// <summary>
		/// Initialize the plugin.<br />
		/// This is the only method of the interface since all actions are performed inside Event Handlers<br />
		/// The implementation of this method should only subscribe to the events raised by the STApplication
		/// </summary>
		void Init(SubtextApplication application);
	}
}
