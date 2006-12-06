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
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using System.Configuration.Provider;
using Subtext.Framework.Configuration;
using System.Collections.Specialized;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Provides a Data Object Source for interacting with Subtext Data.  One example 
	/// is a DataObjectProvider, which stores Subtext data in a SQL Server database.
	/// </summary>
    public abstract class ObjectProvider : ProviderBase
	{
		private static ObjectProvider provider;
		private static GenericProviderCollection<ObjectProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<ObjectProvider>("ObjectProvider", out provider);

		/// <summary>
		/// Returns the currently configured ObjectProvider.
		/// </summary>
		/// <returns></returns>
		public static ObjectProvider Instance()
		{
			return provider;
		}

		/// <summary>
		/// Returns all the configured ObjectProvider.
		/// </summary>
		public static GenericProviderCollection<ObjectProvider> Providers
		{
			get
			{
				return providers;
			}
		}

        /// <summary>
        /// Initializes this provider, setting the connection string.
        /// </summary>
        /// <param name="name">Friendly Name of the provider.</param>
        /// <param name="configValue">Config value.</param>
        public override void Initialize(string name, NameValueCollection configValue)
        {
            _connectionString = ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName", configValue);
            base.Initialize(name, configValue);
        }

        private string _connectionString;
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value></value>
        public string ConnectionString
        {
            //TODO: Make this protected.
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        #region ObjectProvider Specific methods
		#region Host

		/// <summary>
		/// Returns the <see cref="HostInfo"/> for the Subtext installation.
		/// </summary>
		/// <returns>A <see cref="HostInfo"/> instance.</returns>
		public abstract HostInfo LoadHostInfo(HostInfo info);

		/// <summary>
		/// Creates an initial Host instance.
		/// </summary>
		/// <param name="username">The username of the host admin.</param>
		/// <param name="password">The password of the host admin.</param>
		/// <param name="passwordSalt">The password salt.</param>
		/// <param name="email">The email.</param>
		/// <returns></returns>
		public abstract HostInfo CreateHost(HostInfo host, string username, string password, string passwordSalt, string email);
		
		#endregion Host

		#region Blogs
		/// <summary>
		/// Gets a pageable <see cref="IList"/> of <see cref="BlogInfo"/> instances.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
		public abstract PagedCollection<BlogInfo> GetPagedBlogs(string host, int pageIndex, int pageSize, ConfigurationFlag flags);
		
		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public abstract BlogInfo GetBlogById(int blogId);
		#endregion Blogs
		
		#region Entries

		#region Paged Posts

		/// <summary>
		/// Returns a pageable collection of entries ordered by the id descending.
		/// This is used in the admin section.
		/// </summary>
		/// <param name="postType">Type of the post.</param>
		/// <param name="categoryID">The category ID.</param>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
        public abstract IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize);

		/// <summary>
		/// Gets the paged feedback.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="status">A flag for the status types to return.</param>
		/// <param name="excludeStatusMask">A flag for the statuses to exclude.</param>
		/// <param name="type">The type of feedback to return.</param>
		/// <returns></returns>
		public abstract IPagedCollection<FeedbackItem> GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlag status, FeedbackStatusFlag excludeStatusMask, FeedbackType type);
		
		#endregion

		#region EntryDays

		public abstract EntryDay GetEntryDay(DateTime dt);
        public abstract ICollection<EntryDay> GetPostsByMonth(int month, int year);
        public abstract ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int catID);

		/// <summary>
		/// Gets entries within the system that meet the 
		/// <see cref="PostConfig"/> flags.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <param name="pc">Pc.</param>
		/// <returns></returns>
        public abstract ICollection<EntryDay> GetBlogPosts(int itemCount, PostConfig pc);

		#endregion

		#region EntryCollections
		/// <summary>
		/// Returns the previous and next entry to the specified entry.
		/// </summary>
		/// <param name="entryId"></param>
		/// <param name="postType"></param>
		/// <returns></returns>
		public abstract IList<Entry> GetPreviousAndNextEntries(int entryId, PostType postType);
		
		/// <summary>
		/// Gets the entries that meet the <see cref="PostType"/> and 
		/// <see cref="PostConfig"/> flags.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <param name="postType">The type of entry to return.</param>
		/// <param name="postConfig">Post Configuration options.</param>
        /// <param name="includeCategories">Whether or not to include categories</param>
		/// <returns></returns>
		public abstract IList<Entry> GetConditionalEntries(int itemCount, PostType postType, PostConfig postConfig, bool includeCategories);
		
		/// <summary>
		/// Gets the <see cref="FeedbackItem" /> items for the specified entry.
		/// </summary>
		/// <param name="parentEntry">The parent entry.</param>
		/// <returns></returns>
		public abstract IList<FeedbackItem> GetFeedbackForEntry(Entry parentEntry);

		/// <summary>
		/// Gets the feedback by the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public abstract FeedbackItem GetFeedback(int id);

		/// <summary>
		/// Gets the feedback counts for the various top level statuses.
		/// </summary>
		/// <param name="approved">The approved.</param>
		/// <param name="needsModeration">The needs moderation.</param>
		/// <param name="flaggedAsSpam">The flagged as spam.</param>
		/// <param name="deleted">The deleted.</param>
		public abstract void GetFeedbackCounts(out int approved, out int needsModeration, out int flaggedAsSpam, out int deleted);
		
		public abstract IList<Entry> GetPostCollectionByMonth(int month, int year);
		public abstract IList<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool activeOnly);
		public abstract IList<Entry> GetEntriesByCategory(int ItemCount,int catID,bool ActiveOnly);

		#endregion

		#region Single Entry

		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public abstract Entry GetCommentByChecksumHash(string checksumHash);
        
	    /// <summary>
	    /// Returns an <see cref="Entry" /> with the specified id.
	    /// </summary>
	    /// <param name="id">Id of the entry</param>
        /// <param name="activeOnly">Whether or not to only return the entry if it is active.</param>
        /// <param name="includeCategories">Whether the entry should have its Categories property populated</param>
	    /// <returns></returns>
	    public abstract Entry GetEntry(int id, bool activeOnly, bool includeCategories);

        /// <summary>
        /// Returns an <see cref="Entry" /> with the specified entry name.
        /// </summary>
        /// <param name="entryName">Url friendly entry name.</param>
        /// <param name="activeOnly">Whether or not to only return the entry if it is active.</param>
        /// <param name="includeCategories">Whether the entry should have its Categories property populated</param>
        /// <returns></returns>
        public abstract Entry GetEntry(string entryName, bool activeOnly, bool includeCategories);

		#endregion

		#region Delete

		/// <summary>
		/// Deletes the specified entry.
		/// </summary>
		/// <param name="entryId">The entry id.</param>
		/// <returns></returns>
		public abstract bool Delete(int entryId);

		/// <summary>
		/// Completely deletes the specified feedback as 
		/// opposed to moving it to the trash.
		/// </summary>
		/// <param name="id">The id.</param>
		public abstract void DestroyFeedback(int id);

		/// <summary>
		/// Destroys the feedback with the given status.
		/// </summary>
		/// <param name="status">The status.</param>
		public abstract void DestroyFeedback(FeedbackStatusFlag status);
		#endregion

		/// <summary>
		/// Creates a feedback record and returs the id of the newly created item.
		/// </summary>
		/// <param name="feedbackItem"></param>
		/// <returns></returns>
		public abstract int CreateFeedback(FeedbackItem feedbackItem);
		
		/// <summary>
		/// Creates the specified entry attaching the specified categories.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="categoryIds">Category Ids.</param>
		/// <returns></returns>
		public abstract int CreateEntry(Entry entry, int[] categoryIds);

        /// <summary>
        /// Adds a new entry in the database.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public abstract int InsertEntry(Entry entry);

        /// <summary>
        /// Saves changes to the specified entry attaching the specified categories.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <param name="categoryIds">Category Ids.</param>
        /// <returns></returns>
		public abstract bool Update(Entry entry, int[] categoryIds);

		/// <summary>
		/// Saves changes to the specified feedback.
		/// </summary>
		/// <param name="feedbackItem">The feedback item.</param>
		/// <returns></returns>
		public abstract bool Update(FeedbackItem feedbackItem);

		#region Entry Category List

		public abstract bool SetEntryCategoryList(int entryId, int[] categoryIds);

		#endregion

		#endregion

		#region Links/Categories

		#region Paged Links

        public abstract IPagedCollection<Link> GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending);

		#endregion

		#region LinkCollection

        public abstract ICollection<Link> GetLinkCollectionByPostID(int postID);
        public abstract ICollection<Link> GetLinksByCategoryID(int catID, bool activeOnly);

		#endregion

		#region Single Link

		public abstract Link GetLink(int linkID);
		
		#endregion

		#region LinkCategoryCollection

        public abstract ICollection<LinkCategory> GetCategories(CategoryType catType, bool activeOnly);
        public abstract ICollection<LinkCategory> GetActiveCategories();

		#endregion

		#region LinkCategory

		public abstract LinkCategory GetLinkCategory(int categoryId, bool activeOnly);
		public abstract LinkCategory GetLinkCategory(string categoryName, bool activeOnly);

		#endregion

		#region Edit Links/Categories

		public abstract bool UpdateLink(Link link);
		public abstract int CreateLink(Link link);
		public abstract bool UpdateLinkCategory(LinkCategory category);
		public abstract int CreateLinkCategory(LinkCategory category);
		public abstract bool DeleteLinkCategory(int categoryID);
		public abstract bool DeleteLink(int linkID);

		#endregion

		#endregion

		#region Stats

        public abstract IPagedCollection<ViewStat> GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate);
        public abstract IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId);

		public abstract bool TrackEntry(EntryView view);
		public abstract bool TrackEntry(IEnumerable<EntryView> views);

		#endregion

		#region  Configuration

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for
		/// allowing a user with a freshly installed blog to immediately gain access
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="username">The username of the blog owner.</param>
		/// <param name="formattedPassword">The password for the blog owner.</param>
		/// <param name="passwordSalt">The password salt.</param>
		/// <param name="passwordQuestion">The password reset question.</param>
		/// <param name="passwordAnswer">The password reset answer.</param>
		/// <param name="email">The email.</param>
		/// <param name="host">The host.</param>
		/// <param name="subfolder">The subfolder.</param>
		/// <returns></returns>
		public abstract BlogInfo CreateBlog(string title, string username, string formattedPassword, string passwordSalt, string passwordQuestion, string passwordAnswer, string email, string host, string subfolder);

		/// <summary>
		/// Updates the specified blog configuration.
		/// </summary>
		/// <param name="info">Config.</param>
		/// <returns></returns>
		public abstract bool UpdateBlog(BlogInfo info);
		
		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <param name="hostname">Hostname.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		/// <returns></returns>
		/// <param name="hostname">Hostname.</param>
		/// <param name="subfolder">Subfolder.</param>
		/// <returns></returns>
		public BlogInfo GetBlogInfo(string hostname, string subfolder)
		{
			return GetBlogInfo(hostname, subfolder, true);
		}

		/// <summary>
		/// Returns a <see cref="BlogInfo"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <remarks>
		/// Until Subtext supports multiple blogs again (if ever), 
		/// this will always return the same instance.
		/// </remarks>
		/// <param name="hostname">Hostname.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the subfolder and hostname match.</param>
		/// <returns></returns>
		public abstract BlogInfo GetBlogInfo(string hostname, string subfolder, bool strict);
		#endregion

		#region KeyWords

		public abstract KeyWord GetKeyWord(int keyWordID);
        public abstract ICollection<KeyWord> GetKeyWords();
        public abstract IPagedCollection<KeyWord> GetPagedKeyWords(int pageIndex, int pageSize);
		public abstract bool UpdateKeyWord(KeyWord keyWord);
		public abstract int InsertKeyWord(KeyWord keyWord);
		public abstract bool DeleteKeyWord(int id);

		#endregion

		#region Images

        public abstract ImageCollection GetImagesByCategoryID(int catID, bool activeOnly);
		public abstract Image GetImage(int imageID, bool activeOnly);
		public abstract int InsertImage(Image image);
		public abstract bool UpdateImage(Image _image);
		public abstract bool DeleteImage(int imageID);

		#endregion

		#region Archives
        public abstract ICollection<ArchiveCount> GetPostsByYearArchive();
        public abstract ICollection<ArchiveCount> GetPostsByMonthArchive();
        public abstract ICollection<ArchiveCount> GetPostsByCategoryArchive();
		#endregion

		#region Plugins
		/// <summary>
		/// Returns the Guids of all plugins enabled for the current blog
		/// </summary>
		/// <returns>A list of Guids</returns>
		public abstract ICollection<Guid> GetEnabledPlugins();

		/// <summary>
		/// Enable a plugin for the current blog
		/// </summary>
        /// <param name="pluginId">The Guid of the plugin to enable</param>
		/// <returns>True if the operation completed correctly, false otherwise</returns>
		public abstract bool EnablePlugin(Guid pluginId);

		/// <summary>
		/// Disable a plugin for the current blog
		/// </summary>
        /// <param name="pluginId">The Guid of the plugin to disable</param>
		/// <returns>True if the operation completed correctly, false otherwise</returns>
		public abstract bool DisablePlugin(Guid pluginId);

		/// <summary>
		/// Returns a list of all the blog level settings defined for a plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <returns>A strongly type HashTable ettings</returns>
		public abstract NameValueCollection GetPluginGeneralSettings(Guid pluginId);

		/// <summary>
		/// Add a new blog level settings for the plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <param name="key">Key identifying the setting</param>
		/// <param name="value">Value of the setting</param>
		/// <returns>True if the operation completed correctly, false otherwise</returns>
		public abstract bool InsertPluginGeneralSettings(Guid pluginId, string key, string value);

		/// <summary>
		/// Update a blog level settings for the plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <param name="key">Key identifying the setting</param>
		/// <param name="value">New value of the setting</param>
		/// <returns>True if the operation completed correctly, false otherwise</returns>
		public abstract bool UpdatePluginGeneralSettings(Guid pluginId, string key, string value);

		#endregion

        #region Admin

        /// <summary>
        /// Gets the specified page of log entries.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public abstract IDataReader GetPagedLogEntries(int pageIndex, int pageSize);
	    
        public abstract void ClearLog();

        /// <summary>
        /// Clears all content (Entries, Comments, Track/Ping-backs, Statistices, etc...) 
        /// for a the current blog (sans the Image Galleries).
        /// </summary>
        /// <returns>
        ///     TRUE - At least one unit of content was cleared.
        ///     FALSE - No content was cleared.
        /// </returns>
        public abstract bool ClearBlogContent();

        #endregion

        #region Aggregate Data
	    
        /// <summary>
        /// Returns data displayed on an aggregate blog's home page.
        /// </summary>
        /// <returns></returns>
        public abstract DataSet GetAggregateHomePageData(int groupId);

        public abstract DataTable GetAggregateRecentPosts(int groupId);

        #endregion

        #endregion


    }
}