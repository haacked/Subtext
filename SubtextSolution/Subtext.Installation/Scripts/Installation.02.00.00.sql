/*
SCRIPT: Installation.02.00.00

NOTE:	This script is the last of three scripts used to upgrade 
		Subtext from 1.9 to 2.0.

ACTION: This script removes columns that are no longer needed.

*/
/* Remove unneeded columns from subtext_Config */
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'UserName')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
	DROP COLUMN UserName
END
GO

/* Remove the subtext_Config HostApplication constraint. Not sure why this was added. 
	Could not use the INFORMATION_SCHEMA because indexes aren't put in there. :(
*/
IF  EXISTS (SELECT * FROM dbo.sysindexes WHERE id = OBJECT_ID(N'[<dbUser,varchar,dbo>].[subtext_Config]') AND name = N'IX_subtext_Config_HostApplication')
DROP INDEX [<dbUser,varchar,dbo>].[subtext_Config].[IX_subtext_Config_HostApplication]
GO


/* Remove the subtext_Config constraint */
IF EXISTS(
	SELECT * 
	FROM [INFORMATION_SCHEMA].[CONSTRAINT_COLUMN_USAGE]
	WHERE CONSTRAINT_NAME = 'IX_subtext_Config'
	AND CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
BEGIN
	PRINT 'remove constraint on Host + Application'
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] 
	DROP CONSTRAINT IX_subtext_Config
END
GO

/* Remove the Application Folder. Earlier, we created a 
	Subfolder column to hold this info.
*/
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Application')
AND EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Subfolder')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
	DROP COLUMN Application
END
GO

/* Add unique constraint on Host + Subfolder */
IF NOT EXISTS(
	SELECT * 
	FROM [INFORMATION_SCHEMA].[CONSTRAINT_COLUMN_USAGE]
	WHERE CONSTRAINT_NAME = 'IX_subtext_Config__Host_Subfolder'
	AND CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
BEGIN
	PRINT 'add constraint on Host + Subfolder'
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] ADD  CONSTRAINT [IX_subtext_Config] UNIQUE NONCLUSTERED 
	(
		[Subfolder] ASC,
		[Host] ASC
	) ON [PRIMARY]
END
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Password')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
	DROP COLUMN Password
END
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Email')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
	DROP COLUMN Email
END
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config'
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Author')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
	DROP COLUMN Author
END
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Content' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Author')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content]
	DROP COLUMN Author
END
GO

IF EXISTS (SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Content' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Email')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content]
	DROP COLUMN Email
END
GO


/* Drop unnecessary columns from subtext_Host */
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Host' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'HostUserName')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Host]
	DROP COLUMN HostUserName
END
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Host' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Password')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Host]
	DROP COLUMN Password
END
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Host' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'Salt')
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Host]
	DROP COLUMN Salt
END
GO

/*Add the Mail to weblog parameters to subtext_config - GY*/
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'pop3User' 
	)
	
	--Probably should add these one at a time.
	
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] WITH NOCHECK
	ADD [pop3User] [varchar](32) NULL
		,[pop3Pass] [varchar] (32) NULL
		,[pop3Server] [varchar] (56) NULL
		,[pop3StartTag] [varchar] (10) NULL
		,[pop3EndTag] [varchar] (10) NULL
		,[pop3SubjectPrefix] [nvarchar] (20) NULL
		,[pop3MTBEnable] [bit] NULL
		,[pop3DeleteOnlyProcessed] [bit] NULL
		,[pop3InlineAttachedPictures] [bit] NULL
		,[pop3HeightForThumbs] [int] NULL 
GO

/*Add CustomMetaTags to subtext_Config */
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'CustomMetaTags' 
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] WITH NOCHECK
	ADD [CustomMetaTags] [nText] NULL
GO

/*Add TrackingCode to subtext_Config */
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	TABLE_NAME = 'subtext_Config' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
		AND		COLUMN_NAME = 'TrackingCode' 
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] WITH NOCHECK
	ADD [TrackingCode] [nText] NULL
GO

/* Add tables to manage plugin configuration */




/*  
	Create subtext_PluginBlog Table and related FK
*/

IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[TABLES] 
		WHERE	TABLE_NAME = 'subtext_PluginBlog' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
	)
	BEGIN
		CREATE TABLE [<dbUser,varchar,dbo>].[subtext_PluginBlog] (
			[Id] [int] IDENTITY (1, 1) NOT NULL ,
			[BlogId] [int] NOT NULL ,
			[PluginId] [uniqueidentifier] NOT NULL ,
			[Enabled] [Bit] NOT NULL
		CONSTRAINT [PK_subtext_PluginBlog] PRIMARY KEY  CLUSTERED 
		(
			[Id]
		) ON [PRIMARY] 
		) ON [PRIMARY]

		CREATE  UNIQUE  INDEX [IX_subtext_PluginBlog] ON [<dbUser,varchar,dbo>].[subtext_PluginBlog]([BlogId], [PluginId]) ON [PRIMARY]

	END
GO


IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
		WHERE	CONSTRAINT_NAME = 'FK_subtext_PluginBlog_subtext_Config' 
		AND		UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
	BEGIN
		ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PluginBlog] WITH NOCHECK ADD 
			CONSTRAINT [DF_subtext_PluginBlog_Enabled] DEFAULT (0) FOR [Enabled],
			CONSTRAINT [FK_subtext_PluginBlog_subtext_Config] FOREIGN KEY 
			(
				[BlogId]
			) REFERENCES [<dbUser,varchar,dbo>].[subtext_Config] (
				[BlogId]
			)
	END
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PluginBlog] CHECK CONSTRAINT [FK_subtext_PluginBlog_subtext_Config]
GO

/*  
	Create subtext_PluginEntryData Table and related FK
*/

IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[TABLES] 
		WHERE	TABLE_NAME = 'subtext_PluginData' 
		AND		TABLE_SCHEMA = '<dbUser,varchar,dbo>'
	)
	BEGIN
		CREATE TABLE [<dbUser,varchar,dbo>].[subtext_PluginData] (
			[Id] [int] IDENTITY (1, 1) NOT NULL ,
			[BlogPluginId] [int] NOT NULL ,
			[EntryId] [int] NULL ,
			[Key] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
			[Value] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
		CONSTRAINT [PK_subtext_PluginEntryData] PRIMARY KEY  CLUSTERED 
		(
			[Id]
		) ON [PRIMARY] 
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
		
		 CREATE  UNIQUE  INDEX [IX_subtext_PluginEntryData] ON [<dbUser,varchar,dbo>].[subtext_PluginData]([BlogPluginId], [EntryId], [Key]) ON [PRIMARY]

		
		EXEC sp_tableoption  N'[<dbUser,varchar,dbo>].[subtext_PluginData]', 'text in row', '2048'
	END
GO


IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
		WHERE	CONSTRAINT_NAME = 'FK_subtext_PluginData_subtext_PluginBlog' 
		AND		UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
	BEGIN
		ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PluginData] WITH NOCHECK ADD   
			CONSTRAINT [FK_subtext_PluginData_subtext_PluginBlog] FOREIGN KEY 
			(
				[BlogPluginId]
			) REFERENCES [<dbUser,varchar,dbo>].[subtext_PluginBlog] (
				[Id]
			)
	END
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PluginData] CHECK CONSTRAINT [FK_subtext_PluginData_subtext_PluginBlog]
GO


IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
		WHERE	CONSTRAINT_NAME = 'FK_subtext_PluginData_subtext_Content' 
		AND		UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
	BEGIN
		ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PluginData] WITH NOCHECK ADD   
			CONSTRAINT [FK_subtext_PluginData_subtext_Content] FOREIGN KEY 
			(
				[EntryId]
			) REFERENCES [dbo].[subtext_Content] (
				[ID]
			)
	END
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PluginData] CHECK CONSTRAINT [FK_subtext_PluginData_subtext_Content]
GO


/* Drop the old and unused spamPostCount table */
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE TABLE_NAME = 'spamPostCount' 
		AND TABLE_SCHEMA = '<dbUser,varchar,dbo>')
	
	DROP TABLE [<dbUser,varchar,dbo>].[spamPostCount]	
GO
