/* need to fix a bug with the trigger that updates our Entry, Story, Feedback, and PingTrack counts */
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_Content_Trigger]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [<dbUser,varchar,dbo>].[subtext_Content_Trigger]
GO
