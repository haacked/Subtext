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
using System.IO;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Generates RSS
    /// </summary>
    public class RssWriter : BaseRssWriter<Entry>
    {
        /// <summary>
        /// Creates a new <see cref="RssWriter"/> instance.
        /// </summary>
        public RssWriter(TextWriter writer, ICollection<Entry> entries, DateTime dateLastViewedFeedItemPublished,
                         bool useDeltaEncoding, ISubtextContext context)
            : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
        {
            Items = entries;
            UseAggBugs = true;
        }

        /// <summary>
        /// Gets the categories from entry.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <returns></returns>
        protected override ICollection<string> GetCategoriesFromItem(Entry item)
        {
            return item.Categories;
        }

        /// <summary>
        /// Gets the title from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override string GetTitleFromItem(Entry item)
        {
            return item.Title;
        }

        /// <summary>
        /// Gets the link from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override string GetLinkFromItem(Entry item)
        {
            return UrlHelper.EntryUrl(item).ToFullyQualifiedUrl(Blog).ToString();
        }

        /// <summary>
        /// Obtains the syndication date for the specified entry, since 
        /// we don't necessarily know if the type has that field, we 
        /// can delegate this to the inheriting class.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override DateTime GetSyndicationDate(Entry item)
        {
            return item.DateSyndicated;
        }

        /// <summary>
        /// Gets the body from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override string GetBodyFromItem(Entry item)
        {
            return item.SyndicateDescriptionOnly ? item.Description : item.Body; //use desc or full post
        }

        /// <summary>
        /// Gets the author from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override string GetAuthorFromItem(Entry item)
        {
            return item.Author;
        }

        /// <summary>
        /// Gets the publish date from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override DateTime GetPublishedDateUtc(Entry item)
        {
            return Blog.TimeZone.ToUtc(item.DateSyndicated);
        }

        /// <summary>
        /// Returns true if the Item could contain comments.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override bool ItemCouldContainComments(Entry item)
        {
            return true;
        }

        /// <summary>
        /// Returns true if the item allows comments, otherwise false.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override bool ItemAllowsComments(Entry item)
        {
            return item.AllowComments;
        }

        /// <summary>
        /// Returns true if comments are closed, otherwise false.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override bool CommentsClosedOnItem(Entry item)
        {
            return item.CommentingClosed;
        }

        /// <summary>
        /// Gets the feedback count for the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override int GetFeedbackCount(Entry item)
        {
            return item.FeedBackCount;
        }

        protected override EnclosureItem GetEnclosureFromItem(Entry item)
        {
            if(item.Enclosure != null && item.Enclosure.AddToFeed)
            {
                var enc = new EnclosureItem {Url = item.Enclosure.Url, MimeType = item.Enclosure.MimeType, Size = item.Enclosure.Size};
                return enc;
            }
            return null;
        }
    }
}