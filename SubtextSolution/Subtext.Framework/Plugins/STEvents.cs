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
		/// Raises all PreEntryUpdate Events.
		/// </summary>
		public static void OnPreEntryUpdate(Entry entry, STEventArgs e)
		{
			STApplication.Current.ExecutePreEntryUpdate(entry, e);
		}

		/// <summary>
		/// Raises all PostEntryUpdate Events.
		/// </summary>
		public static void OnPostEntryUpdate(Entry entry, STEventArgs e)
		{
			STApplication.Current.ExecutePostEntryUpdate(entry, e);
		}

		/// <summary>
		/// Raises all PreRenderSingleEntry Events.
		/// </summary>
		public static void OnPreRenderSingleEntry(Entry entry, STEventArgs e)
		{
			STApplication.Current.ExecutePreRenderSingleEntry(entry, e);
		}

		/// <summary>
		/// Raises all PreRenderEntry Events.
		/// </summary>
		public static void OnPreRenderEntry(Entry entry, STEventArgs e)
		{
			STApplication.Current.ExecutePreRenderEntry(entry, e);
		}
	}
}
