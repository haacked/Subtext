if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[iter_charlist_to_table]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[iter_charlist_to_table]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_VersionAdd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_VersionAdd]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_VersionGetCurrent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_VersionGetCurrent]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetHost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetHost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UpdateHost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UpdateHost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetCommentByChecksumHash]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetCommentByChecksumHash]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableBlogs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableBlogs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetBlogsByHost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetBlogsByHost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetBlogById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetBlogById]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_DeleteCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_DeleteCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_DeleteImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_DeleteImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_DeleteImageCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_DeleteImageCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_DeleteKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_DeleteKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_DeleteLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_DeleteLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_DeleteLinksByPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_DeleteLinksByPostID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_DeletePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_DeletePost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetActiveCategoriesWithLinkCollection]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetActiveCategoriesWithLinkCollection]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetAllCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetAllCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetBlogKeyWords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetBlogKeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetCategoryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetCategoryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetConditionalEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetConditionalEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetConditionalEntriesByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetConditionalEntriesByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetConfig]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetEntriesByDayRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetEntriesByDayRange]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetEntryCollectionByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetEntryCollectionByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetEntryWithCategoryTitles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetEntryWithCategoryTitles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetEntryWithCategoryTitlesByEntryName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetEntryWithCategoryTitlesByEntryName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetFeedBack]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetFeedBack]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetImageCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetImageCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetLinkCollectionByPostID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetLinkCollectionByPostID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetLinksByActiveCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetLinksByActiveCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetLinksByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableEntriesByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableEntriesByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableFeedback]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableFeedback]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableKeyWords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableKeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableLinks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableLinksByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableReferrers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableReferrers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPageableReferrersByEntryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPageableReferrersByEntryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPostsByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPostsByCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPostsByCategoryIDByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPostsByCategoryIDByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPostsByCategoryName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPostsByCategoryName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPostsByCategoryNameByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPostsByCategoryNameByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPostsByDayRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPostsByDayRange]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPostsByMonth]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPostsByMonth]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPostsByMonthArchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPostsByMonthArchive]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetPostsByYearArchive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetPostsByYearArchive]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetRecentEntries]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetRecentEntries]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetRecentEntriesByDateUpdated]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetRecentEntriesByDateUpdated]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetRecentEntriesWithCategoryTitles]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetRecentEntriesWithCategoryTitles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetSingleDay]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetSingleDay]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetSingleEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetSingleEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetSingleEntryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetSingleEntryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetSingleImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetSingleImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetSingleLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetSingleLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_GetUrlID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_GetUrlID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertEntryViewCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertEntryViewCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertLinkCategoryList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertLinkCategoryList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertPingTrackEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertPingTrackEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertPostCategoryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertPostCategoryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertReferral]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertReferral]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_InsertViewStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_InsertViewStats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_StatsSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_StatsSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_TrackEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_TrackEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UTILITY_AddBlog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UTILITY_AddBlog]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UpdateCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UpdateCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UpdateConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UpdateConfig]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UpdateConfigUpdateTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UpdateConfigUpdateTime]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UpdateEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UpdateEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UpdateImage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UpdateImage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UpdateKeyWord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UpdateKeyWord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_UpdateLink]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_UpdateLink]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_Utility_GetUnHashedPasswords]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_Utility_GetUnHashedPasswords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[subtext_Utility_UpdateToHashedPassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_Utility_UpdateToHashedPassword]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_AddLogEntry]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[subtext_AddLogEntry]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

--Found at: http://www.algonet.se/~sommar/arrays-in-sql.html
  CREATE FUNCTION [dbo].[iter_charlist_to_table]
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


CREATE PROC [dbo].[subtext_DeleteCategory]
(
	@CategoryID int,
	@BlogID int
)
AS
DELETE [dbo].[subtext_Links] FROM [dbo].[subtext_Links] WHERE CategoryID = @CategoryID AND BlogID = @BlogID
DELETE [dbo].[subtext_LinkCategories] FROM [dbo].[subtext_LinkCategories] WHERE subtext_LinkCategories.CategoryID = @CategoryID AND subtext_LinkCategories.BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_DeleteCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_DeleteImage]
(
	@BlogID int,
	@ImageID int
)
AS
DELETE [dbo].[subtext_Images] 
FROM [dbo].[subtext_Images] 
WHERE	ImageID = @ImageID 
	AND BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_DeleteImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_DeleteImageCategory]
(
	@CategoryID int,
	@BlogID int
)
AS
DELETE [dbo].[subtext_Images] FROM [dbo].[subtext_Images] WHERE subtext_Images.CategoryID = @CategoryID AND subtext_Images.BlogID = @BlogID
DELETE [dbo].[subtext_LinkCategories] FROM [dbo].[subtext_LinkCategories] WHERE subtext_LinkCategories.CategoryID = @CategoryID AND subtext_LinkCategories.BlogID = @BlogID



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_DeleteImageCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_DeleteKeyWord]
(
	@KeyWordID int,
	@BlogID int
)

AS

DELETE FROM [dbo].[subtext_KeyWords] WHERE BLOGID = @BlogID AND KeyWordID = @KeyWordID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_DeleteKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_DeleteLink]
(
	@LinkID int,
	@BlogID int
)
AS
DELETE [dbo].[subtext_Links] FROM [dbo].[subtext_Links] WHERE [LinkID] = @LinkID AND BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_DeleteLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_DeleteLinksByPostID]
(
	@PostID int,
	@BlogID int
)
AS
Set NoCount ON
DELETE [dbo].[subtext_Links] FROM [dbo].[subtext_Links] WHERE PostID = @PostID AND BlogID = @BlogID



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_DeleteLinksByPostID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Deletes a record FROM [dbo].[subtext_content], whether it be a post, a comment, etc..
*/
CREATE PROC [dbo].[subtext_DeletePost]
(
	@ID int,
	@BlogID int
)
AS

DECLARE @ParentID int, @PostType int

SELECT @ParentID = ParentID, @PostType = PostType FROM [dbo].[subtext_Content] WHERE [ID] = @ID

-- PostType 3 = Comment
-- PostType 4 = PingBack

IF(@PostType = 3 or @PostType = 4)
BEGIN
	UPDATE [dbo].[subtext_Content]
	SET FeedBackCount = FeedBackCount - 1
	WHERE [ID] = @ParentID
END
ELSE
BEGIN
	DELETE FROM [dbo].[subtext_Content] WHERE ParentID = @ID
	-- This is a refactoring in progress 
	-- to remove meaning from PostId = -1
	DECLARE @PostID int
	SET @PostID = @ID
	IF @ID = -1
		SET @PostID = NULL
	DELETE FROM [dbo].[subtext_Links] WHERE PostID = @PostID
	DELETE FROM [dbo].[subtext_EntryViewCount] WHERE EntryID = @ID
	DELETE FROM [dbo].[subtext_Referrals] WHERE EntryID = @ID
END

DELETE FROM [dbo].[subtext_Content] WHERE [ID] = @ID AND [BlogID] = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_DeletePost]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetActiveCategoriesWithLinkCollection]
(
	@BlogID int
)
AS
SELECT subtext_LinkCategories.CategoryID
	, subtext_LinkCategories.Title
	, subtext_LinkCategories.Active
	, subtext_LinkCategories.CategoryType
	, subtext_LinkCategories.[Description]
FROM [dbo].[subtext_LinkCategories]
WHERE	
			subtext_LinkCategories.Active= 1 
	AND		subtext_LinkCategories.BlogID = @BlogID 
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
FROM [dbo].[subtext_Links] links
	INNER JOIN subtext_LinkCategories categories ON links.CategoryID = categories.CategoryID
WHERE 
		links.Active = 1 
	AND categories.Active = 1
	AND categories.BlogID = @BlogID 
	AND links.BlogID = @BlogID 
	AND categories.CategoryType = 0
ORDER BY 
	links.Title;



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetActiveCategoriesWithLinkCollection]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetAllCategories]
(
	@BlogID int
	, @IsActive bit
	, @CategoryType tinyint
)
As
SELECT subtext_LinkCategories.CategoryID
	, subtext_LinkCategories.Title
	, subtext_LinkCategories.Active
	, subtext_LinkCategories.CategoryType
	, subtext_LinkCategories.[Description]
FROM [dbo].[subtext_LinkCategories]
WHERE subtext_LinkCategories.BlogID = @BlogID 
	AND subtext_LinkCategories.CategoryType = @CategoryType 
	AND subtext_LinkCategories.Active <> CASE @IsActive WHEN 1 THEN 0 ELSE -1 END
ORDER BY subtext_LinkCategories.Title;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetAllCategories]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetBlogKeyWords]
(
	@BlogID int
)
AS

SELECT 
	KeyWordID, Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID
FROM
	subtext_keywords
WHERE 
	BlogID = @BlogID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetBlogKeyWords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetCategory]
(
	@CategoryID int,
	@IsActive bit,
	@BlogID int
)
AS
SELECT	subtext_LinkCategories.CategoryID
		, subtext_LinkCategories.Title
		, subtext_LinkCategories.Active
		, subtext_LinkCategories.CategoryType
		, subtext_LinkCategories.[Description]
FROM [dbo].[subtext_LinkCategories]
WHERE subtext_LinkCategories.CategoryID=@CategoryID 
	AND subtext_LinkCategories.BlogID = @BlogID 
	AND subtext_LinkCategories.Active <> CASE @IsActive WHEN 0 THEN -1 else 0 END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetCategoryByName] 
(
	@CategoryName nvarchar(150),
	@IsActive bit,
	@BlogID int
)
AS
SELECT	subtext_LinkCategories.CategoryID
		, subtext_LinkCategories.Title
		, subtext_LinkCategories.Active
		, subtext_LinkCategories.CategoryType
		, subtext_LinkCategories.[Description]
FROM [dbo].[subtext_LinkCategories]
WHERE	subtext_LinkCategories.Title=@CategoryName 
	AND subtext_LinkCategories.BlogID = @BlogID 
	AND subtext_LinkCategories.Active <> CASE @IsActive WHEN 0 THEN -1 else 0 END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetCategoryByName]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetConditionalEntries]
(
	@ItemCount int 
	, @PostType int
	, @PostConfig int
	, @BlogID int
)
AS

SET ROWCOUNT @ItemCount
SELECT BlogID
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
FROM [dbo].[subtext_Content]
WHERE	PostType = @PostType 
	AND BlogID   = @BlogID
	AND PostConfig & @PostConfig = @PostConfig
ORDER BY ISNULL(DateSyndicated, DateUpdated) DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetConditionalEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
/*
Returns the blog that matches the given host/application combination.

@Strict -- If 0, then we return the one and only blog if there's one and only blog.
*/
CREATE PROC [dbo].[subtext_GetConfig]
(
	@Host nvarchar(100)
	, @Application nvarchar(50)
	, @Strict bit = 1 
)
AS

IF (@Strict = 0) AND (1 = (SELECT COUNT(1) FROM [dbo].[subtext_config]))
BEGIN
	-- Return the one and only record
	SELECT
		subtext_Config.BlogID
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
	FROM [dbo].[subtext_Config]
END
ELSE
BEGIN
	SELECT
		BlogID
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
	FROM [dbo].[subtext_Config]
	WHERE	Host = @Host
		AND Application = @Application
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetConfig]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetEntriesByDayRange]
(
	@StartDate datetime,
	@StopDate datetime,
	@PostType int,
	@IsActive bit,
	@BlogID int
)
AS
SELECT	BlogID
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
FROM [dbo].[subtext_Content]
WHERE 
	(
		DateAdded > @StartDate 
		AND DateAdded < DateAdd(day, 1, @StopDate)
	)
	AND PostType=@PostType 
	AND BlogID = @BlogID 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetEntriesByDayRange]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetEntryCollectionByDateUpdated]
(
	@ItemCount int,
	@IsActive bit,
	@PostType int, 
	@DateUpdated datetime,
	@BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT BlogID
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
FROM [dbo].[subtext_Content]
WHERE 
	PostType=@PostType 
	AND BlogID = @BlogID
	AND DateUpdated > @DateUpdated
	AND PostConfig & 1  <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [dateupdated] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetEntryCollectionByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetEntryWithCategoryTitles]
(
	@PostID int
	, @IsActive bit
	, @BlogID int
)
AS
SELECT	BlogID
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
		, FeedBackCount
		, PostConfig
		, EntryName
		, ParentID 
		, ContentChecksumHash
		, DateSyndicated
FROM [dbo].[subtext_Content]
WHERE	[ID] = @PostID 
	AND  BlogID = @BlogID 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [dateadded] DESC

SELECT	c.Title
		, PostID = ISNULL(l.PostID, -1)
FROM [dbo].[subtext_Links] l
INNER JOIN subtext_LinkCategories c ON l.CategoryID = c.CategoryID
WHERE l.PostID = @PostID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetEntryWithCategoryTitles]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetEntryWithCategoryTitlesByEntryName]
(
	@EntryName nvarchar(150)
	, @IsActive bit
	, @BlogID int
)
AS
DECLARE @PostID int

SELECT	BlogID
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
		, FeedBackCount
		, PostConfig
		, EntryName
		, ParentID 
		, ContentChecksumHash
		, DateSyndicated
FROM [dbo].[subtext_Content]
WHERE	EntryName = @EntryName 
	AND  BlogID = @BlogID 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [dateadded] DESC

SELECT	c.Title
		, PostID = ISNULL(l.PostID, -1)
FROM [dbo].[subtext_Links] l
	INNER JOIN [dbo].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID
	INNER JOIN [dbo].[subtext_Content] content ON l.PostID = content.[ID]
WHERE EntryName = @EntryName


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetEntryWithCategoryTitlesByEntryName]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetFeedBack]
(
	@ParentID int
	, @BlogID int
)
AS

SELECT	BlogID
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
		, FeedBackCount
		, PostConfig
		, EntryName
		, ParentID 
		, ContentChecksumHash
		, DateSyndicated
FROM [dbo].[subtext_Content]
WHERE BlogID = @BlogID AND PostConfig & 1 = 1 AND ParentID = @ParentID
ORDER BY [ID]


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetFeedBack]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetImageCategory]
(
	@CategoryID int
	, @IsActive bit
	, @BlogID int
)
AS
EXEC [dbo].[subtext_GetCategory] @CategoryID, @IsActive, @BlogID

SELECT	Title
		, CategoryID
		, Height
		, Width
		, [File]
		, Active
		, ImageID 
FROM [dbo].[subtext_Images]  
WHERE CategoryID = @CategoryID 
	AND BlogID = @BlogID 
	AND Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetImageCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetKeyWord]
(
	@KeyWordID int
	, @BlogID int
)
AS

SELECT 
	KeyWordID, Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID
FROM
	subtext_keywords
WHERE 
	KeyWordID = @KeyWordID AND BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetLinkCollectionByPostID]
(
	@PostID int,
	@BlogID int
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
FROM [dbo].[subtext_Links]
WHERE PostID = @PostID 
	AND BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetLinkCollectionByPostID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetLinksByActiveCategoryID]
(
	@CategoryID int
	, @BlogID int
)
AS
EXEC [dbo].[subtext_GetCategory] @CategoryID, 0, @BlogID
SELECT	LinkID
		, Title
		, Url
		, Rss
		, Active
		, CategoryID
		, PostID = ISNULL(PostID, -1)
FROM [dbo].[subtext_Links]
WHERE Active = 1 
	AND CategoryID = @CategoryID 
	AND BlogID = @BlogID
ORDER BY Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetLinksByActiveCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetLinksByCategoryID]
(
	@CategoryID int
	, @BlogID int
)
AS
EXEC [dbo].[subtext_GetCategory] @CategoryID, @BlogID
SELECT	LinkID
		, Title
		, Url
		, Rss
		, Active
		, NewWindow
		, CategoryID
		, PostId = ISNULL(PostID, -1)
FROM [dbo].[subtext_Links]
WHERE	CategoryID = @CategoryID 
	AND BlogID = @BlogID
ORDER BY Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetLinksByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
I think this proc gets a page of blog posts 
within the admin section.
*/
CREATE PROC [dbo].[subtext_GetPageableEntries]
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
	FROM [dbo].[subtext_Content] 
	WHERE 	blogID = @BlogID 
		AND PostType = @PostType
	ORDER BY [ID]
END
ELSE
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	[ID] 
	FROM [dbo].[subtext_Content]
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
		, content.ContentChecksumHash
		, content.DateSyndicated
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated
		
FROM [dbo].[subtext_Content] content
    	INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
	Left JOIN  subtext_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogID = @BlogID)
WHERE 	content.BlogID = @BlogID 
	AND tmp.TempID > @PageLowerBound 
	AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs


SELECT COUNT([ID]) AS TotalRecords
FROM [dbo].[subtext_Content] 
WHERE 	blogID = @BlogID 
	AND PostType = @PostType 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPageableEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPageableEntriesByCategoryID]
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
	FROM [dbo].[subtext_Content] blog
		INNER JOIN subtext_Links links ON (blog.[ID] = ISNULL(links.PostID, -1))
		INNER JOIN subtext_LinkCategories cats ON (links.CategoryID = cats.CategoryID)
	WHERE 	blog.blogID = @BlogID 
		AND blog.PostType = @PostType
		AND cats.CategoryID = @CategoryID
	ORDER BY blog.[ID]
END
ELSE
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	blog.[ID] 
	FROM [dbo].[subtext_Content] blog
		INNER JOIN subtext_Links links ON (blog.[ID] = ISNULL(links.PostID, -1))
		INNER JOIN subtext_LinkCategories cats ON (links.CategoryID = cats.CategoryID)
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
		, content.ContentChecksumHash
		, content.DateSyndicated
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated

FROM [dbo].[subtext_Content] content
    INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
	LEFT JOIN  subtext_EntryViewCount vc ON (content.[ID] = vc.EntryID AND vc.BlogID = @BlogID)
WHERE	content.BlogID = @BlogID 
	AND	tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs

SELECT 	COUNT(blog.[ID]) AS TotalRecords
FROM [dbo].[subtext_Content] blog
	INNER JOIN subtext_Links links ON (blog.[ID] = ISNULL(links.PostID, -1))
	INNER JOIN subtext_LinkCategories cats ON (links.CategoryID = cats.CategoryID)
WHERE 	blog.blogID = @BlogID 
	AND blog.PostType = @PostType
	AND cats.CategoryID = @CategoryID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPageableEntriesByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPageableFeedback]
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
	FROM [dbo].[subtext_Content] 
	WHERE 	blogID = @BlogID 
			AND (PostType = 3 or PostType = 4)

	ORDER BY [DateAdded]
END
ELSE
BEGIN
	INSERT INTO #TempPagedEntryIDs (EntryID)
	SELECT	[ID] 
	FROM [dbo].[subtext_Content]
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
		, content.ContentChecksumHash
		, content.DateSyndicated
FROM [dbo].[subtext_Content] content
    INNER JOIN #TempPagedEntryIDs tmp ON (content.[ID] = tmp.EntryID)
WHERE 	content.BlogID = @BlogID 
	AND tmp.TempID > @PageLowerBound 
	AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedEntryIDs

SELECT 	COUNT([ID]) AS TotalRecords
FROM [dbo].[subtext_Content] 
WHERE 	blogID = @BlogID 
	AND (PostType = 3 or PostType = 4)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPageableFeedback]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPageableKeyWords]
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
	FROM [dbo].[subtext_KeyWords] 
	WHERE 	blogID = @BlogID 
	ORDER BY Word
END
Else
BEGIN
	INSERT INTO #TempPagedKeyWordIDs (KeyWordID)
	SELECT	KeyWordID
	FROM [dbo].[subtext_KeyWords] 
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
	subtext_KeyWords words
	INNER JOIN #TempPagedKeyWordIDs tmp ON (words.KeyWordID = tmp.KeyWordID)
WHERE 	
		words.blogID = @BlogID 
	AND tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY
	tmp.TempID
 
DROP TABLE #TempPagedKeyWordIDs

SELECT 	COUNT([KeywordID]) AS 'TotalRecords'
FROM [dbo].[subtext_KeyWords] 
WHERE 	blogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPageableKeyWords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPageableLinks]
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
	FROM [dbo].[subtext_Links] 
	WHERE 	blogID = @BlogID 
		AND PostID IS NULL
	ORDER BY Title
END
ELSE
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM [dbo].[subtext_Links]
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
	subtext_Links links
	INNER JOIN #TempPagedLinkIDs tmp ON (links.LinkID = tmp.LinkID)
WHERE 	
		links.blogID = @BlogID 
	AND tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY
	tmp.TempID
 
DROP TABLE #TempPagedLinkIDs

SELECT 	COUNT([LinkID]) AS TotalRecords
FROM [dbo].[subtext_Links] 
WHERE 	blogID = @BlogID
	AND PostID IS NULL


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPageableLinks]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPageableLinksByCategoryID]
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
	FROM [dbo].[subtext_Links] 
	WHERE 	blogID = @BlogID 
		AND CategoryID = @CategoryID
		AND PostID IS NULL
	ORDER BY Title
END
ELSE
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM [dbo].[subtext_Links]
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
	subtext_Links links
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
FROM [dbo].[subtext_Links] 
WHERE 	blogID = @BlogID 
	AND CategoryID = @CategoryID 
	AND PostID IS NULL


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPageableLinksByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


--SELECT Top 5 * FROM [dbo].[subtext_Referrals] ORDER BY LastUpdated DESC

CREATE PROC [dbo].[subtext_GetPageableReferrers]
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

CREATE TABLE #tempsubtext_Referrals 
(
	TempID INT IDENTITY(1, 1) NOT NULL,
	[EntryID] [int] NOT NULL ,
	[UrlID] [int] NOT NULL ,
	[Count] [int] NOT NULL ,
	[LastUpdated] [datetime] NOT NULL
) 

INSERT INTO #tempsubtext_Referrals (EntryID,UrlID, [Count], LastUpdated)
  SELECT EntryID, UrlID, [Count], LastUpdated
  FROM [dbo].[subtext_Referrals]
  WHERE subtext_Referrals.BlogID = @BlogID
  ORDER BY LastUpdated DESC
   
SELECT	u.URL,
	c.Title,
	r.EntryID,
	c.EntryName,
	LastUpdated,
	[Count]
FROM [dbo].[subtext_Content] c,
	#tempsubtext_Referrals r,
	subtext_URLs u
WHERE r.EntryID = c.ID AND
      c.BlogID = @BlogID
  AND r.UrlID = u.UrlID
  AND r.TempID > @PageLowerBound
  AND r.TempID < @PageUpperBound

ORDER BY TempID


SELECT COUNT(*) AS 'TotalRecords' FROM #tempsubtext_Referrals


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPageableReferrers]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPageableReferrersByEntryID] 
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

CREATE TABLE #tempsubtext_Referrals 
(
	TempID INT IDENTITY(1, 1) NOT NULL,
	[UrlID] [int] NOT NULL ,
	[Count] [int] NOT NULL ,
	[LastUpdated] [datetime] NOT NULL
) 

INSERT INTO #tempsubtext_Referrals 
(
	UrlID
	, [Count]
	, LastUpdated
)
  SELECT UrlID
	, [Count]
	, LastUpdated
  FROM [dbo].[subtext_Referrals]
  WHERE subtext_Referrals.BlogID = @BlogID AND subtext_Referrals.EntryID = @EntryID
  ORDER BY LastUpdated DESC
   
SELECT	u.URL
	, c.Title
	, c.EntryName
	, [EntryId] = @EntryID
	, [Count]
	, r.LastUpdated
	
FROM [dbo].[subtext_Content] c
	, #tempsubtext_Referrals r
	, subtext_URLs u
WHERE c.ID = @EntryID 
	AND c.BlogID = @BlogID
	AND r.UrlID = u.UrlID
	AND r.TempID > @PageLowerBound
	AND r.TempID < @PageUpperBound
	ORDER BY TempID

SELECT COUNT(*) AS 'TotalRecords' FROM #tempsubtext_Referrals


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPageableReferrersByEntryID]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetPostsByCategoryID]
(
	@ItemCount int
	, @CategoryID int
	, @IsActive bit
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
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
	, content.FeedBackCount
	, content.ParentID
	, content.PostConfig
	, content.EntryName 
	, content.ContentChecksumHash
	, content.DateSyndicated
FROM [dbo].[subtext_Content] content WITH (NOLOCK)
	INNER JOIN subtext_Links links WITH (NOLOCK) ON content.ID = ISNULL(links.PostID, -1)
	INNER JOIN subtext_LinkCategories categories WITH (NOLOCK) ON links.CategoryID = categories.CategoryID
WHERE  content.BlogID = @BlogID 
	AND content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END AND categories.CategoryID = @CategoryID
ORDER BY content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPostsByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPostsByCategoryIDByDateUpdated]
(
	@ItemCount int
	, @CategoryID int
	, @IsActive bit
	, @DateUpdated datetime
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
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
	, content.FeedBackCount
	, content.ParentID
	, content.PostConfig
	, content.EntryName 
	, content.ContentChecksumHash
	, content.DateSyndicated
FROM [dbo].[subtext_Content] content
	INNER JOIN subtext_Links ON content.ID = ISNULL(subtext_Links.PostID, -1)
	INNER JOIN subtext_LinkCategories ON subtext_Links.CategoryID = subtext_LinkCategories.CategoryID
WHERE  content.BlogID = @BlogID 
	AND content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END 
	AND subtext_LinkCategories.CategoryID = @CategoryID 
	AND content.DateUpdated > @DateUpdated
ORDER BY content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPostsByCategoryIDByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPostsByCategoryName]
(
	@ItemCount int,
	@CategoryName nvarchar(150),
	@IsActive bit,
	@BlogID int
)
AS
SET ROWCOUNT @ItemCount
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
		, content.FeedBackCount
		, content.ParentID
		, content.PostConfig
		, content.EntryName 
		, content.ContentChecksumHash
		, content.DateSyndicated
FROM [dbo].[subtext_Content] content
	INNER JOIN subtext_Links ON content.ID = ISNULL(subtext_Links.PostID, -1)
	INNER JOIN subtext_LinkCategories ON subtext_Links.CategoryID = subtext_LinkCategories.CategoryID
WHERE	content.BlogID = @BlogID 
	AND content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END 
	AND subtext_LinkCategories.Title = @CategoryName
ORDER BY content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPostsByCategoryName]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetPostsByCategoryNameByDateUpdated]
(
	@ItemCount int
	, @CategoryName nvarchar(150)
	, @IsActive bit
	, @DateUpdated datetime
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
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
		, content.FeedBackCount
		, content.ParentID
		, content.PostConfig
		, content.EntryName 
		, content.ContentChecksumHash
		, content.DateSyndicated
FROM [dbo].[subtext_Content] content
	INNER JOIN subtext_Links ON content.ID = ISNULL(subtext_Links.PostID, -1)
	INNER JOIN subtext_LinkCategories ON subtext_Links.CategoryID = subtext_LinkCategories.CategoryID
WHERE  content.BlogID = @BlogID 
	AND content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END 
	AND subtext_LinkCategories.Title = @CategoryName 
	AND content.DateUpdated > @DateUpdated
ORDER BY content.[ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPostsByCategoryNameByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetPostsByDayRange]
(
	@StartDate datetime,
	@StopDate datetime,
	@BlogID int
)
AS
SELECT	BlogID
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
FROM [dbo].[subtext_Content]
WHERE 
	(
			DateAdded > @StartDate 
		AND DateAdded < DateAdd(day,1,@StopDate)
	)
	AND PostType=1 
	AND BlogID = @BlogID
ORDER BY DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPostsByDayRange]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetPostsByMonth]
(
	@Month int
	, @Year int
	, @BlogID int
)
AS
SELECT	BlogID
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
FROM [dbo].[subtext_Content]
WHERE	PostType=1 
	AND BlogID = @BlogID 
	AND PostConfig & 1 = 1 
	AND Month(DateAdded) = @Month 
	AND Year(DateAdded)  = @Year
ORDER BY DateAdded DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPostsByMonth]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetPostsByMonthArchive]
(
	@BlogID int
)
AS
SELECT Month(DateAdded) AS [Month]
	, Year(DateAdded) AS [Year]
	, 1 AS Day, Count(*) AS [Count] 
FROM [dbo].[subtext_Content] 
WHERE PostType = 1 AND PostConfig & 1 = 1 AND BlogID = @BlogID 
GROUP BY Year(DateAdded), Month(DateAdded) ORDER BY [Year] DESC, [Month] DESC



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPostsByMonthArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetPostsByYearArchive] 
(
	@BlogID int
)
AS
SELECT 1 AS [Month], Year(DateAdded) AS [Year], 1 AS Day, Count(*) AS [Count] FROM [dbo].[subtext_Content] 
WHERE PostType = 1 AND PostConfig & 1 = 1 AND BlogID = @BlogID 
GROUP BY Year(DateAdded) ORDER BY [Year] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetPostsByYearArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

-- Gets recently added entries.
CREATE PROC [dbo].[subtext_GetRecentEntries]
(
	@ItemCount int
	, @IsActive bit
	, @PostType int
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT	BlogID
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
FROM [dbo].[subtext_Content]
WHERE	PostType=@PostType 
	AND BlogID = @BlogID 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [dateadded] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetRecentEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetRecentEntriesByDateUpdated]
(
	@ItemCount int
	, @IsActive bit 
	, @PostType int
	, @DateUpdated datetime
	, @BlogID int
)
AS
SET ROWCOUNT @ItemCount
SELECT	BlogID
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
FROM [dbo].[subtext_Content]
WHERE 
	PostType=@PostType 
	AND BlogID = @BlogID
	AND DateUpdated > @DateUpdated
	AND PostConfig & 1  <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetRecentEntriesByDateUpdated]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetRecentEntriesWithCategoryTitles]
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
SELECT	[ID] 
FROM [dbo].[subtext_Content]
WHERE	PostType=1 
	AND BlogID = @BlogID 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [dateadded] DESC

SELECT	BlogID
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
FROM [dbo].[subtext_Content], #IDs
WHERE [ID] = #IDs.PostID
ORDER BY TempID ASC

SET ROWCOUNT 0

SELECT	c.Title
		, PostId = ISNULL(l.PostID, -1)
FROM [dbo].[subtext_Links] l
	INNER JOIN #IDs ON ISNULL(l.[PostID], -1) = #IDs.[PostID]
	INNER JOIN subtext_LinkCategories c ON l.CategoryID = c.CategoryID
DROP Table #IDs


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetRecentEntriesWithCategoryTitles]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetSingleDay]
(
	@Date datetime
	,@BlogID int
)
AS
SELECT	BlogID
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
FROM [dbo].[subtext_Content]
WHERE Year(DateAdded) = Year(@Date) 
	AND Month(DateAdded) = Month(@Date)
    AND Day(DateAdded) = Day(@Date) 
    And PostType=1
    AND BlogID = @BlogID 
    AND PostConfig & 1 = 1 
ORDER BY DateAdded DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetSingleDay]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_GetSingleEntry]
(
	@ID int
	, @IsActive bit
	, @BlogID int
)
AS
SELECT	BlogID
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
FROM [dbo].[subtext_Content]
WHERE [ID] = @ID 
	AND BlogID = @BlogID 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetSingleEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetSingleEntryByName]
(
	@EntryName nvarchar(150)
	, @IsActive bit
	, @BlogID int
)
AS
SELECT	BlogID
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
FROM [dbo].[subtext_Content]
WHERE [EntryName] = @EntryName 
	AND BlogID = @BlogID 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetSingleEntryByName]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetSingleImage]
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
FROM [dbo].[subtext_Images]  
WHERE ImageID = @ImageID 
	AND BlogID = @BlogID 
	AND  Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetSingleImage]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetSingleLink]
(
	@LinkID int
	, @BlogID int
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
FROM [dbo].[subtext_Links]
WHERE subtext_Links.LinkID = @LinkID AND subtext_Links.BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetSingleLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_GetUrlID]
(
	@Url nvarchar(255)
	, @UrlID int output
)
AS
IF EXISTS(SELECT UrlID FROM [dbo].[subtext_Urls] WHERE Url = @Url)
BEGIN
	SELECT @UrlID = UrlID FROM [dbo].[subtext_Urls] WHERE Url = @Url
END
Else
BEGIN
	INSERT subtext_Urls VALUES (@Url)
	SELECT @UrlID = SCOPE_IDENTITY()
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetUrlID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertCategory]
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
INSERT INTO subtext_LinkCategories 
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

GRANT  EXECUTE  ON [dbo].[subtext_InsertCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertEntryViewCount]-- 1, 0, 1
(
	@EntryID int,
	@BlogID int,
	@IsWeb bit
)

AS

BEGIN
	--Do we have an existing entry in the subtext_InsertEntryViewCount table?
	IF EXISTS(SELECT EntryID FROM [dbo].[subtext_EntryViewCount] WHERE EntryID = @EntryID AND BlogID = @BlogID)
	BEGIN
		if(@IsWeb = 1) -- Is this a web view?
		BEGIN
			UPDATE [dbo].[subtext_EntryViewCount]
			Set [WebCount] = [WebCount] + 1, WebLastUpdated = getdate()
			WHERE EntryID = @EntryID AND BlogID = @BlogID
		END
		else
		BEGIN
			UPDATE [dbo].[subtext_EntryViewCount]
			Set [AggCount] = [AggCount] + 1, AggLastUpdated = getdate()
			WHERE EntryID = @EntryID AND BlogID = @BlogID
		END
	END
	else
	BEGIN
		if(@IsWeb = 1) -- Is this a web view
		BEGIN
			Insert subtext_EntryViewCount (EntryID, BlogID, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogID, 1, 0, getdate(), NULL)
		END
		else
		BEGIN
			Insert subtext_EntryViewCount (EntryID, BlogID, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogID, 0, 1, NULL, getdate())
		END
	END


END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertEntryViewCount]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertImage]
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
Insert subtext_Images
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

GRANT  EXECUTE  ON [dbo].[subtext_InsertImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertKeyWord]
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

Insert subtext_keywords 
	(Word,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogID)
Values
	(@Word,@Text,@ReplaceFirstTimeOnly,@OpenInNewWindow, @CaseSensitive,@Url,@Title,@BlogID)

SELECT @KeyWordID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertLink]
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

INSERT INTO subtext_Links 
( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogID )
VALUES 
(@Title, @Url, @Rss, @Active, @NewWindow, @PostID, @CategoryID, @BlogID);
SELECT @LinkID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertLinkCategoryList]
(
	@CategoryList nvarchar(4000)
	, @PostID int
	, @BlogID int
)
AS

IF @PostID = -1
	SET @PostID = NULL

--DELETE categories that have been removed
DELETE [dbo].[subtext_Links] FROM [dbo].[subtext_Links]
WHERE 
	CategoryID not in (SELECT str FROM iter_charlist_to_table(@CategoryList,','))
And 
	BlogID = @BlogID AND (PostID = @PostID)

--Add updated/new categories
INSERT INTO subtext_Links ( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogID )
SELECT NULL, NULL, NULL, 1, 0, @PostID, Convert(int, [str]), @BlogID
FROM iter_charlist_to_table(@CategoryList,',')
WHERE 
	Convert(int, [str]) not in (SELECT CategoryID FROM [dbo].[subtext_Links] WHERE PostID = @PostID AND BlogID = @BlogID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertLinkCategoryList]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertPostCategoryByName]
(
	@Title nvarchar(150)
	, @PostID int
	, @BlogID int
)
AS
DECLARE @CategoryID int
SELECT @CategoryID = CategoryID FROM [dbo].[subtext_LinkCategories] WHERE Title = @Title AND BlogID = @BlogID AND CategoryType = 1

if(@CategoryID is NULL)
BEGIN

EXEC [dbo].[subtext_InsertCategory] @Title, 1, @BlogID, 1, NULL, @CategoryID = @CategoryID output

END

DECLARE @LinkID int
EXEC [dbo].[subtext_InsertLink] NULL, NULL, NULL, 1, 0, @CategoryID, @PostID, @BlogID, @LinkID output


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertPostCategoryByName]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertReferral]
(
	@EntryID int,
	@BlogID int,
	@Url nvarchar(255)
)
AS

DECLARE @UrlID int

if(@Url is not NULL)
BEGIN
	EXEC [dbo].[subtext_GetUrlID] @Url, @UrlID = @UrlID output
END

if(@UrlID is not NULL)
BEGIN

	IF EXISTS(SELECT EntryID FROM [dbo].[subtext_Referrals] WHERE EntryID = @EntryID AND BlogID = @BlogID AND UrlID = @UrlID)
	BEGIN
		UPDATE [dbo].[subtext_Referrals]
		Set [Count] = [Count] + 1, LastUpdated = getdate()
		WHERE EntryID = @EntryID AND BlogID = @BlogID AND UrlID = @UrlID
	END
	else
	BEGIN
		Insert subtext_Referrals (EntryID, BlogID, UrlID, [Count], LastUpdated)
		       values (@EntryID, @BlogID, @UrlID, 1, getdate())
	END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertReferral]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_InsertViewStats]
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
	EXEC [dbo].[subtext_GetUrlID] @Url, @UrlID = @UrlID output
END
if(@UrlID is NULL)
set @UrlID = -1


IF EXISTS (SELECT BlogID FROM [dbo].[subtext_ViewStats] WHERE BlogID = @BlogID AND PageType = @PageType AND PostID = @PostID AND [Day] = @Day AND UrlID = @UrlID)
BEGIN
	UPDATE [dbo].[subtext_ViewStats]
	Set [Count] = [Count] + 1
	WHERE BlogID = @BlogID AND PageType = @PageType AND PostID = @PostID AND [Day] = @Day AND UrlID = @UrlID
END
Else
BEGIN
	Insert subtext_ViewStats (BlogID, PageType, PostID, [Day], UrlID, [Count])
	Values (@BlogID, @PageType, @PostID, @Day, @UrlID, 1)
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertViewStats]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_StatsSummary]
(
	@BlogID int
)
AS
DECLARE @ReferralCount int
DECLARE @WebCount int
DECLARE @AggCount int

SELECT @ReferralCount = Sum([Count]) FROM [dbo].[subtext_Referrals] WHERE BlogID = @BlogID

SELECT @WebCount = Sum(WebCount), @AggCount = Sum(AggCount) FROM [dbo].[subtext_EntryViewCount] WHERE BlogID = @BlogID

SELECT @ReferralCount AS 'ReferralCount', @WebCount AS 'WebCount', @AggCount AS 'AggCount'


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_StatsSummary]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC [dbo].[subtext_TrackEntry]
(
	@EntryID int,
	@BlogID int,
	@Url nvarchar(255),
	@IsWeb bit
)

AS

if(@Url is not NULL AND @IsWeb = 1)
BEGIN
	EXEC [dbo].[subtext_InsertReferral] @EntryID, @BlogID, @Url
END

EXEC [dbo].[subtext_InsertEntryViewCount] @EntryID, @BlogID, @IsWeb





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_TrackEntry]  TO [public]
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
CREATE PROC [dbo].[subtext_UTILITY_AddBlog]
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

IF NOT EXISTS(SELECT * FROM [dbo].[subtext_config] WHERE Host = @Host AND Application = @Application)
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
	, 'marvin2'
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

GRANT  EXECUTE  ON [dbo].[subtext_UTILITY_AddBlog]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_UpdateCategory]
(
	@CategoryID int,
	@Title nvarchar(150),
	@Active bit,
	@CategoryType tinyint,
	@Description nvarchar(1000),
	@BlogID int
)
AS
UPDATE [dbo].[subtext_LinkCategories] 
SET 
	[Title] = @Title, 
	[Active] = @Active,
	[CategoryType] = @CategoryType,
	[Description] = @Description
WHERE   
		[CategoryID] = @CategoryID 
	AND [BlogID] = @BlogID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_UpdateCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_UpdateConfig]
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
	, @BlogID int
	, @LicenseUrl nvarchar(64) = NULL
	, @DaysTillCommentsClose int = NULL
	, @CommentDelayInMinutes int = NULL
)
AS
UPDATE [dbo].[subtext_Config]
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
WHERE BlogID = @BlogID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_UpdateConfig]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_UpdateConfigUpdateTime]
(
	@BlogID int,
	@LastUpdated datetime
)
AS
UPDATE [dbo].[subtext_Config]
SET LastUpdated = @LastUpdated
WHERE blogid = @blogid


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_UpdateConfigUpdateTime]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_UpdateEntry]
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
	, @BlogID int
)
AS

if(@EntryName is not NULL)
BEGIN
	IF EXISTS(SELECT EntryName FROM [dbo].[subtext_Content] WHERE BlogID = @BlogID AND EntryName = @EntryName AND [ID] <> @ID)
	BEGIN
		RAISERROR('The EntryName you entry is already in use with in this Blog. Please pick a unique EntryName.',11,1) 
		RETURN 1
	END
END

if(Ltrim(Rtrim(@Description)) = '')
set @Description = NULL
UPDATE [dbo].[subtext_Content] 
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
	AND BlogID = @BlogID
EXEC [dbo].[subtext_UpdateConfigUpdateTime] @blogID, @DateUpdated

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_UpdateEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_UpdateImage]
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
UPDATE [dbo].[subtext_Images]
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

GRANT  EXECUTE  ON [dbo].[subtext_UpdateImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_UpdateKeyWord]
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

UPDATE [dbo].[subtext_keywords] 
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

GRANT  EXECUTE  ON [dbo].[subtext_UpdateKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[subtext_UpdateLink]
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
UPDATE [dbo].[subtext_Links] 
SET 
	Title = @Title, 
	Url = @Url, 
	Rss = @Rss, 
	Active = @Active,
	NewWindow = @NewWindow, 
	CategoryID = @CategoryID
WHERE  
		LinkID = @LinkID 
	AND BlogID = @BlogID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_UpdateLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_Utility_GetUnHashedPasswords]
AS

SELECT BlogID, Password FROM [dbo].[subtext_COnfig] WHERE Flag & 8 = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_Utility_GetUnHashedPasswords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_Utility_UpdateToHashedPassword]
(
	@Password nvarchar(100),
	@BlogID int
)

AS

UPDATE [dbo].[subtext_Config]
Set 
	Password = @Password,
	Flag = Flag | 8 
WHERE blogid = @blogid



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_Utility_UpdateToHashedPassword]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Returns a page of blogs within subtext_config table
*/
CREATE PROC [dbo].[subtext_GetPageableBlogs]
(
	@PageIndex int
	, @PageSize int
	, @SortDesc bit
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #TempPagedBlogIDs 
(
	TempID int IDENTITY (1, 1) NOT NULL,
	BlogID int NOT NULL
)	

IF NOT (@SortDesc = 1)
BEGIN
	INSERT INTO #TempPagedBlogIDs (BlogID)
	SELECT	[BlogID] 
	FROM [dbo].[subtext_config]
	ORDER BY [BlogID]
END
ELSE
BEGIN
	INSERT INTO #TempPagedBlogIDs (BlogID)
	SELECT	[BlogID] 
	FROM [dbo].[subtext_config]
	ORDER BY [BlogID] DESC
END

SELECT	blog.BlogID 
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
		
FROM [dbo].[subtext_config] blog
    	INNER JOIN #TempPagedBlogIDs tmp ON (blog.[BlogID] = tmp.BlogID)
WHERE 	tmp.TempID > @PageLowerBound 
	AND tmp.TempID < @PageUpperBound
ORDER BY tmp.TempID
 
DROP TABLE #TempPagedBlogIDs


SELECT COUNT([BlogID]) AS TotalRecords
FROM [dbo].[subtext_config]
GO


GRANT  EXECUTE  ON [dbo].[subtext_GetPageableBlogs]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Returns a single blog within the subtext_config table by id.
*/
CREATE PROC [dbo].[subtext_GetBlogById]
(
	@BlogId int
)
AS

SELECT	blog.BlogID 
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
		
FROM [dbo].[subtext_config] blog
WHERE	blog.BlogId = @BlogId
GO


GRANT  EXECUTE  ON [dbo].[subtext_GetBlogById]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Returns a single blog within the subtext_config table by id.
*/
CREATE PROC [dbo].[subtext_GetBlogsByHost]
(
	@Host nvarchar(100)
)
AS

SELECT	blog.BlogID 
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
		
FROM [dbo].[subtext_config] blog
WHERE	blog.Host = @Host

SELECT COUNT([BlogID]) AS TotalRecords
FROM [dbo].[subtext_config]
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetBlogsByHost]  TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
--//TODO: We can probably merge the following two procedures.
CREATE PROC [dbo].[subtext_InsertPingTrackEntry]
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
	, @BlogID int
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

IF NOT EXISTS(SELECT [ID] FROM [dbo].[subtext_Content] WHERE TitleUrl = @TitleUrl AND ParentID = @ParentID)
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
	, @ContentChecksumHash
	, @ParentID
	, @BlogID
)

SELECT @ID = SCOPE_IDENTITY()

UPDATE [dbo].[subtext_Content]
SET FeedBackCount = FeedBackCount + 1 
WHERE [ID] = @ParentID

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertPingTrackEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[subtext_InsertEntry]
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
	, @BlogID int
	, @DateAdded datetime
	, @ParentID int = NULL
	, @PostConfig int
	, @EntryName nvarchar(150) = NULL
	, @ContentChecksumHash varchar(32)
	, @DateSyndicated DateTime = NULL
	, @ID int output
)
AS

IF(@EntryName IS NOT NULL)
BEGIN
	IF EXISTS(SELECT EntryName FROM [dbo].[subtext_Content] WHERE BlogID = @BlogID AND EntryName = @EntryName)
	BEGIN
		RAISERROR('The EntryName you entry is already in use with in this Blog. Please pick a unique EntryName.', 11, 1) 
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
	, BlogID
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
	, @BlogID
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
	EXEC [dbo].[subtext_UpdateConfigUpdateTime] @blogID, @DateAdded
END
ELSE IF(@PostType = 3) -- Comment
BEGIN
	UPDATE [dbo].[subtext_Content]
	Set FeedBackCount = ISNULL(FeedBackCount, 0) + 1 WHERE [ID] = @ParentID
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_InsertEntry]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Retrieves a comment (or pingback) that has the specified 
ContentChecksumHash.
*/
CREATE PROC [dbo].[subtext_GetCommentByChecksumHash]
(
	@ContentChecksumHash VARCHAR(32)
	,@BlogId int
)
AS
SELECT TOP 1 BlogID
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
FROM [dbo].[subtext_Content]
WHERE 
	ContentChecksumHash = @ContentChecksumHash 
	AND BlogId = @BlogID
	AND (PostType = 3 OR PostType = 4) -- Comment or PingBack
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetCommentByChecksumHash]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Retrieves the Host Information. There should only be 
one record.
*/
CREATE PROC [dbo].[subtext_GetHost]
AS
SELECT 
	[HostUserName]
	, [Password]
	, [Salt]
	, [DateCreated]
FROM [dbo].[subtext_Host]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_GetHost]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Updates the Host information.
*/
CREATE PROC [dbo].[subtext_UpdateHost]
	@HostUserName NVARCHAR(64)
	, @Password NVARCHAR(32)
	, @Salt NVARCHAR(32)
AS
IF EXISTS(SELECT * FROM [dbo].[subtext_Host])
BEGIN
	UPDATE [dbo].[subtext_Host] 
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

GRANT  EXECUTE  ON [dbo].[subtext_UpdateHost]  TO [public]
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [dbo].[DNW_GetRecentPosts]
	@Host nvarchar(100)
	, @GroupID int

AS
SELECT Top 35 Host
	Application
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
FROM [dbo].[subtext_Content] content
INNER JOIN	[dbo].[subtext_Config] config ON content.BlogID = config.BlogID
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

GRANT  EXECUTE  ON [dbo].[DNW_GetRecentPosts]  TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[DNW_Stats]
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
FROM [dbo].[subtext_Config] 
WHERE PostCount > 0 AND subtext_Config.Flag & 2 = 2 AND Host = @Host AND BlogGroup & @GroupID = @GroupID
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


CREATE PROC [dbo].[DNW_Total_Stats]
(
	@Host nvarchar(100),
	@GroupID int
)
AS
SELECT Count(*) AS [BlogCount], Sum(PostCount) AS PostCount, Sum(CommentCount) AS CommentCount, Sum(StoryCount) AS StoryCount, Sum(PingTrackCount) AS PingTrackCount 
FROM [dbo].[subtext_Config] WHERE subtext_Config.Flag & 2 = 2 AND Host = @Host AND BlogGroup & @GroupID = @GroupID

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


CREATE PROC [dbo].[DNW_HomePageData]
(
	@Host nvarchar(100),
	@GroupID int
)
AS 
EXEC [dbo].[DNW_Stats] @Host, @GroupID
EXEC [dbo].[DNW_GetRecentPosts] @Host, @GroupID
EXEC [dbo].[DNW_Total_Stats] @Host, @GroupID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[DNW_HomePageData]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Gets the most recent version in the Version table */
CREATE PROC [dbo].[subtext_VersionGetCurrent]
AS
SELECT	TOP 1
		[Id]
		, [Major]
		, [Minor]
		, [Build]
		, [DateCreated]
FROM	[dbo].[subtext_Version]
ORDER BY [DateCreated] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_VersionGetCurrent]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Gets the most recent version in the Version table */
CREATE PROC [dbo].[subtext_VersionAdd]
(
	 @Major	INT
	, @Minor INT
	, @Build INT
	, @DateCreated DATETIME
	, @Id INT OUTPUT
	
)
AS
INSERT [dbo].[subtext_Version]
SELECT	@Major, @Minor, @Build, @DateCreated

SELECT @Id = SCOPE_IDENTITY()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_VersionAdd]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Creates a record in the subtext_log table */
CREATE PROC [dbo].[subtext_AddLogEntry]
(
	 @Date DateTime
	 , @BlogId int = NULL
	 , @Thread varchar(255)
	 , @Context varchar(512)
	 , @Level varchar(20)
	 , @Logger nvarchar(256)
	 , @Message nvarchar(2000)
	 , @Exception nvarchar(1000)
)
AS

if @BlogId < 0
	SET @BlogId = NULL

INSERT [dbo].[subtext_Log]
SELECT	@BlogId, @Date, @Thread, @Context, @Level, @Logger, @Message, @Exception

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[subtext_AddLogEntry]  TO [public]
GO
