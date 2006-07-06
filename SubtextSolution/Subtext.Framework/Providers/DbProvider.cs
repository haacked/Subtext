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
		/// <param name="itemCount"></param>
		/// <param name="postType"></param>
		/// <param name="postConfiguration"></param>
		/// <returns></returns>
		public abstract IDataReader GetConditionalEntries(int itemCount, PostType postType, PostConfig postConfiguration);
		public abstract IDataReader GetEntriesByDateRange(DateTime start, DateTime stop, PostType postType, bool activeOnly);

		//Maybe under the hood only one call here? 
		//Good Canidate for service/dataset? 
		//Used a lot, maybe it should be both dataset and DataReader?
		public abstract IDataReader GetRecentPosts(int itemCount, PostType postType, bool activeOnly);

		public abstract IDataReader GetFeedBack(int postId);

		public abstract IDataReader GetSingleDay(DateTime dt);

		//move other EntryDay Helper
		public abstract IDataReader GetPostsByCategoryID(int itemCount, int catID);
		public abstract IDataReader GetRecentDayPosts(int itemCount, bool activeOnly);

		//Should Power both List<EntryDay> and EntryCollection
		public abstract IDataReader GetPostCollectionByMonth(int month, int year);
		
		public abstract IDataReader GetEntriesByCategory(int itemCount, int catID, bool activeOnly);
		
		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public abstract IDataReader GetCommentByChecksumHash(string checksumHash);
		public abstract IDataReader GetEntry(int postID, bool activeOnly);
		public abstract IDataReader GetEntry(string entryName, bool activeOnly);
		public abstract IDataReader GetCategoryEntry(int postID, bool activeOnly);

		public abstract DataSet GetRecentPostsWithCategories(int itemCount, bool activeOnly);
		#endregion

		#region Update Blog Data
		public abstract bool DeleteEntry(int entryID);

		//Should just be Entry and check is CategoryEntry?
		public abstract int InsertCategoryEntry(CategoryEntry ce);
		public abstract bool UpdateCategoryEntry(CategoryEntry ce);

		public abstract int InsertEntry(Entry entry); //change to create?
		public abstract bool UpdateEntry(Entry entry);

		public abstract int InsertPingTrackEntry(Entry entry); //Create and add check for PostType. 
		#endregion

		#region Links

		public abstract IDataReader GetLinkCollectionByPostID(int postId);

		public abstract bool SetEntryCategoryList(int postID, int[] categoryIds);

		public abstract bool DeleteLink(int linkId);

		public abstract IDataReader GetSingleLink(int linkID);

		public abstract int InsertLink(Link link); //Create?

		public abstract bool UpdateLink(Link link); 

		public abstract IDataReader GetCategories(CategoryType catType, bool activeOnly);

		public abstract DataSet GetActiveCategories(); //Rename, since it includes LinkCollection as well

		public abstract IDataReader GetLinksByCategoryID(int catID, bool activeOnly); //Add another method for by name



		#endregion

		#region Categories

		public abstract bool DeleteCategory(int catId);
		public abstract IDataReader GetLinkCategory(int catID, bool isActive);
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
		/// <param name="subfolder"></param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public abstract bool AddBlogConfiguration(string title, string userName, string password, string host, string subfolder);

		/// <summary>
		/// Returns a <see cref="IDataReader"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <param name="host">Subfolder Name.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		/// <returns></returns>
		public abstract IDataReader GetBlogInfo(string host, string subfolder);
		
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
		/// <param name="subfolder">Subfolder Name.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the subfolder and hostname match.</param>
		/// <returns></returns>
		public abstract IDataReader GetBlogInfo(string host, string subfolder, bool strict);
		
		public abstract bool UpdateBlog(BlogInfo info);

		#endregion

		#region KeyWord
		public abstract IDataReader GetKeyWord(int keyWordID);
		public abstract IDataReader GetPagedKeyWords(int pageIndex, int pageSize,bool sortDescending);

		public abstract bool DeleteKeyWord(int keywordId);

		public abstract int InsertKeyWord(KeyWord kw);

		public abstract bool UpdateKeyWord(KeyWord kw);

		public abstract IDataReader GetKeyWords();

		#endregion

		#region Statistics

		public abstract bool TrackEntry(EntryView ev);
		public abstract bool TrackEntry(IEnumerable<EntryView> evc);

		#endregion

		#region Images

		public abstract IDataReader GetImagesByCategoryID(int catID, bool activeOnly);
		public abstract IDataReader GetSingleImage(int imageID, bool activeOnly);

		public abstract int InsertImage(Image image);
		public abstract bool UpdateImage(Image image);
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
		public abstract void ClearLog();
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