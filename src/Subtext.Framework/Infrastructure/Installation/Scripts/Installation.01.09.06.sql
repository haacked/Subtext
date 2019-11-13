/* 
    WARNING: This SCRIPT USES SQL TEMPLATE PARAMETERS.
	Be sure to hit CTRL+SHIFT+M in Query Analyzer if running manually.
*/


/* Drop an unused column */
IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'IsAggregated'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
	DROP CONSTRAINT [DF__subtext_Conf__IsAgg__61316BF4]
	
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		DROP COLUMN [IsAggregated]
END

/* add some missing Foreign Keys */

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK_subtext_EntryTag_subtext_Config'
	AND CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryTag] WITH NOCHECK
	ADD CONSTRAINT [FK_subtext_EntryTag_subtext_Config] FOREIGN KEY
	( [BlogId] ) REFERENCES <dbUser,varchar,dbo>.[subtext_Config]
	( [BlogId] )
END
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK_subtext_Tag_subtext_Config'
	AND CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Tag] WITH NOCHECK
	ADD CONSTRAINT [FK_subtext_Tag_subtext_Config] FOREIGN KEY
	( [BlogId] ) REFERENCES <dbUser,varchar,dbo>.[subtext_Config]
	( [BlogId] )
END
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK_subtext_EntryViewCount_subtext_Content'
	AND CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryViewCount] WITH NOCHECK
	ADD CONSTRAINT [FK_subtext_EntryViewCount_subtext_Content] FOREIGN KEY
	( [EntryId] ) REFERENCES <dbUser,varchar,dbo>.[subtext_Content]
	( [ID] )
END
GO 

/* This column is no longer needed and was a temporary column used during 1.9.6 development, but is in use in some production systems */
IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'CustomMetaTags'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		DROP COLUMN [CustomMetaTags]
END
GO


IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'TrackingCode'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [TrackingCode] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL
END
GO

/* Change the subtext_Content.EntryName column to have nVarChar data type */

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS]
			WHERE TABLE_NAME = 'subtext_Content' 
				AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
				AND COLUMN_NAME = 'EntryName' 
				AND DATA_TYPE = 'varchar')
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] ALTER COLUMN EntryName NVARCHAR(100)
GO


/* Create the new MetaTag table */

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS]
	WHERE TABLE_NAME = 'subtext_MetaTag'
	AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	CREATE TABLE [<dbUser,varchar,dbo>].[subtext_MetaTag]
	(
		[Id] int IDENTITY(1,1) NOT NULL,
		[Content] nvarchar(512) NOT NULL,
		[Name] nvarchar(100) NULL,
		[HttpEquiv] nvarchar(100) NULL,
		[BlogId] int NOT NULL,
		[EntryId] int NULL,
		[DateCreated] datetime NULL CONSTRAINT [DF_subtext_MetaTag_DateCreated] DEFAULT (getdate())
		,CONSTRAINT [PK_subtext_MetaTag] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		) ON [PRIMARY]
		,CONSTRAINT [FK_subtext_MetaTag_subtext_Config] FOREIGN KEY
		( [BlogId] ) REFERENCES <dbUser,varchar,dbo>.[subtext_Config]
			( [BlogId] )
		,CONSTRAINT [FK_subtext_MetaTag_subtext_Content] FOREIGN KEY
			( [EntryId] ) REFERENCES <dbUser,varchar,dbo>.[subtext_Content]
			( [ID] )
	)
END
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS]
	WHERE TABLE_NAME = 'subtext_DomainAlias'
	AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	CREATE TABLE [<dbUser,varchar,dbo>].[subtext_DomainAlias]
	(
		[Id] int IDENTITY(0,1) NOT NULL,
		[BlogId] int NOT NULL ,
		[Host] nvarchar(100) NOT NULL ,
		[Application] nvarchar(50) NOT NULL, 
		[IsActive] [bit] NULL 
		,CONSTRAINT [PK_subtext_DomainAlias] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		) ON [PRIMARY]
		,CONSTRAINT [FK_subtext_DomainAlias_subtext_Config] FOREIGN KEY
		( [BlogId] ) REFERENCES <dbUser,varchar,dbo>.[subtext_Config]
		( [BlogId] )
	)
END
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS]
	WHERE TABLE_NAME = 'subtext_BlogGroup'
	AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	CREATE TABLE [<dbUser,varchar,dbo>].[subtext_BlogGroup] 
	(
		[Id] [int] IDENTITY (1, 1) NOT NULL,
		[Title] [nvarchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
		[Active] [bit] NOT NULL ,
		[DisplayOrder] [int] NULL ,
		[Description] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
		,CONSTRAINT [PK_subtext_BlogGroup] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		) ON [PRIMARY]
	)
	
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_BlogGroup] ADD 
	CONSTRAINT [DF_subtext_BlogGroup_Active] DEFAULT (1) FOR [Active]
END
GO

IF NOT EXISTS
(
	SELECT * FROM [<dbUser,varchar,dbo>].[subtext_BlogGroup] WHERE 
	Title = 'Default Group'
)
BEGIN
	INSERT INTO [<dbUser,varchar,dbo>].[subtext_BlogGroup]
	VALUES ('Default Group', 1, NULL, NULL)
END
GO

-- Rename the BlogGroup column in subtext_Config...
IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'BlogGroup'
)
BEGIN
	EXEC sp_rename '<dbUser,varchar,dbo>.subtext_config.BlogGroup', 'BlogGroupId'
END

GO
IF NOT  EXISTS
(
	SELECT * FROM [<dbUser,varchar,dbo>].sysobjects
	WHERE id = OBJECT_ID(N'[<dbUser,varchar,dbo>].[FK_subtext_Config_subtext_BlogGroup]')
	AND type = 'F'
)
BEGIN
ALTER TABLE [<dbUser,varchar,dbo>].subtext_Config WITH NOCHECK ADD CONSTRAINT
	FK_subtext_Config_subtext_BlogGroup FOREIGN KEY
	(
	BlogGroupId
	) REFERENCES [<dbUser,varchar,dbo>].subtext_BlogGroup
	(
	Id
	)
END
GO

/* Add the MobileSkin and MobilSkinCssFile columns */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'MobileSkin'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [MobileSkin] [nvarchar] (50) NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'MobileSkinCssFile'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [MobileSkinCssFile] [nvarchar] (100) NULL
END