
/*
WARNING: This SCRIPT USES SQL TEMPLATE PARAMETERS.
Be sure to hit CTRL+SHIFT+M in Query Analyzer 2005 or choose Specify Values for Template Parameters in Query Analyzer 2008 if running manually.

When generating drop and create from SQL Query Analyzer, you can 
use the following search and replace expressions to convert the 
script to use INFORMATION_SCHEMA.

SEARCH:  IF:b* EXISTS \(SELECT \* FROM dbo\.sysobjects WHERE id = OBJECT_ID\(N'\[[^\]]+\]\.\[{[^\]]+}\]'\) AND OBJECTPROPERTY\(id,:b*N'IsProcedure'\) = 1\)
REPLACE: IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = '\1' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')

*/

/* DROPPED STORED PROCS.  
	These are stored procs that used to be in the system but are no longer needed.
	The statements will only drop the procs if they exist as a form of cleanup.
*/
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetSingleDay]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetSingleDay]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPopularPosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPopularPosts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogStats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetRelatedLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetRelatedLinks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetAllCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetAllCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetCategoryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetCategoryByName]
GO

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

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertEntryTagList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertEntryTagList]
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

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteFeedbackByStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteFeedbackByStatus]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteFeedback]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteFeedback]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetCategory]
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

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetFeedbackCollection]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetFeedbackCollection]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetFeedbackCountsByStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetFeedbackCountsByStatus]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetFeedback]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetFeedback]
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

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntriesForExport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntriesForExport]
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

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertFeedback]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertFeedback]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_UpdateBlogStats')
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateFeedback]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateFeedback]
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

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetRelatedEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetRelatedEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetTopEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetTopEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByCategoriesArchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].subtext_GetPostsByCategoriesArchive
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByTag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByTag]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertEntryTagList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertEntryTagList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetTopTags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetTopTags]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_InsertMetaTag')
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertMetaTag]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_UpdateMetaTag')
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateMetaTag]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_GetMetaTags')
drop procedure [<dbUser,varchar,dbo>].[subtext_GetMetaTags]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_DeleteMetaTag')
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteMetaTag]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_InsertEnclosure')
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertEnclosure]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_UpdateEnclosure')
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateEnclosure]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_DeleteEnclosure')
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteEnclosure]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_ClearBlogContent')
drop procedure [<dbUser,varchar,dbo>].[subtext_ClearBlogContent]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogByDomainAlias]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogByDomainAlias]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableDomainAliases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableDomainAliases]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_CreateDomainAlias]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_CreateDomainAlias]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteDomainAlias]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteDomainAlias]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateDomainAlias]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateDomainAlias]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetDomainAliasById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetDomainAliasById]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteBlogGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteBlogGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertBlogGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertBlogGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateBlogGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateBlogGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_ListBlogGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_ListBlogGroups]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[DNW_GetRecentImages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[DNW_GetRecentImages]
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
            INSERT INTO @tbl (str, nstr) VALUES(@tmpval, @tmpval)
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
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats]
(
	@BlogId int,
	@CurrentDateTime datetime
)
AS
	-- Update the blog comment count.
	UPDATE [<dbUser,varchar,dbo>].[subtext_Config] 
	SET CommentCount = 
		(
			SELECT COUNT(1) 
			FROM  [<dbUser,varchar,dbo>].[subtext_FeedBack] f WITH (NOLOCK)
			    INNER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c WITH (NOLOCK) ON c.ID = f.EntryId
			WHERE f.BlogId = @BlogId
				AND f.StatusFlag & 1 = 1
				AND f.StatusFlag & 8 != 8
				AND f.FeedbackType = 1
				AND c.PostConfig & 1 = 1
				AND c.DateSyndicated <= @CurrentDateTime
		)
	WHERE BlogId = @BlogId
	
	-- Update the blog trackback count.
	UPDATE [<dbUser,varchar,dbo>].[subtext_Config] 
	SET PingTrackCount = 
		(
			SELECT COUNT(1) 
			FROM  [<dbUser,varchar,dbo>].[subtext_FeedBack] f WITH (NOLOCK)
			    INNER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c WITH (NOLOCK) ON c.ID = f.EntryId
			WHERE f.BlogId = @BlogId
				AND f.StatusFlag & 1 = 1
				AND f.StatusFlag & 8 != 8
				AND f.FeedbackType = 2
				AND c.PostConfig & 1 = 1
				AND c.DateSyndicated <= @CurrentDateTime
		)
	WHERE BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats] TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats]
(
	@BlogId int,
	@CurrentDateTime datetime
)
AS

    UPDATE [<dbUser,varchar,dbo>].[subtext_Config]  
    SET PostCount = 
	    (
		    SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Content] 
		    WHERE BlogId = @BlogId 
				AND PostType = 1 
				AND PostConfig & 1 = 1
				AND DateSyndicated <= @CurrentDateTime
	    ),
	    StoryCount = 
	    (
	        SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Content] 
	        WHERE BlogId = @BlogId 
				AND PostType = 2 
				AND PostConfig & 1 = 1
				AND DateSyndicated <= @CurrentDateTime
	    )
    WHERE BlogId = @BlogId
    
    EXEC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats] @BlogId, @CurrentDateTime

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount]
(
	@BlogId int
	,@EntryId int
	,@CurrentDateTime datetime
)
AS
	-- Update the entry comment count.
	UPDATE [<dbUser,varchar,dbo>].[subtext_Content] 
	SET [<dbUser,varchar,dbo>].[subtext_Content].FeedbackCount = 
		(
			SELECT COUNT(1) 
			FROM  [<dbUser,varchar,dbo>].[subtext_FeedBack] f  WITH (NOLOCK)
			WHERE f.EntryId = @EntryId 
				AND f.StatusFlag & 1 = 1
				AND f.StatusFlag & 8 != 8
		)
	WHERE Id = @EntryId

	-- Update the blog comment count.
	EXEC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats] @BlogId, @CurrentDateTime

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] TO [public]
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
Returns a count of feedback for the various statuses.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetFeedbackCountsByStatus]
(
	@BlogId int,
	@ApprovedCount int out,
	@NeedsModerationCount int out,
	@FlaggedSpam int out,
	@Deleted int out	
)
AS

SELECT @ApprovedCount = COUNT(1) 
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] 
WHERE BlogId = @BlogId AND StatusFlag & 1 = 1 AND StatusFlag & 8 != 8

SELECT @NeedsModerationCount = COUNT(1) 
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] 
WHERE BlogId = @BlogId AND StatusFlag & 2 = 2 AND StatusFlag & 8 != 8 AND StatusFlag & 1 != 1

SELECT @FlaggedSpam = COUNT(1) 
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] 
WHERE BlogId = @BlogId AND StatusFlag & 4 = 4 AND StatusFlag & 8 != 8 AND StatusFlag & 1 != 1

SELECT @Deleted = COUNT(1) 
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] 
WHERE BlogId = @BlogId AND StatusFlag & 8 = 8

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetFeedbackCountsByStatus] TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Fully deletes a Feedback item from the db.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteFeedback]
(
	@Id int
	,@CurrentDateTime datetime
)
AS

DECLARE @EntryId int
DECLARE @BlogId int

SELECT @EntryId = EntryId, @BlogId = BlogId FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE [Id] = @Id

DELETE [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE [Id] = @Id

exec [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] @BlogId, @EntryId, @CurrentDateTime
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteFeedback] TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Fully deletes a Feedback item from the db.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteFeedbackByStatus]
(
	@BlogId int
	, @StatusFlag int
)
AS

DELETE [<dbUser,varchar,dbo>].[subtext_FeedBack] 
WHERE [BlogId] = @BlogId 
	AND StatusFlag & @StatusFlag = @StatusFlag
	AND (
			(@StatusFlag = 4 AND StatusFlag & 8 != 8)
			OR
			@StatusFlag != 4
		)	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteFeedbackByStatus] TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Deletes a record FROM [<dbUser,varchar,dbo>].[subtext_Content], whether it be a post, a comment, etc..
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeletePost]
(
	@ID int
	,@CurrentDateTime datetime
)
AS

DECLARE @blogId int
SET @blogId = (select BlogId from [<dbUser,varchar,dbo>].[subtext_Content] where [ID] = @ID)

DELETE FROM [<dbUser,varchar,dbo>].[subtext_EntryTag] WHERE EntryId = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE PostID = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_EntryViewCount] WHERE EntryID = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Referrals] WHERE EntryID = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE EntryId = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Enclosure] WHERE EntryId = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE [ID] = @ID

EXEC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] @blogId, @CurrentDateTime

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
SELECT [CategoryID]
	, [Title]
	, [Active]
	, [CategoryType]
	, [Description]
	, [BlogId]
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories]
WHERE	
			subtext_LinkCategories.Active= 1 
	AND		(subtext_LinkCategories.BlogId = @BlogId OR @BlogId IS NULL)
	AND		subtext_LinkCategories.CategoryType = 5
ORDER BY 
	subtext_LinkCategories.Title;

SELECT Id = links.LinkID
	, links.Title
	, links.Url
	, links.Rss
	, IsActive = links.Active
	, links.NewWindow
	, links.CategoryID
	, Relation = links.Rel
	, links.BlogId
	, PostID = links.PostID
FROM [<dbUser,varchar,dbo>].[subtext_Links] links
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] categories ON links.CategoryID = categories.CategoryID
WHERE 
		links.Active = 1 
	AND categories.Active = 1
	AND (categories.BlogId = @BlogId OR @BlogId IS NULL)
	AND links.BlogId = @BlogId 
	AND categories.CategoryType = 5
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


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetCategory]
(
	@CategoryName nvarchar(150) = NULL
	, @CategoryID int = NULL
	, @IsActive bit
	, @BlogId int = NULL
	, @CategoryType tinyint = NULL
)
AS
SELECT	c.CategoryID
		, c.Title
		, c.Active
		, c.CategoryType
		, c.[Description]
		, c.BlogId
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories] c
WHERE (c.CategoryID = @CategoryID OR @CategoryID IS NULL)
	AND (c.Title = @CategoryName OR @CategoryName IS NULL)
	AND (c.CategoryType = @CategoryType OR @CategoryType IS NULL)
	AND (c.BlogId = @BlogId OR @BlogId IS NULL)
	AND c.Active <> CASE @IsActive WHEN 0 THEN -1 else 0 END
	

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
	, @CurrentDateTime datetime
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
-- Create a temp table to store the Post IDs 
-- of the posts we're interested in.
SET ROWCOUNT @ItemCount

INSERT #IDs (Id)  
SELECT [Id]   
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	PostType = @PostType 
	AND BlogId = COALESCE(@BlogId, BlogId)
	AND PostConfig & @PostConfig = @PostConfig
	AND (@PostConfig & 1 != 1 OR DateSyndicated <= @CurrentDateTime)
ORDER BY ISNULL([DateSyndicated], [DateAdded]) DESC

SET ROWCOUNT 0

-- Now select the content etc... for the posts 
-- in the temp table.
SELECT BlogId
	, [<dbUser,varchar,dbo>].[subtext_Content].[Id]
	, [<dbUser,varchar,dbo>].[subtext_Content].Title
	, DateCreated = DateAdded
	, [Text]
	, [Description]
	, PostType
	, Author
	, Email
	, DateUpdated
	, FeedbackCount = ISNULL(FeedbackCount, 0)
	, PostConfig
	, EntryName 
	, DateSyndicated
	, e.Id as EnclosureId
	, e.Title as EnclosureTitle
	, e.Url as EnclosureUrl
	, e.MimeType as EnclosureMimeType
	, e.Size as EnclosureSize
	, e.EnclosureEnabled as EnclosureEnabled
	, e.AddToFeed
	, e.ShowWithPost
FROM [<dbUser,varchar,dbo>].[subtext_Content]
	INNER JOIN #IDs ON #IDs.[Id] = [<dbUser,varchar,dbo>].[subtext_Content].[Id]
	LEFT JOIN [<dbUser,varchar,dbo>].[subtext_Enclosure] e on [<dbUser,varchar,dbo>].[subtext_Content].[ID] = e.EntryId
ORDER BY #IDs.TempId

IF @IncludeCategories = 1
BEGIN
	-- Select the category title and the associated post id
	SELECT	c.Title  
			, PostId = l.[PostId]
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

/*
Returns a single blog within the subtext_Config table by id.
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
		, blog.TimeZoneId
		, blog.CategoryListPostCount
		, blog.ItemCount
		, blog.[Language]
		, blog.News
		, blog.TrackingCode
		, blog.SecondaryCss
		, blog.LastUpdated
		, blog.PostCount
		, blog.StoryCount
		, blog.PingTrackCount
		, blog.CommentCount
		, blog.Flag
		, blog.SkinCssFile 
		, blog.BlogGroupId
		, blog.LicenseUrl
		, blog.DaysTillCommentsClose
		, blog.CommentDelayInMinutes
		, blog.NumberOfRecentComments
		, blog.RecentCommentsLength
		, blog.AkismetAPIKey
		, blog.FeedBurnerName
		, bgroup.Title AS BlogGroupTitle
		, blog.MobileSkin
		, blog.MobileSkinCssFile
		, blog.OpenIDUrl
		, blog.CardSpaceHash
		, blog.OpenIDServer
		, blog.OpenIDDelegate
FROM [<dbUser,varchar,dbo>].[subtext_Config] blog
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_BlogGroup] bgroup ON
bgroup.Id = blog.BlogGroupId
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
/*
Returns the blog that matches the given host/application combination.

@Strict -- If 0, then we return the one and only blog if there's one and only blog.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetConfig]
(
	@Host nvarchar(100)
	, @Application nvarchar(50) = ''
)
AS

DECLARE @BlogId int

SET @BlogId = (
	SELECT BlogId 
	FROM [<dbUser,varchar,dbo>].[subtext_Config] 
	WHERE Host = @Host 
		AND Application = @Application
)

IF @BlogId IS NULL
	BEGIN
		SET @BlogId = (
			SELECT BlogId 
			FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] 
			WHERE Host = @Host 
				AND Application = @Application
		)
	END

EXEC [<dbUser,varchar,dbo>].[subtext_GetBlogById] @BlogId = @BlogId

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
	@BlogId int,
	@CurrentDateTime datetime
)
AS
SELECT	c.BlogId
	, c.[ID]
	, c.Title
	, DateCreated = c.DateAdded
	, c.[Text]
	, c.[Description]
	, c.PostType
	, c.Author
	, c.Email
	, c.DateUpdated
	, FeedbackCount = ISNULL(c.FeedbackCount, 0)
	, c.PostConfig
	, c.EntryName 
	, c.DateSyndicated
	, e.Id as EnclosureId
	, e.Title as EnclosureTitle
	, e.Url as EnclosureUrl
	, e.MimeType as EnclosureMimeType
	, e.Size as EnclosureSize
	, e.EnclosureEnabled as EnclosureEnabled
	, e.AddToFeed
	, e.ShowWithPost
FROM [<dbUser,varchar,dbo>].[subtext_Content] c
	LEFT JOIN [<dbUser,varchar,dbo>].[subtext_Enclosure] e ON c.[ID] = e.EntryId
WHERE 
	(
		c.DateSyndicated > @StartDate 
		AND c.DateSyndicated < DateAdd(day, 1, @StopDate)
	)
	AND c.PostType = @PostType 
	AND c.BlogId = @BlogId 
	AND c.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
	AND c.DateSyndicated <= CASE @IsActive WHEN 1 THEN @CurrentDateTime ELSE c.DateSyndicated END
ORDER BY c.DateSyndicated DESC;


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

/* Gets all the ACTIVE Feedback (comments, pingbacks/trackbacks) for the entry */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetFeedbackCollection]
(
	@EntryId int
)
AS
	SELECT f.Id 
		, f.Title
		, f.Body
		, f.BlogId
		, f.EntryId
		, f.Author
		, f.IsBlogAuthor
		, f.Email
		, f.Url
		, f.FeedbackType
		, f.StatusFlag
		, f.CommentAPI
		, f.Referrer
		, f.IpAddress
		, f.UserAgent
		, f.FeedbackChecksumHash
		, f.DateCreated
		, f.DateModified
		, ParentEntryDateSyndicated = c.DateSyndicated
		, ParentEntryCreateDate = c.DateAdded
		, ParentEntryName = c.EntryName
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c 
		ON c.Id = f.EntryId
WHERE f.EntryId = @EntryId
	AND f.StatusFlag & 1 = 1
	AND f.StatusFlag & 8 != 8
ORDER BY f.[Id]


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetFeedbackCollection]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/* Returns a single Feedback by id */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetFeedback]
(
	@Id int
)
AS
	SELECT f.Id 
		, f.Title
		, f.Body
		, f.BlogId
		, f.EntryId
		, f.Author
		, f.IsBlogAuthor
		, f.Email
		, f.Url
		, f.FeedbackType
		, f.StatusFlag
		, f.CommentAPI
		, f.Referrer
		, f.IpAddress
		, f.UserAgent
		, f.FeedbackChecksumHash
		, f.DateCreated
		, f.DateModified
		, ParentEntryDateSyndicated = c.DateSyndicated
		, ParentEntryCreateDate = c.DateAdded
		, ParentEntryName = c.EntryName
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c 
		ON c.Id = f.EntryId
WHERE f.Id = @Id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetFeedback] TO [public]
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
EXEC [<dbUser,varchar,dbo>].[subtext_GetCategory] @CategoryID=@CategoryID, @IsActive=@IsActive, @BlogId=@BlogId


SELECT	Title
		, CategoryID
		, Height
		, Width
		, FileName = [File]
		, IsActive = Active
		, ImageID 
		, BlogId
		, Url
FROM [<dbUser,varchar,dbo>].[subtext_Images]  
WHERE CategoryID = @CategoryID 
	AND BlogId = @BlogId 
	AND Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY Title, ImageID


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
	Id = KeyWordID, 
	Word,
	Rel,
	[Text],
	ReplaceFirstTimeOnly,
	OpenInNewWindow, 
	CaseSensitive,
	Url,
	Title,
	BlogId
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
	@PostID int = NULL,
	@BlogId int
)
AS

IF @PostID = -1
	SET @PostID = NULL

SELECT	Id = LinkID
	, Title
	, Url
	, Rss
	, IsActive = Active
	, CategoryID
	, PostID
	, NewWindow 
	, Relation = Rel
	, BlogId
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
SELECT	Id = LinkID
		, Title
		, Url
		, Rss
		, IsActive = Active
		, NewWindow
		, CategoryID
		, Relation = Rel
		, PostId
		, BlogId
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


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetEntries]
(
	@BlogId int
	, @CategoryID int = NULL
	, @PageIndex int
	, @PostType int
	, @PageSize int
)
AS

IF(@CategoryID IS NULL)
BEGIN
WITH OrderedEntries AS
(
	SELECT 	content.BlogId 
			, content.[ID] 
			, content.Title 
			, DateCreated = content.DateAdded 
			, content.[Text] 
			, content.[Description]
			, content.PostType 
			, content.Author 
			, content.Email 
			, content.DateUpdated 
			, FeedbackCount = ISNULL(content.FeedbackCount, 0)
			, content.PostConfig
			, content.EntryName
			, content.DateSyndicated
			, WebCount = ISNULL(vc.WebCount, 0)
			, AggCount = ISNULL(vc.AggCount, 0)
			, vc.WebLastUpdated
			, vc.AggLastUpdated
			, row_number() over(order by content.DateAdded DESC, content.ID DESC) RowNumber
			, e.Id as EnclosureId
			, e.Title as EnclosureTitle
			, e.Url as EnclosureUrl
			, e.MimeType as EnclosureMimeType
			, e.Size as EnclosureSize
			, e.EnclosureEnabled as EnclosureEnabled
			, e.AddToFeed
			, e.ShowWithPost
	FROM [dbo].[subtext_Content] content
		LEFT JOIN [dbo].subtext_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogId = @BlogId)
		LEFT JOIN [dbo].[subtext_Enclosure] e ON content.[ID] = e.EntryId
	WHERE 	content.BlogId = @BlogId
		AND content.PostType = @PostType
)

SELECT * 
FROM OrderedEntries 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize
END
ELSE
BEGIN
WITH OrderedEntries AS
(
	SELECT 	content.BlogId 
			, content.[ID] 
			, content.Title 
			, DateCreated = content.DateAdded 
			, content.[Text] 
			, content.[Description]
			, content.PostType 
			, content.Author 
			, content.Email 
			, content.DateUpdated 
			, FeedbackCount = ISNULL(content.FeedbackCount, 0)
			, content.PostConfig
			, content.EntryName
			, content.DateSyndicated
			, WebCount = ISNULL(vc.WebCount, 0)
			, AggCount = ISNULL(vc.AggCount, 0)
			, vc.WebLastUpdated
			, vc.AggLastUpdated
			, row_number() over(order by content.DateAdded DESC, content.ID DESC) RowNumber
			, e.Id as EnclosureId
			, e.Title as EnclosureTitle
			, e.Url as EnclosureUrl
			, e.MimeType as EnclosureMimeType
			, e.Size as EnclosureSize
			, e.EnclosureEnabled as EnclosureEnabled
			, e.AddToFeed
			, e.ShowWithPost
	FROM [dbo].[subtext_Content] content
		LEFT JOIN [dbo].[subtext_Links] l ON content.[ID] = l.PostID
		LEFT JOIN [dbo].subtext_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogId = @BlogId)
		LEFT JOIN [dbo].[subtext_Enclosure] e ON content.[ID] = e.EntryId
	WHERE 	content.BlogId = @BlogId
		AND content.PostType = @PostType
		AND (l.CategoryID = @CategoryID)
)

SELECT * 
FROM OrderedEntries 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize

END

IF(@CategoryID IS NULL)
	SELECT COUNT([ID]) AS TotalRecords
	FROM [<dbUser,varchar,dbo>].[subtext_Content] 
	WHERE 	BlogId = @BlogId 
		AND PostType = @PostType 
ELSE
	SELECT COUNT(content.[ID]) AS TotalRecords
	FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] links ON content.[ID] = links.PostID
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] cats ON (links.CategoryID = cats.CategoryID)
	WHERE 	content.BlogId = @BlogId 
		AND content.PostType = @PostType 
		AND cats.CategoryID = @CategoryID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
For the admin section. Gets a page of Feedback for the specified blog.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableFeedback]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
	, @StatusFlag int
	, @ExcludeFeedbackStatusMask int = NULL
	, @FeedbackType int = NULL -- Null for all feedback.
)
AS

IF @ExcludeFeedbackStatusMask IS NULL
	SET @ExcludeFeedbackStatusMask = 0;

WITH OrderedFeedbacks AS
(
SELECT  f.Id
		, f.Title
		, f.Body
		, f.BlogId
		, f.EntryId
		, f.Author
		, f.IsBlogAuthor
		, f.Email
		, f.Url
		, f.FeedbackType
		, f.StatusFlag
		, f.CommentAPI
		, f.Referrer
		, f.IpAddress
		, f.UserAgent
		, f.FeedbackChecksumHash
		, f.DateCreated
		, f.DateModified
		, ParentEntryDateSyndicated = c.DateSyndicated
		, ParentEntryCreateDate = c.DateAdded
		, ParentEntryName = c.EntryName
		, row_number() over(order by DateCreated DESC, f.Id DESC) RowNumber
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c 
		ON c.Id = f.EntryId
WHERE 	f.BlogId = @BlogId 
	AND f.StatusFlag & @StatusFlag = @StatusFlag
	AND (@StatusFlag = 8 OR f.StatusFlag & 8 != 8)
	AND (f.StatusFlag & @ExcludeFeedbackStatusMask = 0) -- Make sure the status doesn't have any of the excluded statuses set
	AND (f.FeedbackType = @FeedbackType OR @FeedbackType IS NULL)
)

SELECT * 
FROM OrderedFeedbacks 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize

 
SELECT COUNT(f.[Id]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
WHERE 	f.BlogId = @BlogId 
	AND f.StatusFlag & @StatusFlag = @StatusFlag
	AND (@StatusFlag = 8 OR f.StatusFlag & 8 != 8)
	AND (f.StatusFlag & @ExcludeFeedbackStatusMask = 0) -- Make sure the status doesn't have any of the excluded statuses set
	AND (f.FeedbackType = @FeedbackType OR @FeedbackType IS NULL)
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


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableLogEntries]
(
	@BlogId int = NULL
	, @PageIndex int
	, @PageSize int
)
AS

WITH OrderedLogs as
( 
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
		, row_number() over(order by [log].[ID] DESC) RowNumber
FROM [<dbUser,varchar,dbo>].[subtext_Log] [log]
WHERE 	([log].BlogId = @BlogId or @BlogId IS NULL)
)

SELECT * 
FROM OrderedLogs 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize


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

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableKeyWords]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
)
AS

WITH OrderedKeyWords as
( 
SELECT 	Id = words.KeyWordID
		, words.Word
		, words.Rel
		, words.[Text]
		, words.ReplaceFirstTimeOnly
		, words.OpenInNewWindow
		, words.CaseSensitive
		, words.Url
		, words.Title
		, words.BlogId
		, row_number() over(order by words.Word ASC) RowNumber
FROM 	
	[<dbUser,varchar,dbo>].[subtext_KeyWords] words
WHERE 	
		words.BlogId = @BlogId 
)

SELECT * 
FROM OrderedKeyWords 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize

 
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

WITH OrderedLinks as
( 
SELECT Id = LinkID 
	, Title 
	, Url
	, Rss 
	, IsActive = Active 
	, NewWindow 
	, CategoryID
	, Relation = Rel
	, PostID
	, BlogId
	, row_number() over(order by [LinkID] DESC) RowNumber
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE 	BlogId = @BlogId 
	AND (CategoryID = @CategoryId OR @CategoryId IS NULL)
	AND PostID IS NULL
)

SELECT * 
FROM OrderedLinks 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize


SELECT COUNT([LinkID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Links] 
WHERE 	BlogId = @BlogId 
	AND (CategoryID = @CategoryId OR @CategoryId IS NULL)
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

SELECT 	Id = links.LinkID
		, links.Title
		, links.Url
		, links.Rss 
		, IsActive = links.Active 
		, links.NewWindow 
		, links.CategoryID  
		, Relation = links.Rel
		, PostId
		, links.BlogId
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

WITH OrderedReferrals AS
(
SELECT	
	ReferrerURL = u.URL
	, PostTitle = c.Title
	, c.EntryName
	, r.[EntryId]
	, [Count]
	, LastReferDate = r.LastUpdated
	, BlogId = @BlogId
	, row_number() over(order by [Count] DESC, r.[EntryID] DESC, r.[UrlID] DESC) RowNumber
FROM [<dbUser,varchar,dbo>].[subtext_Referrals] r
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_URLs] u ON u.UrlID = r.UrlID
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c ON c.ID = r.EntryID
WHERE 
	r.EntryID = @EntryID OR @EntryID IS NULL
	AND r.BlogId = @BlogId
)

SELECT * 
FROM OrderedReferrals 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize

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
	, @CurrentDateTime datetime
)
AS
SET ROWCOUNT @ItemCount
SELECT 
	content.BlogId
	, content.[ID]
	, content.Title
	, DateCreated = content.DateAdded
	, content.[Text]
	, content.[Description]
	, content.PostType
	, content.Author
	, content.Email
	, content.DateUpdated
	, FeedbackCount = ISNULL(content.FeedbackCount, 0)
	, content.PostConfig
	, content.EntryName 
	, content.DateSyndicated
	, e.Id as EnclosureId
	, e.Title as EnclosureTitle
	, e.Url as EnclosureUrl
	, e.MimeType as EnclosureMimeType
	, e.Size as EnclosureSize
	, e.EnclosureEnabled as EnclosureEnabled
	, e.AddToFeed
	, e.ShowWithPost
FROM [<dbUser,varchar,dbo>].[subtext_Content] content WITH (NOLOCK)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] links WITH (NOLOCK) ON content.ID = links.PostID
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] categories WITH (NOLOCK) ON links.CategoryID = categories.CategoryID
	left join [<dbUser,varchar,dbo>].[subtext_Enclosure] e on content.[ID] = e.EntryId
WHERE  content.BlogId = @BlogId 
	AND DateSyndicated <= @CurrentDateTime
	AND content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END AND categories.CategoryID = @CategoryID
ORDER BY content.DateSyndicated DESC


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
		, DateCreated = DateAdded
		, [Text]
		, [Description]
		, PostType
		, Author
		, Email
		, DateUpdated
		, FeedbackCount = ISNULL(FeedbackCount, 0)
		, PostConfig
		, EntryName 
		, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE 
	(
			DateSyndicated > @StartDate 
		AND DateSyndicated < DateAdd(day,1,@StopDate)
	)
	AND PostType=1 
	AND BlogId = @BlogId
ORDER BY DateSyndicated DESC;


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
	, @CurrentDateTime datetime
)
AS
SELECT	BlogId
	, [<dbUser,varchar,dbo>].[subtext_Content].[ID]
	, [<dbUser,varchar,dbo>].[subtext_Content].Title
	, DateCreated = DateAdded
	, [Text]
	, [Description]
	, PostType
	, Author
	, Email
	, DateUpdated
	, FeedbackCount = ISNULL(FeedbackCount, 0)
	, PostConfig
	, EntryName 
	, DateSyndicated
	, e.Id as EnclosureId
	, e.Title as EnclosureTitle
	, e.Url as EnclosureUrl
	, e.MimeType as EnclosureMimeType
	, e.Size as EnclosureSize
	, e.EnclosureEnabled as EnclosureEnabled
	, e.AddToFeed
	, e.ShowWithPost
FROM [<dbUser,varchar,dbo>].[subtext_Content]
	left join [<dbUser,varchar,dbo>].[subtext_Enclosure] e on [<dbUser,varchar,dbo>].[subtext_Content].[ID] = e.EntryId
WHERE	PostType=1 
	AND (BlogId = @BlogId OR @BlogId IS NULL)
	AND PostConfig & 1 = 1 
	AND DateSyndicated <= @CurrentDateTime
	AND Month(DateSyndicated) = @Month 
	AND Year(DateSyndicated)  = @Year
ORDER BY DateSyndicated DESC


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
	@BlogId int = NULL,
	@CurrentDateTime datetime
)
AS
SELECT [Month] = Month(DateSyndicated)
	, [Year] = Year(DateSyndicated)
	, [Day] = 1
	, [Count] = Count(*) 
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE PostType = 1 
	AND PostConfig & 1 = 1 
	AND DateSyndicated <= @CurrentDateTime
	AND (BlogId = @BlogId OR @BlogId IS NULL)
	AND NOT DateSyndicated IS NULL
GROUP BY Year(DateSyndicated), Month(DateSyndicated) 
ORDER BY [Year] DESC, [Month] DESC



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
	@BlogId int,
	@CurrentDateTime datetime
)
AS
SELECT [Month] = 1
	, [Year] = Year(DateSyndicated)
	, [Day] = 1
	, [Count] = Count(*)
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE PostType = 1 
	AND PostConfig & 1 = 1 
	AND DateSyndicated <= @CurrentDateTime
	AND BlogId = @BlogId 
GROUP BY Year(DateSyndicated) ORDER BY [Year] DESC

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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleEntry]
(
	@ID int = NULL
	, @EntryName nvarchar(150) = NULL
	, @IsActive bit
	, @BlogId int = NULL
	, @IncludeCategories bit = 0
)
AS
SELECT	BlogId
	, [<dbUser,varchar,dbo>].[subtext_Content].[ID]
	, [<dbUser,varchar,dbo>].[subtext_Content].Title
	, DateCreated = DateAdded
	, [Text]
	, [Description]
	, PostType
	, Author
	, Email
	, DateUpdated
	, FeedbackCount = ISNULL(FeedbackCount, 0)
	, PostConfig
	, EntryName 
	, DateSyndicated
	, e.Id as EnclosureId
	, e.Title as EnclosureTitle
	, e.Url as EnclosureUrl
	, e.MimeType as EnclosureMimeType
	, e.Size as EnclosureSize
	, e.EnclosureEnabled as EnclosureEnabled
	, e.AddToFeed
	, e.ShowWithPost
FROM [<dbUser,varchar,dbo>].[subtext_Content]  
	LEFT JOIN [<dbUser,varchar,dbo>].[subtext_Enclosure] e ON [<dbUser,varchar,dbo>].[subtext_Content].[ID] = e.EntryId
WHERE [<dbUser,varchar,dbo>].[subtext_Content].ID = COALESCE(@ID, [<dbUser,varchar,dbo>].[subtext_Content].ID)
	AND (EntryName = @EntryName OR @EntryName IS NULL) 
	AND (BlogId = @BlogId OR  @BlogId IS NULL)
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [<dbUser,varchar,dbo>].[subtext_Content].[ID] DESC

IF @IncludeCategories = 1
BEGIN
	SELECT c.Title
		, PostId = l.PostID  
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
	, FileName = [File]
	, IsActive = Active
	, ImageID
	, BlogId
	, Url
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
SELECT	Id = subtext_Links.LinkID
		, subtext_Links.Title
		, subtext_Links.Url
		, subtext_Links.Rss
		, IsActive = subtext_Links.Active
		, subtext_Links.NewWindow
		, subtext_Links.CategoryID
		, Relation = subtext_Links.Rel
		, subtext_Links.BlogId
		, PostId =  subtext_Links.PostID
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
	, @UrlID int OUTPUT
)
AS
IF EXISTS(SELECT UrlID FROM [<dbUser,varchar,dbo>].[subtext_Urls] WHERE Url = @Url AND Url != '')
BEGIN
	SELECT @UrlID = UrlID FROM [<dbUser,varchar,dbo>].[subtext_Urls] WHERE Url = @Url
END
Else
BEGIN
	IF(@Url != '' AND NOT @Url IS NULL)
		INSERT INTO subtext_Urls (Url) VALUES (@Url)
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
	, @CategoryID int OUTPUT
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
	--Do we have an existing entry with this ID?
	IF EXISTS(SELECT ID FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE ID = @EntryID AND BlogId = @BlogId)
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
				SET [AggCount] = [AggCount] + 1, AggLastUpdated = getdate()
				WHERE EntryID = @EntryID AND BlogId = @BlogId
			END
		END
		else
		BEGIN
			if(@IsWeb = 1) -- Is this a web view
			BEGIN
				INSERT subtext_EntryViewCount (EntryID, BlogId, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
				   VALUES (@EntryID, @BlogId, 1, 0, getdate(), NULL)
			END
			else
			BEGIN
				INSERT subtext_EntryViewCount (EntryID, BlogId, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
				   VALUES (@EntryID, @BlogId, 0, 1, NULL, getdate())
			END
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
	@Url nvarchar(512) = NULL,
	@ImageID int OUTPUT
)
AS
INSERT subtext_Images
(
	Title, CategoryID, Width, Height, [File], Active, BlogId, Url
)
Values
(
	@Title, @CategoryID, @Width, @Height, @File, @Active, @BlogId, @Url
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
	@Rel nvarchar(100) = NULL,
	@Text nvarchar(100),
	@ReplaceFirstTimeOnly bit,
	@OpenInNewWindow bit,
	@CaseSensitive bit,
	@Url nvarchar(255),
	@Title nvarchar(100),
	@BlogId int,
	@KeyWordID int OUTPUT
)

AS

INSERT [<dbUser,varchar,dbo>].[subtext_KeyWords]
	(Word,Rel,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogId)
Values
	(@Word,@Rel,@Text,@ReplaceFirstTimeOnly,@OpenInNewWindow, @CaseSensitive,@Url,@Title,@BlogId)

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
	@Url nvarchar(255) = NULL,
	@Rss nvarchar(255),
	@Active bit,
	@NewWindow bit,
	@CategoryID int,
	@PostID int = NULL,
	@BlogId int,
	@Rel nvarchar(150) = NULL,
	@LinkID int OUTPUT
)
AS

IF @PostID < 0
	SET @PostID = NULL

INSERT INTO subtext_Links 
( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogId, Rel )
VALUES 
(@Title, @Url, @Rss, @Active, @NewWindow, @PostID, @CategoryID, @BlogId,@Rel);
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

IF @CategoryList = ''
BEGIN
	DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links]
	WHERE 
		BlogId = @BlogId AND (PostID = @PostID)
END
ELSE
BEGIN

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
END

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
	EXEC [<dbUser,varchar,dbo>].[subtext_GetUrlID] @Url, @UrlID = @UrlID OUTPUT
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
	EXEC [<dbUser,varchar,dbo>].[subtext_GetUrlID] @Url, @UrlID = @UrlID OUTPUT
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
	INSERT subtext_ViewStats (BlogId, PageType, PostID, [Day], UrlID, [Count])
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
	@Flag int,
	@BlogGroupId int,
	@Id int OUTPUT
)

AS

IF NOT EXISTS(SELECT * FROM [<dbUser,varchar,dbo>].[subtext_Config] WHERE Host = @Host AND Application = @Application)
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
	, TimeZoneId
	, [Language]
	, ItemCount
	, Flag
	, BlogGroupId
	, MobileSkin
	, MobileSkinCssFile
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
	, @BlogGroupId
	, 'Naked'
	, ''
)

SELECT @Id = SCOPE_IDENTITY()
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
	, @TimeZoneId nvarchar(128)
	, @ItemCount int
	, @CategoryListPostCount int
	, @News nText = NULL
	, @TrackingCode nText = NULL
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
	, @AkismetAPIKey varchar(16) = NULL
	, @FeedBurnerName nvarchar(64) = NULL
	, @BlogGroupId int
	, @MobileSkin nvarchar(50) = NULL
	, @MobileSkinCssFile nvarchar(100) = NULL
	, @OpenIDUrl varchar(255) = NULL
	, @CardSpaceHash nvarchar(512) = NULL
	, @OpenIDServer varchar(255) = NULL
	, @OpenIDDelegate varchar(255) = NULL
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
	, TimeZoneId   = @TimeZoneId
	, ItemCount = @ItemCount
	, CategoryListPostCount = @CategoryListPostCount
	, News      = @News
	, TrackingCode      = @TrackingCode
	, LastUpdated = @LastUpdated
	, Flag = @Flag
	, SecondaryCss = @SecondaryCss
	, SkinCssFile = @SkinCssFile
	, LicenseUrl = @LicenseUrl
	, DaysTillCommentsClose = @DaysTillCommentsClose
	, CommentDelayInMinutes = @CommentDelayInMinutes
	, NumberOfRecentComments = @NumberOfRecentComments
	, RecentCommentsLength = @RecentCommentsLength
	, AkismetAPIKey = @AkismetAPIKey
	, FeedBurnerName = @FeedBurnerName
	, BlogGroupId =  @BlogGroupId
	, MobileSkin = @MobileSkin
	, MobileSkinCssFile = @MobileSkinCssFile
	, OpenIDUrl = @OpenIDUrl
	, CardSpaceHash = @CardSpaceHash
	, OpenIDServer = @OpenIDServer
	, OpenIDDelegate = @OpenIDDelegate
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
	, @Text ntext = NULL
	, @PostType int
	, @Author nvarchar(50) = NULL
	, @Email nvarchar(50) = NULL
	, @Description nvarchar(500) = NULL
	, @DateUpdated datetime = NULL
	, @PostConfig int
	, @EntryName nvarchar(150) = NULL
	, @DateSyndicated DateTime = NULL
	, @BlogId int
	, @CurrentDateTime datetime
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
	, [Text] = @Text 
	, PostType = @PostType
	, Author = @Author 
	, Email = @Email 
	, [Description] = @Description
	, DateUpdated = @DateUpdated
	, PostConfig = @PostConfig
	, EntryName = @EntryName
	, DateSyndicated = @DateSyndicated
WHERE 	
		[ID] = @ID 
	AND BlogId = @BlogId
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime] @BlogId, @DateUpdated
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] @BlogId, @ID, @CurrentDateTime
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] @BlogId, @CurrentDateTime

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
	@ImageID int,
	@Url nvarchar(512) = null
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Images]
Set
	Title = @Title,
	CategoryID = @CategoryID,
	Width = @Width,
	Height = @Height,
	[File] = @File,
	Active = @Active,
	Url = @Url
	
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
	@Rel nvarchar(100),
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
		Rel = @Rel,
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
	@Url nvarchar(255) = NULL,
	@Rss nvarchar(255),
	@Active bit,
	@NewWindow bit,
	@CategoryID int,
	@Rel nvarchar(150) = NULL,
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
	CategoryID = @CategoryID,
	Rel = @Rel
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
Returns a page of blogs within subtext_Config table
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]
(
	@PageIndex int
	, @PageSize int
	, @Host nvarchar(100) = NULL
	, @ConfigurationFlags int
)
AS


WITH OrderedBlogs as
(
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
		, blog.TimeZoneId
		, blog.ItemCount
		, blog.CategoryListPostCount
		, blog.[Language]
		, blog.News
		, blog.TrackingCode
		, blog.SecondaryCss
		, blog.LastUpdated
		, blog.PostCount
		, blog.StoryCount
		, blog.PingTrackCount
		, blog.CommentCount
		, blog.Flag
		, blog.SkinCssFile 
		, blog.BlogGroupId
		, blog.LicenseUrl
		, blog.DaysTillCommentsClose
		, blog.CommentDelayInMinutes
		, blog.NumberOfRecentComments
		, blog.RecentCommentsLength
		, blog.AkismetAPIKey
		, blog.FeedBurnerName
		, bgroup.Title AS BlogGroupTitle
		, blog.MobileSkin
		, blog.MobileSkinCssFile
		, blog.OpenIDUrl
		, blog.OpenIDServer
		, blog.OpenIDDelegate
		, blog.CardSpaceHash
		, row_number() over(order by blog.BlogId ASC) RowNumber
FROM [<dbUser,varchar,dbo>].[subtext_Config] blog
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_BlogGroup] bgroup ON
bgroup.Id = blog.BlogGroupId
WHERE 
	@ConfigurationFlags & Flag = @ConfigurationFlags
	AND (Host = @Host OR @Host IS NULL)
)

SELECT * 
FROM OrderedBlogs 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize



SELECT COUNT([BlogId]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Config]
WHERE @ConfigurationFlags & Flag = @ConfigurationFlags
	AND (Host = @Host OR @Host IS NULL)
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
SET ANSI_WARNINGS OFF
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertFeedback]
(
	@Title nvarchar(256)
	, @Body ntext = NULL
	, @BlogId int
	, @EntryId int = NULL
	, @Author nvarchar(64) = NULL
	, @IsBlogAuthor bit = 0
	, @Email varchar(128) = NULL
	, @Url varchar(256) = NULL
	, @FeedbackType int
	, @StatusFlag int
	, @CommentAPI bit
	, @Referrer varchar(256) = NULL
	, @IpAddress varchar(16) = NULL
	, @UserAgent nvarchar(128) = NULL
	, @FeedbackChecksumHash varchar(32)
	, @DateCreated datetime
	, @DateModified datetime = NULL
	, @CurrentDateTime datetime
	, @Id int OUTPUT
)
AS

IF @DateModified IS NULL
    SET @DateModified = getdate()
    
INSERT INTO [<dbUser,varchar,dbo>].[subtext_FeedBack]
( 
	Title
	, Body
	, BlogId
	, EntryId
	, Author
	, IsBlogAuthor
	, Email
	, Url
	, FeedbackType
	, StatusFlag
	, CommentAPI
	, Referrer
	, IpAddress
	, UserAgent
	, FeedbackChecksumHash
	, DateCreated
	, DateModified
)
VALUES 
(
	@Title
	, @Body
	, @BlogId
	, @EntryId
	, @Author
	, @IsBlogAuthor
	, @Email
	, @Url
	, @FeedbackType
	, @StatusFlag
	, @CommentAPI
	, @Referrer
	, @IpAddress
	, @UserAgent
	, @FeedbackChecksumHash
	, @DateCreated
	, @DateModified
)

SELECT @Id = SCOPE_IDENTITY()

exec [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] @BlogId, @EntryId, @CurrentDateTime


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertFeedback]  TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateFeedback]
(
	@ID int
	, @Title nvarchar(256)
	, @Body ntext = NULL
	, @Author nvarchar(64) = NULL
	, @Email varchar(128) = NULL
	, @Url varchar(256) = NULL
	, @StatusFlag int
	, @FeedbackChecksumHash varchar(32)
	, @DateModified datetime
	, @CurrentDateTime datetime
)
AS

DECLARE @EntryId int
DECLARE @BlogId int
SELECT @EntryId = EntryId, @BlogId = BlogId FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE Id = @Id

UPDATE [<dbUser,varchar,dbo>].[subtext_FeedBack]
SET	Title = @Title
	, Body = @Body
	, Author = @Author
	, Email = @Email
	, Url = @Url
	, StatusFlag = @StatusFlag
	, FeedbackChecksumHash = @FeedbackChecksumHash
	, DateModified = @DateModified
WHERE Id = @Id

exec [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] @BlogId, @EntryId, @CurrentDateTime

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateFeedback]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertEntry]
(
	@Title nvarchar(255)
	, @Text ntext = NULL
	, @PostType int
	, @Author nvarchar(50) = NULL
	, @Email nvarchar(50) = NULL
	, @Description nvarchar(500) = NULL
	, @BlogId int
	, @DateCreated datetime
	, @PostConfig int
	, @EntryName nvarchar(150) = NULL
	, @DateSyndicated DateTime = NULL
	, @CurrentDateTime datetime
	, @ID int OUTPUT
	
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
	, [Text]
	, PostType
	, Author
	, Email
	, DateAdded
	, DateUpdated
	, [Description]
	, PostConfig
	, FeedbackCount
	, BlogId
	, EntryName 
	, DateSyndicated
)
VALUES 
(
	@Title
	, @Text
	, @PostType
	, @Author
	, @Email
	, @DateCreated
	, @DateCreated
	, @Description
	, @PostConfig
	, 0 -- Feedback Count
	, @BlogId
	, @EntryName
	, @DateSyndicated
)
SELECT @ID = SCOPE_IDENTITY()

EXEC [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime] @BlogId, @DateCreated
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] @BlogId, @CurrentDateTime


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
FeedbackChecksumHash.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetCommentByChecksumHash]
(
	@FeedbackChecksumHash varchar(32)
	, @BlogId int
)
AS
SELECT TOP 1 f.Title
		, f.Body
		, f.BlogId
		, f.EntryId
		, f.Author
		, f.IsBlogAuthor
		, f.Email
		, f.Url
		, f.FeedbackType
		, f.StatusFlag
		, f.CommentAPI
		, f.Referrer
		, f.IpAddress
		, f.UserAgent
		, f.FeedbackChecksumHash
		, f.DateCreated
		, f.DateModified
		, ParentEntryDateSyndicated = c.DateSyndicated
		, ParentEntryCreateDate = c.DateAdded
		, ParentEntryName = c.EntryName
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c ON f.EntryId = c.ID
WHERE 
	f.FeedbackChecksumHash = @FeedbackChecksumHash
	AND f.BlogId = @BlogId
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

DECLARE @blogAggregationEnabled bit
SET @blogAggregationEnabled = 0

SELECT 
	[HostUserName]
	, [Email]
	, [Password]
	, [Salt]
	, [DateCreated]
	, [BlogAggregationEnabled] = @blogAggregationEnabled /* Will put this column in DB */
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
	, @Email NVARCHAR(256)
	, @Password NVARCHAR(32)
	, @Salt NVARCHAR(32)
AS
IF EXISTS(SELECT * FROM [<dbUser,varchar,dbo>].[subtext_Host])
BEGIN
	UPDATE [<dbUser,varchar,dbo>].[subtext_Host] 
		SET [HostUserName] = @HostUserName
			, [Email] = @Email
			, [Password] = @Password
			, [Salt] = @Salt
END
ELSE
BEGIN
	INSERT subtext_Host
	(
		[HostUserName]
		,[Email]
		,[Password]
		,[Salt]
		,[DateCreated]
	)
	VALUES
	(
		@HostUserName
		,@Email
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
	, @GroupID int = NULL
	, @CurrentDateTime datetime
	, @RowCount int = 10

AS
SET ROWCOUNT @RowCount
SELECT
	Host
	, Application
	, [EntryName] = IsNull(content.EntryName, content.[ID])
	, content.[ID]
	, content.Title
	, DateCreated = content.DateAdded
	, content.DateSyndicated
	, content.PostType
	, content.Author
	, content.Email
	, content.FeedbackCount
	, content.EntryName
	, [IsXHTML] = Convert(bit,CASE WHEN content.PostConfig & 2 = 2 THEN 1 else 0 END) 
	, [BlogTitle] = content.Title
	, content.PostConfig
	, config.TimeZoneId
	, [Description] = IsNull(CASE WHEN PostConfig & 32 = 32 THEN content.[Description] else content.[Text] END, '')
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
INNER JOIN	[<dbUser,varchar,dbo>].[subtext_Config] config ON content.BlogId = config.BlogId
WHERE  content.PostType = 1 
	AND content.PostConfig & 1 = 1 
	AND DateSyndicated <= @CurrentDateTime
	AND content.PostConfig & 64 = 64 
	AND config.Flag & 2 = 2 
	AND config.Host = @Host
	AND (BlogGroupId = @GroupID or @GroupID is NULL)
ORDER BY DateSyndicated DESC


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
	@GroupID int = NULL
)
AS
SELECT blog.BlogId
	, blog.Author
	, blog.Application
	, blog.Host
	, blog.Title
	, blog.PostCount
	, blog.CommentCount
	, blog.StoryCount
	, blog.PingTrackCount
	, blog.LastUpdated
	, blog.BlogGroupId
	, bgroup.Title AS BlogGroupTitle
FROM [<dbUser,varchar,dbo>].[subtext_Config] blog
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_BlogGroup] bgroup ON
bgroup.Id = blog.BlogGroupId
WHERE blog.Flag & 2 = 2 
	AND blog.Flag & 1 = 1
	AND blog.Host = @Host
	AND bgroup.Active = 1
	AND blog.IsActive = 1
	AND (blog.BlogGroupId = @GroupID OR @GroupID is null)
ORDER BY bgroup.DisplayOrder, bgroup.Id,  blog.PostCount DESC


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
	@GroupID int = NULL
)
AS
SELECT Count(*) AS [BlogCount]
	, Sum(PostCount) AS PostCount
	, Sum(CommentCount) AS CommentCount
	, Sum(StoryCount) AS StoryCount
	, Sum(PingTrackCount) AS PingTrackCount 
FROM [<dbUser,varchar,dbo>].[subtext_Config] 
	WHERE subtext_Config.Flag & 2 = 2 
		AND IsActive = 1
		AND Host = @Host 
		AND (BlogGroupId = @GroupID OR @GroupID is NULL)

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

INSERT INTO [<dbUser,varchar,dbo>].[subtext_Version] (Major, Minor, Build, DateCreated)  
VALUES (@Major, @Minor, @Build, @DateCreated)

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
	 , @Message nvarchar(255)
	 , @Exception nvarchar(2745)
	 , @Url varchar(255)
)
AS

if @BlogId < 0
	SET @BlogId = NULL

INSERT [<dbUser,varchar,dbo>].[subtext_Log] 
(
	BlogId, 
	[Date], 
	Thread, 
	Context, 
	[Level], 
	Logger, 
	Message, 
	Exception, 
	Url
)
VALUES 
(
	@BlogId, 
	@Date, 
	@Thread, 
	@Context, 
	@Level, 
	@Logger, 
	@Message, 
	@Exception, 
	@Url
)

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

CREATE PROC [<dbUser,varchar,dbo>].[subtext_SearchEntries]
(
	@BlogId int
	,@SearchStr nvarchar(30)
	,@CurrentDateTime datetime
)
as

Set @SearchStr = '%' + @SearchStr + '%'

Select [ID]
	, Title
	, DateCreated = DateAdded
	, EntryName
	, PostType
From [<dbUser,varchar,dbo>].[subtext_Content]
Where (PostType = 1 OR PostType = 2)
	AND PostConfig & 1 = 1 -- IsActive!
	AND DateSyndicated <= @CurrentDateTime
	AND ([Text] LIKE @SearchStr OR Title LIKE @SearchStr)
	AND BlogId = @BlogId
ORDER by DateSyndicated DESC
	
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_SearchEntries]  TO [public]
GO

/*Previous Next*/
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntry_PreviousNext]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntry_PreviousNext]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetEntry_PreviousNext]
(
	@ID int
	, @PostType int
	, @BlogId int
	,@CurrentDateTime datetime
)
AS

DECLARE @DateSyndicated DateTime
SELECT @DateSyndicated = ISNULL(DateSyndicated, DateAdded) 
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE ID = @ID

SELECT * FROM
(
	SELECT Top 1 [ID]
		, Title
		, DateSyndicated = ISNULL(DateSyndicated, DateAdded) /* usage optimization to fulfill interface */
		, PostType
		, EntryName 
		, ViewCount = 0 /* Not Used */
	FROM [<dbUser,varchar,dbo>].[subtext_Content]
	WHERE ISNULL([DateSyndicated], [DateAdded]) >= @DateSyndicated
		AND subtext_Content.BlogId = @BlogId 
		AND subtext_Content.PostConfig & 1 = 1 
		AND subtext_Content.DateSyndicated <= @CurrentDateTime
		AND PostType = @PostType
		AND [ID] != @ID
	ORDER BY DateSyndicated ASC
) [Previous]
UNION
SELECT * FROM
(
	SELECT Top 1 [ID]
		, Title
		, DateSyndicated = ISNULL(DateSyndicated, DateAdded) /* usage optimization to fulfill interface */
		, PostType
		, EntryName 
		, ViewCount = 0 /* Not Used */
	FROM [<dbUser,varchar,dbo>].[subtext_Content]
	WHERE ISNULL([DateSyndicated], [DateAdded]) <= @DateSyndicated
		AND subtext_Content.BlogId = @BlogId 
		AND subtext_Content.PostConfig & 1 = 1 
		AND subtext_Content.DateSyndicated <= @CurrentDateTime
		AND PostType = @PostType
		AND [ID] != @ID
	ORDER BY DateSyndicated DESC
) [Next]

ORDER BY DateSyndicated DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetEntry_PreviousNext]  TO [public]
GO


/*Get Related Links (called from RelatedLinks.ascx) - GY*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetRelatedEntries]
	@BlogId int,  
	@EntryID int,
	@RowCount int
AS  
SET ROWCOUNT @RowCount
SELECT DISTINCT c.ID
	, c.Title
	, c.EntryName
	, ViewCount = 0 /* not needed here */
	, DateSyndicated = c.DateAdded
FROM [<dbUser,varchar,dbo>].subtext_LinkCategories lc
	INNER JOIN [<dbUser,varchar,dbo>].subtext_Links l ON l.CategoryID = lc.CategoryID
	INNER JOIN [<dbUser,varchar,dbo>].subtext_Content c ON l.PostID = c.ID  
WHERE lc.CategoryType = 1   
	AND lc.Active = 1  
	AND l.CategoryID In (Select CategoryID From [<dbUser,varchar,dbo>].subtext_links Where PostID = @EntryID)  
	AND c.BlogId = @BlogId
	AND c.ID <> @EntryID
ORDER BY c.DateAdded DESC
SET ROWCOUNT 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetRelatedEntries]  TO [public]
GO

/*Top10Posts - (called from Top10Module.ascx) - GY*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetTopEntries]
	@BlogId int,
	@RowCount int
AS
SET ROWCOUNT @RowCount
SELECT DISTINCT
	Id = evc.EntryId
	, c.EntryName
	, ViewCount = (evc.WebCount + evc.AggCount)
	, c.Title
	, DateSyndicated = c.DateAdded
FROM [<dbUser,varchar,dbo>].subtext_EntryViewCount evc
	INNER JOIN [<dbUser,varchar,dbo>].subtext_Content c  
		ON evc.EntryId = c.Id
WHERE c.BlogId = @BlogId
ORDER BY ViewCount DESC
SET ROWCOUNT 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetTopEntries]  TO [public]
GO

/*
Selects a page of blog posts for export to blogml. These are 
sorted ascending by id to map to the database.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetEntriesForExport]
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
		, DateCreated = content.DateAdded 
		, content.DateSyndicated
		, content.[Text] 
		, content.[Description]
		, content.PostType 
		, content.Author 
		, content.Email 
		, content.DateUpdated 
		, FeedbackCount = ISNULL(content.FeedbackCount, 0)
		, content.PostConfig
		, content.EntryName
		, content.DateSyndicated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	INNER JOIN #IDs idTable ON idTable.Id = content.[ID]
ORDER BY idTable.[ID] ASC
 
SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE 	BlogId = @BlogId 
	AND PostType = 1 OR PostType = 2

-- Select associated categories
SELECT	PostId = p.[Id]
		, c.CategoryID
		, c.Title
	FROM [<dbUser,varchar,dbo>].[subtext_Links] l
		INNER JOIN #IDs p ON l.[PostID] = p.[ID]  
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID
	ORDER BY p.[ID] ASC

-- Select associated comments
SELECT	f.[Id]
		, Title
		, Body
		, BlogId
		, EntryId
		, Author
		, IsBlogAuthor
		, Email
		, Url
		, FeedbackType
		, StatusFlag
		, CommentAPI
		, Referrer
		, IpAddress
		, UserAgent
		, FeedbackChecksumHash
		, DateCreated
		, DateModified
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	INNER JOIN #IDs idTable ON idTable.Id = f.[EntryId]
	WHERE f.FeedbackType = 1 -- Comment
ORDER BY idTable.[ID] ASC

-- Select associated track/ping backs.
SELECT	f.[Id]
		, Title
		, Body
		, BlogId
		, EntryId
		, Author
		, IsBlogAuthor
		, Email
		, Url
		, FeedbackType
		, StatusFlag
		, CommentAPI
		, Referrer
		, IpAddress
		, UserAgent
		, FeedbackChecksumHash
		, DateCreated
		, DateModified
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	INNER JOIN #IDs idTable ON idTable.Id = f.[EntryId]
	WHERE f.FeedbackType = 2 -- Trackback/Pingback
ORDER BY idTable.[ID] ASC

-- Get the Post's author(s)
SELECT	p.[Id]
		, @BlogId AS AuthorId-- Hardcoding the AuthorId since right now we only have one.
	FROM #IDs p
	ORDER BY p.[ID] ASC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetEntriesForExport] TO [public]
GO




/*
	subtext_GetPostsByCategoriesArchive - (called from CategoryCloud.ascx) - SCH
	retrieves all active categories with their post count
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

SELECT	[Id] = c.CategoryID
		, c.Title 
		, [Count] = COUNT(1)
		, [Month] = 1
		, [Year] = 1
		, [Day] = 1
	
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories] c 
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] l on c.CategoryID = l.CategoryID
WHERE	c.Active= 1 
	AND	(c.BlogId = @BlogId OR @BlogId IS NULL)
	AND	c.CategoryType = 1 -- post category

GROUP BY c.CategoryID, c.Title
ORDER BY c.Title

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoriesArchive]  TO [public]
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
	Id = KeyWordID
	, Word
	, Rel
	, [Text]
	, ReplaceFirstTimeOnly
	, OpenInNewWindow
	, CaseSensitive
	, Url
	, Title
	, BlogId
FROM
	[<dbUser,varchar,dbo>].[subtext_keywords]
WHERE 
	BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]  TO [public]
GO


/*	ClearBlogContent - used to delete all content (Entries, Comments, Track/Ping-backs, Statistics, etc...)
	for a given blog (sans the Image Galleries). Used from the Admin -> Import/Export Page.
*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_ClearBlogContent]
	@BlogId int
AS
DELETE FROM [<dbUser,varchar,dbo>].subtext_Links WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_MetaTag WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_EntryTag WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_EntryViewCount WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_Referrals WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_Log WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_Images WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_LinkCategories WHERE BlogId = @BlogId AND CategoryType <> 3 -- We're not doing Image Galleries.
DELETE FROM [<dbUser,varchar,dbo>].subtext_KeyWords WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_EntryViewCount WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_FeedBack WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_Tag WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_Content WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_URLs WHERE UrlID NOT IN (SELECT UrlId FROM subtext_Referrals)

DECLARE @Now datetime
set @Now = getdate()

EXEC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] @BlogId, @Now

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_ClearBlogContent]  TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByTag]
(
	@ItemCount int
	, @Tag nvarchar(256)
	, @BlogId int
	, @IsActive bit=1
	, @CurrentDateTime datetime
)
AS
DECLARE @TagId int
SELECT @TagId = Id FROM subtext_Tag WHERE BlogId = @BlogId AND [Name] = @Tag

SET ROWCOUNT @ItemCount
SELECT	content.BlogId
	, content.[ID]
	, content.Title
	, DateCreated = content.DateAdded
	, content.[Text]
	, content.[Description]
	, content.PostType
	, content.Author
	, content.Email
	, content.DateUpdated
	, FeedbackCount = ISNULL(content.FeedbackCount, 0)
	, content.PostConfig
	, content.EntryName 
	, content.DateSyndicated
	, e.Id as EnclosureId
	, e.Title as EnclosureTitle
	, e.Url as EnclosureUrl
	, e.MimeType as EnclosureMimeType
	, e.Size as EnclosureSize
	, e.EnclosureEnabled as EnclosureEnabled
	, e.AddToFeed
	, e.ShowWithPost
FROM [<dbUser,varchar,dbo>].[subtext_Content] content WITH (NOLOCK)
	LEFT JOIN [<dbUser,varchar,dbo>].[subtext_Enclosure] e on content.[ID] = e.EntryId
WHERE  content.BlogId = @BlogId 
	AND content.PostConfig & 1 = 1
	AND content.DateSyndicated <= @CurrentDateTime
	AND content.ID IN 
	(
		SELECT EntryId 
		FROM [<dbUser,varchar,dbo>].[subtext_EntryTag] 
		WHERE BlogId = @BlogId 
			AND TagId = @TagId
	)
ORDER BY content.DateSyndicated DESC
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByTag]  TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertEntryTagList] 
	(
	@EntryId int,
	@BlogId int,
	@TagList ntext
	)
AS

-- When taglist is empty, delete any potentially existing tags.
IF CONVERT(nvarchar(1),@TagList) = ''
BEGIN
	DELETE FROM subtext_EntryTag 
		WHERE BlogId = @BlogId AND EntryId = @EntryId
END
ELSE
BEGIN
	DECLARE @Tags TABLE (tagId int,
						 tag nvarchar(2000))
	-- Populate the in-memory table with the @TagList string broken out into rows.
	INSERT INTO @Tags 
		SELECT t.Id, c.nstr
		FROM iter_charlist_to_table(@TagList, ',') c
		LEFT OUTER JOIN subtext_Tag t ON BlogId = @BlogId AND t.[Name] = c.nstr

	-- If a tag doesn't exist, it needs to be created in subtext_Tag.
	INSERT INTO subtext_Tag (BlogId, [Name])
		SELECT @BlogId, tag  FROM @Tags 
		WHERE tag NOT IN (SELECT [Name] FROM subtext_Tag WHERE BlogId = @BlogID)
	
	-- If tags were created above, we need to update @Tags with their Ids.
	UPDATE @Tags SET tagId = s.Id FROM @Tags t, subtext_Tag s 
		WHERE s.BlogId = @BlogId AND s.[Name] = t.tag AND t.TagId IS NULL

	-- If tags exist for an entry that have been removed, remove the link.
	DELETE FROM subtext_EntryTag 
		WHERE BlogId = @BlogId AND EntryId = @EntryId 
		AND TagId NOT IN (SELECT tagId FROM @Tags)

	-- Now add any tags that aren't already linked.
	INSERT INTO subtext_EntryTag (BlogId, EntryId, TagId)
	SELECT @BlogId, @EntryId, tagId FROM @Tags
		WHERE tagId NOT IN 
			(SELECT TagId FROM subtext_EntryTag 
				WHERE BlogId = @BlogId AND EntryId = @EntryId)
END
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertEntryTagList] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetTopTags] 
	(
	@ItemCount int
	, @BlogId int
	)
AS
SET ROWCOUNT @ItemCount
SELECT t.[Name], COUNT(1) AS TagCount FROM [<dbUser,varchar,dbo>].subtext_Tag t, [<dbUser,varchar,dbo>].subtext_EntryTag e , [<dbUser,varchar,dbo>].subtext_content c
WHERE t.BlogId = @BlogId AND t.Id = e.TagId and c.id=e.entryid and c.datesyndicated <= getdate()
GROUP BY t.[Name]
ORDER BY Count(*) DESC
GO 

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetTopTags] TO [public]
GO


-----------------------------------------
-- MetaTags Create, Update, Delete, Get
-----------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertMetaTag] 
	(
		@Content nvarchar(512),
		@Name nvarchar(100) = NULL,
		@HttpEquiv nvarchar(100) = NULL,
		@BlogId int,
		@EntryId int = NULL,
		@DateCreated datetime = NULL,
		@Id int OUTPUT
	)
AS
	IF @DateCreated IS NULL 
		SET @DateCreated = getdate()
		
	IF LEN(@Name) = 0
		SET @Name = NULL
	IF LEN(@HttpEquiv) = 0
		SET @HttpEquiv = NULL

	INSERT INTO [<dbUser,varchar,dbo>].subtext_MetaTag
		([Content], [Name], HttpEquiv, BlogId, EntryId, DateCreated)
	VALUES
		(@Content, @Name, @HttpEquiv, @BlogId, @EntryId, @DateCreated)

	SELECT @Id = SCOPE_IDENTITY()

GO 

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_InsertMetaTag] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateMetaTag] 
	(
		@Id int,
		@Content nvarchar(512),
		@Name nvarchar(100) = NULL,
		@HttpEquiv nvarchar(100) = NULL,
		@BlogId int,
		@EntryId int = NULL
	)
AS
		
	IF LEN(RTRIM(LTRIM(@Name))) = 0
		SET @Name = NULL
	IF LEN(RTRIM(LTRIM(@HttpEquiv))) = 0
		SET @HttpEquiv = NULL

	UPDATE [<dbUser,varchar,dbo>].subtext_MetaTag
	SET
		[Content] = @Content,
		[Name] = @Name,
		HttpEquiv = @HttpEquiv,
		BlogId = @BlogId,
		EntryId = @EntryId
	WHERE
		[Id] = @Id

GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UpdateMetaTag] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetMetaTags] 
	(
		@BlogId int,
		@EntryId int = NULL,
		@PageIndex int,
		@PageSize int
	)
AS

WITH OrderedMetaTags as
(
SELECT Id
	, [Content]
	, [Name]
	, HttpEquiv
	, BlogId
	, EntryId
	, DateCreated 
	, row_number() over(order by DateCreated ASC, Id ASC) RowNumber
FROM [<dbUser,varchar,dbo>].subtext_MetaTag
WHERE BlogId = @BlogId
	AND (@EntryId is null OR EntryId = @EntryId)
)

SELECT * 
FROM OrderedMetaTags 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize


SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_MetaTag]
WHERE 	BlogId = @BlogId 
	AND (@EntryId is null OR EntryId = @EntryId)

GO 

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetMetaTags] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteMetaTag] 
	(
		@Id int
	)
AS
	DELETE FROM [<dbUser,varchar,dbo>].[subtext_MetaTag] WHERE [Id] = @Id

GO

GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetBlogByDomainAlias]
	(
		  @Host	VARCHAR(100)
		, @Application nvarchar(50)
		, @Strict bit = 1 
	)
AS
DECLARE
	@BlogId int

	IF @Strict = 0 
		AND NOT EXISTS(SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].subtext_Config WHERE Host = @Host)
		AND (1 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].subtext_DomainAlias WHERE Host = @Host))
	BEGIN
		SELECT @BlogId = BlogId FROM [<dbUser,varchar,dbo>].subtext_DomainAlias WHERE (Host = @Host OR Host = 'www.' + @Host) AND IsActive = 1
	END
	ELSE
	BEGIN
		SELECT @BlogId = BlogId FROM [<dbUser,varchar,dbo>].subtext_DomainAlias WHERE (Host = @Host OR Host = 'www.' + @Host) AND Application = @Application AND IsActive = 1
	END
	EXEC [<dbUser,varchar,dbo>].[subtext_GetBlogById]  @BlogId = @BlogId
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetBlogByDomainAlias] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableDomainAliases]
(
	  @PageIndex int
	, @PageSize int
	, @BlogId int
)
AS

WITH OrderedAliases as
(
SELECT	
		  Id
		, BlogId
		, Host
		, Subfolder = Application
		, IsActive
		, row_number() over(order by Id ASC) RowNumber
FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] 
WHERE BlogId = @BlogId
)

SELECT * 
FROM OrderedAliases 
WHERE RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize


SELECT COUNT([BlogId]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias]
WHERE BlogId = @BlogId
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetPageableDomainAliases] TO [public]
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_CreateDomainAlias]
	(
		  @BlogId int
		, @Host	nvarchar(100)
		, @Application nvarchar(50)
		, @Active bit = 1
		, @Id int OUTPUT
	)
AS

SELECT @Id = Id 
FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] 
WHERE Host = @Host AND Application = @Application

IF @Id IS NULL
BEGIN
	INSERT INTO [<dbUser,varchar,dbo>].[subtext_DomainAlias]		
	(
		 BlogId
		,Host
		,Application
		,IsActive
	)
	VALUES
	(
		 @BlogId
		,@Host
		,@Application
		,@Active
	)

	SELECT @Id = SCOPE_IDENTITY()
END
ELSE
	SELECT @Id
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CreateDomainAlias] TO [public]
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteDomainAlias]
	(
		  @Id	INT
	)
AS
	DELETE 
	FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] 
	WHERE Id = @Id
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_DeleteDomainAlias] TO [public]
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateDomainAlias]
	(
		  @Id int
		, @BlogId int
		, @Host	nvarchar(100)
		, @Application nvarchar(50)
		, @Active bit = 1
	)
AS
	UPDATE [<dbUser,varchar,dbo>].[subtext_DomainAlias]		
	SET  BlogId			=@BlogId
		,Host			=@Host
		,Application	=@Application
		,IsActive		=@Active
	WHERE Id = @Id	
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UpdateDomainAlias] TO [public]
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetDomainAliasById]
	(
		  @Id	INT
	)
AS
	SELECT Id, 
		BlogId, 
		Host, 
		Subfolder = Application, 
		IsActive
	FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] 
	WHERE Id = @Id
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetDomainAliasById] TO [public]
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteBlogGroup]
(
	@Id int
)
AS
DELETE FROM [<dbUser,varchar,dbo>].[subtext_BlogGroup] WHERE Id = @Id
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_DeleteBlogGroup] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetBlogGroup]
(
	@Id int
	, @Active bit
)
AS
SELECT	c.Id
		, c.Title
		, IsActive = c.Active
		, c.DisplayOrder
		, c.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_BlogGroup] c
WHERE c.Id = @Id AND c.Active <> CASE @Active WHEN 0 THEN -1 else 0 END
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetBlogGroup] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertBlogGroup]
(
	@Title nvarchar(150)
	, @Active bit
	, @DisplayOrder int = NULL
	, @Description nvarchar(1000) = NULL
	, @Id int OUTPUT
)
AS
Set NoCount ON
INSERT INTO [<dbUser,varchar,dbo>].[subtext_BlogGroup]
( 
	Title
	, Active
	, [Description]
	, DisplayOrder 
)
VALUES 
(
	@Title
	, @Active
	, @Description
	, @DisplayOrder
)
SELECT @Id = SCOPE_IDENTITY()
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_InsertBlogGroup] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateBlogGroup]
(
	@Id int,
	@Title nvarchar(150),
	@Active bit,
	@Description nvarchar(1000) = NULL,
	@DisplayOrder int = NULL
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_BlogGroup] 
SET 
	[Title] = @Title, 
	[Active] = @Active,
	[Description] = @Description,
	[DisplayOrder] = @DisplayOrder
WHERE   
		[Id] = @Id
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UpdateBlogGroup] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_ListBlogGroups]
(
	@Active bit
)
AS
SELECT	c.Id
		, c.Title
		, IsActive = c.Active
		, c.DisplayOrder
		, c.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_BlogGroup] c
WHERE c.Active <> CASE @Active WHEN 0 THEN -1 else 0 END
ORDER BY DisplayOrder, Title
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_ListBlogGroups] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[DNW_GetRecentImages]
	@Host nvarchar(100)
	, @GroupID int = NULL
	, @rowCount int

AS
SET ROWCOUNT @rowCount
SELECT [Blog.Host] = Host
	, images.BlogId
	, [Blog.Application] = Application
	, images.ImageID
	, images.Title
	, FileName = images.[File]
	, images.Width
	, images.Height
	, [Blog.TimeZoneId] = config.TimeZoneId
	, [Blog.Title] = config.Title
	, [Category.Title] = categories.Title
	, images.CategoryID
	, IsActive = images.Active
	, Url = images.Url
FROM [<dbUser,varchar,dbo>].[subtext_Images] images
INNER JOIN	[<dbUser,varchar,dbo>].[subtext_Config] config ON images.BlogId = config.BlogId
INNER JOIN  [<dbUser,varchar,dbo>].[subtext_LinkCategories] categories ON categories.CategoryID = images.CategoryID
WHERE  images.Active > 0
	AND config.Host = @Host
	AND (config.BlogGroupId = @GroupID OR @GroupID is NULL)
ORDER BY [ImageID] DESC
SET ROWCOUNT 0
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[DNW_GetRecentImages] TO [public]
GO

-- Enclosures

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertEnclosure] 
	(
		@Title nvarchar(256),
		@Url nvarchar(256),
		@MimeType nvarchar(50),
		@Size bigint,
		@AddToFeed bit,
		@ShowWithPost bit,
		@EntryId int,
		@Id int OUTPUT
	)
AS

	INSERT INTO [<dbUser,varchar,dbo>].subtext_Enclosure
		([Title], [Url], [MimeType], [Size], [EntryId], [AddToFeed], [ShowWithPost])
	VALUES
		(@Title, @Url, @MimeType, @Size, @EntryId, @AddToFeed, @ShowWithPost)

	SELECT @Id = SCOPE_IDENTITY()

GO 

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_InsertEnclosure] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateEnclosure] 
	(
		@Title nvarchar(256),
		@Url nvarchar(256),
		@MimeType nvarchar(50),
		@Size bigint,
		@AddToFeed bit,
		@ShowWithPost bit,
		@EntryId int,
		@Id int
	)
AS

	UPDATE [<dbUser,varchar,dbo>].subtext_Enclosure
	SET
		[Title] = @Title,
		[Url] = @Url,
		MimeType = @MimeType,
		Size = @Size,
		EntryId = @EntryId,
		AddToFeed = @AddToFeed,
		ShowWithPost = @ShowWithPost
	WHERE
		[Id] = @Id

GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UpdateEnclosure] TO [public]
GO


CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteEnclosure] 
	(
		@Id int
	)
AS
	DELETE FROM [<dbUser,varchar,dbo>].[subtext_Enclosure] WHERE [Id] = @Id

GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_DeleteEnclosure] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetBlogStats] 
(
	@BlogId int
)
AS

SELECT ActivePostCount = (
		SELECT COUNT(1) 
		FROM subtext_Content
		WHERE BlogId = @BlogId
			AND PostType = 1 /* BlogPost */
			AND PostConfig & 1 = 1
	),
	DraftPostCount = (
		SELECT COUNT(1) 
		FROM subtext_Content
		WHERE BlogId = @BlogId
			AND PostType = 1 /* BlogPost */
			AND PostConfig & 1 != 1
	),
	ActiveArticleCount = (
		SELECT COUNT(1) 
		FROM subtext_Content
		WHERE BlogId = @BlogId
			AND PostType = 2 /* Story */
			AND PostConfig & 1 = 1
	),
	DraftArticleCount = (
		SELECT COUNT(1) 
		FROM subtext_Content
		WHERE BlogId = @BlogId
			AND PostType = 2 /* Story */
			AND PostConfig & 1 != 1
	),
	FeedbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 1 /* Comment */
	),
	ApprovedFeedbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 1 /* Comment */
			AND StatusFlag & 1 = 1
			AND StatusFlag & 8 != 8
	),
	ApprovedTrackbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 2 /* PingTrack */
			AND StatusFlag & 1 = 1
			AND StatusFlag & 8 != 8
	),
	SpamFalsePositiveFeedbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 1 /* Comment */
			AND StatusFlag & 1 = 1
			AND StatusFlag & 8 != 8
			AND StatusFlag & 4 = 4
	),
	SpamFalsePositiveTrackbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 2 /* PingTrack */
			AND StatusFlag & 1 = 1
			AND StatusFlag & 4 = 4
			AND StatusFlag & 8 != 8
	),
	AwaitingModerationFeedbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 1 /* Comment */
			AND StatusFlag & 2 = 2
			AND StatusFlag & 8 != 8
	),
	AwaitingModerationTrackbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 2 /* PingTrack */
			AND StatusFlag & 2 = 2
			AND StatusFlag & 8 != 8
	),
	FlaggedAsSpamFeedbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 1 /* Comment */
			AND StatusFlag = 4
			AND StatusFlag & 8 != 8
	),
	FlaggedAsSpamTrackbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 2 /* PingTrack */
			AND StatusFlag = 4
			AND StatusFlag & 8 != 8
	),
	DeletedFeedbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 1 /* Comment */
			AND StatusFlag & 8 = 8
	),
	DeletedTrackbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 2 /* PingTrack */
			AND StatusFlag & 8 = 8
			AND StatusFlag & 4 != 4
	),
	DeletedSpamFeedbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 1 /* Comment */
			AND StatusFlag & 4 = 4 
			AND StatusFlag & 8 = 8
	),
	DeletedSpamTrackbackCount = (
		SELECT COUNT(1) 
		FROM subtext_Feedback
		WHERE BlogId = @BlogId
			AND FeedbackType = 2 /* PingTrack */
			AND StatusFlag & 4 = 4 
			AND StatusFlag & 8 = 8
	),
	AverageCommentsPerPost = (
		SELECT  AVG(CommentsPerPost)
		FROM    ( SELECT    COUNT([<dbUser,varchar,dbo>].subtext_Feedback.Id) CommentsPerPost,
							[<dbUser,varchar,dbo>].subtext_Content.Id
				  FROM      [<dbUser,varchar,dbo>].subtext_Feedback
							RIGHT JOIN [<dbUser,varchar,dbo>].subtext_Content
								ON [<dbUser,varchar,dbo>].subtext_Content.Id = [<dbUser,varchar,dbo>].subtext_Feedback.EntryId
				  WHERE     FeedbackType = 1
							AND subtext_Feedback.BlogId = @BlogId
				  GROUP BY  [<dbUser,varchar,dbo>].subtext_Content.Id
				) commentsPerPost
	),	
	AveragePostsPerMonth = (
		SELECT AVG(PostsPerMonth)
		FROM    ( SELECT    DATEADD(year, YEAR(DateAdded) - 1900,
									DATEADD(month, MONTH(DateAdded)-1, 0)) Date,
							COUNT(*) PostsPerMonth
				  FROM      subtext_Content
				  WHERE		BlogId = @BlogId
				  GROUP BY  MONTH(DateAdded),
							YEAR(DateAdded)
				) postsPerMonth
	),
	AveragePostsPerWeek = (
		SELECT  AVG(postsPerWeek)
		FROM    ( SELECT    DATEPART(week, dateadded) weekNum,
							YEAR(dateadded) [year],
							COUNT(*) postsPerWeek
				  FROM      subtext_Content
				  WHERE		BlogId = @BlogId
				  GROUP BY  DATEPART(week, dateadded),
							YEAR(DateAdded)
				) postsPerWeek
	),
	AverageCommentsPerMonth = (
		SELECT  AVG(CommentsPerMonth)
		FROM	(
					SELECT	[month] = DATEPART(month, DateCreated),
							[Year] = YEAR(DateCreated),	
							CommentsPerMonth = COUNT(*)
					FROM    subtext_Feedback
					WHERE   feedbacktype = 1
						AND BlogId = @BlogId
					GROUP BY	DATEPART(month, DateCreated),
								YEAR(DateCreated)
				 ) CommentsPerMonth
	)

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetBlogStats] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPopularPosts] 
(
	@BlogId int, 
	@MinDate datetime = NULL
)
AS

/* Most Popular Posts */
SELECT TOP 10
        Id = EntryId,
        EntryName,
        DateCreated = [<dbUser,varchar,dbo>].subtext_Content.DateAdded,
        DateUpdated,
        DateSyndicated,
        PostType,
        PostConfig,
        Title,
        WebCount,
        AggCount,
        FeedBackCount = CommentCount,
        WeightedScore = ( WebCount * 15 ) + ( AggCount * 10 ) + ( CommentCount * 35 )
FROM    [<dbUser,varchar,dbo>].subtext_EntryViewCount,
        ( SELECT    COUNT([<dbUser,varchar,dbo>].subtext_Feedback.Id) CommentCount,
                    [<dbUser,varchar,dbo>].subtext_Content.Id
          FROM      [<dbUser,varchar,dbo>].subtext_Feedback
                    RIGHT JOIN [<dbUser,varchar,dbo>].subtext_Content ON [<dbUser,varchar,dbo>].subtext_Content.Id = [<dbUser,varchar,dbo>].subtext_Feedback.EntryId
          WHERE     FeedbackType = 1
          GROUP BY  [<dbUser,varchar,dbo>].subtext_Content.Id
        ) Comments,
        subtext_Content
WHERE   Comments.Id = EntryId
        AND [<dbUser,varchar,dbo>].Subtext_Content.Id = EntryId
		AND [<dbUser,varchar,dbo>].Subtext_Content.BlogId = @BlogId
		AND (@MinDate IS NULL OR [<dbUser,varchar,dbo>].subtext_Content.DateAdded >= @MinDate)
ORDER BY ( WebCount * 15 ) + ( AggCount * 10 ) + ( CommentCount * 35 ) DESC

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetPopularPosts] TO [public]
GO
