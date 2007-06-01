/*
SCRIPT: Installation.01.99.90

NOTE:	This script does not correspond to a released version of Subtext.
		It is the first of three scripts used to upgrade Subtext from 1.9 to 2.0

ACTION: This script creates the Subtext Membership Provider tables and 
		related schema changes. It adds the ApplicationId column to 
		subtext_Config and subtext_Host.
			
		A later script will remove outdated columns.
*/
/*
Search and Replace Regexes:

Table creation:
	SEARCH: IF NOT EXISTS \(SELECT \* FROM dbo\.sysobjects WHERE id = OBJECT_ID\(N'\[dbo\]\.\[{[^\]]+}\]'\) AND OBJECTPROPERTY\(id, N'IsUserTable'\) = 1\)
	REPLACE: IF NOT EXISTS\n(\n\tSELECT * FROM [INFORMATION_SCHEMA].[TABLES]\n\tWHERE [TABLE_NAME] = '\1'\n\tAND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'\n)
	
Foreign Keys:
	SEARCH: IF NOT EXISTS \(SELECT \* FROM dbo\.sysobjects WHERE id = OBJECT_ID\(N'\[dbo\]\.\[{[^\]]+}\]'\) AND type = 'F'\)
	REPLACE:IF NOT EXISTS\n(\n\tSELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]\n\tWHERE CONSTRAINT_NAME = '\1'\n\tAND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'\n)
*/

/* Add an ApplicationId to the subtext_Config table so we can 
	map a blog to its application */
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	[TABLE_NAME] = 'subtext_Config' 
		AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
		AND	[COLUMN_NAME] = 'ApplicationId'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] ADD
	[ApplicationId] uniqueidentifier NULL 
	CONSTRAINT DF_subtext_Config_ApplicationId DEFAULT NEWID()
GO

/* Create a new ApplicationId for each entry, to be used later when populating the Membership tables. */
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET [ApplicationId] = NEWID() WHERE [ApplicationId] IS NULL
GO

/* Add a Subfolder column to the subtext_Config table. We'll move the data in 
	the misnamed 'Application' column here.
*/
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	[TABLE_NAME] = 'subtext_Config' 
		AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
		AND	[COLUMN_NAME] = 'Subfolder'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] ADD
	[Subfolder] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
	CONSTRAINT DF_subtext_Config_Subfolder DEFAULT ''
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	[TABLE_NAME] = 'subtext_Config' 
		AND		[TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
		AND		[COLUMN_NAME] = 'Application')
AND EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	[TABLE_NAME] = 'subtext_Config' 
		AND		[TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
		AND		[COLUMN_NAME] = 'Subfolder')
BEGIN
	UPDATE [<dbUser,varchar,dbo>].[subtext_Config] 
	SET Subfolder = Application
END
GO

/* Add an ApplicationId to the subtext_Host table so we can 
	map the Subtext host installation to its application */
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	[TABLE_NAME] = 'subtext_Host' 
		AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
		AND	[COLUMN_NAME] = 'ApplicationId'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Host] ADD
	[ApplicationId] uniqueidentifier NOT NULL
	CONSTRAINT DF_subtext_Host_ApplicationId DEFAULT NEWID()
GO


/* add the AuthorId field to Content table */
IF NOT EXISTS 
	(
		SELECT	[COLUMN_NAME] 
		FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	[TABLE_NAME] = 'subtext_Content' 
		AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
		AND [COLUMN_NAME] = 'AuthorId'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] ADD
	[AuthorId]  uniqueidentifier NOT NULL 
	CONSTRAINT DF_subtext_Content_AuthorId DEFAULT CONVERT(uniqueidentifier , '00000000-0000-0000-0000-000000000000')
GO

/*	Adds OwnerId to the subtext_Host table. This is the overall owner 
	of the subtext installation. The default HostAdmin. */
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	[TABLE_NAME] = 'subtext_Host' 
		AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
		AND	[COLUMN_NAME] = 'OwnerId'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Host] ADD
	[OwnerId] uniqueidentifier NOT NULL 
	CONSTRAINT DF_subtext_Host_OwnerId DEFAULT CONVERT(uniqueidentifier , '00000000-0000-0000-0000-000000000000')
GO


/* Modifies the Config Table to have OwnerId which correlates to
   the UserId of the main Administrator (to be created next) */
IF NOT EXISTS 
	(
		SELECT	* FROM [INFORMATION_SCHEMA].[COLUMNS] 
		WHERE	[TABLE_NAME] = 'subtext_Config' 
		AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
		AND	[COLUMN_NAME] = 'OwnerId'
	)
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] ADD
	[OwnerId] uniqueidentifier NOT NULL 
	CONSTRAINT DF_subtext_Config_OwnerId DEFAULT CONVERT(uniqueidentifier , '00000000-0000-0000-0000-000000000000')
GO

/* THE FOLLOWING SCRIPTS ADD ALL THE MEMBERSHIP TABLES */

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_SchemaVersions'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_SchemaVersions](
	[Feature] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IsCurrentVersion] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Feature] ASC,
	[CompatibleSchemaVersion] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_WebEvent_Events'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_WebEvent_Events](
	[EventId] [char](32) NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[EventSequence] [decimal](19, 0) NOT NULL,
	[EventOccurrence] [decimal](19, 0) NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[Message] [nvarchar](1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ApplicationPath] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ApplicationVirtualPath] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MachineName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RequestUrl] [nvarchar](1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ExceptionType] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Details] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_Applications'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Applications](
	[ApplicationName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LoweredApplicationName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Description] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_UsersInRoles'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_PersonalizationAllUsers'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers](
	[PathId] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PathId] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_PersonalizationPerUser'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[PathId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_Users'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Users](
	[UserId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[UserName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LoweredUserName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IsAnonymous] [bit] NOT NULL DEFAULT (0),
	/* Columns that used to be in the Membership table */
	[Password] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PasswordFormat] [int] NOT NULL DEFAULT (0),
	[PasswordSalt] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MobilePIN] [nvarchar](16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Email] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LoweredEmail] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PasswordQuestion] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PasswordAnswer] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,

PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_Roles'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[RoleName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LoweredRoleName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_Paths'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Paths](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Path] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LoweredPath] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[PathId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[TABLES]
	WHERE [TABLE_NAME] = 'subtext_Profile'
	AND [TABLE_SCHEMA] = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [<dbUser,varchar,dbo>].[subtext_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PropertyValuesString] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PropertyValuesBinary] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK__subtext_U__RoleI__31EC6D26'
	AND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [<dbUser,varchar,dbo>].[subtext_Roles] ([RoleId])
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK__subtext_U__UserI__30F848ED'
	AND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [<dbUser,varchar,dbo>].[subtext_Users] ([UserId])
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK__subtext_P__PathI__45F365D3'
	AND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [<dbUser,varchar,dbo>].[subtext_Paths] ([PathId])
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK__subtext_P__PathI__49C3F6B7'
	AND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [<dbUser,varchar,dbo>].[subtext_Paths] ([PathId])
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK__subtext_P__UserI__4AB81AF0'
	AND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [<dbUser,varchar,dbo>].[subtext_Users] ([UserId])
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK__subtext_R__Appli__2D27B809'
	AND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Roles]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [<dbUser,varchar,dbo>].[subtext_Applications] ([ApplicationId])
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK__subtext_P__Appli__403A8C7D'
	AND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Paths]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [<dbUser,varchar,dbo>].[subtext_Applications] ([ApplicationId])
GO

IF NOT EXISTS
(
	SELECT * FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS]
	WHERE CONSTRAINT_NAME = 'FK__subtext_P__UserI__239E4DCF'
	AND UNIQUE_CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [<dbUser,varchar,dbo>].[subtext_Users] ([UserId])
GO
