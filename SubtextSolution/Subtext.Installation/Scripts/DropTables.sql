/*
WARNING: This SCRIPT USES SQL TEMPLATE PARAMETERS.
Be sure to hit CTRL+SHIFT+M in Query Analyzer if running manually.
*/

if exists (select * from dbo.sysobjects where id = object_id(N'[PK_subtext_Log]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] DROP CONSTRAINT PK_subtext_Log
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Log_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Log] DROP CONSTRAINT FK_subtext_Log_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Log]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [subtext_Log]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Log]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Log]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Version]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Version]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Host]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Host]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Content_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] DROP CONSTRAINT FK_subtext_Content_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_EntryViewCount_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_EntryViewCount] DROP CONSTRAINT FK_subtext_EntryViewCount_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Images_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Images] DROP CONSTRAINT FK_subtext_Images_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_KeyWords_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_KeyWords] DROP CONSTRAINT FK_subtext_KeyWords_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_LinkCategories_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_LinkCategories] DROP CONSTRAINT FK_subtext_LinkCategories_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Referrals_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Referrals] DROP CONSTRAINT FK_subtext_Referrals_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_Content]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_Content
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Images_subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Images] DROP CONSTRAINT FK_subtext_Images_subtext_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Referrals_subtext_URLs]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Referrals] DROP CONSTRAINT FK_subtext_Referrals_subtext_URLs
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Content_Trigger]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [<dbUser,varchar,dbo>].[subtext_Content_Trigger]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Config]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Config]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Content]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Content]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_EntryViewCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_EntryViewCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Images]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Images]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_KeyWords]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_KeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_LinkCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Links]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Links]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Referrals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_Referrals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_URLs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [<dbUser,varchar,dbo>].[subtext_URLs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[spamPostCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [spamPostCount]
GO
