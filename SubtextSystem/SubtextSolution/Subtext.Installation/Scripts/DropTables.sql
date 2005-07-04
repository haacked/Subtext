if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_Host]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_Host]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Content_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_Content] DROP CONSTRAINT FK_subtext_Content_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_EntryViewCount_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_EntryViewCount] DROP CONSTRAINT FK_subtext_EntryViewCount_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Images_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_Images] DROP CONSTRAINT FK_subtext_Images_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_KeyWords_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_KeyWords] DROP CONSTRAINT FK_subtext_KeyWords_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_LinkCategories_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_LinkCategories] DROP CONSTRAINT FK_subtext_LinkCategories_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Referrals_subtext_Config]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_Referrals] DROP CONSTRAINT FK_subtext_Referrals_subtext_Config
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_Content]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_Content
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Images_subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_Images] DROP CONSTRAINT FK_subtext_Images_subtext_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Links_subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_Links] DROP CONSTRAINT FK_subtext_Links_subtext_LinkCategories
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[FK_subtext_Referrals_subtext_URLs]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[subtext_Referrals] DROP CONSTRAINT FK_subtext_Referrals_subtext_URLs
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_Content_Trigger]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[subtext_Content_Trigger]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[iter_charlist_to_table]') and xtype in (N'FN', N'IF', N'TF'))
drop function [iter_charlist_to_table]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_Config]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_Config]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_Content]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_Content]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_EntryViewCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_EntryViewCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_Images]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_Images]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_KeyWords]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_KeyWords]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_LinkCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_LinkCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_Links]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_Links]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_Referrals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_Referrals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[subtext_URLs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[subtext_URLs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[spamPostCount]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [spamPostCount]
GO