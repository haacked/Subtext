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
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Security;
using log4net;
using Microsoft.ApplicationBlocks.Data;
using SubSonic;
using Subtext.Data;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
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
		private readonly static ILog log = new Log();

		private static SqlParameter BlogIdParam
		{
			get
			{
				return DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, DataHelper.CheckNull(BlogId));
			}
		}

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
			string sql = "subtext_GetPageableBlogs";

			SqlConnection conn = new SqlConnection(ConnectionString);

			SqlCommand command = new SqlCommand(sql, conn);

			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, host));
			command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(DataHelper.MakeInParam("@ConfigurationFlags", SqlDbType.Int, 4, flags));

			IDataReader reader;

			try
			{
				conn.Open();
				reader = command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (SqlException)
			{
				conn.Open();
				//If we were upgrading, we need to call the old version of the stored proc.
				pageIndex++;
				command.Parameters.Clear();
				command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
				command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
				command.Parameters.Add(DataHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, 0));
				reader = command.ExecuteReader(CommandBehavior.CloseConnection);
			}

			using (reader)
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
			string sql = "subtext_GetBlogById";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);

			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, DataHelper.CheckNull(blogId)));

			conn.Open();

			using (IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
			{
				if (reader.Read())
				{
					BlogInfo info = DataHelper.LoadBlog(reader);
					reader.Close();
					return info;
				}
				reader.Close();
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
		/// <param name="categoryID">The category ID.</param>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
		public override IPagedCollection<Entry> GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize)
		{
			// default setup is for unfiltered pageable results
			bool useCategoryID = categoryID > 0;

			string sql = useCategoryID ? "subtext_GetPageableEntriesByCategoryID" : "subtext_GetPageableEntries";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);
			command.CommandType = CommandType.StoredProcedure;

			command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(DataHelper.MakeInParam("@PostType", SqlDbType.Int, 4, postType));
			command.Parameters.Add(BlogIdParam);

			if (useCategoryID)
			{
				command.Parameters.Add(DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(categoryID)));
			}
			conn.Open();

			IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
			try
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
		public override IPagedCollection<FeedbackItem> GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlags status, FeedbackStatusFlags excludeStatusMask, FeedbackType type)
		{
			object feedbackType = type;
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
					ed.Add(DataHelper.LoadEntry(reader));
				}
				return ed;
			}
		}

		/// <summary>
		/// Returns blog posts that meet the criteria specified in the <see cref="PostConfig"/> flags.
		/// </summary>
		/// <param name="itemCount">Item count.</param>
		/// <param name="pc">Pc.</param>
		/// <returns></returns>
		public override ICollection<EntryDay> GetBlogPosts(int itemCount, PostConfig pc)
		{
			IDataReader reader = GetConditionalEntriesData(itemCount, PostType.BlogPost, pc, false);
			try
			{
				ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader, true);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override ICollection<EntryDay> GetPostsByMonth(int month, int year)
		{
			IDataReader reader = GetPostDataByMonth(month, year);
			try
			{
				ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader, true);
				return edc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override ICollection<EntryDay> GetPostsByCategoryID(int itemCount, int catID)
		{
			IDataReader reader = GetEntriesDataByCategory(itemCount, catID, true);
			try
			{
				ICollection<EntryDay> edc = DataHelper.LoadEntryDayCollection(reader, true);
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
		/// <param name="postType"></param>
		/// <returns></returns>
		/// <param name="postType"></param>
		public override IList<Entry> GetPreviousAndNextEntries(int entryId, PostType postType)
		{
			SqlParameter[] p =
					{
						DataHelper.MakeInParam("@ID", SqlDbType.Int, 4, entryId),
						BlogIdParam
					};

			using (IDataReader reader =
				SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "Subtext_GetEntry_PreviousNext", p))
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
			using (IDataReader reader = GetConditionalEntriesData(itemCount, postType, postConfig, includeCategories))
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
			using (IDataReader reader = GetPostDataByMonth(month, year))
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

			SqlParameter outParam = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 256, feedbackItem.Title), 
				DataHelper.MakeInParam("@Body", DataHelper.CheckNull(feedbackItem.Body)), 
				DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, DataHelper.CheckNull(feedbackItem.EntryId)),
				DataHelper.MakeInParam("@Author", SqlDbType.NVarChar, 64, DataHelper.CheckNull(feedbackItem.Author)), 
				DataHelper.MakeInParam("@IsBlogAuthor", SqlDbType.Bit, 1, feedbackItem.IsBlogAuthor),
				DataHelper.MakeInParam("@Email", SqlDbType.VarChar, 128, DataHelper.CheckNull(feedbackItem.Email)), 
				DataHelper.MakeInParam("@Url", SqlDbType.VarChar, 256, sourceUrl), 
				DataHelper.MakeInParam("@FeedbackType", SqlDbType.Int, 4, (int)feedbackItem.FeedbackType),
				DataHelper.MakeInParam("@StatusFlag", SqlDbType.Int, 4, (int)feedbackItem.Status),
				DataHelper.MakeInParam("@CommentAPI", SqlDbType.Bit, 1, feedbackItem.CreatedViaCommentAPI),
				DataHelper.MakeInParam("@Referrer", SqlDbType.NVarChar, 256, feedbackItem.Referrer), 
				DataHelper.MakeInParam("@IpAddress", SqlDbType.VarChar, 16, ipAddress),
				DataHelper.MakeInParam("@UserAgent", SqlDbType.NVarChar, 128, feedbackItem.UserAgent), 
				DataHelper.MakeInParam("@FeedbackChecksumHash", SqlDbType.VarChar, 32, feedbackItem.ChecksumHash), 
				DataHelper.MakeInParam("@DateCreated", SqlDbType.DateTime, 8, Config.CurrentBlog.TimeZone.Now),
				BlogIdParam,
				outParam
			};

			NonQueryInt("subtext_InsertFeedback", p);
			return (int)outParam.Value;
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
				throw new ArgumentNullException("link", Resources.ArgumentNull_Generic);

			SqlParameter outIdParam = DataHelper.MakeOutParam("@ID", SqlDbType.Int, 4);

			MembershipUser author = Membership.GetUser();
			Guid authorId = (Guid)(author ?? Config.CurrentBlog.Owner).ProviderUserKey;

			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
				//TODO: Maybe the author should be set as a property of Entry.
				DataHelper.MakeInParam("@AuthorId", SqlDbType.UniqueIdentifier, 16, authorId),
				DataHelper.MakeInParam("@Text", SqlDbType.NText, 0, entry.Body), 
				DataHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
				DataHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, DataHelper.CheckNull(entry.Description)), 
				DataHelper.MakeInParam("@DateAdded", SqlDbType.DateTime, 8, entry.DateCreated), 
				DataHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
				DataHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, StringHelper.ReturnNullForEmpty(entry.EntryName)), 
				DataHelper.MakeInParam("@DateSyndicated", SqlDbType.DateTime, 8, DataHelper.CheckNull(entry.DateSyndicated)), 
				BlogIdParam,
				outIdParam
			};

			NonQueryInt("subtext_InsertEntry", p);
			return (int)outIdParam.Value;
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
			string sourceUrl = null;
			if (feedbackItem.SourceUrl != null)
				sourceUrl = feedbackItem.SourceUrl.ToString();

			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Id",  SqlDbType.Int, 4, feedbackItem.Id), 
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 256, feedbackItem.Title), 
				DataHelper.MakeInParam("@Body", DataHelper.CheckNull(feedbackItem.Body)), 
				DataHelper.MakeInParam("@Author", SqlDbType.NVarChar, 64, DataHelper.CheckNull(feedbackItem.Author)), 
				DataHelper.MakeInParam("@Email", SqlDbType.VarChar, 128, DataHelper.CheckNull(feedbackItem.Email)), 
				DataHelper.MakeInParam("@Url", SqlDbType.VarChar, 256, sourceUrl), 
				DataHelper.MakeInParam("@StatusFlag", SqlDbType.Int, 4, (int)feedbackItem.Status),
				DataHelper.MakeInParam("@FeedbackChecksumHash", SqlDbType.VarChar, 32, feedbackItem.ChecksumHash), 
				DataHelper.MakeInParam("@DateModified", SqlDbType.DateTime, 8, Config.CurrentBlog.TimeZone.Now),
			};
			return NonQueryBool("subtext_UpdateFeedback", p);
		}

		/// <summary>
		/// Saves changes to the specified entry attaching the specified categories.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <param name="categoryIds">Category Ids.</param>
		/// <returns></returns>
		public override bool Update(Entry entry, params int[] categoryIds)
		{
			FormatEntry(entry, false);

			if(!UpdateEntry(entry))
			{
				return false;
			}

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
			return true;
		}

		/// <summary>
		/// Updates the specified entry in the database.
		/// </summary>
		/// <remarks>
		/// The method <see cref="SetEntryCategoryList" /> is used to save the entry's categories.
		/// </remarks>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		private bool UpdateEntry(Entry entry)
		{
			MembershipUser author = Membership.GetUser();
			Guid authorId = (Guid)(author ?? Config.CurrentBlog.Owner).ProviderUserKey;

			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ID", SqlDbType.Int, 4, entry.Id), 
				DataHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
				DataHelper.MakeInParam("@Text", SqlDbType.NText, 0, entry.Body), 
				DataHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
				DataHelper.MakeInParam("@AuthorId", SqlDbType.UniqueIdentifier, 16, authorId), 
				DataHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, DataHelper.CheckNull(entry.Description)), 
				DataHelper.MakeInParam("@DateUpdated", SqlDbType.DateTime, 4, entry.DateModified), 
				DataHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
				DataHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, DataHelper.CheckNull(entry.EntryName)), 
				DataHelper.MakeInParam("@DateSyndicated", SqlDbType.DateTime, 8, DataHelper.CheckNull(entry.DateSyndicated)), 
				BlogIdParam
			};
			return NonQueryBool("subtext_UpdateEntry", p);
		}

		#endregion

		#region SetCategoriesList

		public override bool SetEntryCategoryList(int entryId, int[] categoryIds)
		{
			if (categoryIds == null || categoryIds.Length == 0)
			{
				return false;
			}

			string[] cats = new string[categoryIds.Length];
			for (int i = 0; i < categoryIds.Length; i++)
			{
				cats[i] = categoryIds[i].ToString(CultureInfo.InvariantCulture);
			}
			string catList = string.Join(",", cats);

			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@PostID", SqlDbType.Int, 4, DataHelper.CheckNull(entryId)),
				BlogIdParam,
				DataHelper.MakeInParam("@CategoryList", SqlDbType.NVarChar, 4000, catList)
			};
			return NonQueryBool("subtext_InsertLinkCategoryList", p);
		}

		#endregion

        #region SetTagList

        public override bool SetEntryTagList(int entryId, List<string> tags)
        {
			if (tags == null)
				throw new ArgumentNullException("tags", "Tags cannot be null.");

			SqlParameter tagParam = new SqlParameter("@TagList", SqlDbType.NText);
			tagParam.Value = string.Join(",", tags.ToArray());

			SqlParameter[] p =
                {
                    DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, DataHelper.CheckNull(entryId)),
                    BlogIdParam,
                    tagParam
                };
			return NonQueryBool("subtext_InsertEntryTagList", p);
        }

        #endregion

		private static void FormatEntry(Entry e, bool UseKeyWords)
		{
			//Do this before we validate the text
			if (UseKeyWords)
			{
				KeyWords.Format(e);
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

		public override IPagedCollection<Link> GetPagedLinks(int categoryTypeID, int pageIndex, int pageSize, bool sortDescending)
		{
			bool useCategory = categoryTypeID > -1;
			string sql = "subtext_GetPageableLinks";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);
			command.CommandType = CommandType.StoredProcedure;

			command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(BlogIdParam);

			if (useCategory)
			{
				command.Parameters.Add(DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(categoryTypeID)));
			}

			conn.Open();

			IDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
			try
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
			finally
			{
				reader.Close();
			}

		}

		#endregion

		#region LinkCollection

		public override ICollection<Link> GetLinkCollectionByPostID(int postID)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PostID", SqlDbType.Int, 4, DataHelper.CheckNull(postID)),
				BlogIdParam
			};

			IDataReader reader = GetReader("subtext_GetLinkCollectionByPostID", p);
			try
			{
				ICollection<Link> lc = new List<Link>();
				while (reader.Read())
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

		public override ICollection<Link> GetLinksByCategoryID(int catID, bool activeOnly)
		{
			string sql = activeOnly ? "subtext_GetLinksByActiveCategoryID" : "subtext_GetLinksByCategoryID";
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4 ,DataHelper.CheckNull(catID)),
				BlogIdParam
			};

			IDataReader reader = GetReader(sql, p);
			List<Link> lc = new List<Link>();
			try
			{
				while (reader.Read())
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
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@LinkID",SqlDbType.Int,4,linkID),
				BlogIdParam
			};

			IDataReader reader = GetReader("subtext_GetSingleLink", p);
			try
			{
				Link link = null;
				while (reader.Read())
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

		#region ICollection<LinkCategory>

		public override ICollection<LinkCategory> GetCategories(CategoryType catType, bool activeOnly)
		{
			SqlParameter[] p ={DataHelper.MakeInParam("@CategoryType", SqlDbType.TinyInt, 1, catType),
							  DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, activeOnly),
								  BlogIdParam};

			using (IDataReader reader = GetReader("subtext_GetCategory", p))
			{
				ICollection<LinkCategory> lcc = new List<LinkCategory>();
				while (reader.Read())
				{
					lcc.Add(DataHelper.LoadLinkCategory(reader));
				}
				return lcc;
			}
		}

		public override ICollection<LinkCategory> GetActiveCategories()
		{
			SqlParameter[] p ={ BlogIdParam };
			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "subtext_GetActiveCategoriesWithLinkCollection", p);

			DataRelation dl = new DataRelation("CategoryID", ds.Tables[0].Columns["CategoryID"], ds.Tables[1].Columns["CategoryID"], false);
			ds.Relations.Add(dl);

			ICollection<LinkCategory> lcc = new List<LinkCategory>();
			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				LinkCategory lc = DataHelper.LoadLinkCategory(dr);
				lc.Links = new List<Link>();
				foreach (DataRow drLink in dr.GetChildRows("CategoryID"))
				{
					lc.Links.Add(DataHelper.LoadLink(drLink));
				}
				lcc.Add(lc);
			}
			return lcc;
		}

		#endregion

		#region LinkCategory

		public override LinkCategory GetLinkCategory(int categoryId, bool activeOnly)
		{
			using (IDataReader reader = GetLinkCategoryGeneric(categoryId, activeOnly))
			{
				if (reader.Read())
				{
					LinkCategory lc = DataHelper.LoadLinkCategory(reader);
					return lc;
				}
				return null;
			}
		}

		public override LinkCategory GetLinkCategory(string categoryName, bool activeOnly)
		{
			using (IDataReader reader = GetLinkCategoryGeneric(categoryName, activeOnly))
			{
				if (reader.Read())
				{
					LinkCategory lc = DataHelper.LoadLinkCategory(reader);
					return lc;
				}
				return null;		
			}
		}

		/// <summary>
		/// Returns a data reader for the specified category. The Category Key should either 
		/// be an Int (category id) or a string (category name).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="categoryKey"></param>
		/// <param name="isActive"></param>
		/// <returns></returns>
		private IDataReader GetLinkCategoryGeneric<T>(T categoryKey, bool isActive)
		{
			SqlParameter id = DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, null);
			SqlParameter name = DataHelper.MakeInParam("@CategoryName", SqlDbType.NVarChar, 150, null);

			if (typeof(T) == typeof(int))
				id.Value = categoryKey;
			if (typeof(T) == typeof(string))
				name.Value = categoryKey;

			SqlParameter[] p = 
			{
				id,
				name,
				DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, isActive),
				BlogIdParam
			};
			return GetReader("subtext_GetCategory", p);
		}


		#endregion

		#region Edit Links/Categories

		public override bool UpdateLink(Link link)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,link.Title),
				DataHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,link.Url),
				DataHelper.MakeInParam("@Rss",SqlDbType.NVarChar,255,DataHelper.CheckNull(link.Rss)),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,link.IsActive),
				DataHelper.MakeInParam("@NewWindow",SqlDbType.Bit,1,link.NewWindow),
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(link.CategoryID)),
				DataHelper.MakeInParam("@LinkID",SqlDbType.Int,4,link.Id),
				BlogIdParam
			};
			return NonQueryBool("subtext_UpdateLink", p);
		}

		public override int CreateLink(Link link)
		{
			if (link == null)
				throw new ArgumentNullException("link", Resources.ArgumentNull_Generic);

			SqlParameter outParam = DataHelper.MakeOutParam("@LinkID", SqlDbType.Int, 4);
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,link.Title),
				DataHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,link.Url),
				DataHelper.MakeInParam("@Rss",SqlDbType.NVarChar,255,DataHelper.CheckNull(link.Rss)),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,link.IsActive),
				DataHelper.MakeInParam("@NewWindow",SqlDbType.Bit,1,link.NewWindow),
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(link.CategoryID)),
				DataHelper.MakeInParam("@PostID", SqlDbType.Int, 4, DataHelper.CheckNull(link.PostID)),
				BlogIdParam,
				outParam
			};
			NonQueryInt("subtext_InsertLink", p);
			return (int)outParam.Value;
		}

		public override bool UpdateLinkCategory(LinkCategory category)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category", Resources.ArgumentNull_Generic);
			}

			SqlParameter[] p =
			{

				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,category.Title),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,category.IsActive),
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(category.Id)),
				DataHelper.MakeInParam("@CategoryType",SqlDbType.TinyInt,1,category.CategoryType),
				DataHelper.MakeInParam("@Description",SqlDbType.NVarChar,1000,DataHelper.CheckNull(category.Description)),
				BlogIdParam
			};
			return NonQueryBool("subtext_UpdateCategory", p);
		}

		public override int CreateLinkCategory(LinkCategory category)
		{
			if (category == null)
				throw new ArgumentNullException("category", Resources.ArgumentNull_Generic);

			SqlParameter outParam = DataHelper.MakeOutParam("@CategoryID", SqlDbType.Int, 4);
			SqlParameter[] p =
			{

				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,category.Title),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,category.IsActive),
				DataHelper.MakeInParam("@CategoryType",SqlDbType.TinyInt,1,category.CategoryType),
				DataHelper.MakeInParam("@Description",SqlDbType.NVarChar,1000,DataHelper.CheckNull(category.Description)),
				BlogIdParam,
				outParam
			};
			NonQueryInt("subtext_InsertCategory", p);
			return (int)outParam.Value;
		}

		public override bool DeleteLinkCategory(int categoryID)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(categoryID)),
				BlogIdParam
			};
			return NonQueryBool("subtext_DeleteCategory", p);
		}

		public override bool DeleteLink(int linkID)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@LinkID", SqlDbType.Int, 4, linkID),
				BlogIdParam
			};
			return NonQueryBool("subtext_DeleteLink", p);
		}

		#endregion

		#endregion

		#region Stats

		public override IPagedCollection<ViewStat> GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			SqlParameter[] p =
			{
				BlogIdParam,
				DataHelper.MakeInParam("@BeginDate", SqlDbType.DateTime, 4, beginDate),
				DataHelper.MakeInParam("@EndDate", SqlDbType.DateTime, 4, endDate),
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};

			IDataReader reader = GetReader("subtext_GetPageableViewStats", p);
			try
			{
				IPagedCollection<ViewStat> vs = new PagedCollection<ViewStat>();
				while (reader.Read())
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
			SqlParameter[] p =
			{
				BlogIdParam,
				DataHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, DataHelper.CheckNull(entryId)),
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};

			IDataReader reader = GetReader("subtext_GetPageableReferrers", p);
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

		public override bool TrackEntry(EntryView view)
		{
			//Note, for the paramater @URL, do NOT convert null values into empty strings.
			SqlParameter[] p =	
			{
						DataHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, DataHelper.CheckNull(view.EntryId)),
						DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, DataHelper.CheckNull(view.BlogId)),
						DataHelper.MakeInParam("@URL", SqlDbType.NVarChar, 255, view.ReferralUrl),
						DataHelper.MakeInParam("@IsWeb", SqlDbType.Bit,1, view.PageViewType)
			};
			try
			{
				return NonQueryBool("subtext_TrackEntry", p);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message + e.StackTrace);
			}
			return false;
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
			using (IDataReader reader = GetReader("subtext_UTILITY_AddBlog", title, host, subfolder, owner.ProviderUserKey, DateTime.UtcNow))
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
		public override bool UpdateBlog(BlogInfo info)
		{
			object daysTillCommentsClose = null;
			if (info.DaysTillCommentsClose > -1 && info.DaysTillCommentsClose < int.MaxValue)
			{
				daysTillCommentsClose = info.DaysTillCommentsClose;
			}

			object commentDelayInMinutes = null;
			if (info.CommentDelayInMinutes > 0 && info.CommentDelayInMinutes < int.MaxValue)
			{
				commentDelayInMinutes = info.CommentDelayInMinutes;
			}

			object numberOfRecentComments = null;
			if (info.NumberOfRecentComments > 0 && info.NumberOfRecentComments < int.MaxValue)
			{
				numberOfRecentComments = info.NumberOfRecentComments;
			}

			object recentCommentsLength = null;
			if (info.RecentCommentsLength > 0 && info.RecentCommentsLength < int.MaxValue)
			{
				recentCommentsLength = info.RecentCommentsLength;
			}

			object ownerId = info.Owner == null ? null : info.Owner.ProviderUserKey;

			SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@BlogId", SqlDbType.Int,  4, DataHelper.CheckNull(info.Id))
					,DataHelper.MakeInParam("@OwnerId", ownerId) 
					,DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 100, info.Title) 
					,DataHelper.MakeInParam("@SubTitle", SqlDbType.NVarChar, 250, info.SubTitle) 
					,DataHelper.MakeInParam("@Skin", SqlDbType.NVarChar, 50, info.Skin.TemplateFolder) 
					,DataHelper.MakeInParam("@Subfolder", SqlDbType.NVarChar, 50, info.CleanSubfolder) 
					,DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, info.Host) 
					,DataHelper.MakeInParam("@TimeZone", SqlDbType.Int, 4, info.TimeZoneId) 
					,DataHelper.MakeInParam("@Language", SqlDbType.NVarChar, 10, info.Language) 
					,DataHelper.MakeInParam("@News", SqlDbType.NText, 0, DataHelper.CheckNull(info.News)) 
					,DataHelper.MakeInParam("@ItemCount", SqlDbType.Int,  4, info.ItemCount) 
					,DataHelper.MakeInParam("@CategoryListPostCount", SqlDbType.Int,  4, info.CategoryListPostCount) 
					,DataHelper.MakeInParam("@Flag", SqlDbType.Int,  4, (int)info.Flag) 
					,DataHelper.MakeInParam("@LastUpdated", SqlDbType.DateTime,  8, info.LastUpdated) 
					,DataHelper.MakeInParam("@SecondaryCss", SqlDbType.Text, 0, DataHelper.CheckNull(info.Skin.CustomCssText)) 
					,DataHelper.MakeInParam("@SkinCssFile", SqlDbType.VarChar, 100, DataHelper.CheckNull(info.Skin.SkinStyleSheet)) 
					,DataHelper.MakeInParam("@LicenseUrl", SqlDbType.NVarChar, 64, info.LicenseUrl)
					,DataHelper.MakeInParam("@DaysTillCommentsClose", SqlDbType.Int, 4, daysTillCommentsClose)
					,DataHelper.MakeInParam("@CommentDelayInMinutes", SqlDbType.Int, 4, commentDelayInMinutes)
					,DataHelper.MakeInParam("@NumberOfRecentComments", SqlDbType.Int, 4, numberOfRecentComments)
					,DataHelper.MakeInParam("@RecentCommentsLength", SqlDbType.Int, 4, recentCommentsLength)
					,DataHelper.MakeInParam("@AkismetAPIKey", SqlDbType.VarChar, 16, DataHelper.ReturnNullIfEmpty(info.FeedbackSpamServiceKey))
					,DataHelper.MakeInParam("@FeedBurnerName", SqlDbType.NVarChar, 64, DataHelper.ReturnNullIfEmpty(info.FeedBurnerName))
					,DataHelper.MakeInParam("@pop3User", SqlDbType.VarChar, 32, info.pop3User)
					,DataHelper.MakeInParam("@pop3Pass", SqlDbType.VarChar, 32, info.pop3Pass)
					,DataHelper.MakeInParam("@pop3Server", SqlDbType.VarChar, 56, info.pop3Server)
					,DataHelper.MakeInParam("@pop3StartTag", SqlDbType.NVarChar, 10, info.pop3StartTag)
					,DataHelper.MakeInParam("@pop3EndTag", SqlDbType.NVarChar, 10, info.pop3EndTag)
					,DataHelper.MakeInParam("@pop3SubjectPrefix", SqlDbType.NVarChar, 10, info.pop3SubjectPrefix)
					,DataHelper.MakeInParam("@pop3MTBEnable", SqlDbType.Bit, 1, info.pop3MTBEnable)
					,DataHelper.MakeInParam("@pop3DeleteOnlyProcessed", SqlDbType.Bit, 1, info.pop3DeleteOnlyProcessed)
					,DataHelper.MakeInParam("@pop3InlineAttachedPictures", SqlDbType.Bit, 1, info.pop3InlineAttachedPictures)
					,DataHelper.MakeInParam("@pop3HeightForThumbs", SqlDbType.Int, 4, info.pop3HeightForThumbs)
				};

			return NonQueryBool("subtext_UpdateConfig", p);
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
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, hostname)
				,DataHelper.MakeInParam("@Subfolder", SqlDbType.NVarChar, 50, subfolder)
				,DataHelper.MakeInParam("@Strict", SqlDbType.Bit, 1, strict)
			};

			IDataReader reader = GetReader("subtext_GetBlog", p);
			try
			{
				BlogInfo info = null;
				while (reader.Read())
				{
					info = DataHelper.LoadBlog(reader);
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
			SqlParameter[] p = 
                {
                    DataHelper.MakeInParam("@ItemCount", SqlDbType.Int, 4, ItemCount),
                    BlogIdParam
                };

			using (IDataReader reader = GetReader("subtext_GetTopTags", p))
            {
                IDictionary<string, int> tags = DataHelper.LoadTags(reader);
                return tags;
            }
        }

        #endregion

        #region KeyWords

        public override KeyWord GetKeyWord(int keyWordId)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@KeyWordID", SqlDbType.Int, 4, keyWordId),
				BlogIdParam
			};

			IDataReader reader = GetReader("subtext_GetKeyWord", p);
			try
			{
				KeyWord kw = null;
				while (reader.Read())
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

		public override ICollection<KeyWord> GetKeyWords()
		{
			SqlParameter[] p =
			{
				BlogIdParam
			};

			IDataReader reader = GetReader("subtext_GetBlogKeyWords", p);
			try
			{
				List<KeyWord> kwc = new List<KeyWord>();
				while (reader.Read())
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
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
				BlogIdParam
			};

			IDataReader reader = GetReader("subtext_GetPageableKeyWords", p);
			try
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
			finally
			{
				reader.Close();
			}
		}

		public override bool UpdateKeyWord(KeyWord keyWord)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@KeyWordID",SqlDbType.Int,4,keyWord.Id),
				DataHelper.MakeInParam("@Word",SqlDbType.NVarChar,100,keyWord.Word),
				DataHelper.MakeInParam("@Text",SqlDbType.NVarChar,100,keyWord.Text),
				DataHelper.MakeInParam("@ReplaceFirstTimeOnly",SqlDbType.Bit,1,keyWord.ReplaceFirstTimeOnly),
				DataHelper.MakeInParam("@OpenInNewWindow",SqlDbType.Bit,1,keyWord.OpenInNewWindow),
				DataHelper.MakeInParam("@CaseSensitive",SqlDbType.Bit,1,keyWord.CaseSensitive),
				DataHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,keyWord.Url),
				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,100,keyWord.Title),
				BlogIdParam
			};
			return NonQueryBool("subtext_UpdateKeyWord", p);
		}

		public override int InsertKeyWord(KeyWord keyWord)
		{
			if (keyWord == null)
				throw new ArgumentNullException("keyword", Resources.ArgumentNull_Generic);

			SqlParameter outParam = DataHelper.MakeOutParam("@KeyWordID", SqlDbType.Int, 4);
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Word", SqlDbType.NVarChar, 100, keyWord.Word)
				,DataHelper.MakeInParam("@Rel", SqlDbType.NVarChar, 100, keyWord.Rel)
				,DataHelper.MakeInParam("@Text", SqlDbType.NVarChar, 100, keyWord.Text)
				,DataHelper.MakeInParam("@ReplaceFirstTimeOnly", SqlDbType.Bit, 1, keyWord.ReplaceFirstTimeOnly)
				,DataHelper.MakeInParam("@OpenInNewWindow", SqlDbType.Bit, 1, keyWord.OpenInNewWindow)
				,DataHelper.MakeInParam("@CaseSensitive", SqlDbType.Bit, 1, keyWord.CaseSensitive)
				,DataHelper.MakeInParam("@Url", SqlDbType.NVarChar, 255, keyWord.Url)
				,DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 100, keyWord.Title)
				,BlogIdParam
				,outParam
			};
			NonQueryInt("subtext_InsertKeyWord", p);
			return (int)outParam.Value;
		}

		public override bool DeleteKeyWord(int id)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@KeyWordID", SqlDbType.Int, 4, id),
				BlogIdParam
			};
			return NonQueryBool("subtext_DeleteKeyWord", p);
		}

		#endregion

		#region Images

		public override ImageCollection GetImagesByCategoryID(int catID, bool activeOnly)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int, 4, DataHelper.CheckNull(catID)),
				DataHelper.MakeInParam("@IsActive",SqlDbType.Bit,1, activeOnly),
				BlogIdParam
			};

			IDataReader reader = GetReader("subtext_GetImageCategory", p);
			try
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
			finally
			{
				reader.Close();
			}
		}

		public override Image GetImage(int imageID, bool activeOnly)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ImageID",SqlDbType.Int,4,imageID),
				DataHelper.MakeInParam("@IsActive",SqlDbType.Bit,1, activeOnly),
				BlogIdParam
			};

			IDataReader reader = GetReader("subtext_GetSingleImage", p);
			try
			{
				Image image = null;
				while (reader.Read())
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

		public override int InsertImage(Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image", Resources.ArgumentNull_Generic);

			SqlParameter outParam = DataHelper.MakeOutParam("@ImageID", SqlDbType.Int, 4);
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,250,image.Title),
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(image.CategoryID)),
				DataHelper.MakeInParam("@Width",SqlDbType.Int,4,image.Width),
				DataHelper.MakeInParam("@Height",SqlDbType.Int,4,image.Height),
				DataHelper.MakeInParam("@File",SqlDbType.NVarChar,50,image.File),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,image.IsActive),
				BlogIdParam,
				outParam
			};
			NonQueryInt("subtext_InsertImage", p);
			return (int)outParam.Value;
		}

		public override bool UpdateImage(Image image)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,250,image.Title),
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(image.CategoryID)),
				DataHelper.MakeInParam("@Width",SqlDbType.Int,4,image.Width),
				DataHelper.MakeInParam("@Height",SqlDbType.Int,4,image.Height),
				DataHelper.MakeInParam("@File",SqlDbType.NVarChar,50,image.File),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,image.IsActive),
				BlogIdParam,
				DataHelper.MakeInParam("@ImageID",SqlDbType.Int,4,image.ImageID)
			};
			return NonQueryBool("subtext_UpdateImage", p);
		}

		public override bool DeleteImage(int imageID)
		{
			SqlParameter[] p = 
			{
				BlogIdParam,
				DataHelper.MakeInParam("@ImageID",SqlDbType.Int,4,imageID)
			};
			return NonQueryBool("subtext_DeleteImage", p);
		}

		#endregion

		#region Archives

		public override ICollection<ArchiveCount> GetPostsByMonthArchive()
		{
			SqlParameter[] p = { BlogIdParam };

			IDataReader reader = GetReader("subtext_GetPostsByMonthArchive", p);
			try
			{
				ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override ICollection<ArchiveCount> GetPostsByYearArchive()
		{
			SqlParameter[] p = { BlogIdParam };

			IDataReader reader = GetReader("subtext_GetPostsByYearArchive", p);
			try
			{
				ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override ICollection<ArchiveCount> GetPostsByCategoryArchive()
		{
			SqlParameter[] p = { BlogIdParam };

			IDataReader reader = GetReader("subtext_GetPostsByCategoriesArchive", p);
			try
			{
				ICollection<ArchiveCount> acc = DataHelper.LoadArchiveCount(reader);
				return acc;
			}
			finally
			{
				reader.Close();
			}
		}

		#endregion

		#region Plugins

		public override ICollection<Guid> GetEnabledPlugins()
		{
			if (BlogIdParam.Value == null)
				return new List<Guid>();

			SqlParameter[] p = { BlogIdParam };

			IDataReader reader = GetReader("subtext_GetPluginBlog", p);
			try
			{
				List<Guid> plc = new List<Guid>();
				while (reader.Read())
				{
					plc.Add(reader.GetGuid(reader.GetOrdinal("PluginId")));
				}
				return plc;
			}
			finally
			{
				reader.Close();
			}
		}

		public override bool EnablePlugin(Guid pluginId)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PluginID",SqlDbType.UniqueIdentifier,16,pluginId),
				BlogIdParam
			};
			return NonQueryBool("subtext_InsertPluginBlog", p);
		}

		public override bool DisablePlugin(Guid pluginId)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PluginID",SqlDbType.UniqueIdentifier,16,pluginId),
				BlogIdParam
			};
			return NonQueryBool("subtext_DeletePluginBlog", p);
		}

		public override NameValueCollection GetPluginBlogSettings(Guid pluginId)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PluginID",SqlDbType.UniqueIdentifier,16,pluginId),
				DataHelper.MakeInParam("@EntryID",SqlDbType.Int,4,DBNull.Value),
				BlogIdParam
			};

			IDataReader reader = GetReader("subtext_GetPluginData", p);
			try
			{
				NameValueCollection dict = new NameValueCollection();
				while (reader.Read())
				{
					dict.Add(DataHelper.LoadPluginSettings(reader));
				}
				return dict;
			}
			finally
			{
				reader.Close();
			}
		}

		public override bool InsertPluginBlogSettings(Guid pluginId, string key, string value)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PluginID",SqlDbType.UniqueIdentifier,16,pluginId),
				DataHelper.MakeInParam("@EntryID",SqlDbType.Int,4,DBNull.Value),
				DataHelper.MakeInParam("@Key",SqlDbType.NVarChar,256,key),
				DataHelper.MakeInParam("@Value",SqlDbType.NText,0,value),
				BlogIdParam
			};
			return NonQueryBool("subtext_InsertPluginData", p);
		}

		public override bool UpdatePluginBlogSettings(Guid pluginId, string key, string value)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PluginID",SqlDbType.UniqueIdentifier,16,pluginId),
				DataHelper.MakeInParam("@EntryID",SqlDbType.Int,4,DBNull.Value),
				DataHelper.MakeInParam("@Key",SqlDbType.NVarChar,256,key),
				DataHelper.MakeInParam("@Value",SqlDbType.NText,0,value),
				BlogIdParam
			};
			return NonQueryBool("subtext_UpdatePluginData", p);
		}


        public override NameValueCollection GetPluginEntrySettings(Guid pluginId, int entryId)
        {
            SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PluginID",SqlDbType.UniqueIdentifier,16,pluginId),
				DataHelper.MakeInParam("@EntryID",SqlDbType.Int,4,entryId),
				BlogIdParam
			};

            IDataReader reader = GetReader("subtext_GetPluginData", p);
            try
            {
                NameValueCollection dict = new NameValueCollection();
                while (reader.Read())
                {
                    dict.Add(DataHelper.LoadPluginSettings(reader));
                }
                return dict;
            }
            finally
            {
                reader.Close();
            }
        }

        public override bool InsertPluginEntrySettings(Guid pluginId, int entryId, string key, string value)
        {
            SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PluginID",SqlDbType.UniqueIdentifier,16,pluginId),
				DataHelper.MakeInParam("@EntryID",SqlDbType.Int,4,entryId),
				DataHelper.MakeInParam("@Key",SqlDbType.NVarChar,256,key),
				DataHelper.MakeInParam("@Value",SqlDbType.NText,0,value),
				BlogIdParam
			};
            return NonQueryBool("subtext_InsertPluginData", p);
        }

        public override bool UpdatePluginEntrySettings(Guid pluginId, int entryId, string key, string value)
        {
            SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PluginID",SqlDbType.UniqueIdentifier,16,pluginId),
				DataHelper.MakeInParam("@EntryID",SqlDbType.Int,4,entryId),
				DataHelper.MakeInParam("@Key",SqlDbType.NVarChar,256,key),
				DataHelper.MakeInParam("@Value",SqlDbType.NText,0,value),
				BlogIdParam
			};
            return NonQueryBool("subtext_UpdatePluginData", p);
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
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
				BlogIdParam
			};
			return GetReader("subtext_GetPageableLogEntries", p);
		}

		/// <summary>
		/// Clears the log.
		/// </summary>
		public override void ClearLog()
		{
			SqlParameter[] p =
			{
				BlogIdParam
			};
			NonQueryInt("subtext_LogClear", p);
		}

		/// <summary>
		/// Clears all content (Entries, Comments, Track/Ping-backs, Statistices, etc...)
		/// for a the current blog (sans the Image Galleries).
		/// </summary>
		/// <returns>
		/// TRUE - At least one unit of content was cleared.
		/// FALSE - No content was cleared.
		/// </returns>
		public override bool ClearBlogContent()
		{
			SqlParameter[] p = { BlogIdParam };
			return NonQueryBool("subtext_ClearBlogContent", p);
		}

		#endregion

		#region Aggregate Data

		public override DataSet GetAggregateHomePageData(int groupId)
		{
			string sql = "DNW_HomePageData";
			SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, ConfigurationManager.AppSettings["AggregateHost"]),
					DataHelper.MakeInParam("@GroupID", SqlDbType.Int, 4, groupId)
				};

			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, sql, p);
		}

		public override DataTable GetAggregateRecentPosts(int groupId)
		{
			string sql = "DNW_GetRecentPosts";
			string conn = ConnectionString;

			SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@Host", SqlDbType.NVarChar,100, ConfigurationManager.AppSettings["AggregateHost"]),
					DataHelper.MakeInParam("@GroupID", SqlDbType.Int, 4, groupId)
				};

			return DataHelper.ExecuteDataTable(conn, CommandType.StoredProcedure, sql, p);
		}

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
		private IDataReader GetConditionalEntriesData(int itemCount, PostType postType, PostConfig postConfiguration, bool includeCategories)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ItemCount", SqlDbType.Int, 4,itemCount),
				DataHelper.MakeInParam("@PostType", SqlDbType.Int, 4, postType),
				DataHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, postConfiguration),
				DataHelper.MakeInParam("@IncludeCategories", SqlDbType.Bit, 0, includeCategories),
				BlogIdParam				
			};

			return GetReader("subtext_GetConditionalEntries", p);
		}

		//Should power both EntryCollection and EntryDayCollection
		private IDataReader GetPostDataByMonth(int month, int year)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Month",SqlDbType.Int,4,month),
				DataHelper.MakeInParam("@Year",SqlDbType.Int,4,year),
				BlogIdParam};
			return GetReader("subtext_GetPostsByMonth", p);
		}

		private IDataReader GetEntriesDataByCategory(int itemCount, int catID, bool activeOnly)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ItemCount", SqlDbType.Int, 4, itemCount),
				DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(catID)),
				DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, activeOnly),
				BlogIdParam
			};
			return GetReader("subtext_GetPostsByCategoryID", p);

		}

		#endregion

		#region Helpers

		private IDataReader GetReader(string sql, SqlParameter[] parameters)
		{
			LogSql(sql, parameters);
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, sql, parameters);
		}

		private IDataReader GetReader(string sql, params object[] parameterValues)
		{
			LogSql(sql, parameterValues);
			return SqlHelper.ExecuteReader(ConnectionString, sql, parameterValues);
		}

		private int NonQueryInt(string sql, SqlParameter[] parameters)
		{
			LogSql(sql, parameters);
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, sql, parameters);
		}

		private bool NonQueryBool(string sql, SqlParameter[] parameters)
		{
			LogSql(sql, parameters);
			return NonQueryInt(sql, parameters) > 0;
		}

		static void LogSql(string sql, object[] parameterValues)
		{
#if DEBUG
			if (parameterValues == null)
			{
				log.Debug("SQL: " + sql);
				return;
			}

			string query = sql + StringHelper.Join(", ", parameterValues, delegate(object item)
			{
				if (item != null)
					return item.ToString();
				return "{NULL}";
			});

			log.Debug("SQL: " + query);

#endif
		}

		static void LogSql(string sql, SqlParameter[] parameters)
		{
#if DEBUG
			if (parameters == null)
			{
				log.Debug("SQL: " + sql);
				return;
			}

			string query = sql + StringHelper.Join(", ", parameters, delegate(SqlParameter item)
			{
				return item.ParameterName + "=" + item.Value;
			});

			log.Debug("SQL: " + query);
#endif
		}

		#endregion
	}
}
