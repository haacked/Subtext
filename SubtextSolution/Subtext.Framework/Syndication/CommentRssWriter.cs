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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Syndication;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Writes a CommentRSS feed to the underlying stream.  Inherits from 
	/// BaseRssWriter which ultimately inherits from <see cref="XmlTextWriter"/>.
	/// </summary>
	public class CommentRssWriter : BaseRssWriter<FeedbackItem>
	{
		protected Entry CommentEntry = null;

		/// <summary>
		/// Creates a new <see cref="CommentRssWriter"/> instance.
		/// </summary>
		/// <param name="commentEntries">Ec.</param>
		/// <param name="entry">Ce.</param>
		public CommentRssWriter(IList<FeedbackItem> commentEntries, Entry entry)
			: base(NullValue.NullDateTime, false)
		{
			if (commentEntries == null)
			{
				throw new ArgumentNullException("commentEntries", "Cannot generate a comment rss feed for a null collection of entries.");
			}

			if (entry == null)
			{
				throw new ArgumentNullException("entry", "Comment RSS feed must be associated to an entry.");
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
			RssImageElement image = new RssImageElement(GetRssImage(), CommentEntry.Title, CommentEntry.FullyQualifiedUrl, 77, 60, null);
			BuildChannel(CommentEntry.Title, CommentEntry.FullyQualifiedUrl.ToString(), CommentEntry.Author.Email, CommentEntry.HasDescription ? CommentEntry.Description : CommentEntry.Body, info.Language, info.Author, Config.CurrentBlog.LicenseUrl, image);
		}

		/// <summary>
		/// Gets the categories from entry.
		/// </summary>
		/// <param name="item">The entry.</param>
		/// <returns></returns>
		protected override StringCollection GetCategoriesFromItem(FeedbackItem item)
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
			return item.DisplayUrl.ToString();
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
			return Config.CurrentBlog.TimeZone.ToUniversalTime(item.DateCreated);
		}
	}
}