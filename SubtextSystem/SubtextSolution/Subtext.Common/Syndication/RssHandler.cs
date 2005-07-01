#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using Subtext.Framework;
using Subtext.Framework.Syndication;
using DTCF = Subtext.Framework.Configuration;

namespace Subtext.Common.Syndication
{
	/// <summary>
	/// Class used to handle requests for an RSS feed.
	/// </summary>
	public class RssHandler : Subtext.Framework.Syndication.BaseSyndicationHandler
	{
		BaseSyndicationWriter writer = null;

		/// <summary>
		/// Returns the cache key for the feed.
		/// </summary>
		/// <returns></returns>
		protected override string CacheKey(int lastViewedId)
		{
			const string key = "IndividualMainFeed;BlogID:{0};LastViewed:{1}";
			return string.Format(key, CurrentBlog.BlogID, lastViewedId);
		}

		/// <summary>
		/// Caches the specified RSS feed.
		/// </summary>
		/// <param name="feed">Feed.</param>
		protected override void Cache(CachedFeed feed)
		{
			Context.Cache.Insert(CacheKey(this.SyndicationWriter.LatestFeedItemId), feed, null, DateTime.Now.AddSeconds((double)Subtext.Common.Data.CacheTime.Medium), TimeSpan.Zero);
		}

		/// <summary>
		/// Gets the syndication writer.
		/// </summary>
		/// <returns></returns>
		protected override BaseSyndicationWriter SyndicationWriter
		{
			get
			{
				if(writer == null)
				{
					writer = new RssWriter(Entries.GetMainSyndicationEntries(CurrentBlog.ItemCount), this.LastFeedItemReceived, this.UseDeltaEncoding);
				}
				return writer;
			}
		}
	}
}