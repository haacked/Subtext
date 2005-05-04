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
using System.Data;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Components
{
	public abstract class BlogDataProvider
	{
		#region Content
		//Core Data IBlogContent
		//--------------------------------------------------------------

		//Get Content
		public abstract EntryDayCollection GetRecentDayPosts(int ItemCount, bool ActiveOnly);
		public abstract EntryDayCollection GetPostsByMonth(int month, int year);

		public abstract EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly);
		public abstract EntryCollection GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated);

		public abstract	EntryCollection GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly);

//		public abstract EntryCollection GetAllStoreis(bool ActiveOnly);
//		public abstract EntryCollection GetAllStoreis(bool ActiveOnly, DateTime UpdatedSince);

		public abstract EntryCollection GetPostCollectionByMonth(int month, int year);
		public abstract EntryCollection GetEntriesByDateRangle(DateTime start, DateTime stop, PostType postType, bool ActiveOnly);

		public abstract EntryCollection GetFeedBack(int PostID);
		
		public abstract EntryDay GetSingleDay(DateTime dt);
		
		public abstract Entry GetSingleEntry(int postID, bool ActiveOnly);
		public abstract CategoryEntry GetSingleCategoryEntry(int postID, bool ActiveOnly);

		//Provide a Raw view of the data for web services. ReadOnly 
		public abstract DataSet RawEntries(int ItemCount, PostType postType, bool ActiveOnly);
		public abstract DataSet RawEntriesByCategoryID(int ItemCount, int CategoryID, bool ActiveOnly);

	
		public abstract BlogConfig GetConfig(string host, string application);
		
		//Update/Add/Remove
		public abstract bool UpdateEntry(Entry _entry);
		public abstract bool UpdateCategoryEntry(CategoryEntry ce);
		public abstract void DeleteEntry(int PostID);
		public abstract int InsertEntry(Entry _entry);
		public abstract int InsertPingTrackEntry(Entry entry);
		public abstract int InsertCategoryEntry(CategoryEntry ce);
		public abstract void UpdateConfigData(BlogConfig config);
		//public abstract void RemoveComment(int CommentID);

		#endregion

		#region Links and Categories 
		
		//Links and Content ...Likely IBlogLinks
		//-------------------------------------------------------------------

		//Content and Links
		public abstract EntryDayCollection GetPostsByCategoryID(int ItemCount, int catID);
		
		public abstract EntryCollection GetEntriesByCategory(int ItemCount, int catID, bool ActiveOnly);
		public abstract EntryCollection GetEntriesByCategory(int ItemCount, int catID, DateTime DateUpdated, bool ActiveOnly);

		//Just link content
		public abstract LinkCollection GetLinksByCategoryID(int catID, bool ActiveOnly);

		//LinkCategoryCollection GetAllCategories(CategoryType catType);
		
		public abstract Link GetSingleLink(int linkID);		
		
		public abstract LinkCategory GetLinkCategory(int catID);		
		
		public abstract LinkCategoryCollection GetActiveCategories();
		
		public abstract LinkCategoryCollection GetCategories(CategoryType catType, bool ActiveOnly);		
	
		public abstract LinkCollection GetLinkCollectionByPostID(int PostID); 

		public abstract LinkCategory GetPostsByMonthArchive();
		
		public abstract void DeleteLink(int LinkID);
		public abstract void DeleteCategory(int CatID);


		//Update/Add/Remove
		public abstract void AddEntryToCategories(int PostID, LinkCollection lc);
		public abstract void UpdateLink(Link _link);
		public abstract int InsertLink(Link _link);
		public abstract void UpdateCategory(LinkCategory lc);
		public abstract int InsertCategory(LinkCategory lc);

		#endregion

		#region Images

		public abstract ImageCollection GetImagesByCategoryID(int catID, bool ActiveOnly);

		public abstract Image GetSingleImage(int imageID, bool ActiveOnly);

		public abstract int InsertImage(Image _image);

		public abstract void UpdateImage(Image _image);

		public abstract void DeleteImage(int imageID);

		#endregion

		#region Stats

		public abstract void TrackPage(PageType PageType ,int PostID, string Referral);
		public abstract void TrackPages(Referrer[] _feferrers);

		#endregion

		#region Admin Stuff

		//Need generic PageItem collectin? 

		public abstract PagedLinkCollection GetPagedLinks(int CategoryID, int pageIndex, int pageSize, bool sortDescending);
		public abstract PagedEntryCollection GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending);
		public abstract PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate);
		public abstract PagedReferrerCollection GetPagedReferrers(int pageIndex, int pageSize, int EntryID);
		public abstract PagedEntryCollection GetPagedEntries(PostType postType, int categoryTypeID, int pageIndex, int pageSize, bool sortDescending);
		public abstract PagedViewStatCollection GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate);
		public abstract PagedKeyWordCollection GetPagedKeyWords(int pageIndex, int pageSize,bool sortDescending);


		#endregion

		#region KeyWords

		public abstract KeyWordCollection GetKeyWords();
		public abstract KeyWord GetKeyWord(int KeyWordID);
		public abstract void UpdateKeyWord(KeyWord kw);
		public abstract int InsertKeyWord(KeyWord kw);
		public abstract bool DeleteKeyWord(int KeyWordID);

		#endregion


	}
}

