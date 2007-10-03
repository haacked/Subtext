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
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Security;
using SubSonic;
using Subtext.Data;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;
using Subtext.Framework.Util;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Concrete implementation of <see cref="ObjectProvider"/>. This
	/// provides objects persisted to a SQL Server database.
	/// </summary>
	public class DatabaseObjectProvider : ObjectProvider
	{
		private static int BlogId
		{
			get
			{
				if (InstallationManager.IsInHostAdminDirectory || Config.CurrentBlog == null)
					return NullValue.NullInt32;
				else
					return Config.CurrentBlog.Id;
			}
		}

		/// <summary>
		/// Returns the related links for the entry
		/// </summary>
		/// <param name="entryId"></param>
		/// <returns></returns>
		public override IList<RelatedLink> GetRelatedLinks(int entryId)
		{
			List<RelatedLink> links = new List<RelatedLink>();
			using(IDataReader reader = StoredProcedures.GetRelatedLinks(BlogId, entryId).GetReader())
			{
				int id = DataHelper.ReadInt32(reader, "EntryID");
				string title = DataHelper.ReadString(reader, "Title");
				DateTime dateAdded = DataHelper.ReadDate(reader, "DateAdded");
				string myURL = Config.CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(dateAdded, id.ToString(CultureInfo.InvariantCulture));

				links.Add(new RelatedLink(title, myURL));
			}
			return links;
		}

		/// <summary>
		/// Gets the top links for the current blog.
		/// </summary>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public override IList<RelatedLink> GetTopLinks(int count)
		{
			List<RelatedLink> links = new List<RelatedLink>();
			using (IDataReader reader = StoredProcedures.GetTop10byBlogId(BlogId).GetReader())
			{
				int id = DataHelper.ReadInt32(reader, "EntryID");
				string title = DataHelper.ReadString(reader, "Title");
				DateTime dateAdded = DataHelper.ReadDate(reader, "DateAdded");
				string myURL = Config.CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(dateAdded, id.ToString(CultureInfo.InvariantCulture));

				links.Add(new RelatedLink(title, myURL));
			}
			return links;
		}
		
		#region Host
		/// <summary>
		/// Returns the <see cref="HostInfo"/> for the Subtext installation.
		/// </summary>
		/// <returns>A <see cref="HostInfo"/> instance.</returns>
		public override void LoadHostInfo(HostInfo info)
		{
			using (IDataReader reader = StoredProcedures.GetHost().GetReader())
			{
				if (reader.Read())
				{
					DataHelper.LoadHost(reader, info);
				}
			}
		}

		/// <summary>
		/// Updates the <see cref="HostInfo"/> instance.  If the host record is not in the
		/// database, one is created. There should only be one host record.
		/// </summary>
		/// <param name="owner">The username of the host admin.</param>
		/// <param name="info">The info.</param>
		public override void CreateHost(MembershipUser owner, HostInfo info)
		{
			using (IDataReader reader = StoredProcedures.CreateHost((Guid)owner.ProviderUserKey).GetReader())
			{
				if (reader.Read())
				{
					DataHelper.LoadHost(reader, info);
				}
			}
		}

		#endregion Host

		#region Blogs
		/// <summary>
		/// Gets a pageable <see cref="IList{T}"/> of <see cref="BlogInfo"/> instances.
		/// </summary>
		/// <param name="host">The host filter. Set to null to return all blogs.</param>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="flags"></param>
		/// <returns></returns>
		/// <param name="flags"></param>
		public override PagedCollection<BlogInfo> GetPagedBlogs(string host, int pageIndex, int pageSize, ConfigurationFlags flags)
		{
			using (IDataReader reader = StoredProcedures.GetPageableBlogs(pageIndex, pageSize, host, (int) flags).GetReader())
			{
				PagedCollection<BlogInfo> pec = new PagedCollection<BlogInfo>();
				while (reader.Read())
				{
					pec.Add(DataHelper.LoadBlog(reader));
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
			using (IDataReader reader = StoredProcedures.GetBlogById(blogId).GetReader())
			{
				if (reader.Read())
				{
					BlogInfo info = DataHelper.LoadBlog(reader);
					return info;
				}
			}
			return null;
		}
		#endregion

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
		public override IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryId, int pageIndex, int pageSize)
		{	
			StoredProcedure proc;
			if (categoryId > 0)
				proc = StoredProcedures.GetPageableEntriesByCategoryID(BlogId, categoryId, pageIndex, pageSize, (int)postType);
			else
				proc = StoredProcedures.GetPageableEntries(BlogId, pageIndex, pageSize, (int)postType);

			return GetPagedEntries(proc);
		}

		private static IPagedCollection<Entry> GetPagedEntries(StoredProcedure proc)
		{
			using(IDataReader reader = proc.GetReader())
			{
				PagedCollection<Entry> pec = new PagedCollection<Entry>();
				while (reader.Read())
				{
					pec.Add(DataHelper.LoadEntryStatsView(reader));
				}
				reader.NextResult();
				pec.MaxItems = DataHelper.GetMaxItems(reader);
				return pec;
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
		public override IPagedCollection<FeedbackItem> GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlags status, FeedbackStatusFlags excludeStatusMask, FeedbackType type)
		{
			int? feedbackType = (int?)type;
			if (type == FeedbackType.None)
				feedbackType = null;

			using (IDataReader reader = StoredProcedures.GetPageableFeedback(BlogId, pageIndex, pageSize, (int)status, (int)excludeStatusMask, feedbackType).GetReader())
			{
				IPagedCollection<FeedbackItem> pec = new PagedCollection<FeedbackItem>();
				while (reader.Read())
				{
					pec.Add(DataHelper.LoadFeedbackItem(reader));
				}
				reader.NextResult();
				pec.MaxItems = DataHelper.GetMaxItems(reader);

				return pec;
			}
		}

		#endregion

		#region EntryDays

		/// <summary>
		/// Gets the entries for the day.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns></returns>
		public override EntryDay GetEntryDay(DateTime date)
		{
			using(IDataReader reader = StoredProcedures.GetSingleDay(date, BlogId).GetReader())
			{
				EntryDay ed = new EntryDay(date);
				while (reader.Read())
				{
                    DateTime syndicatedDate = DataHelper.ReadDate(reader, "DateSyndicated");
                    if (syndicatedDate == NullValue.NullDateTime || syndicatedDate <= DateTime.Now)
                    {
                        ed.Add(DataHelper.LoadEntry(reader));
                    }
				}
				return ed;
			}
		}

		/// <summary>
		/// Returns blog posts that meet the criteria specified in the <see cref="PostConfig"/> flags.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <param name="postConfiguration">Pc.</param>
		/// <returns></returns>
		public override ICollection<EntryDay> GetBlogPosts(int itemCount, PostConfig postConfiguration)
		{
			using(IDataReader reader = StoredProcedures.GetConditionalEntries(itemCount, (int)PostType.BlogPost, (int)postConfiguration, BlogId, false).GetReader())
			{
				ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader, true);
				return edc;
			}
		}

		/// <summary>
		/// Gets the posts by category ID.
		/// </summary>
		/// <param name="itemCount">The item count.</param>
		/// <param name="categoryId">The cat ID.</param>
		/// <returns></returns>
		public override ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int categoryId)
		{
			using (IDataReader reader = StoredProcedures.GetPostsByCategoryID(itemCount, DataHelper.CheckNull(categoryId), true, BlogId).GetReader())
			{
				ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader, true);
				return edc;
			}
		}

		#endregion

		#region EntryCollections
		/// <summary>
		/// Returns the previous and next entry to the specified entry.
		/// </summary>
		/// <param name="entryId"></param>
		/// <param name="postType"></param>
		/// <returns></returns>
		/// <param name="postType"></param>
		public override IList<Entry> GetPreviousAndNextEntries(int entryId, PostType postType)
		{
			using (IDataReader reader = StoredProcedures.GetEntryPreviousNext(entryId, (int)postType, BlogId).GetReader())
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
		/// <param name="postConfiguration">Post configuration options.</param>
		/// <param name="includeCategories">Whether or not to include categories</param>
		/// <returns></returns>
		public override IList<Entry> GetConditionalEntries(int itemCount, PostType postType, PostConfig postConfiguration, bool includeCategories)
		{
			using (IDataReader reader = StoredProcedures.GetConditionalEntries(itemCount, (int)postType, (int)postConfiguration, BlogId, includeCategories).GetReader())
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
			using(IDataReader reader = StoredProcedures.GetFeedbackCollection(parentEntry.Id).GetReader())
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
			using (IDataReader reader = StoredProcedures.GetFeedback(id).GetReader())
			{
				if (reader.Read())
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
			StoredProcedure proc = StoredProcedures.GetFeedbackCountsByStatus(BlogId, 0, 0, 0, 0);
			proc.Execute();
			List<object> outputs = proc.OutputValues;

			approved = (int)outputs[0];
			needsModeration = (int)outputs[1];
			flaggedAsSpam = (int)outputs[2];
			deleted = (int)outputs[3];
		}

		public override IList<Entry> GetPostCollectionByMonth(int month, int year)
		{
			using (IDataReader reader = StoredProcedures.GetPostsByMonth(month, year, BlogId).GetReader())
			{
				return DataHelper.LoadEntryCollectionFromDataReader(reader);
			}
		}

		public override IList<Entry> GetPostsByDayRange(DateTime start, DateTime stop, PostType postType, bool activeOnly)
		{
			DateTime min = start;
			DateTime max = stop;

			if (stop < start)
			{
				min = stop;
				max = start;
			}

			using (IDataReader reader = StoredProcedures.GetEntriesByDayRange(min, max, (int)postType, activeOnly, BlogId).GetReader())
			{
				return DataHelper.LoadEntryCollectionFromDataReader(reader);
			}
		}

		/// <summary>
		/// Gets the entries by category.
		/// </summary>
		/// <param name="itemCount">The item count.</param>
		/// <param name="categoryId">The category id.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override IList<Entry> GetEntriesByCategory(int itemCount, int categoryId, bool activeOnly)
		{
			using (IDataReader reader = StoredProcedures.GetPostsByCategoryID(itemCount, categoryId, activeOnly, BlogId).GetReader())
			{
				return DataHelper.LoadEntryCollectionFromDataReader(reader);
			}
		}

		/// <summary>
		/// Gets the entries by tag.
		/// </summary>
		/// <param name="itemCount">The item count.</param>
		/// <param name="tagName">Name of the tag.</param>
		/// <returns></returns>
        public override IList<Entry> GetEntriesByTag(int itemCount, string tagName)
        {
			using (IDataReader reader = StoredProcedures.GetPostsByTag(itemCount, tagName, BlogId).GetReader())
            {
                return DataHelper.LoadEntryCollectionFromDataReader(reader);
            }
        }
		#endregion

		#region Single Entry
		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public override Entry GetCommentByChecksumHash(string checksumHash)
		{
			using (IDataReader reader = StoredProcedures.GetCommentByChecksumHash(checksumHash, BlogId).GetReader())
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
			using (IDataReader reader = StoredProcedures.GetSingleEntry(id, null, activeOnly, BlogId, includeCategories).GetReader())
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
			using (IDataReader reader = StoredProcedures.GetSingleEntry(null, entryName, activeOnly, BlogId, includeCategories).GetReader())
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
		public override void Delete(int entryId)
		{
			StoredProcedures.DeletePost(entryId).Execute();
		}

		#endregion
		/// <summary>
		/// Completely deletes the feedback from the system.
		/// </summary>
		/// <param name="id">The id.</param>
		public override void DestroyFeedback(int id)
		{
			StoredProcedures.DeleteFeedback(id).Execute();
		}

		/// <summary>
		/// Destroys the feedback with the given status.
		/// </summary>
		/// <param name="status">The status.</param>
		public override void DestroyFeedback(FeedbackStatusFlags status)
		{
			StoredProcedures.DeleteFeedbackByStatus(BlogId, (int)status).Execute();
		}

		/// <summary>
		/// Creates a feedback record and returs the id of the newly created item.
		/// </summary>
		/// <param name="feedbackItem"></param>
		/// <returns></returns>
		public override int CreateFeedback(FeedbackItem feedbackItem)
		{
			if (feedbackItem == null)
				throw new ArgumentNullException("feedbackItem", Resources.ArgumentNull_Generic);

			string ipAddress = null;
			if (feedbackItem.IpAddress != null)
				ipAddress = feedbackItem.IpAddress.ToString();

			string sourceUrl = null;
			if (feedbackItem.SourceUrl != null)
				sourceUrl = feedbackItem.SourceUrl.ToString();

			StoredProcedure proc = StoredProcedures.InsertFeedback(feedbackItem.Title
											, DataHelper.CheckNull(feedbackItem.Body)
			                                , BlogId
											, DataHelper.CheckNull(feedbackItem.EntryId)
											, DataHelper.CheckNull(feedbackItem.Author)
			                                , feedbackItem.IsBlogAuthor
											, DataHelper.CheckNull(feedbackItem.Email)
			                                , sourceUrl
			                                , (int) feedbackItem.FeedbackType
			                                , (int) feedbackItem.Status
			                                , feedbackItem.CreatedViaCommentAPI
			                                , feedbackItem.Referrer
											, ipAddress
			                                , feedbackItem.UserAgent
			                                , feedbackItem.ChecksumHash
			                                , feedbackItem.DateCreated
			                                , feedbackItem.DateModified
			                                , 0);

			proc.Execute();
			return (int) proc.OutputValues[0];

		}

		#region Create Entry
		/// <summary>
		/// Creates the specified entry in the back end data store attaching 
		/// the specified category ids.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="categoryIds">Category I ds.</param>
		/// <returns></returns>
		public override int CreateEntry(Entry entry, int[] categoryIds)
		{
			FormatEntry(entry, true);

			entry.Id = InsertEntry(entry);

			if (categoryIds != null)
			{
				SetEntryCategoryList(entry.Id, categoryIds);
			}

			if (entry.Id > -1 && Config.Settings.Tracking.UseTrackingServices)
			{
				entry.Url = Config.CurrentBlog.UrlFormats.EntryUrl(entry);
			}

			if (entry.Id > -1)
			{
				Config.CurrentBlog.LastUpdated = entry.DateCreated;
			}

			return entry.Id;
		}

		/// <summary>
		/// Adds a new entry to the blog.  Whether the entry be a blog post, article,
		/// </summary>
		/// <remarks>
		/// The method <see cref="SetEntryCategoryList" /> is used to save the entry's categories.
		/// </remarks>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public override int InsertEntry(Entry entry)
		{
			if (entry == null)
				throw new ArgumentNullException("entry", Resources.ArgumentNull_Generic);

			MembershipUser author = Membership.GetUser();
			Guid authorId = (Guid)(author ?? Config.CurrentBlog.Owner).ProviderUserKey;

			StoredProcedure proc = StoredProcedures.InsertEntry(entry.Title
			                             , authorId
			                             , entry.Body
			                             , (int)entry.PostType
			                             , DataHelper.CheckNull(entry.Description)
			                             , BlogId
			                             , entry.DateCreated
			                             , (int) entry.PostConfig
			                             , StringHelper.ReturnNullForEmpty(entry.EntryName)
										 , DataHelper.CheckNull(entry.DateSyndicated)
			                             , 0);
			proc.Execute();
			return (int) proc.OutputValues[0];
		}
		#endregion

		#region Update

		/// <summary>
		/// Saves changes to the specified feedback.
		/// </summary>
		/// <param name="feedbackItem">The feedback item.</param>
		/// <returns></returns>
		public override void Update(FeedbackItem feedbackItem)
		{
			string sourceUrl = null;
			if (feedbackItem.SourceUrl != null)
				sourceUrl = feedbackItem.SourceUrl.ToString();

			StoredProcedures.UpdateFeedback(feedbackItem.Id
			                                , feedbackItem.Title
			                                , DataHelper.CheckNull(feedbackItem.Body)
			                                , DataHelper.CheckNull(feedbackItem.Author)
			                                , DataHelper.CheckNull(feedbackItem.Email)
			                                , sourceUrl
			                                , (int) feedbackItem.Status
			                                , feedbackItem.ChecksumHash
			                                , Config.CurrentBlog.TimeZone.Now).Execute();
		}

		/// <summary>
		/// Saves changes to the specified entry attaching the specified categories.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="categoryIds">Category Ids.</param>
		/// <returns></returns>
		public override void Update(Entry entry, params int[] categoryIds)
		{
			FormatEntry(entry, false);

			UpdateEntry(entry);

			if (categoryIds != null && categoryIds.Length > 0)
			{
				SetEntryCategoryList(entry.Id, categoryIds);
			}

			if (Config.Settings.Tracking.UseTrackingServices)
			{
				if (entry.PostType == PostType.BlogPost)
				{
					entry.Url = Config.CurrentBlog.UrlFormats.EntryUrl(entry);
				}
				else
				{
					entry.Url = Config.CurrentBlog.UrlFormats.ArticleUrl(entry);
				}

				if (entry.Id > -1)
				{
					Config.CurrentBlog.LastUpdated = entry.DateModified;
				}
			}
		}

		/// <summary>
		/// Updates the specified entry in the database.
		/// </summary>
		/// <remarks>
		/// The method <see cref="SetEntryCategoryList" /> is used to save the entry's categories.
		/// </remarks>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		private static void UpdateEntry(Entry entry)
		{
			MembershipUser author = Membership.GetUser();
			Guid authorId = (Guid)(author ?? Config.CurrentBlog.Owner).ProviderUserKey;

			StoredProcedures.UpdateEntry(entry.Id
			                             , entry.Title
			                             , entry.Body
			                             , (int) entry.PostType
			                             , authorId
			                             , DataHelper.CheckNull(entry.Description)
			                             , entry.DateModified
			                             , (int) entry.PostConfig
			                             , DataHelper.CheckNull(entry.EntryName)
			                             , DataHelper.CheckNull(entry.DateSyndicated), BlogId).Execute();
		}

		#endregion

		#region SetCategoriesList

		public override void SetEntryCategoryList(int entryId, int[] categoryIds)
		{
			if (categoryIds == null || categoryIds.Length == 0)
				return;

			string[] cats = new string[categoryIds.Length];
			for (int i = 0; i < categoryIds.Length; i++)
			{
				cats[i] = categoryIds[i].ToString(CultureInfo.InvariantCulture);
			}
			string catList = string.Join(",", cats);

			StoredProcedures.InsertLinkCategoryList(catList, DataHelper.CheckNull(entryId), BlogId).Execute();
		}

		#endregion

        #region SetTagList

		/// <summary>
		/// Sets the tags for the entry.
		/// </summary>
		/// <param name="entryId"></param>
		/// <param name="tags"></param>
        public override void SetEntryTagList(int entryId, IList<string> tags)
        {
			if (tags == null)
				throw new ArgumentNullException("tags", "Tags cannot be null.");

        	string[] tagNames = new string[tags.Count];
        	tags.CopyTo(tagNames, 0);
			string tagList = string.Join(",", tagNames);

			StoredProcedures.InsertEntryTagList(DataHelper.CheckNull(entryId), BlogId, tagList).Execute();
        }

        #endregion

		private static void FormatEntry(Entry e, bool UseKeyWords)
		{
			//Do this before we validate the text
			if (UseKeyWords)
			{
				Keywords.Format(e);
			}

			//TODO: Make this a configuration option.
			e.Body = Transform.EmoticonTransforms(e.Body);

			// Exceptions are thrown if illegal content is found
			HtmlHelper.CheckForIllegalContent(e.Body);
			HtmlHelper.CheckForIllegalContent(e.Title);
			HtmlHelper.CheckForIllegalContent(e.Description);
			HtmlHelper.CheckForIllegalContent(e.Url);
			HtmlHelper.ConvertHtmlToXHtml(e);
		}

		#region Links/Categories

		#region Paged Links

		/// <summary>
		/// Gets the paged links.
		/// </summary>
		/// <param name="categoryId">The category type id.</param>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">if set to <c>true</c> [sort descending].</param>
		/// <returns></returns>
		public override IPagedCollection<Link> GetPagedLinks(int categoryId, int pageIndex, int pageSize, bool sortDescending)
		{
			int? categoryFilter = categoryId;

			if (categoryId < 0)
				categoryFilter = null;

			using(IDataReader reader = StoredProcedures.GetPageableLinks(BlogId, categoryFilter, pageIndex, pageSize).GetReader())
			{
				IPagedCollection<Link> plc = new PagedCollection<Link>();
				while (reader.Read())
				{
					plc.Add(DataHelper.LoadLink(reader));
				}
				reader.NextResult();
				plc.MaxItems = DataHelper.GetMaxItems(reader);
				return plc;
			}
		}

		#endregion

		#region LinkCollection

		public override ICollection<Link> GetLinkCollectionByPostID(int postId)
		{
			using(IDataReader reader = StoredProcedures.GetLinkCollectionByPostID(DataHelper.CheckNull(postId), BlogId).GetReader())
			{
				ICollection<Link> links = new List<Link>();
				while (reader.Read())
				{
					links.Add(DataHelper.LoadLink(reader));
				}
				return links;
			}
		}
		#endregion

		#region Single Link

		/// <summary>
		/// Gets the link.
		/// </summary>
		/// <param name="linkId">The link ID.</param>
		/// <returns></returns>
		public override Link GetLink(int linkId)
		{
			using(IDataReader reader = StoredProcedures.GetSingleLink(linkId, BlogId).GetReader())
			{
				Link link = null;
				while (reader.Read())
				{
					link = DataHelper.LoadLink(reader);
					break;
				}
				return link;
			}
		}

		#endregion

		#region ICollection<LinkCategory>

		/// <summary>
		/// Gets the categories.
		/// </summary>
		/// <param name="categoryType">Type of the cat.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override IList<LinkCategory> GetCategories(CategoryType categoryType, bool activeOnly)
		{
			using (IDataReader reader = StoredProcedures.GetCategory(null, null, activeOnly, BlogId, (short)categoryType).GetReader())
			{
				IList<LinkCategory> lcc = new List<LinkCategory>();
				while (reader.Read())
				{
					lcc.Add(DataHelper.LoadLinkCategory(reader));
				}
				return lcc;
			}
		}

		/// <summary>
		/// Gets the active categories.
		/// </summary>
		/// <returns></returns>
		public override IList<LinkCategory> GetActiveLinkCollections()
		{
			DataSet ds = StoredProcedures.GetActiveCategoriesWithLinkCollection(BlogId).GetDataSet();

			DataRelation dataRelation = new DataRelation("CategoryID", ds.Tables[0].Columns["CategoryID"], ds.Tables[1].Columns["CategoryID"], false);
			ds.Relations.Add(dataRelation);

			IList<LinkCategory> linkCategories = new List<LinkCategory>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				LinkCategory category = DataHelper.LoadLinkCategory(dr);
				category.Links = new List<Link>();
				foreach (DataRow linkDataRow in dr.GetChildRows("CategoryID"))
				{
					category.Links.Add(DataHelper.LoadLink(linkDataRow));
				}
				linkCategories.Add(category);
			}
			return linkCategories;
		}

		#endregion

		#region LinkCategory

		/// <summary>
		/// Gets the link category.
		/// </summary>
		/// <param name="categoryId">The category id.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override LinkCategory GetLinkCategory(int categoryId, bool activeOnly)
		{
			using (IDataReader reader = StoredProcedures.GetCategory(null, categoryId, activeOnly, BlogId, null).GetReader())
			{
				if (reader.Read())
				{
					LinkCategory category = DataHelper.LoadLinkCategory(reader);
					return category;
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the link category.
		/// </summary>
		/// <param name="categoryName">Name of the category.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override LinkCategory GetLinkCategory(string categoryName, bool activeOnly)
		{
			using (IDataReader reader = StoredProcedures.GetCategory(categoryName, null, activeOnly, BlogId, null).GetReader())
			{
				if (reader.Read())
				{
					LinkCategory lc = DataHelper.LoadLinkCategory(reader);
					return lc;
				}
				return null;		
			}
		}

		#endregion

		#region Edit Links/Categories

		/// <summary>
		/// Updates the link.
		/// </summary>
		/// <param name="link">The link.</param>
		public override void UpdateLink(Link link)
		{
			StoredProcedures.UpdateLink(DataHelper.CheckNull(link.Id)
				, link.Title
				, link.Url
				, DataHelper.CheckNull(link.Rss)
				, link.IsActive
				, link.NewWindow
				, DataHelper.CheckNull(link.CategoryID)
				, BlogId).Execute();
		}

		/// <summary>
		/// Creates the link.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <returns></returns>
		public override int CreateLink(Link link)
		{
			if (link == null)
				throw new ArgumentNullException("link", Resources.ArgumentNull_Generic);

			StoredProcedure proc = StoredProcedures.InsertLink(link.Title
			                            , link.Url
			                            , DataHelper.CheckNull(link.Rss)
			                            , link.IsActive
			                            , link.NewWindow
			                            , DataHelper.CheckNull(link.CategoryID)
			                            , DataHelper.CheckNull(link.PostID)
										, BlogId
										, 0);
			proc.Execute();			
			return (int)proc.OutputValues[0];
		}

		/// <summary>
		/// Updates the link category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		public override void UpdateLinkCategory(LinkCategory category)
		{
			if (category == null)
				throw new ArgumentNullException("category", Resources.ArgumentNull_Generic);

			StoredProcedures.UpdateCategory(DataHelper.CheckNull(category.Id)
				, category.Title
				, category.IsActive
				, (short)category.CategoryType
				, DataHelper.CheckNull(category.Description)
				, BlogId).Execute();
		}

		/// <summary>
		/// Creates the link category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		public override int CreateLinkCategory(LinkCategory category)
		{
			if (category == null)
				throw new ArgumentNullException("category", Resources.ArgumentNull_Generic);

			StoredProcedure proc = StoredProcedures.InsertCategory(category.Title
			                                                       , category.IsActive
			                                                       , BlogId
																   , (short) category.CategoryType
			                                                       , DataHelper.CheckNull(category.Description)
																   , 0);

			proc.Execute();
			return (int)proc.OutputValues[0];
		}

		/// <summary>
		/// Deletes the link category.
		/// </summary>
		/// <param name="categoryId">The category ID.</param>
		/// <returns></returns>
		public override void DeleteLinkCategory(int categoryId)
		{
			StoredProcedures.DeleteCategory(DataHelper.CheckNull(categoryId), BlogId).Execute();
		}

		/// <summary>
		/// Deletes the link.
		/// </summary>
		/// <param name="linkId">The link ID.</param>
		/// <returns></returns>
		public override void DeleteLink(int linkId)
		{
			StoredProcedures.DeleteLink(linkId, BlogId).Execute();
		}

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
		public override IPagedCollection<Referrer> GetPagedReferrers(int pageIndex, int pageSize, int entryId)
		{
			using (IDataReader reader = StoredProcedures.GetPageableReferrers(BlogId, DataHelper.CheckNull(entryId), pageIndex, pageSize).GetReader())
			{
				return LoadPagedReferrersCollection(reader);
			}
		}

		private static IPagedCollection<Referrer> LoadPagedReferrersCollection(IDataReader reader)
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

		/// <summary>
		/// Tracks the entry.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns></returns>
		public override void TrackEntry(EntryView view)
		{
			StoredProcedures.TrackEntry(DataHelper.CheckNull(view.EntryId)
				, DataHelper.CheckNull(view.BlogId)
				, view.ReferralUrl
				, view.PageViewType == PageViewType.WebView).Execute();
		}

		public override bool TrackEntry(IEnumerable<EntryView> views)
		{
			if (views != null)
			{
				foreach (EntryView view in views)
				{
					TrackEntry(view);
				}
				return true;
			}

			return false;
		}

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
		/// <param name="owner">The blog owner</param>
		/// <returns></returns>
		public override BlogInfo CreateBlog(string title, string host, string subfolder, MembershipUser owner)
		{
			using (IDataReader reader = StoredProcedures.UTILITYAddBlog(title
				, host
				, subfolder
				, (Guid)owner.ProviderUserKey
				, DateTime.UtcNow).GetReader())
			{
				if (reader.Read())
					return DataHelper.LoadBlog(reader);
				return null;
			}
		}

		/// <summary>
		/// Updates the specified blog configuration.
		/// </summary>
		/// <param name="info">Config.</param>
		/// <returns></returns>
		public override void UpdateBlog(BlogInfo info)
		{
			int? daysTillCommentsClose = null;
			if (info.DaysTillCommentsClose > -1 && info.DaysTillCommentsClose < int.MaxValue)
			{
				daysTillCommentsClose = info.DaysTillCommentsClose;
			}

			int? commentDelayInMinutes = null;
			if (info.CommentDelayInMinutes > 0 && info.CommentDelayInMinutes < int.MaxValue)
			{
				commentDelayInMinutes = info.CommentDelayInMinutes;
			}

			int? numberOfRecentComments = null;
			if (info.NumberOfRecentComments > 0 && info.NumberOfRecentComments < int.MaxValue)
			{
				numberOfRecentComments = info.NumberOfRecentComments;
			}

			int? recentCommentsLength = null;
			if (info.RecentCommentsLength > 0 && info.RecentCommentsLength < int.MaxValue)
			{
				recentCommentsLength = info.RecentCommentsLength;
			}

			Guid? ownerId = info.Owner == null ? null : (Guid?)info.Owner.ProviderUserKey;

			StoredProcedures.UpdateConfig(ownerId
			                              , info.Title
			                              , info.SubTitle
			                              , info.Skin.TemplateFolder
			                              , info.Subfolder
			                              , info.Host
			                              , info.Language
			                              , info.TimeZoneId
			                              , info.ItemCount
			                              , info.CategoryListPostCount
			                              , info.News
										  , info.CustomMetaTags
										  , info.TrackingCode
			                              , info.LastUpdated
			                              , DataHelper.CheckNull(info.Skin.CustomCssText)
			                              , DataHelper.CheckNull(info.Skin.SkinStyleSheet)
			                              , (int)info.Flag
			                              , DataHelper.CheckNull(info.Id)
			                              , info.LicenseUrl
										  , daysTillCommentsClose
										  , commentDelayInMinutes
										  , numberOfRecentComments
										  , recentCommentsLength
			                              , DataHelper.ReturnNullIfEmpty(info.FeedbackSpamServiceKey)
			                              , DataHelper.ReturnNullIfEmpty(info.FeedBurnerName)
			                              , info.pop3User
			                              , info.pop3Pass
			                              , info.pop3Server
			                              , info.pop3StartTag
			                              , info.pop3EndTag
			                              , info.pop3SubjectPrefix
			                              , info.pop3MTBEnable
			                              , info.pop3DeleteOnlyProcessed
			                              , info.pop3InlineAttachedPictures
			                              , info.pop3HeightForThumbs).Execute();
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
			using(IDataReader reader = StoredProcedures.GetBlog(hostname, subfolder, strict).GetReader())
			{
				BlogInfo info = null;
				while (reader.Read())
				{
					info = DataHelper.LoadBlog(reader);
					break;
				}
				return info;
			}
		}
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
        public override IDictionary<string, int> GetTopTags(int itemCount)
        {
			using (IDataReader reader = StoredProcedures.GetTopTags(itemCount, BlogId).GetReader())
            {
                IDictionary<string, int> tags = DataHelper.LoadTags(reader);
                return tags;
            }
        }

        #endregion

		#region MetaTags

	    public override int Create(MetaTag metaTag)
	    {
			StoredProcedure proc = StoredProcedures.InsertMetaTag(metaTag.Content, metaTag.Name, metaTag.HttpEquiv, metaTag.BlogId, metaTag.EntryId, metaTag.DateCreated, null);
	    	proc.Execute();
	    	return (int)proc.OutputValues[0];
	    }


	    public override bool Update(MetaTag metaTag)
	    {
	    	StoredProcedures.UpdateMetaTag(metaTag.Id, metaTag.Content, metaTag.Name, metaTag.HttpEquiv, metaTag.BlogId,
	    	                               metaTag.EntryId).Execute();
	    	return true;
	    }

	    public override IList<MetaTag> GetMetaTagsForBlog(BlogInfo blog)
		{
			using (IDataReader reader = StoredProcedures.GetMetaTagsForBlog(blog.Id).GetReader())
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
	        using (IDataReader reader = StoredProcedures.GetMetaTagsForEntry(Config.CurrentBlog.Id, entry.Id).GetReader())
	        {
	            List<MetaTag> tags = new List<MetaTag>();

                while (reader.Read())
                {
                    tags.Add(DataHelper.LoadMetaTag(reader));
                }

	            return tags;
	        }
	    }
	    #endregion


        #region KeyWords

		/// <summary>
		/// Gets the key word by its id.
		/// </summary>
		/// <param name="keyWordId">The key word id.</param>
		/// <returns></returns>
        public override KeyWord GetKeyword(int keyWordId)
		{
			using(IDataReader reader = StoredProcedures.GetKeyWord(keyWordId, BlogId).GetReader())
			{
				KeyWord kw = null;
				while (reader.Read())
				{
					kw = DataHelper.LoadKeyWord(reader);
					break;
				}
				return kw;
			}
		}

		/// <summary>
		/// Gets the key words.
		/// </summary>
		/// <returns></returns>
		public override ICollection<KeyWord> GetKeywords()
		{
			using(IDataReader reader = StoredProcedures.GetBlogKeyWords(BlogId).GetReader())
			{
				List<KeyWord> kwc = new List<KeyWord>();
				while (reader.Read())
				{
					kwc.Add(DataHelper.LoadKeyWord(reader));
				}
				return kwc;
			}
		}

		/// <summary>
		/// Gets the key words by page.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
		public override IPagedCollection<KeyWord> GetKeywordsByPage(int pageIndex, int pageSize)
		{
			using(IDataReader reader = StoredProcedures.GetPageableKeyWords(BlogId, pageIndex, pageSize).GetReader())
			{
				IPagedCollection<KeyWord> pkwc = new PagedCollection<KeyWord>();
				while (reader.Read())
				{
					pkwc.Add(DataHelper.LoadKeyWord(reader));
				}
				reader.NextResult();
				pkwc.MaxItems = DataHelper.GetMaxItems(reader);

				return pkwc;
			}
		}

		/// <summary>
		/// Updates the keyword.
		/// </summary>
		/// <param name="keyword">The keyword.</param>
		/// <returns></returns>
		public override void UpdateKeyword(KeyWord keyword)
		{
			StoredProcedures.UpdateKeyWord(keyword.Id
			                               , keyword.Word
			                               , keyword.Rel
			                               , keyword.Text
			                               , keyword.ReplaceFirstTimeOnly
			                               , keyword.OpenInNewWindow
			                               , keyword.CaseSensitive
			                               , keyword.Url
			                               , keyword.Title
			                               , BlogId).Execute();

		}

		/// <summary>
		/// Inserts the keyword.
		/// </summary>
		/// <param name="keyWord">The key word.</param>
		/// <returns></returns>
		public override int InsertKeyword(KeyWord keyWord)
		{
			if (keyWord == null)
				throw new ArgumentNullException("keyWord", Resources.ArgumentNull_Generic);

			StoredProcedure proc = StoredProcedures.InsertKeyWord(keyWord.Word
			                               , keyWord.Rel
			                               , keyWord.Text
			                               , keyWord.ReplaceFirstTimeOnly
			                               , keyWord.OpenInNewWindow
			                               , keyWord.CaseSensitive
			                               , keyWord.Url
			                               , keyWord.Title
			                               , BlogId
			                               , 0);
			proc.Execute();
			return (int) proc.OutputValues[0];
		}

		/// <summary>
		/// Deletes the keyword.
		/// </summary>
		/// <param name="id">The id.</param>
		public override void DeleteKeyword(int id)
		{
			StoredProcedures.DeleteKeyWord(id, BlogId).Execute();
		}

		#endregion

		#region Images

		/// <summary>
		/// Gets the images by category ID.
		/// </summary>
		/// <param name="categoryId">The cat ID.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override ImageCollection GetImagesByCategoryID(int categoryId, bool activeOnly)
		{
			using (IDataReader reader = StoredProcedures.GetImageCategory(DataHelper.CheckNull(categoryId), activeOnly, BlogId).GetReader())
			{
				ImageCollection ic = new ImageCollection();
				while (reader.Read())
				{
					ic.Category = DataHelper.LoadLinkCategory(reader);
					break;
				}
				reader.NextResult();
				while (reader.Read())
				{
					ic.Add(DataHelper.LoadImage(reader));
				}
				return ic;
			}
		}

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="imageId">The image ID.</param>
		/// <param name="activeOnly">if set to <c>true</c> [active only].</param>
		/// <returns></returns>
		public override Image GetImage(int imageId, bool activeOnly)
		{
			using(IDataReader reader = StoredProcedures.GetSingleImage(imageId, activeOnly, BlogId).GetReader())
			{
				Image image = null;
				while (reader.Read())
				{
					image = DataHelper.LoadImage(reader);
				}
				return image;
			}
		}

		/// <summary>
		/// Inserts the image and returns the id.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		public override int InsertImage(Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image", Resources.ArgumentNull_Generic);

			StoredProcedure proc = StoredProcedures.InsertImage(image.Title
				, DataHelper.CheckNull(image.CategoryID)
				, image.Width
				, image.Height
				, image.FileName
				, image.IsActive
				, BlogId
				, 0);
			proc.Execute();
			return (int) proc.OutputValues[0];
		}

		/// <summary>
		/// Updates the image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <returns></returns>
		public override void UpdateImage(Image image)
		{
			StoredProcedures.UpdateImage(image.Title
			                             , DataHelper.CheckNull(image.CategoryID)
			                             , image.Width
			                             , image.Height
			                             , image.FileName
			                             , image.IsActive
			                             , BlogId
			                             , image.ImageID).Execute();
		}

		/// <summary>
		/// Deletes the image.
		/// </summary>
		/// <param name="imageId">The image id.</param>
		public override void DeleteImage(int imageId)
		{
			StoredProcedures.DeleteImage(BlogId, imageId).Execute();
		}

		#endregion

		#region Plugins

		/// <summary>
		/// Returns the Guids of all plugins enabled for the current blog
		/// </summary>
		/// <returns>A list of Guids</returns>
		public override ICollection<Guid> GetEnabledPlugins()
		{
			if (BlogId == NullValue.NullInt32)
				return new List<Guid>();

			using(IDataReader reader = StoredProcedures.GetPluginBlog(BlogId).GetReader())
			{
				List<Guid> plc = new List<Guid>();
				while (reader.Read())
				{
					plc.Add(reader.GetGuid(reader.GetOrdinal("PluginId")));
				}
				return plc;
			}
		}

		/// <summary>
		/// Enable a plugin for the current blog
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin to enable</param>
		public override void EnablePlugin(Guid pluginId)
		{
			StoredProcedures.InsertPluginBlog(pluginId, BlogId).Execute();
		}

		/// <summary>
		/// Disable a plugin for the current blog
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin to disable</param>
		public override void DisablePlugin(Guid pluginId)
		{
			StoredProcedures.DeletePluginBlog(pluginId, BlogId).Execute();
		}

		/// <summary>
		/// Returns a list of all the blog level settings defined for a plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <returns>A strongly typed HashTable with settings</returns>
		public override NameValueCollection GetPluginBlogSettings(Guid pluginId)
		{
			return GetPluginSettings(pluginId, null);
		}

		private static NameValueCollection GetPluginSettings(Guid pluginId, int? entryId)
		{
			using (IDataReader reader = StoredProcedures.GetPluginData(pluginId, BlogId, entryId).GetReader())
			{
				NameValueCollection dict = new NameValueCollection();
				while (reader.Read())
				{
					dict.Add(DataHelper.LoadPluginSettings(reader));
				}
				return dict;
			}
		}

		/// <summary>
		/// Add a new blog level settings for the plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <param name="key">Key identifying the setting</param>
		/// <param name="value">Value of the setting</param>
		/// <returns>
		/// True if the operation completed correctly, false otherwise
		/// </returns>
		public override void InsertPluginBlogSettings(Guid pluginId, string key, string value)
		{
			StoredProcedures.InsertPluginData(pluginId, BlogId, null, key, value).Execute();
		}

		/// <summary>
		/// Update a blog level settings for the plugin
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <param name="key">Key identifying the setting</param>
		/// <param name="value">New value of the setting</param>
		public override void UpdatePluginBlogSettings(Guid pluginId, string key, string value)
		{
			StoredProcedures.UpdatePluginData(pluginId, BlogId, null, key, value).Execute();
		}

		/// <summary>
		/// Retrieves plugin settings for a specified entry from the storage
		/// </summary>
		/// <param name="pluginId">GUID of the plugin</param>
		/// <param name="entryId">Id of the blog entry</param>
		/// <returns>
		/// A NameValueCollection with all the settings
		/// </returns>
        public override NameValueCollection GetPluginEntrySettings(Guid pluginId, int entryId)
        {
            return GetPluginSettings(pluginId, entryId);
        }

		/// <summary>
		/// Inserts a new value in the plugin settings list for a specified entry
		/// </summary>
		/// <param name="pluginId">GUID of the plugin</param>
		/// <param name="entryId">Id of the blog entry</param>
		/// <param name="key">Setting name</param>
		/// <param name="value">Setting value</param>
        public override void InsertPluginEntrySettings(Guid pluginId, int entryId, string key, string value)
        {
			StoredProcedures.InsertPluginData(pluginId, BlogId, entryId, key, value).Execute();
        }

		/// <summary>
		/// Updates a plugin setting for a specified entry
		/// </summary>
		/// <param name="pluginId">The Guid of the plugin</param>
		/// <param name="entryId">Id of the blog entry</param>
		/// <param name="key">Key identifying the setting</param>
		/// <param name="value">New value of the setting</param>
        public override void UpdatePluginEntrySettings(Guid pluginId, int entryId, string key, string value)
        {
			StoredProcedures.UpdatePluginData(pluginId, BlogId, entryId, key, value).Execute();
        }


		#endregion

		#region Admin

		/// <summary>
		/// Gets the specified page of log entries.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
		public override IDataReader GetPagedLogEntries(int pageIndex, int pageSize)
		{
			return StoredProcedures.GetPageableLogEntries(BlogId, pageIndex, pageSize).GetReader();
		}

		/// <summary>
		/// Clears the log.
		/// </summary>
		public override void ClearLog()
		{
			StoredProcedures.LogClear(BlogId).Execute();
		}

		/// <summary>
		/// Clears all content (Entries, Comments, Track/Ping-backs, Statistices, etc...)
		/// for a the current blog (sans the Image Galleries).
		/// </summary>
		/// <returns>
		/// TRUE - At least one unit of content was cleared.
		/// FALSE - No content was cleared.
		/// </returns>
		public override void ClearBlogContent()
		{
			StoredProcedures.ClearBlogContent(BlogId).Execute();
		}

		#endregion

		/// <summary>
		/// Gets the posts by month archive.
		/// </summary>
		/// <returns></returns>
		public override ICollection<ArchiveCount> GetPostCountByMonth()
		{
			//TODO: We ought to be able to do this with a single proc and a clever parameter.
			//		Either a bitmask or a date param that is the prototype for  the records to return.
			using (IDataReader reader = StoredProcedures.GetPostsByMonthArchive(BlogId).GetReader())
			{
				ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
		}

		/// <summary>
		/// Gets the posts by year archive.
		/// </summary>
		/// <returns></returns>
		public override ICollection<ArchiveCount> GetPostCountByYear()
		{
			using (IDataReader reader = StoredProcedures.GetPostsByYearArchive(BlogId).GetReader())
			{
				ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
		}

		/// <summary>
		/// Gets the posts by category archive.
		/// </summary>
		/// <returns></returns>
		public override ICollection<ArchiveCount> GetPostCountByCategory()
		{
			using (IDataReader reader = StoredProcedures.GetPostsByCategoriesArchive(BlogId).GetReader())
			{
				ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
		}
		
		#region Aggregate Data

		/// <summary>
		/// Returns data displayed on an aggregate blog's home page.
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		public override DataSet GetAggregateHomePageData(int groupId)
		{
			//TODO: Do not pull the aggregate host setting directly from appsettings. This should come from some other class. Maybe a HostSettings or HostInfo.Current.
			return StoredProcedures.DNWHomePageData(ConfigurationManager.AppSettings["AggregateHost"], groupId).GetDataSet();
		}

		/// <summary>
		/// Gets the aggregate recent posts.
		/// </summary>
		/// <param name="groupId">The group id.</param>
		/// <returns></returns>
		public override DataTable GetAggregateRecentPosts(int groupId)
		{
			//TODO: Do not pull the aggregate host setting directly from appsettings. This should come from some other class. Maybe a HostSettings or HostInfo.Current.
			DataSet ds = StoredProcedures.DNWGetRecentPosts(ConfigurationManager.AppSettings["AggregateHost"], groupId).GetDataSet();
			if (ds != null && ds.Tables.Count > 0)
				return ds.Tables[0];
			
			return null;
		}

		#endregion
	}
}
