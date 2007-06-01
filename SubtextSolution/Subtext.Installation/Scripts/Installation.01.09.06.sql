/* 
	Need to have this file so that we'll kick off the Auto-Upgrade logic 
    because we need to update the Stored Procedures to fix some bugs
    
    WARNING: This SCRIPT USES SQL TEMPLATE PARAMETERS.
	Be sure to hit CTRL+SHIFT+M in Query Analyzer if running manually.
    
*/


IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'CustomMetaTags'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
		ADD [CustomMetaTags] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL
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