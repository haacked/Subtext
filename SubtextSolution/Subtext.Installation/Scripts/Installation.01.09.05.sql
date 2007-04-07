/* Need to have this file so that we'll kick off the Auto-Upgrade logic 
    because we need to update the Stored Procedures to fix some bugs
    
    - Fixing the blog creation proc.
*/

IF NOT EXISTS 
(
    SELECT * FROM [information_schema].[columns] 
    WHERE   table_name = 'subtext_KeyWords' 
    AND table_schema = 'dbo'
    AND column_name = 'Rel'
)
BEGIN
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords]
		ADD [Rel] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
END
GO