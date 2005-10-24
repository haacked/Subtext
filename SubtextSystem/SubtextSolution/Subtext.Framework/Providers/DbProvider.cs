using System;
using System.Data;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// If the <see cref="DatabaseObjectProvider"/> is used to supply data objects to 
	/// Subtext, then this provider is used to configure the underlying database 
	/// used. One example of a class that implements this provider is the <see cref="SqlDataProvider"/>.
	/// </summary>
	public abstract class DbProvider : ProviderBase
	{
		string _name;

		/// <summary>
		/// Returns the configured concrete instance of a <see cref="DbProvider"/>.
		/// </summary>
		/// <returns></returns>
		public static DbProvider Instance()
		{
			return (DbProvider)ProviderBase.Instance("Database");
		}

		/// <summary>
		/// Initializes this provider, setting the connection string.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection configValue)
		{
			_name = name;
			_connectionString = GetSettingValue("connectionString", configValue);
		}

		/// <summary>
		/// Returns the friendly name of the provider when the provider is initialized.
		/// </summary>
		/// <value></value>
		public override string Name
		{
			get
			{
				return _name;
			}
		}

		private string _connectionString;
		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value></value>
		public string ConnectionString
		{
			get {return this._connectionString;}
			set {this._connectionString = value;}
		}

		#region DbProvider specific methods
		#region Host Data
		/// <summary>
		/// Returns the data for the Host.
		/// </summary>
		public abstract IDataReader GetHost();

		/// <summary>
		/// Updates the <see cref="HostInfo"/> instance.  If the host record is not in the 
		/// database, one is created. There should only be one host record.
		/// </summary>
		/// <param name="host">The host information.</param>
		public abstract bool UpdateHost(HostInfo host);
		#endregion Host Data

		#region Get Blog Data
		/// <summary>
		/// Returns the specified number of blog entries
		/// </summary>
		/// <param name="ItemCount"></param>
		/// <param name="pt"></param>
		/// <param name="pc"></param>
		/// <returns></returns>
		public abstract IDataReader GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc);
		public abstract IDataReader GetEntriesByDateRangle(DateTime start, DateTime stop, PostType postType, bool ActiveOnly);

		//Maybe under the hood only one call here? 
		//Good Canidate for service/dataset? 
		//Used a lot, maybe it should be both dataset and DataReader?
		public abstract IDataReader GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly);
		public abstract IDataReader GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated);

		public abstract IDataReader GetFeedBack(int PostID);

		public abstract IDataReader GetSingleDay(DateTime dt);

		//move other EntryDay Helper
		public abstract IDataReader GetPostsByCategoryID(int ItemCount, int catID);
		public abstract IDataReader GetRecentDayPosts(int ItemCount, bool ActiveOnly);

		//Should Power both EntryDayCollection and EntryCollection
		public abstract IDataReader GetPostCollectionByMonth(int month, int year);
		
		public abstract IDataReader GetEntriesByCategory(int ItemCount, string CategoryName, bool ActiveOnly);
		public abstract IDataReader GetEntriesByCategory(int ItemCount, string CategoryName, DateTime DateUpdated, bool ActiveOnly);

		public abstract IDataReader GetEntriesByCategory(int ItemCount, int catID, bool ActiveOnly);
		public abstract IDataReader GetEntriesByCategory(int ItemCount, int catID, DateTime DateUpdated, bool ActiveOnly);
		
		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public abstract IDataReader GetCommentByChecksumHash(string checksumHash);
		public abstract IDataReader GetEntry(int postID, bool ActiveOnly);
		public abstract IDataReader GetEntry(string EntryName, bool ActiveOnly);
		public abstract IDataReader GetCategoryEntry(int postID, bool ActiveOnly);
		public abstract IDataReader GetCategoryEntry(string EntryName, bool ActiveOnly);

		public abstract DataSet GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly);
		#endregion

		#region Update Blog Data
		public abstract bool DeleteEntry(int EntryID);

		//Should just be Entry and check is CategoryEntry?
		public abstract int InsertCategoryEntry(CategoryEntry ce);
		public abstract bool UpdateCategoryEntry(CategoryEntry ce);

		public abstract int InsertEntry(Entry entry); //change to create?
		public abstract bool UpdateEntry(Entry entry);

		public abstract int InsertPingTrackEntry(Entry entry); //Create and add check for PostType. 
		#endregion

		#region Links

		public abstract IDataReader GetLinkCollectionByPostID(int PostID);

		//use charlist_to_table
		public abstract bool AddEntryToCategories(int PostID, LinkCollection lc);

		public abstract bool SetEntryCategoryList(int PostID, int[] Categories);

		public abstract bool DeleteLink(int LinkID);

		public abstract IDataReader GetSingleLink(int linkID);

		public abstract int InsertLink(Link link); //Create?

		public abstract bool UpdateLink(Link link); 

		public abstract IDataReader GetCategories(CategoryType catType, bool ActiveOnly);

		public abstract DataSet GetActiveCategories(); //Rename, since it includes LinkCollection as well

		public abstract IDataReader GetLinksByCategoryID(int catID, bool ActiveOnly); //Add another method for by name



		#endregion

		#region Categories

		public abstract bool DeleteCategory(int CatID);
		public abstract IDataReader GetLinkCategory(int catID, bool IsActive);
		public abstract IDataReader GetLinkCategory(string categoryName, bool IsActive);

		public abstract bool UpdateCategory(LinkCategory lc);

		public abstract int InsertCategory(LinkCategory lc);

		#endregion

		#region Config
		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="host"></param>
		/// <param name="application"></param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public abstract bool AddBlogConfiguration(string title, string userName, string password, string host, string application);

		/// <summary>
		/// Returns a <see cref="IDataReader"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <param name="host">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public abstract IDataReader GetBlogInfo(string host, string application);
		
		/// <summary>
		/// Returns a <see cref="IDataReader"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <remarks>
		/// Until Subtext supports multiple blogs again (if ever), 
		/// this will always return the same instance.
		/// </remarks>
		/// <param name="host">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the application and hostname match.</param>
		/// <returns></returns>
		public abstract IDataReader GetBlogInfo(string host, string application, bool strict);
		
		/// <summary>
		/// Returns a <see cref="IDataReader"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// blog id.
		/// </summary>
		/// <param name="blogId">The unique identifier for the blog to retrieve.</param>
		/// <returns></returns>
		public abstract IDataReader GetBlogInfo(int blogId);

		public abstract bool UpdateBlog(BlogInfo info);

		#endregion

		#region KeyWord
		public abstract IDataReader GetKeyWord(int KeyWordID);
		public abstract IDataReader GetPagedKeyWords(int pageIndex, int pageSize,bool sortDescending);

		public abstract bool DeleteKeyWord(int KeyWordID);

		public abstract int InsertKeyWord(KeyWord kw);

		public abstract bool UpdateKeyWord(KeyWord kw);

		public abstract IDataReader GetKeyWords();

		#endregion

		#region Statistics

		public abstract bool TrackEntry(EntryView ev);
		public abstract bool TrackEntry(EntryViewCollection evc);

		//		bool TrackPages(Referrer[] _feferrers);
		//		bool TrackPage(PageType PageType, int PostID, string Referral);

		#endregion

		#region Images

		public abstract IDataReader GetImagesByCategoryID(int catID, bool ActiveOnly);
		public abstract IDataReader GetSingleImage(int imageID, bool ActiveOnly);

		public abstract int InsertImage(Image _image);
		public abstract bool UpdateImage(Image _image);
		public abstract bool DeleteImage(int imageID);

		#endregion

		#region Admin

		/// <summary>
		/// Returns a list of all the blogs within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <returns></returns>
		public abstract IDataReader GetPagedBlogs(int pageIndex, int pageSize, bool sortDescending);
		
		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public abstract IDataReader GetBlogById(int blogId);
		
		/// <summary>
		/// Returns an instance of <see cref="IDataReader"/> used to 
		/// iterate through a result set containing blog_config rows 
		/// with the specified host.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
		public abstract IDataReader GetBlogsByHost(string host);
		
		public abstract IDataReader GetPagedLinks(int CategoryID, int pageIndex, int pageSize, bool sortDescending);
		public abstract IDataReader GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending);
		public abstract IDataReader GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending);
		
		/// <summary>
		/// Gets the specified page of log entries.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDirection">The sort direction.</param>
		/// <returns></returns>
		public abstract IDataReader GetPagedLogEntries(int pageIndex, int pageSize, SortDirection sortDirection);
		public abstract IDataReader GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate);
		public abstract IDataReader GetPagedReferrers(int pageIndex, int pageSize);
		public abstract IDataReader GetPagedReferrers(int pageIndex, int pageSize, int EntryID);

		#endregion

		#region Archives

		public abstract IDataReader GetPostsByMonthArchive();
		public abstract IDataReader GetPostsByYearArchive();

		#endregion
		#endregion
	}
}
 