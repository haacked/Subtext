if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Content_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Content] DROP CONSTRAINT FK_blog_Content_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_EntryViewCount_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_EntryViewCount] DROP CONSTRAINT FK_blog_EntryViewCount_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Images_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Images] DROP CONSTRAINT FK_blog_Images_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_KeyWords_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_KeyWords] DROP CONSTRAINT FK_blog_KeyWords_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_LinkCategories_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_LinkCategories] DROP CONSTRAINT FK_blog_LinkCategories_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Links_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Links] DROP CONSTRAINT FK_blog_Links_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Referrals_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Referrals] DROP CONSTRAINT FK_blog_Referrals_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Links_blog_Content]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Links] DROP CONSTRAINT FK_blog_Links_blog_Content
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Images_blog_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Images] DROP CONSTRAINT FK_blog_Images_blog_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Links_blog_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Links] DROP CONSTRAINT FK_blog_Links_blog_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Referrals_blog_URLs]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Referrals] DROP CONSTRAINT FK_blog_Referrals_blog_URLs
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Content_Trigger]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[blog_Content_Trigger]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[iter_charlist_to_table]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[iter_charlist_to_table]
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
	[BlogGroup] [int] NULL ,
	[LicenseUrl] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[DaysTillCommentsClose] [int] NULL ,
	[CommentDelayInMinutes] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_Content] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
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
	[EntryName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ContentChecksumHash] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
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
	[ImageID] [int] IDENTITY (1, 1) NOT NULL ,
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
	[KeyWordID] [int] IDENTITY (1, 1) NOT NULL ,
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
	[CategoryID] [int] IDENTITY (1, 1) NOT NULL ,
	[Title] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Active] [bit] NOT NULL ,
	[BlogID] [int] NOT NULL ,
	[CategoryType] [tinyint] NULL ,
	[Description] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[blog_Links] (
	[LinkID] [int] IDENTITY (1, 1) NOT NULL ,
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
	[UrlID] [int] IDENTITY (1, 1) NOT NULL ,
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

ALTER TABLE [dbo].[blog_Content] WITH NOCHECK ADD 
	CONSTRAINT [PK_blog_Content] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[blog_Images] WITH NOCHECK ADD 
	CONSTRAINT [PK_blog_Images] PRIMARY KEY  CLUSTERED 
	(
		[ImageID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[blog_KeyWords] WITH NOCHECK ADD 
	CONSTRAINT [PK_blog_KeyWords] PRIMARY KEY  CLUSTERED 
	(
		[KeyWordID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[blog_LinkCategories] WITH NOCHECK ADD 
	CONSTRAINT [PK_blog_LinkCategories] PRIMARY KEY  CLUSTERED 
	(
		[CategoryID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[blog_Links] WITH NOCHECK ADD 
	CONSTRAINT [PK_blog_Links] PRIMARY KEY  CLUSTERED 
	(
		[LinkID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[blog_URLs] WITH NOCHECK ADD 
	CONSTRAINT [PK_blog_URLs] PRIMARY KEY  CLUSTERED 
	(
		[UrlID]
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

ALTER TABLE [dbo].[blog_Content] ADD 
	CONSTRAINT [FK_blog_Content_blog_Config] FOREIGN KEY 
	(
		[BlogID]
	) REFERENCES [dbo].[blog_Config] (
		[BlogID]
	)
GO

ALTER TABLE [dbo].[blog_EntryViewCount] ADD 
	CONSTRAINT [FK_blog_EntryViewCount_blog_Config] FOREIGN KEY 
	(
		[BlogID]
	) REFERENCES [dbo].[blog_Config] (
		[BlogID]
	)
GO

ALTER TABLE [dbo].[blog_Images] ADD 
	CONSTRAINT [FK_blog_Images_blog_Config] FOREIGN KEY 
	(
		[BlogID]
	) REFERENCES [dbo].[blog_Config] (
		[BlogID]
	),
	CONSTRAINT [FK_blog_Images_blog_LinkCategories] FOREIGN KEY 
	(
		[CategoryID]
	) REFERENCES [dbo].[blog_LinkCategories] (
		[CategoryID]
	)
GO

ALTER TABLE [dbo].[blog_KeyWords] ADD 
	CONSTRAINT [FK_blog_KeyWords_blog_Config] FOREIGN KEY 
	(
		[BlogID]
	) REFERENCES [dbo].[blog_Config] (
		[BlogID]
	)
GO

ALTER TABLE [dbo].[blog_LinkCategories] ADD 
	CONSTRAINT [FK_blog_LinkCategories_blog_Config] FOREIGN KEY 
	(
		[BlogID]
	) REFERENCES [dbo].[blog_Config] (
		[BlogID]
	)
GO

ALTER TABLE [dbo].[blog_Links] ADD 
	CONSTRAINT [FK_blog_Links_blog_Config] FOREIGN KEY 
	(
		[BlogID]
	) REFERENCES [dbo].[blog_Config] (
		[BlogID]
	),
	CONSTRAINT [FK_blog_Links_blog_Content] FOREIGN KEY 
	(
		[PostID]
	) REFERENCES [dbo].[blog_Content] (
		[ID]
	),
	CONSTRAINT [FK_blog_Links_blog_LinkCategories] FOREIGN KEY 
	(
		[CategoryID]
	) REFERENCES [dbo].[blog_LinkCategories] (
		[CategoryID]
	)
GO

ALTER TABLE [dbo].[blog_Referrals] ADD 
	CONSTRAINT [FK_blog_Referrals_blog_Config] FOREIGN KEY 
	(
		[BlogID]
	) REFERENCES [dbo].[blog_Config] (
		[BlogID]
	),
	CONSTRAINT [FK_blog_Referrals_blog_URLs] FOREIGN KEY 
	(
		[UrlID]
	) REFERENCES [dbo].[blog_URLs] (
		[UrlID]
	)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO




--Found at: http://www.algonet.se/~sommar/arrays-in-sql.html
CREATE FUNCTION [dbo].[iter_charlist_to_table]
(
	@list      ntext
	, @delimiter nchar(1) = N','
)
RETURNS @tbl TABLE 
(
	listpos int IDENTITY(1, 1) NOT NULL
	, str     varchar(4000)
	, nstr    nvarchar(2000)
) AS

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


CREATE TRIGGER blog_Content_Trigger
ON blog_Content
AFTER INSERT, UPDATE, DELETE
AS

DECLARE @BlogID INT

--Get the current blogid
SELECT @BlogID = BlogID FROM INSERTED

-- much more likely to be an insert than delete
-- need to run on updates as well, incase an item is marked as inactive
IF(@BlogID IS NULL)
Begin
	Select @BlogID = BlogID From DELETED	
End

UPDATE blog_Config
SET
	PostCount= (Select Count(*) FROM blog_Content WHERE blog_Content.BlogID = blog_Config.BlogID and PostType = 1 and PostConfig & 1 = 1),
	CommentCount =  (Select Count(*) FROM blog_Content WHERE blog_Content.BlogID = blog_Config.BlogID and PostType = 3 and PostConfig & 1 = 1),
	StoryCount =  (Select Count(*) FROM blog_Content WHERE blog_Content.BlogID = blog_Config.BlogID and PostType = 2 and PostConfig & 1 = 1),
	PingTrackCount =  (Select Count(*) FROM blog_Content WHERE blog_Content.BlogID = blog_Config.BlogID and PostType = 4 and PostConfig & 1 = 1)
WHERE BlogID = @BlogID



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

