/*
WARNING: This SCRIPT USES SQL TEMPLATE PARAMETERS.
Be sure to hit CTRL+SHIFT+M in Query Analyzer if running manually.
*/

/* DROPPED STORED PROCS.  
	These are stored procs that used to be in the system but are no longer needed.
	The statements will only drop the procs if they exist as a form of cleanup.
*/
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableReferrersByEntryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableReferrersByEntryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogsByHost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogsByHost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetConditionalEntriesByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetConditionalEntriesByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntryCollectionByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntryCollectionByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryNameByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryNameByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryIDByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryIDByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntryWithCategoryTitlesByEntryName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntryWithCategoryTitlesByEntryName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetRecentEntriesByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetRecentEntriesByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetRecentEntriesWithCategoryTitles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetRecentEntriesWithCategoryTitles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetRecentEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetRecentEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetSingleEntryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetSingleEntryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntryWithCategoryTitles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntryWithCategoryTitles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertPostCategoryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertPostCategoryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]
GO

/* The Rest of the script */

/* Note: DNW_* are the aggregate blog procs */
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[DNW_GetRecentPosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[DNW_GetRecentPosts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[DNW_HomePageData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[DNW_HomePageData]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[DNW_Stats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[DNW_Stats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[DNW_Total_Stats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[DNW_Total_Stats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[iter_charlist_to_table]') and xtype in (N'FN', N'IF', N'TF'))
drop function [<dbUser,varchar,dbo>].[iter_charlist_to_table]
GO
	
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_VersionAdd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_VersionAdd]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_VersionGetCurrent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_VersionGetCurrent]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetHost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetHost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateHost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateHost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetCommentByChecksumHash]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetCommentByChecksumHash]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogById]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteImageCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteImageCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteLinksByPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteLinksByPostID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeletePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeletePost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetAllCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetAllCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetCategoryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetCategoryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetConditionalEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetConditionalEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetConfig]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntriesByDayRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntriesByDayRange]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetFeedBack]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetFeedBack]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetImageCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetImageCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetLinkCollectionByPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetLinkCollectionByPostID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableFeedback]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableFeedback]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableLogEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableLogEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableKeyWords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableKeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableLinks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableReferrers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableReferrers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByDayRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByDayRange]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByMonth]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByMonth]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByMonthArchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByMonthArchive]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByYearArchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByYearArchive]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetSingleDay]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetSingleDay]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetSingleEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetSingleEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetSingleImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetSingleImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetSingleLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetSingleLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetUrlID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetUrlID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertLinkCategoryList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertLinkCategoryList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertPingTrackEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertPingTrackEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertReferral]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertReferral]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertViewStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertViewStats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_StatsSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_StatsSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_TrackEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_TrackEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UTILITY_AddBlog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UTILITY_AddBlog]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateConfig]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Utility_GetUnHashedPasswords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_Utility_GetUnHashedPasswords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Utility_UpdateToHashedPassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_Utility_UpdateToHashedPassword]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_AddLogEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_AddLogEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_LogClear]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_LogClear]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_SearchEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_SearchEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetRelatedLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetRelatedLinks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByCategoriesArchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].subtext_GetPostsByCategoriesArchive
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

--Found at: http://www.algonet.se/~sommar/arrays-in-sql.html
  CREATE FUNCTION [<dbUser,varchar,dbo>].[iter_charlist_to_table]
                    (@list      ntext,
                     @delimiter nchar(1) = N',')
         RETURNS @tbl TABLE (listpos int IDENTITY(1, 1) NOT NULL,
                             str     varchar(4000),
                             nstr    nvarchar(2000)) AS

   BEGIN
      DECLARE @pos      int,
              @textpos  int,
              @chunklen smallint,
              @tmpstr   nvarchar(4000),
              @leftover nvarchar(4000),
              @tmpval   nvarchar(4000)

      SET @textpos = 1 
           SET @leftover = ''
                 WHILE @textpos <= datalength(@list) / 2
                       BEGIN
         SET @chunklen = 4000 - datalength(@leftover) / 2
         SET @tmpstr = @leftover + substring(@list, @textpos, @chunklen)
         SET @textpos = @textpos + @chunklen

         SET @pos = charindex(@delimiter, @tmpstr)

         WHILE @pos > 0
         BEGIN
            SET @tmpval = ltrim(rtrim(left(@tmpstr, @pos - 1)))
            INSERT @tbl (str, nstr) VALUES(@tmpval, @tmpval)
            SET @tmpstr = substring(@tmpstr, @pos + 1, len(@tmpstr))
            SET @pos = charindex(@delimiter, @tmpstr)
         END

         SET @leftover = @tmpstr
      END

      INSERT @tbl(str, nstr) VALUES (ltrim(rtrim(@leftover)), ltrim(rtrim(@leftover)))
   RETURN
   END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteCategory]
(
	@CategoryID int,
	@BlogId int
)
AS
DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE CategoryID = @CategoryID AND BlogId = @BlogId
DELETE [<dbUser,varchar,dbo>].[subtext_LinkCategories] FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories] WHERE subtext_LinkCategories.CategoryID = @CategoryID AND subtext_LinkCategories.BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteImage]
(
	@BlogId int,
	@ImageID int
)
AS
DELETE [<dbUser,varchar,dbo>].[subtext_Images] 
FROM [<dbUser,varchar,dbo>].[subtext_Images] 
WHERE	ImageID = @ImageID 
	AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteImageCategory]
(
	@CategoryID int,
	@BlogId int
)
AS
DELETE [<dbUser,varchar,dbo>].[subtext_Images] FROM [<dbUser,varchar,dbo>].[subtext_Images] WHERE subtext_Images.CategoryID = @CategoryID AND subtext_Images.BlogId = @BlogId
DELETE [<dbUser,varchar,dbo>].[subtext_LinkCategories] FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories] WHERE subtext_LinkCategories.CategoryID = @CategoryID AND subtext_LinkCategories.BlogId = @BlogId



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteImageCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteKeyWord]
(
	@KeyWordID int,
	@BlogId int
)

AS

DELETE FROM [<dbUser,varchar,dbo>].[subtext_KeyWords] WHERE BlogId = @BlogId AND KeyWordID = @KeyWordID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteLink]
(
	@LinkID int,
	@BlogId int
)
AS
DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE [LinkID] = @LinkID AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteLinksByPostID]
(
	@PostID int,
	@BlogId int
)
AS
Set NoCount ON
DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE PostID = @PostID AND BlogId = @BlogId



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteLinksByPostID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Deletes a record FROM [<dbUser,varchar,dbo>].[subtext_content], whether it be a post, a comment, etc..
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeletePost]
(
	@ID int,
	@BlogId int
)
AS

DECLARE @ParentID int, @PostType int

SELECT @ParentID = ParentID, @PostType = PostType FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE [ID] = @ID

-- PostType 3 = Comment
-- PostType 4 = PingBack

IF(@PostType = 3 or @PostType = 4)
BEGIN
	UPDATE [<dbUser,varchar,dbo>].[subtext_Content]
	SET FeedBackCount = FeedBackCount - 1
	WHERE [ID] = @ParentID
END
ELSE
BEGIN
	DELETE FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE ParentID = @ID
	-- This is a refactoring in progress 
	-- to remove meaning from PostId = -1
	DECLARE @PostID int
	SET @PostID = @ID
	IF @ID = -1
		SET @PostID = NULL
	DELETE FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE PostID = @PostID
	DELETE FROM [<dbUser,varchar,dbo>].[subtext_EntryViewCount] WHERE EntryID = @ID
	DELETE FROM [<dbUser,varchar,dbo>].[subtext_Referrals] WHERE EntryID = @ID
END

DELETE FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE [ID] = @ID AND [BlogId] = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeletePost]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]
(
	@BlogId int = NULL
)
AS
SELECT subtext_LinkCategories.CategoryID
	, subtext_LinkCategories.Title
	, subtext_LinkCategories.Active
	, subtext_LinkCategories.CategoryType
	, subtext_LinkCategories.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories]
WHERE	
			subtext_LinkCategories.Active= 1 
	AND		(subtext_LinkCategories.BlogId = @BlogId OR @BlogId IS NULL)
	AND		subtext_LinkCategories.CategoryType = 0
ORDER BY 
	subtext_LinkCategories.Title;

SELECT links.LinkID
	, links.Title
	, links.Url
	, links.Rss
	, links.Active
	, links.NewWindow
	, links.CategoryID
	, PostID = ISNULL(links.PostID, -1)
FROM [<dbUser,varchar,dbo>].[subtext_Links] links
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] categories ON links.CategoryID = categories.CategoryID
WHERE 
		links.Active = 1 
	AND categories.Active = 1
	AND (categories.BlogId = @BlogId OR @BlogId IS NULL)
	AND links.BlogId = @BlogId 
	AND categories.CategoryType = 0
ORDER BY 
	links.Title;



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetAllCategories]
(
	@BlogId int = NULL
	, @IsActive bit
	, @CategoryType tinyint
)
As
SELECT subtext_LinkCategories.CategoryID
	, subtext_LinkCategories.Title
	, subtext_LinkCategories.Active
	, subtext_LinkCategories.CategoryType
	, subtext_LinkCategories.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories]
WHERE (subtext_LinkCategories.BlogId = @BlogId OR @BlogId IS NULL)
	AND subtext_LinkCategories.CategoryType = @CategoryType 
	AND subtext_LinkCategories.Active <> CASE @IsActive WHEN 1 THEN 0 ELSE -1 END
ORDER BY subtext_LinkCategories.Title;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetAllCategories]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]
(
	@BlogId int
)
AS

SELECT 
	KeyWordID, Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogId
FROM
	subtext_keywords
WHERE 
	BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetCategory]
(
	@CategoryID int,
	@IsActive bit,
	@BlogId int
)
AS
SELECT	subtext_LinkCategories.CategoryID
		, subtext_LinkCategories.Title
		, subtext_LinkCategories.Active
		, subtext_LinkCategories.CategoryType
		, subtext_LinkCategories.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories]
WHERE subtext_LinkCategories.CategoryID=@CategoryID 
	AND subtext_LinkCategories.BlogId = @BlogId 
	AND subtext_LinkCategories.Active <> CASE @IsActive WHEN 0 THEN -1 else 0 END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetCategoryByName] 
(
	@CategoryName nvarchar(150),
	@IsActive bit,
	@BlogId int
)
AS
SELECT	subtext_LinkCategories.CategoryID
		, subtext_LinkCategories.Title
		, subtext_LinkCategories.Active
		, subtext_LinkCategories.CategoryType
		, subtext_LinkCategories.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories]
WHERE	subtext_LinkCategories.Title=@CategoryName 
	AND subtext_LinkCategories.BlogId = @BlogId 
	AND subtext_LinkCategories.Active <> CASE @IsActive WHEN 0 THEN -1 else 0 END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetCategoryByName]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetConditionalEntries]
(
	@ItemCount int 
	, @PostType int
	, @PostConfig int
	, @BlogId int = NULL
	, @IncludeCategories bit = 0
)
AS
/* 
//TODO: This proc is being used to populate home page 
and feed. But it should sort on different dates for each.
*/
CREATE Table #IDs  
(  
	 TempId int IDENTITY (0, 1) NOT NULL,  
	 Id int not NULL  
)

INSERT #IDs (Id)  
SELECT [Id]   
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	PostType = @PostType 
	AND BlogId = COALESCE(@BlogId, BlogId)
	AND PostConfig & @PostConfig = @PostConfig
ORDER BY ISNULL([DateSyndicated], [DateAdded]) DESC

SET ROWCOUNT @ItemCount
SELECT BlogId
	, [<dbUser,varchar,dbo>].[subtext_Content].[Id]
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, SourceUrl
	, PostType
	, Author
	, Email
	, SourceName
	, DateUpdated
	, TitleUrl
	, FeedBackCount = ISNULL(FeedBackCount, 0)
	, ParentID
	, PostConfig
	, EntryName 
	, ContentChecksumHash
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
	INNER JOIN #IDs ON #IDs.[Id] = [<dbUser,varchar,dbo>].[subtext_Content].[Id]
ORDER BY #IDs.TempId

IF @IncludeCategories = 1
BEGIN
	SELECT	c.Title  
			, p.[Id]
	FROM [<dbUser,varchar,dbo>].[subtext_Links] l
		INNER JOIN #IDs p ON l.[PostID] = p.[ID]  
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID
	ORDER BY p.[TempID] DESC
END
DROP TABLE #IDs

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetConditionalEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
/*
Returns the blog that matches the given host/application combination.

@Strict -- If 0, then we return the one and only blog if there's one and only blog.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetConfig]
(
	@Host nvarchar(100)
	, @Application nvarchar(50)
	, @Strict bit = 1 
)
AS

IF (@Strict = 0) AND (1 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_config]))
BEGIN
	-- Return the one and only record
	SELECT
		subtext_Config.BlogId
		, UserName
		, [Password]
		, Email
		, Title
		, SubTitle
		, Skin
		, Application
		, Host
		, Author
		, TimeZone
		, ItemCount
		, [Language]
		, News
		, SecondaryCss
		, LastUpdated
		, PostCount
		, StoryCount
		, PingTrackCount
		, CommentCount
		, Flag
		, SkinCssFile 
		, LicenseUrl
		, DaysTillCommentsClose
		, CommentDelayInMinutes
		, NumberOfRecentComments
		, RecentCommentsLength
	FROM [<dbUser,varchar,dbo>].[subtext_Config]
END
ELSE
BEGIN
	SELECT
		BlogId
		, UserName
		, [Password]
		, Email
		, Title
		, SubTitle
		, Skin
		, Application
		, Host
		, Author
		, TimeZone
		, ItemCount
		, [Language]
		, News
		, SecondaryCss
		, LastUpdated
		, PostCount
		, StoryCount
		, PingTrackCount
		, CommentCount
		, Flag
		, SkinCssFile 
		, LicenseUrl
		, DaysTillCommentsClose
		, CommentDelayInMinutes
		, NumberOfRecentComments
		, RecentCommentsLength
	FROM [<dbUser,varchar,dbo>].[subtext_Config]
	WHERE	Host = @Host
		AND Application = @Application
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetConfig]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetEntriesByDayRange]
(
	@StartDate datetime,
	@StopDate datetime,
	@PostType int,
	@IsActive bit,
	@BlogId int
)
AS
SELECT	BlogId
	, [ID]
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, SourceUrl
	, PostType
	, Author
	, Email
	, SourceName
	, DateUpdated
	, TitleUrl
	, FeedBackCount = ISNULL(FeedBackCount, 0)
	, ParentID
	, PostConfig
	, EntryName 
	, ContentChecksumHash
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE 
	(
		DateAdded > @StartDate 
		AND DateAdded < DateAdd(day, 1, @StopDate)
	)
	AND PostType=@PostType 
	AND BlogId = @BlogId 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetEntriesByDayRange]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetFeedBack]
(
	@ParentID int
	, @BlogId int
)
AS

SELECT	BlogId
		, [ID]
		, Title
		, DateAdded
		, [Text]
		, [Description]
		, SourceUrl
		, PostType
		, Author
		, Email
		, SourceName
		, DateUpdated
		, TitleUrl
		, ParentID
		, FeedBackCount = ISNULL(FeedBackCount, 0)
		, PostConfig
		, EntryName
		, ParentID 
		, ContentChecksumHash
		, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE BlogId = @BlogId 
	AND PostConfig & 1 = 1
	AND ParentID = @ParentID
ORDER BY [ID]


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetFeedBack]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetImageCategory]
(
	@CategoryID int
	, @IsActive bit
	, @BlogId int
)
AS
EXEC [<dbUser,varchar,dbo>].[subtext_GetCategory] @CategoryID, @IsActive, @BlogId

SELECT	Title
		, CategoryID
		, Height
		, Width
		, [File]
		, Active
		, ImageID 
FROM [<dbUser,varchar,dbo>].[subtext_Images]  
WHERE CategoryID = @CategoryID 
	AND BlogId = @BlogId 
	AND Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetImageCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetKeyWord]
(
	@KeyWordID int
	, @BlogId int
)
AS

SELECT 
	KeyWordID, Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogId
FROM
	subtext_keywords
WHERE 
	KeyWordID = @KeyWordID AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetLinkCollectionByPostID]
(
	@PostID int,
	@BlogId int
)
AS

IF @PostID = -1
	SET @PostID = NULL

SELECT	LinkID
	, Title
	, Url
	, Rss
	, Active
	, CategoryID
	, PostID = ISNULL(PostID, -1)
	, NewWindow 
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE PostID = @PostID 
	AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetLinkCollectionByPostID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]
(
	@CategoryID int
	, @BlogId int
)
AS
EXEC [<dbUser,varchar,dbo>].[subtext_GetCategory] @CategoryID, 0, @BlogId
SELECT	LinkID
		, Title
		, Url
		, Rss
		, Active
		, CategoryID
		, PostID = ISNULL(PostID, -1)
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE Active = 1 
	AND CategoryID = @CategoryID 
	AND BlogId = @BlogId
ORDER BY Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]
(
	@CategoryID int
	, @BlogId int
)
AS
EXEC [<dbUser,varchar,dbo>].[subtext_GetCategory] @CategoryID, @BlogId
SELECT	LinkID
		, Title
		, Url
		, Rss
		, Active
		, NewWindow
		, CategoryID
		, PostId = ISNULL(PostID, -1)
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE	CategoryID = @CategoryID 
	AND BlogId = @BlogId
ORDER BY Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Selects a page of blog posts within the admin section.
Updated this to use a more efficient paging technique:
http://www.4guysfromrolla.com/webtech/041206-1.shtml
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableEntries]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
	, @PostType int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = [ID] FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	BlogId = @BlogId 
	AND PostType = @PostType 
ORDER BY [ID] DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	content.BlogId 
		, content.[ID] 
		, content.Title 
		, content.DateAdded 
		, content.[Text] 
		, content.[Description]
		, content.SourceUrl 
		, content.PostType 
		, content.Author 
		, content.Email 
		, content.SourceName 
		, content.DateUpdated 
		, content.TitleUrl 
		, FeedBackCount = ISNULL(content.FeedBackCount, 0)
		, content.ParentID
		, content.PostConfig
		, content.EntryName
		, content.ContentChecksumHash
		, content.DateSyndicated
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	Left JOIN  subtext_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogId = @BlogId)
WHERE 	content.BlogId = @BlogId 
	AND content.[ID] <= @FirstId
	AND PostType = @PostType
ORDER BY content.[ID] DESC
 
SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE 	BlogId = @BlogId 
	AND PostType = @PostType 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Selects a page of blog posts within the admin section, when a category 
is selected.
Updated this to use a more efficient paging technique:
http://www.4guysfromrolla.com/webtech/041206-1.shtml
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]
(
	@BlogId int
	, @CategoryID int
	, @PageIndex int
	, @PageSize int
	, @PostType int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = content.[ID] 
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] links ON content.[ID] = ISNULL(links.PostID, -1)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] cats ON (links.CategoryID = cats.CategoryID)
WHERE	content.BlogId = @BlogId 
	AND content.PostType = @PostType 
	AND cats.CategoryID = @CategoryID
ORDER BY content.[ID] DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	content.BlogId 
		, content.[ID] 
		, content.Title 
		, content.DateAdded 
		, content.[Text] 
		, content.[Description]
		, content.SourceUrl 
		, content.PostType 
		, content.Author 
		, content.Email 
		, content.SourceName 
		, content.DateUpdated 
		, content.TitleUrl 
		, FeedBackCount = ISNULL(content.FeedBackCount, 0)
		, content.ParentID
		, content.PostConfig
		, content.EntryName
		, content.ContentChecksumHash
		, content.DateSyndicated
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] l ON content.[ID] = ISNULL(l.PostID, -1)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] cats ON (l.CategoryID = cats.CategoryID)
	Left JOIN  subtext_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogId = @BlogId)
WHERE 	content.BlogId = @BlogId 
	AND content.[ID] <= @FirstId
	AND content.PostType = @PostType
	AND cats.CategoryID = @CategoryID
ORDER BY content.[ID] DESC
 
SELECT COUNT(content.[ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] links ON content.[ID] = ISNULL(links.PostID, -1)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] cats ON (links.CategoryID = cats.CategoryID)
WHERE 	content.BlogId = @BlogId 
	AND content.PostType = @PostType 
	AND cats.CategoryID = @CategoryID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableFeedback]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
	, @PostConfig int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT @FirstId = [ID] 
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE 	BlogId = @BlogId 
	AND (PostType = 3 OR PostType = 4)
	AND PostConfig & @PostConfig = @PostConfig
ORDER BY [ID] DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	content.BlogId 
		, content.[ID] 
		, content.Title 
		, content.DateAdded 
		, content.[Text] 
		, content.[Description]
		, content.SourceUrl 
		, content.PostType 
		, content.Author 
		, content.Email 
		, content.SourceName 
		, content.DateUpdated 
		, content.TitleUrl 
		, FeedBackCount = ISNULL(content.FeedBackCount, 0)
		, content.ParentID
		, content.PostConfig
		, content.EntryName
		, content.ContentChecksumHash
		, content.DateSyndicated
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	Left JOIN  subtext_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogId = @BlogId)
WHERE 	content.BlogId = @BlogId 
	AND content.[ID] <= @FirstId
	AND (PostType = 3 OR PostType = 4)
	AND PostConfig & @PostConfig = @PostConfig
ORDER BY content.[ID] DESC
 
SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE 	BlogId = @BlogId 
	AND (PostType = 3 OR PostType = 4)
	AND PostConfig & @PostConfig = @PostConfig

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableFeedback]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Selects a page of log posts within the admin section.
Updated this to use a more efficient paging technique:
http://www.4guysfromrolla.com/webtech/041206-1.shtml
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableLogEntries]
(
	@BlogId int = NULL
	, @PageIndex int
	, @PageSize int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = [ID] FROM [<dbUser,varchar,dbo>].[subtext_Log] 
WHERE	BlogId = @BlogId OR @BlogId IS NULL
ORDER BY [ID] DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	[log].[Id]
		, [log].[BlogId]
		, [log].[Date]
		, [log].[Thread]
		, [log].[Context]
		, [log].[Level]
		, [log].[Logger]
		, [log].[Message]
		, [log].[Exception]
		, [log].[Url]
FROM [<dbUser,varchar,dbo>].[subtext_Log] [log]
WHERE 	([log].BlogId = @BlogId or @BlogId IS NULL)
	AND [log].[ID] <= @FirstId
ORDER BY [log].[ID] DESC
 
SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Log] 
WHERE 	BlogId = @BlogId 
	OR 	@BlogId IS NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableLogEntries]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Selects a page of keywords within the admin section.
Updated this to use a more efficient paging technique:
http://www.4guysfromrolla.com/webtech/041206-1.shtml
*/

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableKeyWords]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
)
AS
DECLARE @FirstWord nvarchar(100)
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstWord = [Word] FROM [<dbUser,varchar,dbo>].[subtext_KeyWords]
WHERE	BlogId = @BlogId 
ORDER BY [Word] ASC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT 	words.KeyWordID
		, words.Word
		, words.[Text]
		, words.ReplaceFirstTimeOnly
		, words.OpenInNewWindow
		, words.CaseSensitive
		, words.Url
		, words.Title
		, words.BlogId
FROM 	
	[<dbUser,varchar,dbo>].[subtext_KeyWords] words
WHERE 	
		words.BlogId = @BlogId 
	AND words.Word >= @FirstWord
ORDER BY
		words.Word ASC
 
SELECT 	COUNT([KeywordId]) AS 'TotalRecords'
FROM [<dbUser,varchar,dbo>].[subtext_KeyWords] 
WHERE 	BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableKeyWords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Returns a page of links for the admin section */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableLinks]
(
	@BlogId int
	, @CategoryId int = NULL
	, @PageIndex int
	, @PageSize int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT @FirstId = LinkID
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE 	BlogId = @BlogId 
	AND (CategoryID = @CategoryID OR @CategoryID IS NULL)
	AND PostID IS NULL
ORDER BY [LinkID] DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT links.LinkID 
	, links.Title 
	, links.Url
	, links.Rss 
	, links.Active 
	, links.NewWindow 
	, links.CategoryID
	, PostID = ISNULL(links.PostID, -1)
FROM [<dbUser,varchar,dbo>].[subtext_Links] links
WHERE 	links.BlogId = @BlogId 
	AND links.[LinkId] <= @FirstId
	AND (CategoryID = @CategoryID OR @CategoryID IS NULL)
	AND PostID IS NULL
ORDER BY links.[LinkID] DESC
 
SELECT COUNT([LinkID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Links] 
WHERE 	BlogId = @BlogId 
	AND (CategoryID = @CategoryID OR @CategoryID IS NULL)
	AND PostID IS NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableLinks]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]
(
	@BlogId int
	, @CategoryID int = NULL
	, @PageIndex int
	, @PageSize int
	, @SortDesc bit
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #TempPagedLinkIDs 
(
	TempID int IDENTITY (1, 1) NOT NULL,
	LinkID int NOT NULL
)	

IF NOT (@SortDesc = 1)
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM [<dbUser,varchar,dbo>].[subtext_Links] 
	WHERE 	BlogId = @BlogId 
		AND CategoryID = @CategoryID
		AND PostID IS NULL
	ORDER BY Title
END
ELSE
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM [<dbUser,varchar,dbo>].[subtext_Links]
	WHERE 	BlogId = @BlogId 
		AND CategoryID = @CategoryID
		AND PostID IS NULL
	ORDER BY Title DESC
END

SELECT 	links.LinkID
		, links.Title
		, links.Url
		, links.Rss 
		, links.Active 
		, links.NewWindow 
		, links.CategoryID  
		, PostId = ISNULL(links.PostID, -1)
FROM 	
	subtext_Links links
	INNER JOIN #TempPagedLinkIDs tmp ON (links.LinkID = tmp.LinkID)
WHERE 	
		links.BlogId = @BlogId 
	AND links.CategoryID = @CategoryID
	AND tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY
	tmp.TempID
 
DROP TABLE #TempPagedLinkIDs


SELECT  COUNT([LinkID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Links] 
WHERE 	BlogId = @BlogId 
	AND CategoryID = @CategoryID 
	AND PostID IS NULL


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableReferrers] 
(
	@BlogId INT,
	@EntryID int = NULL,
	@PageIndex INT,
	@PageSize INT
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = [UrlID] FROM [<dbUser,varchar,dbo>].[subtext_Referrals]
WHERE	BlogId = @BlogId 
	AND (EntryID = @EntryID OR @EntryID IS NULL)
ORDER BY [UrlID] DESC

SET ROWCOUNT @PageSize

SELECT	
	u.URL
	, c.Title
	, c.EntryName
	, [EntryId] = @EntryID
	, [Count]
	, r.LastUpdated
FROM [<dbUser,varchar,dbo>].[subtext_Referrals] r
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_URLs] u ON u.UrlID = r.UrlID
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c ON c.ID = r.EntryID
WHERE 
	u.UrlID <= @FirstId
	AND (r.EntryID = @EntryID OR @EntryID IS NULL)
	AND r.BlogId = @BlogId
ORDER BY u.[UrlID] DESC

SELECT COUNT([UrlID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Referrals] 
WHERE 	BlogId = @BlogId 
	AND (EntryID = @EntryID OR @EntryID IS NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableReferrers]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryID]
(
	@ItemCount int
	, @CategoryID int
	, @IsActive bit
	, @BlogId int
)
AS
SET ROWCOUNT @ItemCount
SELECT	content.BlogId
	, content.[ID]
	, content.Title
	, content.DateAdded
	, content.[Text]
	, content.[Description]
	, content.SourceUrl
	, content.PostType
	, content.Author
	, content.Email
	, content.SourceName
	, content.DateUpdated
	, content.TitleUrl
	, FeedBackCount = ISNULL(content.FeedBackCount, 0)
	, content.ParentID
	, content.PostConfig
	, content.EntryName 
	, content.ContentChecksumHash
	, content.DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content] content WITH (NOLOCK)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] links WITH (NOLOCK) ON content.ID = ISNULL(links.PostID, -1)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] categories WITH (NOLOCK) ON links.CategoryID = categories.CategoryID
WHERE  content.BlogId = @BlogId 
	AND content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END AND categories.CategoryID = @CategoryID
ORDER BY content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByDayRange]
(
	@StartDate datetime,
	@StopDate datetime,
	@BlogId int
)
AS
SELECT	BlogId
		, [ID]
		, Title
		, DateAdded
		, [Text]
		, [Description]
		, SourceUrl
		, PostType
		, Author
		, Email
		, SourceName
		, DateUpdated
		, TitleUrl
		, FeedBackCount = ISNULL(FeedBackCount, 0)
		, ParentID
		, PostConfig
		, EntryName 
		, ContentChecksumHash
		, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE 
	(
			DateAdded > @StartDate 
		AND DateAdded < DateAdd(day,1,@StopDate)
	)
	AND PostType=1 
	AND BlogId = @BlogId
ORDER BY DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByDayRange]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByMonth]
(
	@Month int
	, @Year int
	, @BlogId int = NULL
)
AS
SELECT	BlogId
	, [ID]
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, SourceUrl
	, PostType
	, Author
	, Email
	, SourceName
	, DateUpdated
	, TitleUrl
	, FeedBackCount = ISNULL(FeedBackCount, 0)
	, ParentID
	, PostConfig
	, EntryName 
	, ContentChecksumHash
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	PostType=1 
	AND (BlogId = @BlogId OR @BlogId IS NULL)
	AND PostConfig & 1 = 1 
	AND Month(DateAdded) = @Month 
	AND Year(DateAdded)  = @Year
ORDER BY DateAdded DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByMonth]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByMonthArchive]
(
	@BlogId int = NULL
)
AS
SELECT Month(DateAdded) AS [Month]
	, Year(DateAdded) AS [Year]
	, 1 AS Day, Count(*) AS [Count] 
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE PostType = 1 AND PostConfig & 1 = 1 AND (BlogId = @BlogId OR @BlogId IS NULL)
GROUP BY Year(DateAdded), Month(DateAdded) ORDER BY [Year] DESC, [Month] DESC



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByMonthArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByYearArchive] 
(
	@BlogId int
)
AS
SELECT 1 AS [Month], Year(DateAdded) AS [Year], 1 AS Day, Count(*) AS [Count] FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE PostType = 1 AND PostConfig & 1 = 1 AND BlogId = @BlogId 
GROUP BY Year(DateAdded) ORDER BY [Year] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByYearArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleDay]
(
	@Date datetime
	,@BlogId int
)
AS
SELECT	BlogId
	, [ID]
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, SourceUrl
	, PostType
	, Author
	, Email
	, SourceName
	, DateUpdated
	, TitleUrl
	, FeedBackCount = ISNULL(FeedBackCount, 0)
	, ParentID
	, PostConfig
	, EntryName 
	, ContentChecksumHash
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE Year(DateAdded) = Year(@Date) 
	AND Month(DateAdded) = Month(@Date)
    AND Day(DateAdded) = Day(@Date) 
    And PostType=1
    AND BlogId = @BlogId 
    AND PostConfig & 1 = 1 
ORDER BY DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetSingleDay]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleEntry]
(
	@ID int = NULL
	, @EntryName nvarchar(150) = NULL
	, @IsActive bit
	, @BlogId int
	, @IncludeCategories bit = 0
)
AS
SELECT	BlogId
	, [ID]
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, SourceUrl
	, PostType
	, Author
	, Email
	, SourceName
	, DateUpdated
	, TitleUrl
	, FeedBackCount = ISNULL(FeedBackCount, 0)
	, ParentID
	, PostConfig
	, EntryName 
	, ContentChecksumHash
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE ID = COALESCE(@ID, ID)
	AND IsNull(EntryName, '') = COALESCE(@EntryName, EntryName, '') 
	AND BlogId = @BlogId 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [ID] DESC

IF @IncludeCategories = 1
BEGIN
	SELECT c.Title
		, PostID = l.PostID  
	FROM [<dbUser,varchar,dbo>].[subtext_Links] l  
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID  
	WHERE l.PostID = @Id
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetSingleEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleImage]
(
	@ImageID int
	, @IsActive bit
	, @BlogId int
)
AS
SELECT Title
	, CategoryID
	, Height
	, Width
	, [File]
	, Active
	, ImageID 
FROM [<dbUser,varchar,dbo>].[subtext_Images]  
WHERE ImageID = @ImageID 
	AND BlogId = @BlogId 
	AND  Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetSingleImage]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleLink]
(
	@LinkID int
	, @BlogId int
)
AS
SELECT	subtext_Links.LinkID
		, subtext_Links.Title
		, subtext_Links.Url
		, subtext_Links.Rss
		, subtext_Links.Active
		, subtext_Links.NewWindow
		, subtext_Links.CategoryID
		, PostId = ISNULL(subtext_Links.PostID, -1)
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE subtext_Links.LinkID = @LinkID AND subtext_Links.BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetSingleLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetUrlID]
(
	@Url nvarchar(255)
	, @UrlID int output
)
AS
IF EXISTS(SELECT UrlID FROM [<dbUser,varchar,dbo>].[subtext_Urls] WHERE Url = @Url AND Url != '')
BEGIN
	SELECT @UrlID = UrlID FROM [<dbUser,varchar,dbo>].[subtext_Urls] WHERE Url = @Url
END
Else
BEGIN
	IF(@Url != '' AND NOT @Url IS NULL)
		INSERT subtext_Urls VALUES (@Url)
		SELECT @UrlID = SCOPE_IDENTITY()
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetUrlID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertCategory]
(
	@Title nvarchar(150)
	, @Active bit
	, @BlogId int
	, @CategoryType tinyint
	, @Description nvarchar(1000)
	, @CategoryID int output
)
AS
Set NoCount ON
INSERT INTO subtext_LinkCategories 
( 
	Title
	, Active
	, CategoryType
	, [Description]
	, BlogId )
VALUES 
(
	@Title
	, @Active
	, @CategoryType
	, @Description
	, @BlogId
)
SELECT @CategoryID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount]-- 1, 0, 1
(
	@EntryID int,
	@BlogId int,
	@IsWeb bit
)

AS

BEGIN
	--Do we have an existing entry in the subtext_InsertEntryViewCount table?
	IF EXISTS(SELECT EntryID FROM [<dbUser,varchar,dbo>].[subtext_EntryViewCount] WHERE EntryID = @EntryID AND BlogId = @BlogId)
	BEGIN
		if(@IsWeb = 1) -- Is this a web view?
		BEGIN
			UPDATE [<dbUser,varchar,dbo>].[subtext_EntryViewCount]
			Set [WebCount] = [WebCount] + 1, WebLastUpdated = getdate()
			WHERE EntryID = @EntryID AND BlogId = @BlogId
		END
		else
		BEGIN
			UPDATE [<dbUser,varchar,dbo>].[subtext_EntryViewCount]
			Set [AggCount] = [AggCount] + 1, AggLastUpdated = getdate()
			WHERE EntryID = @EntryID AND BlogId = @BlogId
		END
	END
	else
	BEGIN
		if(@IsWeb = 1) -- Is this a web view
		BEGIN
			Insert subtext_EntryViewCount (EntryID, BlogId, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogId, 1, 0, getdate(), NULL)
		END
		else
		BEGIN
			Insert subtext_EntryViewCount (EntryID, BlogId, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogId, 0, 1, NULL, getdate())
		END
	END


END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertImage]
(
	@Title nvarchar(250),
	@CategoryID int,
	@Width int,
	@Height int,
	@File nvarchar(50),
	@Active bit,
	@BlogId int,
	@ImageID int output
)
AS
Insert subtext_Images
(
	Title, CategoryID, Width, Height, [File], Active, BlogId
)
Values
(
	@Title, @CategoryID, @Width, @Height, @File, @Active, @BlogId
)
Set @ImageID = SCOPE_IDENTITY()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertKeyWord]
(
	@Word nvarchar(100),
	@Text nvarchar(100),
	@ReplaceFirstTimeOnly bit,
	@OpenInNewWindow bit,
	@CaseSensitive bit,
	@Url nvarchar(255),
	@Title nvarchar(100),
	@BlogId int,
	@KeyWordID int output
)

AS

Insert subtext_keywords 
	(Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogId)
Values
	(@Word,@Text,@ReplaceFirstTimeOnly,@OpenInNewWindow, @CaseSensitive,@Url,@Title,@BlogId)

SELECT @KeyWordID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertLink]
(
	@Title nvarchar(150),
	@Url nvarchar(255),
	@Rss nvarchar(255),
	@Active bit,
	@NewWindow bit,
	@CategoryID int,
	@PostID int = NULL,
	@BlogId int,
	@LinkID int output
)
AS

IF @PostID = -1
	SET @PostID = NULL

INSERT INTO subtext_Links 
( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogId )
VALUES 
(@Title, @Url, @Rss, @Active, @NewWindow, @PostID, @CategoryID, @BlogId);
SELECT @LinkID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertLinkCategoryList]
(
	@CategoryList nvarchar(4000)
	, @PostID int
	, @BlogId int
)
AS

IF @PostID = -1
	SET @PostID = NULL

--DELETE categories that have been removed
DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE 
	CategoryID not in (SELECT str FROM iter_charlist_to_table(@CategoryList,','))
And 
	BlogId = @BlogId AND (PostID = @PostID)

--Add updated/new categories
INSERT INTO subtext_Links ( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogId )
SELECT NULL, NULL, NULL, 1, 0, @PostID, Convert(int, [str]), @BlogId
FROM iter_charlist_to_table(@CategoryList,',')
WHERE 
	Convert(int, [str]) not in (SELECT CategoryID FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE PostID = @PostID AND BlogId = @BlogId)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertLinkCategoryList]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertReferral]
(
	@EntryID int,
	@BlogId int,
	@Url nvarchar(255)
)
AS

DECLARE @UrlID int

if(@Url is not NULL)
BEGIN
	EXEC [<dbUser,varchar,dbo>].[subtext_GetUrlID] @Url, @UrlID = @UrlID output
END

if(@UrlID is not NULL)
BEGIN

	IF EXISTS(SELECT EntryID FROM [<dbUser,varchar,dbo>].[subtext_Referrals] WHERE EntryID = @EntryID AND BlogId = @BlogId AND UrlID = @UrlID)
	BEGIN
		UPDATE [<dbUser,varchar,dbo>].[subtext_Referrals]
		Set [Count] = [Count] + 1, LastUpdated = getdate()
		WHERE EntryID = @EntryID AND BlogId = @BlogId AND UrlID = @UrlID
	END
	else
	BEGIN
		Insert [<dbUser,varchar,dbo>].[subtext_Referrals] (EntryID, BlogId, UrlID, [Count], LastUpdated)
		       values (@EntryID, @BlogId, @UrlID, 1, getdate())
	END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertReferral]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertViewStats]
(
	@BlogId int,
	@PageType tinyint,
	@PostID int,
	@Day datetime,
	@Url nvarchar(255)
)
AS

DECLARE @UrlID int

if(@Url is not NULL)
BEGIN
	EXEC [<dbUser,varchar,dbo>].[subtext_GetUrlID] @Url, @UrlID = @UrlID output
END
if(@UrlID is NULL)
	set @UrlID = NULL


IF EXISTS (SELECT BlogId FROM [<dbUser,varchar,dbo>].[subtext_ViewStats] WHERE BlogId = @BlogId AND PageType = @PageType AND PostID = @PostID AND [Day] = @Day AND UrlID = @UrlID AND NOT @UrlID IS NULL)
BEGIN
	UPDATE [<dbUser,varchar,dbo>].[subtext_ViewStats]
	Set [Count] = [Count] + 1
	WHERE BlogId = @BlogId AND PageType = @PageType AND PostID = @PostID AND [Day] = @Day AND UrlID = @UrlID
END
Else
BEGIN
	Insert subtext_ViewStats (BlogId, PageType, PostID, [Day], UrlID, [Count])
	Values (@BlogId, @PageType, @PostID, @Day, @UrlID, 1)
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertViewStats]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_StatsSummary]
(
	@BlogId int
)
AS
DECLARE @ReferralCount int
DECLARE @WebCount int
DECLARE @AggCount int

SELECT @ReferralCount = Sum([Count]) FROM [<dbUser,varchar,dbo>].[subtext_Referrals] WHERE BlogId = @BlogId

SELECT @WebCount = Sum(WebCount), @AggCount = Sum(AggCount) FROM [<dbUser,varchar,dbo>].[subtext_EntryViewCount] WHERE BlogId = @BlogId

SELECT @ReferralCount AS 'ReferralCount', @WebCount AS 'WebCount', @AggCount AS 'AggCount'


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_StatsSummary]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_TrackEntry]
(
	@EntryID int,
	@BlogId int,
	@Url nvarchar(255) = NULL,
	@IsWeb bit
)

AS

if(@Url is not NULL AND @IsWeb = 1)
BEGIN
	EXEC [<dbUser,varchar,dbo>].[subtext_InsertReferral] @EntryID, @BlogId, @Url
END

EXEC [<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount] @EntryID, @BlogId, @IsWeb





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_TrackEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_UTILITY_AddBlog]
(
	@Title nvarchar(100), 
	@UserName nvarchar(50),
	@Password nvarchar(50),
	@Email nvarchar(50),
	@Host nvarchar(50),
	@Application nvarchar(50),
	@IsHashed bit
)

AS

DECLARE @Flag int
Set @Flag = 55
if(@IsHashed = 1)
	Set @Flag = 63

IF NOT EXISTS(SELECT * FROM [<dbUser,varchar,dbo>].[subtext_config] WHERE Host = @Host AND Application = @Application)
BEGIN

INSERT subtext_Config  
(
	LastUpdated
	, UserName
	, Password
	, Email
	, Title
	, SubTitle
	, Skin
	, SkinCssFile
	, Application
	, Host
	, Author
	, TimeZone
	, [Language]
	, ItemCount
	, Flag
)
Values             
(
	getdate()
	, @UserName
	, @Password
	, @Email
	, @Title
	, 'Another Subtext Powered Blog'
	, 'RedBook'
	, 'blue.css'
	, @Application
	, @Host
	, 'Blog Author'
	, -5
	,'en-US'
	, 10
	, @Flag
)
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UTILITY_AddBlog]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateCategory]
(
	@CategoryID int,
	@Title nvarchar(150),
	@Active bit,
	@CategoryType tinyint,
	@Description nvarchar(1000),
	@BlogId int
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_LinkCategories] 
SET 
	[Title] = @Title, 
	[Active] = @Active,
	[CategoryType] = @CategoryType,
	[Description] = @Description
WHERE   
		[CategoryID] = @CategoryID 
	AND [BlogId] = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateConfig]
(
	@UserName nvarchar(50)
	, @Password nvarchar(50)
	, @Email nvarchar(50)
	, @Title nvarchar(100)
	, @SubTitle nvarchar(250)
	, @Skin nvarchar(50)
	, @Application nvarchar(50)
	, @Host nvarchar(100)
	, @Author nvarchar(100)
	, @Language nvarchar(10)
	, @TimeZone int = NULL
	, @ItemCount int
	, @News nText = NULL
	, @LastUpdated datetime = NULL
	, @SecondaryCss nText = NULL
	, @SkinCssFile varchar(100) = NULL
	, @Flag int = NULL
	, @BlogId int
	, @LicenseUrl nvarchar(64) = NULL
	, @DaysTillCommentsClose int = NULL
	, @CommentDelayInMinutes int = NULL
	, @NumberOfRecentComments int = NULL
	, @RecentCommentsLength int = NULL
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Config]
Set
	UserName = @UserName     
	, [Password] = @Password     
	, Email = @Email        
	, Title	   =   @Title        
	, SubTitle   =   @SubTitle     
	, Skin	  =    @Skin         
	, Application =  @Application  
	, Host	  =    @Host         
	, Author	   =   @Author
	, [Language] = @Language
	, TimeZone   = @TimeZone
	, ItemCount = @ItemCount
	, News      = @News
	, LastUpdated = @LastUpdated
	, Flag = @Flag
	, SecondaryCss = @SecondaryCss
	, SkinCssFile = @SkinCssFile
	, LicenseUrl = @LicenseUrl
	, DaysTillCommentsClose = @DaysTillCommentsClose
	, CommentDelayInMinutes = @CommentDelayInMinutes
	, NumberOfRecentComments = @NumberOfRecentComments
	, RecentCommentsLength = @RecentCommentsLength
WHERE BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateConfig]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime]
(
	@BlogId int,
	@LastUpdated datetime
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Config]
SET LastUpdated = @LastUpdated
WHERE BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateEntry]
(
	@ID int
	, @Title nvarchar(255)
	, @TitleUrl nvarchar(255) = NULL
	, @Text ntext = NULL
	, @SourceUrl nvarchar(200) = NULL
	, @PostType int
	, @Author nvarchar(50) = NULL
	, @Email nvarchar(50) = NULL
	, @Description nvarchar(500) = NULL
	, @SourceName nvarchar(200) = NULL
	, @DateUpdated datetime = NULL
	, @PostConfig int
	, @ParentID int = NULL
	, @EntryName nvarchar(150) = NULL
	, @ContentChecksumHash varchar(32)
	, @DateSyndicated DateTime = NULL
	, @BlogId int
)
AS

IF(LEN(RTRIM(LTRIM(@EntryName))) = 0)
	SET @EntryName = NULL

IF(@EntryName IS NOT NULL)
BEGIN
	IF EXISTS(SELECT EntryName FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = @BlogId AND EntryName = @EntryName AND [ID] <> @ID)
	BEGIN
		RAISERROR('The EntryName of your entry is already in use with in this Blog. Please pick a unique EntryName.', 11, 1) 
		RETURN 1
	END
END
IF(LTRIM(RTRIM(@Description)) = '')
SET @Description = NULL

UPDATE [<dbUser,varchar,dbo>].[subtext_Content] 
SET 
	Title = @Title 
	, TitleUrl = @TitleUrl 
	, [Text] = @Text 
	, SourceUrl = @SourceUrl 
	, PostType = @PostType
	, Author = @Author 
	, Email = @Email 
	, [Description] = @Description
	, DateUpdated = @DateUpdated
	, PostConfig = @PostConfig
	, ParentID = @ParentID
	, SourceName = @SourceName
	, EntryName = @EntryName
	, ContentChecksumHash = @ContentChecksumHash
	, DateSyndicated = @DateSyndicated
WHERE 	
		[ID] = @ID 
	AND BlogId = @BlogId
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime] @BlogId, @DateUpdated

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateImage]
(
	@Title nvarchar(250),
	@CategoryID int,
	@Width int,
	@Height int,
	@File nvarchar(50),
	@Active bit,
	@BlogId int,
	@ImageID int
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Images]
Set
	Title = @Title,
	CategoryID = @CategoryID,
	Width = @Width,
	Height = @Height,
	[File] = @File,
	Active = @Active
	
WHERE
	ImageID = @ImageID AND BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateKeyWord]
(
	@KeyWordID int,
	@Word nvarchar(100),
	@Text nvarchar(100),
	@ReplaceFirstTimeOnly bit,
	@OpenInNewWindow bit,
	@CaseSensitive bit,
	@Url nvarchar(255),
	@Title nvarchar(100),
	@BlogId int
)

AS

UPDATE [<dbUser,varchar,dbo>].[subtext_keywords] 
	Set
		Word = @Word,
		[Text] = @Text,
		ReplaceFirstTimeOnly = @ReplaceFirstTimeOnly,
		OpenInNewWindow = @OpenInNewWindow,
		CaseSensitive = @CaseSensitive,
		Url = @Url,
		Title = @Title
	WHERE
		BlogId = @BlogId AND KeyWordID = @KeyWordID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateLink]
(
	@LinkID int,
	@Title nvarchar(150),
	@Url nvarchar(255),
	@Rss nvarchar(255),
	@Active bit,
	@NewWindow bit,
	@CategoryID int,
	@BlogId int
	
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Links] 
SET 
	Title = @Title, 
	Url = @Url, 
	Rss = @Rss, 
	Active = @Active,
	NewWindow = @NewWindow, 
	CategoryID = @CategoryID
WHERE  
		LinkID = @LinkID 
	AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_Utility_GetUnHashedPasswords]
AS

SELECT BlogId, Password FROM [<dbUser,varchar,dbo>].[subtext_COnfig] WHERE Flag & 8 = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_Utility_GetUnHashedPasswords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_Utility_UpdateToHashedPassword]
(
	@Password nvarchar(100),
	@BlogId int
)

AS

UPDATE [<dbUser,varchar,dbo>].[subtext_Config]
Set 
	Password = @Password,
	Flag = Flag | 8 
WHERE BlogId = @BlogId



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_Utility_UpdateToHashedPassword]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Returns a page of blogs within subtext_config table
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]
(
	@PageIndex int
	, @PageSize int
	, @Host nvarchar(100) = NULL
	, @ConfigurationFlags int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = [BlogId] FROM [<dbUser,varchar,dbo>].[subtext_Config]
WHERE @ConfigurationFlags & Flag = @ConfigurationFlags
	AND (Host = @Host OR @Host IS NULL)
ORDER BY [BlogId] DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	blog.BlogId 
		, blog.UserName
		, blog.[Password]
		, blog.Email
		, blog.Title
		, blog.SubTitle
		, blog.Skin
		, blog.Application
		, blog.Host
		, blog.Author
		, blog.TimeZone
		, blog.ItemCount
		, blog.[Language]
		, blog.News
		, blog.SecondaryCss
		, blog.LastUpdated
		, blog.PostCount
		, blog.StoryCount
		, blog.PingTrackCount
		, blog.CommentCount
		, blog.IsAggregated
		, blog.Flag
		, blog.SkinCssFile 
		, blog.BlogGroup
		, blog.LicenseUrl
		, blog.DaysTillCommentsClose
		, blog.CommentDelayInMinutes
		, blog.NumberOfRecentComments
		, blog.RecentCommentsLength
		
FROM [<dbUser,varchar,dbo>].[subtext_config] blog
WHERE blog.BlogId <= @FirstId
	AND @ConfigurationFlags & Flag = @ConfigurationFlags
	AND (Host = @Host OR @Host IS NULL)

SELECT COUNT([BlogId]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_config]
WHERE @ConfigurationFlags & Flag = @ConfigurationFlags
	AND (Host = @Host OR @Host IS NULL)
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Returns a single blog within the subtext_config table by id.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetBlogById]
(
	@BlogId int
)
AS

SELECT	blog.BlogId 
		, blog.UserName
		, blog.[Password]
		, blog.Email
		, blog.Title
		, blog.SubTitle
		, blog.Skin
		, blog.Application
		, blog.Host
		, blog.Author
		, blog.TimeZone
		, blog.ItemCount
		, blog.[Language]
		, blog.News
		, blog.SecondaryCss
		, blog.LastUpdated
		, blog.PostCount
		, blog.StoryCount
		, blog.PingTrackCount
		, blog.CommentCount
		, blog.IsAggregated
		, blog.Flag
		, blog.SkinCssFile 
		, blog.BlogGroup
		, blog.LicenseUrl
		, blog.DaysTillCommentsClose
		, blog.CommentDelayInMinutes
		, blog.NumberOfRecentComments
		, blog.RecentCommentsLength
		
FROM [<dbUser,varchar,dbo>].[subtext_config] blog
WHERE	blog.BlogId = @BlogId
GO


GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetBlogById]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
--//TODO: We can probably merge the following two procedures.
CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertPingTrackEntry]
(
	@Title nvarchar(255)
	, @TitleUrl nvarchar(255) = NULL
	, @Text ntext = NULL
	, @SourceUrl nvarchar(200) = NULL
	, @PostType int
	, @Author nvarchar(50) = NULL
	, @Email nvarchar(50) = NULL
	, @SourceName nvarchar(200) = NULL
	, @Description nvarchar(500) = NULL
	, @BlogId int
	, @DateAdded datetime
	, @ParentID int = NULL
	, @PostConfig int
	, @EntryName nvarchar(150) = NULL
	, @ContentChecksumHash varchar(32)
	, @ID int output
)
AS

-- Do not insert EntryNames. No needed for comments AND tracks. 
-- To messy anyway

SET @ID = -1

IF NOT EXISTS(SELECT [ID] FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE TitleUrl = @TitleUrl AND ParentID = @ParentID)
BEGIN

IF(LTRIM(RTRIM(@Description)) = '')
SET @Description = NULL

INSERT INTO subtext_Content 
( 
	PostConfig
	, Title
	, TitleUrl
	, [Text]
	, SourceUrl
	, PostType
	, Author
	, Email
	, DateAdded
	, DateUpdated
	, SourceName
	, [Description]
	, ContentChecksumHash
	, ParentID
	, BlogId
)
VALUES 
(
	@PostConfig
	, @Title
	, @TitleUrl
	, @Text
	, @SourceUrl
	, @PostType
	, @Author
	, @Email
	, @DateAdded
	, @DateAdded
	, @SourceName
	, @Description
	, @ContentChecksumHash
	, @ParentID
	, @BlogId
)

SELECT @ID = SCOPE_IDENTITY()

UPDATE [<dbUser,varchar,dbo>].[subtext_Content]
SET FeedBackCount = FeedBackCount + 1 
WHERE [ID] = @ParentID

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertPingTrackEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertEntry]
(
	@Title nvarchar(255)
	, @TitleUrl nvarchar(255) = NULL
	, @Text ntext = NULL
	, @SourceUrl nvarchar(200) = NULL
	, @PostType int
	, @Author nvarchar(50) = NULL
	, @Email nvarchar(50) = NULL
	, @SourceName nvarchar(200) = NULL
	, @Description nvarchar(500) = NULL
	, @BlogId int
	, @DateAdded datetime
	, @ParentID int = NULL
	, @PostConfig int
	, @EntryName nvarchar(150) = NULL
	, @ContentChecksumHash varchar(32)
	, @DateSyndicated DateTime = NULL
	, @ID int output
)
AS

IF(LEN(RTRIM(LTRIM(@EntryName))) = 0)
	SET @EntryName = NULL

IF(@EntryName IS NOT NULL)
BEGIN
	IF EXISTS(SELECT EntryName FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = @BlogId AND EntryName = @EntryName)
	BEGIN
		RAISERROR('The EntryName of your entry is already in use with in this Blog. Please pick a unique EntryName.', 11, 1) 
		RETURN 1
	END
END
IF(LTRIM(RTRIM(@Description)) = '')
SET @Description = NULL

INSERT INTO subtext_Content 
(
	Title
	, TitleUrl
	, [Text]
	, SourceUrl
	, PostType
	, Author
	, Email
	, DateAdded
	, DateUpdated
	, SourceName
	, [Description]
	, PostConfig
	, ParentID
	, FeedBackCount
	, BlogId
	, EntryName 
	, ContentChecksumHash
	, DateSyndicated
)
VALUES 
(
	@Title
	, @TitleUrl
	, @Text
	, @SourceUrl
	, @PostType
	, @Author
	, @Email
	, @DateAdded
	, @DateAdded
	, @SourceName
	, @Description
	, @PostConfig
	, @ParentID
	, 0
	, @BlogId
	, @EntryName
	, @ContentChecksumHash
	, @DateSyndicated
)
SELECT @ID = SCOPE_IDENTITY()
-- PostType
--	1 = BlogPost
--	2 = Story
if(@PostType = 1 or @PostType = 2)
BEGIN
	EXEC [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime] @BlogId, @DateAdded
END
ELSE IF(@PostType = 3) -- Comment
BEGIN
	UPDATE [<dbUser,varchar,dbo>].[subtext_Content]
	Set FeedBackCount = ISNULL(FeedBackCount, 0) + 1 WHERE [ID] = @ParentID
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertEntry]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Retrieves a comment (or pingback) that has the specified 
ContentChecksumHash.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetCommentByChecksumHash]
(
	@ContentChecksumHash VARCHAR(32)
	,@BlogId int
)
AS
SELECT TOP 1 BlogId
	, [ID]
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, SourceUrl
	, PostType
	, Author
	, Email
	, SourceName
	, DateUpdated
	, TitleUrl
	, FeedBackCount
	, ParentID
	, PostConfig
	, EntryName 
	, ContentChecksumHash
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE 
	ContentChecksumHash = @ContentChecksumHash 
	AND BlogId = @BlogId
	AND (PostType = 3 OR PostType = 4) -- Comment or PingBack
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetCommentByChecksumHash]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Retrieves the Host Information. There should only be 
one record.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetHost]
AS
SELECT 
	[HostUserName]
	, [Password]
	, [Salt]
	, [DateCreated]
FROM [<dbUser,varchar,dbo>].[subtext_Host]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetHost]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Updates the Host information.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateHost]
	@HostUserName NVARCHAR(64)
	, @Password NVARCHAR(32)
	, @Salt NVARCHAR(32)
AS
IF EXISTS(SELECT * FROM [<dbUser,varchar,dbo>].[subtext_Host])
BEGIN
	UPDATE [<dbUser,varchar,dbo>].[subtext_Host] 
		SET [HostUserName] = @HostUserName
			, [Password] = @Password
			, [Salt] = @Salt
END
ELSE
BEGIN
	INSERT subtext_Host
	(
		[HostUserName]
		,[Password]
		,[Salt]
		,[DateCreated]
	)
	VALUES
	(
		@HostUserName
		,@Password
		,@Salt
		,getdate()
	)
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateHost]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[DNW_GetRecentPosts]
	@Host nvarchar(100)
	, @GroupID int

AS
SELECT Top 35 Host
	, Application
	, [EntryName] = IsNull(content.EntryName, content.[ID])
	, content.[ID]
	, content.Title
	, content.DateAdded
	, content.SourceUrl
	, content.PostType
	, content.Author
	, content.Email
	, content.FeedBackCount
	, content.SourceName
	, content.EntryName
	, [IsXHTML] = Convert(bit,CASE WHEN content.PostConfig & 2 = 2 THEN 1 else 0 END) 
	, [BlogTitle] = content.Title
	, content.PostConfig
	, config.TimeZone
	, [Description] = IsNull(CASE WHEN PostConfig & 32 = 32 THEN content.[Description] else content.[Text] END, '')
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
INNER JOIN	[<dbUser,varchar,dbo>].[subtext_Config] config ON content.BlogId = config.BlogId
WHERE  content.PostType = 1 
	AND content.PostConfig & 1 = 1 
	AND content.PostConfig & 64 = 64 
	AND config.Flag & 2 = 2 
	AND config.Host = @Host
	AND BlogGroup & @GroupID = @GroupID
ORDER BY [ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[DNW_GetRecentPosts]  TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[DNW_Stats]
(
	@Host nvarchar(100),
	@GroupID int
)
AS
SELECT BlogId
	, Author
	, Application
	, Host
	, Title
	, PostCount
	, CommentCount
	, StoryCount
	, PingTrackCount
	, LastUpdated
FROM [<dbUser,varchar,dbo>].[subtext_Config] 
WHERE PostCount > 0 AND subtext_Config.Flag & 2 = 2 AND Host = @Host AND BlogGroup & @GroupID = @GroupID
ORDER BY PostCount DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[DNW_Stats]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[DNW_Total_Stats]
(
	@Host nvarchar(100),
	@GroupID int
)
AS
SELECT Count(*) AS [BlogCount], Sum(PostCount) AS PostCount, Sum(CommentCount) AS CommentCount, Sum(StoryCount) AS StoryCount, Sum(PingTrackCount) AS PingTrackCount 
FROM [<dbUser,varchar,dbo>].[subtext_Config] WHERE subtext_Config.Flag & 2 = 2 AND Host = @Host AND BlogGroup & @GroupID = @GroupID

SET QUOTED_IDENTIFIER ON


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [DNW_Total_Stats]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[DNW_HomePageData]
(
	@Host nvarchar(100),
	@GroupID int
)
AS 
EXEC [<dbUser,varchar,dbo>].[DNW_Stats] @Host, @GroupID
EXEC [<dbUser,varchar,dbo>].[DNW_GetRecentPosts] @Host, @GroupID
EXEC [<dbUser,varchar,dbo>].[DNW_Total_Stats] @Host, @GroupID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[DNW_HomePageData]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Gets the most recent version in the Version table */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_VersionGetCurrent]
AS
SELECT	TOP 1
		[Id]
		, [Major]
		, [Minor]
		, [Build]
		, [DateCreated]
FROM	[<dbUser,varchar,dbo>].[subtext_Version]
ORDER BY [DateCreated] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_VersionGetCurrent]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Gets the most recent version in the Version table */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_VersionAdd]
(
	 @Major	INT
	, @Minor INT
	, @Build INT
	, @DateCreated DATETIME = NULL
	, @Id INT = NULL OUTPUT
)
AS

IF @DateCreated IS NULL
	SET @DateCreated = getdate()

INSERT [<dbUser,varchar,dbo>].[subtext_Version]
SELECT	@Major, @Minor, @Build, @DateCreated

SELECT @Id = SCOPE_IDENTITY()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_VersionAdd]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Creates a record in the subtext_log table */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_LogClear]
(
	@BlogId int = NULL
)
AS

IF(@BlogId IS NULL)
	TRUNCATE TABLE [<dbUser,varchar,dbo>].[subtext_Log]
ELSE
	DELETE [<dbUser,varchar,dbo>].[subtext_Log] WHERE [BlogId] = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_LogClear]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Creates a record in the subtext_log table */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_AddLogEntry]
(
	 @Date DateTime
	 , @BlogId int = NULL
	 , @Thread varchar(255)
	 , @Context varchar(512)
	 , @Level varchar(20)
	 , @Logger nvarchar(256)
	 , @Message nvarchar(2000)
	 , @Exception nvarchar(1000)
	 , @Url varchar(255)
)
AS

if @BlogId < 0
	SET @BlogId = NULL

INSERT [<dbUser,varchar,dbo>].[subtext_Log]
SELECT	@BlogId, @Date, @Thread, @Context, @Level, @Logger, @Message, @Exception, @Url

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_AddLogEntry]  TO [public]
GO

/*Search Entries-GY*/
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].subtext_SearchEntries
(
	@BlogId int
	, @SearchStr nvarchar(30)
)
as

Set @SearchStr = '%' + @SearchStr + '%'

Select [ID]
	, Title
	, DateAdded
	, EntryName
	, TitleUrl
	, PostType
From [<dbUser,varchar,dbo>].[subtext_Content]
Where (PostType = 1 OR PostType = 2)
	AND PostConfig & 1 = 1 -- IsActive!
	AND ([Text] LIKE @SearchStr OR Title LIKE @SearchStr)
	AND BlogId = @BlogId
	
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_SearchEntries]  TO [public]
GO

/*Previous Next*/
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[Subtext_GetEntry_PreviousNext]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[Subtext_GetEntry_PreviousNext]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[Subtext_GetEntry_PreviousNext]
(
	@ID int
	, @PostType int = 1
	, @BlogId int
)
AS

DECLARE @DateSyndicated DateTime
SELECT @DateSyndicated = ISNULL(DateSyndicated, DateAdded) 
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE ID = @ID

SELECT * FROM
(
	SELECT Top 1 BlogId
		, [ID]
		, Title
		, DateAdded
		, PostType
		, TitleUrl
		, PostConfig
		, EntryName 
		, DateSyndicated
		, CardinalityDate = ISNULL(DateSyndicated, DateAdded) -- Must be here to order by
	FROM [<dbUser,varchar,dbo>].[subtext_Content]
	WHERE ISNULL([DateSyndicated], [DateAdded]) >= @DateSyndicated
		AND Subtext_Content.BlogId = @BlogId 
		AND Subtext_Content.PostConfig & 1 = 1 
		AND PostType = @PostType
		AND [ID] != @ID
	ORDER BY ISNULL(DateSyndicated, DateAdded) ASC
) [Previous]
UNION
SELECT * FROM
(
	SELECT Top 1 BlogId
		, [ID]
		, Title
		, DateAdded
		, PostType
		, TitleUrl
		, PostConfig
		, EntryName 
		, DateSyndicated
		, CardinalityDate = ISNULL(DateSyndicated, DateAdded)
	FROM [<dbUser,varchar,dbo>].[subtext_Content]
	WHERE ISNULL([DateSyndicated], [DateAdded]) <= @DateSyndicated
		AND Subtext_Content.BlogId = @BlogId 
		AND Subtext_Content.PostConfig & 1 = 1 
		AND PostType = @PostType
		AND [ID] != @ID
	ORDER BY ISNULL(DateSyndicated, DateAdded) DESC
) [Next]

ORDER BY CardinalityDate DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[Subtext_GetEntry_PreviousNext]  TO [public]
GO


/*Get Related Links (called from RelatedLinks.ascx) - GY*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetRelatedLinks] 
@BlogId int,
@EntryID int
AS

Select Distinct Top 10 c.ID EntryID, c.Title, c.DateAdded 
From [<dbUser,varchar,dbo>].subtext_LinkCategories lc, [<dbUser,varchar,dbo>].subtext_Links l, [<dbUser,varchar,dbo>].subtext_Content c 
Where lc.CategoryType = 1 
And lc.Active = 1
And l.CategoryID = lc.CategoryID
And l.CategoryID In (Select CategoryID From [<dbUser,varchar,dbo>].subtext_links Where PostID = @EntryID)
And l.PostID = c.ID
And c.BlogId = @BlogId --param
And c.ID <> @EntryID --param --do not list the same entry in related links
Order By c.DateAdded Desc


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetRelatedLinks]  TO [public]
GO

/*Top10Posts - (called from Top10Module.ascx) - GY*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]  
@BlogId int
AS
Select Distinct top 10 evc.EntryId, (evc.WebCount + evc.AggCount) As mcount, c.title, c.DateAdded
From [<dbUser,varchar,dbo>].subtext_EntryViewCount evc, [<dbUser,varchar,dbo>].subtext_Content c
Where evc.EntryId = c.Id
And c.BlogId = @BlogId --param
Order By mcount desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]  TO [public]
GO

/*
Selects a page of blog posts for export to blogml. These are 
sorted ascending by id to map to the database.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = [ID] FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	BlogId = @BlogId 
	AND (PostType = 1 OR PostType = 2) -- PostType 1 = BlogPost, 2 = Story
ORDER BY [ID] ASC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

CREATE Table #IDs  
(  
	 TempId int IDENTITY (0, 1) NOT NULL,  
	 Id int not NULL  
)

-- Store the IDs for this page in a temp table.
INSERT #IDs (Id)  
SELECT [Id]   
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	(PostType = 1 OR PostType = 2)
	AND BlogId = @BlogId
	AND [ID] >= @FirstId
ORDER BY Id ASC

SET ROWCOUNT 0

SELECT	content.BlogId 
		, idTable.[ID] 
		, content.Title 
		, content.DateAdded 
		, content.[Text] 
		, content.[Description]
		, content.SourceUrl 
		, content.PostType 
		, content.Author 
		, content.Email 
		, content.SourceName 
		, content.DateUpdated 
		, content.TitleUrl 
		, FeedBackCount = ISNULL(content.FeedBackCount, 0)
		, content.ParentID
		, content.PostConfig
		, content.EntryName
		, content.ContentChecksumHash
		, content.DateSyndicated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	INNER JOIN #IDs idTable ON idTable.Id = content.[ID]
ORDER BY idTable.[ID] ASC
 
SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE 	BlogId = @BlogId 
	AND PostType = 1 OR PostType = 2

-- Select associated categories
SELECT	p.[Id]
		, c.CategoryID
	FROM [<dbUser,varchar,dbo>].[subtext_Links] l
		INNER JOIN #IDs p ON l.[PostID] = p.[ID]  
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID
	ORDER BY p.[ID] ASC

-- Select associated comments
SELECT	content.BlogId 
		, idTable.[ID] 
		, content.Title 
		, content.DateAdded 
		, content.[Text] 
		, content.[Description]
		, content.SourceUrl 
		, content.PostType 
		, content.Author 
		, content.Email 
		, content.SourceName 
		, content.DateUpdated 
		, content.TitleUrl 
		, FeedBackCount = ISNULL(content.FeedBackCount, 0)
		, content.ParentID
		, content.PostConfig
		, content.EntryName
		, content.ContentChecksumHash
		, content.DateSyndicated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	INNER JOIN #IDs idTable ON idTable.Id = content.[ParentID]
	WHERE content.PostType = 3 -- Comment
ORDER BY idTable.[ID] ASC

-- Select associated track/ping backs.
SELECT	content.BlogId 
		, idTable.[ID] 
		, content.Title 
		, content.DateAdded 
		, content.[Text] 
		, content.[Description]
		, content.SourceUrl 
		, content.PostType 
		, content.Author 
		, content.Email 
		, content.SourceName 
		, content.DateUpdated 
		, content.TitleUrl 
		, FeedBackCount = ISNULL(content.FeedBackCount, 0)
		, content.ParentID
		, content.PostConfig
		, content.EntryName
		, content.ContentChecksumHash
		, content.DateSyndicated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	INNER JOIN #IDs idTable ON idTable.Id = content.[ParentID]
	WHERE content.PostType = 4 -- Pingback
ORDER BY idTable.[ID] ASC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl] TO [public]
GO




/*
	subtext_GetPostsByCategoriesArchive - (called from CategoryCloud.ascx) - SCH
	retrieves all active categories with realative post number
*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoriesArchive]  
(
	@BlogId int = NULL
)
AS


select subtext_LinkCategories.CategoryID, subtext_LinkCategories.Title, count(*) as numPosts

from subtext_LinkCategories inner join subtext_Links on subtext_LinkCategories.CategoryID=subtext_Links.CategoryID
where			subtext_LinkCategories.Active= 1 
	AND		(subtext_LinkCategories.BlogId = @BlogId OR @BlogId IS NULL)
	AND		subtext_LinkCategories.CategoryType = 1 -- post category

group by subtext_LinkCategories.CategoryID, subtext_LinkCategories.Title
order by subtext_LinkCategories.Title

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoriesArchive]  TO [public]
GO