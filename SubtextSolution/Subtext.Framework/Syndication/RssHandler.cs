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
using System.Globalization;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Infrastructure;
using System.IO;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Class used to handle requests for an RSS feed.
    /// </summary>
    public class RssHandler : BaseSyndicationHandler
    {
        BaseSyndicationWriter<Entry> _writer;

        public RssHandler(ISubtextContext subtextContext) : base(subtextContext)
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
                    _writer = new RssWriter(new StringWriter(),
                                           Repository.GetMainSyndicationEntries(Blog.ItemCount),
                                           PublishDateOfLastFeedItemReceived, 
                                           UseDeltaEncoding, 
                                           SubtextContext);
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
            const string key = "RSS;IndividualMainFeed;BlogId:{0};LastViewed:{1}";
            return string.Format(CultureInfo.InvariantCulture, key, Blog.Id, dateLastViewedFeedItemPublished);
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
                cache.InsertDuration(CacheKey(SyndicationWriter.DateLastViewedFeedItemPublished), feed,
                                     Cacher.MediumDuration, null);
            }
        }
    }
}