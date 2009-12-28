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
using System.Xml;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Writes a CommentRSS feed to the underlying stream.  Inherits from 
    /// BaseRssWriter which ultimately inherits from <see cref="XmlTextWriter"/>.
    /// </summary>
    public class CommentRssWriter : BaseRssWriter<FeedbackItem>
    {
        protected Entry CommentEntry;

        /// <summary>
        /// Creates a new <see cref="CommentRssWriter"/> instance.
        /// </summary>
        public CommentRssWriter(TextWriter writer, ICollection<FeedbackItem> commentEntries, Entry entry,
                                ISubtextContext context) : base(writer, NullValue.NullDateTime, false, context)
        {
            if(commentEntries == null)
            {
                throw new ArgumentNullException("commentEntries");
            }

            if(entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            Items = commentEntries;
            CommentEntry = entry;
            UseAggBugs = false;
            AllowComments = false;
        }

        /// <summary>
        /// Writes the RSS channel to the underlying stream.
        /// </summary>
        protected override void WriteChannel()
        {
            Uri entryUrl = UrlHelper.EntryUrl(CommentEntry).ToFullyQualifiedUrl(Blog);
            var image = new RssImageElement(GetRssImage(), CommentEntry.Title, entryUrl, 77, 60, null);

            BuildChannel(CommentEntry.Title,
                         entryUrl,
                         CommentEntry.Email,
                         CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body,
                         Blog.Language,
                         Blog.Author,
                         Blog.LicenseUrl,
                         image);
        }

        /// <summary>
        /// Gets the categories from entry.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <returns></returns>
        protected override ICollection<string> GetCategoriesFromItem(FeedbackItem item)
        {
            return null;
        }

        /// <summary>
        /// Gets the title from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override string GetTitleFromItem(FeedbackItem item)
        {
            return item.Title;
        }

        /// <summary>
        /// Gets the link from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override string GetLinkFromItem(FeedbackItem item)
        {
            return UrlHelper.FeedbackUrl(item).ToFullyQualifiedUrl(Blog).ToString();
        }

        /// <summary>
        /// Gets the body from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override string GetBodyFromItem(FeedbackItem item)
        {
            return item.Body;
        }

        /// <summary>
        /// Gets the author from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override string GetAuthorFromItem(FeedbackItem item)
        {
            return item.Author;
        }

        /// <summary>
        /// Returns true if the Item could contain comments.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override bool ItemCouldContainComments(FeedbackItem item)
        {
            return false;
        }

        /// <summary>
        /// Returns true if the item allows comments, otherwise false.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override bool ItemAllowsComments(FeedbackItem item)
        {
            return false;
        }

        /// <summary>
        /// Returns true if comments are closed, otherwise false.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override bool CommentsClosedOnItem(FeedbackItem item)
        {
            return true;
        }

        /// <summary>
        /// Gets the feedback count for the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override int GetFeedbackCount(FeedbackItem item)
        {
            return 0;
        }

        /// <summary>
        /// Obtains the syndication date for the specified entry, since 
        /// we don't necessarily know if the type has that field, we 
        /// can delegate this to the inheriting class.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override DateTime GetSyndicationDate(FeedbackItem item)
        {
            return item.DateCreated;
        }

        /// <summary>
        /// Gets the publish date from item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected override DateTime GetPublishedDateUtc(FeedbackItem item)
        {
            return Blog.TimeZone.ToUtc(item.DateCreated);
        }

        protected override EnclosureItem GetEnclosureFromItem(FeedbackItem item)
        {
            return null;
        }
    }
}