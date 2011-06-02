#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Infrastructure;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Class used to handle requests for an RSS feed.
    /// </summary>
    public class RssTagHandler : BaseSyndicationHandler
    {
        BaseSyndicationWriter<Entry> _writer;

        public RssTagHandler(ISubtextContext subtextContext) : base(subtextContext)
        {
        }

        /// <summary>
        /// Gets the syndication writer.
        /// </summary>
        /// <returns></returns>
        protected override BaseSyndicationWriter SyndicationWriter
        {
            get
            {
                if(_writer == null)
                {
                    // timheuer: changed this to GetEntriesByTag
                    _writer = new RssWriter(HttpContext.Response.Output,
                                           Repository.GetEntriesByTag(Blog.ItemCount, GetTagName()),
                                           PublishDateOfLastFeedItemReceived, UseDeltaEncoding, SubtextContext);
                }
                return _writer;
            }
        }

        /// <summary>
        /// Returns true if the feed is the main feed.  False for category feeds and comment feeds.
        /// </summary>
        protected override bool IsMainfeed
        {
            get { return true; }
        }

        /// <summary>
        /// Returns the key used to cache this feed.
        /// </summary>
        /// <param name="dateLastViewedFeedItemPublished">Date last viewed feed item published.</param>
        /// <returns></returns>
        protected override string CacheKey(DateTime dateLastViewedFeedItemPublished)
        {
            const string key = "RSS;IndividualMainFeed;BlogId:{0};LastViewed:{1};Tag:{2}";
            return string.Format(key, Blog.Id, dateLastViewedFeedItemPublished, GetTagName());
        }

        // timheuer - overridden method to bypass the feedburner check
        protected override void ProcessFeed()
        {
            if(base.IsLocalCacheOk())
            {
                HttpContext.Response.StatusCode = 304;
                return;
            }

            // Checks our cache against last modified header.
            if(!base.IsHttpCacheOk())
            {
                Feed = base.BuildFeed();
                if(Feed != null)
                {
                    if(UseDeltaEncoding && Feed.ClientHasAllFeedItems)
                    {
                        HttpContext.Response.StatusCode = 304;
                        return;
                    }
                    Cache(Feed);
                }
            }

            base.WriteFeed();
        }

        // TODO: Make this not a hack. timheuer - this is a slight hack to get the tag name
        private string GetTagName()
        {
            Uri url = HttpContext.Request.Url;
            string tagName = HttpUtility.UrlDecode(url.Segments[url.Segments.Length - 2].Replace("/", ""));
            return tagName;
        }

        /// <summary>
        /// Caches the specified RSS feed.
        /// </summary>
        /// <param name="feed">Feed.</param>
        protected override void Cache(CachedFeed feed)
        {
            ICache cache = SubtextContext.Cache;
            if(cache != null)
            {
                cache.InsertDuration(CacheKey(SyndicationWriter.DateLastViewedFeedItemPublishedUtc), feed,
                                     Cacher.MediumDuration, null);
            }
        }
    }
}