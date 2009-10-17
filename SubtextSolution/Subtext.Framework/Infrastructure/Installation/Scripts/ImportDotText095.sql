/*
WARNING: This SCRIPT USES SQL TEMPLATE PARAMETERS.
Be sure to hit CTRL+SHIFT+M in Query Analyzer if running manually.

This script imports data from a .TEXT 0.95 database 
into the Subtext database.

This script is written with the following assumptions:
    1) it is being run from the dotText schema (so be sure to connect to it)
    2) both DBs are on the same server and have the same user/pwd.
    3) i think there was something else?

TODOs:
	1) figure out how to take advantage of 2 seperate DB connections
		possibly by using the USE <databaseName> keyword?
	2) clean up this UGLY SQL and format it for readability.
	3) I'm sure there's a lot more to be done...

DECLARE @user_name varchar(30)
SELECT @user_name = user_name()

*/

-- subtext_Config
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Config] ON

INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Config] 
( 
	BlogId
	, UserName
	, [Password]
	, Email
	, Title
	, SubTitle
	, Skin
	, [Application]
	, [Host]
	, Author
	, TimeZone
	, IsActive
	, [Language]
	, ItemCount
	, LastUpdated
	, News
	, SecondaryCss
	, PostCount
	, StoryCount
	, PingTrackCount
	, CommentCount
	, Flag
	, SkinCssFile
	, BlogGroup
	, LicenseUrl
	, DaysTillCommentsClose
	, CommentDelayInMinutes 
)
	SELECT 
		BlogId
		, UserName
		, [Password]
		, Email
		, Title
		, SubTitle
		, Skin
		, [Application]
		, [Host]
		, Author
		, TimeZone
		, IsActive
		, [Language]
		, ItemCount
		, LastUpdated
		, News
		, SecondaryCss
		, PostCount
		, StoryCount
		, PingTrackCount
		, CommentCount
		, Flag
		, SkinCssFile
		, ISNULL(BlogGroup, 1)
		, null -- LicenseUrl
		, null -- DaysTillCommentsClose
		, null -- CommentDelayInMinutes
	FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_Config]
	WHERE 1=1

SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Config] OFF
GO

UPDATE [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Config] 
SET RecentCommentsLength = 50 WHERE 1=1
GO

-- subtext_Content
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Content] ON
INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Content] 
( [ID], Title, DateAdded, PostType, Author, Email, BlogId, [Description],
	DateUpdated, [Text], FeedBackCount, PostConfig, EntryName, 
	DateSyndicated )
	SELECT 
		[ID], Title, DateAdded, PostType, Author, Email, BlogId, [Description],
		DateUpdated, [Text], ISNULL(FeedBackCount, 0), PostConfig, EntryName, DateUpdated 
	FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_Content]
	WHERE (PostType <> 3 AND PostType <> 4) -- don't insert comments or track/ping backs, they belong in the subtext_Feedback table
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Content] OFF
GO

UPDATE [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Content] 
SET DateSyndicated = DateUpdated
-- Post is syndicated and active
WHERE PostConfig & 16 = 16 AND PostConfig & 1 = 1
GO

/*	Still need to update the ContentChecksumHash column
	for all of the imported Subtext records		*/

-- subtext_Feedback
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_FeedBack] ON 
INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_FeedBack]
    ([Id],[Title],[Body],[BlogId],[EntryId],[Author], [IsBlogAuthor], [Email], [Url], [FeedbackType], [StatusFlag],
        [CommentAPI], [Referrer], [IpAddress], [UserAgent], [FeedbackChecksumHash], [DateCreated], [DateModified])
SELECT [Id] = [ID]
    , Title
	, [Body] = [Text]
	, BlogId = BlogID
	, EntryId = ParentID
	, Author = LEFT(Author, 32)
	, 0
	, Email
	, Url = TitleUrl
	, FeedbackType = CASE PostType WHEN 3 THEN 1 ELSE 2 END
	, StatusFlag = 1
	, CommentAPI = 0
	, Referrer = NULL
	, IpAddress = SUBSTRING(SourceName,0,16)
	, UserAgent = NULL
	, FeedbackChecksumHash = ''
	, DateCreated = DateAdded
	, DateModified = getdate()
FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_Content]
WHERE (PostType = 3 OR PostType = 4) -- Comment or PingBack
	
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_FeedBack] OFF
GO

UPDATE [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_FeedBack]
SET EntryId = NULL WHERE EntryId = -1
GO

-- subtext_EntryViewCount
INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_EntryViewCount] 
( EntryID, BlogId, WebCount, AggCount, WebLastUpdated, AggLastUpdated )
    SELECT 
        EntryID, BlogId, WebCount, AggCount, WebLastUpdated, AggLastUpdated
    FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_EntryViewCount]
    WHERE 1=1
GO

-- subtext_LinkCategories
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_LinkCategories]  ON
INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_LinkCategories] 
( CategoryID, Title, Active, BlogId, CategoryType, Description )
    SELECT 
		CategoryID, Title, Active, BlogId, CategoryType, Description
    FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_LinkCategories]
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_LinkCategories] OFF
GO

-- From subText 1.9.00 install script
UPDATE [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_LinkCategories] SET CategoryType = 5 WHERE CategoryType = 0
GO

-- subtext_KeyWords
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_KeyWords] ON
INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_KeyWords] 
( KeyWordID, Word, [Text], ReplaceFirstTimeOnly, OpenInNewWindow, Url, Title, BlogId, CaseSensitive )
    SELECT 
        KeyWordID, Word, [Text], ReplaceFirstTimeOnly, OpenInNewWindow, Url, Title, BlogId, CaseSensitive
    FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_KeyWords]
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_KeyWords] OFF
GO

-- subtext_Images
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Images] ON

INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Images]
( ImageID, Title, CategoryID, Width, Height, [File], Active, BlogId )
    SELECT
        ImageID, Title, CategoryID, Width, Height, [File], Active, BlogId
    FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_Images]

SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Images] OFF
GO

-- subtext_Links
/*
	Due to the Foreign Key constraints between subtext_Links.PostID --> subtext_Content.ID 
	AND subtext_Links.CategoryID --> subtext_LinkCategories.CategoryID, we have to perform 
	sub queries to specify NULL for these columns if the corresponding data does not exist.
*/
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Links] ON
INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Links]
( 
	LinkID
	, Title
	, Url
	, Rss
	, Active
	, CategoryID
	, BlogId
	, PostID
	, NewWindow 
)
SELECT
      LinkID
      , Title
      , Url
      , Rss
      , Active
      , CategoryID = 	CASE
				WHEN NOT EXISTS
				(
					SELECT slc.[CategoryID] 
					FROM [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_LinkCategories] slc 
					WHERE slc.[CategoryID] = [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_Links].CategoryID
				)
				THEN NULL
				ELSE CategoryID
			END
      , BlogId
      , PostID =	CASE
				WHEN NOT EXISTS
				(
					SELECT sct.[ID] 
					FROM [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Content] sct 
					WHERE sct.[ID] = [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_Links].PostID
				)
				THEN NULL
				ELSE PostID
			END
      , NewWindow  
FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_Links]
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Links] OFF
GO

-- subtext_URLs
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_URLs] ON
INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_URLs] 
( UrlID, Url )
    SELECT
      UrlID, Url 
    FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_URLs]
SET IDENTITY_INSERT [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_URLs] OFF
GO

-- subtext_Referrals
/*
	Due to the stranded Referral records that we've seen coming from
	dotText095 db's, we need the extra WHERE clause below. This will
	prevent any Refferral records that have a bad UrlID from breaking 
	the FK constraint to the URLs table.
*/
INSERT INTO [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Referrals] 
( EntryID, BlogId, UrlID, [Count], LastUpdated )
    SELECT
        EntryID, BlogId, UrlID, [Count], LastUpdated
    FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_Referrals] 
        WHERE UrlID IN (SELECT UrlID FROM [<dottext_db_name,varchar,DotTextData>].[<dotTextDbUser,varchar,dbo>].[blog_URLs])
        AND EntryId IN (SELECT [ID] FROM [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Content])
GO

-- Now we need to do a little "cleanup" to remove any references to comments/trackback from 
-- the subtext_Referrals table.
DELETE FROM [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Referrals]  
	WHERE EXISTS (SELECT * FROM [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Content] sC 
		WHERE (sc.PostType = 3 OR sC.PostType = 4) AND [<subtext_db_name,varchar,SubtextData>].[<dbUser,varchar,dbo>].[subtext_Referrals].EntryID = sc.[ID])
GO



--  DONE
