/* Need to have this file so that we'll kick off the Auto-Upgrade logic 
    because we need to update the Stored Procedures to fix some bugs
    
    - Fixing the blog creation proc.
*/

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_KeyWords' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'Rel'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords]
		ADD [Rel] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
END
GO

/*----------- {subtext_Tag} ------------*/
IF NOT EXISTS 
(
    SELECT      * FROM [INFORMATION_SCHEMA].[TABLES] 
    WHERE TABLE_NAME = 'subtext_Tag' 
    AND         TABLE_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Tag]
	(
		[Id] int IDENTITY(1,1) NOT NULL,
		[BlogId] int NOT NULL,
		[Name] nvarchar(256) NOT NULL,
		[DateCreated] datetime NULL CONSTRAINT [DF_subtext_Tag_DateCreated]  DEFAULT (getdate()),
		CONSTRAINT [PK_subtext_Tag] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)
		ON [PRIMARY]
	)
END
GO

/*
We cannot use the information_schema views because 
indexes are not represented by those views.
*/
CREATE TABLE #Indexes
(
	index_name nvarchar(100)
	, index_description nvarchar(256)
	, index_key nvarchar(256)
)
INSERT INTO #Indexes
	EXEC sp_helpindex '<dbUser,varchar,dbo>.subtext_Tag'

IF NOT EXISTS(SELECT * FROM #Indexes WHERE index_name = 'IX_subtext_Tag_Name')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [IX_subtext_Tag_Name] ON [<dbUser,varchar,dbo>].[subtext_Tag] 
	(
		[Name] ASC,
		[BlogId] ASC
	) ON [PRIMARY]
END
DROP TABLE #Indexes
GO

/*----------- {subtext_EntryTag} ------------*/
IF NOT EXISTS 
(
    SELECT      * FROM [INFORMATION_SCHEMA].[TABLES] 
    WHERE TABLE_NAME = 'subtext_EntryTag' 
    AND         TABLE_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
	CREATE TABLE [<dbUser,varchar,dbo>].[subtext_EntryTag](
		[Id] int IDENTITY(1,1) NOT NULL,
		[BlogId] int NOT NULL,
		[EntryId] int NOT NULL,
		[TagId] int NOT NULL,
		[DateCreated] datetime NULL CONSTRAINT [DF_subtext_EntryTag_DateCreated]  DEFAULT (getdate()),
		CONSTRAINT [PK_subtext_EntryTag] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		) ON [PRIMARY]
	)
END
GO

/*
We cannot use the information_schema views because 
indexes are not represented by those views.
*/
CREATE TABLE #Indexes2
(
	index_name nvarchar(100)
	, index_description nvarchar(256)
	, index_key nvarchar(256)
)
INSERT INTO #Indexes2
	EXEC sp_helpindex '<dbUser,varchar,dbo>.subtext_EntryTag'

IF NOT EXISTS(SELECT * FROM #Indexes2 WHERE index_name = 'IX_subtext_EntryTag')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [IX_subtext_EntryTag] ON [<dbUser,varchar,dbo>].[subtext_EntryTag] 
	(
		[EntryId] ASC,
		[TagId] ASC,
		[BlogId] ASC
	) ON [PRIMARY]
END
DROP TABLE #Indexes2
GO

IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
		WHERE	CONSTRAINT_NAME = 'FK_subtext_EntryTag_subtext_Content' 
		AND		UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryTag]  
	WITH CHECK ADD CONSTRAINT [FK_subtext_EntryTag_subtext_Content] FOREIGN KEY([EntryId])
	REFERENCES [<dbUser,varchar,dbo>].[subtext_Content] ([ID])
END
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryTag] CHECK CONSTRAINT [FK_subtext_EntryTag_subtext_Content]
GO

IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
		WHERE	CONSTRAINT_NAME = 'FK_subtext_EntryTag_subtext_Tag' 
		AND		UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
	)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryTag]  WITH CHECK ADD  CONSTRAINT [FK_subtext_EntryTag_subtext_Tag] FOREIGN KEY([TagId])
	REFERENCES [<dbUser,varchar,dbo>].[subtext_Tag] ([Id])
END
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryTag] CHECK CONSTRAINT [FK_subtext_EntryTag_subtext_Tag]
GO