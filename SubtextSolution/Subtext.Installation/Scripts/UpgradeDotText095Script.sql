/* This script needs to be updated since table and stored proc names have changed */

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
	ALTER TABLE [blog_Config] ADD
		LicenseUrl nvarchar(64) NULL
	COMMIT
END

GO
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
	ALTER TABLE [blog_Config] ADD
		[DaysTillCommentsClose] INT NULL
	COMMIT
END
GO
/*
Adds the CommentDelayInMinutes column which, if specified, determines 
the number of minutes that must elapse between posts from the same 
IP address.
*/
IF NOT EXISTS 
(
	SELECT * FROM SysObjects O INNER JOIN SysColumns C ON O.ID=C.ID
	WHERE ObjectProperty(O.ID,'IsUserTable')=1
	AND O.Name = 'blog_Config'
	AND C.Name = 'CommentDelayInMinutes'
) 
BEGIN
	PRINT 'Adding Column CommentDelayInMinutes to Table blog_config'
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
	ALTER TABLE [blog_Config] ADD
		[CommentDelayInMinutes] INT NULL
	COMMIT
END
GO
/*
Adds the ContentChecksumHash column to the blog_content table. 
This is used as a fast way to check for duplicate content.
*/
IF NOT EXISTS 
(
	SELECT * FROM SysObjects O INNER JOIN SysColumns C ON O.ID=C.ID
	WHERE ObjectProperty(O.ID,'IsUserTable')=1
	AND O.Name = 'blog_content'
	AND C.Name = 'ContentChecksumHash'
) 
BEGIN
	PRINT 'Adding Column ContentChecksumHash to Table blog_content'
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
	ALTER TABLE [blog_content] ADD
		ContentChecksumHash VARCHAR(32) NULL
	COMMIT
END
GO
/*
Add the blog_Host table.
*/
IF NOT exists (select * from dbo.sysobjects where id = object_id(N'[blog_Host]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
CREATE TABLE [blog_Host] (
	[HostUserName] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Password] [nvarchar] (64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Salt] [nvarchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
/*
Update various tables so that they conform to Foreign Key Constraints.  This 
primarily means having values of -1 be updated to NULL.
*/
UPDATE [blog_Links]
SET	PostID = NULL WHERE PostID = -1

UPDATE [blog_Content]
SET	ParentID = NULL WHERE ParentID = -1

GO
/*
Adds the DateSyndicated column to the blog_content table. 
*/
IF NOT EXISTS 
(
	SELECT * FROM SysObjects O INNER JOIN SysColumns C ON O.ID=C.ID
	WHERE ObjectProperty(O.ID,'IsUserTable')=1
	AND O.Name = 'blog_content'
	AND C.Name = 'DateSyndicated'
) 
BEGIN
	PRINT 'Adding Column DateSyndicated to Table blog_content'
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
	ALTER TABLE [blog_content] ADD
		DateSyndicated DateTime NULL
	COMMIT
END
GO

UPDATE blog_Content 
SET DateSyndicated = DateUpdated
-- Post is syndicated and active
WHERE PostConfig & 16 = 16 AND PostConfig & 1 = 1

/*
Adds the NumberOfRecentComments column which specifies the number of 
comments to display in the RecentComments control.
*/
IF NOT EXISTS 
(
	SELECT * FROM SysObjects O INNER JOIN SysColumns C ON O.ID=C.ID
	WHERE ObjectProperty(O.ID,'IsUserTable')=1
	AND O.Name = 'subtext_Config'
	AND C.Name = 'NumberOfRecentComments'
) 
BEGIN
	PRINT 'Adding Column NumberOfRecentComments to Table subtext_config'
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
	ALTER TABLE [subtext_Config] ADD
		[NumberOfRecentComments] INT NULL
	COMMIT
END
GO

/*
Adds the RecentCommentsLength column which specifies the number of characters
to display for recent comments in the RecentComments control.
*/
IF NOT EXISTS 
(
	SELECT * FROM SysObjects O INNER JOIN SysColumns C ON O.ID=C.ID
	WHERE ObjectProperty(O.ID,'IsUserTable')=1
	AND O.Name = 'subtext_Config'
	AND C.Name = 'RecentCommentsLength'
) 
BEGIN
	PRINT 'Adding Column RecentCommentsLength to Table subtext_config'
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
	ALTER TABLE [subtext_Config] ADD
		[RecentCommentsLength] INT NULL
	COMMIT
END
GO