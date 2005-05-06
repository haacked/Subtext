if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[iter_charlist_to_table]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[iter_charlist_to_table]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DNW_GetRecentPosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DNW_GetRecentPosts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DNW_HomePageData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DNW_HomePageData]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DNW_Stats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DNW_Stats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DNW_Total_Stats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DNW_Total_Stats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_DeleteCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_DeleteCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_DeleteImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_DeleteImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_DeleteImageCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_DeleteImageCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_DeleteKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_DeleteKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_DeleteLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_DeleteLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_DeleteLinksByPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_DeleteLinksByPostID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_DeletePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_DeletePost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetActiveCategoriesWithLinkCollection]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetActiveCategoriesWithLinkCollection]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetAllCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetAllCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetBlogKeyWords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetBlogKeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetCategoryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetCategoryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetConditionalEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetConditionalEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetConditionalEntriesByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetConditionalEntriesByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetConfig]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetEntriesByDayRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetEntriesByDayRange]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetEntryCollectionByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetEntryCollectionByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetEntryWithCategoryTitles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetEntryWithCategoryTitles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetEntryWithCategoryTitlesByEntryName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetEntryWithCategoryTitlesByEntryName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetFeedBack]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetFeedBack]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetImageCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetImageCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetLinkCollectionByPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetLinkCollectionByPostID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetLinksByActiveCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetLinksByActiveCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetLinksByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPageableEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPageableEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPageableEntriesByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPageableEntriesByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPageableFeedback]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPageableFeedback]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPageableKeyWords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPageableKeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPageableLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPageableLinks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPageableLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPageableLinksByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPageableReferrers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPageableReferrers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPageableReferrersByEntryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPageableReferrersByEntryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPostsByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPostsByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPostsByCategoryIDByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPostsByCategoryIDByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPostsByCategoryName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPostsByCategoryName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPostsByCategoryNameByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPostsByCategoryNameByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPostsByDayRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPostsByDayRange]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPostsByMonth]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPostsByMonth]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPostsByMonthArchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPostsByMonthArchive]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetPostsByYearArchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetPostsByYearArchive]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetRecentEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetRecentEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetRecentEntriesByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetRecentEntriesByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetRecentEntriesWithCategoryTitles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetRecentEntriesWithCategoryTitles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetSingleDay]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetSingleDay]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetSingleEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetSingleEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetSingleEntryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetSingleEntryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetSingleImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetSingleImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetSingleLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetSingleLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_GetUrlID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_GetUrlID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertEntryViewCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertEntryViewCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertLinkCategoryList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertLinkCategoryList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertPingTrackEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertPingTrackEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertPostCategoryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertPostCategoryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertReferral]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertReferral]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_InsertViewStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_InsertViewStats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_StatsSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_StatsSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_TrackEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_TrackEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_UTILITY_AddBlog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_UTILITY_AddBlog]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_UpdateCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_UpdateCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_UpdateConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_UpdateConfig]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_UpdateConfigUpdateTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_UpdateConfigUpdateTime]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_UpdateEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_UpdateEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_UpdateImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_UpdateImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_UpdateKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_UpdateKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_UpdateLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_UpdateLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Utility_GetUnHashedPasswords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_Utility_GetUnHashedPasswords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Utility_UpdateToHashedPassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[blog_Utility_UpdateToHashedPassword]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Config]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Config]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Content]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Content]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_EntryViewCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_EntryViewCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Images]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Images]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_KeyWords]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_KeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_LinkCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_LinkCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Links]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Links]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Referrals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Referrals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_URLs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_URLs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spamPostCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[spamPostCount]
GO

CREATE TABLE [dbo].[blog_Config] (
	[BlogID] [int] IDENTITY (0, 1) NOT NULL ,
	[UserName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Title] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[SubTitle] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Skin] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Application] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Host] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Author] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[TimeZone] [int] NULL ,
	[IsActive] [bit] NULL ,
	[Language] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ItemCount] [int] NULL ,
	[LastUpdated] [datetime] NULL ,
	[News] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[SecondaryCss] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PostCount] [int] NULL ,
	[StoryCount] [int] NULL ,
	[PingTrackCount] [int] NULL ,
	[CommentCount] [int] NULL ,
	[IsAggregated] [bit] NULL ,
	[Flag] [int] NULL ,
	[SkinCssFile] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[BlogGroup] [int] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_Content] (
	[ID] [int] NOT NULL ,
	[Title] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateAdded] [smalldatetime] NOT NULL ,
	[SourceUrl] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PostType] [int] NOT NULL ,
	[Author] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[SourceName] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[BlogID] [int] NULL ,
	[Description] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[DateUpdated] [smalldatetime] NULL ,
	[TitleUrl] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Text] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ParentID] [int] NULL ,
	[FeedBackCount] [int] NULL ,
	[PostConfig] [int] NULL ,
	[EntryName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_EntryViewCount] (
	[EntryID] [int] NOT NULL ,
	[BlogID] [int] NOT NULL ,
	[WebCount] [int] NOT NULL ,
	[AggCount] [int] NOT NULL ,
	[WebLastUpdated] [datetime] NULL ,
	[AggLastUpdated] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_Images] (
	[ImageID] [int] NOT NULL ,
	[Title] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[CategoryID] [int] NOT NULL ,
	[Width] [int] NOT NULL ,
	[Height] [int] NOT NULL ,
	[File] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Active] [bit] NOT NULL ,
	[BlogID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_KeyWords] (
	[KeyWordID] [int] NOT NULL ,
	[Word] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Text] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ReplaceFirstTimeOnly] [bit] NOT NULL ,
	[OpenInNewWindow] [bit] NOT NULL ,
	[Url] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Title] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[BlogID] [int] NOT NULL ,
	[CaseSensitive] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_LinkCategories] (
	[CategoryID] [int] NOT NULL ,
	[Title] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Active] [bit] NOT NULL ,
	[BlogID] [int] NOT NULL ,
	[CategoryType] [tinyint] NULL ,
	[Description] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_Links] (
	[LinkID] [int] NOT NULL ,
	[Title] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Url] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Rss] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Active] [bit] NOT NULL ,
	[CategoryID] [int] NULL ,
	[BlogID] [int] NOT NULL ,
	[PostID] [int] NULL ,
	[NewWindow] [bit] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_Referrals] (
	[EntryID] [int] NOT NULL ,
	[BlogID] [int] NOT NULL ,
	[UrlID] [int] NOT NULL ,
	[Count] [int] NOT NULL ,
	[LastUpdated] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_URLs] (
	[UrlID] [int] IDENTITY (0, 1) NOT NULL ,
	[URL] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[spamPostCount] (
	[theCount] [int] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[blog_Config] WITH NOCHECK ADD 
	CONSTRAINT [PK_blog_Config] PRIMARY KEY  CLUSTERED 
	(
		[BlogID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[blog_Config] WITH NOCHECK ADD 
	CONSTRAINT [DF_blog_Config_TimeZone] DEFAULT (0) FOR [TimeZone],
	CONSTRAINT [DF__blog_Conf__IsAct__047AA831] DEFAULT (1) FOR [IsActive],
	CONSTRAINT [DF_Blog_Config_Language] DEFAULT ('en-US') FOR [Language],
	CONSTRAINT [DF__blog_Conf__ItemC__0662F0A3] DEFAULT (15) FOR [ItemCount],
	CONSTRAINT [DF__blog_Conf__PostC__5D60DB10] DEFAULT (0) FOR [PostCount],
	CONSTRAINT [DF__blog_Conf__Story__5E54FF49] DEFAULT (0) FOR [StoryCount],
	CONSTRAINT [DF__blog_Conf__PingT__5F492382] DEFAULT (0) FOR [PingTrackCount],
	CONSTRAINT [DF__blog_Conf__Comme__603D47BB] DEFAULT (0) FOR [CommentCount],
	CONSTRAINT [DF__blog_Conf__IsAgg__61316BF4] DEFAULT (1) FOR [IsAggregated],
	CONSTRAINT [IX_blog_Config] UNIQUE  NONCLUSTERED 
	(
		[Application],
		[Host]
	)  ON [PRIMARY] 
GO

 CREATE  UNIQUE  INDEX [IX_blog_Config_HostApplication] ON [dbo].[blog_Config]([BlogID], [Host], [Application]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO








CREATE    Proc DNW_GetRecentPosts  -- 'localhost', 1
	@Host nvarchar(100),
	@GroupID int

as
SELECT Top 35 Host, Application, 
IsNull(blog_Content.EntryName,blog_Content.[ID]) as [EntryName], blog_Content.[ID],  blog_Content.Title, blog_Content.DateAdded, 
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.FeedBackCount,
blog_Content.SourceName, blog_Content.EntryName, Convert(bit,case when blog_Content.PostConfig & 2 = 2 then 1 else 0 end) as 'IsXHTML', blog_Config.Title as [BlogTitle],  blog_Content.PostCOnfig, blog_Config.TimeZone
, IsNull(case when PostConfig & 32 = 32 then blog_Content.[Description] else blog_Content.[Text] end,'') as [Description]

FROM blog_Content
inner join blog_Config on blog_Content.BlogID = blog_Config.BlogID
WHERE  blog_Content.PostType = 1 and  blog_Content.PostConfig & 1 = 1 
	and blog_Content.PostConfig & 64 = 64 and blog_Config.Flag & 2 = 2 and blog_Config.Host = @Host
	and BlogGroup & @GroupID = @GroupID
order by [ID] desc






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE        Proc  DNW_Stats
(
	@Host nvarchar(100),
	@GroupID int
)
as
Select BlogID, Author, Application, Host, Title, PostCount, CommentCount, StoryCount, PingTrackCount, LastUpdated
From blog_Config 
where PostCount > 0 and blog_Config.Flag & 2 = 2 and Host = @Host and BlogGroup & @GroupID = @GroupID
order by PostCount desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE  Proc DNW_Total_Stats
(
	@Host nvarchar(100),
	@GroupID int
)
as
Select Count(*) as [BlogCount], Sum(PostCount) as PostCount, Sum(CommentCount) as CommentCount, Sum(StoryCount) as StoryCount, Sum(PingTrackCount) as PingTrackCount 
From blog_Config where blog_Config.Flag & 2 = 2 and Host = @Host and BlogGroup & @GroupID = @GroupID

SET QUOTED_IDENTIFIER ON


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE   Proc DNW_HomePageData
(
	@Host nvarchar(100),
	@GroupID int
)
as 
exec DNW_Stats @Host, @GroupID
exec DNW_GetRecentPosts @Host, @GroupID
exec DNW_Total_Stats @Host, @GroupID



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO








CREATE  Proc blog_DeleteCategory
(
	@CategoryID int,
	@BlogID int
)
as
Delete blog_Links From blog_Links Where blog_Links.CategoryID = @CategoryID and blog_Links.BlogID = @BlogID
Delete blog_LinkCategories From blog_LinkCategories Where blog_LinkCategories.CategoryID = @CategoryID and blog_LinkCategories.BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








Create Proc blog_DeleteImage
(
	@BlogID int,
	@ImageID int
)
as
Delete blog_Images From blog_Images 
Where ImageID = @ImageID and BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO








Create  Proc blog_DeleteImageCategory
(
	@CategoryID int,
	@BlogID int
)
as
Delete blog_Images From blog_Images Where blog_Images.CategoryID = @CategoryID and blog_Images.BlogID = @BlogID
Delete blog_LinkCategories From blog_LinkCategories Where blog_LinkCategories.CategoryID = @CategoryID and blog_LinkCategories.BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




Create Proc blog_DeleteKeyWord
(
	@KeyWordID int,
	@BlogID int
)

as

Delete From blog_KeyWords where BLOGID = @BlogID and KeyWordID = @KeyWordID




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








Create Proc blog_DeleteLink
(
	@LinkID int,
	@BlogID int
)
as
Delete blog_Links From blog_Links Where blog_Links.[LinkID] = @LinkID and blog_Links.BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








Create Proc blog_DeleteLinksByPostID
(
	@PostID int,
	@BlogID int
)
as
Set NoCount On
Delete blog_Links From blog_Links where PostID = @PostID and BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO











CREATE      Proc blog_DeletePost
(
	@ID int,
	@BlogID int
)
as

Declare @ParentID int, @PostType int

Select @ParentID = ParentID, @PostType = PostType From blog_Content where [ID] = @ID


if(@PostType = 3 or @PostType = 4)
Begin
	Update blog_Content
	Set FeedBackCount = FeedBackCount - 1
	where [ID] = @ParentID

End
Else
Begin

	Delete From blog_Content Where ParentID = @ID
	Delete From blog_Links where PostID = @ID
	Delete From blog_EntryViewCount where EntryID = @ID
	Delete From blog_Referrals where EntryID = @ID
End

Delete From blog_Content Where blog_Content.[ID] = @ID and blog_Content.BlogID = @BlogID










GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE       Proc blog_GetActiveCategoriesWithLinkCollection
(
	@BlogID int
)
as
SELECT blog_LinkCategories.CategoryID, blog_LinkCategories.Title, blog_LinkCategories.Active, blog_LinkCategories.CategoryType, blog_LinkCategories.[Description]
FROM blog_LinkCategories
WHERE blog_LinkCategories.Active= 1 and blog_LinkCategories.BlogID = @BlogID and blog_LinkCategories.CategoryType = 0
ORDER BY blog_LinkCategories.Title;
SELECT blog_Links.LinkID, blog_Links.Title, blog_Links.Url, blog_Links.Rss, blog_Links.Active, blog_Links.NewWindow, blog_Links.CategoryID,  blog_Links.PostID 
FROM blog_Links INNER JOIN blog_LinkCategories ON blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE blog_Links.Active=1 AND blog_LinkCategories.Active=1
and blog_LinkCategories.BlogID = @BlogID and  blog_Links.BlogID = @BlogID and blog_LinkCategories.CategoryType = 0
ORDER BY blog_Links.Title;









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE       Proc blog_GetAllCategories
(
	@BlogID int,
	@IsActive bit,
	@CategoryType tinyint
)
As
SELECT blog_LinkCategories.CategoryID, blog_LinkCategories.Title, blog_LinkCategories.Active, 
blog_LinkCategories.CategoryType, blog_LinkCategories.[Description]
FROM blog_LinkCategories
where blog_LinkCategories.BlogID = @BlogID and blog_LinkCategories.CategoryType = @CategoryType 
and blog_LinkCategories.Active <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_LinkCategories.Title;









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





CREATE  Proc blog_GetBlogKeyWords
(
	@BlogID int
)
as

Select 
	KeyWordID, Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID
From
	blog_keywords
Where 
	BlogID = @BlogID





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO










CREATE      Proc blog_GetCategory
(
	@CategoryID int,
	@IsActive bit,
	@BlogID int
)
as
SELECT blog_LinkCategories.CategoryID, blog_LinkCategories.Title, blog_LinkCategories.Active, 
blog_LinkCategories.CategoryType, blog_LinkCategories.[Description]
FROM blog_LinkCategories
WHERE blog_LinkCategories.CategoryID=@CategoryID and blog_LinkCategories.BlogID = @BlogID and blog_LinkCategories.Active <> case @IsActive when 0 then -1 else 0 end










GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO










CREATE     Proc blog_GetCategoryByName 
(
	@CategoryName nvarchar(150),
	@IsActive bit,
	@BlogID int
)
as
SELECT blog_LinkCategories.CategoryID, blog_LinkCategories.Title, blog_LinkCategories.Active, 
blog_LinkCategories.CategoryType, blog_LinkCategories.[Description]
FROM blog_LinkCategories
WHERE blog_LinkCategories.Title=@CategoryName and blog_LinkCategories.BlogID = @BlogID and blog_LinkCategories.Active <> case @IsActive when 0 then -1 else 0 end










GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




CREATE   Proc blog_GetConditionalEntries
(
	@ItemCount int, 
	@PostType int,
	@PostConfig int,
	@BlogID int
)
as

set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, Blog_Content.PostConfig, blog_Content.EntryName FROM blog_Content
WHERE blog_Content.PostType=@PostType and blog_Content.BlogID = @BlogID
 and blog_Content.PostConfig & @PostConfig = @PostConfig
ORDER BY blog_Content.[dateadded] desc





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




CREATE   Proc blog_GetConditionalEntriesByDateUpdated
(
	@DateUpdated datetime,
	@ItemCount int, 
	@PostType int,
	@PostConfig int,
	@BlogID int
)
as

set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, Blog_Content.PostConfig, blog_Content.ENtryName FROM blog_Content
WHERE blog_Content.PostType=@PostType and blog_Content.BlogID = @BlogID
 and blog_Content.PostConfig & @PostConfig = @PostConfig and blog_Content.DateUpdated > @DateUpdated
ORDER BY blog_Content.[dateadded] desc





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


/*
Obtains the configuration for the blog.  Subtext 1.0 will only 
support single user blogs, so these parameters are no longer 
necessary.
*/
CREATE PROC blog_GetConfig
(
	@Host nvarchar(100) -- Depracated
	, @Application nvarchar(50) -- Depracated
)
as
/* Out with the old
Select 	blog_Config.BlogID, UserName, [Password], Email, Title, SubTitle, Skin, Application, Host, 
	Author, TimeZone, ItemCount, [Language], News, SecondaryCss, 
	LastUpdated, PostCount, StoryCount, PingTrackCount, CommentCount, Flag, SkinCssFile From blog_Config

Where Host = @Host and Application = @Application and Flag & 1 > 0
*/

-- In with the new
Select TOP 1 blog_Config.BlogID, UserName, [Password], Email, Title, SubTitle, Skin, Application, Host, 
	Author, TimeZone, ItemCount, [Language], News, SecondaryCss, 
	LastUpdated, PostCount, StoryCount, PingTrackCount, CommentCount, Flag, SkinCssFile From blog_Config

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO












CREATE          Proc blog_GetEntriesByDayRange
(
	@StartDate datetime,
	@StopDate datetime,
	@PostType int,
	@IsActive bit,
	@BlogID int
)
as
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig,blog_Content.EntryName FROM blog_Content
WHERE 
	(blog_Content.DateAdded > @StartDate and blog_Content.DateAdded < DateAdd(day,1,@StopDate))
And blog_Content.PostType=@PostType and blog_Content.BlogID = @BlogID and 
blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.DateAdded DESC;












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO












CREATE       Proc blog_GetEntryCollectionByDateUpdated
(
	@ItemCount int,
	@IsActive bit,
	@PostType int, 
	@DateUpdated datetime,
	@BlogID int
)
as
Set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID],  blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig, blog_Content.EntryName FROM blog_Content
WHERE 
	blog_Content.PostType=@PostType and blog_Content.BlogID = @BlogID
	and blog_Content.DateUpdated > @DateUpdated
	and blog_Content.PostConfig & 1  <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.[dateupdated] desc












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO









Create    Proc blog_GetEntryWithCategoryTitles
(
	@PostID int,
	@IsActive bit,
	@BlogID int
)
as
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl, blog_Content.ParentID,
blog_Content.FeedBackCount, blog_Content.PostConfig, blog_Content.EntryName, blog_Content.ParentID FROM blog_Content
WHERE blog_Content.[ID] = @PostID and  blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.[dateadded] desc
Select c.Title, l.PostID From blog_Links l
inner join blog_LinkCategories c on l.CategoryID = c.CategoryID
where l.PostID = @PostID











GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO












CREATE   Proc blog_GetEntryWithCategoryTitlesByEntryName
(
	@EntryName nvarchar(150),
	@IsActive bit,
	@BlogID int
)
as
Declare @PostID int

SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl, blog_Content.ParentID,
blog_Content.FeedBackCount, blog_Content.PostConfig, blog_Content.EntryName FROM blog_Content
WHERE blog_Content.EntryName = @EntryName and  blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.[dateadded] desc

Select c.Title, l.PostID From blog_Links l
inner join blog_LinkCategories c on l.CategoryID = c.CategoryID
inner join blog_Content on l.PostID = blog_Content.[ID]
where blog_Content.EntryName = @EntryName












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO













CREATE          Proc blog_GetFeedBack 
(
	@ParentID int,
	@BlogID int
)
as

SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig, blog_Content.EntryName FROM blog_Content
WHERE blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 = 1 and blog_Content.ParentID = @ParentID
ORDER BY [ID]













GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE  Proc blog_GetImageCategory
(
	@CategoryID int,
	@IsActive bit,
	@BlogID int
)
as
exec blog_GetCategory @CategoryID, @IsActive, @BlogID
Select Title, CategoryID, Height, Width, [File], Active, ImageID From blog_Images  
where CategoryID = @CategoryID and BlogID = @BlogID and Active <> Case @IsActive When 1 then 0 Else -1 End
order by Title









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





CREATE  Proc blog_GetKeyWord
(
	@KeyWordID int,
	@BlogID int
)
as

Select 
	KeyWordID, Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID
From
	blog_keywords
Where 
	KeyWordID = @KeyWordID and BlogID = @BlogID





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE     Proc blog_GetLinkCollectionByPostID
(
	@PostID int,
	@BlogID int
)
as
SELECT blog_Links.LinkID, blog_Links.Title, blog_Links.Url, blog_Links.Rss, blog_Links.Active, blog_Links.CategoryID,  blog_Links.PostID, blog_Links.NewWindow 
FROM blog_Links
WHERE blog_Links.PostID=@PostID and blog_Links.BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE   Proc blog_GetLinksByActiveCategoryID
(
	@CategoryID int,
	@BlogID int
)
as
exec blog_GetCategory @CategoryID, 0, @BlogID
SELECT blog_Links.LinkID, blog_Links.Title, blog_Links.Url, blog_Links.Rss, blog_Links.Active, blog_Links.CategoryID,  blog_Links.PostID 
FROM blog_Links
WHERE blog_Links.Active=1 AND blog_Links.CategoryID=@CategoryID and blog_Links.BlogID = @BlogID
ORDER BY blog_Links.Title








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE     Proc blog_GetLinksByCategoryID
(
	@CategoryID int,
	@BlogID int
)
as
exec blog_GetCategory @CategoryID, @BlogID
SELECT blog_Links.LinkID, blog_Links.Title, blog_Links.Url, blog_Links.Rss, blog_Links.Active, blog_Links.NewWindow, blog_Links.CategoryID,  blog_Links.PostID
FROM blog_Links
WHERE blog_Links.CategoryID=@CategoryID and blog_Links.BlogID = @BlogID
ORDER BY blog_Links.Title








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE      PROC blog_GetPageableEntries --0, 1,10,3,1
(
	@BlogID int,
	@PageIndex int,
	@PageSize int,
	@PostType int,
	@SortDesc bit
	--@TotalRecords int output
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #TempPagedEntryIDs 
(
	TempID int IDENTITY (1, 1) NOT NULL,
	EntryID int NOT NULL
)	

IF NOT (@SortDesc = 1)
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	[ID] 
	FROM 	blog_Content 
	WHERE 	blogID = @BlogID 
			AND PostType = @PostType
	ORDER BY [ID]
END
ELSE
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	[ID] 
	FROM 	blog_Content
	WHERE 	blogID = @BlogID 
			AND PostType = @PostType
	ORDER BY [ID] DESC
END

SELECT	content.BlogID, 
		content.[ID], 
		content.Title, 
		content.DateAdded, 
		content.[Text], 
		content.[Description],
		content.SourceUrl, 
		content.PostType, 
		content.Author, 
		content.Email, 
		content.SourceName, 
		content.DateUpdated, 
		content.TitleUrl, 
		content.FeedbackCount,
		content.ParentID,
		content.PostConfig,
		content.EntryName,
		vc.WebCount,
		vc.AggCount,
		vc.WebLastUpdated,
		vc.AggLastUpdated
		
FROM  	blog_Content content
    	INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
	Left JOIN  blog_EntryViewCount vc ON (content.[ID] = vc.EntryID and vc.BlogID = @BlogID)
WHERE 	content.BlogID = @BlogID 
		AND tmp.TempID > @PageLowerBound 
		AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs


SELECT COUNT([ID]) as TotalRecords
FROM 	blog_Content 
WHERE 	blogID = @BlogID 
		AND PostType = @PostType 









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO










CREATE      PROC blog_GetPageableEntriesByCategoryID
(
	@BlogID int,
	@CategoryID int,
	@PageIndex int,
	@PageSize int,
	@PostType int,
	@SortDesc bit
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

-- ? Only Posts ?


CREATE TABLE #TempPagedEntryIDs 
(
	TempID int IDENTITY (1, 1) NOT NULL,
	EntryID int NOT NULL
)	

IF NOT (@SortDesc = 1)
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	blog.[ID] 
	FROM 	blog_Content blog
			INNER JOIN blog_Links links ON (blog.[ID] = links.PostID)
			INNER JOIN blog_LinkCategories cats ON (links.CategoryID = cats.CategoryID)
	WHERE 	blog.blogID = @BlogID 
			AND blog.PostType = @PostType
			AND cats.CategoryID = @CategoryID
	ORDER BY blog.[ID]
END
ELSE
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	blog.[ID] 
	FROM 	blog_Content blog
			INNER JOIN blog_Links links ON (blog.[ID] = links.PostID)
			INNER JOIN blog_LinkCategories cats ON (links.CategoryID = cats.CategoryID)
	WHERE 	blog.blogID = @BlogID 
			AND blog.PostType = @PostType
			AND cats.CategoryID = @CategoryID
	ORDER BY blog.[ID] DESC
END
	
 
SELECT	content.BlogID, 
		content.[ID], 
		content.Title, 
		content.DateAdded, 
		content.[Text], 
		content.[Description],
		content.SourceUrl, 
		content.PostType, 
		content.Author, 
		content.Email, 
		content.SourceName, 
		content.DateUpdated, 
		content.TitleUrl, 
		content.FeedbackCount,
		content.ParentID,
		content.PostConfig,
		content.EntryName,
		vc.WebCount,
		vc.AggCount,
		vc.WebLastUpdated,
		vc.AggLastUpdated

FROM	blog_Content content
    	INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
	Left JOIN  blog_EntryViewCount vc ON (content.[ID] = vc.EntryID and vc.BlogID = @BlogID)
WHERE	content.BlogID = @BlogID AND
		tmp.TempID > @PageLowerBound AND
		tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs

SELECT 	COUNT(blog.[ID]) as TotalRecords
FROM 	blog_Content blog
		INNER JOIN blog_Links links ON (blog.[ID] = links.PostID)
		INNER JOIN blog_LinkCategories cats ON (links.CategoryID = cats.CategoryID)
WHERE 	blog.blogID = @BlogID 
		AND blog.PostType = @PostType
		AND cats.CategoryID = @CategoryID









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE     PROC blog_GetPageableFeedback 
(
	@BlogID int,
	@PageIndex int,
	@PageSize int,
	@SortDesc bit
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #TempPagedEntryIDs 
(
	TempID int IDENTITY (1, 1) NOT NULL,
	EntryID int NOT NULL
)	

IF NOT (@SortDesc = 1)
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	[ID] 
	FROM 	blog_Content 
	WHERE 	blogID = @BlogID 
			AND (PostType = 3 or PostType = 4)

	ORDER BY [DateAdded]
END
ELSE
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	[ID] 
	FROM 	blog_Content
	WHERE 	blogID = @BlogID 
			AND (PostType = 3 or PostType = 4)
	ORDER BY [DateAdded] DESC
END

SELECT	content.BlogID, 
		content.[ID], 
		content.Title, 
		content.DateAdded, 
		content.[Text], 
		content.[Description],
		content.SourceUrl, 
		content.PostType, 
		content.Author, 
		content.Email, 
		content.SourceName, 
		content.DateUpdated, 
		content.TitleUrl, 
		content.FeedbackCount,
		content.ParentID,
		content.PostConfig,
		content.EntryName
FROM  	blog_Content content
    	INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
WHERE 	content.BlogID = @BlogID 
		AND tmp.TempID > @PageLowerBound 
		AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs



SELECT 	COUNT([ID]) as TotalRecords
FROM 	blog_Content 
WHERE 	blogID = @BlogID 
		AND (PostType = 3 or PostType = 4)






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO










CREATE    PROC blog_GetPageableKeyWords -- 0,1,2,1
(
	@BlogID int,
	@PageIndex int,
	@PageSize int,
	@SortDesc bit
--	@TotalRecords int output
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #TempPagedKeyWordIDs 
(
	TempID int IDENTITY (1, 1) NOT NULL,
	KeywordID int NOT NULL
)	

if(@SortDesc = 1)
BEGIN
	INSERT INTO #TempPagedKeyWordIDs (KeyWordID)
	SELECT	KeyWordID
	FROM 	blog_KeyWords 
	WHERE 	blogID = @BlogID 
	ORDER BY Word
END
Else
BEGIN
	INSERT INTO #TempPagedKeyWordIDs (KeyWordID)
	SELECT	KeyWordID
	FROM 	blog_KeyWords 
	WHERE 	blogID = @BlogID 
	ORDER BY Word desc
END

SELECT 	words.KeyWordID, 
	words.Word,
	words.[Text],
	words.ReplaceFirstTimeOnly,
	words.OpenInNewWindow,
	words.CaseSensitive,
	words.Url,
	words.Title,
	words.BlogID
FROM 	
	blog_KeyWords words
	INNER JOIN #TempPagedKeyWordIDs tmp ON (words.KeyWordID = tmp.KeyWordID)
WHERE 	
	words.blogID = @BlogID 
	AND tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY
	tmp.TempID
 
DROP TABLE #TempPagedKeyWordIDs

SELECT 	COUNT([KeywordID]) as 'TotalRecords'
FROM 	blog_KeyWords 
WHERE 	blogID = @BlogID





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE   PROC blog_GetPageableLinks
(
	@BlogID int,
	@PageIndex int,
	@PageSize int,
	@SortDesc bit
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
	FROM 	blog_Links 
	WHERE 	blogID = @BlogID 
		AND PostID = -1
	ORDER BY Title
END
ELSE
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM 	blog_Links
	WHERE 	blogID = @BlogID 
		AND PostID = -1
	ORDER BY [Title] DESC
END

SELECT 	links.LinkID, 
	links.Title, 
	links.Url,
	links.Rss, 
	links.Active, 
	links.NewWindow, 
	links.CategoryID,
	links.PostID
FROM 	
	blog_Links links
	INNER JOIN #TempPagedLinkIDs tmp ON (links.LinkID = tmp.LinkID)
WHERE 	
	links.blogID = @BlogID 
	AND tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY
	tmp.TempID
 
DROP TABLE #TempPagedLinkIDs

SELECT 	COUNT([LinkID]) as TotalRecords
FROM 	blog_Links 
WHERE 	blogID = @BlogID
	AND PostID = -1






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE    PROC blog_GetPageableLinksByCategoryID
(
	@BlogID int,
	@CategoryID int,
	@PageIndex int,
	@PageSize int,
	@SortDesc bit
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
	FROM 	blog_Links 
	WHERE 	blogID = @BlogID 
		AND CategoryID = @CategoryID
		AND PostID = -1
	ORDER BY Title
END
ELSE
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM 	blog_Links
	WHERE 	blogID = @BlogID 
		AND CategoryID = @CategoryID
		AND PostID = -1
	ORDER BY Title DESC
END

SELECT 	links.LinkID, 
	links.Title, 
	links.Url,
	links.Rss, 
	links.Active, 
	links.NewWindow, 
	links.CategoryID,  
	links.PostID
FROM 	
	blog_Links links
	INNER JOIN #TempPagedLinkIDs tmp ON (links.LinkID = tmp.LinkID)
WHERE 	
	links.blogID = @BlogID 
	AND links.CategoryID = @CategoryID
	AND tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY
	tmp.TempID
 
DROP TABLE #TempPagedLinkIDs


SELECT  COUNT([LinkID]) as TotalRecords
FROM 	blog_Links 
WHERE 	blogID = @BlogID 
	AND CategoryID = @CategoryID 
	AND PostID = -1






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





--Select Top 5 * FROM blog_Referrals order by LastUpdated desc

CREATE     Proc blog_GetPageableReferrers-- 0, 1, 20
(
	@BlogID INT,
	@PageIndex INT,
	@PageSize INT
)
AS


DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #tempblog_Referrals 
(
	TempID INT IDENTITY(1, 1) NOT NULL,
	[EntryID] [int] NOT NULL ,
	[UrlID] [int] NOT NULL ,
	[Count] [int] NOT NULL ,
	[LastUpdated] [datetime] NOT NULL
) 




INSERT INTO #tempblog_Referrals (EntryID,UrlID, [Count], LastUpdated)
  SELECT EntryID, UrlID, [Count], LastUpdated
  FROM blog_Referrals
  WHERE blog_Referrals.BlogID = @BlogID
  Order by LastUpdated desc
   


SELECT	u.URL,
	c.Title,
	r.EntryID,
	c.EntryName,
	LastUpdated,
	[Count]
FROM 	blog_Content c,
	#tempblog_Referrals r,
	blog_URLs u
WHERE r.EntryID = c.ID and
      c.BlogID = @BlogID
  AND r.UrlID = u.UrlID
  AND r.TempID > @PageLowerBound
  AND r.TempID < @PageUpperBound

Order by TempID



SELECT COUNT(*) as 'TotalRecords' FROM #tempblog_Referrals





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO






--blog_GetPageableReferrersByEntryID 0, 7043, 1, 10

CREATE     Proc blog_GetPageableReferrersByEntryID 
(
	@BlogID INT,
	@EntryID int,
	@PageIndex INT,
	@PageSize INT
)
AS


DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #tempblog_Referrals 
(
	TempID INT IDENTITY(1, 1) NOT NULL,
	[UrlID] [int] NOT NULL ,
	[Count] [int] NOT NULL ,
	[LastUpdated] [datetime] NOT NULL
) 




INSERT INTO #tempblog_Referrals (UrlID, [Count], LastUpdated)
  SELECT UrlID, [Count], LastUpdated
  FROM blog_Referrals
  WHERE blog_Referrals.BlogID = @BlogID and blog_Referrals.EntryID = @EntryID
  Order by LastUpdated desc
   


SELECT	u.URL,
	c.Title,
	c.EntryName,
	@EntryID as 'EntryID',
	LastUpdated,
	[Count]
FROM 	blog_Content c,
	#tempblog_Referrals r,
	blog_URLs u
WHERE c.ID = @EntryID and 
      c.BlogID = @BlogID
  AND r.UrlID = u.UrlID
  AND r.TempID > @PageLowerBound
  AND r.TempID < @PageUpperBound
  Order by TempID



SELECT COUNT(*) as 'TotalRecords' FROM #tempblog_Referrals












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO









--blog_GetPostsByCategoryID 15,72,0,0



CREATE           Proc blog_GetPostsByCategoryID -- 15,72,0
(
	@ItemCount int,
	@CategoryID int,
	@IsActive bit,
	@BlogID int
)
as
Set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig,
blog_Content.EntryName FROM blog_Content with(nolock)
INNER JOIN blog_Links with(nolock) on blog_Content.ID = blog_Links.PostID
INNER JOIN blog_LinkCategories with(nolock) on blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE  blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End and blog_LinkCategories.CategoryID = @CategoryID
ORDER BY blog_Content.[ID] desc













GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO












CREATE          Proc blog_GetPostsByCategoryIDByDateUpdated
(
	@ItemCount int,
	@CategoryID int,
	@IsActive bit,
	@DateUpdated datetime,
	@BlogID int
)
as
Set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig, 
blog_Content.EntryName FROM blog_Content
INNER JOIN blog_Links on blog_Content.ID = blog_Links.PostID
INNER JOIN blog_LinkCategories on blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE  blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End and blog_LinkCategories.CategoryID = @CategoryID and blog_Content.DateUpdated > @DateUpdated
ORDER BY blog_Content.[ID] desc












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO













CREATE          Proc blog_GetPostsByCategoryName -- 15,72,0
(
	@ItemCount int,
	@CategoryName nvarchar(150),
	@IsActive bit,
	@BlogID int
)
as
Set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig,
blog_Content.EntryName FROM blog_Content
INNER JOIN blog_Links on blog_Content.ID = blog_Links.PostID
INNER JOIN blog_LinkCategories on blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE  blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End 
and blog_LinkCategories.Title = @CategoryName
ORDER BY blog_Content.[ID] desc













GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO













CREATE          Proc blog_GetPostsByCategoryNameByDateUpdated
(
	@ItemCount int,
	@CategoryName nvarchar(150),
	@IsActive bit,
	@DateUpdated datetime,
	@BlogID int
)
as
Set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig, 
blog_Content.EntryName FROM blog_Content
INNER JOIN blog_Links on blog_Content.ID = blog_Links.PostID
INNER JOIN blog_LinkCategories on blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE  blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End 
and blog_LinkCategories.Title = @CategoryName and blog_Content.DateUpdated > @DateUpdated
ORDER BY blog_Content.[ID] desc













GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO











CREATE          Proc blog_GetPostsByDayRange
(
	@StartDate datetime,
	@StopDate datetime,
	@BlogID int
)
as
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig,
blog_Content.EntryName FROM blog_Content
WHERE 
	(blog_Content.DateAdded > @StartDate and blog_Content.DateAdded < DateAdd(day,1,@StopDate))
And blog_Content.PostType=1 and blog_Content.BlogID = @BlogID
ORDER BY blog_Content.DateAdded DESC;











GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO












CREATE            Proc blog_GetPostsByMonth -- 2,2003,0
(
	@Month int,
	@Year int,
	@BlogID int
)
as
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig,
blog_Content.EntryName FROM blog_Content
WHERE blog_Content.PostType=1 and blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 = 1 
	and Month(blog_Content.DateAdded)  = @Month and Year(blog_Content.DateAdded)  = @Year
ORDER BY blog_Content.DateAdded desc












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE   Proc blog_GetPostsByMonthArchive 
(
	@BlogID int
)
as
Select Month(DateAdded) as [Month], Year(DateAdded) as [Year], 1 as Day, Count(*) as [Count] From blog_Content 
where PostType = 1 and PostConfig & 1 = 1 and BlogID = @BlogID 
Group by Year(DateAdded), Month(DateAdded) order by [Year] desc, [Month] desc




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE   Proc blog_GetPostsByYearArchive 
(
	@BlogID int
)
as
Select 1 as [Month], Year(DateAdded) as [Year], 1 as Day, Count(*) as [Count] From blog_Content 
where PostType = 1 and PostConfig & 1 = 1 and BlogID = @BlogID 
Group by Year(DateAdded) order by [Year] desc




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO












CREATE         Proc blog_GetRecentEntries
(
	@ItemCount int,
	@IsActive bit,
	@PostType int,
	@BlogID int
)
as
Set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, Blog_Content.PostConfig,
blog_Content.EntryName FROM blog_Content
WHERE blog_Content.PostType=@PostType and blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.[dateadded] desc












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO






CREATE       Proc blog_GetRecentEntriesByDateUpdated
(
	@ItemCount int,
	@IsActive bit, 
	@PostType int,
	@DateUpdated datetime,
	@BlogID int
)
as
Set rowcount @ItemCount
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig,
blog_Content.EntryName FROM blog_Content
WHERE 
	blog_Content.PostType=@PostType and blog_Content.BlogID = @BlogID
	and blog_Content.DateUpdated > @DateUpdated
	and blog_Content.PostConfig & 1  <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.[ID] desc












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





CREATE         Proc blog_GetRecentEntriesWithCategoryTitles
(
	@ItemCount int,
	@IsActive bit,
	@BlogID int
)
as
Set rowcount @ItemCount
Create Table #IDs
(
	TempID int IDENTITY (0, 1) NOT NULL,
	PostID int not null
)
Insert #IDs (PostID)
SELECT blog_Content.[ID] FROM blog_Content
WHERE blog_Content.PostType=1 and blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.[dateadded] desc
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig,
blog_Content.EntryName FROM blog_Content, #IDs
WHERE blog_Content.[ID] = #IDs.PostID
order by TempID asc
Set rowcount 0
Select c.Title, l.PostID From blog_Links l
inner join #IDs on l.[PostID] = #IDs.[PostID]
inner join blog_LinkCategories c on l.CategoryID = c.CategoryID
DROP Table #IDs











GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE       Proc blog_GetSingleDay --'5/29/2003', 0
(
	@Date datetime,
	@BlogID int
)
as
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig,
blog_Content.EntryName FROM blog_Content
WHERE Year(blog_Content.DateAdded)=Year(@Date) And Month(blog_Content.DateAdded)=Month(@Date)
      And Day(blog_Content.DateAdded)=Day(@Date) And blog_Content.PostType=1
      And blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 = 1 
ORDER BY blog_Content.DateAdded DESC;











GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO





CREATE        Proc blog_GetSingleEntry
(
	@ID int,
	@IsActive bit,
	@BlogID int
)
as
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig, 
blog_Content.EntryName FROM blog_Content
WHERE blog_Content.[ID] = @ID and blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.[ID] desc




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



Create Proc blog_GetSingleEntryByName
(
	@EntryName nvarchar(150),
	@IsActive bit,
	@BlogID int
)
as
SELECT blog_Content.BlogID, blog_Content.[ID], blog_Content.Title, blog_Content.DateAdded, blog_Content.[Text], blog_Content.[Description],
blog_Content.SourceUrl, blog_Content.PostType, blog_Content.Author, blog_Content.Email, blog_Content.SourceName, blog_Content.DateUpdated, blog_Content.TitleUrl,
blog_Content.FeedBackCount, blog_Content.ParentID, blog_Content.PostConfig, 
blog_Content.EntryName FROM blog_Content
WHERE blog_Content.[EntryName] = @EntryName and blog_Content.BlogID = @BlogID and blog_Content.PostConfig & 1 <> Case @IsActive When 1 then 0 Else -1 End
ORDER BY blog_Content.[ID] desc



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








Create Proc blog_GetSingleImage
(
	@ImageID int,
	@IsActive bit, 
	@BlogID int
)
as
Select Title, CategoryID, Height, Width, [File], Active, ImageID From blog_Images  
where ImageID = @ImageID and BlogID = @BlogID and  Active <> Case @IsActive When 1 then 0 Else -1 End








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE    Proc blog_GetSingleLink
(
	@LinkID int,
	@BlogID int
)
as
SELECT blog_Links.LinkID, blog_Links.Title, blog_Links.Url, blog_Links.Rss, blog_Links.Active, blog_Links.NewWindow, blog_Links.CategoryID,  blog_Links.PostID 
FROM blog_Links
WHERE blog_Links.LinkID=@LinkID and blog_Links.BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








Create Proc blog_GetUrlID
(
	@Url nvarchar(255),
	@UrlID int output
)
as
if exists(Select UrlID From blog_Urls where Url = @Url)
Begin
	Select @UrlID = UrlID From blog_Urls where Url = @Url
End
Else
Begin
	Insert blog_Urls Values (@Url)
	Select @UrlID = @@Identity
End








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE    Proc blog_InsertCategory
(
	@Title nvarchar(150),
	@Active bit,
	@BlogID int,
	@CategoryType tinyint,
	@Description nvarchar(1000),
	@CategoryID int output
)
as
Set NoCount On
INSERT INTO blog_LinkCategories ( Title, Active, CategoryType, [Description], BlogID )
VALUES (@Title,@Active, @CategoryType, @Description, @BlogID)
Select @CategoryID = @@Identity









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC blog_UpdateConfig
(
	@UserName nvarchar(50),
	@Password nvarchar(50),
	@Email nvarchar(50),
	@Title nvarchar(100),
	@SubTitle nvarchar(250),
	@Skin nvarchar(50),
	@Application nvarchar(50),
	@Host nvarchar(100),
	@Author nvarchar(100),
	@Language nvarchar(10),
	@TimeZone int,
	@ItemCount int,
	@News nText,
	@LastUpdated datetime,
	@SecondaryCss nText,
	@SkinCssFile varchar(100),
	@Flag int,
	@BlogID int
)
as
Update blog_Config
Set
	UserName  =    @UserName,     
	[Password]  =  @Password ,    
	Email	   =   @Email,        
	Title	   =   @Title ,       
	SubTitle   =   @SubTitle  ,   
	Skin	  =    @Skin   ,      
	Application =  @Application , 
	Host	  =    @Host  ,       
	Author	   =   @Author,
	[Language] = @Language,
	TimeZone   = @TimeZone,
	ItemCount = @ItemCount,
	News      = @News,
	LastUpdated = @LastUpdated,
	Flag = @Flag,
	SecondaryCss = @SecondaryCss,
	SkinCssFile = @SkinCssFile
Where 
	BlogID = @BlogID









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








Create Proc blog_UpdateConfigUpdateTime
(
	@BlogID int,
	@LastUpdated datetime
)
as
Update blog_Config
Set LastUpdated = @LastUpdated
where blogid = @blogid








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO










CREATE   Proc blog_InsertEntry
(
	@Title nvarchar(255),
	@TitleUrl nvarchar(255),	
	@Text ntext,
	@SourceUrl nvarchar(200),
	@PostType int,
	@Author nvarchar(50),
	@Email nvarchar(50),
	@SourceName nvarchar(200),
	@Description nvarchar(500),
	@BlogID int,
	@DateAdded datetime,
	@ParentID int,
	@PostConfig int,
	@EntryName nvarchar(150),
	@ID int output)
as

if(@EntryName is not null)
Begin
	if exists(Select EntryName From blog_Content where BlogID = @BlogID and EntryName = @EntryName)
	Begin
		RAISERROR('The EntryName you entry is already in use with in this Blog. Please pick a unique EntryName.',11,1) 
		RETURN 1
	End
End
if(Ltrim(Rtrim(@Description)) = '')
set @Description = null
INSERT INTO blog_Content 
(Title, TitleUrl, [Text], SourceUrl, PostType, Author, Email, DateAdded,DateUpdated, SourceName, [Description], PostConfig, ParentID, BlogID, EntryName )
VALUES 
(@Title, @TitleUrl, @Text, @SourceUrl, @PostType, @Author, @Email, @DateAdded, @DateAdded, @SourceName, @Description, @PostConfig, @ParentID, @BlogID, @EntryName)
Select @ID = @@Identity

if(@PostType = 1 or @PostType = 2)
Begin
	exec blog_UpdateConfigUpdateTime @blogID, @DateAdded
End
Else if(@PostType = 3)
Begin
	Update blog_Content
	Set FeedBackCount = FeedBackCount + 1 where [ID] = @ParentID
End













GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE Proc blog_InsertEntryViewCount-- 1, 0, 1
(
	@EntryID int,
	@BlogID int,
	@IsWeb bit
)

as

Begin
	--Do we have an existing entry in the blog_InsertEntryViewCount table?
	if exists(Select EntryID From blog_EntryViewCount where EntryID = @EntryID and BlogID = @BlogID)
	begin
		if(@IsWeb = 1) -- Is this a web view?
		begin
			Update blog_EntryViewCount
			Set [WebCount] = [WebCount] + 1, WebLastUpdated = getdate()
			where EntryID = @EntryID and BlogID = @BlogID
		end
		else
		begin
			Update blog_EntryViewCount
			Set [AggCount] = [AggCount] + 1, AggLastUpdated = getdate()
			where EntryID = @EntryID and BlogID = @BlogID
		end
	end
	else
	begin
		if(@IsWeb = 1) -- Is this a web view
		begin
			Insert blog_EntryViewCount (EntryID, BlogID, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogID, 1, 0, getdate(), null)
		end
		else
		begin
			Insert blog_EntryViewCount (EntryID, BlogID, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogID, 0, 1, null, getdate())
		end
	end


End




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








Create Proc blog_InsertImage
(
	@Title nvarchar(250),
	@CategoryID int,
	@Width int,
	@Height int,
	@File nvarchar(50),
	@Active bit,
	@BlogID int,
	@ImageID int output
)
as
Insert blog_Images
(
	Title, CategoryID, Width, Height, [File], Active, BlogID
)
Values
(
	@Title, @CategoryID, @Width, @Height, @File, @Active, @BlogID
)
Set @ImageID = @@Identity








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





CREATE  Proc blog_InsertKeyWord
(
	@Word nvarchar(100),
	@Text nvarchar(100),
	@ReplaceFirstTimeOnly bit,
	@OpenInNewWindow bit,
	@CaseSensitive bit,
	@Url nvarchar(255),
	@Title nvarchar(100),
	@BlogID int,
	@KeyWordID int output
)

as

Insert blog_keywords 
	(Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID)
Values
	(@Word,@Text,@ReplaceFirstTimeOnly,@OpenInNewWindow, @CaseSensitive,@Url,@Title,@BlogID)

Select @KeyWordID = @@Identity






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE    Proc blog_InsertLink
(
	@Title nvarchar(150),
	@Url nvarchar(255),
	@Rss nvarchar(255),
	@Active bit,
	@NewWindow bit,
	@CategoryID int,
	@PostID int,
	@BlogID int,
	@LinkID int output
)
as
INSERT INTO blog_Links 
( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogID )
VALUES 
(@Title, @Url, @Rss, @Active, @NewWindow, @PostID, @CategoryID, @BlogID);
Select @LinkID = @@Identity








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE    Proc blog_InsertLinkCategoryList
(
	@CategoryList nvarchar(4000),
	@PostID int,
	@BlogID int
)
as

--Delete categories that have been removed
Delete blog_Links From blog_Links
where 
	CategoryID not in (Select str From iter_charlist_to_table(@CategoryList,','))
And 
	BlogID = @BlogID and PostID = @PostID

--Add updated/new categories
INSERT INTO blog_Links ( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogID )
Select null, null, null, 1, 0, @PostID, Convert(int, [str]), @BlogID
From iter_charlist_to_table(@CategoryList,',')
where 
	Convert(int, [str]) not in (Select CategoryID From blog_Links where PostID = @PostID and BlogID = @BlogID)



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO














CREATE     Proc blog_InsertPingTrackEntry
(
	@Title nvarchar(255),
	@TitleUrl nvarchar(255),	
	@Text ntext,
	@SourceUrl nvarchar(200),
	@PostType int,
	@Author nvarchar(50),
	@Email nvarchar(50),
	@SourceName nvarchar(200),
	@Description nvarchar(500),
	@BlogID int,
	@DateAdded datetime,
	@ParentID int,
	@PostConfig int,
	@EntryName nvarchar(150),
	@ID int output)
as

--Do not insert EntryNames. No needed for comments and tracks. To messy anyway

Set @ID = -1

if not exists (Select [ID] From blog_Content where TitleUrl = @TitleUrl and ParentID = @ParentID)
Begin

if(Ltrim(Rtrim(@Description)) = '')
set @Description = null
INSERT INTO blog_Content 
( PostConfig, Title, TitleUrl, [Text], SourceUrl, PostType, Author, Email, DateAdded,DateUpdated, SourceName, [Description], ParentID, BlogID)
VALUES 
(@PostConfig, @Title, @TitleUrl, @Text, @SourceUrl, @PostType, @Author, @Email, @DateAdded, @DateAdded, @SourceName, @Description, @ParentID, @BlogID)

Select @ID = @@Identity

Update blog_Content
Set FeedBackCount = FeedBackCount + 1 
where [ID] = @ParentID

End














GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO







CREATE   Proc blog_InsertPostCategoryByName
(
	@Title nvarchar(150),
	@PostID int,
	@BlogID int
)
as
Declare @CategoryID int
Select @CategoryID = CategoryID From blog_LinkCategories where Title = @Title and BlogID = @BlogID and CategoryType = 1

if(@CategoryID is null)
Begin

exec blog_InsertCategory @Title, 1, @BlogID, 1, null, @CategoryID = @CategoryID output

End

Declare @LinkID int
exec blog_InsertLink null, null, null, 1, 0, @CategoryID, @PostID, @BlogID, @LinkID output










GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



Create Proc blog_InsertReferral
(
	@EntryID int,
	@BlogID int,
	@Url nvarchar(255)
)

as

Declare @UrlID int

if(@Url is not null)
Begin
	exec blog_GetUrlID @Url, @UrlID = @UrlID output
End

if(@UrlID is not null)
Begin

	if exists(Select EntryID From blog_Referrals where EntryID = @EntryID and BlogID = @BlogID and UrlID = @UrlID)
	begin
		Update blog_Referrals
		Set [Count] = [Count] + 1, LastUpdated = getdate()
		where EntryID = @EntryID and BlogID = @BlogID and UrlID = @UrlID
	end
	else
	begin
		Insert blog_Referrals (EntryID, BlogID, UrlID, [Count], LastUpdated)
		       values (@EntryID, @BlogID, @UrlID, 1, getdate())
	end


End



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE  Proc blog_InsertViewStats
(
	@BlogID int,
	@PageType tinyint,
	@PostID int,
	@Day datetime,
	@Url nvarchar(255)
)
as

Declare @UrlID int

if(@Url is not null)
Begin
	exec blog_GetUrlID @Url, @UrlID = @UrlID output
End
if(@UrlID is null)
set @UrlID = -1


if exists (Select BlogID from blog_ViewStats where BlogID = @BlogID and PageType = @PageType and PostID = @PostID and [Day] = @Day and UrlID = @UrlID)
Begin
	Update blog_ViewStats
	Set [Count] = [Count] + 1
	Where BlogID = @BlogID and PageType = @PageType and PostID = @PostID and [Day] = @Day and UrlID = @UrlID
End
Else
Begin
	Insert blog_ViewStats (BlogID, PageType, PostID, [Day], UrlID, [Count])
	Values (@BlogID, @PageType, @PostID, @Day, @UrlID, 1)
End







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



Create Proc blog_StatsSummary
(
	@BlogID int
)
as
Declare @ReferralCount int
Declare @WebCount int
Declare @AggCount int

Select @ReferralCount = Sum([Count]) From blog_Referrals where BlogID = @BlogID

Select @WebCount = Sum(WebCount), @AggCount = Sum(AggCount) From blog_EntryViewCount where BlogID = @BlogID

Select @ReferralCount as 'ReferralCount', @WebCount as 'WebCount', @AggCount as 'AggCount'




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE  Proc blog_TrackEntry
(
	@EntryID int,
	@BlogID int,
	@Url nvarchar(255),
	@IsWeb bit
)

as

if(@Url is not null and @IsWeb = 1)
begin
	exec blog_InsertReferral @EntryID, @BlogID, @Url
end

exec blog_InsertEntryViewCount @EntryID, @BlogID, @IsWeb




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Subtext 1.0 will only support single user blogs, thus this 
proc will make sure there is only one blog in the system 
and will fail to add a blog if one already exists.
*/
CREATE PROC blog_UTILITY_AddBlog
(
	@UserName nvarchar(50),
	@Password nvarchar(50),
	@Email nvarchar(50),
	@Host nvarchar(50),
	@Application nvarchar(50),
	@IsHashed bit
)

as

IF(NOT EXISTS (SELECT * FROM blog_config))
BEGIN
	Declare @Flag int
	Set @Flag = 55
	if(@IsHashed = 1)
	Set @Flag = 63

	INSERT blog_Config  
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
		, 'Subtext Blog'
		, 'Another Subtext Powered Blog'
		, 'marvin2'
		, 'blue.css'
		, @Application
		, @Host
		, 'Blog Author'
		, -5
		,'en-US'
		, 10
		, @Flag)

END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE     Proc blog_UpdateCategory
(
	@CategoryID int,
	@Title nvarchar(150),
	@Active bit,
	@CategoryType tinyint,
	@Description nvarchar(1000),
	@BlogID int
)
as
UPDATE blog_LinkCategories 
SET 
	blog_LinkCategories.Title = @Title, 
	blog_LinkCategories.Active = @Active,
	blog_LinkCategories.CategoryType = @CategoryType,
	blog_LinkCategories.[Description] = @Description
WHERE   
	blog_LinkCategories.CategoryID=@CategoryID and blog_LinkCategories.BlogID = @BlogID









GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO












CREATE            Proc blog_UpdateEntry
(
	@ID int,
	@Title nvarchar(255),
	@TitleUrl nvarchar(255),
	@Text ntext,
	@SourceUrl nvarchar(200),
	@PostType int,
	@Author nvarchar(50),
	@Email nvarchar(50),
	@Description nvarchar(500),
	@SourceName nvarchar(200),
	@DateUpdated datetime,
	@PostConfig int,
	@ParentID int,
	@EntryName nvarchar(150),
	@BlogID int
)
as

if(@EntryName is not null)
Begin
	if exists(Select EntryName From blog_Content where BlogID = @BlogID and EntryName = @EntryName and [ID] <> @ID)
	Begin
		RAISERROR('The EntryName you entry is already in use with in this Blog. Please pick a unique EntryName.',11,1) 
		RETURN 1
	End
End

if(Ltrim(Rtrim(@Description)) = '')
set @Description = null
UPDATE blog_Content 
SET 
	blog_Content.Title = @Title, 
	blog_Content.TitleUrl = @TitleUrl, 
	blog_Content.[Text] = @Text, 
	blog_Content.SourceUrl = @SourceUrl, 
	blog_Content.PostType = @PostType,
	blog_Content.Author = @Author, 
	blog_Content.Email = @Email, 
	blog_Content.[Description] = @Description,
	blog_Content.DateUpdated = @DateUpdated,
	blog_Content.PostConfig = @PostConfig,
	blog_Content.ParentID = @ParentID,
	blog_Content.SourceName = @SourceName,
	blog_Content.EntryName = @EntryName
WHERE 	
	blog_Content.[ID]=@ID and blog_Content.BlogID = @BlogID
exec blog_UpdateConfigUpdateTime @blogID, @DateUpdated












GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE  Proc blog_UpdateImage
(
	@Title nvarchar(250),
	@CategoryID int,
	@Width int,
	@Height int,
	@File nvarchar(50),
	@Active bit,
	@BlogID int,
	@ImageID int
)
as
Update blog_Images
Set
	Title = @Title,
	CategoryID = @CategoryID,
	Width = @Width,
	Height = @Height,
	[File] = @File,
	Active = @Active
	
Where
	ImageID = @ImageID and BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





CREATE  Proc blog_UpdateKeyWord
(
	@KeyWordID int,
	@Word nvarchar(100),
	@Text nvarchar(100),
	@ReplaceFirstTimeOnly bit,
	@OpenInNewWindow bit,
	@CaseSensitive bit,
	@Url nvarchar(255),
	@Title nvarchar(100),
	@BlogID int
)

as

Update blog_keywords 
	Set
		Word = @Word,
		[Text] = @Text,
		ReplaceFirstTimeOnly = @ReplaceFirstTimeOnly,
		OpenInNewWindow = @OpenInNewWindow,
		CaseSensitive = @CaseSensitive,
		Url = @Url,
		Title = @Title
	Where
		BlogID = @BlogID and KeyWordID = @KeyWordID






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO








CREATE   Proc blog_UpdateLink
(
	@LinkID int,
	@Title nvarchar(150),
	@Url nvarchar(255),
	@Rss nvarchar(255),
	@Active bit,
	@NewWindow bit,
	@CategoryID int,
	@BlogID int
	
)
as
UPDATE blog_Links 
SET 
	blog_Links.Title = @Title, 
	blog_Links.Url = @Url, 
	blog_Links.Rss = @Rss, 
	blog_Links.Active = @Active,
	blog_Links.NewWindow = @NewWindow, 
	blog_Links.CategoryID = @CategoryID
WHERE  
	blog_Links.LinkID=@LinkID and blog_Links.BlogID = @BlogID








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



Create Proc blog_Utility_GetUnHashedPasswords
as

Select BlogID, Password FROM blog_COnfig where Flag & 8 = 0



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



Create Proc blog_Utility_UpdateToHashedPassword
(
	@Password nvarchar(100),
	@BlogID int
)

as

Update blog_Config
Set 
	Password = @Password,
	Flag = Flag | 8 
where blogid = @blogid


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



--Found at: http://www.algonet.se/~sommar/arrays-in-sql.html
  CREATE FUNCTION iter_charlist_to_table
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

