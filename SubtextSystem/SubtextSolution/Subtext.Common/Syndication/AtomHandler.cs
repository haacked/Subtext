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
using Subtext.Framework;
using Subtext.Framework.Syndication;
using DTCF = Subtext.Framework.Configuration;

namespace Subtext.Common.Syndication
{
	/// <summary>
	/// Summary description for RssHandler.
	/// </summary>
	public class AtomHandler : Subtext.Framework.Syndication.BaseSyndicationHandler
	{
		public AtomHandler(){}

		protected override string CacheKey()
		{
			const string key = "IndividualAtomFeed:BlogID{0}";
			return string.Format(key,CurrentBlog.BlogID);
		}

		protected override CachedFeed BuildFeed()
		{
			CachedFeed feed = new CachedFeed();
			feed.LastModified = this.ConvertLastUpdatedDate(CurrentBlog.LastUpdated);
			AtomWriter writer = new AtomWriter(Entries.GetMainSyndicationEntries(CurrentBlog.ItemCount));
			feed.Xml = writer.GetXml;
			return feed;
		}

		protected override void Cache(CachedFeed feed)
		{
			Context.Cache.Insert(CacheKey(),feed,null,DateTime.Now.AddSeconds((double)Subtext.Common.Data.CacheTime.Medium),TimeSpan.Zero);
		}





	}
}

