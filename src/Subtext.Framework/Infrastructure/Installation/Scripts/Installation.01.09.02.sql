/*  Looks like we made a mistake in Subtext 1.09.01 with links 
	This query fixes it.
*/

UPDATE [<dbUser,varchar,dbo>].[subtext_LinkCategories] SET CategoryType = 5 WHERE CategoryType = 0


IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'AkismetAPIKey'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [AkismetAPIKey] VARCHAR(16) NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'CategoryListPostCount'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [CategoryListPostCount] [int] NULL 
		CONSTRAINT defaultCategoryListPostCount DEFAULT 10
END
GO

IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[TABLES] 
		WHERE	TABLE_NAME = 'subtext_Feedback' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
	)
	BEGIN
	/* Create COMMENT table
	
		Url			- the URL the user enters for comments, or the URL of the trackback/pingback.
		CommentType - 1 for Comment, 2 Pingback/Trackback, 3 Contact Message via contact page (if routing is turned on).
		StatusFlag	-	0 for uncategorized, 
						1 for active (whether specifically approved or not)
						2 for needs moderation, 
							3 (ie 2 + 1) for moderator approved, 
						4 for flagged as spam
							5 for flagged as spam, but approved. False positive.
						8 for confirmed spam
		Referrer		- HTTP_REFERER
	*/
		CREATE TABLE [<dbUser,varchar,dbo>].[subtext_FeedBack]
		(
			[Id] [int] IDENTITY(1,1) NOT NULL,
			[Title] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
			[Body] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
			[BlogId] [int] NOT NULL,
			[EntryId] [int] NULL,
			[Author] [nvarchar](64) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
			[IsBlogAuthor] [bit],
			[Email] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
			[Url] [varchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
			[FeedbackType] [int] NOT NULL,
			[StatusFlag] [int] NOT NULL,
			[CommentAPI] [bit] NOT NULL CONSTRAINT [DF_subtext_FeedBack_CommentAPI]  DEFAULT (0),
			[Referrer] [varchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
			[IpAddress] [varchar](16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
			[UserAgent] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
			[FeedbackChecksumHash] [varchar](32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
			[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_subtext_Feedback_DateCreated]  DEFAULT (getdate()),
			[DateModified] [datetime] NOT NULL CONSTRAINT [DF_subtext_Feedback_DateModified]  DEFAULT (getdate()),
		 CONSTRAINT [PK_subtext_Feedback] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	END
GO
-- ADDS The Foreign key to subtext_Content
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
		WHERE	CONSTRAINT_NAME = 'FK_subtext_Feedback_subtext_Content' 
		AND		UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
	BEGIN
		ALTER TABLE [<dbUser,varchar,dbo>].[subtext_FeedBack]  WITH NOCHECK ADD  CONSTRAINT [FK_subtext_Feedback_subtext_Content] FOREIGN KEY([EntryId])
		REFERENCES [<dbUser,varchar,dbo>].[subtext_Content] ([ID])
	END
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_FeedBack] CHECK CONSTRAINT [FK_subtext_Feedback_subtext_Content]
GO

-- ADDS The Foreign key to subtext_Config
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
		WHERE	CONSTRAINT_NAME = 'FK_subtext_Feedback_subtext_Config' 
		AND		UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
	BEGIN
		ALTER TABLE [<dbUser,varchar,dbo>].[subtext_FeedBack]  WITH NOCHECK ADD  CONSTRAINT [FK_subtext_Feedback_subtext_Config] FOREIGN KEY([BlogId])
		REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] ([BlogID])
	END
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_FeedBack] CHECK CONSTRAINT [FK_subtext_Feedback_subtext_Config]
GO

-- Now we need to do a little "cleanup" to remove any references to comments/trackback from 
-- the subtext_Referrals table.
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Referrals]  
	WHERE EXISTS (SELECT * FROM [<dbUser,varchar,dbo>].[subtext_Content] sC 
		WHERE (sc.PostType = 3 OR sC.PostType = 4) AND [<dbUser,varchar,dbo>].[subtext_Referrals].EntryID = sc.ID)

IF EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Content' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'ParentId'
	)
	BEGIN
	/* clean up parent id */
	UPDATE [<dbUser,varchar,dbo>].[subtext_Content] SET ParentID = NULL 
	WHERE (PostType = 3 OR PostType = 4)
		AND NOT ParentID IS NULL 
		AND ParentID NOT IN 
		(
			SELECT ID FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE PostType != 3 AND PostType != 4
		)
	
	/* Transfer comments over to subtext_Feedback */
		INSERT [<dbUser,varchar,dbo>].[subtext_FeedBack]
		SELECT Title
			, Body = [Text]
			, BlogId
			, EntryId = ParentID
			, Author = LEFT(Author, 32)
			, 0
			, Email
			, Url = TitleUrl
			, FeedbackType = CASE PostType WHEN 3 THEN 1 ELSE 2 END
			, StatusFlag = 1
			, CommentAPI = 0
			, Referrer = NULL
			, IpAddress = LEFT(SourceName, 15)
			, UserAgent = NULL
			, FeedbackChecksumHash = ISNULL(ContentChecksumHash, '')
			, DateCreated = DateAdded
			, DateModified = getdate()
		FROM [<dbUser,varchar,dbo>].[subtext_Content]
		WHERE 
			(PostType = 3 OR PostType = 4) -- Comment or PingBack

	/* Delete comments from old table */
		DELETE [<dbUser,varchar,dbo>].[subtext_Content]
		WHERE (PostType = 3 OR PostType = 4) -- Comment or PingBack
	
	/* Drop the ParentID column */
		ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
		DROP COLUMN [ParentId]
	END
GO

/* Clean up the subtext_Content table by removing columns no longer needed! */
IF EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Content' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'ContentChecksumHash'
	)
	/* Add an URL column so we can see which URL caused the problem */
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
	DROP COLUMN [ContentChecksumHash]
GO

/* Drop the TitleUrl column */
IF EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Content' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'TitleUrl'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
	DROP COLUMN [TitleUrl]
GO


/* DROP the SourceUrl column */
IF EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Content' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'SourceUrl'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
	DROP COLUMN [SourceUrl]
GO

/* DROP the SourceName column */
IF EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Content' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'SourceName'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
	DROP COLUMN [SourceName]
GO

/* Add REL column to the Keyword table */
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Keywords' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Rel'
	)
	
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords] WITH NOCHECK
	ADD [Rel] [nvarchar](64) NULL

GO

/* ADD the FeedBurnerName column to the subtext_Config table */
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'FeedBurnerName'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] WITH NOCHECK
	ADD [FeedBurnerName] [nvarchar](64) NULL
GO

/* UPDATE old timezones */
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -1069290744 WHERE TimeZone = -12
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 733176969 WHERE TimeZone = -11
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 1106595067 WHERE TimeZone = -10
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 997436142 WHERE TimeZone = -9
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -2037797565 WHERE TimeZone = -8
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 2123325864 WHERE TimeZone = -7 -- Mountain Time
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -590151426 WHERE TimeZone = -6 -- Central Time
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -1188006249 WHERE TimeZone = -5 -- Eastern Time
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 558059746 WHERE TimeZone = -4 -- Atlantic time.
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -1602501789 WHERE TimeZone = -3.5
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 352495817 WHERE TimeZone = -3 -- Brasilia
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -1639373905 WHERE TimeZone = -2
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 1455188603 WHERE TimeZone = -1 -- Azores
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 276583904 WHERE TimeZone = 0 -- GMT
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 1638717133 WHERE TimeZone = 1 -- Brussels, Copenhagen, Madrid, Paris
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 489559338 WHERE TimeZone = 2 -- Jerusalem
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -402707908 WHERE TimeZone = 3 -- Moscow
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 563311159 WHERE TimeZone = 3.5
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -2085021355 WHERE TimeZone = 4 -- Abu Dhabi
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 195872777 WHERE TimeZone = 4.5
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -1229352363 WHERE TimeZone = 5 -- Islamabad, Karachi, Tashkent
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 658848125 WHERE TimeZone = 5.5
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -1249671084 WHERE TimeZone = 5.75
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -175654566 WHERE TimeZone = 6 -- Almaty, Novosibirsk
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 1081970031 WHERE TimeZone = 6.5
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -640089798 WHERE TimeZone = 7 -- Bangkok, Hanoi, Jakarta
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 1612399360 WHERE TimeZone = 8 -- Taipei
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -1556155466 WHERE TimeZone = 9 -- Seoul
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -159819906 WHERE TimeZone = 9.5 -- Adelaide
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -1972898444 WHERE TimeZone = 10 -- Guam, Port Moresby
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = 619026968 WHERE TimeZone = 11
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZone = -695818228 WHERE TimeZone = 12 -- Auckland, Wellington
GO

