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
using System.Data;
using Subtext.Extensibility;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Components
{
	public abstract class BlogDataProvider
	{
		#region Content
		//Core Data IBlogContent
		//--------------------------------------------------------------

		//Get Content
		public abstract ICollection<EntryDay> GetRecentDayPosts(int itemCount, bool activeOnly);
        public abstract ICollection<EntryDay> GetPostsByMonth(int month, int year);

        public abstract IList<Entry> GetRecentPosts(int itemCount, PostType postType, bool activeOnly);
        public abstract IList<Entry> GetRecentPosts(int itemCount, PostType postType, bool activeOnly, DateTime DateUpdated);

        public abstract ICollection<CategoryEntry> GetRecentPostsWithCategories(int itemCount, bool activeOnly);

		public abstract IList<Entry> GetPostCollectionByMonth(int month, int year);
		public abstract IList<Entry> GetEntriesByDateRangle(DateTime start, DateTime stop, PostType postType, bool activeOnly);

		public abstract IList<Entry> GetFeedBack(int PostID);
		
		public abstract EntryDay GetSingleDay(DateTime dt);
		
		public abstract Entry GetSingleEntry(int postID, bool activeOnly);
		public abstract CategoryEntry GetSingleCategoryEntry(int postID, bool activeOnly);

		//Provide a Raw view of the data for web services. ReadOnly 
		public abstract DataSet RawEntries(int itemCount, PostType postType, bool activeOnly);
		public abstract DataSet RawEntriesByCategoryID(int itemCount, int CategoryID, bool activeOnly);

	
		public abstract BlogInfo GetBlogInfo(string host, string subfolder);
		
		//Update/Add/Remove
		public abstract bool UpdateEntry(Entry _entry);
		public abstract bool UpdateCategoryEntry(CategoryEntry ce);
		public abstract void DeleteEntry(int PostID);
		public abstract int InsertEntry(Entry _entry);
		public abstract int InsertPingTrackEntry(Entry entry);
		public abstract int InsertCategoryEntry(CategoryEntry ce);
		public abstract void UpdateConfigData(BlogInfo info);
		//public abstract void RemoveComment(int CommentID);

		#endregion

		#region Links and Categories 
		
		//Links and Content ...Likely IBlogLinks
		//-------------------------------------------------------------------

		//Content and Links
        public abstract ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int catID);
		
		public abstract IList<Entry> GetEntriesByCategory(int itemCount, int catID, bool activeOnly);
		public abstract IList<Entry> GetEntriesByCategory(int itemCount, int catID, DateTime DateUpdated, bool activeOnly);

		//Just link content
		public abstract ICollection<Link> GetLinksByCategoryID(int catID, bool activeOnly);
	
		public abstract Link GetSingleLink(int linkID);

        public abstract ICollection<Link> GetLinkCategory(int catID);		
		
		public abstract ICollection<LinkCategory> GetActiveCategories();

        public abstract ICollection<LinkCategory> GetCategories(CategoryType catType, bool activeOnly);		
	
		public abstract ICollection<Link> GetLinkCollectionByPostID(int PostID);

        public abstract ICollection<Link> GetPostsByMonthArchive();
		
		public abstract void DeleteLink(int LinkID);
		public abstract void DeleteCategory(int CatID);


		//Update/Add/Remove
		public abstract void AddEntryToCategories(int PostID, IList<Link> lc);
		public abstract void UpdateLink(Link _link);
		public abstract int InsertLink(Link _link);
		public abstract void UpdateCategory(LinkCategory lc);
		public abstract int InsertCategory(LinkCategory lc);

		#endregion

		#region Images

		public abstract ICollection<Image> GetImagesByCategoryID(int catID, bool activeOnly);

		public abstract Image GetSingleImage(int imageID, bool activeOnly);

		public abstract int InsertImage(Image _image);

		public abstract void UpdateImage(Image _image);

		public abstract void DeleteImage(int imageID);

		#endregion

		#region Stats

		public abstract void TrackPage(PageType pageType, int PostID, string Referral);
		public abstract void TrackPages(Referrer[] referrers);

		#endregion

		#region Admin Stuff

		//Need generic PageItem collectin? 

        public abstract IPagedCollection<Link> GetPagedLinks(int CategoryID, int pageIndex, int pageSize, bool sortDescending);
		public abstract IPagedCollection<Entry> GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending);
		public abstract IPagedCollection<LogEntry> GetPagedLogEntries(int pageIndex, int pageSize, bool sortDescending);
        public abstract IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate);
        public abstract IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int EntryID);
        public abstract PagedCollection<Entry> GetPagedEntries(PostType postType, int categoryTypeID, int pageIndex, int pageSize, bool sortDescending);
        public abstract IPagedCollection<ViewStat> GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate);
        public abstract IPagedCollection<KeyWord> GetPagedKeyWords(int pageIndex, int pageSize, bool sortDescending);


		#endregion

		#region KeyWords

		public abstract ICollection<KeyWord> GetKeyWords();
		public abstract KeyWord GetKeyWord(int KeyWordID);
		public abstract void UpdateKeyWord(KeyWord kw);
		public abstract int InsertKeyWord(KeyWord kw);
		public abstract bool DeleteKeyWord(int KeyWordID);

		#endregion


	}
}

