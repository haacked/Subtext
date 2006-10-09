/*  Looks like we made a mistake in Subtext 1.09.01 with links 
	This query fixes it.
*/
UPDATE [<dbUser,varchar,dbo>].[subtext_LinkCategories] SET CategoryType = 5 WHERE CategoryType = 0


IF NOT EXISTS 
(
    SELECT * FROM [information_schema].[columns] 
    WHERE   table_name = 'subtext_Config' 
    AND table_schema = 'dbo'
    AND column_name = 'AkismetAPIKey'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [AkismetAPIKey] VARCHAR(16) NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [information_schema].[columns] 
    WHERE   table_name = 'subtext_Config' 
    AND table_schema = 'dbo'
    AND column_name = 'CategoryListPostCount'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [CategoryListPostCount] [int] NULL 
		CONSTRAINT defaultCategoryListPostCount DEFAULT 10
END
GO

IF NOT EXISTS 
	(
		SELECT	* FROM [information_schema].[tables] 
		WHERE	table_name = 'subtext_Feedback' 
		AND		table_schema = '<dbUser,varchar,dbo>'
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
			[Author] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
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
		SELECT	* FROM [information_schema].[referential_constraints] 
		WHERE	constraint_name = 'FK_subtext_Feedback_subtext_Content' 
		AND		unique_constraint_schema = '<dbUser,varchar,dbo>'
	)
	BEGIN
		ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback]  WITH NOCHECK ADD  CONSTRAINT [FK_subtext_Feedback_subtext_Content] FOREIGN KEY([EntryId])
		REFERENCES [<dbUser,varchar,dbo>].[subtext_Content] ([ID])
	END
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback] CHECK CONSTRAINT [FK_subtext_Feedback_subtext_Content]
GO

-- ADDS The Foreign key to subtext_Config
IF NOT EXISTS 
	(
		SELECT	* FROM [information_schema].[referential_constraints] 
		WHERE	constraint_name = 'FK_subtext_Feedback_subtext_Config' 
		AND		unique_constraint_schema = '<dbUser,varchar,dbo>'
	)
	BEGIN
		ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback]  WITH NOCHECK ADD  CONSTRAINT [FK_subtext_Feedback_subtext_Config] FOREIGN KEY([BlogId])
		REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] ([BlogID])
	END
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback] CHECK CONSTRAINT [FK_subtext_Feedback_subtext_Config]
GO

-- Now we need to do a little "cleanup" to remove any references to comments/trackback from 
-- the subtext_Referrals table.
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Referrals]  
	WHERE EXISTS (SELECT * FROM [<dbUser,varchar,dbo>].[subtext_Content] sC 
		WHERE (sc.PostType = 3 OR sC.PostType = 4) AND [<dbUser,varchar,dbo>].[subtext_Referrals].EntryID = sc.ID)


IF EXISTS 
	(
		SELECT	* FROM [information_schema].[columns] 
		WHERE	table_name = 'subtext_Content' 
		AND		table_schema = '<dbUser,varchar,dbo>'
		AND		column_name = 'ParentId'
	)
	BEGIN
	/* Transfer comments over to subtext_Feedback */
		INSERT [<dbUser,varchar,dbo>].[subtext_Feedback]
		SELECT Title
			, Body = [Text]
			, BlogId
			, EntryId = ParentID
			, Author
			, Email
			, Url = TitleUrl
			, FeedbackType = CASE PostConfig WHEN 3 THEN 1 ELSE 2 END
			, StatusFlag = 1
			, CommentAPI = 0
			, Referrer = NULL
			, IpAddress = SourceName
			, UserAgent = NULL
			, FeedbackChecksumHash = ISNULL(ContentChecksumHash, '')
			, DateCreated = DateAdded
			, DateModified = getdate()
		FROM [dbo].[subtext_Content]
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
		SELECT	* FROM [information_schema].[columns] 
		WHERE	table_name = 'subtext_Content' 
		AND		table_schema = '<dbUser,varchar,dbo>'
		AND		column_name = 'ContentChecksumHash'
	)
	/* Add an URL column so we can see which URL caused the problem */
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
	DROP COLUMN [ContentChecksumHash]
GO

/* Drop the TitleUrl column */
IF EXISTS 
	(
		SELECT	* FROM [information_schema].[columns] 
		WHERE	table_name = 'subtext_Content' 
		AND		table_schema = '<dbUser,varchar,dbo>'
		AND		column_name = 'TitleUrl'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
	DROP COLUMN [TitleUrl]
GO


/* DROP the SourceUrl column */
IF EXISTS 
	(
		SELECT	* FROM [information_schema].[columns] 
		WHERE	table_name = 'subtext_Content' 
		AND		table_schema = '<dbUser,varchar,dbo>'
		AND		column_name = 'SourceUrl'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
	DROP COLUMN [SourceUrl]
GO

/* DROP the SourceName column */
IF EXISTS 
	(
		SELECT	* FROM [information_schema].[columns] 
		WHERE	table_name = 'subtext_Content' 
		AND		table_schema = '<dbUser,varchar,dbo>'
		AND		column_name = 'SourceName'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] 
	DROP COLUMN [SourceName]
GO

/* Add REL column to the Keyword table */
IF NOT EXISTS 
	(
		SELECT	* FROM [information_schema].[columns] 
		WHERE	table_name = 'subtext_Keywords' 
		AND		table_schema = '<dbUser,varchar,dbo>'
		AND		column_name = 'Rel'
	)
	
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords] WITH NOCHECK
	ADD [Rel] [nvarchar](64) NULL

GO