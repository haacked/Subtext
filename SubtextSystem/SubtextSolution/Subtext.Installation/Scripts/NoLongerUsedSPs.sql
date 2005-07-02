SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [blog_GetConditionalEntriesByDateUpdated]
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
	, blog_Content.EntryName 
	, blog_Content.ContentChecksumHash
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

GRANT  EXECUTE  ON [blog_GetConditionalEntriesByDateUpdated]  TO [public]
GO
