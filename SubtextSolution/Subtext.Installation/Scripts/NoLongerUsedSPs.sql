SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [dbo].[blog_GetConditionalEntriesByDateUpdated]
(
	@DateUpdated datetime
	, @ItemCount int
	, @PostType int
	, @PostConfig int
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
FROM [dbo].[blog_Content]
WHERE	PostType=@PostType 
	AND BlogID = @BlogID
	AND PostConfig & @PostConfig = @PostConfig 
	AND DateUpdated > @DateUpdated
ORDER BY [ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[blog_GetConditionalEntriesByDateUpdated]  TO [public]
GO
