
/*
WARNING: This SCRIPT USES SQL TEMPLATE PARAMETERS.
Be sure to hit CTRL+SHIFT+M in Query Analyzer if running manually.
*/

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[<dbUser,varchar,dbo>].[FK_subtext_Feedback_subtext_Config]') AND type = 'F')
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_FeedBack] DROP CONSTRAINT [FK_subtext_Feedback_subtext_Config]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[<dbUser,varchar,dbo>].[FK_subtext_Feedback_subtext_Content]') AND type = 'F')
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_FeedBack] DROP CONSTRAINT [FK_subtext_Feedback_subtext_Content]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[<dbUser,varchar,dbo>].[subtext_FeedBack]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
DROP TABLE [<dbUser,varchar,dbo>].[subtext_FeedBack]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[PK_subtext_Log]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] DROP CONSTRAINT PK_subtext_Log
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Log_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] DROP CONSTRAINT FK_subtext_Log_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Log]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Log]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Version]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Version]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Host]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Host]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Content_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] DROP CONSTRAINT FK_subtext_Content_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_EntryViewCount_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryViewCount] DROP CONSTRAINT FK_subtext_EntryViewCount_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Images_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Images] DROP CONSTRAINT FK_subtext_Images_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_KeyWords_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords] DROP CONSTRAINT FK_subtext_KeyWords_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_LinkCategories_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_LinkCategories] DROP CONSTRAINT FK_subtext_LinkCategories_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Referrals_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Referrals] DROP CONSTRAINT FK_subtext_Referrals_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_Content]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_Content
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Images_subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Images] DROP CONSTRAINT FK_subtext_Images_subtext_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Referrals_subtext_URLs]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Referrals] DROP CONSTRAINT FK_subtext_Referrals_subtext_URLs
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Content_Trigger]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [<dbUser,varchar,dbo>].[subtext_Content_Trigger]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Config]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Config]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Content]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Content]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_EntryViewCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_EntryViewCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Images]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Images]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_KeyWords]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_KeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_LinkCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Links]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Links]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Referrals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Referrals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_URLs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_URLs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[spamPostCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[spamPostCount]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Config] (
	[BlogId] [int] IDENTITY (0, 1) NOT NULL ,
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
	[BlogGroup] [int] NULL ,
	[LicenseUrl] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[DaysTillCommentsClose] [int] NULL ,
	[CommentDelayInMinutes] [int] NULL ,
	[NumberOfRecentComments] [int] NULL,
	[RecentCommentsLength] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Content] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Title] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateAdded] [smalldatetime] NOT NULL ,
	[SourceUrl] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PostType] [int] NOT NULL ,
	[Author] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[SourceName] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[BlogId] [int] NULL ,
	[Description] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[DateUpdated] [smalldatetime] NULL ,
	[TitleUrl] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Text] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ParentID] [int] NULL ,
	[FeedBackCount] [int] NULL ,
	[PostConfig] [int] NULL ,
	[EntryName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ContentChecksumHash] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[DateSyndicated] [DateTime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_EntryViewCount] (
	[EntryID] [int] NOT NULL ,
	[BlogId] [int] NOT NULL ,
	[WebCount] [int] NOT NULL ,
	[AggCount] [int] NOT NULL ,
	[WebLastUpdated] [datetime] NULL ,
	[AggLastUpdated] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Images] (
	[ImageID] [int] IDENTITY (1, 1) NOT NULL ,
	[Title] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[CategoryID] [int] NOT NULL ,
	[Width] [int] NOT NULL ,
	[Height] [int] NOT NULL ,
	[File] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Active] [bit] NOT NULL ,
	[BlogId] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords] (
	[KeyWordID] [int] IDENTITY (1, 1) NOT NULL ,
	[Word] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Text] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ReplaceFirstTimeOnly] [bit] NOT NULL ,
	[OpenInNewWindow] [bit] NOT NULL ,
	[Url] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Title] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[BlogId] [int] NOT NULL ,
	[CaseSensitive] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_LinkCategories] (
	[CategoryID] [int] IDENTITY (1, 1) NOT NULL ,
	[Title] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Active] [bit] NOT NULL ,
	[BlogId] [int] NOT NULL ,
	[CategoryType] [tinyint] NULL ,
	[Description] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Links] (
	[LinkID] [int] IDENTITY (1, 1) NOT NULL ,
	[Title] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Url] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Rss] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Active] [bit] NOT NULL ,
	[CategoryID] [int] NULL ,
	[BlogId] [int] NOT NULL ,
	[PostID] [int] NULL ,
	[NewWindow] [bit] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Referrals] (
	[EntryID] [int] NOT NULL ,
	[BlogId] [int] NOT NULL ,
	[UrlID] [int] NOT NULL ,
	[Count] [int] NOT NULL ,
	[LastUpdated] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_URLs] (
	[UrlID] [int] IDENTITY (1, 1) NOT NULL ,
	[URL] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [<dbUser,varchar,dbo>].[spamPostCount] (
	[theCount] [int] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_Config] PRIMARY KEY  CLUSTERED 
	(
		[BlogId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_Content] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Images] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_Images] PRIMARY KEY  CLUSTERED 
	(
		[ImageID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_KeyWords] PRIMARY KEY  CLUSTERED 
	(
		[KeyWordID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_LinkCategories] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_LinkCategories] PRIMARY KEY  CLUSTERED 
	(
		[CategoryID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Links] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_Links] PRIMARY KEY  CLUSTERED 
	(
		[LinkID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_URLs] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_URLs] PRIMARY KEY  CLUSTERED 
	(
		[UrlID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] WITH NOCHECK ADD 
	CONSTRAINT [DF_subtext_Config_TimeZone] DEFAULT (0) FOR [TimeZone],
	CONSTRAINT [DF__subtext_Conf__IsAct__047AA831] DEFAULT (1) FOR [IsActive],
	CONSTRAINT [DF_subtext_Config_Language] DEFAULT ('en-US') FOR [Language],
	CONSTRAINT [DF__subtext_Conf__ItemC__0662F0A3] DEFAULT (15) FOR [ItemCount],
	CONSTRAINT [DF__subtext_Conf__PostC__5D60DB10] DEFAULT (0) FOR [PostCount],
	CONSTRAINT [DF__subtext_Conf__Story__5E54FF49] DEFAULT (0) FOR [StoryCount],
	CONSTRAINT [DF__subtext_Conf__PingT__5F492382] DEFAULT (0) FOR [PingTrackCount],
	CONSTRAINT [DF__subtext_Conf__Comme__603D47BB] DEFAULT (0) FOR [CommentCount],
	CONSTRAINT [DF__subtext_Conf__IsAgg__61316BF4] DEFAULT (1) FOR [IsAggregated],
	CONSTRAINT [IX_subtext_Config] UNIQUE  NONCLUSTERED 
	(
		[Application],
		[Host]
	)  ON [PRIMARY] 
GO

 CREATE  UNIQUE  INDEX [IX_subtext_Config_HostApplication] ON [<dbUser,varchar,dbo>].[subtext_Config]([BlogId], [Host], [Application]) WITH  FILLFACTOR = 90 ON [PRIMARY]
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] ADD 
	CONSTRAINT [FK_subtext_Content_subtext_Config] FOREIGN KEY 
	(
		[BlogId]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
		[BlogId]
	)
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryViewCount] ADD 
	CONSTRAINT [FK_subtext_EntryViewCount_subtext_Config] FOREIGN KEY 
	(
		[BlogId]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
		[BlogId]
	)
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Images] ADD 
	CONSTRAINT [FK_subtext_Images_subtext_Config] FOREIGN KEY 
	(
		[BlogId]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
		[BlogId]
	),
	CONSTRAINT [FK_subtext_Images_subtext_LinkCategories] FOREIGN KEY 
	(
		[CategoryID]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_LinkCategories] (
		[CategoryID]
	)
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords] ADD 
	CONSTRAINT [FK_subtext_KeyWords_subtext_Config] FOREIGN KEY 
	(
		[BlogId]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
		[BlogId]
	)
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_LinkCategories] ADD 
	CONSTRAINT [FK_subtext_LinkCategories_subtext_Config] FOREIGN KEY 
	(
		[BlogId]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
		[BlogId]
	)
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Links] ADD 
	CONSTRAINT [FK_subtext_Links_subtext_Config] FOREIGN KEY 
	(
		[BlogId]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
		[BlogId]
	),
	CONSTRAINT [FK_subtext_Links_subtext_Content] FOREIGN KEY 
	(
		[PostID]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Content] (
		[ID]
	),
	CONSTRAINT [FK_subtext_Links_subtext_LinkCategories] FOREIGN KEY 
	(
		[CategoryID]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_LinkCategories] (
		[CategoryID]
	)
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Referrals] ADD 
	CONSTRAINT [FK_subtext_Referrals_subtext_Config] FOREIGN KEY 
	(
		[BlogId]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
		[BlogId]
	),
	CONSTRAINT [FK_subtext_Referrals_subtext_URLs] FOREIGN KEY 
	(
		[UrlID]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_URLs] (
		[UrlID]
	)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE TRIGGER [<dbUser,varchar,dbo>].[subtext_Content_Trigger]
ON [<dbUser,varchar,dbo>].[subtext_Content]
AFTER INSERT, UPDATE, DELETE
AS

DECLARE @BlogId INT

--Get the current Blogid
SELECT @BlogId = BlogId FROM INSERTED

-- much more likely to be an insert than delete
-- need to run on updates as well, incase an item is marked as inactive
IF(@BlogId IS NULL)
Begin
	Select @BlogId = BlogId From DELETED	
End

UPDATE [<dbUser,varchar,dbo>].[subtext_Config]
SET
	PostCount= (Select Count(*) FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = [<dbUser,varchar,dbo>].[subtext_Config].BlogId and PostType = 1 and PostConfig & 1 = 1),
	CommentCount =  (Select Count(*) FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = [<dbUser,varchar,dbo>].[subtext_Config].BlogId and PostType = 3 and PostConfig & 1 = 1),
	StoryCount =  (Select Count(*) FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = [<dbUser,varchar,dbo>].[subtext_Config].BlogId and PostType = 2 and PostConfig & 1 = 1),
	PingTrackCount =  (Select Count(*) FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = [<dbUser,varchar,dbo>].[subtext_Config].BlogId and PostType = 4 and PostConfig & 1 = 1)
WHERE BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Host] (
	[HostUserName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Password] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Salt] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Version] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Major] [int] NOT NULL ,
	[Minor] [int] NOT NULL ,
	[Build] [int] NOT NULL ,
	[DateCreated] [datetime] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Version] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_Version] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Log] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[BlogId] [int] NULL ,
	[Date] [datetime] NOT NULL ,
	[Thread] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Context] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Level] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Logger] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Message] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Exception] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]

GO
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] WITH NOCHECK ADD 
	CONSTRAINT [PK_subtext_Log] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 

GO
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] ADD 
	CONSTRAINT [FK_subtext_Log_subtext_Config] FOREIGN KEY 
	(
		[BlogId]
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
		[BlogId]
	)
GO
