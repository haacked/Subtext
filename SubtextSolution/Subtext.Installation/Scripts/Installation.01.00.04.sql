/* Shrink the Message Column to make room for the Exception Column */
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] 
ALTER COLUMN [Message] NVARCHAR(255)
GO

/* Expand the Exception Column to fit more details */
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] 
ALTER COLUMN [Exception] NVARCHAR(2745)
GO

/* Add the Url column to the subtext_Log table if it doesn't exist */
IF NOT EXISTS 
	(
		SELECT	* FROM [information_schema].[columns] 
		WHERE	table_name = 'subtext_Log' 
		AND		table_schema = '<dbUser,varchar,dbo>'
		AND		column_name = 'Url'
	)
	/* Add an URL column so we can see which URL caused the problem */
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] 
	ADD [Url] VARCHAR(255) NULL
GO

UPDATE [<dbUser,varchar,dbo>].[subtext_Content] 
SET [FeedBackCount] = 0
WHERE [FeedBackCount] IS NULL
GO