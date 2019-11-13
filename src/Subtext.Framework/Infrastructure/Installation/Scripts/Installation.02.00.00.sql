/* 
	Placeholder file for the changes to happen in version 2.0
	SQL changes that were originally for 1.9.6 are still in their own file
*/


/*----------- {subtext_Enclosure} ------------*/
IF NOT EXISTS 
(
    SELECT      * FROM [INFORMATION_SCHEMA].[TABLES] 
    WHERE TABLE_NAME = 'subtext_Enclosure' 
    AND         TABLE_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Enclosure]
	(
		[Id] int IDENTITY(1,1) NOT NULL,
		[EntryId] int NOT NULL,
		[Title] nvarchar(256) NULL,
		[Url] nvarchar(256) NOT NULL,
		[Size] bigint NOT NULL,
		[MimeType] nvarchar(256) NOT NULL,
		[EnclosureEnabled] bit NOT NULL DEFAULT 1,
		[AddToFeed] bit NOT NULL,
		[ShowWithPost] bit NOT NULL,
		CONSTRAINT [PK_subtext_Enclosure] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)
		ON [PRIMARY]
	)
END
GO

/*----------- {Login / Identity columns} ------------*/
/* Add the OpenID Url column */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'OpenIDUrl'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [OpenIDUrl] [nvarchar] (255) NULL
END
GO

/* Add the OpenID Server column */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'OpenIDServer'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [OpenIDServer] [nvarchar] (255) NULL
END
GO

/* Add the OpenID Delegate column */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'OpenIDDelegate'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [OpenIDDelegate] [nvarchar] (255) NULL
END
GO

/* Add the CardSpace Hash column */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'CardSpaceHash'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD CardSpaceHash [nvarchar] (512) NULL
END
GO

/*----------- {Links/ XFN Relations} ------------*/
IF NOT EXISTS
(
SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Links' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'Rel'
)
BEGIN
	ALTER TABLE[<dbUser,varchar,dbo>].[subtext_Links]
	ADD Rel [nvarchar](100) NULL
END
GO



UPDATE    [<dbUser,varchar,dbo>].[subtext_Content]
SET     [DateSyndicated] = [DateAdded]
GO