#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Data;
using System.Data.SqlClient;

namespace Subtext.Framework.Data
{
    public partial class StoredProcedures
    {
        public virtual IDataReader GetRecentImages(string host, int? groupId, int rowCount)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@GroupID", groupId),		
				DataHelper.MakeInParam("@rowCount", rowCount),		
			};

            return GetReader("DNW_GetRecentImages", p);
        }

        public virtual IDataReader GetRecentPosts(string host, int? groupId, DateTime currentDateTime, int? rowCount)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@GroupID", groupId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
				DataHelper.MakeInParam("@RowCount", rowCount),		
			};

            return GetReader("DNW_GetRecentPosts", p);
        }

        public virtual IDataReader Stats(string host, int? groupId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@GroupID", groupId),		
			};

            return GetReader("DNW_Stats", p);
        }

        public virtual IDataReader TotalStats(string host, int? groupId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@GroupID", groupId),		
			};

            return GetReader("DNW_Total_Stats", p);
        }

        public virtual bool AddLogEntry(DateTime date, int? blogId, string thread, string context, string level, string logger, string message, string exception, string url)
        {
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

        public virtual bool ClearBlogContent(int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_ClearBlogContent", p);
        }

        public virtual int CreateDomainAlias(int blogId, string host, string application, bool? active)
        {
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

        public virtual bool DeleteBlogGroup(int id)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};


            return NonQueryBool("subtext_DeleteBlogGroup", p);
        }

        public virtual bool DeleteCategory(int categoryId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_DeleteCategory", p);
        }

        public virtual bool DeleteDomainAlias(int id)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};


            return NonQueryBool("subtext_DeleteDomainAlias", p);
        }

        public virtual bool DeleteEnclosure(int id)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};


            return NonQueryBool("subtext_DeleteEnclosure", p);
        }

        public virtual bool DeleteFeedback(int id, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};


            return NonQueryBool("subtext_DeleteFeedback", p);
        }

        public virtual bool DeleteFeedbackByStatus(int blogId, int statusFlag)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@StatusFlag", statusFlag),		
			};


            return NonQueryBool("subtext_DeleteFeedbackByStatus", p);
        }

        public virtual bool DeleteImage(int blogId, int imageId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@ImageID", imageId),		
			};


            return NonQueryBool("subtext_DeleteImage", p);
        }

        public virtual bool DeleteImageCategory(int categoryId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_DeleteImageCategory", p);
        }

        public virtual bool DeleteKeyWord(int keyWordId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@KeyWordID", keyWordId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_DeleteKeyWord", p);
        }

        public virtual bool DeleteLink(int linkId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@LinkID", linkId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_DeleteLink", p);
        }

        public virtual bool DeleteLinksByPostID(int postId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_DeleteLinksByPostID", p);
        }

        public virtual bool DeleteMetaTag(int id)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};


            return NonQueryBool("subtext_DeleteMetaTag", p);
        }

        public virtual bool DeletePost(int id, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};


            return NonQueryBool("subtext_DeletePost", p);
        }

        public virtual IDataReader GetActiveCategoriesWithLinkCollection(int? blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetActiveCategoriesWithLinkCollection", p);
        }

        public virtual IDataReader GetBlogByDomainAlias(string host, string application, bool? strict)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Application", application),		
				DataHelper.MakeInParam("@Strict", strict),		
			};

            return GetReader("subtext_GetBlogByDomainAlias", p);
        }

        public virtual IDataReader GetBlogById(int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetBlogById", p);
        }

        public virtual IDataReader GetBlogGroup(int id, bool active)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@Active", active),		
			};

            return GetReader("subtext_GetBlogGroup", p);
        }

        public virtual IDataReader GetBlogKeyWords(int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetBlogKeyWords", p);
        }

        public virtual IDataReader GetBlogStats(int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetBlogStats", p);
        }

        public virtual IDataReader GetCategory(string categoryName, int? categoryId, bool isActive, int? blogId, int? categoryType)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryName", categoryName),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryType", categoryType),		
			};

            return GetReader("subtext_GetCategory", p);
        }

        public virtual IDataReader GetCommentByChecksumHash(string feedbackChecksumHash, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@FeedbackChecksumHash", feedbackChecksumHash),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetCommentByChecksumHash", p);
        }

        public virtual IDataReader GetConditionalEntries(int itemCount, int postType, int postConfig, int? blogId, bool includeCategories, DateTime currentDateTime)
        {
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

        public virtual IDataReader GetConfig(string host, string application)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Application", application),		
			};

            return GetReader("subtext_GetConfig", p);
        }

        public virtual IDataReader GetDomainAliasById(int id)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};

            return GetReader("subtext_GetDomainAliasById", p);
        }

        public virtual IDataReader GetEntries(int blogId, int? categoryId, int pageIndex, int postType, int pageSize)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};

            return GetReader("subtext_GetEntries", p);
        }

        public virtual IDataReader GetEntriesByDayRange(DateTime startDate, DateTime stopDate, int postType, bool isActive, int blogId, DateTime currentDateTime)
        {
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

        public virtual IDataReader GetEntriesForExport(int blogId, int pageIndex, int pageSize)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};

            return GetReader("subtext_GetEntriesForExport", p);
        }

        public virtual IDataReader GetEntryPreviousNext(int id, int postType, int blogId, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};

            return GetReader("subtext_GetEntry_PreviousNext", p);
        }

        public virtual IDataReader GetFeedback(int id)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
			};

            return GetReader("subtext_GetFeedback", p);
        }

        public virtual IDataReader GetFeedbackCollection(int entryId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryId", entryId),		
			};

            return GetReader("subtext_GetFeedbackCollection", p);
        }

        public virtual void GetFeedbackCountsByStatus(int blogId, out int approvedCount, out int needsModerationCount, out int flaggedSpam, out int deleted)
        {
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

        public virtual IDataReader GetHost()
        {
            return GetReader("subtext_GetHost");
        }

        public virtual IDataReader GetImageCategory(int categoryId, bool isActive, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetImageCategory", p);
        }

        public virtual IDataReader GetKeyWord(int keyWordId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@KeyWordID", keyWordId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetKeyWord", p);
        }

        public virtual IDataReader GetLinkCollectionByPostID(int? postId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetLinkCollectionByPostID", p);
        }

        public virtual IDataReader GetLinksByCategoryID(int categoryId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetLinksByCategoryID", p);
        }

        public virtual IDataReader GetMetaTags(int blogId, int? entryId, int pageIndex, int pageSize)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};

            return GetReader("subtext_GetMetaTags", p);
        }

        public virtual IDataReader GetPageableBlogs(int pageIndex, int pageSize, string host, int configurationFlags)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@ConfigurationFlags", configurationFlags),		
			};

            return GetReader("subtext_GetPageableBlogs", p);
        }

        public virtual IDataReader GetPageableDomainAliases(int pageIndex, int pageSize, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetPageableDomainAliases", p);
        }

        public virtual IDataReader GetPageableFeedback(int blogId, int pageIndex, int pageSize, int statusFlag, int? excludeFeedbackStatusMask, int? feedbackType)
        {
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

        public virtual IDataReader GetPageableKeyWords(int blogId, int pageIndex, int pageSize)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};

            return GetReader("subtext_GetPageableKeyWords", p);
        }

        public virtual IDataReader GetPageableLinks(int blogId, int? categoryId, int pageIndex, int pageSize)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryId", categoryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};

            return GetReader("subtext_GetPageableLinks", p);
        }

        public virtual bool GetPageableLinksByCategoryID(int blogId, int? categoryId, int pageIndex, int pageSize, bool sortDesc)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
				DataHelper.MakeInParam("@SortDesc", sortDesc),		
			};


            return NonQueryBool("subtext_GetPageableLinksByCategoryID", p);
        }

        public virtual IDataReader GetPageableLogEntries(int? blogId, int pageIndex, int pageSize)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};

            return GetReader("subtext_GetPageableLogEntries", p);
        }

        public virtual IDataReader GetPageableReferrers(int blogId, int? entryId, int pageIndex, int pageSize)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@PageIndex", pageIndex),		
				DataHelper.MakeInParam("@PageSize", pageSize),		
			};

            return GetReader("subtext_GetPageableReferrers", p);
        }

        public virtual IDataReader GetPopularPosts(int blogId, DateTime? minDate)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@MinDate", minDate),		
			};

            return GetReader("subtext_GetPopularPosts", p);
        }

        public virtual IDataReader GetPostsByCategoriesArchive(int? blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetPostsByCategoriesArchive", p);
        }

        public virtual IDataReader GetPostsByCategoryID(int itemCount, int categoryId, bool isActive, int blogId, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};

            return GetReader("subtext_GetPostsByCategoryID", p);
        }

        public virtual IDataReader GetPostsByMonth(int month, int year, int? blogId, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Month", month),		
				DataHelper.MakeInParam("@Year", year),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};

            return GetReader("subtext_GetPostsByMonth", p);
        }

        public virtual IDataReader GetPostsByMonthArchive(int? blogId, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};

            return GetReader("subtext_GetPostsByMonthArchive", p);
        }

        public virtual IDataReader GetPostsByTag(int itemCount, string tag, int blogId, bool? isActive, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@Tag", tag),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};

            return GetReader("subtext_GetPostsByTag", p);
        }

        public virtual IDataReader GetPostsByYearArchive(int blogId, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};

            return GetReader("subtext_GetPostsByYearArchive", p);
        }

        public virtual IDataReader GetRelatedEntries(int blogId, int entryId, int rowCount)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@RowCount", rowCount),		
			};

            return GetReader("subtext_GetRelatedEntries", p);
        }

        public virtual IDataReader GetSingleEntry(int? id, string entryName, bool isActive, int? blogId, bool includeCategories)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@EntryName", entryName),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@IncludeCategories", includeCategories),		
			};

            return GetReader("subtext_GetSingleEntry", p);
        }

        public virtual IDataReader GetSingleImage(int imageId, bool isActive, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ImageID", imageId),		
				DataHelper.MakeInParam("@IsActive", isActive),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetSingleImage", p);
        }

        public virtual IDataReader GetSingleLink(int linkId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@LinkID", linkId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetSingleLink", p);
        }

        public virtual IDataReader GetTopEntries(int blogId, int rowCount)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@RowCount", rowCount),		
			};

            return GetReader("subtext_GetTopEntries", p);
        }

        public virtual IDataReader GetTopTags(int itemCount, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_GetTopTags", p);
        }

        public virtual int GetUrlID(string url)
        {
            var outParam0 = DataHelper.MakeOutParam("@UrlID", SqlDbType.Int, 4);
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Url", url),		
				outParam0,
			};

            NonQueryInt("subtext_GetUrlID", p);
            return (int)outParam0.Value;
        }

        public virtual int InsertBlogGroup(string title, bool active, int? displayOrder, string description)
        {
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

        public virtual int InsertCategory(string title, bool active, int blogId, int categoryType, string description)
        {
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

        public virtual int InsertEnclosure(string title, string url, string mimeType, long size, bool addToFeed, bool showWithPost, int entryId)
        {
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

        public virtual int InsertEntry(string title, string text, int postType, string author, string email, string description, int blogId, DateTime dateCreatedUtc, int postConfig, string entryName, DateTime? datePublishedUtc)
        {
            var outParam0 = DataHelper.MakeOutParam("@ID", SqlDbType.Int, 4);
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Text", text),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@Author", author),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Description", description),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@DateCreatedUtc", dateCreatedUtc),		
				DataHelper.MakeInParam("@PostConfig", postConfig),		
				DataHelper.MakeInParam("@EntryName", entryName),		
				DataHelper.MakeInParam("@DatePublishedUtc", datePublishedUtc),		
				outParam0,
			};

            NonQueryInt("subtext_InsertEntry", p);
            return (int)outParam0.Value;
        }

        public virtual bool InsertEntryTagList(int entryId, int blogId, string tagList)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@TagList", tagList),		
			};


            return NonQueryBool("subtext_InsertEntryTagList", p);
        }

        public virtual bool InsertEntryViewCount(int entryId, int blogId, bool isWeb)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@IsWeb", isWeb),		
			};


            return NonQueryBool("subtext_InsertEntryViewCount", p);
        }

        public virtual int InsertFeedback(string title, string body, int blogId, int? entryId, string author, bool isBlogAuthor, string email, string url, int feedbackType, int statusFlag, bool commentAPI, string referrer, string ipAddress, string userAgent, string feedbackChecksumHash, DateTime dateCreatedUtc, DateTime? dateModifiedUtc, DateTime currentDateTime)
        {
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
				DataHelper.MakeInParam("@DateCreatedUtc", dateCreatedUtc),		
				DataHelper.MakeInParam("@DateModifiedUtc", dateModifiedUtc),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
				outParam0,
			};

            NonQueryInt("subtext_InsertFeedback", p);
            return (int)outParam0.Value;
        }

        public virtual int InsertImage(string title, int categoryId, int width, int height, string file, bool active, int blogId, string url)
        {
            var outParam0 = DataHelper.MakeOutParam("@ImageID", SqlDbType.Int, 4);
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@Width", width),		
				DataHelper.MakeInParam("@Height", height),		
				DataHelper.MakeInParam("@File", file),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Url", url),		
				outParam0,
			};

            NonQueryInt("subtext_InsertImage", p);
            return (int)outParam0.Value;
        }

        public virtual int InsertKeyWord(string word, string rel, string text, bool replaceFirstTimeOnly, bool openInNewWindow, bool caseSensitive, string url, string title, int blogId)
        {
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

        public virtual int InsertLink(string title, string url, string rss, bool active, bool newWindow, int categoryId, int? postId, int blogId, string rel)
        {
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

        public virtual bool InsertLinkCategoryList(string categoryList, int postId, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@CategoryList", categoryList),		
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_InsertLinkCategoryList", p);
        }

        public virtual int InsertMetaTag(string content, string name, string httpEquiv, int blogId, int? entryId, DateTime? dateCreatedUtc)
        {
            var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Content", content),		
				DataHelper.MakeInParam("@Name", name),		
				DataHelper.MakeInParam("@HttpEquiv", httpEquiv),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@DateCreatedUtc", dateCreatedUtc),		
				outParam0,
			};

            NonQueryInt("subtext_InsertMetaTag", p);
            return (int)outParam0.Value;
        }

        public virtual bool InsertReferral(int entryId, int blogId, string url)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Url", url),		
			};


            return NonQueryBool("subtext_InsertReferral", p);
        }

        public virtual IDataReader InsertViewStats(int blogId, int pageType, int postId, DateTime day, string url)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@PageType", pageType),		
				DataHelper.MakeInParam("@PostID", postId),		
				DataHelper.MakeInParam("@Day", day),		
				DataHelper.MakeInParam("@Url", url),		
			};

            return GetReader("subtext_InsertViewStats", p);
        }

        public virtual IDataReader ListBlogGroups(bool active)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Active", active),		
			};

            return GetReader("subtext_ListBlogGroups", p);
        }

        public virtual bool LogClear(int? blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_LogClear", p);
        }

        public virtual IDataReader SearchEntries(int blogId, string searchStr, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@SearchStr", searchStr),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};

            return GetReader("subtext_SearchEntries", p);
        }

        public virtual IDataReader StatsSummary(int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
			};

            return GetReader("subtext_StatsSummary", p);
        }

        public virtual bool TrackEntry(int entryId, int blogId, string url, bool isWeb)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@EntryID", entryId),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@IsWeb", isWeb),		
			};


            return NonQueryBool("subtext_TrackEntry", p);
        }

        public virtual bool UpdateBlogGroup(int id, string title, bool active, string description, int? displayOrder)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@Description", description),		
				DataHelper.MakeInParam("@DisplayOrder", displayOrder),		
			};


            return NonQueryBool("subtext_UpdateBlogGroup", p);
        }

        public virtual bool UpdateBlogStats(int blogId, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};


            return NonQueryBool("subtext_UpdateBlogStats", p);
        }

        public virtual bool UpdateCategory(int categoryId, string title, bool active, int categoryType, string description, int blogId)
        {
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

        public virtual bool UpdateConfig(string userName, string password, string email, string title, string subTitle, string skin, string application, string host, string author, string language, string timeZoneId, int timeZoneOffset, int itemCount, int categoryListPostCount, string news, string trackingCode, DateTime? dateModifiedUtc, string secondaryCss, string skinCssFile, int? flag, int blogId, string licenseUrl, int? daysTillCommentsClose, int? commentDelayInMinutes, int? numberOfRecentComments, int? recentCommentsLength, string akismetAPIKey, string feedBurnerName, int blogGroupId, string mobileSkin, string mobileSkinCssFile, string openIDUrl, string cardSpaceHash, string openIDServer, string openIDDelegate)
        {
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
				DataHelper.MakeInParam("@TimeZoneId", timeZoneId),		
				DataHelper.MakeInParam("@TimeZoneOffset", timeZoneOffset),		
				DataHelper.MakeInParam("@ItemCount", itemCount),		
				DataHelper.MakeInParam("@CategoryListPostCount", categoryListPostCount),		
				DataHelper.MakeInParam("@News", news),		
				DataHelper.MakeInParam("@TrackingCode", trackingCode),		
				DataHelper.MakeInParam("@DateModifiedUtc", dateModifiedUtc),		
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

        public virtual bool UpdateConfigUpdateTime(int blogId, DateTime dateModifiedUtc)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@DateModifiedUtc", dateModifiedUtc),		
			};


            return NonQueryBool("subtext_UpdateConfigUpdateTime", p);
        }

        public virtual bool UpdateDomainAlias(int id, int blogId, string host, string application, bool? active)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Id", id),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@Host", host),		
				DataHelper.MakeInParam("@Application", application),		
				DataHelper.MakeInParam("@Active", active),		
			};


            return NonQueryBool("subtext_UpdateDomainAlias", p);
        }

        public virtual bool UpdateEnclosure(string title, string url, string mimeType, long size, bool addToFeed, bool showWithPost, int entryId, int id)
        {
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

        public virtual bool UpdateEntry(int id, string title, string text, int postType, string author, string email, string description, DateTime dateModifiedUtc, int postConfig, string entryName, DateTime? datePublishedUtc, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Text", text),		
				DataHelper.MakeInParam("@PostType", postType),		
				DataHelper.MakeInParam("@Author", author),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Description", description),		
				DataHelper.MakeInParam("@DateModifiedUtc", dateModifiedUtc),		
				DataHelper.MakeInParam("@PostConfig", postConfig),		
				DataHelper.MakeInParam("@EntryName", entryName),		
				DataHelper.MakeInParam("@DatePublishedUtc", datePublishedUtc),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_UpdateEntry", p);
        }

        public virtual bool UpdateFeedback(int id, string title, string body, string author, string email, string url, int statusFlag, string feedbackChecksumHash, DateTime dateModifiedUtc)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@ID", id),		
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@Body", body),		
				DataHelper.MakeInParam("@Author", author),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Url", url),		
				DataHelper.MakeInParam("@StatusFlag", statusFlag),		
				DataHelper.MakeInParam("@FeedbackChecksumHash", feedbackChecksumHash),		
				DataHelper.MakeInParam("@DateModifiedUtc", dateModifiedUtc),		
			};


            return NonQueryBool("subtext_UpdateFeedback", p);
        }

        public virtual bool UpdateFeedbackCount(int blogId, int entryId, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@EntryId", entryId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};


            return NonQueryBool("subtext_UpdateFeedbackCount", p);
        }

        public virtual bool UpdateFeedbackStats(int blogId, DateTime currentDateTime)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@CurrentDateTime", currentDateTime),		
			};


            return NonQueryBool("subtext_UpdateFeedbackStats", p);
        }

        public virtual bool UpdateHost(string hostUserName, string email, string password, string salt)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@HostUserName", hostUserName),		
				DataHelper.MakeInParam("@Email", email),		
				DataHelper.MakeInParam("@Password", password),		
				DataHelper.MakeInParam("@Salt", salt),		
			};


            return NonQueryBool("subtext_UpdateHost", p);
        }

        public virtual bool UpdateImage(string title, int categoryId, int width, int height, string file, bool active, int blogId, int imageId, string url)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Title", title),		
				DataHelper.MakeInParam("@CategoryID", categoryId),		
				DataHelper.MakeInParam("@Width", width),		
				DataHelper.MakeInParam("@Height", height),		
				DataHelper.MakeInParam("@File", file),		
				DataHelper.MakeInParam("@Active", active),		
				DataHelper.MakeInParam("@BlogId", blogId),		
				DataHelper.MakeInParam("@ImageID", imageId),		
				DataHelper.MakeInParam("@Url", url),		
			};


            return NonQueryBool("subtext_UpdateImage", p);
        }

        public virtual bool UpdateKeyWord(int keyWordId, string word, string rel, string text, bool replaceFirstTimeOnly, bool openInNewWindow, bool caseSensitive, string url, string title, int blogId)
        {
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

        public virtual bool UpdateLink(int linkId, string title, string url, string rss, bool active, bool newWindow, int categoryId, string rel, int blogId)
        {
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

        public virtual bool UpdateMetaTag(int id, string content, string name, string httpEquiv, int blogId, int? entryId)
        {
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

        public virtual int UTILITYAddBlog(string title, string userName, string password, string email, string host, string application, int flag, int blogGroupId, DateTime? dateCreatedUtc)
        {
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
				DataHelper.MakeInParam("@DateCreatedUtc", dateCreatedUtc),		
				outParam0,
			};

            NonQueryInt("subtext_UTILITY_AddBlog", p);
            return (int)outParam0.Value;
        }

        public virtual IDataReader UtilityGetUnHashedPasswords()
        {
            return GetReader("subtext_Utility_GetUnHashedPasswords");
        }

        public virtual bool UtilityUpdateToHashedPassword(string password, int blogId)
        {
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Password", password),		
				DataHelper.MakeInParam("@BlogId", blogId),		
			};


            return NonQueryBool("subtext_Utility_UpdateToHashedPassword", p);
        }

        public virtual int? VersionAdd(int major, int minor, int build, DateTime? dateCreatedUtc)
        {
            var outParam0 = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
            SqlParameter[] p = {
				DataHelper.MakeInParam("@Major", major),		
				DataHelper.MakeInParam("@Minor", minor),		
				DataHelper.MakeInParam("@Build", build),		
				DataHelper.MakeInParam("@DateCreatedUtc", dateCreatedUtc),		
				outParam0,
			};

            NonQueryInt("subtext_VersionAdd", p);
            if (outParam0.Value == null)
            {
                return null;
            }
            return (int)outParam0.Value;
        }

        public virtual IDataReader VersionGetCurrent()
        {
            return GetReader("subtext_VersionGetCurrent");
        }

    }
}
