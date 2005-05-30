using System;
using System.Web;
using System.Web.Caching;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used to filter incoming comments.  This will get replaced 
	/// with a plugin once the plugin architecture is complete, but the 
	/// logic will probably get ported.
	/// </summary>
	public sealed class CommentFilter
	{
		private const string FILTER_CACHE_KEY = "COMMENT FILTER:";
		private CommentFilter() {}

		/// <summary>
		/// Filters the comment. Throws an exception should the comment not be allowed. 
		/// Otherwise returns true.  This interface may be changed.
		/// </summary>
		/// <param name="entry">Entry.</param>
		public static void FilterComment(Entry entry)
		{
			if(!SourceFrequencyIsValid(entry))
				throw new Subtext.Framework.Exceptions.CommentFrequencyException();
		}

		// Returns true if the source of the entry is not 
		// posting too many.
		static bool SourceFrequencyIsValid(Entry entry)
		{
			Cache cache = HttpContext.Current.Cache;
			object lastComment = cache.Get(FILTER_CACHE_KEY + entry.SourceName);
			

			if(lastComment != null)
			{
				//Comment was made too frequently.
				return false;
			}

			//Add to cache.
            cache.Insert(FILTER_CACHE_KEY + entry.SourceName, string.Empty, null, DateTime.Now.AddMinutes(Config.CurrentBlog.CommentDelayInMinutes), TimeSpan.Zero);
			return true;
		}
	}
}
