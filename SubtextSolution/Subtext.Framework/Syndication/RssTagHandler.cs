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

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Class used to handle requests for an RSS feed.
    /// </summary>
    public class RssTagHandler : Subtext.Framework.Syndication.BaseSyndicationHandler<Entry>
    {
        BaseSyndicationWriter<Entry> writer;

        /// <summary>
        /// Returns the key used to cache this feed.
        /// </summary>
        /// <param name="dateLastViewedFeedItemPublished">Date last viewed feed item published.</param>
        /// <returns></returns>
        protected override string CacheKey(DateTime dateLastViewedFeedItemPublished)
        {
            const string key = "RSS;IndividualMainFeed;BlogId:{0};LastViewed:{1};Tag:{2}";
            return string.Format(key, Blog.Id, dateLastViewedFeedItemPublished, _getTagName());
        }

        // timheuer - overridden method to bypass the feedburner check
        protected override void ProcessFeed()
        {
            if (base.IsLocalCacheOK())
            {
                base.HttpContext.Response.StatusCode = 304;
                return;
            }

            // Checks our cache against last modified header.
            if (!base.IsHttpCacheOK())
            {
              base.Feed = base.BuildFeed();
                if (base.Feed != null)
                {
                    if (base.UseDeltaEncoding && base.Feed.ClientHasAllFeedItems)
                    {
                        base.HttpContext.Response.StatusCode = 304;
                        return;
                    }
                    Cache(Feed);
                }
            }

            base.WriteFeed();
        }

        // timheuer - this is a slight hack to get the tag name
        private string _getTagName()
        {
            Uri url = base.HttpContext.Request.Url;
            string tagName = System.Web.HttpUtility.UrlDecode(url.Segments[url.Segments.Length - 2].Replace("/", ""));
            return tagName;
        }

        /// <summary>
        /// Caches the specified RSS feed.
        /// </summary>
        /// <param name="feed">Feed.</param>
        protected override void Cache(CachedFeed feed)
        {
            HttpContext.Cache.Insert(CacheKey(this.SyndicationWriter.DateLastViewedFeedItemPublished), feed, null, DateTime.Now.AddSeconds((double)CacheDuration.Medium), TimeSpan.Zero);
        }

        /// <summary>
        /// Gets the syndication writer.
        /// </summary>
        /// <returns></returns>
        protected override BaseSyndicationWriter SyndicationWriter
        {
            get
            {
                if (writer == null)
                {
                    // timheuer: changed this to GetEntriesByTag
                    writer = new RssWriter(HttpContext.Response.Output, Entries.GetEntriesByTag(Blog.ItemCount, _getTagName()), PublishDateOfLastFeedItemReceived, UseDeltaEncoding, SubtextContext);
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
    }
}