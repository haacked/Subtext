/*
Search and Replace Regexes:

Table creation:
	SEARCH: IF NOT EXISTS \(SELECT \* FROM dbo\.sysobjects WHERE id = OBJECT_ID\(N'\[dbo\]\.\[{[^\]]+}\]'\) AND OBJECTPROPERTY\(id, N'IsUserTable'\) = 1\)
	REPLACE: IF NOT EXISTS\n(\n\tSELECT * FROM [information_schema].[tables]\n\tWHERE table_name = '\1'\n\tAND table_schema = '<dbUser,varchar,dbo>'\n)
	
Foreign Keys:
	SEARCH: IF NOT EXISTS \(SELECT \* FROM dbo\.sysobjects WHERE id = OBJECT_ID\(N'\[dbo\]\.\[{[^\]]+}\]'\) AND type = 'F'\)
	REPLACE:IF NOT EXISTS\n(\n\tSELECT * FROM [information_schema].[referential_constraints]\n\tWHERE constraint_name = '\1'\n\tAND unique_constraint_schema = '<dbUser,varchar,dbo>'\n)
*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_SchemaVersions'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_SchemaVersions](
	[Feature] [nvarchar](128) NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) NOT NULL,
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_WebEvent_Events'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_WebEvent_Events](
	[EventId] [char](32) NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](256) NOT NULL,
	[EventSequence] [decimal](19, 0) NOT NULL,
	[EventOccurrence] [decimal](19, 0) NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[Message] [nvarchar](1024) NULL,
	[ApplicationPath] [nvarchar](256) NULL,
	[ApplicationVirtualPath] [nvarchar](256) NULL,
	[MachineName] [nvarchar](256) NOT NULL,
	[RequestUrl] [nvarchar](1024) NULL,
	[ExceptionType] [nvarchar](256) NULL,
	[Details] [ntext] NULL,
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_Applications'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_Applications](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Description] [nvarchar](256) NULL,
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_UsersInRoles'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_UsersInRoles](
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_PersonalizationAllUsers'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_PersonalizationAllUsers](
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_PersonalizationPerUser'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_PersonalizationPerUser](
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_Users'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[MobileAlias] [nvarchar](16) NULL DEFAULT (null),
	[IsAnonymous] [bit] NOT NULL DEFAULT (0),
	[LastActivityDate] [datetime] NOT NULL,
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_Roles'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_Paths'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_Paths](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Path] [nvarchar](256) NOT NULL,
	[LoweredPath] [nvarchar](256) NOT NULL,
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_Membership'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_Membership](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL DEFAULT (0),
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
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
	SELECT * FROM [information_schema].[tables]
	WHERE table_name = 'subtext_Profile'
	AND table_schema = '<dbUser,varchar,dbo>'
)
BEGIN
CREATE TABLE [dbo].[subtext_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [ntext] NOT NULL,
	[PropertyValuesString] [ntext] NOT NULL,
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
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_U__RoleI__31EC6D26'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[subtext_Roles] ([RoleId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_U__UserI__30F848ED'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[subtext_Users] ([UserId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_P__PathI__45F365D3'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_PersonalizationAllUsers]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[subtext_Paths] ([PathId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_P__PathI__49C3F6B7'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[subtext_Paths] ([PathId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_P__UserI__4AB81AF0'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[subtext_Users] ([UserId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_U__Appli__7E6CC920'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_Users]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[subtext_Applications] ([ApplicationId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_R__Appli__2D27B809'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_Roles]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[subtext_Applications] ([ApplicationId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_P__Appli__403A8C7D'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_Paths]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[subtext_Applications] ([ApplicationId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_M__Appli__0EA330E9'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_Membership]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[subtext_Applications] ([ApplicationId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_M__UserI__0F975522'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_Membership]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[subtext_Users] ([UserId])
GO

IF NOT EXISTS
(
	SELECT * FROM [information_schema].[referential_constraints]
	WHERE constraint_name = 'FK__subtext_P__UserI__239E4DCF'
	AND unique_constraint_schema = '<dbUser,varchar,dbo>'
)
ALTER TABLE [dbo].[subtext_Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[subtext_Users] ([UserId])
