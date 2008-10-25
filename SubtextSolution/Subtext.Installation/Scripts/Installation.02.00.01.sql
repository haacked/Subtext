/*----------- {Config/Rss Email Author} ------------*/
/*add the ShowEmailRssFeed column */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'ShowEmailRssFeed'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [ShowEmailRssFeed] [bit] NULL
END
GO
