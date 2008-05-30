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
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Concrete implementation of <see cref="ObjectProvider"/>. This 
	/// provides objects persisted to a database.  The specific database 
	/// is configured via a <see cref="DbProvider"/>.
	/// </summary>
	public class DatabaseObjectProvider : ObjectProvider
	{	
		#region Host
		/// <summary>
		/// Returns the <see cref="HostInfo"/> for the Subtext installation.
		/// </summary>
		/// <returns>A <see cref="HostInfo"/> instance.</returns>
		public override HostInfo LoadHostInfo(HostInfo hostInfo)
		{
			using(IDataReader reader = DbProvider.Instance().GetHost())
			{
				if(reader.Read())
				{
					DataHelper.LoadHost(reader, hostInfo);
					reader.Close();
					return hostInfo;
				}
				reader.Close();
			}
			return null;
		}

		/// <summary>
		/// Updates the <see cref="HostInfo"/> instance.  If the host record is not in the 
		/// database, one is created. There should only be one host record.
		/// </summary>
		/// <param name="host">The host information.</param>
		public override bool UpdateHost(HostInfo host)
		{
			return DbProvider.Instance().UpdateHost(host);
		}

		#endregion Host

		#region Blogs
        public override void ClearBlogContent(int blogId) 
        {
            DbProvider.Instance().ClearBlogContent(blogId);
        }

		/// <summary>
		/// Gets a pageable <see cref="IList"/> of <see cref="BlogInfo"/> instances.
		/// </summary>
		/// <param name="host">The host filter. Set to null to return all blogs.</param>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
        /// <param name="flags"></param>
        public override PagedCollection<BlogInfo> GetPagedBlogs(string host, int pageIndex, int pageSize, ConfigurationFlags flags)
		{
			using(IDataReader reader = DbProvider.Instance().GetPagedBlogs(host, pageIndex, pageSize, flags))
			{
                PagedCollection<BlogInfo> pec = new PagedCollection<BlogInfo>();
				while(reader.Read())
				{
					pec.Add(DataHelper.LoadConfigData(reader));
				}
				reader.NextResult();
				pec.MaxItems = DataHelper.GetMaxItems(reader);
				return pec;
			}
		}

		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public override BlogInfo GetBlogById(int blogId)
		{
			using(IDataReader reader = DbProvider.Instance().GetBlogById(blogId))
			{
				if(reader.Read())
				{
					BlogInfo info = DataHelper.LoadConfigData(reader);
					return info;
				}
			}
			return null;
		}

		public override BlogAlias GetBlogAliasById(int aliasId)
		{
			BlogAlias alias = null;
			using (IDataReader reader = DbProvider.Instance().GetBlogAliasById(aliasId))
			{
				if (reader.Read())
				{
					alias = DataHelper.LoadBlogAlias(reader);
				}
				reader.Close();
			}
			return alias;
		}
		public override BlogInfo GetBlogByDomainAlias(string host, string subfolder, bool strict)
		{
			BlogInfo info = null;
			using (IDataReader reader = DbProvider.Instance().GetBlogByDomainAlias(host, subfolder, strict))
			{	
				if (reader.Read())
				{
					info = DataHelper.LoadConfigData(reader);
				}
				reader.Close();
			}
			return info;
		}

		public override PagedCollection<BlogAlias> GetPagedBlogDomainAlias(BlogInfo blog, int pageIndex, int pageSize)
		{
			IDataReader reader = DbProvider.Instance().GetPagedBlogDomainAliases(blog.Id, pageIndex, pageSize);
			try
			{
				PagedCollection<BlogAlias> pec = new PagedCollection<BlogAlias>();
				while (reader.Read())
				{
					pec.Add(DataHelper.LoadBlogAlias(reader));
				}
				reader.NextResult();
				pec.MaxItems = DataHelper.GetMaxItems(reader);
				return pec;

			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region Blog Groups
		/// <summary>
		/// Gets the blog group.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override BlogGroup GetBlogGroup(int id, bool activeOnly)
		{
			BlogGroup group;
			using (IDataReader reader = DbProvider.Instance().GetBlogGroup(id, activeOnly))
			{
				if (!reader.Read())
					return null;

				group = DataHelper.LoadBlogGroup(reader);
			}

			if (group != null)
			{
				//TODO: Make this more efficient.
				IPagedCollection<BlogInfo> blogs =
					BlogInfo.GetBlogs(0, int.MaxValue, activeOnly ? ConfigurationFlags.IsActive : ConfigurationFlags.None);
				group.Blogs = new List<BlogInfo>();
				foreach (BlogInfo blog in blogs)
				{
					if (blog.BlogGroupId == group.Id)
						group.Blogs.Add(blog);
				}
			}
			return group;
		}

		/// <summary>
		/// Lists the blog groups.
		/// </summary>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override IList<BlogGroup> ListBlogGroups(bool activeOnly)
		{
			using(IDataReader reader = DbProvider.Instance().ListBlogGroups(activeOnly))
			{
				List<BlogGroup> groups = new List<BlogGroup>();
				while(reader.Read())
				{
					groups.Add(DataHelper.LoadBlogGroup(reader));
				}
				return groups;
			}
		}
		#endregion

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
        public override IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize)
		{
			IDataReader reader = DbProvider.Instance().GetPagedEntries(postType, categoryID, pageIndex, pageSize);
			try
			{
                PagedCollection<Entry> pec = new PagedCollection<Entry>();
				while(reader.Read())
				{
					pec.Add(DataHelper.LoadEntryStatsView(reader));
				}
				reader.NextResult();
				pec.MaxItems = DataHelper.GetMaxItems(reader);
				return pec;
				
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Gets the paged feedback.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="status">A flag for the status types to return.</param>
		/// <param name="excludeStatusMask">A flag for the statuses to exclude.</param>
		/// <param name="type">The type of feedback to return.</param>
		/// <returns></returns>
        public override IPagedCollection<FeedbackItem> GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlag status, FeedbackStatusFlag excludeStatusMask, FeedbackType type)
		{
			IDataReader reader = DbProvider.Instance().GetPagedFeedback(pageIndex, pageSize, status, excludeStatusMask, type);
			IPagedCollection<FeedbackItem> pec = new PagedCollection<FeedbackItem>();
			while(reader.Read())
			{
				pec.Add(DataHelper.LoadFeedbackItem(reader));
			}
			reader.NextResult();
			pec.MaxItems = DataHelper.GetMaxItems(reader);
			return pec;
		}

		#endregion

		#region EntryDays

		public override EntryDay GetEntryDay(DateTime dt)
		{
			IDataReader reader = DbProvider.Instance().GetEntryDayReader(dt);
			try
			{
				EntryDay ed = new EntryDay(dt);
				while(reader.Read())
				{
					ed.Add(DataHelper.LoadEntry(reader));
				}
				return ed;
			}
			finally
			{
				reader.Close();
			}
		}

		/// <summary>
		/// Returns blog posts that meet the criteria specified in the <see cref="PostConfig"/> flags.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <param name="pc">Pc.</param>
		/// <returns></returns>
        public override IList<EntryDay> GetBlogPosts(int itemCount, PostConfig pc)
		{
			IDataReader reader = DbProvider.Instance().GetConditionalEntries(itemCount, PostType.BlogPost, pc, false);
			try
			{
                IList<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

        public override IList<EntryDay> GetPostsByMonth(int month, int year)
		{
			IDataReader reader = DbProvider.Instance().GetPostCollectionByMonth(month,year);
			try
			{
                IList<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

        public override IList<EntryDay> GetPostsByCategoryID(int itemCount, int catID)
		{
			IDataReader reader = DbProvider.Instance().GetEntriesByCategory(itemCount, catID, true);
			try
			{
                IList<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region EntryCollections
		/// <summary>
		/// Returns the previous and next entry to the specified entry.
		/// </summary>
		/// <param name="entryId"></param>
		/// <returns></returns>
		/// <param name="postType"></param>
		public override IList<Entry> GetPreviousAndNextEntries(int entryId, PostType postType)
		{
			using(IDataReader reader = DbProvider.Instance().GetPreviousNext(entryId))
			{
				return DataHelper.LoadEntryCollectionFromDataReader(reader);
			}
		}
		
		/// <summary>
		/// Gets the entries that meet the specific <see cref="PostType"/> 
		/// and the <see cref="PostConfig"/> flags.
		/// </summary>
		/// <remarks>
		/// This is called to get the main syndicated entries.
		/// </remarks>
		/// <param name="itemCount">Item count.</param>
		/// <param name="postType">The type of post to retrieve.</param>
		/// <param name="postConfig">Post configuration options.</param>
		/// <param name="includeCategories">Whether or not to include categories</param>
		/// <returns></returns>
		public override IList<Entry> GetConditionalEntries(int itemCount, PostType postType, PostConfig postConfig, bool includeCategories)
		{
            using(IDataReader reader = DbProvider.Instance().GetConditionalEntries(itemCount, postType, postConfig, includeCategories))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
		}

		/// <summary>
		/// Returns all the active entries for the specified post.
		/// </summary>
		/// <param name="parentEntry"></param>
		/// <returns></returns>
		public override IList<FeedbackItem> GetFeedbackForEntry(Entry parentEntry)
		{
			using (IDataReader reader = DbProvider.Instance().GetFeedBackItems(parentEntry.Id))
			{
				List<FeedbackItem> ec = new List<FeedbackItem>();
				while (reader.Read())
				{
					//Don't build links.
					FeedbackItem feedbackItem = DataHelper.LoadFeedbackItem(reader, parentEntry);
					ec.Add(feedbackItem);
				}
				return ec;
			}
		}

		/// <summary>
		/// Returns the feedback by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public override FeedbackItem GetFeedback(int id)
		{
			using(IDataReader reader = DbProvider.Instance().GetFeedBackItem(id))
			{
				if(reader.Read())
				{
					return DataHelper.LoadFeedbackItem(reader);
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the feedback counts for the various top level statuses.
		/// </summary>
		/// <param name="approved">The approved.</param>
		/// <param name="needsModeration">The needs moderation.</param>
		/// <param name="flaggedAsSpam">The flagged as spam.</param>
		/// <param name="deleted">The deleted.</param>
		public override void GetFeedbackCounts(out int approved, out int needsModeration, out int flaggedAsSpam, out int deleted)
		{
			DbProvider.Instance().GetFeedbackCounts(out approved, out needsModeration, out flaggedAsSpam, out deleted);
		}

        public override IList<Entry> GetPostCollectionByMonth(int month, int year)
		{
            using(IDataReader reader = DbProvider.Instance().GetPostCollectionByMonth(month, year))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
		}

		public override IList<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool activeOnly)
		{
            DateTime min = start;
            DateTime max = stop;
		    
		    if(stop < start)
		    {
		        min = stop;
		        max = start;
		    }

            using(IDataReader reader = DbProvider.Instance().GetEntriesByDateRange(min, max, postType, activeOnly))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
		}

		public override IList<Entry> GetEntriesByCategory(int itemCount, int catID, bool activeOnly)
		{
            using(IDataReader reader = DbProvider.Instance().GetEntriesByCategory(itemCount, catID, activeOnly))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
		}

        public override IList<Entry> GetEntriesByTag(int itemCount, string tagName)
        {
            using (IDataReader reader = DbProvider.Instance().GetEntriesByTag(itemCount, tagName))
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
        }
		#endregion

		#region Single Entry
		/// <summary>
		/// Returns an active <see cref="Entry" /> by the id regardless of which blog it is 
		/// located in.
		/// </summary>
		/// <param name="id">Id of the entry</param>
		/// <param name="includeCategories">Whether the entry should have its Categories property populated</param>
		/// <returns></returns>
		public override Entry GetEntry(int id, bool includeCategories)
		{
			using (IDataReader reader = DbProvider.Instance().GetEntryReader(id, includeCategories))
			{
				if (reader.Read())
				{
					return DataHelper.LoadEntry(reader);
				}
				return null;
			}
		}
		
		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public override Entry GetCommentByChecksumHash(string checksumHash)
		{
            using (IDataReader reader = DbProvider.Instance().GetCommentByChecksumHash(checksumHash))
            {
                if (reader.Read())
                {
                    return DataHelper.LoadEntry(reader);
                }
                return null;
            }
		}

        /// <summary>
        /// Returns an <see cref="Entry" /> with the specified id.
        /// </summary>
        /// <param name="id">Id of the entry</param>
        /// <param name="activeOnly">Whether or not to only return the entry if it is active.</param>
        /// <param name="includeCategories">Whether the entry should have its Categories property populated</param>
        /// <returns></returns>
        public override Entry GetEntry(int id, bool activeOnly, bool includeCategories)
		{
            using (IDataReader reader = DbProvider.Instance().GetEntryReader(id, activeOnly, includeCategories))
            {
                if (reader.Read())
                {
                    return DataHelper.LoadEntryWithCategories(reader);
                }
                return null;
            }
		}

        /// <summary>
        /// Returns an <see cref="Entry" /> with the specified entry name.
        /// </summary>
        /// <param name="entryName">Url friendly entry name.</param>
        /// <param name="activeOnly">Whether or not to only return the entry if it is active.</param>
        /// <param name="includeCategories">Whether the entry should have its Categories property populated</param>
        /// <returns></returns>
        public override Entry GetEntry(string entryName, bool activeOnly, bool includeCategories)
		{
            using (IDataReader reader = DbProvider.Instance().GetEntryReader(entryName, activeOnly, includeCategories))
            {
                if (reader.Read())
                {
                    return DataHelper.LoadEntryWithCategories(reader);
                }
                return null;
            }
		}		
		#endregion

		#region Delete

		/// <summary>
		/// Deletes the specified entry.
		/// </summary>
		/// <param name="entryId">The entry id.</param>
		/// <returns></returns>
		public override bool Delete(int entryId)
		{
			return DbProvider.Instance().DeleteEntry(entryId);
		}

		#endregion
		/// <summary>
		/// Completely deletes the feedback from the system.
		/// </summary>
		/// <param name="id">The id.</param>
		public override void DestroyFeedback(int id)
		{
			DbProvider.Instance().DestroyFeedback(id);
		}

		public override void DestroyFeedback(FeedbackStatusFlag status)
		{
			DbProvider.Instance().DestroyFeedback(status);
		}
		
		public override int Create(FeedbackItem feedbackItem)
		{
			return DbProvider.Instance().InsertFeedback(feedbackItem);
		}

		#region Create Entry
		/// <summary>
		/// Creates the specified entry in the back end data store attaching 
		/// the specified category ids.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="categoryIds">Category I ds.</param>
		/// <returns></returns>
		public override int Create(Entry entry, int[] categoryIds)
		{
			if(!FormatEntry(entry,true))
			{
				throw new BlogFailedPostException("Failed post exception");
			}		

		    entry.Id = DbProvider.Instance().InsertEntry(entry);	
	
			if(categoryIds != null)
			{
				DbProvider.Instance().SetEntryCategoryList(entry.Id, categoryIds);
			}

			if(entry.Id > -1 && Config.Settings.Tracking.UseTrackingServices)
			{
				entry.Url = Config.CurrentBlog.UrlFormats.EntryUrl(entry);
			}

			if(entry.Id > -1)
			{
				Config.CurrentBlog.LastUpdated = entry.DateCreated;
			}

			return entry.Id;
		}

		#endregion

		#region Update

		/// <summary>
		/// Saves changes to the specified feedback.
		/// </summary>
		/// <param name="feedbackItem">The feedback item.</param>
		/// <returns></returns>
		public override bool Update(FeedbackItem feedbackItem)
		{
			return DbProvider.Instance().UpdateFeedback(feedbackItem);
		}
		
	    /// <summary>
        /// Saves changes to the specified entry attaching the specified categories.
        /// </summary>
        /// <param name="entry">Entry.</param>
        /// <param name="categoryIds">Category Ids.</param>
        /// <returns></returns>
	    public override bool Update(Entry entry, params int[] categoryIds)
		{
			if(!FormatEntry(entry, false))
			{
				throw new BlogFailedPostException("Failed post exception");
			}

			if(!DbProvider.Instance().UpdateEntry(entry))
			{
				return false;
			}
	
			if(categoryIds != null && categoryIds.Length > 0)
			{
				DbProvider.Instance().SetEntryCategoryList(entry.Id,categoryIds);
			}
		
			if(Config.Settings.Tracking.UseTrackingServices)
			{
				if(entry.PostType == PostType.BlogPost)
				{
					entry.Url = Config.CurrentBlog.UrlFormats.EntryUrl(entry);
				}
				else
				{
					entry.Url = Config.CurrentBlog.UrlFormats.ArticleUrl(entry);
				}

				if(entry.Id > -1)
				{
					Config.CurrentBlog.LastUpdated = entry.DateModified;
				}
			}
			return true;
		}

		#endregion

		#region SetCategoriesList

		public override bool SetEntryCategoryList(int entryId, int[] categoryIds)
		{
			return DbProvider.Instance().SetEntryCategoryList(entryId, categoryIds);
		}

		#endregion

        #region SetTagList

        public override bool SetEntryTagList(int entryId, IList<string> tags)
        {
            return DbProvider.Instance().SetEntryTagList(entryId, tags);
        }

        #endregion

        #region Format Helper

        private static bool FormatEntry(Entry e, bool UseKeyWords)
		{
			//Do this before we validate the text
			if(UseKeyWords)
			{
				KeyWords.Format(e);
			}

			//TODO: Make this a configuration option.
			e.Body = Transform.EmoticonTransforms(e.Body);

			if (!Config.Settings.AllowScriptsInPosts && HtmlHelper.HasIllegalContent(e.Body))
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}

			//Never allow scripts in the title.
			if(HtmlHelper.HasIllegalContent(e.Title))
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}

			if (!Config.Settings.AllowScriptsInPosts && HtmlHelper.HasIllegalContent(e.Description))
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}

			//never allow scripts in the url.
			if(HtmlHelper.HasIllegalContent(e.Url))
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}

			if(!HtmlHelper.ConvertHtmlToXHtml(e))
			{
				return false;
			}

			return true;
		}

		#endregion

		#endregion

		#region Links/Categories

		#region Paged Links

        public override IPagedCollection<Link> GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending)
		{
			IDataReader reader = DbProvider.Instance().GetPagedLinks(categoryTypeID, pageIndex, pageSize, sortDescending);
			try
			{
                IPagedCollection<Link> plc = new PagedCollection<Link>();
				while(reader.Read())
				{
					plc.Add(DataHelper.LoadLink(reader));
				}
				reader.NextResult();
				plc.MaxItems = DataHelper.GetMaxItems(reader);
				return plc;
			}
			finally
			{
				reader.Close();
			}

		}

		#endregion

		#region LinkCollection

		public override IList<Link> GetLinkCollectionByPostID(int PostID)
		{
			IDataReader reader = DbProvider.Instance().GetLinkCollectionByPostID(PostID);
			try
			{
				IList<Link> lc = new List<Link>();
				while(reader.Read())
				{
					lc.Add(DataHelper.LoadLink(reader));
				}
				return lc;
			}
			finally
			{
				reader.Close();
			}
		}
		#endregion

		#region Single Link

		public override Link GetLink(int linkID)
		{
			IDataReader reader = DbProvider.Instance().GetLinkReader(linkID);
			try
			{
				Link link = null;
				while(reader.Read())
				{
					link = DataHelper.LoadLink(reader);
					break;
				}
				return link;
			}
			finally
			{
				reader.Close();
			}			
		}

		#endregion

        #region IList<LinkCategory>

		/// <summary>
		/// Gets the categories for the specified category type.
		/// </summary>
		/// <param name="catType">Type of the cat.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
        public override IList<LinkCategory> GetCategories(CategoryType catType, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetCategories(catType, activeOnly);
            IList<LinkCategory> lcc = new List<LinkCategory>();
			try
			{
				while(reader.Read())
				{
					lcc.Add(DataHelper.LoadLinkCategory(reader));
				}
				return lcc;
			}
			finally
			{
				reader.Close();
			}
		}

        public override IList<LinkCategory> GetActiveCategories()
		{
			DataSet ds = DbProvider.Instance().GetActiveCategories();
            IList<LinkCategory> lcc = new List<LinkCategory>();
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				LinkCategory lc = DataHelper.LoadLinkCategory(dr);
				lc.Links = new List<Link>();
				foreach(DataRow drLink in dr.GetChildRows("CategoryID"))
				{
					lc.Links.Add(DataHelper.LoadLink(drLink));
				}
				lcc.Add(lc);				
			}
			return lcc;
		}

		#endregion

		#region LinkCategory

		/// <summary>
		/// Gets the link category for the specified category id.
		/// </summary>
		/// <param name="categoryId">The category id.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override LinkCategory GetLinkCategory(int categoryId, bool activeOnly)
		{
			using(IDataReader reader = DbProvider.Instance().GetLinkCategory(categoryId, activeOnly))
			{
				return LoadLinkCategoryFromReader(reader);
			}
		}

		/// <summary>
		/// Gets the link category for the specified category name.
		/// </summary>
		/// <param name="categoryName">The category name.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override LinkCategory GetLinkCategory(string categoryName, bool activeOnly)
		{
			using(IDataReader reader = DbProvider.Instance().GetLinkCategory(categoryName, activeOnly))
			{
				return LoadLinkCategoryFromReader(reader);
			}
		}

		// Expects that the caller will dispose of the reader.
		private static LinkCategory LoadLinkCategoryFromReader(IDataReader reader)
		{
			if (reader.Read())
			{
				LinkCategory lc = DataHelper.LoadLinkCategory(reader);
				return lc;
			}
			return null;
		}
		#endregion

		#region Edit Links/Categories

		public override bool UpdateLink(Link link)
		{
			return DbProvider.Instance().UpdateLink(link);
		}

		public override int CreateLink(Link link)
		{
			return DbProvider.Instance().InsertLink(link);
		}

		public override bool UpdateLinkCategory(LinkCategory lc)
		{
			return DbProvider.Instance().UpdateCategory(lc);
		}
		
		public override int CreateLinkCategory(LinkCategory lc)
		{
			return DbProvider.Instance().InsertCategory(lc);
		}

		public override bool DeleteLinkCategory(int CategoryID)
		{
			return DbProvider.Instance().DeleteCategory(CategoryID);
		}

		public override bool DeleteLink(int LinkID)
		{
			return DbProvider.Instance().DeleteLink(LinkID);
		}

		#endregion

		#endregion

		#region Stats

        public override IPagedCollection<ViewStat> GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			IDataReader reader = DbProvider.Instance().GetPagedViewStats(pageIndex,pageSize,beginDate,endDate);
			try
			{
                IPagedCollection<ViewStat> vs = new PagedCollection<ViewStat>();
				while(reader.Read())
				{
					vs.Add(DataHelper.LoadViewStat(reader));
				}
				reader.NextResult();
				vs.MaxItems = DataHelper.GetMaxItems(reader);
				return vs;
			}
			finally
			{
				reader.Close();
			}	
		}

        public override IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId)
		{
			IDataReader reader = DbProvider.Instance().GetPagedReferrers(pageIndex, pageSize, entryId);
            return LoadPagedReferrersCollection(reader);
		}
	    
	    private static IPagedCollection<Referrer> LoadPagedReferrersCollection(IDataReader reader)
	    {
            try
            {
                IPagedCollection<Referrer> prc = new PagedCollection<Referrer>();
                while (reader.Read())
                {
                    prc.Add(DataHelper.LoadReferrer(reader));
                }
                reader.NextResult();
                prc.MaxItems = DataHelper.GetMaxItems(reader);
                return prc;
            }
            finally
            {
                reader.Close();
            }
	    }

		public override bool TrackEntry(EntryView ev)
		{
			return DbProvider.Instance().TrackEntry(ev);
		}

		public override bool TrackEntry(IEnumerable<EntryView> evc)
		{
			return DbProvider.Instance().TrackEntry(evc);
		}

		#endregion

		#region  Configuration

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
        public override bool CreateBlog(string title, string userName, string password, string host, string subfolder)
		{
			return DbProvider.Instance().AddBlogConfiguration(title, userName, password, host, subfolder, 1);
		}

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for
		/// allowing a user with a freshly installed blog to immediately gain access
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <param name="host"></param>
		/// <param name="subfolder"></param>
		/// <param name="blogGroupId"></param>
		/// <returns></returns>
        public override bool CreateBlog(string title, string userName, string password, string host, string subfolder, int blogGroupId)
        {
            return DbProvider.Instance().AddBlogConfiguration(title, userName, password, host, subfolder, blogGroupId);
        }

		public override bool CreateBlogAlias(BlogAlias alias)
		{
			return DbProvider.Instance().AddBlogAlias(alias);
		}

		public override bool UpdateBlogAlias(BlogAlias alias)
		{
			return DbProvider.Instance().UpdateBlogAlias(alias);
		}
		public override bool DeleteBlogAlias(BlogAlias alias)
		{
			return DbProvider.Instance().DeleteBlogAlias(alias);
		}

		public override bool UpdateBlog(BlogInfo info)
		{
			return DbProvider.Instance().UpdateBlog(info);
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
		/// <param name="subfolder">Subfolder.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the subfolder and hostname match.</param>
		/// <returns></returns>
		public override BlogInfo GetBlogInfo(string hostname, string subfolder, bool strict)
		{
			IDataReader reader = DbProvider.Instance().GetBlogInfo(hostname, subfolder, strict);
			try
			{
				BlogInfo info = null;
				while(reader.Read())
				{
					info = DataHelper.LoadConfigData(reader);
					break;
				}
				return info;
			}
			finally
			{
				reader.Close();
			}
		}
		#endregion

        #region Tags

        public override IDictionary<string, int> GetTopTags(int ItemCount)
        {
            using (IDataReader reader = DbProvider.Instance().GetTopTags(ItemCount))
            {
                IDictionary<string, int> tags = DataHelper.LoadTags(reader);
                return tags;
            }
        }

        #endregion

		#region MetaTags

	    public override int Create(MetaTag metaTag)
	    {
	        return DbProvider.Instance().InsertMetaTag(metaTag);
	    }


	    public override bool Update(MetaTag metaTag)
	    {
	        return DbProvider.Instance().UpdateMetaTag(metaTag);
	    }

	    public override IList<MetaTag> GetMetaTagsForBlog(BlogInfo blog)
		{
			using (IDataReader reader = DbProvider.Instance().GetMetaTagsForBlog(blog))
			{
				List<MetaTag> tags = new List<MetaTag>();

				while(reader.Read())
				{
					tags.Add(DataHelper.LoadMetaTag(reader));
				}

				return tags;
			}
		}


	    public override IList<MetaTag> GetMetaTagsForEntry(Entry entry)
	    {
	        using (IDataReader reader = DbProvider.Instance().GetMetaTagsForEntry(entry))
	        {
	            List<MetaTag> tags = new List<MetaTag>();

                while (reader.Read())
                {
                    tags.Add(DataHelper.LoadMetaTag(reader));
                }

	            return tags;
	        }
	    }


	    public override bool DeleteMetaTag(int metaTagId)
	    {
	        return DbProvider.Instance().DeleteMetaTag(metaTagId);
	    }

	    #endregion

        #region Enclosures

        public override int Create(Enclosure enclosure)
        {
            return DbProvider.Instance().InsertEnclosure(enclosure);
        }

        public override bool Update(Enclosure enclosure)
        {
            return DbProvider.Instance().UpdateEnclosure(enclosure);
        }

        public override bool DeleteEnclosure(int enclosureId)
        {
            return DbProvider.Instance().DeleteEnclosure(enclosureId);
        }

        #endregion

        #region KeyWords

        public override KeyWord GetKeyWord(int KeyWordID)
		{
			IDataReader reader = DbProvider.Instance().GetKeyWord(KeyWordID);
			try
			{
				KeyWord kw = null;
				while(reader.Read())
				{
					kw = DataHelper.LoadKeyWord(reader);
					break;
				}
				return kw;
			}
			finally
			{
				reader.Close();
			}
		}
		
		public override IList<KeyWord> GetKeyWords()
		{
			IDataReader reader = DbProvider.Instance().GetKeyWords();
			try
			{
				List<KeyWord> kwc = new List<KeyWord>();
				while(reader.Read())
				{
					kwc.Add(DataHelper.LoadKeyWord(reader));
				}
				return kwc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override IPagedCollection<KeyWord> GetPagedKeyWords(int pageIndex, int pageSize)
		{
			IDataReader reader = DbProvider.Instance().GetPagedKeyWords(pageIndex, pageSize);
			try
			{
				IPagedCollection<KeyWord> pkwc = new PagedCollection<KeyWord>();
				while(reader.Read())
				{
					pkwc.Add(DataHelper.LoadKeyWord(reader));
				}
				reader.NextResult();
				pkwc.MaxItems = DataHelper.GetMaxItems(reader);

				return pkwc;
			}
			finally
			{
				reader.Close();
			}
		}
		
		public override bool UpdateKeyWord(KeyWord keyWord)
		{
			return DbProvider.Instance().UpdateKeyWord(keyWord);
		}

		public override int InsertKeyWord(KeyWord keyWord)
		{
			return DbProvider.Instance().InsertKeyWord(keyWord);
		}

		public override bool DeleteKeyWord(int id)
		{
			return DbProvider.Instance().DeleteKeyWord(id);
		}

		#endregion

		#region Images

		public override ImageCollection GetImagesByCategoryID(int catID, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetImagesByCategoryID(catID, activeOnly);
			try
			{
				ImageCollection ic = new ImageCollection();
				while(reader.Read())
				{
					ic.Category = DataHelper.LoadLinkCategory(reader);
					break;
				}
				reader.NextResult();
				while(reader.Read())
				{
					ic.Add(DataHelper.LoadImage(reader));
				}
				return ic;
			}
			finally
			{
				reader.Close();
			}
		}

		public override Image GetImage(int imageID, bool activeOnly)
		{
			IDataReader reader = DbProvider.Instance().GetImage(imageID, activeOnly);
			try
			{
				Image image = null;
				while(reader.Read())
				{
					image = DataHelper.LoadImage(reader);
				}
				return image;
			}
			finally
			{
				reader.Close();
			}
		}

		public override int InsertImage(Image _image)
		{
			return DbProvider.Instance().InsertImage(_image);
		}

		public override bool UpdateImage(Image image)
		{
			return DbProvider.Instance().UpdateImage(image);
		}

		public override bool DeleteImage(int ImageID)
		{
			return DbProvider.Instance().DeleteImage(ImageID);
		}

		#endregion

		#region Archives

		public override IList<ArchiveCount> GetPostsByMonthArchive()
		{
			IDataReader reader = DbProvider.Instance().GetPostsByMonthArchive();
			try
			{
				IList<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override IList<ArchiveCount> GetPostsByYearArchive()
		{
			IDataReader reader = DbProvider.Instance().GetPostsByYearArchive();
			try
			{
				IList<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override IList<ArchiveCount> GetPostsByCategoryArchive()
		{
			IDataReader reader = DbProvider.Instance().GetPostsByCategoryArchive();
			try
			{
				IList<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion
	}
}
