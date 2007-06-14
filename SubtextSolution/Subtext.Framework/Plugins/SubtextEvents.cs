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
	/// Static helper class responsible for executing all events handlers
	/// </summary>
	public static class SubtextEvents
	{
		/// <summary>
		/// Raises all EntryUpdating Events.
		/// </summary>
		public static void OnEntryUpdating(object sender, SubtextEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntryUpdating(sender, e);
		}

		/// <summary>
		/// Raises all EntryUpdated Events.
		/// </summary>
		public static void OnEntryUpdated(object sender, SubtextEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntryUpdated(sender, e);
		}

		/// <summary>
		/// Raises all SingleEntryRendering Events.
		/// </summary>
		public static void OnSingleEntryRendering(object sender, SubtextEventArgs e)
		{
			SubtextApplication.Current.ExecuteSingleEntryRendering(sender, e);
		}

		/// <summary>
		/// Raises all EntryRendering Events.
		/// </summary>
		public static void OnEntryRendering(object sender, SubtextEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntryRendering(sender, e);
		}
		
		/// <summary>
		/// Raises all EntrySyndicating Events.
		/// </summary>
		public static void OnEntrySyndicating(object sender, SubtextEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntrySyndicating(sender, e);
		}
	}
}
