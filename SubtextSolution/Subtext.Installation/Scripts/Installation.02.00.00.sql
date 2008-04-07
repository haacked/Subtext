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
		CONSTRAINT [PK_subtext_Enclosure] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)
		ON [PRIMARY]
	)
END
GO