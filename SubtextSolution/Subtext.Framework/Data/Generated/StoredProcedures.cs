using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using SubSonic;
using SubSonic.Utilities;

namespace Subtext.Data{
    public partial class StoredProcedures{
        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_GetUserByName Procedure
        /// </summary>
        public static StoredProcedure MembershipGetUserByName(string UserName, DateTime? CurrentTimeUtc, bool? UpdateLastActivity)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_GetUserByName" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddParameter("@UpdateLastActivity", UpdateLastActivity,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_GetUserByUserId Procedure
        /// </summary>
        public static StoredProcedure MembershipGetUserByUserId(Guid? UserId, DateTime? CurrentTimeUtc, bool? UpdateLastActivity)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_GetUserByUserId" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserId", UserId,DbType.Guid);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddParameter("@UpdateLastActivity", UpdateLastActivity,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetFeedback Procedure
        /// </summary>
        public static StoredProcedure GetFeedback(int? Id)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetFeedback" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Id", Id,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetMetaTagsForEntry Procedure
        /// </summary>
        public static StoredProcedure GetMetaTagsForEntry(int? BlogId, int? EntryId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetMetaTagsForEntry" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryId", EntryId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetImageCategory Procedure
        /// </summary>
        public static StoredProcedure GetImageCategory(int? CategoryID, bool? IsActive, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetImageCategory" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@IsActive", IsActive,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteMetaTag Procedure
        /// </summary>
        public static StoredProcedure DeleteMetaTag(int? Id)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteMetaTag" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Id", Id,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetKeyWord Procedure
        /// </summary>
        public static StoredProcedure GetKeyWord(int? KeyWordID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetKeyWord" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@KeyWordID", KeyWordID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetBlogByDomainAlias Procedure
        /// </summary>
        public static StoredProcedure GetBlogByDomainAlias(string Host, string Application, bool? Strict)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetBlogByDomainAlias" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@Application", Application,DbType.String);
        	    
            sp.Command.AddParameter("@Strict", Strict,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetLinkCollectionByPostID Procedure
        /// </summary>
        public static StoredProcedure GetLinkCollectionByPostID(int? PostID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetLinkCollectionByPostID" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PostID", PostID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableDomainAliases Procedure
        /// </summary>
        public static StoredProcedure GetPageableDomainAliases(int? PageIndex, int? PageSize, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableDomainAliases" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetLinksByCategoryID Procedure
        /// </summary>
        public static StoredProcedure GetLinksByCategoryID(int? CategoryID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetLinksByCategoryID" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_CreateDomainAlias Procedure
        /// </summary>
        public static StoredProcedure CreateDomainAlias(int? BlogId, string Host, string Application, bool? Active, int? AliasId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_CreateDomainAlias" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@Application", Application,DbType.String);
        	    
            sp.Command.AddParameter("@Active", Active,DbType.Boolean);
        	    
            sp.Command.AddOutputParameter("@AliasId",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableEntries Procedure
        /// </summary>
        public static StoredProcedure GetPageableEntries(int? BlogId, int? PageIndex, int? PageSize, int? PostType)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableEntries" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddParameter("@PostType", PostType,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteDomainAlias Procedure
        /// </summary>
        public static StoredProcedure DeleteDomainAlias(int? AliasId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteDomainAlias" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@AliasId", AliasId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableEntriesByCategoryID Procedure
        /// </summary>
        public static StoredProcedure GetPageableEntriesByCategoryID(int? BlogId, int? CategoryID, int? PageIndex, int? PageSize, int? PostType)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableEntriesByCategoryID" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddParameter("@PostType", PostType,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateDomainAlias Procedure
        /// </summary>
        public static StoredProcedure UpdateDomainAlias(int? AliasId, int? BlogId, string Host, string Application, bool? Active)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateDomainAlias" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@AliasId", AliasId,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@Application", Application,DbType.String);
        	    
            sp.Command.AddParameter("@Active", Active,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableFeedback Procedure
        /// </summary>
        public static StoredProcedure GetPageableFeedback(int? BlogId, int? PageIndex, int? PageSize, int? StatusFlag, int? ExcludeFeedbackStatusMask, int? FeedbackType)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableFeedback" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddParameter("@StatusFlag", StatusFlag,DbType.Int32);
        	    
            sp.Command.AddParameter("@ExcludeFeedbackStatusMask", ExcludeFeedbackStatusMask,DbType.Int32);
        	    
            sp.Command.AddParameter("@FeedbackType", FeedbackType,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetDomainAliasById Procedure
        /// </summary>
        public static StoredProcedure GetDomainAliasById(int? AliasId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetDomainAliasById" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@AliasId", AliasId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableLogEntries Procedure
        /// </summary>
        public static StoredProcedure GetPageableLogEntries(int? BlogId, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableLogEntries" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableKeyWords Procedure
        /// </summary>
        public static StoredProcedure GetPageableKeyWords(int? BlogId, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableKeyWords" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableLinks Procedure
        /// </summary>
        public static StoredProcedure GetPageableLinks(int? BlogId, int? CategoryId, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableLinks" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@CategoryId", CategoryId,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableLinksByCategoryID Procedure
        /// </summary>
        public static StoredProcedure GetPageableLinksByCategoryID(int? BlogId, int? CategoryID, int? PageIndex, int? PageSize, bool? SortDesc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableLinksByCategoryID" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddParameter("@SortDesc", SortDesc,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableReferrers Procedure
        /// </summary>
        public static StoredProcedure GetPageableReferrers(int? BlogId, int? EntryID, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableReferrers" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryID", EntryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPostsByCategoryID Procedure
        /// </summary>
        public static StoredProcedure GetPostsByCategoryID(int? ItemCount, int? CategoryID, bool? IsActive, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPostsByCategoryID" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ItemCount", ItemCount,DbType.Int32);
        	    
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@IsActive", IsActive,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPostsByDayRange Procedure
        /// </summary>
        public static StoredProcedure GetPostsByDayRange(DateTime? StartDate, DateTime? StopDate, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPostsByDayRange" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@StartDate", StartDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@StopDate", StopDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPostsByMonth Procedure
        /// </summary>
        public static StoredProcedure GetPostsByMonth(int? Month, int? Year, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPostsByMonth" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Month", Month,DbType.Int32);
        	    
            sp.Command.AddParameter("@Year", Year,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPostsByMonthArchive Procedure
        /// </summary>
        public static StoredProcedure GetPostsByMonthArchive(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPostsByMonthArchive" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPostsByYearArchive Procedure
        /// </summary>
        public static StoredProcedure GetPostsByYearArchive(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPostsByYearArchive" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetSingleDay Procedure
        /// </summary>
        public static StoredProcedure GetSingleDay(DateTime? Date, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetSingleDay" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Date", Date,DbType.DateTime);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetSingleEntry Procedure
        /// </summary>
        public static StoredProcedure GetSingleEntry(int? ID, string EntryName, bool? IsActive, int? BlogId, bool? IncludeCategories)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetSingleEntry" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ID", ID,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryName", EntryName,DbType.String);
        	    
            sp.Command.AddParameter("@IsActive", IsActive,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@IncludeCategories", IncludeCategories,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetSingleImage Procedure
        /// </summary>
        public static StoredProcedure GetSingleImage(int? ImageID, bool? IsActive, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetSingleImage" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ImageID", ImageID,DbType.Int32);
        	    
            sp.Command.AddParameter("@IsActive", IsActive,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetSingleLink Procedure
        /// </summary>
        public static StoredProcedure GetSingleLink(int? LinkID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetSingleLink" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@LinkID", LinkID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetUrlID Procedure
        /// </summary>
        public static StoredProcedure GetUrlID(string Url, int? UrlID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetUrlID" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            sp.Command.AddOutputParameter("@UrlID",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertCategory Procedure
        /// </summary>
        public static StoredProcedure InsertCategory(string Title, bool? Active, int? BlogId, short? CategoryType, string Description, int? CategoryID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertCategory" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@Active", Active,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@CategoryType", CategoryType,DbType.Int16);
        	    
            sp.Command.AddParameter("@Description", Description,DbType.String);
        	    
            sp.Command.AddOutputParameter("@CategoryID",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertEntryViewCount Procedure
        /// </summary>
        public static StoredProcedure InsertEntryViewCount(int? EntryID, int? BlogId, bool? IsWeb)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertEntryViewCount" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@EntryID", EntryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@IsWeb", IsWeb,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertImage Procedure
        /// </summary>
        public static StoredProcedure InsertImage(string Title, int? CategoryID, int? Width, int? Height, string File, bool? Active, int? BlogId, int? ImageID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertImage" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Width", Width,DbType.Int32);
        	    
            sp.Command.AddParameter("@Height", Height,DbType.Int32);
        	    
            sp.Command.AddParameter("@File", File,DbType.String);
        	    
            sp.Command.AddParameter("@Active", Active,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@ImageID",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertKeyWord Procedure
        /// </summary>
        public static StoredProcedure InsertKeyWord(string Word, string Rel, string Text, bool? ReplaceFirstTimeOnly, bool? OpenInNewWindow, bool? CaseSensitive, string Url, string Title, int? BlogId, int? KeyWordID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertKeyWord" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Word", Word,DbType.String);
        	    
            sp.Command.AddParameter("@Rel", Rel,DbType.String);
        	    
            sp.Command.AddParameter("@Text", Text,DbType.String);
        	    
            sp.Command.AddParameter("@ReplaceFirstTimeOnly", ReplaceFirstTimeOnly,DbType.Boolean);
        	    
            sp.Command.AddParameter("@OpenInNewWindow", OpenInNewWindow,DbType.Boolean);
        	    
            sp.Command.AddParameter("@CaseSensitive", CaseSensitive,DbType.Boolean);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@KeyWordID",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertLink Procedure
        /// </summary>
        public static StoredProcedure InsertLink(string Title, string Url, string Rss, bool? Active, bool? NewWindow, int? CategoryID, int? PostID, int? BlogId, int? LinkID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertLink" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            sp.Command.AddParameter("@Rss", Rss,DbType.String);
        	    
            sp.Command.AddParameter("@Active", Active,DbType.Boolean);
        	    
            sp.Command.AddParameter("@NewWindow", NewWindow,DbType.Boolean);
        	    
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@PostID", PostID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@LinkID",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertLinkCategoryList Procedure
        /// </summary>
        public static StoredProcedure InsertLinkCategoryList(string CategoryList, int? PostID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertLinkCategoryList" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@CategoryList", CategoryList,DbType.String);
        	    
            sp.Command.AddParameter("@PostID", PostID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertReferral Procedure
        /// </summary>
        public static StoredProcedure InsertReferral(int? EntryID, int? BlogId, string Url)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertReferral" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@EntryID", EntryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertViewStats Procedure
        /// </summary>
        public static StoredProcedure InsertViewStats(int? BlogId, short? PageType, int? PostID, DateTime? Day, string Url)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertViewStats" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageType", PageType,DbType.Int16);
        	    
            sp.Command.AddParameter("@PostID", PostID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Day", Day,DbType.DateTime);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_StatsSummary Procedure
        /// </summary>
        public static StoredProcedure StatsSummary(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_StatsSummary" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_TrackEntry Procedure
        /// </summary>
        public static StoredProcedure TrackEntry(int? EntryID, int? BlogId, string Url, bool? IsWeb)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_TrackEntry" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@EntryID", EntryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            sp.Command.AddParameter("@IsWeb", IsWeb,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateCategory Procedure
        /// </summary>
        public static StoredProcedure UpdateCategory(int? CategoryID, string Title, bool? Active, short? CategoryType, string Description, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateCategory" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@Active", Active,DbType.Boolean);
        	    
            sp.Command.AddParameter("@CategoryType", CategoryType,DbType.Int16);
        	    
            sp.Command.AddParameter("@Description", Description,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateConfig Procedure
        /// </summary>
        public static StoredProcedure UpdateConfig(Guid? OwnerId, string Title, string SubTitle, string Skin, string Subfolder, string Host, string Language, int? TimeZone, int? ItemCount, int? CategoryListPostCount, string News, string CustomMetaTags, string TrackingCode, DateTime? LastUpdated, string SecondaryCss, string SkinCssFile, int? Flag, int? BlogId, string LicenseUrl, int? DaysTillCommentsClose, int? CommentDelayInMinutes, int? NumberOfRecentComments, int? RecentCommentsLength, string AkismetAPIKey, string FeedBurnerName, string pop3User, string pop3Pass, string pop3Server, string pop3StartTag, string pop3EndTag, string pop3SubjectPrefix, bool? pop3MTBEnable, bool? pop3DeleteOnlyProcessed, bool? pop3InlineAttachedPictures, int? pop3HeightForThumbs)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateConfig" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@OwnerId", OwnerId,DbType.Guid);
        	    
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@SubTitle", SubTitle,DbType.String);
        	    
            sp.Command.AddParameter("@Skin", Skin,DbType.String);
        	    
            sp.Command.AddParameter("@Subfolder", Subfolder,DbType.String);
        	    
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@Language", Language,DbType.String);
        	    
            sp.Command.AddParameter("@TimeZone", TimeZone,DbType.Int32);
        	    
            sp.Command.AddParameter("@ItemCount", ItemCount,DbType.Int32);
        	    
            sp.Command.AddParameter("@CategoryListPostCount", CategoryListPostCount,DbType.Int32);
        	    
            sp.Command.AddParameter("@News", News,DbType.String);
        	    
            sp.Command.AddParameter("@CustomMetaTags", CustomMetaTags,DbType.String);
        	    
            sp.Command.AddParameter("@TrackingCode", TrackingCode,DbType.String);
        	    
            sp.Command.AddParameter("@LastUpdated", LastUpdated,DbType.DateTime);
        	    
            sp.Command.AddParameter("@SecondaryCss", SecondaryCss,DbType.String);
        	    
            sp.Command.AddParameter("@SkinCssFile", SkinCssFile,DbType.String);
        	    
            sp.Command.AddParameter("@Flag", Flag,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@LicenseUrl", LicenseUrl,DbType.String);
        	    
            sp.Command.AddParameter("@DaysTillCommentsClose", DaysTillCommentsClose,DbType.Int32);
        	    
            sp.Command.AddParameter("@CommentDelayInMinutes", CommentDelayInMinutes,DbType.Int32);
        	    
            sp.Command.AddParameter("@NumberOfRecentComments", NumberOfRecentComments,DbType.Int32);
        	    
            sp.Command.AddParameter("@RecentCommentsLength", RecentCommentsLength,DbType.Int32);
        	    
            sp.Command.AddParameter("@AkismetAPIKey", AkismetAPIKey,DbType.String);
        	    
            sp.Command.AddParameter("@FeedBurnerName", FeedBurnerName,DbType.String);
        	    
            sp.Command.AddParameter("@pop3User", pop3User,DbType.String);
        	    
            sp.Command.AddParameter("@pop3Pass", pop3Pass,DbType.String);
        	    
            sp.Command.AddParameter("@pop3Server", pop3Server,DbType.String);
        	    
            sp.Command.AddParameter("@pop3StartTag", pop3StartTag,DbType.String);
        	    
            sp.Command.AddParameter("@pop3EndTag", pop3EndTag,DbType.String);
        	    
            sp.Command.AddParameter("@pop3SubjectPrefix", pop3SubjectPrefix,DbType.String);
        	    
            sp.Command.AddParameter("@pop3MTBEnable", pop3MTBEnable,DbType.Boolean);
        	    
            sp.Command.AddParameter("@pop3DeleteOnlyProcessed", pop3DeleteOnlyProcessed,DbType.Boolean);
        	    
            sp.Command.AddParameter("@pop3InlineAttachedPictures", pop3InlineAttachedPictures,DbType.Boolean);
        	    
            sp.Command.AddParameter("@pop3HeightForThumbs", pop3HeightForThumbs,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateConfigUpdateTime Procedure
        /// </summary>
        public static StoredProcedure UpdateConfigUpdateTime(int? BlogId, DateTime? LastUpdated)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateConfigUpdateTime" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@LastUpdated", LastUpdated,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateEntry Procedure
        /// </summary>
        public static StoredProcedure UpdateEntry(int? ID, string Title, string Text, int? PostType, Guid? AuthorId, string Description, DateTime? DateUpdated, int? PostConfig, string EntryName, DateTime? DateSyndicated, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateEntry" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ID", ID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@Text", Text,DbType.String);
        	    
            sp.Command.AddParameter("@PostType", PostType,DbType.Int32);
        	    
            sp.Command.AddParameter("@AuthorId", AuthorId,DbType.Guid);
        	    
            sp.Command.AddParameter("@Description", Description,DbType.String);
        	    
            sp.Command.AddParameter("@DateUpdated", DateUpdated,DbType.DateTime);
        	    
            sp.Command.AddParameter("@PostConfig", PostConfig,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryName", EntryName,DbType.String);
        	    
            sp.Command.AddParameter("@DateSyndicated", DateSyndicated,DbType.DateTime);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateImage Procedure
        /// </summary>
        public static StoredProcedure UpdateImage(string Title, int? CategoryID, int? Width, int? Height, string File, bool? Active, int? BlogId, int? ImageID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateImage" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Width", Width,DbType.Int32);
        	    
            sp.Command.AddParameter("@Height", Height,DbType.Int32);
        	    
            sp.Command.AddParameter("@File", File,DbType.String);
        	    
            sp.Command.AddParameter("@Active", Active,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@ImageID", ImageID,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateKeyWord Procedure
        /// </summary>
        public static StoredProcedure UpdateKeyWord(int? KeyWordID, string Word, string Rel, string Text, bool? ReplaceFirstTimeOnly, bool? OpenInNewWindow, bool? CaseSensitive, string Url, string Title, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateKeyWord" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@KeyWordID", KeyWordID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Word", Word,DbType.String);
        	    
            sp.Command.AddParameter("@Rel", Rel,DbType.String);
        	    
            sp.Command.AddParameter("@Text", Text,DbType.String);
        	    
            sp.Command.AddParameter("@ReplaceFirstTimeOnly", ReplaceFirstTimeOnly,DbType.Boolean);
        	    
            sp.Command.AddParameter("@OpenInNewWindow", OpenInNewWindow,DbType.Boolean);
        	    
            sp.Command.AddParameter("@CaseSensitive", CaseSensitive,DbType.Boolean);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateLink Procedure
        /// </summary>
        public static StoredProcedure UpdateLink(int? LinkID, string Title, string Url, string Rss, bool? Active, bool? NewWindow, int? CategoryID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateLink" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@LinkID", LinkID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            sp.Command.AddParameter("@Rss", Rss,DbType.String);
        	    
            sp.Command.AddParameter("@Active", Active,DbType.Boolean);
        	    
            sp.Command.AddParameter("@NewWindow", NewWindow,DbType.Boolean);
        	    
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPageableBlogs Procedure
        /// </summary>
        public static StoredProcedure GetPageableBlogs(int? PageIndex, int? PageSize, string Host, int? ConfigurationFlags)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPageableBlogs" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@ConfigurationFlags", ConfigurationFlags,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertFeedback Procedure
        /// </summary>
        public static StoredProcedure InsertFeedback(string Title, string Body, int? BlogId, int? EntryId, string Author, bool? IsBlogAuthor, string Email, string Url, int? FeedbackType, int? StatusFlag, bool? CommentAPI, string Referrer, string IpAddress, string UserAgent, string FeedbackChecksumHash, DateTime? DateCreated, DateTime? DateModified, int? Id)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertFeedback" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@Body", Body,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryId", EntryId,DbType.Int32);
        	    
            sp.Command.AddParameter("@Author", Author,DbType.String);
        	    
            sp.Command.AddParameter("@IsBlogAuthor", IsBlogAuthor,DbType.Boolean);
        	    
            sp.Command.AddParameter("@Email", Email,DbType.String);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            sp.Command.AddParameter("@FeedbackType", FeedbackType,DbType.Int32);
        	    
            sp.Command.AddParameter("@StatusFlag", StatusFlag,DbType.Int32);
        	    
            sp.Command.AddParameter("@CommentAPI", CommentAPI,DbType.Boolean);
        	    
            sp.Command.AddParameter("@Referrer", Referrer,DbType.String);
        	    
            sp.Command.AddParameter("@IpAddress", IpAddress,DbType.String);
        	    
            sp.Command.AddParameter("@UserAgent", UserAgent,DbType.String);
        	    
            sp.Command.AddParameter("@FeedbackChecksumHash", FeedbackChecksumHash,DbType.String);
        	    
            sp.Command.AddParameter("@DateCreated", DateCreated,DbType.DateTime);
        	    
            sp.Command.AddParameter("@DateModified", DateModified,DbType.DateTime);
        	    
            sp.Command.AddOutputParameter("@Id",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateFeedback Procedure
        /// </summary>
        public static StoredProcedure UpdateFeedback(int? ID, string Title, string Body, string Author, string Email, string Url, int? StatusFlag, string FeedbackChecksumHash, DateTime? DateModified)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateFeedback" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ID", ID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@Body", Body,DbType.String);
        	    
            sp.Command.AddParameter("@Author", Author,DbType.String);
        	    
            sp.Command.AddParameter("@Email", Email,DbType.String);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            sp.Command.AddParameter("@StatusFlag", StatusFlag,DbType.Int32);
        	    
            sp.Command.AddParameter("@FeedbackChecksumHash", FeedbackChecksumHash,DbType.String);
        	    
            sp.Command.AddParameter("@DateModified", DateModified,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertEntry Procedure
        /// </summary>
        public static StoredProcedure InsertEntry(string Title, Guid? AuthorId, string Text, int? PostType, string Description, int? BlogId, DateTime? DateAdded, int? PostConfig, string EntryName, DateTime? DateSyndicated, int? ID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertEntry" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@AuthorId", AuthorId,DbType.Guid);
        	    
            sp.Command.AddParameter("@Text", Text,DbType.String);
        	    
            sp.Command.AddParameter("@PostType", PostType,DbType.Int32);
        	    
            sp.Command.AddParameter("@Description", Description,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@DateAdded", DateAdded,DbType.DateTime);
        	    
            sp.Command.AddParameter("@PostConfig", PostConfig,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryName", EntryName,DbType.String);
        	    
            sp.Command.AddParameter("@DateSyndicated", DateSyndicated,DbType.DateTime);
        	    
            sp.Command.AddOutputParameter("@ID",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetCommentByChecksumHash Procedure
        /// </summary>
        public static StoredProcedure GetCommentByChecksumHash(string FeedbackChecksumHash, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetCommentByChecksumHash" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@FeedbackChecksumHash", FeedbackChecksumHash,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the DNW_GetRecentPosts Procedure
        /// </summary>
        public static StoredProcedure DNWGetRecentPosts(string Host, int? GroupID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("DNW_GetRecentPosts" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@GroupID", GroupID,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the DNW_Stats Procedure
        /// </summary>
        public static StoredProcedure DNWStats(string Host, int? GroupID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("DNW_Stats" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@GroupID", GroupID,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the DNW_Total_Stats Procedure
        /// </summary>
        public static StoredProcedure DNWTotalStats(string Host, int? GroupID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("DNW_Total_Stats" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@GroupID", GroupID,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the DNW_HomePageData Procedure
        /// </summary>
        public static StoredProcedure DNWHomePageData(string Host, int? GroupID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("DNW_HomePageData" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@GroupID", GroupID,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_VersionGetCurrent Procedure
        /// </summary>
        public static StoredProcedure VersionGetCurrent()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_VersionGetCurrent" , DataService.GetInstance("Subtext"));
        	
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_VersionAdd Procedure
        /// </summary>
        public static StoredProcedure VersionAdd(int? Major, int? Minor, int? Build, DateTime? DateCreated, int? Id)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_VersionAdd" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Major", Major,DbType.Int32);
        	    
            sp.Command.AddParameter("@Minor", Minor,DbType.Int32);
        	    
            sp.Command.AddParameter("@Build", Build,DbType.Int32);
        	    
            sp.Command.AddParameter("@DateCreated", DateCreated,DbType.DateTime);
        	    
            sp.Command.AddOutputParameter("@Id",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_LogClear Procedure
        /// </summary>
        public static StoredProcedure LogClear(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_LogClear" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_AddLogEntry Procedure
        /// </summary>
        public static StoredProcedure AddLogEntry(DateTime? Date, int? BlogId, string Thread, string Context, string Level, string Logger, string Message, string Exception, string Url)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_AddLogEntry" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Date", Date,DbType.DateTime);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@Thread", Thread,DbType.String);
        	    
            sp.Command.AddParameter("@Context", Context,DbType.String);
        	    
            sp.Command.AddParameter("@Level", Level,DbType.String);
        	    
            sp.Command.AddParameter("@Logger", Logger,DbType.String);
        	    
            sp.Command.AddParameter("@Message", Message,DbType.String);
        	    
            sp.Command.AddParameter("@Exception", Exception,DbType.String);
        	    
            sp.Command.AddParameter("@Url", Url,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_SearchEntries Procedure
        /// </summary>
        public static StoredProcedure SearchEntries(int? BlogId, string SearchStr)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_SearchEntries" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@SearchStr", SearchStr,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetEntry_PreviousNext Procedure
        /// </summary>
        public static StoredProcedure GetEntryPreviousNext(int? ID, int? PostType, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetEntry_PreviousNext" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ID", ID,DbType.Int32);
        	    
            sp.Command.AddParameter("@PostType", PostType,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetRelatedLinks Procedure
        /// </summary>
        public static StoredProcedure GetRelatedLinks(int? BlogId, int? EntryID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetRelatedLinks" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryID", EntryID,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetTop10byBlogId Procedure
        /// </summary>
        public static StoredProcedure GetTop10byBlogId(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetTop10byBlogId" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetEntriesForBlogMl Procedure
        /// </summary>
        public static StoredProcedure GetEntriesForBlogMl(int? BlogId, int? PageIndex, int? PageSize)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetEntriesForBlogMl" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPostsByCategoriesArchive Procedure
        /// </summary>
        public static StoredProcedure GetPostsByCategoriesArchive(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPostsByCategoriesArchive" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetBlogKeyWords Procedure
        /// </summary>
        public static StoredProcedure GetBlogKeyWords(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetBlogKeyWords" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_ClearBlogContent Procedure
        /// </summary>
        public static StoredProcedure ClearBlogContent(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_ClearBlogContent" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPostsByTag Procedure
        /// </summary>
        public static StoredProcedure GetPostsByTag(int? ItemCount, string Tag, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPostsByTag" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ItemCount", ItemCount,DbType.Int32);
        	    
            sp.Command.AddParameter("@Tag", Tag,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertEntryTagList Procedure
        /// </summary>
        public static StoredProcedure InsertEntryTagList(int? EntryId, int? BlogId, string TagList)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertEntryTagList" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@EntryId", EntryId,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@TagList", TagList,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetTopTags Procedure
        /// </summary>
        public static StoredProcedure GetTopTags(int? ItemCount, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetTopTags" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ItemCount", ItemCount,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdatePluginData Procedure
        /// </summary>
        public static StoredProcedure UpdatePluginData(Guid? PluginID, int? BlogID, int? EntryID, string Key, string Value)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdatePluginData" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PluginID", PluginID,DbType.Guid);
        	    
            sp.Command.AddParameter("@BlogID", BlogID,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryID", EntryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Key", Key,DbType.String);
        	    
            sp.Command.AddParameter("@Value", Value,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertPluginData Procedure
        /// </summary>
        public static StoredProcedure InsertPluginData(Guid? PluginID, int? BlogID, int? EntryID, string Key, string Value)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertPluginData" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PluginID", PluginID,DbType.Guid);
        	    
            sp.Command.AddParameter("@BlogID", BlogID,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryID", EntryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@Key", Key,DbType.String);
        	    
            sp.Command.AddParameter("@Value", Value,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeletePluginBlog Procedure
        /// </summary>
        public static StoredProcedure DeletePluginBlog(Guid? PluginID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeletePluginBlog" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PluginID", PluginID,DbType.Guid);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPluginBlog Procedure
        /// </summary>
        public static StoredProcedure GetPluginBlog(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPluginBlog" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertPluginBlog Procedure
        /// </summary>
        public static StoredProcedure InsertPluginBlog(Guid? PluginID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertPluginBlog" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PluginID", PluginID,DbType.Guid);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetPluginData Procedure
        /// </summary>
        public static StoredProcedure GetPluginData(Guid? PluginID, int? BlogId, int? EntryId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetPluginData" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PluginID", PluginID,DbType.Guid);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryId", EntryId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Setup_RestorePermissions Procedure
        /// </summary>
        public static StoredProcedure SetupRestorePermissions(string name)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Setup_RestorePermissions" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@name", name,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Setup_RemoveAllRoleMembers Procedure
        /// </summary>
        public static StoredProcedure SetupRemoveAllRoleMembers(string name)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Setup_RemoveAllRoleMembers" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@name", name,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_RegisterSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure RegisterSchemaVersion(string Feature, string CompatibleSchemaVersion, bool? IsCurrentVersion, bool? RemoveIncompatibleSchema)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_RegisterSchemaVersion" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Feature", Feature,DbType.String);
        	    
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion,DbType.String);
        	    
            sp.Command.AddParameter("@IsCurrentVersion", IsCurrentVersion,DbType.Boolean);
        	    
            sp.Command.AddParameter("@RemoveIncompatibleSchema", RemoveIncompatibleSchema,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_CheckSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure CheckSchemaVersion(string Feature, string CompatibleSchemaVersion)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_CheckSchemaVersion" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Feature", Feature,DbType.String);
        	    
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UnRegisterSchemaVersion Procedure
        /// </summary>
        public static StoredProcedure UnRegisterSchemaVersion(string Feature, string CompatibleSchemaVersion)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UnRegisterSchemaVersion" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Feature", Feature,DbType.String);
        	    
            sp.Command.AddParameter("@CompatibleSchemaVersion", CompatibleSchemaVersion,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_GetUserByEmail Procedure
        /// </summary>
        public static StoredProcedure MembershipGetUserByEmail(string Email)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_GetUserByEmail" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Email", Email,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_GetPasswordWithFormat Procedure
        /// </summary>
        public static StoredProcedure MembershipGetPasswordWithFormat(string UserName, bool? UpdateLastLoginActivityDate, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_GetPasswordWithFormat" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@UpdateLastLoginActivityDate", UpdateLastLoginActivityDate,DbType.Boolean);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_UpdateUserInfo Procedure
        /// </summary>
        public static StoredProcedure MembershipUpdateUserInfo(string UserName, bool? IsPasswordCorrect, bool? UpdateLastLoginActivityDate, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, DateTime? CurrentTimeUtc, DateTime? LastLoginDate, DateTime? LastActivityDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_UpdateUserInfo" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@IsPasswordCorrect", IsPasswordCorrect,DbType.Boolean);
        	    
            sp.Command.AddParameter("@UpdateLastLoginActivityDate", UpdateLastLoginActivityDate,DbType.Boolean);
        	    
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts,DbType.Int32);
        	    
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow,DbType.Int32);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddParameter("@LastLoginDate", LastLoginDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@LastActivityDate", LastActivityDate,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_GetPassword Procedure
        /// </summary>
        public static StoredProcedure MembershipGetPassword(string UserName, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, DateTime? CurrentTimeUtc, string PasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_GetPassword" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts,DbType.Int32);
        	    
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow,DbType.Int32);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_GetNumberOfUsersOnline Procedure
        /// </summary>
        public static StoredProcedure MembershipGetNumberOfUsersOnline(int? MinutesSinceLastInActive, DateTime? CurrentTimeUtc, int? OnlineUserCount)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_GetNumberOfUsersOnline" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@MinutesSinceLastInActive", MinutesSinceLastInActive,DbType.Int32);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddOutputParameter("@OnlineUserCount",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_SetPassword Procedure
        /// </summary>
        public static StoredProcedure MembershipSetPassword(string UserName, string NewPassword, string PasswordSalt, DateTime? CurrentTimeUtc, int? PasswordFormat)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_SetPassword" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@NewPassword", NewPassword,DbType.String);
        	    
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt,DbType.String);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_ResetPassword Procedure
        /// </summary>
        public static StoredProcedure MembershipResetPassword(string UserName, string NewPassword, int? MaxInvalidPasswordAttempts, int? PasswordAttemptWindow, string PasswordSalt, DateTime? CurrentTimeUtc, int? PasswordFormat, string PasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_ResetPassword" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@NewPassword", NewPassword,DbType.String);
        	    
            sp.Command.AddParameter("@MaxInvalidPasswordAttempts", MaxInvalidPasswordAttempts,DbType.Int32);
        	    
            sp.Command.AddParameter("@PasswordAttemptWindow", PasswordAttemptWindow,DbType.Int32);
        	    
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt,DbType.String);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat,DbType.Int32);
        	    
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_UnlockUser Procedure
        /// </summary>
        public static StoredProcedure MembershipUnlockUser(string UserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_UnlockUser" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_UpdateUser Procedure
        /// </summary>
        public static StoredProcedure MembershipUpdateUser(string UserName, string Email, string Comment, bool? IsApproved, DateTime? LastLoginDate, DateTime? LastActivityDate, int? UniqueEmail, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_UpdateUser" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@Email", Email,DbType.String);
        	    
            sp.Command.AddParameter("@Comment", Comment,DbType.String);
        	    
            sp.Command.AddParameter("@IsApproved", IsApproved,DbType.Boolean);
        	    
            sp.Command.AddParameter("@LastLoginDate", LastLoginDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@LastActivityDate", LastActivityDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@UniqueEmail", UniqueEmail,DbType.Int32);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_ChangePasswordQuestionAndAnswer Procedure
        /// </summary>
        public static StoredProcedure MembershipChangePasswordQuestionAndAnswer(string UserName, string NewPasswordQuestion, string NewPasswordAnswer)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_ChangePasswordQuestionAndAnswer" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@NewPasswordQuestion", NewPasswordQuestion,DbType.String);
        	    
            sp.Command.AddParameter("@NewPasswordAnswer", NewPasswordAnswer,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Profile_GetProperties Procedure
        /// </summary>
        public static StoredProcedure ProfileGetProperties(string UserName, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Profile_GetProperties" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Profile_DeleteInactiveProfiles Procedure
        /// </summary>
        public static StoredProcedure ProfileDeleteInactiveProfiles(int? ProfileAuthOptions, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Profile_DeleteInactiveProfiles" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions,DbType.Int32);
        	    
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Profile_GetNumberOfInactiveProfiles Procedure
        /// </summary>
        public static StoredProcedure ProfileGetNumberOfInactiveProfiles(int? ProfileAuthOptions, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Profile_GetNumberOfInactiveProfiles" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions,DbType.Int32);
        	    
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UsersInRoles_IsUserInRole Procedure
        /// </summary>
        public static StoredProcedure UsersInRolesIsUserInRole(string ApplicationName, string UserName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UsersInRoles_IsUserInRole" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@RoleName", RoleName,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UsersInRoles_GetRolesForUser Procedure
        /// </summary>
        public static StoredProcedure UsersInRolesGetRolesForUser(string ApplicationName, string UserName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UsersInRoles_GetRolesForUser" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Roles_DeleteRole Procedure
        /// </summary>
        public static StoredProcedure RolesDeleteRole(string ApplicationName, string RoleName, bool? DeleteOnlyIfRoleIsEmpty)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Roles_DeleteRole" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@RoleName", RoleName,DbType.String);
        	    
            sp.Command.AddParameter("@DeleteOnlyIfRoleIsEmpty", DeleteOnlyIfRoleIsEmpty,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Roles_RoleExists Procedure
        /// </summary>
        public static StoredProcedure RolesRoleExists(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Roles_RoleExists" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@RoleName", RoleName,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UsersInRoles_AddUsersToRoles Procedure
        /// </summary>
        public static StoredProcedure UsersInRolesAddUsersToRoles(string ApplicationName, string UserNames, string RoleNames, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UsersInRoles_AddUsersToRoles" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@UserNames", UserNames,DbType.String);
        	    
            sp.Command.AddParameter("@RoleNames", RoleNames,DbType.String);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UsersInRoles_RemoveUsersFromRoles Procedure
        /// </summary>
        public static StoredProcedure UsersInRolesRemoveUsersFromRoles(string ApplicationName, string UserNames, string RoleNames)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UsersInRoles_RemoveUsersFromRoles" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@UserNames", UserNames,DbType.String);
        	    
            sp.Command.AddParameter("@RoleNames", RoleNames,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Roles_GetAllRoles Procedure
        /// </summary>
        public static StoredProcedure RolesGetAllRoles(string ApplicationName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Roles_GetAllRoles" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UsersInRoles_GetUsersInRoles Procedure
        /// </summary>
        public static StoredProcedure UsersInRolesGetUsersInRoles(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UsersInRoles_GetUsersInRoles" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@RoleName", RoleName,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UsersInRoles_FindUsersInRole Procedure
        /// </summary>
        public static StoredProcedure UsersInRolesFindUsersInRole(string ApplicationName, string RoleName, string UserNameToMatch)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UsersInRoles_FindUsersInRole" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@RoleName", RoleName,DbType.String);
        	    
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Paths_CreatePath Procedure
        /// </summary>
        public static StoredProcedure PathsCreatePath(Guid? ApplicationId, string Path, Guid? PathId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Paths_CreatePath" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationId", ApplicationId,DbType.Guid);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            sp.Command.AddOutputParameter("@PathId",DbType.Guid);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_WebEvent_LogEvent Procedure
        /// </summary>
        public static StoredProcedure WebEventLogEvent(string EventId, DateTime? EventTimeUtc, DateTime? EventTime, string EventType, decimal? EventSequence, decimal? EventOccurrence, int? EventCode, int? EventDetailCode, string Message, string ApplicationPath, string ApplicationVirtualPath, string MachineName, string RequestUrl, string ExceptionType, string Details)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_WebEvent_LogEvent" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@EventId", EventId,DbType.AnsiStringFixedLength);
        	    
            sp.Command.AddParameter("@EventTimeUtc", EventTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddParameter("@EventTime", EventTime,DbType.DateTime);
        	    
            sp.Command.AddParameter("@EventType", EventType,DbType.String);
        	    
            sp.Command.AddParameter("@EventSequence", EventSequence,DbType.Decimal);
        	    
            sp.Command.AddParameter("@EventOccurrence", EventOccurrence,DbType.Decimal);
        	    
            sp.Command.AddParameter("@EventCode", EventCode,DbType.Int32);
        	    
            sp.Command.AddParameter("@EventDetailCode", EventDetailCode,DbType.Int32);
        	    
            sp.Command.AddParameter("@Message", Message,DbType.String);
        	    
            sp.Command.AddParameter("@ApplicationPath", ApplicationPath,DbType.String);
        	    
            sp.Command.AddParameter("@ApplicationVirtualPath", ApplicationVirtualPath,DbType.String);
        	    
            sp.Command.AddParameter("@MachineName", MachineName,DbType.String);
        	    
            sp.Command.AddParameter("@RequestUrl", RequestUrl,DbType.String);
        	    
            sp.Command.AddParameter("@ExceptionType", ExceptionType,DbType.String);
        	    
            sp.Command.AddParameter("@Details", Details,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Personalization_GetApplicationId Procedure
        /// </summary>
        public static StoredProcedure PersonalizationGetApplicationId(string ApplicationName, Guid? ApplicationId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Personalization_GetApplicationId" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddOutputParameter("@ApplicationId",DbType.Guid);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Profile_GetProfiles Procedure
        /// </summary>
        public static StoredProcedure ProfileGetProfiles(string ApplicationName, int? ProfileAuthOptions, int? PageIndex, int? PageSize, string UserNameToMatch, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Profile_GetProfiles" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@ProfileAuthOptions", ProfileAuthOptions,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch,DbType.String);
        	    
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_FindUsersByNameOrEmail Procedure
        /// </summary>
        public static StoredProcedure MembershipFindUsersByNameOrEmail(string ApplicationName, string UserNameToMatch, string EmailToMatch, int? PageIndex, int? PageSize, int? TotalCount)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_FindUsersByNameOrEmail" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@UserNameToMatch", UserNameToMatch,DbType.String);
        	    
            sp.Command.AddParameter("@EmailToMatch", EmailToMatch,DbType.String);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@TotalCount",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_GetAllUsers Procedure
        /// </summary>
        public static StoredProcedure MembershipGetAllUsers(string ApplicationName, int? PageIndex, int? PageSize, int? TotalCount)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_GetAllUsers" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@TotalCount",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Users_DeleteUser Procedure
        /// </summary>
        public static StoredProcedure UsersDeleteUser(string UserName, int? TablesToDeleteFrom, int? NumTablesDeletedFrom)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Users_DeleteUser" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@TablesToDeleteFrom", TablesToDeleteFrom,DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@NumTablesDeletedFrom",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Applications_CreateApplication Procedure
        /// </summary>
        public static StoredProcedure ApplicationsCreateApplication(string ApplicationName, Guid? ApplicationId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Applications_CreateApplication" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddOutputParameter("@ApplicationId",DbType.Guid);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_AnyDataInTables Procedure
        /// </summary>
        public static StoredProcedure AnyDataInTables(int? TablesToCheck)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_AnyDataInTables" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@TablesToCheck", TablesToCheck,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationAllUsers_SetPageSettings Procedure
        /// </summary>
        public static StoredProcedure PersonalizationAllUsersSetPageSettings(string ApplicationName, string Path, byte[] PageSettings, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationAllUsers_SetPageSettings" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            sp.Command.AddParameter("@PageSettings", PageSettings,DbType.Binary);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Roles_CreateRole Procedure
        /// </summary>
        public static StoredProcedure RolesCreateRole(string ApplicationName, string RoleName)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Roles_CreateRole" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@RoleName", RoleName,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Profile_SetProperties Procedure
        /// </summary>
        public static StoredProcedure ProfileSetProperties(string ApplicationName, string PropertyNames, string PropertyValuesString, byte[] PropertyValuesBinary, string UserName, bool? IsUserAnonymous, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Profile_SetProperties" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@PropertyNames", PropertyNames,DbType.String);
        	    
            sp.Command.AddParameter("@PropertyValuesString", PropertyValuesString,DbType.String);
        	    
            sp.Command.AddParameter("@PropertyValuesBinary", PropertyValuesBinary,DbType.Binary);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@IsUserAnonymous", IsUserAnonymous,DbType.Boolean);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Membership_CreateUser Procedure
        /// </summary>
        public static StoredProcedure MembershipCreateUser(string UserName, string Password, string PasswordSalt, string Email, string PasswordQuestion, string PasswordAnswer, bool? IsApproved, DateTime? CurrentTimeUtc, DateTime? CreateDate, bool? UniqueEmail, int? PasswordFormat, Guid? UserId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Membership_CreateUser" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@Password", Password,DbType.String);
        	    
            sp.Command.AddParameter("@PasswordSalt", PasswordSalt,DbType.String);
        	    
            sp.Command.AddParameter("@Email", Email,DbType.String);
        	    
            sp.Command.AddParameter("@PasswordQuestion", PasswordQuestion,DbType.String);
        	    
            sp.Command.AddParameter("@PasswordAnswer", PasswordAnswer,DbType.String);
        	    
            sp.Command.AddParameter("@IsApproved", IsApproved,DbType.Boolean);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            sp.Command.AddParameter("@CreateDate", CreateDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@UniqueEmail", UniqueEmail,DbType.Boolean);
        	    
            sp.Command.AddParameter("@PasswordFormat", PasswordFormat,DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@UserId",DbType.Guid);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationPerUser_SetPageSettings Procedure
        /// </summary>
        public static StoredProcedure PersonalizationPerUserSetPageSettings(string ApplicationName, string UserName, string Path, byte[] PageSettings, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationPerUser_SetPageSettings" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            sp.Command.AddParameter("@PageSettings", PageSettings,DbType.Binary);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_Profile_DeleteProfiles Procedure
        /// </summary>
        public static StoredProcedure ProfileDeleteProfiles(string UserNames)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_Profile_DeleteProfiles" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@UserNames", UserNames,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetBlogById Procedure
        /// </summary>
        public static StoredProcedure GetBlogById(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetBlogById" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationAllUsers_GetPageSettings Procedure
        /// </summary>
        public static StoredProcedure PersonalizationAllUsersGetPageSettings(string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationAllUsers_GetPageSettings" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateFeedbackStats Procedure
        /// </summary>
        public static StoredProcedure UpdateFeedbackStats(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateFeedbackStats" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationAllUsers_ResetPageSettings Procedure
        /// </summary>
        public static StoredProcedure PersonalizationAllUsersResetPageSettings(string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationAllUsers_ResetPageSettings" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateBlogStats Procedure
        /// </summary>
        public static StoredProcedure UpdateBlogStats(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateBlogStats" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationPerUser_GetPageSettings Procedure
        /// </summary>
        public static StoredProcedure PersonalizationPerUserGetPageSettings(string ApplicationName, string UserName, string Path, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationPerUser_GetPageSettings" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateFeedbackCount Procedure
        /// </summary>
        public static StoredProcedure UpdateFeedbackCount(int? BlogId, int? EntryId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateFeedbackCount" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryId", EntryId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationPerUser_ResetPageSettings Procedure
        /// </summary>
        public static StoredProcedure PersonalizationPerUserResetPageSettings(string ApplicationName, string UserName, string Path, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationPerUser_ResetPageSettings" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteCategory Procedure
        /// </summary>
        public static StoredProcedure DeleteCategory(int? CategoryID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteCategory" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationAdministration_DeleteAllState Procedure
        /// </summary>
        public static StoredProcedure PersonalizationAdministrationDeleteAllState(bool? AllUsersScope, string ApplicationName, int? Count)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationAdministration_DeleteAllState" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope,DbType.Boolean);
        	    
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddOutputParameter("@Count",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteImage Procedure
        /// </summary>
        public static StoredProcedure DeleteImage(int? BlogId, int? ImageID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteImage" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@ImageID", ImageID,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationAdministration_ResetSharedState Procedure
        /// </summary>
        public static StoredProcedure PersonalizationAdministrationResetSharedState(int? Count, string ApplicationName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationAdministration_ResetSharedState" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddOutputParameter("@Count",DbType.Int32);
        	    
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteImageCategory Procedure
        /// </summary>
        public static StoredProcedure DeleteImageCategory(int? CategoryID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteImageCategory" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationAdministration_ResetUserState Procedure
        /// </summary>
        public static StoredProcedure PersonalizationAdministrationResetUserState(int? Count, string ApplicationName, DateTime? InactiveSinceDate, string UserName, string Path)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationAdministration_ResetUserState" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddOutputParameter("@Count",DbType.Int32);
        	    
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteKeyWord Procedure
        /// </summary>
        public static StoredProcedure DeleteKeyWord(int? KeyWordID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteKeyWord" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@KeyWordID", KeyWordID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationAdministration_GetCountOfState Procedure
        /// </summary>
        public static StoredProcedure PersonalizationAdministrationGetCountOfState(int? Count, bool? AllUsersScope, string ApplicationName, string Path, string UserName, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationAdministration_GetCountOfState" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddOutputParameter("@Count",DbType.Int32);
        	    
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope,DbType.Boolean);
        	    
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteLink Procedure
        /// </summary>
        public static StoredProcedure DeleteLink(int? LinkID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteLink" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@LinkID", LinkID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_PersonalizationAdministration_FindState Procedure
        /// </summary>
        public static StoredProcedure PersonalizationAdministrationFindState(bool? AllUsersScope, string ApplicationName, int? PageIndex, int? PageSize, string Path, string UserName, DateTime? InactiveSinceDate)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_PersonalizationAdministration_FindState" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@AllUsersScope", AllUsersScope,DbType.Boolean);
        	    
            sp.Command.AddParameter("@ApplicationName", ApplicationName,DbType.String);
        	    
            sp.Command.AddParameter("@PageIndex", PageIndex,DbType.Int32);
        	    
            sp.Command.AddParameter("@PageSize", PageSize,DbType.Int32);
        	    
            sp.Command.AddParameter("@Path", Path,DbType.String);
        	    
            sp.Command.AddParameter("@UserName", UserName,DbType.String);
        	    
            sp.Command.AddParameter("@InactiveSinceDate", InactiveSinceDate,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteLinksByPostID Procedure
        /// </summary>
        public static StoredProcedure DeleteLinksByPostID(int? PostID, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteLinksByPostID" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@PostID", PostID,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetFeedbackCountsByStatus Procedure
        /// </summary>
        public static StoredProcedure GetFeedbackCountsByStatus(int? BlogId, int? ApprovedCount, int? NeedsModerationCount, int? FlaggedSpam, int? Deleted)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetFeedbackCountsByStatus" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@ApprovedCount",DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@NeedsModerationCount",DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@FlaggedSpam",DbType.Int32);
        	    
            sp.Command.AddOutputParameter("@Deleted",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteFeedback Procedure
        /// </summary>
        public static StoredProcedure DeleteFeedback(int? Id)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteFeedback" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Id", Id,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetHost Procedure
        /// </summary>
        public static StoredProcedure GetHost()
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetHost" , DataService.GetInstance("Subtext"));
        	
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeleteFeedbackByStatus Procedure
        /// </summary>
        public static StoredProcedure DeleteFeedbackByStatus(int? BlogId, int? StatusFlag)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeleteFeedbackByStatus" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@StatusFlag", StatusFlag,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_DeletePost Procedure
        /// </summary>
        public static StoredProcedure DeletePost(int? ID)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_DeletePost" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ID", ID,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetActiveCategoriesWithLinkCollection Procedure
        /// </summary>
        public static StoredProcedure GetActiveCategoriesWithLinkCollection(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetActiveCategoriesWithLinkCollection" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetCategory Procedure
        /// </summary>
        public static StoredProcedure GetCategory(string CategoryName, int? CategoryID, bool? IsActive, int? BlogId, short? CategoryType)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetCategory" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@CategoryName", CategoryName,DbType.String);
        	    
            sp.Command.AddParameter("@CategoryID", CategoryID,DbType.Int32);
        	    
            sp.Command.AddParameter("@IsActive", IsActive,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@CategoryType", CategoryType,DbType.Int16);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UTILITY_AddBlog Procedure
        /// </summary>
        public static StoredProcedure UTILITYAddBlog(string Title, string Host, string Subfolder, Guid? OwnerID, DateTime? CurrentTimeUtc)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UTILITY_AddBlog" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Title", Title,DbType.String);
        	    
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@Subfolder", Subfolder,DbType.String);
        	    
            sp.Command.AddParameter("@OwnerID", OwnerID,DbType.Guid);
        	    
            sp.Command.AddParameter("@CurrentTimeUtc", CurrentTimeUtc,DbType.DateTime);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetConditionalEntries Procedure
        /// </summary>
        public static StoredProcedure GetConditionalEntries(int? ItemCount, int? PostType, int? PostConfig, int? BlogId, bool? IncludeCategories)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetConditionalEntries" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@ItemCount", ItemCount,DbType.Int32);
        	    
            sp.Command.AddParameter("@PostType", PostType,DbType.Int32);
        	    
            sp.Command.AddParameter("@PostConfig", PostConfig,DbType.Int32);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@IncludeCategories", IncludeCategories,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_CreateHost Procedure
        /// </summary>
        public static StoredProcedure CreateHost(Guid? OwnerId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_CreateHost" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@OwnerId", OwnerId,DbType.Guid);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetBlog Procedure
        /// </summary>
        public static StoredProcedure GetBlog(string Host, string Subfolder, bool? Strict)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetBlog" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Host", Host,DbType.String);
        	    
            sp.Command.AddParameter("@Subfolder", Subfolder,DbType.String);
        	    
            sp.Command.AddParameter("@Strict", Strict,DbType.Boolean);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_InsertMetaTag Procedure
        /// </summary>
        public static StoredProcedure InsertMetaTag(string Content, string Name, string HttpEquiv, int? BlogId, int? EntryId, DateTime? DateCreated, int? Id)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_InsertMetaTag" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Content", Content,DbType.String);
        	    
            sp.Command.AddParameter("@Name", Name,DbType.String);
        	    
            sp.Command.AddParameter("@HttpEquiv", HttpEquiv,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryId", EntryId,DbType.Int32);
        	    
            sp.Command.AddParameter("@DateCreated", DateCreated,DbType.DateTime);
        	    
            sp.Command.AddOutputParameter("@Id",DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetEntriesByDayRange Procedure
        /// </summary>
        public static StoredProcedure GetEntriesByDayRange(DateTime? StartDate, DateTime? StopDate, int? PostType, bool? IsActive, int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetEntriesByDayRange" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@StartDate", StartDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@StopDate", StopDate,DbType.DateTime);
        	    
            sp.Command.AddParameter("@PostType", PostType,DbType.Int32);
        	    
            sp.Command.AddParameter("@IsActive", IsActive,DbType.Boolean);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_UpdateMetaTag Procedure
        /// </summary>
        public static StoredProcedure UpdateMetaTag(int? Id, string Content, string Name, string HttpEquiv, int? BlogId, int? EntryId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_UpdateMetaTag" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@Id", Id,DbType.Int32);
        	    
            sp.Command.AddParameter("@Content", Content,DbType.String);
        	    
            sp.Command.AddParameter("@Name", Name,DbType.String);
        	    
            sp.Command.AddParameter("@HttpEquiv", HttpEquiv,DbType.String);
        	    
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            sp.Command.AddParameter("@EntryId", EntryId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetFeedbackCollection Procedure
        /// </summary>
        public static StoredProcedure GetFeedbackCollection(int? EntryId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetFeedbackCollection" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@EntryId", EntryId,DbType.Int32);
        	    
            return sp;
        }

        
        /// <summary>
        /// Creates an object wrapper for the subtext_GetMetaTagsForBlog Procedure
        /// </summary>
        public static StoredProcedure GetMetaTagsForBlog(int? BlogId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("subtext_GetMetaTagsForBlog" , DataService.GetInstance("Subtext"));
        	
            sp.Command.AddParameter("@BlogId", BlogId,DbType.Int32);
        	    
            return sp;
        }

        
    }

    
}

