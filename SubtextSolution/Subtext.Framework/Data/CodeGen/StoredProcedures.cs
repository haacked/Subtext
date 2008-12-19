using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using log4net;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;

namespace Subtext.Framework.Data {
	public partial class StoredProcedures {
		public IDataReader GetRecentImages(string host, int? groupId, int rowCount) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@GroupID", groupId),		
				DataHelper.MakeInParam("@rowCount", rowCount),		
			};
			
			return GetReader("DNW_GetRecentImages", p);
		}
		
		public IDataReader GetRecentPosts(string host, int? groupId, DateTime currentDateTime, int? rowCount) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@GroupID", groupId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
				DataHelper.MakeInParam("@RowCount", rowCount),		
			};
			
			return GetReader("DNW_GetRecentPosts", p);
		}
		
		public IDataReader Stats(string host, int? groupId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@GroupID", groupId),		
			};
			
			return GetReader("DNW_Stats", p);
		}
		
		public IDataReader TotalStats(string host, int groupId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@GroupID", groupId),		
			};
			
			return GetReader("DNW_Total_Stats", p);
		}
		
		public bool AddLogEntry(DateTime date, int? blogId, string thread, string context, string level, string logger, string message, string exception, string url) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Date", date),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Thread", thread),		
				DataHelper.MakeInParam("@Context", context),		
				DataHelper.MakeInParam("@Level", level),		
				DataHelper.MakeInParam("@Logger", logger),		
				DataHelper.MakeInParam("@Message", message),		
				DataHelper.MakeInParam("@Exception", exception),		
				DataHelper.MakeInParam("@Url", url),		
			};
			
	
			return NonQueryBool("subtext_AddLogEntry", p);
		}
		
		public bool ClearBlogContent(int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_ClearBlogContent", p);
		}
		
		public int CreateDomainAlias(int blogId, string host, string application, bool? active) {
			var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Application", application),		
				DataHelper.MakeInParam("@Active", active),		
				outParam0,
			};
			
			NonQueryInt("subtext_CreateDomainAlias", p);
			return (int)outParam0.Value;
		}
		
		public bool DeleteBlogGroup(int id) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};
			
	
			return NonQueryBool("subtext_DeleteBlogGroup", p);
		}
		
		public bool DeleteCategory(int categoryId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_DeleteCategory", p);
		}
		
		public bool DeleteDomainAlias(int id) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};
			
	
			return NonQueryBool("subtext_DeleteDomainAlias", p);
		}
		
		public bool DeleteEnclosure(int id) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};
			
	
			return NonQueryBool("subtext_DeleteEnclosure", p);
		}
		
		public bool DeleteFeedback(int id, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
	
			return NonQueryBool("subtext_DeleteFeedback", p);
		}
		
		public bool DeleteFeedbackByStatus(int blogId, int statusFlag) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@StatusFlag", statusFlag),		
			};
			
	
			return NonQueryBool("subtext_DeleteFeedbackByStatus", p);
		}
		
		public bool DeleteImage(int blogId, int imageId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@ImageID", imageId),		
			};
			
	
			return NonQueryBool("subtext_DeleteImage", p);
		}
		
		public bool DeleteImageCategory(int categoryId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_DeleteImageCategory", p);
		}
		
		public bool DeleteKeyWord(int keyWordId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@KeyWordID", keyWordId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_DeleteKeyWord", p);
		}
		
		public bool DeleteLink(int linkId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@LinkID", linkId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_DeleteLink", p);
		}
		
		public bool DeleteLinksByPostID(int postId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_DeleteLinksByPostID", p);
		}
		
		public bool DeleteMetaTag(int id) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};
			
	
			return NonQueryBool("subtext_DeleteMetaTag", p);
		}
		
		public bool DeletePost(int id, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
	
			return NonQueryBool("subtext_DeletePost", p);
		}
		
		public IDataReader GetActiveCategoriesWithLinkCollection(int? blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetActiveCategoriesWithLinkCollection", p);
		}
		
		public IDataReader GetBlogByDomainAlias(string host, string application, bool? strict) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Application", application),		
				DataHelper.MakeInParam("@Strict", strict),		
			};
			
			return GetReader("subtext_GetBlogByDomainAlias", p);
		}
		
		public IDataReader GetBlogById(int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetBlogById", p);
		}
		
		public IDataReader GetBlogGroup(int id, bool active) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@Active", active),		
			};
			
			return GetReader("subtext_GetBlogGroup", p);
		}
		
		public IDataReader GetBlogKeyWords(int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetBlogKeyWords", p);
		}
		
		public IDataReader GetCategory(string categoryName, int? categoryId, bool isActive, int? blogId, int? categoryType) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryName", categoryName),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryType", categoryType),		
			};
			
			return GetReader("subtext_GetCategory", p);
		}
		
		public IDataReader GetCommentByChecksumHash(string feedbackChecksumHash, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@FeedbackChecksumHash", feedbackChecksumHash),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetCommentByChecksumHash", p);
		}
		
		public IDataReader GetConditionalEntries(int itemCount, int postType, int postConfig, int? blogId, bool includeCategories, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@PostConfig", postConfig),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@IncludeCategories", includeCategories),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetConditionalEntries", p);
		}
		
		public IDataReader GetConfig(string host, string application, bool? strict) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Application", application),		
				DataHelper.MakeInParam("@Strict", strict),		
			};
			
			return GetReader("subtext_GetConfig", p);
		}
		
		public IDataReader GetDomainAliasById(int id) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};
			
			return GetReader("subtext_GetDomainAliasById", p);
		}
		
		public IDataReader GetEntriesByDayRange(DateTime startDate, DateTime stopDate, int postType, bool isActive, int blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@StartDate", startDate),		
				DataHelper.MakeInParam("@StopDate", stopDate),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetEntriesByDayRange", p);
		}
		
		public IDataReader GetEntriesForBlogMl(int blogId, int pageIndex, int pageSize) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};
			
			return GetReader("subtext_GetEntriesForBlogMl", p);
		}
		
		public IDataReader GetEntryPreviousNext(int id, int postType, int blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetEntry_PreviousNext", p);
		}
		
		public IDataReader GetFeedback(int id) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};
			
			return GetReader("subtext_GetFeedback", p);
		}
		
		public IDataReader GetFeedbackCollection(int entryId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryId", entryId),		
			};
			
			return GetReader("subtext_GetFeedbackCollection", p);
		}
		
		public void GetFeedbackCountsByStatus(int blogId, out int approvedCount, out int needsModerationCount, out int flaggedSpam, out int deleted) {
			var outParam0 = DataHelper.MakeOutParam("@ApprovedCount", SqlDbType.Int, 4);
			var outParam1 = DataHelper.MakeOutParam("@NeedsModerationCount", SqlDbType.Int, 4);
			var outParam2 = DataHelper.MakeOutParam("@FlaggedSpam", SqlDbType.Int, 4);
			var outParam3 = DataHelper.MakeOutParam("@Deleted", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				outParam0,
				outParam1,
				outParam2,
				outParam3,
			};
			
			NonQueryBool("subtext_GetFeedbackCountsByStatus", p); 
			approvedCount = (int)outParam0.Value;			
			needsModerationCount = (int)outParam1.Value;			
			flaggedSpam = (int)outParam2.Value;			
			deleted = (int)outParam3.Value;			
		}
		
		public IDataReader GetHost() {
			return GetReader("subtext_GetHost");
		}
		
		public IDataReader GetImageCategory(int categoryId, bool isActive, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetImageCategory", p);
		}
		
		public IDataReader GetKeyWord(int keyWordId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@KeyWordID", keyWordId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetKeyWord", p);
		}
		
		public IDataReader GetLinkCollectionByPostID(int? postId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetLinkCollectionByPostID", p);
		}
		
		public IDataReader GetLinksByCategoryID(int categoryId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetLinksByCategoryID", p);
		}
		
		public IDataReader GetMetaTags(int blogId, int? entryId, int pageIndex, int pageSize) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};
			
			return GetReader("subtext_GetMetaTags", p);
		}
		
		public IDataReader GetPageableBlogs(int pageIndex, int pageSize, string host, int configurationFlags) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@ConfigurationFlags", configurationFlags),		
			};
			
			return GetReader("subtext_GetPageableBlogs", p);
		}
		
		public IDataReader GetPageableDomainAliases(int pageIndex, int pageSize, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetPageableDomainAliases", p);
		}
		
		public IDataReader GetPageableEntries(int blogId, int pageIndex, int pageSize, int postType) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@PostType", postType),		
			};
			
			return GetReader("subtext_GetPageableEntries", p);
		}
		
		public IDataReader GetPageableEntriesByCategoryID(int blogId, int categoryId, int pageIndex, int pageSize, int postType) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@PostType", postType),		
			};
			
			return GetReader("subtext_GetPageableEntriesByCategoryID", p);
		}
		
		public IDataReader GetPageableFeedback(int blogId, int pageIndex, int pageSize, int statusFlag, int? excludeFeedbackStatusMask, int? feedbackType) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@StatusFlag", statusFlag),		
				DataHelper.MakeInParam("@ExcludeFeedbackStatusMask", excludeFeedbackStatusMask),		
				DataHelper.MakeInParam("@FeedbackType", feedbackType),		
			};
			
			return GetReader("subtext_GetPageableFeedback", p);
		}
		
		public IDataReader GetPageableKeyWords(int blogId, int pageIndex, int pageSize) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};
			
			return GetReader("subtext_GetPageableKeyWords", p);
		}
		
		public IDataReader GetPageableLinks(int blogId, int? categoryId, int pageIndex, int pageSize) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryId", categoryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};
			
			return GetReader("subtext_GetPageableLinks", p);
		}
		
		public bool GetPageableLinksByCategoryID(int blogId, int? categoryId, int pageIndex, int pageSize, bool sortDesc) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@SortDesc", sortDesc),		
			};
			
	
			return NonQueryBool("subtext_GetPageableLinksByCategoryID", p);
		}
		
		public IDataReader GetPageableLogEntries(int? blogId, int pageIndex, int pageSize) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};
			
			return GetReader("subtext_GetPageableLogEntries", p);
		}
		
		public IDataReader GetPageableReferrers(int blogId, int? entryId, int pageIndex, int pageSize) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};
			
			return GetReader("subtext_GetPageableReferrers", p);
		}
		
		public IDataReader GetPostsByCategoriesArchive(int? blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetPostsByCategoriesArchive", p);
		}
		
		public IDataReader GetPostsByCategoryID(int itemCount, int categoryId, bool isActive, int blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetPostsByCategoryID", p);
		}
		
		public IDataReader GetPostsByDayRange(DateTime startDate, DateTime stopDate, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@StartDate", startDate),		
				DataHelper.MakeInParam("@StopDate", stopDate),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetPostsByDayRange", p);
		}
		
		public IDataReader GetPostsByMonth(int month, int year, int? blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Month", month),		
				DataHelper.MakeInParam("@Year", year),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetPostsByMonth", p);
		}
		
		public IDataReader GetPostsByMonthArchive(int? blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetPostsByMonthArchive", p);
		}
		
		public IDataReader GetPostsByTag(int itemCount, string tag, int blogId, bool? isActive, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@Tag", tag),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetPostsByTag", p);
		}
		
		public IDataReader GetPostsByYearArchive(int blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetPostsByYearArchive", p);
		}
		
		public IDataReader GetRelatedEntries(int blogId, int entryId, int rowCount) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@RowCount", rowCount),		
			};
			
			return GetReader("subtext_GetRelatedEntries", p);
		}
		
		public IDataReader GetSingleDay(DateTime date, int blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Date", date),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_GetSingleDay", p);
		}
		
		public IDataReader GetSingleEntry(int? id, string entryName, bool isActive, int? blogId, bool includeCategories) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@EntryName", entryName),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@IncludeCategories", includeCategories),		
			};
			
			return GetReader("subtext_GetSingleEntry", p);
		}
		
		public IDataReader GetSingleImage(int imageId, bool isActive, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ImageID", imageId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetSingleImage", p);
		}
		
		public IDataReader GetSingleLink(int linkId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@LinkID", linkId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetSingleLink", p);
		}
		
		public IDataReader GetTopEntries(int blogId, int rowCount) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@RowCount", rowCount),		
			};
			
			return GetReader("subtext_GetTopEntries", p);
		}
		
		public IDataReader GetTopTags(int itemCount, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_GetTopTags", p);
		}
		
		public int GetUrlID(string url) {
			var outParam0 = DataHelper.MakeOutParam("@UrlID", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Url", url),		
				outParam0,
			};
			
			NonQueryInt("subtext_GetUrlID", p);
			return (int)outParam0.Value;
		}
		
		public int InsertBlogGroup(string title, bool active, int? displayOrder, string description) {
			var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@DisplayOrder", displayOrder),		
				DataHelper.MakeInParam("@Description", description),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertBlogGroup", p);
			return (int)outParam0.Value;
		}
		
		public int InsertCategory(string title, bool active, int blogId, int categoryType, string description) {
			var outParam0 = DataHelper.MakeOutParam("@CategoryID", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryType", categoryType),		
				DataHelper.MakeInParam("@Description", description),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertCategory", p);
			return (int)outParam0.Value;
		}
		
		public int InsertEnclosure(string title, string url, string mimeType, long size, bool addToFeed, bool showWithPost, int entryId) {
			var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@MimeType", mimeType),		
				DataHelper.MakeInParam("@Size", size),		
				DataHelper.MakeInParam("@AddToFeed", addToFeed),		
				DataHelper.MakeInParam("@ShowWithPost", showWithPost),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertEnclosure", p);
			return (int)outParam0.Value;
		}
		
		public int InsertEntry(string title, string text, int postType, string author, string email, string description, int blogId, DateTime dateAdded, int postConfig, string entryName, DateTime? dateSyndicated, DateTime currentDateTime) {
			var outParam0 = DataHelper.MakeOutParam("@ID", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Text", text),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@Author", author),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Description", description),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@DateAdded", dateAdded),		
				DataHelper.MakeInParam("@PostConfig", postConfig),		
				DataHelper.MakeInParam("@EntryName", entryName),		
				DataHelper.MakeInParam("@DateSyndicated", dateSyndicated),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertEntry", p);
			return (int)outParam0.Value;
		}
		
		public bool InsertEntryTagList(int entryId, int blogId, string tagList) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@TagList", tagList),		
			};
			
	
			return NonQueryBool("subtext_InsertEntryTagList", p);
		}
		
		public bool InsertEntryViewCount(int entryId, int blogId, bool isWeb) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@IsWeb", isWeb),		
			};
			
	
			return NonQueryBool("subtext_InsertEntryViewCount", p);
		}
		
		public int InsertFeedback(string title, string body, int blogId, int? entryId, string author, bool isBlogAuthor, string email, string url, int feedbackType, int statusFlag, bool commentAPI, string referrer, string ipAddress, string userAgent, string feedbackChecksumHash, DateTime dateCreated, DateTime? dateModified, DateTime currentDateTime) {
			var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Body", body),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@Author", author),		
				DataHelper.MakeInParam("@IsBlogAuthor", isBlogAuthor),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@FeedbackType", feedbackType),		
				DataHelper.MakeInParam("@StatusFlag", statusFlag),		
				DataHelper.MakeInParam("@CommentAPI", commentAPI),		
				DataHelper.MakeInParam("@Referrer", referrer),		
				DataHelper.MakeInParam("@IpAddress", ipAddress),		
				DataHelper.MakeInParam("@UserAgent", userAgent),		
				DataHelper.MakeInParam("@FeedbackChecksumHash", feedbackChecksumHash),		
				DataHelper.MakeInParam("@DateCreated", dateCreated),		
				DataHelper.MakeInParam("@DateModified", dateModified),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertFeedback", p);
			return (int)outParam0.Value;
		}
		
		public int InsertImage(string title, int categoryId, int width, int height, string file, bool active, int blogId) {
			var outParam0 = DataHelper.MakeOutParam("@ImageID", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@Width", width),		
				DataHelper.MakeInParam("@Height", height),		
				DataHelper.MakeInParam("@File", file),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertImage", p);
			return (int)outParam0.Value;
		}
		
		public int InsertKeyWord(string word, string rel, string text, bool replaceFirstTimeOnly, bool openInNewWindow, bool caseSensitive, string url, string title, int blogId) {
			var outParam0 = DataHelper.MakeOutParam("@KeyWordID", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Word", word),		
				DataHelper.MakeInParam("@Rel", rel),		
				DataHelper.MakeInParam("@Text", text),		
				DataHelper.MakeInParam("@ReplaceFirstTimeOnly", replaceFirstTimeOnly),		
				DataHelper.MakeInParam("@OpenInNewWindow", openInNewWindow),		
				DataHelper.MakeInParam("@CaseSensitive", caseSensitive),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertKeyWord", p);
			return (int)outParam0.Value;
		}
		
		public int InsertLink(string title, string url, string rss, bool active, bool newWindow, int categoryId, int? postId, int blogId, string rel) {
			var outParam0 = DataHelper.MakeOutParam("@LinkID", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@Rss", rss),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@NewWindow", newWindow),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Rel", rel),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertLink", p);
			return (int)outParam0.Value;
		}
		
		public bool InsertLinkCategoryList(string categoryList, int postId, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryList", categoryList),		
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_InsertLinkCategoryList", p);
		}
		
		public int InsertMetaTag(string content, string name, string httpEquiv, int blogId, int? entryId, DateTime? dateCreated) {
			var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Content", content),		
				DataHelper.MakeInParam("@Name", name),		
				DataHelper.MakeInParam("@HttpEquiv", httpEquiv),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@DateCreated", dateCreated),		
				outParam0,
			};
			
			NonQueryInt("subtext_InsertMetaTag", p);
			return (int)outParam0.Value;
		}
		
		public bool InsertReferral(int entryId, int blogId, string url) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Url", url),		
			};
			
	
			return NonQueryBool("subtext_InsertReferral", p);
		}
		
		public IDataReader InsertViewStats(int blogId, int pageType, int postId, DateTime day, string url) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageType", pageType),		
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@Day", day),		
				DataHelper.MakeInParam("@Url", url),		
			};
			
			return GetReader("subtext_InsertViewStats", p);
		}
		
		public IDataReader ListBlogGroups(bool active) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Active", active),		
			};
			
			return GetReader("subtext_ListBlogGroups", p);
		}
		
		public bool LogClear(int? blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_LogClear", p);
		}
		
		public IDataReader SearchEntries(int blogId, string searchStr, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@SearchStr", searchStr),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
			return GetReader("subtext_SearchEntries", p);
		}
		
		public IDataReader StatsSummary(int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
			return GetReader("subtext_StatsSummary", p);
		}
		
		public bool TrackEntry(int entryId, int blogId, string url, bool isWeb) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@IsWeb", isWeb),		
			};
			
	
			return NonQueryBool("subtext_TrackEntry", p);
		}
		
		public bool UpdateBlogGroup(int id, string title, bool active, string description, int? displayOrder) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@Description", description),		
				DataHelper.MakeInParam("@DisplayOrder", displayOrder),		
			};
			
	
			return NonQueryBool("subtext_UpdateBlogGroup", p);
		}
		
		public bool UpdateBlogStats(int blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
	
			return NonQueryBool("subtext_UpdateBlogStats", p);
		}
		
		public bool UpdateCategory(int categoryId, string title, bool active, int categoryType, string description, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@CategoryType", categoryType),		
				DataHelper.MakeInParam("@Description", description),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_UpdateCategory", p);
		}
		
		public bool UpdateConfig(string userName, string password, string email, string title, string subTitle, string skin, string application, string host, string author, string language, int? timeZone, int itemCount, int categoryListPostCount, string news, string trackingCode, DateTime? lastUpdated, string secondaryCss, string skinCssFile, int? flag, int blogId, string licenseUrl, int? daysTillCommentsClose, int? commentDelayInMinutes, int? numberOfRecentComments, int? recentCommentsLength, string akismetAPIKey, string feedBurnerName, int blogGroupId, string mobileSkin, string mobileSkinCssFile, string openIDUrl, string cardSpaceHash, string openIDServer, string openIDDelegate) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@UserName", userName),		
				DataHelper.MakeInParam("@Password", password),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@SubTitle", subTitle),		
				DataHelper.MakeInParam("@Skin", skin),		
				DataHelper.MakeInParam("@Application", application),		
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Author", author),		
				DataHelper.MakeInParam("@Language", language),		
				DataHelper.MakeInParam("@TimeZone", timeZone),		
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@CategoryListPostCount", categoryListPostCount),		
				DataHelper.MakeInParam("@News", news),		
				DataHelper.MakeInParam("@TrackingCode", trackingCode),		
				DataHelper.MakeInParam("@LastUpdated", lastUpdated),		
				DataHelper.MakeInParam("@SecondaryCss", secondaryCss),		
				DataHelper.MakeInParam("@SkinCssFile", skinCssFile),		
				DataHelper.MakeInParam("@Flag", flag),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@LicenseUrl", licenseUrl),		
				DataHelper.MakeInParam("@DaysTillCommentsClose", daysTillCommentsClose),		
				DataHelper.MakeInParam("@CommentDelayInMinutes", commentDelayInMinutes),		
				DataHelper.MakeInParam("@NumberOfRecentComments", numberOfRecentComments),		
				DataHelper.MakeInParam("@RecentCommentsLength", recentCommentsLength),		
				DataHelper.MakeInParam("@AkismetAPIKey", akismetAPIKey),		
				DataHelper.MakeInParam("@FeedBurnerName", feedBurnerName),		
				DataHelper.MakeInParam("@BlogGroupId", blogGroupId),		
				DataHelper.MakeInParam("@MobileSkin", mobileSkin),		
				DataHelper.MakeInParam("@MobileSkinCssFile", mobileSkinCssFile),		
				DataHelper.MakeInParam("@OpenIDUrl", openIDUrl),		
				DataHelper.MakeInParam("@CardSpaceHash", cardSpaceHash),		
				DataHelper.MakeInParam("@OpenIDServer", openIDServer),		
				DataHelper.MakeInParam("@OpenIDDelegate", openIDDelegate),		
			};
			
	
			return NonQueryBool("subtext_UpdateConfig", p);
		}
		
		public bool UpdateConfigUpdateTime(int blogId, DateTime lastUpdated) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@LastUpdated", lastUpdated),		
			};
			
	
			return NonQueryBool("subtext_UpdateConfigUpdateTime", p);
		}
		
		public bool UpdateDomainAlias(int id, int blogId, string host, string application, bool? active) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Application", application),		
				DataHelper.MakeInParam("@Active", active),		
			};
			
	
			return NonQueryBool("subtext_UpdateDomainAlias", p);
		}
		
		public bool UpdateEnclosure(string title, string url, string mimeType, long size, bool addToFeed, bool showWithPost, int entryId, int id) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@MimeType", mimeType),		
				DataHelper.MakeInParam("@Size", size),		
				DataHelper.MakeInParam("@AddToFeed", addToFeed),		
				DataHelper.MakeInParam("@ShowWithPost", showWithPost),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@Id", id),		
			};
			
	
			return NonQueryBool("subtext_UpdateEnclosure", p);
		}
		
		public bool UpdateEntry(int id, string title, string text, int postType, string author, string email, string description, DateTime? dateUpdated, int postConfig, string entryName, DateTime? dateSyndicated, int blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Text", text),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@Author", author),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Description", description),		
				DataHelper.MakeInParam("@DateUpdated", dateUpdated),		
				DataHelper.MakeInParam("@PostConfig", postConfig),		
				DataHelper.MakeInParam("@EntryName", entryName),		
				DataHelper.MakeInParam("@DateSyndicated", dateSyndicated),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
	
			return NonQueryBool("subtext_UpdateEntry", p);
		}
		
		public bool UpdateFeedback(int id, string title, string body, string author, string email, string url, int statusFlag, string feedbackChecksumHash, DateTime dateModified, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Body", body),		
				DataHelper.MakeInParam("@Author", author),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@StatusFlag", statusFlag),		
				DataHelper.MakeInParam("@FeedbackChecksumHash", feedbackChecksumHash),		
				DataHelper.MakeInParam("@DateModified", dateModified),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
	
			return NonQueryBool("subtext_UpdateFeedback", p);
		}
		
		public bool UpdateFeedbackCount(int blogId, int entryId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
	
			return NonQueryBool("subtext_UpdateFeedbackCount", p);
		}
		
		public bool UpdateFeedbackStats(int blogId, DateTime currentDateTime) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};
			
	
			return NonQueryBool("subtext_UpdateFeedbackStats", p);
		}
		
		public bool UpdateHost(string hostUserName, string password, string salt) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@HostUserName", hostUserName),		
				DataHelper.MakeInParam("@Password", password),		
				DataHelper.MakeInParam("@Salt", salt),		
			};
			
	
			return NonQueryBool("subtext_UpdateHost", p);
		}
		
		public bool UpdateImage(string title, int categoryId, int width, int height, string file, bool active, int blogId, int imageId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@Width", width),		
				DataHelper.MakeInParam("@Height", height),		
				DataHelper.MakeInParam("@File", file),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@ImageID", imageId),		
			};
			
	
			return NonQueryBool("subtext_UpdateImage", p);
		}
		
		public bool UpdateKeyWord(int keyWordId, string word, string rel, string text, bool replaceFirstTimeOnly, bool openInNewWindow, bool caseSensitive, string url, string title, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@KeyWordID", keyWordId),		
				DataHelper.MakeInParam("@Word", word),		
				DataHelper.MakeInParam("@Rel", rel),		
				DataHelper.MakeInParam("@Text", text),		
				DataHelper.MakeInParam("@ReplaceFirstTimeOnly", replaceFirstTimeOnly),		
				DataHelper.MakeInParam("@OpenInNewWindow", openInNewWindow),		
				DataHelper.MakeInParam("@CaseSensitive", caseSensitive),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_UpdateKeyWord", p);
		}
		
		public bool UpdateLink(int linkId, string title, string url, string rss, bool active, bool newWindow, int categoryId, string rel, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@LinkID", linkId),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@Rss", rss),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@NewWindow", newWindow),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@Rel", rel),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_UpdateLink", p);
		}
		
		public bool UpdateMetaTag(int id, string content, string name, string httpEquiv, int blogId, int? entryId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@Content", content),		
				DataHelper.MakeInParam("@Name", name),		
				DataHelper.MakeInParam("@HttpEquiv", httpEquiv),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryId", entryId),		
			};
			
	
			return NonQueryBool("subtext_UpdateMetaTag", p);
		}
		
		public int UTILITYAddBlog(string title, string userName, string password, string email, string host, string application, int flag, int blogGroupId) {
			var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@UserName", userName),		
				DataHelper.MakeInParam("@Password", password),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Application", application),		
				DataHelper.MakeInParam("@Flag", flag),		
				DataHelper.MakeInParam("@BlogGroupId", blogGroupId),		
				outParam0,
			};
			
			NonQueryInt("subtext_UTILITY_AddBlog", p);
			return (int)outParam0.Value;
		}
		
		public IDataReader UtilityGetUnHashedPasswords() {
			return GetReader("subtext_Utility_GetUnHashedPasswords");
		}
		
		public bool UtilityUpdateToHashedPassword(string password, int blogId) {
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Password", password),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};
			
	
			return NonQueryBool("subtext_Utility_UpdateToHashedPassword", p);
		}
		
		public int? VersionAdd(int major, int minor, int build, DateTime? dateCreated) {
			var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p = {
				DataHelper.MakeInParam("@Major", major),		
				DataHelper.MakeInParam("@Minor", minor),		
				DataHelper.MakeInParam("@Build", build),		
				DataHelper.MakeInParam("@DateCreated", dateCreated),		
				outParam0,
			};
			
			NonQueryInt("subtext_VersionAdd", p);
			if(outParam0.Value == null) {
			  return null;
			}
			return (int)outParam0.Value;
		}
		
		public IDataReader VersionGetCurrent() {
			return GetReader("subtext_VersionGetCurrent");
		}
		
	}
}
