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
using System.Configuration.Provider;
using System.Data;
using Subtext.Extensibility;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
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
		private static DbProvider provider;
		private static readonly GenericProviderCollection<DbProvider> providers = ProviderConfigurationHelper.LoadProviderCollection("Database", out provider);

		/// <summary>
		/// Returns the currently configured DbProvider.
		/// </summary>
		/// <returns></returns>
		public static DbProvider Instance()
		{
			return provider;
		}

		/// <summary>
		/// Returns all the configured DbProvider.
		/// </summary>
		public static GenericProviderCollection<DbProvider> Providers
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
			get {return _connectionString;}
			set {_connectionString = value;}
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

        #region Aggregate HomePage Data
		/// <summary>
		/// Returns data displayed on an aggregate blog's home page.
		/// </summary>
		/// <param name="groupId">The group id.</param>
		/// <returns></returns>
	    public abstract DataSet GetAggregateHomePageData(int groupId);

        public abstract DataSet GetAggregateStats(int groupId);

        public abstract DataSet GetAggregateTotalStats(int groupId);

        public abstract DataTable GetAggregateRecentPosts(int groupId);

        public abstract DataTable GetAggregateRecentImages(int groupId);
        
	    /// <summary>
	    /// Returns a data set with the previous and next entries.
	    /// </summary>
	    /// <param name="entryId"></param>
	    /// <returns></returns>
	    public abstract IDataReader GetPreviousNext(int entryId);
        #endregion

        #region Get Blog Data
        /// <summary>
		/// Returns the specified number of blog entries
		/// </summary>
		/// <param name="itemCount"></param>
		/// <param name="postType"></param>
		/// <param name="postConfiguration"></param>
        /// <param name="includeCategories">Whether or not to include categories</param>
		/// <returns></returns>
		public abstract IDataReader GetConditionalEntries(int itemCount, PostType postType, PostConfig postConfiguration, bool includeCategories);
		public abstract IDataReader GetEntriesByDateRange(DateTime start, DateTime stop, PostType postType, bool activeOnly);

		/// <summary>
		/// Gets the feed back item by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public abstract IDataReader GetFeedBackItem(int id);

		/// <summary>
		/// Gets the feedback counts for the various top level statuses.
		/// </summary>
		/// <param name="approved">The approved.</param>
		/// <param name="needsModeration">The needs moderation.</param>
		/// <param name="flaggedAsSpam">The flagged as spam.</param>
		/// <param name="deleted">The deleted.</param>
		public abstract void GetFeedbackCounts(out int approved, out int needsModeration, out int flaggedAsSpam, out int deleted);
		
		/// <summary>
		/// Gets the feed back items belonging to the specified post.
		/// </summary>
		/// <param name="postId">The post id.</param>
		/// <returns></returns>
		public abstract IDataReader GetFeedBackItems(int postId);
        public abstract IDataReader GetEntryDayReader(DateTime dt);

		//Should Power both List<EntryDay> and EntryCollection
		public abstract IDataReader GetPostCollectionByMonth(int month, int year);
		
		public abstract IDataReader GetEntriesByCategory(int itemCount, int catID, bool activeOnly);
        public abstract IDataReader GetEntriesByTag(int itemCount, string tagName);

		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public abstract IDataReader GetCommentByChecksumHash(string checksumHash);
	    
	    /// <summary>
	    /// Returns a Data Reader pointing to the entry specified by the entry id. 
	    /// Only returns entries for the current blog (Config.CurrentBlog).
	    /// </summary>
	    /// <param name="id"></param>
	    /// <param name="activeOnly"></param>
	    /// <param name="includeCategories"></param>
	    /// <returns></returns>
        public abstract IDataReader GetEntryReader(int id, bool activeOnly, bool includeCategories);

        /// <summary>
	    /// Returns a Data Reader pointing to the active entry specified by the entry id no matter 
	    /// which bog it belongs to.
	    /// </summary>
	    /// <param name="id"></param>
        /// <param name="includeCategories"></param>
	    /// <returns></returns>
        public abstract IDataReader GetEntryReader(int id, bool includeCategories);
        
	    /// <summary>
        /// Returns a Data Reader pointing to the entry specified by the entry name.
		/// Only returns entries for the current blog (Config.CurrentBlog).
        /// </summary>
        /// <param name="entryName">Url friendly entry name.</param>
        /// <param name="activeOnly"></param>
        /// <param name="includeCategories"></param>
        /// <returns></returns>
	    public abstract IDataReader GetEntryReader(string entryName, bool activeOnly, bool includeCategories);
		#endregion

		#region Update Blog Data
	    /// <summary>
	    /// Deletes an entry with the specified id.
	    /// </summary>
	    /// <param name="id"></param>
	    /// <returns></returns>
		public abstract bool DeleteEntry(int id);
		
		/// <summary>
		/// Completely deletes a feedback item from the database.
		/// </summary>
		/// <param name="id"></param>
		public abstract void DestroyFeedback(int id);

		/// <summary>
		/// Completely deletes all feedback that fit the status.
		/// </summary>
		/// <param name="status"></param>
		public abstract void DestroyFeedback(FeedbackStatusFlag status);
	    
	    /// <summary>
	    /// Adds a new entry in the database.
	    /// </summary>
	    /// <param name="entry"></param>
	    /// <returns></returns>
		public abstract int InsertEntry(Entry entry);
		
	    /// <summary>
	    /// Updates an existing entry.
	    /// </summary>
	    /// <param name="entry"></param>
	    /// <returns></returns>
	    public abstract bool UpdateEntry(Entry entry);

		/// <summary>
		/// Updates an existing feedback.
		/// </summary>
		/// <param name="feedbackItem"></param>
		/// <returns></returns>
		public abstract bool UpdateFeedback(FeedbackItem feedbackItem);
		
	    /// <summary>
	    /// Adds comment or a ping/trackback for an entry in the database.
	    /// </summary>
	    /// <param name="feedbackItem"></param>
	    /// <returns></returns>
		public abstract int InsertFeedback(FeedbackItem feedbackItem);
		#endregion

		#region Links

		public abstract IDataReader GetLinkCollectionByPostID(int postId);

		public abstract bool SetEntryCategoryList(int postId, int[] categoryIds);

        public abstract bool SetEntryTagList(int postId, IEnumerable<string> tags);

		public abstract bool DeleteLink(int linkId);

		public abstract IDataReader GetLinkReader(int linkID);

		public abstract int InsertLink(Link link); //Create?

		public abstract bool UpdateLink(Link link); 

		public abstract IDataReader GetCategories(CategoryType catType, bool activeOnly);

		public abstract DataSet GetActiveCategories(); //Rename, since it includes LinkCollection as well

		#endregion

		#region Categories

		public abstract bool DeleteCategory(int catId);
		/// <summary>
		/// Returns a data reader for the specified category id.
		/// </summary>
		/// <param name="categoryId"></param>
		/// <param name="isActive"></param>
		/// <returns></returns>
		public abstract IDataReader GetLinkCategory(int categoryId, bool isActive);

		/// <summary>
		/// Returns a data reader for the specified category name.
		/// </summary>
		/// <param name="categoryName"></param>
		/// <param name="isActive"></param>
		/// <returns></returns>
		public abstract IDataReader GetLinkCategory(string categoryName, bool isActive);

		public abstract bool UpdateCategory(LinkCategory lc);

		public abstract int InsertCategory(LinkCategory category);

		#endregion

        #region BlogGroups

        public abstract bool DeleteBlogGroup(int BlogGroupId);
        /// <summary>
        /// Deletes the specified BlogGroup id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public abstract IDataReader GetBlogGroup(int id, bool activeOnly);

		/// <summary>
		/// Lists the blog groups.
		/// </summary>
		/// <param name="isActive">if set to <c>true</c> [is active].</param>
		/// <returns></returns>
        public abstract IDataReader ListBlogGroups(bool isActive);

		/// <summary>
		/// Sets the group active.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="isActive">if set to <c>true</c> [is active].</param>
		/// <returns></returns>
        public abstract IDataReader SetGroupActive(int id, bool isActive);

		/// <summary>
		/// Updates the blog group.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="title">The title.</param>
		/// <param name="isActive">if set to <c>true</c> [is active].</param>
		/// <param name="displayOrder">The display order.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
        public abstract bool UpdateBlogGroup(int id, string title, bool isActive, int displayOrder, string description);

		/// <summary>
		/// Inserts the blog group.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="isActive">if set to <c>true</c> [is active].</param>
		/// <param name="displayOrder">The display order.</param>
		/// <param name="description">The description.</param>
		/// <returns></returns>
        public abstract int InsertBlogGroup(string title, bool isActive, int displayOrder, string description);

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
		/// Adds the initial blog configuration.  This is a convenience method for
		/// allowing a user with a freshly installed blog to immediately gain access
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <param name="host">The host.</param>
		/// <param name="subfolder">The subfolder.</param>
		/// <param name="blogGroupId">The blog group.</param>
		/// <returns></returns>
        public abstract bool AddBlogConfiguration(string title, string userName, string password, string host, string subfolder, int blogGroupId);

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

		#region BlogAlias

		public abstract bool AddBlogAlias(BlogAlias alias);
		
		public abstract bool DeleteBlogAlias(BlogAlias alias);
		
		public abstract bool UpdateBlogAlias(BlogAlias alias);

		#endregion
		#region Tags

		public abstract IDataReader GetTopTags(int ItemCount);

        #endregion

		#region MetaTags

	    public abstract int InsertMetaTag(MetaTag metaTag);

	    public abstract bool UpdateMetaTag(MetaTag metaTag);

		public abstract IDataReader GetMetaTagsForBlog(BlogInfo blog);

	    public abstract IDataReader GetMetaTagsForEntry(Entry entry);

        public abstract bool DeleteMetaTag(int metaTagId);

		#endregion

		#region KeyWord
		public abstract IDataReader GetKeyWord(int keyWordID);
		public abstract IDataReader GetPagedKeyWords(int pageIndex, int pageSize);

		public abstract bool DeleteKeyWord(int keywordId);

		public abstract int InsertKeyWord(KeyWord keyword);

		public abstract bool UpdateKeyWord(KeyWord kw);

		public abstract IDataReader GetKeyWords();

		#endregion

		#region Statistics

		public abstract bool TrackEntry(EntryView ev);
		public abstract bool TrackEntry(IEnumerable<EntryView> evc);

		#endregion

		#region Images

		public abstract IDataReader GetImagesByCategoryID(int catID, bool activeOnly);
		public abstract IDataReader GetImage(int imageID, bool activeOnly);

		public abstract int InsertImage(Image image);
		public abstract bool UpdateImage(Image image);
		public abstract bool DeleteImage(int imageID);

		#endregion

		#region Admin

		/// <summary>
		/// Returns a list of all the blogs within the specified range.
		/// </summary>
		/// <param name="host">Hostname for the blogs to grab</param>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="flags">Filter blogs retrieved.</param>
		/// <returns></returns>
		public abstract IDataReader GetPagedBlogs(string host, int pageIndex, int pageSize, ConfigurationFlag flags);

		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public abstract IDataReader GetBlogById(int blogId);

		/// <summary>
		/// Get blog using a domain alias
		/// </summary>
		/// <param name="host"></param>
		/// <param name="subfolder"></param>
		/// <param name="strict"></param>
		/// <returns></returns>
		public abstract IDataReader GetBlogByDomainAlias(string host,string subfolder, bool strict);

		public abstract IDataReader GetPagedBlogDomainAliases(int blogId, int pageIndex, int pageSize);

		public abstract IDataReader GetPagedLinks(int categoryId, int pageIndex, int pageSize, bool sortDescending);
		
		/// <summary>
		/// Returns a data reader (<see cref="IDataReader" />) pointing to all the blog entries 
		/// ordered by ID Descending for the specified page index (0-based) and page size.
		/// </summary>
		/// <param name="postType"></param>
		/// <param name="categoryID"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public abstract IDataReader GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize);
		
		/// <summary>
		/// Returns a data reader (<see cref="IDataReader" />) pointing to all the comments 
		/// ordered by ID Descending for the specified page index (0-based) and page size.
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="excludeStatusMask">A flag for the statuses to exclude.</param>
		/// <param name="status">Status flag for the feedback to return.</param>
		/// <param name="type">Feedback Type (comment, comment api, etc..) for the feedback to return.</param>
		/// <returns></returns>
		public abstract IDataReader GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlag status, FeedbackStatusFlag excludeStatusMask, FeedbackType type);
		
		/// <summary>
		/// Gets the specified page of log entries.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
		public abstract IDataReader GetPagedLogEntries(int pageIndex, int pageSize);
		public abstract void ClearLog();
		public abstract IDataReader GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate);
		public abstract IDataReader GetPagedReferrers(int pageIndex, int pageSize, int entryId);
	    
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

		#region Archives

		public abstract IDataReader GetPostsByMonthArchive();
		public abstract IDataReader GetPostsByYearArchive();
		public abstract IDataReader GetPostsByCategoryArchive();

		#endregion
		#endregion

		public abstract IDataReader GetBlogAliasById(int aliasId);
	}
}
