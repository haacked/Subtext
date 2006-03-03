if exists (select * from dbo.sysobjects where id = object_id(N'[DNW_GetRecentPosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [DNW_GetRecentPosts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[DNW_HomePageData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [DNW_HomePageData]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[DNW_Stats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [DNW_Stats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[DNW_Total_Stats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [DNW_Total_Stats]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [DNW_GetRecentPosts]
	@Host nvarchar(100)
	, @GroupID int

AS
SELECT Top 35 Host
	Application
	, [EntryName] = IsNull(blog_Content.EntryName, blog_Content.[ID])
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.FeedBackCount
	, blog_Content.SourceName
	, blog_Content.EntryName
	, [IsXHTML] = Convert(bit,CASE WHEN blog_Content.PostConfig & 2 = 2 THEN 1 else 0 END) 
	, [BlogTitle] = blog_Config.Title
	, blog_Content.PostConfig
	, blog_Config.TimeZone
	, [Description] = IsNull(CASE WHEN PostConfig & 32 = 32 THEN blog_Content.[Description] else blog_Content.[Text] END, '')
FROM blog_Content
INNER JOIN	blog_Config ON blog_Content.BlogID = blog_Config.BlogID
WHERE  blog_Content.PostType = 1 
	AND blog_Content.PostConfig & 1 = 1 
	AND blog_Content.PostConfig & 64 = 64 
	AND blog_Config.Flag & 2 = 2 
	AND blog_Config.Host = @Host
	AND BlogGroup & @GroupID = @GroupID
ORDER BY [ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [DNW_GetRecentPosts]  TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [DNW_Stats]
(
	@Host nvarchar(100),
	@GroupID int
)
AS
SELECT BlogID
	, Author
	, Application
	, Host
	, Title
	, PostCount
	, CommentCount
	, StoryCount
	, PingTrackCount
	, LastUpdated
FROM blog_Config 
WHERE PostCount > 0 AND blog_Config.Flag & 2 = 2 AND Host = @Host AND BlogGroup & @GroupID = @GroupID
ORDER BY PostCount DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [DNW_Stats]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [DNW_Total_Stats]
(
	@Host nvarchar(100),
	@GroupID int
)
AS
SELECT Count(*) AS [BlogCount], Sum(PostCount) AS PostCount, Sum(CommentCount) AS CommentCount, Sum(StoryCount) AS StoryCount, Sum(PingTrackCount) AS PingTrackCount 
FROM blog_Config WHERE blog_Config.Flag & 2 = 2 AND Host = @Host AND BlogGroup & @GroupID = @GroupID

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


CREATE PROC [DNW_HomePageData]
(
	@Host nvarchar(100),
	@GroupID int
)
AS 
EXEC DNW_Stats @Host, @GroupID
EXEC DNW_GetRecentPosts @Host, @GroupID
EXEC DNW_Total_Stats @Host, @GroupID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [DNW_HomePageData]  TO [public]
GO

