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
using Subtext.Framework.Data;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Syndication;
using Subtext.Framework.Util;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// RssCommentHandler is a proposed extention to the CommentApi. This is still beta/etc.
	/// The Main Rss feed now contains an element for each entry, which will generate a rss feed 
	/// containing the comments for each post.
	/// </summary>
	public class RssCategoryHandler : EntryCollectionHandler<Entry>
	{
		protected LinkCategory Category;
        IList<Entry> posts;

        protected override IList<Entry> GetFeedEntries()
		{
			if(Category == null)
			{
				Category = Cacher.SingleCategory(CacheDuration.Short);
			}

			if(Category != null && posts == null)
			{
				posts = Cacher.GetEntriesByCategory(10, CacheDuration.Short, Category.Id);
			}

			return posts;
		}


		/// <summary>
		/// Builds the feed using delta encoding if it's true.
		/// </summary>
		/// <returns></returns>
		protected override CachedFeed BuildFeed()
		{
			CachedFeed feed =null;

			posts = GetFeedEntries();

			if(posts != null && posts.Count > 0)
			{
				feed = new CachedFeed();
				CategoryWriter cw = new CategoryWriter(posts, Category,WebPathStripper.RemoveRssSlash(Context.Request.Url.ToString()));
				feed.LastModified = ConvertLastUpdatedDate(posts[0].DateCreated);
				feed.Xml = cw.Xml;
			}
			return feed;
		}

		protected override BaseSyndicationWriter SyndicationWriter
		{
			get
			{
				return new CategoryWriter(posts, Category,WebPathStripper.RemoveRssSlash(Context.Request.Url.ToString()));
			}
		}

		/// <summary>
		/// Returns true if the feed is the main feed.  False for category feeds and comment feeds.
		/// </summary>
		protected override bool IsMainfeed
		{
			get { return false; }
		}


		/// <summary>
		/// Gets the item created date.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		protected override DateTime GetItemCreatedDate(Entry item)
		{
			return item.DateCreated;
		}
	}
}

