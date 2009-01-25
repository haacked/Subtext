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
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Syndication;
using DTCF = Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Summary description for RssHandler.
	/// </summary>
	public class AtomHandler : BaseSyndicationHandler<Entry>
	{
		BaseSyndicationWriter<Entry> writer;

		/// <summary>
		/// Returns the key used to cache this feed.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished">Date last viewed feed item published.</param>
		/// <returns></returns>
		protected override string CacheKey(DateTime dateLastViewedFeedItemPublished)
		{
			const string key = "ATOM;IndividualMainFeed;BlogId:{0};LastViewed:{1}";
			return string.Format(key, Blog.Id, dateLastViewedFeedItemPublished);
		}

		/// <summary>
		/// Gets the syndication writer.
		/// </summary>
		/// <returns></returns>
		protected override BaseSyndicationWriter SyndicationWriter
		{
			get
			{
				if(writer == null) {
					writer = new AtomWriter(SubtextContext.RequestContext.HttpContext.Response.Output, Entries.GetMainSyndicationEntries(Blog.ItemCount), this.PublishDateOfLastFeedItemReceived, this.UseDeltaEncoding, SubtextContext);
				}
				return writer;
			}
		}

		/// <summary>
		/// Returns true if the feed is the main feed.  False for category feeds and comment feeds.
		/// </summary>
		protected override bool IsMainfeed
		{
			get { return true; }
		}

		protected override void Cache(CachedFeed feed)
		{
            if (HttpContext.Cache != null) {
                HttpContext.Cache.Insert(CacheKey(this.SyndicationWriter.DateLastViewedFeedItemPublished), feed, null, DateTime.Now.AddSeconds((double)CacheDuration.Medium), TimeSpan.Zero);
            }
		}
	}
}

