if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Host]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Host]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Content_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Content] DROP CONSTRAINT FK_blog_Content_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_EntryViewCount_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_EntryViewCount] DROP CONSTRAINT FK_blog_EntryViewCount_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Images_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Images] DROP CONSTRAINT FK_blog_Images_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_KeyWords_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_KeyWords] DROP CONSTRAINT FK_blog_KeyWords_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_LinkCategories_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_LinkCategories] DROP CONSTRAINT FK_blog_LinkCategories_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Links_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Links] DROP CONSTRAINT FK_blog_Links_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Referrals_blog_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Referrals] DROP CONSTRAINT FK_blog_Referrals_blog_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Links_blog_Content]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Links] DROP CONSTRAINT FK_blog_Links_blog_Content
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Images_blog_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Images] DROP CONSTRAINT FK_blog_Images_blog_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Links_blog_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Links] DROP CONSTRAINT FK_blog_Links_blog_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_blog_Referrals_blog_URLs]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[blog_Referrals] DROP CONSTRAINT FK_blog_Referrals_blog_URLs
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Content_Trigger]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[blog_Content_Trigger]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[iter_charlist_to_table]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[iter_charlist_to_table]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Config]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Config]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Content]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Content]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_EntryViewCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_EntryViewCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Images]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Images]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_KeyWords]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_KeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_LinkCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_LinkCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Links]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Links]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_Referrals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_Referrals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[blog_URLs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[blog_URLs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spamPostCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[spamPostCount]
GO