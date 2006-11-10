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

using Subtext.Framework.Components;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Static helper class responsible for executing all events handlers
	/// </summary>
	public static class STEvents
	{
		/// <summary>
		/// Raises all EntryUpdating Events.
		/// </summary>
		public static void OnEntryUpdating(Entry entry, STEventArgs e)
		{
			STApplication.Current.ExecuteEntryUpdating(entry, e);
		}

		/// <summary>
		/// Raises all EntryUpdated Events.
		/// </summary>
		public static void OnEntryUpdated(Entry entry, STEventArgs e)
		{
			STApplication.Current.ExecuteEntryUpdated(entry, e);
		}

		/// <summary>
		/// Raises all SingleEntryRendering Events.
		/// </summary>
		public static void OnSingleEntryRendering(Entry entry, STEventArgs e)
		{
			STApplication.Current.ExecuteSingleEntryRendering(entry, e);
		}

		/// <summary>
		/// Raises all EntryRendering Events.
		/// </summary>
		public static void OnEntryRendering(Entry entry, STEventArgs e)
		{
			STApplication.Current.ExecuteEntryRendering(entry, e);
		}
	}
}
