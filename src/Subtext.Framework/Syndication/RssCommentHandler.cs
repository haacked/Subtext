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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Web;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// RssCommentHandler is a proposed extention to the CommentApi. This is still beta/etc.
    /// The Main Rss feed now contains an element for each entry, which will generate a rss feed 
    /// containing the comments for each post.
    /// </summary>
    public class RssCommentHandler : EntryCollectionHandler<FeedbackItem>
    {
        ICollection<FeedbackItem> _comments;
        protected ICollection<FeedbackItem> Comments;
        protected Entry ParentEntry;

        public RssCommentHandler(ISubtextContext subtextContext) : base(subtextContext)
        {
        }

        protected override BaseSyndicationWriter SyndicationWriter
        {
            get { return new CommentRssWriter(HttpContext.Response.Output, _comments, ParentEntry, SubtextContext); }
        }

        /// <summary>
        /// Returns true if the feed is the main feed.  False for category feeds and comment feeds.
        /// </summary>
        protected override bool IsMainfeed
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the feed entries.
        /// </summary>
        /// <returns></returns>
        protected override ICollection<FeedbackItem> GetFeedEntries()
        {
            if(ParentEntry == null)
            {
                ParentEntry = Cacher.GetEntryFromRequest(false, SubtextContext);
            }

            if(ParentEntry == null)
            {
                // bad news... we couldn't find the entry the request is looking for - return 404.
                HttpHelper.SetFileNotFoundResponse();
            }

            if(ParentEntry != null && Comments == null)
            {
                Comments = Cacher.GetFeedback(ParentEntry, SubtextContext);
            }

            return Comments;
        }

        protected virtual CommentRssWriter GetCommentWriter(ICollection<FeedbackItem> comments, Entry entry)
        {
            return new CommentRssWriter(HttpContext.Response.Output, comments, entry, SubtextContext);
        }

        /// <summary>
        /// Builds the feed using delta encoding if it's true.
        /// </summary>
        /// <returns></returns>
        protected override CachedFeed BuildFeed()
        {
            _comments = GetFeedEntries() ?? new List<FeedbackItem>();

            var feed = new CachedFeed();
            CommentRssWriter crw = GetCommentWriter(_comments, ParentEntry);
            feed.LastModified = _comments.Count > 0 ? ConvertLastUpdatedDate(_comments.Last().DateCreated) : ParentEntry.DateCreated;
            feed.Xml = crw.Xml;
            return feed;
        }

        protected override bool IsLocalCacheOk()
        {
            string dt = LastModifiedHeader;
            if(dt != null)
            {
                _comments = GetFeedEntries();

                if(_comments != null && _comments.Count > 0)
                {
                    return
                        DateTime.Compare(DateTime.Parse(dt, CultureInfo.InvariantCulture),
                                         ConvertLastUpdatedDate(_comments.Last().DateCreated)) == 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the item created date.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override DateTime GetItemCreatedDate(FeedbackItem item)
        {
            return item.DateCreated;
        }
    }
}