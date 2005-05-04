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
using Subtext.Common.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Syndication;
using DTCF = Subtext.Framework.Configuration;

namespace Subtext.Common.Syndication
{
	/// <summary>
	/// RssCommentHandler is a proposed extention to the CommentApi. This is still beta/etc.
	/// The Main Rss feed now contains an element for each entry, which will generate a rss feed 
	/// containing the comments for each post.
	/// </summary>
	public class RssCommentHandler : EntryCollectionHandler
	{
		public RssCommentHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected Entry ParentEntry = null;
		protected EntryCollection Comments = null;

		protected override EntryCollection GetFeedEntries()
		{
			if(ParentEntry == null)
			{
				ParentEntry = Cacher.GetEntryFromRequest(Context,CacheTime.Short);
			}

			if(ParentEntry != null && Comments == null)
			{
				Comments = Cacher.GetComments(ParentEntry,CacheTime.Short,Context);
			}

			return Comments;
		}


		protected override CachedFeed BuildFeed()
		{
			CachedFeed feed =null;

			EntryCollection comments = GetFeedEntries();

			if(comments != null && comments.Count > 0)
			{
				feed = new CachedFeed();
				CommentRssWriter crw = new CommentRssWriter(comments,ParentEntry);
				feed.LastModified = this.ConvertLastUpdatedDate(comments[comments.Count-1].DateCreated);
				feed.Xml = crw.GetXml;
			}
			return feed;
		}

		protected override bool IsLocalCacheOK()
		{
			string dt = LastModifiedHeader;
			if(dt != null)
			{
				EntryCollection comments = GetFeedEntries();

				if(comments != null && comments.Count > 0)
				{
					return DateTime.Compare(DateTime.Parse(dt),this.ConvertLastUpdatedDate(comments[comments.Count-1].DateCreated)) == 0;
				}
			}
			return false;			
		}

	}
}

