/*
Adds the LicenseUrl column to the blog_config table.  
If specified, this will be displayed in the RSS feed 
using the CreativeCommons module (see http://backend.userland.com/creativeCommonsRssModule). 
This can be used to display any license.
*/
IF NOT EXISTS 
(
	SELECT * FROM SysObjects O INNER JOIN SysColumns C ON O.ID=C.ID
	WHERE ObjectProperty(O.ID,'IsUserTable')=1
	AND O.Name = 'blog_Config'
	AND C.Name = 'LicenseUrl'
) 
BEGIN
	PRINT 'Adding Column LicenseUrl to Table blog_config'
	BEGIN TRANSACTION
	SET QUOTED_IDENTIFIER ON
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	SET ARITHABORT ON
	SET NUMERIC_ROUNDABORT OFF
	SET CONCAT_NULL_YIELDS_NULL ON
	SET ANSI_NULLS ON
	SET ANSI_PADDING ON
	SET ANSI_WARNINGS ON
	COMMIT
	BEGIN TRANSACTION
	ALTER TABLE dbo.blog_Config ADD
		LicenseUrl nvarchar(64) NULL
	COMMIT
END


/*
Adds the DaysTillCommentsClose column which specifies the number of 
days comments are allowed to be posted to an individual post.  Afterwards, 
comments are not allowed.
*/
IF NOT EXISTS 
(
	SELECT * FROM SysObjects O INNER JOIN SysColumns C ON O.ID=C.ID
	WHERE ObjectProperty(O.ID,'IsUserTable')=1
	AND O.Name = 'blog_Config'
	AND C.Name = 'DaysTillCommentsClose'
) 
BEGIN
	PRINT 'Adding Column DaysTillCommentsClose to Table blog_config'
	BEGIN TRANSACTION
	SET QUOTED_IDENTIFIER ON
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	SET ARITHABORT ON
	SET NUMERIC_ROUNDABORT OFF
	SET CONCAT_NULL_YIELDS_NULL ON
	SET ANSI_NULLS ON
	SET ANSI_PADDING ON
	SET ANSI_WARNINGS ON
	COMMIT
	BEGIN TRANSACTION
	ALTER TABLE dbo.blog_Config ADD
		DaysTillCommentsClose INT NULL
	COMMIT
END