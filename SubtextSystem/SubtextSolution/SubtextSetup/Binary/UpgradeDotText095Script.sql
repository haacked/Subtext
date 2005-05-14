/*
Adds the LicenseUrl column to the blog_config table.  
If specified, this will be displayed in the RSS feed 
using the CreativeCommons module (see http://backend.userland.com/creativeCommonsRssModule). 
This can be used to display any license.
*/
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
GO
COMMIT
