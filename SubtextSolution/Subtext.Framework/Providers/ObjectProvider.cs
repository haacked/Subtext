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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Security;
using System.Collections.Specialized;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using System.Configuration.Provider;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Provides a Data Object Source for interacting with Subtext Data.  One example 
	/// is a DataObjectProvider, which stores Subtext data in a SQL Server database.
	/// </summary>
	public abstract class ObjectProvider : ProviderBase
	{
		private static ObjectProvider provider;
		private static GenericProviderCollection<ObjectProvider> providers = ProviderConfigurationHelper.LoadProviderCollection("ObjectProvider", out provider);

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
		/// <param name="config"></param>
		public override void Initialize(string name, NameValueCollection config)
		{
			_connectionString = ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName", config);
			base.Initialize(name, config);
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

		/// <summary>
		/// Gets the related links for the entry.
		/// </summary>
		/// <param name="entryId">The entry id.</param>
		/// <returns></returns>
		public abstract IList<RelatedLink> GetRelatedLinks(int entryId);

		/// <summary>
		/// Gets the top links for the current blog.
		/// </summary>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public abstract IList<RelatedLink> GetTopLinks(int count);

		#region ObjectProvider Specific methods
		#region Host

		/// <summary>
		/// Returns the <see cref="HostInfo"/> for the Subtext installation.
		/// </summary>
		/// <param name="info">The host info object to load.</param>
		public abstract void LoadHostInfo(HostInfo info);

		/// <summary>
		/// Creates an initial Host instance.
		/// </summary>
		/// <param name="owner">The owner of this host installation</param>
		/// <param name="info">The info.</param>
		/// <returns></returns>
		public abstract void CreateHost(MembershipUser owner, HostInfo info);

		#endregion Host

		#region Blogs
		/// <summary>
		/// Gets a pageable <see cref="IList"/> of <see cref="BlogInfo"/> instances.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
		/// <param name="flags"></param>
		public abstract PagedCollection<BlogInfo> GetPagedBlogs(string host, int pageIndex, int pageSize, ConfigurationFlags flags);

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
		/// <param name="categoryId">The category id filter. Pass in NullValue.NullInt32 to not filter by a category ID</param>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Number of records to return per page.</param>
		/// <returns></returns>
		public abstract IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryId, int pageIndex, int pageSize);

		/// <summary>
		/// Gets the paged feedback.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="status">A flag for the status types to return.</param>
		/// <param name="excludeStatusMask">A flag for the statuses to exclude.</param>
		/// <param name="type">The type of feedback to return.</param>
		/// <returns></returns>
		public abstract IPagedCollection<FeedbackItem> GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlags status, FeedbackStatusFlags excludeStatusMask, FeedbackType type);

		#endregion

		#region EntryDays

		/// <summary>
		/// Gets the entry day.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns></returns>
		public abstract EntryDay GetEntryDay(DateTime date);
		/// <summary>
		/// Gets the posts by category ID.
		/// </summary>
		/// <param name="itemCount">The item count.</param>
		/// <param name="categoryId">The cat ID.</param>
		/// <returns></returns>
		public abstract ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int categoryId);

		/// <summary>
		/// Gets entries within the system that meet the 
		/// <see cref="PostConfig"/> flags.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <param name="postConfiguration">Pc.</param>
		/// <returns></returns>
		public abstract ICollection<EntryDay> GetBlogPosts(int itemCount, PostConfig postConfiguration);

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
		/// <param name="postConfiguration">Post Configuration options.</param>
		/// <param name="includeCategories">Whether or not to include categories</param>
		/// <returns></returns>
		public abstract IList<Entry> GetConditionalEntries(int itemCount, PostType postType, PostConfig postConfiguration, bool includeCategories);

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
		public abstract IList<Entry> GetEntriesByCategory(int itemCount, int categoryId, bool activeOnly);
        public abstract IList<Entry> GetEntriesByTag(int itemCount, string tagName);

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
		public abstract void Delete(int entryId);

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
		public abstract void DestroyFeedback(FeedbackStatusFlags status);
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
		public abstract void Update(Entry entry, int[] categoryIds);

		/// <summary>
		/// Saves changes to the specified feedback.
		/// </summary>
		/// <param name="feedbackItem">The feedback item.</param>
		/// <returns></returns>
		public abstract void Update(FeedbackItem feedbackItem);

		#region Entry Category List

		public abstract void SetEntryCategoryList(int entryId, int[] categoryIds);

		#endregion

        #region Entry Tag List

		/// <summary>
		/// Sets the tags for the entry.
		/// </summary>
		/// <param name="entryId"></param>
		/// <param name="tags"></param>
		/// <returns></returns>
		public abstract void SetEntryTagList(int entryId, IList<string> tags);

        #endregion

		/// <summary>
		/// Gets the paged links.
		/// </summary>
		/// <param name="categoryId">The category type id.</param>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">if set to <c>true</c> [sort descending].</param>
		/// <returns></returns>
		public abstract IPagedCollection<Link> GetPagedLinks(int categoryId, int pageIndex, int pageSize, bool sortDescending);

		#endregion

		#region LinkCollection

		/// <summary>
		/// Gets the link collection by post ID.
		/// </summary>
		/// <param name="postId">The post id.</param>
		/// <returns></returns>
		public abstract ICollection<Link> GetLinkCollectionByPostID(int postId);

		#endregion

		#region Single Link

		public abstract Link GetLink(int linkId);

		#endregion

		#region LinkCategoryCollection

		/// <summary>
		/// Returns a collection of categories for the given category type.
		/// </summary>
		/// <param name="categoryType">The category type: PostCollection, StoryCollection, ImageCollection, ArchiveMonthCollection, LinkCollection</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public abstract IList<LinkCategory> GetCategories(CategoryType categoryType, bool activeOnly);
		
		/// <summary>
		/// Returns a collection of LinkCategories of type LinkCollection populated 
		/// with their corresponding links.
		/// </summary>
		/// <returns></returns>
		public abstract IList<LinkCategory> GetActiveLinkCollections();

		#endregion

		#region LinkCategory

		public abstract LinkCategory GetLinkCategory(int categoryId, bool activeOnly);
		public abstract LinkCategory GetLinkCategory(string categoryName, bool activeOnly);

		#endregion

		#region Edit Links/Categories

		/// <summary>
		/// Updates the link.
		/// </summary>
		/// <param name="link">The link.</param>
		public abstract void UpdateLink(Link link);
		/// <summary>
		/// Creates the link.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public abstract int CreateLink(Link link);
		/// <summary>
		/// Updates the link category.
		/// </summary>
		/// <param name="category">The category.</param>
		public abstract void UpdateLinkCategory(LinkCategory category);
		/// <summary>
		/// Creates the link category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		public abstract int CreateLinkCategory(LinkCategory category);
		/// <summary>
		/// Deletes the link category.
		/// </summary>
		/// <param name="categoryId">The category ID.</param>
		/// <returns></returns>
		public abstract void DeleteLinkCategory(int categoryId);
		/// <summary>
		/// Deletes the link.
		/// </summary>
		/// <param name="linkId">The link ID.</param>
		/// <returns></returns>
		public abstract void DeleteLink(int linkId);

		#endregion

		#endregion

        #region Stats
		/// <summary>
		/// Gets the paged referrers.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="entryId">The entry id.</param>
		/// <returns></returns>
		public abstract IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId);

		/// <summary>
		/// Writes a tracking record for the specified <see cref="EntryView" />.
		/// </summary>
		/// <param name="view">The view.</param>
		public abstract void TrackEntry(EntryView view);
		
		/// <summary>
		/// Writes a tracking record for each <see cref="EntryView" />.
		/// </summary>
		/// <param name="views"></param>
		/// <returns></returns>
		public abstract bool TrackEntry(IEnumerable<EntryView> views);

		#endregion

		#region  Configuration

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for
		/// allowing a user with a freshly installed blog to immediately gain access
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="host">The host.</param>
		/// <param name="subfolder">The subfolder.</param>
		/// <param name="owner">The blog owner.</param>
		/// <returns></returns>
		public abstract BlogInfo CreateBlog(string title, string host, string subfolder, MembershipUser owner);

		/// <summary>
		/// Updates the specified blog configuration.
		/// </summary>
		/// <param name="info">Config.</param>
		/// <returns></returns>
		public abstract void UpdateBlog(BlogInfo info);

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

        #region Tags

        /// <summary>
        /// Gets the top tags from the database sorted by tag name.
        /// </summary>
        /// <param name="itemCount">The number of tags to return.</param>
        /// <returns>
        /// A sorted dictionary with the tag name as key and entry count
        /// as value.
        /// </returns>
        public abstract IDictionary<string, int> GetTopTags(int itemCount);

        #endregion

		#region MetaTags
        /// <summary>
        /// Adds the given MetaTag to the data store.
        /// </summary>
        /// <param name="metaTag"></param>
        /// <returns></returns>
	    public abstract int Create(MetaTag metaTag);

        /// <summary>
        /// Updates the given MetaTag in the data store.
        /// </summary>
        /// <param name="metaTag"></param>
        /// <returns></returns>
	    public abstract bool Update(MetaTag metaTag);

        /// <summary>
		/// Gets a collection of MetaTags for the given Blog.
		/// </summary>
		/// <returns></returns>
		public abstract IList<MetaTag> GetMetaTagsForBlog(BlogInfo blog);

        /// <summary>
        /// Gets a collection of MetaTags for the given Entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
	    public abstract IList<MetaTag> GetMetaTagsForEntry(Entry entry);

        #endregion

        #region KeyWords

		/// <summary>
		/// Gets the keyword by its id.
		/// </summary>
		/// <param name="keyWordID">The key word ID.</param>
		/// <returns></returns>
        public abstract KeyWord GetKeyword(int keyWordID);
		
		/// <summary>
		/// Gets the keywords for the current blog.
		/// </summary>
		/// <returns></returns>
        public abstract ICollection<KeyWord> GetKeywords();
		/// <summary>
		/// Gets the keywords by page.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
        public abstract IPagedCollection<KeyWord> GetKeywordsByPage(int pageIndex, int pageSize);
		/// <summary>
		/// Updates the keyword.
		/// </summary>
		/// <param name="keyword">The keyword.</param>
		public abstract void UpdateKeyword(KeyWord keyword);
		/// <summary>
		/// Inserts the keyword.
		/// </summary>
		/// <param name="keyWord">The key word.</param>
		/// <returns></returns>
		public abstract int InsertKeyword(KeyWord keyWord);
		/// <summary>
		/// Deletes the keyword.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public abstract void DeleteKeyword(int id);

		#endregion

		#region Images

		/// <summary>
		/// Gets the images by category ID.
		/// </summary>
		/// <param name="catID">The cat ID.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public abstract ImageCollection GetImagesByCategoryID(int catID, bool activeOnly);
		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="imageID">The image ID.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public abstract Image GetImage(int imageID, bool activeOnly);
		/// <summary>
		/// Inserts the image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		public abstract int InsertImage(Image image);
		/// <summary>
		/// Updates the image.
		/// </summary>
		/// <param name="image">The image.</param>
		public abstract void UpdateImage(Image image);
		/// <summary>
		/// Deletes the image.
		/// </summary>
		/// <param name="imageId">The image ID.</param>
		/// <returns></returns>
		public abstract void DeleteImage(int imageId);

		#endregion

		#region Archives
		//TODO: Document these methods better.

		/// <summary>
		/// Gets the posts grouped by year.
		/// </summary>
		/// <returns></returns>
		public abstract ICollection<ArchiveCount> GetPostCountByYear();
		/// <summary>
		/// Gets the posts grouped by month.
		/// </summary>
		/// <returns></returns>
		public abstract ICollection<ArchiveCount> GetPostCountByMonth();
		/// <summary>
		/// Gets the posts grouped by category.
		/// </summary>
		/// <returns></returns>
		public abstract ICollection<ArchiveCount> GetPostCountByCategory();
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
		public abstract void EnablePlugin(Guid pluginId);

		/// <summary>
		/// Disable a plugin for the current blog
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin to disable</param>
		/// <returns>True if the operation completed correctly, false otherwise</returns>
		public abstract void DisablePlugin(Guid pluginId);

		/// <summary>
		/// Returns a list of all the blog level settings defined for a plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <returns>A strongly typed HashTable with settings</returns>
		public abstract NameValueCollection GetPluginBlogSettings(Guid pluginId);

		/// <summary>
		/// Add a new blog level settings for the plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <param name="key">Key identifying the setting</param>
		/// <param name="value">Value of the setting</param>
		/// <returns>True if the operation completed correctly, false otherwise</returns>
		public abstract void InsertPluginBlogSettings(Guid pluginId, string key, string value);

		/// <summary>
		/// Update a blog level settings for the plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <param name="key">Key identifying the setting</param>
		/// <param name="value">New value of the setting</param>
		/// <returns>True if the operation completed correctly, false otherwise</returns>
		public abstract void UpdatePluginBlogSettings(Guid pluginId, string key, string value);

        /// <summary>
        /// Retrieves plugin settings for a specified entry from the storage
        /// </summary>
        /// <param name="pluginId">GUID of the plugin</param>
        /// <param name="entryId">Id of the blog entry</param>
        /// <returns>A NameValueCollection with all the settings</returns>
        public abstract NameValueCollection GetPluginEntrySettings(Guid pluginId, int entryId);

        /// <summary>
        /// Inserts a new value in the plugin settings list for a specified entry
        /// </summary>
        /// <param name="pluginId">GUID of the plugin</param>
        /// <param name="entryId">Id of the blog entry</param>
        /// <param name="key">Setting name</param>
        /// <param name="value">Setting value</param>
        /// <returns>True if the operation completed correctly, false otherwise</returns>
        public abstract void InsertPluginEntrySettings(Guid pluginId, int entryId, string key, string value);

        /// <summary>
		/// Updates a plugin setting for a specified entry
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
        /// <param name="entryId">Id of the blog entry</param>
		/// <param name="key">Key identifying the setting</param>
		/// <param name="value">New value of the setting</param>
		/// <returns>True if the operation completed correctly, false otherwise</returns>
        public abstract void UpdatePluginEntrySettings(Guid pluginId, int entryId, string key, string value);



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
		public abstract void ClearBlogContent();

		#endregion

		#region Aggregate Data

		/// <summary>
		/// Returns data displayed on an aggregate blog's home page.
		/// </summary>
		/// <returns></returns>
		public abstract DataSet GetAggregateHomePageData(int groupId);

		public abstract DataTable GetAggregateRecentPosts(int groupId);

		#endregion
	}
}
