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

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Interface that must be implemented by Subtext controls that raise events that can notify
	/// a message to the user interface:
	/// OnEntryUpdating
	/// OnEntryUpdated
	/// </summary>
	public interface INotifiableControl
	{
		/// <summary>
		/// Show a message on the notification area of the current web page.
		/// The message will be perceived by the user as an error
		/// </summary>
		/// <param name="message">Text message specifing the error</param>
		void ShowError(string message);

		/// <summary>
		/// Show a message on the notification area of the current web page.
		/// The message will be perceived by the user as an informative message
		/// </summary>
		/// <param name="message">Text message</param>
		void ShowMessage(string message);
	}
}
