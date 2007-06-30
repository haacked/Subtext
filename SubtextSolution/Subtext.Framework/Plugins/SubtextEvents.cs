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
		public static void OnEntryUpdating(object sender, CancellableEntryEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntryUpdating(sender, e);
		}

		/// <summary>
		/// Raises all EntryUpdated Events.
		/// </summary>
		public static void OnEntryUpdated(object sender, EntryEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntryUpdated(sender, e);
		}

		/// <summary>
		/// Raises all SingleEntryRendering Events.
		/// </summary>
		public static void OnSingleEntryRendering(object sender, EntryEventArgs e)
		{
			SubtextApplication.Current.ExecuteSingleEntryRendering(sender, e);
		}

		/// <summary>
		/// Raises all SingleEntryRendered Events.
		/// </summary>
		public static void OnSingleEntryRendered(object sender, EntryEventArgs e)
		{
			SubtextApplication.Current.ExecuteSingleEntryRendered(sender, e);
		}

		/// <summary>
		/// Raises all EntryRendering Events.
		/// </summary>
		public static void OnEntryRendering(object sender, EntryEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntryRendering(sender, e);
		}

		/// <summary>
		/// Raises all EntryRendered Events.
		/// </summary>
		public static void OnEntryRendered(object sender, EntryEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntryRendered(sender, e);
		}
		
		/// <summary>
		/// Raises all EntrySyndicating Events.
		/// </summary>
		public static void OnEntrySyndicating(object sender, EntryEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntrySyndicating(sender, e);
		}

		/// <summary>
		/// Raises all EntrySyndicated Events.
		/// </summary>
		public static void OnEntrySyndicated(object sender, EntryEventArgs e)
		{
			SubtextApplication.Current.ExecuteEntrySyndicated(sender, e);
		}


		/// <summary>
		/// Raises all CommentUpdating Events.
		/// </summary>
		public static void OnCommentUpdating(object sender, FeedbackEventArgs e)
		{
			SubtextApplication.Current.ExecuteCommentUpdating(sender, e);
		}

		/// <summary>
		/// Raises all CommentUpdated Events.
		/// </summary>
		public static void OnCommentUpdated(object sender, FeedbackEventArgs e)
		{
			SubtextApplication.Current.ExecuteCommentUpdated(sender, e);
		}

		/// <summary>
		/// Raises all CommentRendering Events.
		/// </summary>
		public static void OnCommentRendering(object sender, FeedbackEventArgs e)
		{
			SubtextApplication.Current.ExecuteCommentRendering(sender, e);
		}

		/// <summary>
		/// Raises all CommentRendered Events.
		/// </summary>
		public static void OnCommentRendered(object sender, FeedbackEventArgs e)
		{
			SubtextApplication.Current.ExecuteCommentRendered(sender, e);
		}

		/// <summary>
		/// Raises all ImageUpdating Events.
		/// </summary>
		public static void OnImageUpdating(object sender, ImageEventArgs e)
		{
			SubtextApplication.Current.ExecuteImageUpdating(sender, e);
		}

		/// <summary>
		/// Raises all ImageUpdated Events.
		/// </summary>
		public static void OnImageUpdated(object sender, ImageEventArgs e)
		{
			SubtextApplication.Current.ExecuteImageUpdated(sender, e);
		}

		/// <summary>
		/// Raises all ImageRendering Events.
		/// </summary>
		public static void OnImageRendering(object sender, ImageEventArgs e)
		{
			SubtextApplication.Current.ExecuteImageRendering(sender, e);
		}

		/// <summary>
		/// Raises all ImageRendered Events.
		/// </summary>
		public static void OnImageRendered(object sender, ImageEventArgs e)
		{
			SubtextApplication.Current.ExecuteImageRendered(sender, e);
		}

	}
}
