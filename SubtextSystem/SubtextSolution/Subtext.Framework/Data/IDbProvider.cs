using System;
using System.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Summary description for IBlogDataProvider.
	/// </summary>
	public interface IDbProvider
	{

		string ConnectionString
		{
			get;
			set;
		}

		#region Get Blog Data

		IDataReader GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc);
		IDataReader GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc, DateTime DateUpdated);

		IDataReader GetEntriesByDateRangle(DateTime start, DateTime stop, PostType postType, bool ActiveOnly);

		//Maybe under the hood only one call here? 
		//Good Canidate for service/dataset? 
		//Used a lot, maybe it should be both dataset and DataReader?
		IDataReader GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly);
		IDataReader GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated);

		IDataReader GetFeedBack(int PostID);

		IDataReader GetSingleDay(DateTime dt);

		//move other EntryDay Helper
		IDataReader GetPostsByCategoryID(int ItemCount, int catID);
		IDataReader GetRecentDayPosts(int ItemCount, bool ActiveOnly);

		//Should Power both EntryDayCollection and EntryCollection
		IDataReader GetPostCollectionByMonth(int month, int year);
		
		IDataReader GetEntriesByCategory(int ItemCount, string CategoryName, bool ActiveOnly);
		IDataReader GetEntriesByCategory(int ItemCount, string CategoryName, DateTime DateUpdated, bool ActiveOnly);

		IDataReader GetEntriesByCategory(int ItemCount, int catID, bool ActiveOnly);
		IDataReader GetEntriesByCategory(int ItemCount, int catID, DateTime DateUpdated, bool ActiveOnly);
		
		IDataReader GetEntry(int postID, bool ActiveOnly);
		IDataReader GetEntry(string EntryName, bool ActiveOnly);
		IDataReader GetCategoryEntry(int postID, bool ActiveOnly);
		IDataReader GetCategoryEntry(string EntryName, bool ActiveOnly);

		DataSet GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly);
		#endregion

		#region Update Blog Data
		bool DeleteEntry(int EntryID);

		//Should just be Entry and check is CategoryEntry?
		int InsertCategoryEntry(CategoryEntry ce);
		bool UpdateCategoryEntry(CategoryEntry ce);

		int InsertEntry(Entry entry); //change to create?
		bool UpdateEntry(Entry entry);

		int InsertPingTrackEntry(Entry entry); //Create and add check for PostType. 
		#endregion

		#region Links

		IDataReader GetLinkCollectionByPostID(int PostID);

		//use charlist_to_table
		bool AddEntryToCategories(int PostID, LinkCollection lc);

		bool SetEntryCategoryList(int PostID, int[] Categories);

		bool DeleteLink(int LinkID);

		IDataReader GetSingleLink(int linkID);

		int InsertLink(Link link); //Create?

		bool UpdateLink(Link link); 

		IDataReader GetCategories(CategoryType catType, bool ActiveOnly);

		DataSet GetActiveCategories(); //Rename, since it includes LinkCollection as well

		IDataReader GetLinksByCategoryID(int catID, bool ActiveOnly); //Add another method for by name



		#endregion

		#region Categories

		bool DeleteCategory(int CatID);
		IDataReader GetLinkCategory(int catID, bool IsActive);
		IDataReader GetLinkCategory(string categoryName, bool IsActive);

		bool UpdateCategory(LinkCategory lc);

		int InsertCategory(LinkCategory lc);

		#endregion

		#region Config
		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		bool AddInitialBlogConfiguration(string userName, string password);

		IDataReader GetConfig(string host, string application);
		IDataReader GetConfig(int BlogID);

		bool UpdateConfigData(BlogConfig config);

		#endregion

		#region KeyWord
		IDataReader GetKeyWord(int KeyWordID);
		IDataReader GetPagedKeyWords(int pageIndex, int pageSize,bool sortDescending);

		bool DeleteKeyWord(int KeyWordID);

		int InsertKeyWord(KeyWord kw);

		bool UpdateKeyWord(KeyWord kw);

		IDataReader GetKeyWords();

		#endregion

		#region Statistics

		bool TrackEntry(EntryView ev);
		bool TrackEntry(EntryViewCollection evc);

//		bool TrackPages(Referrer[] _feferrers);
//		bool TrackPage(PageType PageType, int PostID, string Referral);

		#endregion

		#region Images

		IDataReader GetImagesByCategoryID(int catID, bool ActiveOnly);
		IDataReader GetSingleImage(int imageID, bool ActiveOnly);

		int InsertImage(Image _image);
		bool UpdateImage(Image _image);
		bool DeleteImage(int imageID);

		#endregion

		#region Admin

		IDataReader GetPagedLinks(int CategoryID, int pageIndex, int pageSize, bool sortDescending);
		IDataReader GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending);
		IDataReader GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending);
		IDataReader GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate);
		IDataReader GetPagedReferrers(int pageIndex, int pageSize);
		IDataReader GetPagedReferrers(int pageIndex, int pageSize, int EntryID);

		#endregion

		#region Archives

		IDataReader GetPostsByMonthArchive();
		IDataReader GetPostsByYearArchive();

		#endregion


	}
}
