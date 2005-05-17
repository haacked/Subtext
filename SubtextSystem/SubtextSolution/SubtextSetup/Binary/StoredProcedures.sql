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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC DNW_GetRecentPosts  -- 'localhost', 1
	@Host nvarchar(100)
	, @GroupID int

AS
SELECT Top 35 Host
	Application
	, [EntryName] = IsNull(blog_Content.EntryName,blog_Content.[ID])
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

GRANT  EXECUTE  ON [dbo].[DNW_GetRecentPosts]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC DNW_HomePageData
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

GRANT  EXECUTE  ON [dbo].[DNW_HomePageData]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC DNW_Stats
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

GRANT  EXECUTE  ON [dbo].[DNW_Stats]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC DNW_Total_Stats
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

GRANT  EXECUTE  ON [dbo].[DNW_Total_Stats]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_DeleteCategory
(
	@CategoryID int,
	@BlogID int
)
AS
DELETE blog_Links FROM blog_Links WHERE blog_Links.CategoryID = @CategoryID AND blog_Links.BlogID = @BlogID
DELETE blog_LinkCategories FROM blog_LinkCategories WHERE blog_LinkCategories.CategoryID = @CategoryID AND blog_LinkCategories.BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_DeleteCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_DeleteImage
(
	@BlogID int,
	@ImageID int
)
AS
DELETE	blog_Images 
FROM	blog_Images 
WHERE	ImageID = @ImageID 
	AND BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_DeleteImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_DeleteImageCategory
(
	@CategoryID int,
	@BlogID int
)
AS
DELETE blog_Images FROM blog_Images WHERE blog_Images.CategoryID = @CategoryID AND blog_Images.BlogID = @BlogID
DELETE blog_LinkCategories FROM blog_LinkCategories WHERE blog_LinkCategories.CategoryID = @CategoryID AND blog_LinkCategories.BlogID = @BlogID



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_DeleteImageCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_DeleteKeyWord
(
	@KeyWordID int,
	@BlogID int
)

AS

DELETE FROM blog_KeyWords WHERE BLOGID = @BlogID AND KeyWordID = @KeyWordID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_DeleteKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_DeleteLink
(
	@LinkID int,
	@BlogID int
)
AS
DELETE blog_Links FROM blog_Links WHERE blog_Links.[LinkID] = @LinkID AND blog_Links.BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_DeleteLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_DeleteLinksByPostID
(
	@PostID int,
	@BlogID int
)
AS
Set NoCount ON
DELETE blog_Links FROM blog_Links WHERE PostID = @PostID AND BlogID = @BlogID



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_DeleteLinksByPostID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Deletes a record from blog_content, whether it be a post, a comment, etc..
*/
CREATE PROC blog_DeletePost
(
	@ID int,
	@BlogID int
)
AS

DECLARE @ParentID int, @PostType int

SELECT @ParentID = ParentID, @PostType = PostType FROM blog_Content WHERE [ID] = @ID

-- PostType 3 = Comment
-- PostType 4 = PingBack

IF(@PostType = 3 or @PostType = 4)
BEGIN
	UPDATE blog_Content
	SET FeedBackCount = FeedBackCount - 1
	WHERE [ID] = @ParentID
END
ELSE
BEGIN
	DELETE FROM blog_Content WHERE ParentID = @ID
	-- This is a refactoring in progress 
	-- to remove meaning from PostId = -1
	DECLARE @PostID int
	SET @PostID = @ID
	IF @ID = -1
		SET @PostID = NULL
	DELETE FROM blog_Links WHERE PostID = @PostID
	DELETE FROM blog_EntryViewCount WHERE EntryID = @ID
	DELETE FROM blog_Referrals WHERE EntryID = @ID
END

DELETE FROM blog_Content WHERE blog_Content.[ID] = @ID AND blog_Content.BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_DeletePost]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetActiveCategoriesWithLinkCollection
(
	@BlogID int
)
AS
SELECT blog_LinkCategories.CategoryID
	, blog_LinkCategories.Title
	, blog_LinkCategories.Active
	, blog_LinkCategories.CategoryType
	, blog_LinkCategories.[Description]
FROM	blog_LinkCategories
WHERE	
			blog_LinkCategories.Active= 1 
	AND		blog_LinkCategories.BlogID = @BlogID 
	AND		blog_LinkCategories.CategoryType = 0
ORDER BY 
	blog_LinkCategories.Title;

SELECT blog_Links.LinkID
	, blog_Links.Title
	, blog_Links.Url
	, blog_Links.Rss
	, blog_Links.Active
	, blog_Links.NewWindow
	, blog_Links.CategoryID
	, PostID = ISNULL(blog_Links.PostID, -1)
FROM blog_Links 
	INNER JOIN blog_LinkCategories ON blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE 
		blog_Links.Active = 1 
	AND blog_LinkCategories.Active=1
	AND blog_LinkCategories.BlogID = @BlogID 
	AND blog_Links.BlogID = @BlogID 
	AND blog_LinkCategories.CategoryType = 0
ORDER BY 
	blog_Links.Title;



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetActiveCategoriesWithLinkCollection]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetAllCategories
(
	@BlogID int
	, @IsActive bit
	, @CategoryType tinyint
)
As
SELECT blog_LinkCategories.CategoryID
	, blog_LinkCategories.Title
	, blog_LinkCategories.Active
	, blog_LinkCategories.CategoryType
	, blog_LinkCategories.[Description]
FROM blog_LinkCategories
WHERE blog_LinkCategories.BlogID = @BlogID 
	AND blog_LinkCategories.CategoryType = @CategoryType 
	AND blog_LinkCategories.Active <> CASE @IsActive WHEN 1 THEN 0 ELSE -1 END
ORDER BY blog_LinkCategories.Title;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetAllCategories]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC blog_GetBlogKeyWords
(
	@BlogID int
)
AS

SELECT 
	KeyWordID, Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID
FROM
	blog_keywords
WHERE 
	BlogID = @BlogID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetBlogKeyWords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetCategory
(
	@CategoryID int,
	@IsActive bit,
	@BlogID int
)
AS
SELECT	blog_LinkCategories.CategoryID
		, blog_LinkCategories.Title
		, blog_LinkCategories.Active
		, blog_LinkCategories.CategoryType
		, blog_LinkCategories.[Description]
FROM blog_LinkCategories
WHERE blog_LinkCategories.CategoryID=@CategoryID 
	AND blog_LinkCategories.BlogID = @BlogID 
	AND blog_LinkCategories.Active <> CASE @IsActive WHEN 0 THEN -1 else 0 END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetCategoryByName 
(
	@CategoryName nvarchar(150),
	@IsActive bit,
	@BlogID int
)
AS
SELECT	blog_LinkCategories.CategoryID
		, blog_LinkCategories.Title
		, blog_LinkCategories.Active
		, blog_LinkCategories.CategoryType
		, blog_LinkCategories.[Description]
FROM blog_LinkCategories
WHERE	blog_LinkCategories.Title=@CategoryName 
	AND blog_LinkCategories.BlogID = @BlogID 
	AND blog_LinkCategories.Active <> CASE @IsActive WHEN 0 THEN -1 else 0 END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetCategoryByName]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetConditionalEntries
(
	@ItemCount int 
	, @PostType int
	, @PostConfig int
	, @BlogID int
)
AS

SET ROWCOUNT @ItemCount
SELECT blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, Blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE	blog_Content.PostType=@PostType 
	AND blog_Content.BlogID = @BlogID
	AND blog_Content.PostConfig & @PostConfig = @PostConfig
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetConditionalEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetConditionalEntriesByDateUpdated
(
	@DateUpdated datetime
	, @ItemCount int
	, @PostType int
	, @PostConfig int
	, @BlogID int
)
AS

SET ROWCOUNT @ItemCount
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, Blog_Content.PostConfig
	, blog_Content.ENtryName 
FROM blog_Content
WHERE	blog_Content.PostType=@PostType 
	AND blog_Content.BlogID = @BlogID
	AND blog_Content.PostConfig & @PostConfig = @PostConfig 
	AND blog_Content.DateUpdated > @DateUpdated
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetConditionalEntriesByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC blog_GetConfig
(
	@Host nvarchar(100) -- Depracated
	, @Application nvarchar(50) -- Depracated
)
AS
SELECT TOP 1 
	blog_Config.BlogID
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
FROM blog_Config

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetConfig]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC blog_GetEntriesByDayRange
(
	@StartDate datetime,
	@StopDate datetime,
	@PostType int,
	@IsActive bit,
	@BlogID int
)
AS
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE 
	(
		blog_Content.DateAdded > @StartDate 
		AND blog_Content.DateAdded < DateAdd(day, 1, @StopDate)
	)
	AND blog_Content.PostType=@PostType 
	AND blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetEntriesByDayRange]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetEntryCollectionByDateUpdated
(
	@ItemCount int,
	@IsActive bit,
	@PostType int, 
	@DateUpdated datetime,
	@BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE 
	blog_Content.PostType=@PostType 
	AND blog_Content.BlogID = @BlogID
	AND blog_Content.DateUpdated > @DateUpdated
	AND blog_Content.PostConfig & 1  <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.[dateupdated] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetEntryCollectionByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetEntryWithCategoryTitles
(
	@PostID int
	, @IsActive bit
	, @BlogID int
)
AS
SELECT	blog_Content.BlogID
		, blog_Content.[ID]
		, blog_Content.Title
		, blog_Content.DateAdded
		, blog_Content.[Text]
		, blog_Content.[Description]
		, blog_Content.SourceUrl
		, blog_Content.PostType
		, blog_Content.Author
		, blog_Content.Email
		, blog_Content.SourceName
		, blog_Content.DateUpdated
		, blog_Content.TitleUrl
		, blog_Content.ParentID
		, blog_Content.FeedBackCount
		, blog_Content.PostConfig
		, blog_Content.EntryName
		, blog_Content.ParentID 
FROM blog_Content
WHERE	blog_Content.[ID] = @PostID 
	AND  blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.[dateadded] DESC

SELECT	c.Title
		, PostID = ISNULL(l.PostID, -1)
FROM blog_Links l
INNER JOIN blog_LinkCategories c ON l.CategoryID = c.CategoryID
WHERE l.PostID = @PostID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetEntryWithCategoryTitles]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC blog_GetEntryWithCategoryTitlesByEntryName
(
	@EntryName nvarchar(150)
	, @IsActive bit
	, @BlogID int
)
AS
DECLARE @PostID int

SELECT	blog_Content.BlogID
		, blog_Content.[ID]
		, blog_Content.Title
		, blog_Content.DateAdded
		, blog_Content.[Text]
		, blog_Content.[Description]
		, blog_Content.SourceUrl
		, blog_Content.PostType
		, blog_Content.Author
		, blog_Content.Email
		, blog_Content.SourceName
		, blog_Content.DateUpdated
		, blog_Content.TitleUrl
		, blog_Content.ParentID
		, blog_Content.FeedBackCount
		, blog_Content.PostConfig
		, blog_Content.EntryName
		, blog_Content.ParentID 
FROM blog_Content
WHERE	blog_Content.EntryName = @EntryName 
	AND  blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.[dateadded] DESC

SELECT	c.Title
		, PostID = ISNULL(l.PostID, -1)
FROM blog_Links l
	INNER JOIN blog_LinkCategories c ON l.CategoryID = c.CategoryID
	INNER JOIN blog_Content ON l.PostID = blog_Content.[ID]
WHERE blog_Content.EntryName = @EntryName


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetEntryWithCategoryTitlesByEntryName]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetFeedBack 
(
	@ParentID int
	, @BlogID int
)
AS

SELECT	blog_Content.BlogID
		, blog_Content.[ID]
		, blog_Content.Title
		, blog_Content.DateAdded
		, blog_Content.[Text]
		, blog_Content.[Description]
		, blog_Content.SourceUrl
		, blog_Content.PostType
		, blog_Content.Author
		, blog_Content.Email
		, blog_Content.SourceName
		, blog_Content.DateUpdated
		, blog_Content.TitleUrl
		, blog_Content.ParentID
		, blog_Content.FeedBackCount
		, blog_Content.PostConfig
		, blog_Content.EntryName
		, blog_Content.ParentID 
FROM blog_Content
WHERE blog_Content.BlogID = @BlogID AND blog_Content.PostConfig & 1 = 1 AND blog_Content.ParentID = @ParentID
ORDER BY [ID]


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetFeedBack]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetImageCategory
(
	@CategoryID int
	, @IsActive bit
	, @BlogID int
)
AS
EXEC blog_GetCategory @CategoryID, @IsActive, @BlogID

SELECT	Title
		, CategoryID
		, Height
		, Width
		, [File]
		, Active
		, ImageID 
FROM blog_Images  
WHERE CategoryID = @CategoryID 
	AND BlogID = @BlogID 
	AND Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetImageCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetKeyWord
(
	@KeyWordID int
	, @BlogID int
)
AS

SELECT 
	KeyWordID, Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID
FROM
	blog_keywords
WHERE 
	KeyWordID = @KeyWordID AND BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetLinkCollectionByPostID
(
	@PostID int,
	@BlogID int
)
AS

IF @PostID = -1
	SET @PostID = NULL

SELECT	blog_Links.LinkID
	, blog_Links.Title
	, blog_Links.Url
	, blog_Links.Rss
	, blog_Links.Active
	, blog_Links.CategoryID
	, PostID = ISNULL(blog_Links.PostID, -1)
	, blog_Links.NewWindow 
FROM blog_Links
WHERE blog_Links.PostID = @PostID 
	AND blog_Links.BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetLinkCollectionByPostID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetLinksByActiveCategoryID
(
	@CategoryID int
	, @BlogID int
)
AS
EXEC blog_GetCategory @CategoryID, 0, @BlogID
SELECT	blog_Links.LinkID
		, blog_Links.Title
		, blog_Links.Url
		, blog_Links.Rss
		, blog_Links.Active
		, blog_Links.CategoryID
		, PostID = ISNULL(blog_Links.PostID, -1)
FROM blog_Links
WHERE blog_Links.Active = 1 
	AND blog_Links.CategoryID = @CategoryID 
	AND blog_Links.BlogID = @BlogID
ORDER BY blog_Links.Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetLinksByActiveCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetLinksByCategoryID
(
	@CategoryID int
	, @BlogID int
)
AS
EXEC blog_GetCategory @CategoryID, @BlogID
SELECT	blog_Links.LinkID
		, blog_Links.Title
		, blog_Links.Url
		, blog_Links.Rss
		, blog_Links.Active
		, blog_Links.NewWindow
		, blog_Links.CategoryID
		, PostId = ISNULL(blog_Links.PostID, -1)
FROM blog_Links
WHERE	blog_Links.CategoryID = @CategoryID 
	AND blog_Links.BlogID = @BlogID
ORDER BY blog_Links.Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetLinksByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
I think this proc gets a page of blog posts 
within the admin section.
*/
CREATE PROC blog_GetPageableEntries
(
	@BlogID int
	, @PageIndex int
	, @PageSize int
	, @PostType int
	, @SortDesc bit
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

SELECT	content.BlogID 
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
		, content.FeedbackCount
		, content.ParentID
		, content.PostConfig
		, content.EntryName
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated
		
FROM  	blog_Content content
    	INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
	Left JOIN  blog_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogID = @BlogID)
WHERE 	content.BlogID = @BlogID 
	AND tmp.TempID > @PageLowerBound 
	AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs


SELECT COUNT([ID]) AS TotalRecords
FROM 	blog_Content 
WHERE 	blogID = @BlogID 
	AND PostType = @PostType 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPageableEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPageableEntriesByCategoryID
(
	@BlogID int
	, @CategoryID int
	, @PageIndex int
	, @PageSize int
	, @PostType int
	, @SortDesc bit
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
		INNER JOIN blog_Links links ON (blog.[ID] = ISNULL(links.PostID, -1))
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
		INNER JOIN blog_Links links ON (blog.[ID] = ISNULL(links.PostID, -1))
		INNER JOIN blog_LinkCategories cats ON (links.CategoryID = cats.CategoryID)
	WHERE 	blog.blogID = @BlogID 
		AND blog.PostType = @PostType
		AND cats.CategoryID = @CategoryID
	ORDER BY blog.[ID] DESC
END
 
SELECT	content.BlogID 
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
		, content.FeedbackCount
		, content.ParentID
		, content.PostConfig
		, content.EntryName
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated

FROM	blog_Content content
    INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
	LEFT JOIN  blog_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogID = @BlogID)
WHERE	content.BlogID = @BlogID 
	AND	tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs

SELECT 	COUNT(blog.[ID]) AS TotalRecords
FROM 	blog_Content blog
	INNER JOIN blog_Links links ON (blog.[ID] = ISNULL(links.PostID, -1))
	INNER JOIN blog_LinkCategories cats ON (links.CategoryID = cats.CategoryID)
WHERE 	blog.blogID = @BlogID 
	AND blog.PostType = @PostType
	AND cats.CategoryID = @CategoryID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPageableEntriesByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPageableFeedback 
(
	@BlogID int
	, @PageIndex int
	, @PageSize int
	, @SortDesc bit
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

SELECT	content.BlogID 
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
		, content.FeedbackCount
		, content.ParentID
		, content.PostConfig
		, content.EntryName
FROM  	blog_Content content
    INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
WHERE 	content.BlogID = @BlogID 
	AND tmp.TempID > @PageLowerBound 
	AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs

SELECT 	COUNT([ID]) AS TotalRecords
FROM 	blog_Content 
WHERE 	blogID = @BlogID 
	AND (PostType = 3 or PostType = 4)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPageableFeedback]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPageableKeyWords
(
	@BlogID int
	, @PageIndex int
	, @PageSize int
	, @SortDesc bit
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1


CREATE TABLE #TempPagedKeyWordIDs 
(
	TempID int IDENTITY (1, 1) NOT NULL
	, KeywordID int NOT NULL
)	

IF(@SortDesc = 1)
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
	ORDER BY Word DESC
END

SELECT 	words.KeyWordID
		, words.Word
		, words.[Text]
		, words.ReplaceFirstTimeOnly
		, words.OpenInNewWindow
		, words.CaseSensitive
		, words.Url
		, words.Title
		, words.BlogID
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

SELECT 	COUNT([KeywordID]) AS 'TotalRecords'
FROM 	blog_KeyWords 
WHERE 	blogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPageableKeyWords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPageableLinks
(
	@BlogID int
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
	FROM 	blog_Links 
	WHERE 	blogID = @BlogID 
		AND PostID IS NULL
	ORDER BY Title
END
ELSE
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM 	blog_Links
	WHERE 	blogID = @BlogID 
		AND PostID IS NULL
	ORDER BY [Title] DESC
END

SELECT 	links.LinkID 
	, links.Title 
	, links.Url
	, links.Rss 
	, links.Active 
	, links.NewWindow 
	, links.CategoryID
	, PostID = ISNULL(links.PostID, -1)
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

SELECT 	COUNT([LinkID]) AS TotalRecords
FROM 	blog_Links 
WHERE 	blogID = @BlogID
	AND PostID IS NULL


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPageableLinks]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPageableLinksByCategoryID
(
	@BlogID int
	, @CategoryID int
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
	FROM 	blog_Links 
	WHERE 	blogID = @BlogID 
		AND CategoryID = @CategoryID
		AND PostID IS NULL
	ORDER BY Title
END
ELSE
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM 	blog_Links
	WHERE 	blogID = @BlogID 
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


SELECT  COUNT([LinkID]) AS TotalRecords
FROM 	blog_Links 
WHERE 	blogID = @BlogID 
	AND CategoryID = @CategoryID 
	AND PostID IS NULL


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPageableLinksByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


--SELECT Top 5 * FROM blog_Referrals ORDER BY LastUpdated DESC

CREATE PROC blog_GetPageableReferrers
(
	@BlogID INT
	, @PageIndex INT
	, @PageSize INT
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
  ORDER BY LastUpdated DESC
   
SELECT	u.URL,
	c.Title,
	r.EntryID,
	c.EntryName,
	LastUpdated,
	[Count]
FROM 	blog_Content c,
	#tempblog_Referrals r,
	blog_URLs u
WHERE r.EntryID = c.ID AND
      c.BlogID = @BlogID
  AND r.UrlID = u.UrlID
  AND r.TempID > @PageLowerBound
  AND r.TempID < @PageUpperBound

ORDER BY TempID


SELECT COUNT(*) AS 'TotalRecords' FROM #tempblog_Referrals


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPageableReferrers]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPageableReferrersByEntryID 
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

INSERT INTO #tempblog_Referrals 
(
	UrlID
	, [Count]
	, LastUpdated
)
  SELECT UrlID
	, [Count]
	, LastUpdated
  FROM blog_Referrals
  WHERE blog_Referrals.BlogID = @BlogID AND blog_Referrals.EntryID = @EntryID
  ORDER BY LastUpdated DESC
   
SELECT	u.URL
	, c.Title
	, c.EntryName
	, [EntryId] = @EntryID
	, LastUpdated
	[Count]
FROM 	blog_Content c
	, #tempblog_Referrals r
	, blog_URLs u
WHERE c.ID = @EntryID 
	AND c.BlogID = @BlogID
	AND r.UrlID = u.UrlID
	AND r.TempID > @PageLowerBound
	AND r.TempID < @PageUpperBound
	ORDER BY TempID

SELECT COUNT(*) AS 'TotalRecords' FROM #tempblog_Referrals


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPageableReferrersByEntryID]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPostsByCategoryID
(
	@ItemCount int
	, @CategoryID int
	, @IsActive bit
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content WITH (NOLOCK)
	INNER JOIN blog_Links WITH (NOLOCK) ON blog_Content.ID = ISNULL(blog_Links.PostID, -1)
	INNER JOIN blog_LinkCategories WITH (NOLOCK) ON blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE  blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END AND blog_LinkCategories.CategoryID = @CategoryID
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPostsByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPostsByCategoryIDByDateUpdated
(
	@ItemCount int
	, @CategoryID int
	, @IsActive bit
	, @DateUpdated datetime
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
	INNER JOIN blog_Links ON blog_Content.ID = ISNULL(blog_Links.PostID, -1)
	INNER JOIN blog_LinkCategories ON blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE  blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END 
	AND blog_LinkCategories.CategoryID = @CategoryID 
	AND blog_Content.DateUpdated > @DateUpdated
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPostsByCategoryIDByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPostsByCategoryName -- 15,72,0
(
	@ItemCount int,
	@CategoryName nvarchar(150),
	@IsActive bit,
	@BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT	blog_Content.BlogID
		, blog_Content.[ID]
		, blog_Content.Title
		, blog_Content.DateAdded
		, blog_Content.[Text]
		, blog_Content.[Description]
		, blog_Content.SourceUrl
		, blog_Content.PostType
		, blog_Content.Author
		, blog_Content.Email
		, blog_Content.SourceName
		, blog_Content.DateUpdated
		, blog_Content.TitleUrl
		, blog_Content.FeedBackCount
		, blog_Content.ParentID
		, blog_Content.PostConfig
		, blog_Content.EntryName 
FROM blog_Content
	INNER JOIN blog_Links ON blog_Content.ID = ISNULL(blog_Links.PostID, -1)
	INNER JOIN blog_LinkCategories ON blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE	blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END 
	AND blog_LinkCategories.Title = @CategoryName
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPostsByCategoryName]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPostsByCategoryNameByDateUpdated
(
	@ItemCount int
	, @CategoryName nvarchar(150)
	, @IsActive bit
	, @DateUpdated datetime
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT	blog_Content.BlogID
		, blog_Content.[ID]
		, blog_Content.Title
		, blog_Content.DateAdded
		, blog_Content.[Text]
		, blog_Content.[Description]
		, blog_Content.SourceUrl
		, blog_Content.PostType
		, blog_Content.Author
		, blog_Content.Email
		, blog_Content.SourceName
		, blog_Content.DateUpdated
		, blog_Content.TitleUrl
		, blog_Content.FeedBackCount
		, blog_Content.ParentID
		, blog_Content.PostConfig
		, blog_Content.EntryName 
FROM blog_Content
	INNER JOIN blog_Links ON blog_Content.ID = ISNULL(blog_Links.PostID, -1)
	INNER JOIN blog_LinkCategories ON blog_Links.CategoryID = blog_LinkCategories.CategoryID
WHERE  blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END 
	AND blog_LinkCategories.Title = @CategoryName 
	AND blog_Content.DateUpdated > @DateUpdated
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPostsByCategoryNameByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPostsByDayRange
(
	@StartDate datetime,
	@StopDate datetime,
	@BlogID int
)
AS
SELECT	blog_Content.BlogID
		, blog_Content.[ID]
		, blog_Content.Title
		, blog_Content.DateAdded
		, blog_Content.[Text]
		, blog_Content.[Description]
		, blog_Content.SourceUrl
		, blog_Content.PostType
		, blog_Content.Author
		, blog_Content.Email
		, blog_Content.SourceName
		, blog_Content.DateUpdated
		, blog_Content.TitleUrl
		, blog_Content.FeedBackCount
		, blog_Content.ParentID
		, blog_Content.PostConfig
		, blog_Content.EntryName 
FROM blog_Content
WHERE 
	(
			blog_Content.DateAdded > @StartDate 
		AND blog_Content.DateAdded < DateAdd(day,1,@StopDate)
	)
	AND blog_Content.PostType=1 
	AND blog_Content.BlogID = @BlogID
ORDER BY blog_Content.DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPostsByDayRange]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPostsByMonth
(
	@Month int
	, @Year int
	, @BlogID int
)
AS
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE	blog_Content.PostType=1 
	AND blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 = 1 
	AND Month(blog_Content.DateAdded) = @Month 
	AND Year(blog_Content.DateAdded)  = @Year
ORDER BY blog_Content.DateAdded DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPostsByMonth]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPostsByMonthArchive 
(
	@BlogID int
)
AS
SELECT Month(DateAdded) AS [Month]
	, Year(DateAdded) AS [Year]
	, 1 AS Day, Count(*) AS [Count] 
FROM blog_Content 
WHERE PostType = 1 AND PostConfig & 1 = 1 AND BlogID = @BlogID 
GROUP BY Year(DateAdded), Month(DateAdded) ORDER BY [Year] DESC, [Month] DESC



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPostsByMonthArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetPostsByYearArchive 
(
	@BlogID int
)
AS
SELECT 1 AS [Month], Year(DateAdded) AS [Year], 1 AS Day, Count(*) AS [Count] FROM blog_Content 
WHERE PostType = 1 AND PostConfig & 1 = 1 AND BlogID = @BlogID 
GROUP BY Year(DateAdded) ORDER BY [Year] DESC



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetPostsByYearArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetRecentEntries
(
	@ItemCount int
	, @IsActive bit
	, @PostType int
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, Blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE	blog_Content.PostType=@PostType 
	AND blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.[dateadded] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetRecentEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetRecentEntriesByDateUpdated
(
	@ItemCount int
	, @IsActive bit 
	, @PostType int
	, @DateUpdated datetime
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, Blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE 
	blog_Content.PostType=@PostType 
	AND blog_Content.BlogID = @BlogID
	AND blog_Content.DateUpdated > @DateUpdated
	AND blog_Content.PostConfig & 1  <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetRecentEntriesByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetRecentEntriesWithCategoryTitles
(
	@ItemCount int,
	@IsActive bit,
	@BlogID int
)
AS
SET ROWCOUNT @ItemCount
CREATE Table #IDs
(
	TempID int IDENTITY (0, 1) NOT NULL,
	PostID int not NULL
)
INSERT #IDs (PostID)
SELECT	blog_Content.[ID] 
FROM	blog_Content
WHERE	blog_Content.PostType=1 
	AND blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.[dateadded] DESC

SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, Blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content, #IDs
WHERE blog_Content.[ID] = #IDs.PostID
ORDER BY TempID ASC

SET ROWCOUNT 0

SELECT	c.Title
		, PostId = ISNULL(l.PostID, -1)
FROM blog_Links l
	INNER JOIN #IDs ON ISNULL(l.[PostID], -1) = #IDs.[PostID]
	INNER JOIN blog_LinkCategories c ON l.CategoryID = c.CategoryID
DROP Table #IDs


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetRecentEntriesWithCategoryTitles]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetSingleDay
(
	@Date datetime
	,@BlogID int
)
AS
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, Blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE Year(blog_Content.DateAdded) = Year(@Date) 
	AND Month(blog_Content.DateAdded) = Month(@Date)
    AND Day(blog_Content.DateAdded) = Day(@Date) 
    And blog_Content.PostType=1
    AND blog_Content.BlogID = @BlogID 
    AND blog_Content.PostConfig & 1 = 1 
ORDER BY blog_Content.DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetSingleDay]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetSingleEntry
(
	@ID int
	, @IsActive bit
	, @BlogID int
)
AS
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, Blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE blog_Content.[ID] = @ID 
	AND blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetSingleEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetSingleEntryByName
(
	@EntryName nvarchar(150)
	, @IsActive bit
	, @BlogID int
)
AS
SELECT	blog_Content.BlogID
	, blog_Content.[ID]
	, blog_Content.Title
	, blog_Content.DateAdded
	, blog_Content.[Text]
	, blog_Content.[Description]
	, blog_Content.SourceUrl
	, blog_Content.PostType
	, blog_Content.Author
	, blog_Content.Email
	, blog_Content.SourceName
	, blog_Content.DateUpdated
	, blog_Content.TitleUrl
	, blog_Content.FeedBackCount
	, blog_Content.ParentID
	, Blog_Content.PostConfig
	, blog_Content.EntryName 
FROM blog_Content
WHERE blog_Content.[EntryName] = @EntryName 
	AND blog_Content.BlogID = @BlogID 
	AND blog_Content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY blog_Content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetSingleEntryByName]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetSingleImage
(
	@ImageID int
	, @IsActive bit
	, @BlogID int
)
AS
SELECT Title
	, CategoryID
	, Height
	, Width
	, [File]
	, Active
	, ImageID 
FROM blog_Images  
WHERE ImageID = @ImageID 
	AND BlogID = @BlogID 
	AND  Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetSingleImage]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetSingleLink
(
	@LinkID int
	, @BlogID int
)
AS
SELECT	blog_Links.LinkID
		, blog_Links.Title
		, blog_Links.Url
		, blog_Links.Rss
		, blog_Links.Active
		, blog_Links.NewWindow
		, blog_Links.CategoryID
		, PostId = ISNULL(blog_Links.PostID, -1)
FROM blog_Links
WHERE blog_Links.LinkID = @LinkID AND blog_Links.BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetSingleLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_GetUrlID
(
	@Url nvarchar(255)
	, @UrlID int output
)
AS
IF EXISTS(SELECT UrlID FROM blog_Urls WHERE Url = @Url)
BEGIN
	SELECT @UrlID = UrlID FROM blog_Urls WHERE Url = @Url
END
Else
BEGIN
	INSERT blog_Urls VALUES (@Url)
	SELECT @UrlID = SCOPE_IDENTITY()
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetUrlID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_InsertCategory
(
	@Title nvarchar(150)
	, @Active bit
	, @BlogID int
	, @CategoryType tinyint
	, @Description nvarchar(1000)
	, @CategoryID int output
)
AS
Set NoCount ON
INSERT INTO blog_LinkCategories 
( 
	Title
	, Active
	, CategoryType
	, [Description]
	, BlogID )
VALUES 
(
	@Title
	, @Active
	, @CategoryType
	, @Description
	, @BlogID
)
SELECT @CategoryID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_InsertEntry
(
	@Title nvarchar(255)
	, @TitleUrl nvarchar(255)
	, @Text ntext
	, @SourceUrl nvarchar(200)
	, @PostType int
	, @Author nvarchar(50)
	, @Email nvarchar(50)
	, @SourceName nvarchar(200)
	, @Description nvarchar(500)
	, @BlogID int
	, @DateAdded datetime
	, @ParentID int
	, @PostConfig int
	, @EntryName nvarchar(150)
	, @ID int output
)
AS

IF(@EntryName IS NOT NULL)
BEGIN
	IF EXISTS(SELECT EntryName FROM blog_Content WHERE BlogID = @BlogID AND EntryName = @EntryName)
	BEGIN
		RAISERROR('The EntryName you entry is already in use with in this Blog. Please pick a unique EntryName.', 11, 1) 
		RETURN 1
	END
END
IF(LTRIM(RTRIM(@Description)) = '')
SET @Description = NULL

INSERT INTO blog_Content 
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
	, BlogID
	, EntryName 
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
	, @BlogID
	, @EntryName
)
SELECT @ID = SCOPE_IDENTITY()
-- PostType
--	1 = BlogPost
--	2 = Story
if(@PostType = 1 or @PostType = 2)
BEGIN
	EXEC blog_UpdateConfigUpdateTime @blogID, @DateAdded
END
ELSE IF(@PostType = 3) -- Comment
BEGIN
	UPDATE blog_Content
	Set FeedBackCount = ISNULL(FeedBackCount, 0) + 1 WHERE [ID] = @ParentID
END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROC blog_InsertEntryViewCount-- 1, 0, 1
(
	@EntryID int,
	@BlogID int,
	@IsWeb bit
)

AS

BEGIN
	--Do we have an existing entry in the blog_InsertEntryViewCount table?
	IF EXISTS(SELECT EntryID FROM blog_EntryViewCount WHERE EntryID = @EntryID AND BlogID = @BlogID)
	BEGIN
		if(@IsWeb = 1) -- Is this a web view?
		BEGIN
			Update blog_EntryViewCount
			Set [WebCount] = [WebCount] + 1, WebLastUpdated = getdate()
			WHERE EntryID = @EntryID AND BlogID = @BlogID
		END
		else
		BEGIN
			Update blog_EntryViewCount
			Set [AggCount] = [AggCount] + 1, AggLastUpdated = getdate()
			WHERE EntryID = @EntryID AND BlogID = @BlogID
		END
	END
	else
	BEGIN
		if(@IsWeb = 1) -- Is this a web view
		BEGIN
			Insert blog_EntryViewCount (EntryID, BlogID, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogID, 1, 0, getdate(), NULL)
		END
		else
		BEGIN
			Insert blog_EntryViewCount (EntryID, BlogID, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogID, 0, 1, NULL, getdate())
		END
	END


END





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertEntryViewCount]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE PROC blog_InsertImage
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
AS
Insert blog_Images
(
	Title, CategoryID, Width, Height, [File], Active, BlogID
)
Values
(
	@Title, @CategoryID, @Width, @Height, @File, @Active, @BlogID
)
Set @ImageID = SCOPE_IDENTITY()



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO






CREATE PROC blog_InsertKeyWord
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

AS

Insert blog_keywords 
	(Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID)
Values
	(@Word,@Text,@ReplaceFirstTimeOnly,@OpenInNewWindow, @CaseSensitive,@Url,@Title,@BlogID)

SELECT @KeyWordID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









CREATE PROC blog_InsertLink
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
AS

IF @PostID = -1
	SET @PostID = NULL

INSERT INTO blog_Links 
( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogID )
VALUES 
(@Title, @Url, @Rss, @Active, @NewWindow, @PostID, @CategoryID, @BlogID);
SELECT @LinkID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROC blog_InsertLinkCategoryList
(
	@CategoryList nvarchar(4000)
	, @PostID int
	, @BlogID int
)
AS

IF @PostID = -1
	SET @PostID = NULL

--DELETE categories that have been removed
DELETE blog_Links FROM blog_Links
WHERE 
	CategoryID not in (SELECT str FROM iter_charlist_to_table(@CategoryList,','))
And 
	BlogID = @BlogID AND (PostID = @PostID)

--Add updated/new categories
INSERT INTO blog_Links ( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogID )
SELECT NULL, NULL, NULL, 1, 0, @PostID, Convert(int, [str]), @BlogID
FROM iter_charlist_to_table(@CategoryList,',')
WHERE 
	Convert(int, [str]) not in (SELECT CategoryID FROM blog_Links WHERE PostID = @PostID AND BlogID = @BlogID)




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertLinkCategoryList]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_InsertPingTrackEntry
(
	@Title nvarchar(255)
	, @TitleUrl nvarchar(255)
	, @Text ntext
	, @SourceUrl nvarchar(200)
	, @PostType int
	, @Author nvarchar(50)
	, @Email nvarchar(50)
	, @SourceName nvarchar(200)
	, @Description nvarchar(500)
	, @BlogID int
	, @DateAdded datetime
	, @ParentID int
	, @PostConfig int
	, @EntryName nvarchar(150)
	, @ID int output
)
AS

--Do not insert EntryNames. No needed for comments AND tracks. To messy anyway

SET @ID = -1

IF NOT EXISTS(SELECT [ID] FROM blog_Content WHERE TitleUrl = @TitleUrl AND ParentID = @ParentID)
BEGIN

IF(LTRIM(RTRIM(@Description)) = '')
SET @Description = NULL

INSERT INTO blog_Content 
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
	,DateUpdated
	, SourceName
	, [Description]
	, ParentID
	, BlogID
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
	, @ParentID
	, @BlogID
)

SELECT @ID = SCOPE_IDENTITY()

UPDATE blog_Content
SET FeedBackCount = FeedBackCount + 1 
WHERE [ID] = @ParentID

END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertPingTrackEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_InsertPostCategoryByName
(
	@Title nvarchar(150)
	, @PostID int
	, @BlogID int
)
AS
DECLARE @CategoryID int
SELECT @CategoryID = CategoryID FROM blog_LinkCategories WHERE Title = @Title AND BlogID = @BlogID AND CategoryType = 1

if(@CategoryID is NULL)
BEGIN

EXEC blog_InsertCategory @Title, 1, @BlogID, 1, NULL, @CategoryID = @CategoryID output

END

DECLARE @LinkID int
EXEC blog_InsertLink NULL, NULL, NULL, 1, 0, @CategoryID, @PostID, @BlogID, @LinkID output


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertPostCategoryByName]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROC blog_InsertReferral
(
	@EntryID int,
	@BlogID int,
	@Url nvarchar(255)
)

AS

DECLARE @UrlID int

if(@Url is not NULL)
BEGIN
	EXEC blog_GetUrlID @Url, @UrlID = @UrlID output
END

if(@UrlID is not NULL)
BEGIN

	IF EXISTS(SELECT EntryID FROM blog_Referrals WHERE EntryID = @EntryID AND BlogID = @BlogID AND UrlID = @UrlID)
	BEGIN
		Update blog_Referrals
		Set [Count] = [Count] + 1, LastUpdated = getdate()
		WHERE EntryID = @EntryID AND BlogID = @BlogID AND UrlID = @UrlID
	END
	else
	BEGIN
		Insert blog_Referrals (EntryID, BlogID, UrlID, [Count], LastUpdated)
		       values (@EntryID, @BlogID, @UrlID, 1, getdate())
	END


END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertReferral]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_InsertViewStats
(
	@BlogID int,
	@PageType tinyint,
	@PostID int,
	@Day datetime,
	@Url nvarchar(255)
)
AS

DECLARE @UrlID int

if(@Url is not NULL)
BEGIN
	EXEC blog_GetUrlID @Url, @UrlID = @UrlID output
END
if(@UrlID is NULL)
set @UrlID = -1


IF EXISTS (SELECT BlogID FROM blog_ViewStats WHERE BlogID = @BlogID AND PageType = @PageType AND PostID = @PostID AND [Day] = @Day AND UrlID = @UrlID)
BEGIN
	Update blog_ViewStats
	Set [Count] = [Count] + 1
	WHERE BlogID = @BlogID AND PageType = @PageType AND PostID = @PostID AND [Day] = @Day AND UrlID = @UrlID
END
Else
BEGIN
	Insert blog_ViewStats (BlogID, PageType, PostID, [Day], UrlID, [Count])
	Values (@BlogID, @PageType, @PostID, @Day, @UrlID, 1)
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_InsertViewStats]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_StatsSummary
(
	@BlogID int
)
AS
DECLARE @ReferralCount int
DECLARE @WebCount int
DECLARE @AggCount int

SELECT @ReferralCount = Sum([Count]) FROM blog_Referrals WHERE BlogID = @BlogID

SELECT @WebCount = Sum(WebCount), @AggCount = Sum(AggCount) FROM blog_EntryViewCount WHERE BlogID = @BlogID

SELECT @ReferralCount AS 'ReferralCount', @WebCount AS 'WebCount', @AggCount AS 'AggCount'


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_StatsSummary]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC blog_TrackEntry
(
	@EntryID int,
	@BlogID int,
	@Url nvarchar(255),
	@IsWeb bit
)

AS

if(@Url is not NULL AND @IsWeb = 1)
BEGIN
	EXEC blog_InsertReferral @EntryID, @BlogID, @Url
END

EXEC blog_InsertEntryViewCount @EntryID, @BlogID, @IsWeb





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_TrackEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



/*
Subtext 1.0 will only support single user blogs, thus this 
proc will make sure there is only one blog in the system 
AND will fail to add a blog if one already exists.
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

AS

IF(NOT EXISTS (SELECT * FROM blog_config))
BEGIN
	DECLARE @Flag int
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

GRANT  EXECUTE  ON [dbo].[blog_UTILITY_AddBlog]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_UpdateCategory
(
	@CategoryID int,
	@Title nvarchar(150),
	@Active bit,
	@CategoryType tinyint,
	@Description nvarchar(1000),
	@BlogID int
)
AS
UPDATE blog_LinkCategories 
SET 
	blog_LinkCategories.Title = @Title, 
	blog_LinkCategories.Active = @Active,
	blog_LinkCategories.CategoryType = @CategoryType,
	blog_LinkCategories.[Description] = @Description
WHERE   
	blog_LinkCategories.CategoryID=@CategoryID AND blog_LinkCategories.BlogID = @BlogID



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_UpdateCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC blog_UpdateConfig
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
	, @TimeZone int
	, @ItemCount int
	, @News nText
	, @LastUpdated datetime
	, @SecondaryCss nText
	, @SkinCssFile varchar(100)
	, @Flag int
	, @BlogID int
	, @LicenseUrl nvarchar(64)
	, @DaysTillCommentsClose int = NULL
)
AS
Update blog_Config
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
WHERE BlogID = @BlogID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_UpdateConfig]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_UpdateConfigUpdateTime
(
	@BlogID int,
	@LastUpdated datetime
)
AS
UPDATE blog_Config
SET LastUpdated = @LastUpdated
WHERE blogid = @blogid


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_UpdateConfigUpdateTime]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_UpdateEntry
(
	@ID int
	, @Title nvarchar(255)
	, @TitleUrl nvarchar(255)
	, @Text ntext
	, @SourceUrl nvarchar(200)
	, @PostType int
	, @Author nvarchar(50)
	, @Email nvarchar(50)
	, @Description nvarchar(500)
	, @SourceName nvarchar(200)
	, @DateUpdated datetime
	, @PostConfig int
	, @ParentID int
	, @EntryName nvarchar(150)
	, @BlogID int
)
AS

if(@EntryName is not NULL)
BEGIN
	IF EXISTS(SELECT EntryName FROM blog_Content WHERE BlogID = @BlogID AND EntryName = @EntryName AND [ID] <> @ID)
	BEGIN
		RAISERROR('The EntryName you entry is already in use with in this Blog. Please pick a unique EntryName.',11,1) 
		RETURN 1
	END
END

if(Ltrim(Rtrim(@Description)) = '')
set @Description = NULL
UPDATE blog_Content 
SET 
	blog_Content.Title = @Title 
	, blog_Content.TitleUrl = @TitleUrl 
	, blog_Content.[Text] = @Text 
	, blog_Content.SourceUrl = @SourceUrl 
	, blog_Content.PostType = @PostType
	, blog_Content.Author = @Author 
	, blog_Content.Email = @Email 
	, blog_Content.[Description] = @Description
	, blog_Content.DateUpdated = @DateUpdated
	, blog_Content.PostConfig = @PostConfig
	, blog_Content.ParentID = @ParentID
	, blog_Content.SourceName = @SourceName
	, blog_Content.EntryName = @EntryName
WHERE 	
	blog_Content.[ID]=@ID AND blog_Content.BlogID = @BlogID
EXEC blog_UpdateConfigUpdateTime @blogID, @DateUpdated



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_UpdateEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_UpdateImage
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
AS
Update blog_Images
Set
	Title = @Title,
	CategoryID = @CategoryID,
	Width = @Width,
	Height = @Height,
	[File] = @File,
	Active = @Active
	
WHERE
	ImageID = @ImageID AND BlogID = @BlogID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_UpdateImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC blog_UpdateKeyWord
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

AS

Update blog_keywords 
	Set
		Word = @Word,
		[Text] = @Text,
		ReplaceFirstTimeOnly = @ReplaceFirstTimeOnly,
		OpenInNewWindow = @OpenInNewWindow,
		CaseSensitive = @CaseSensitive,
		Url = @Url,
		Title = @Title
	WHERE
		BlogID = @BlogID AND KeyWordID = @KeyWordID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_UpdateKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC blog_UpdateLink
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
AS
UPDATE blog_Links 
SET 
	blog_Links.Title = @Title, 
	blog_Links.Url = @Url, 
	blog_Links.Rss = @Rss, 
	blog_Links.Active = @Active,
	blog_Links.NewWindow = @NewWindow, 
	blog_Links.CategoryID = @CategoryID
WHERE  
	blog_Links.LinkID = @LinkID AND blog_Links.BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_UpdateLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROC blog_Utility_GetUnHashedPasswords
AS

SELECT BlogID, Password FROM blog_COnfig WHERE Flag & 8 = 0




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_Utility_GetUnHashedPasswords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROC blog_Utility_UpdateToHashedPassword
(
	@Password nvarchar(100),
	@BlogID int
)

AS

Update blog_Config
Set 
	Password = @Password,
	Flag = Flag | 8 
WHERE blogid = @blogid



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_Utility_UpdateToHashedPassword]  TO [public]
GO

