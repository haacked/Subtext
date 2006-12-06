/*
SCRIPT: Installation.01.99.91

NOTE:	This script does not correspond to a released version of Subtext.
		It is the second of three scripts used to upgrade Subtext from 1.9 to 2.0

ACTION: This script creates the Subtext Membership data based on data from 
		the pre-existing tables, if any.
*/


/* Create the Host Application */
IF(0 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Applications] WHERE ApplicationName = '/')
	AND 0 != (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Host])
)
BEGIN
	PRINT 'Creating Host Application'
	
	INSERT [<dbUser,varchar,dbo>].[subtext_Applications]
	SELECT Application_Name = '/'
		, LoweredApplicationName = '/'
		, (SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Host])
		, 'Host Admin Application'
END
GO

/* Create the HostAdmins Role */
IF(0 = (SELECT COUNT(1) 
		FROM [<dbUser,varchar,dbo>].[subtext_Roles] 
		WHERE LoweredRoleName = 'hostadmins' 
			AND ApplicationId = (SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Host])
		)
	)
BEGIN
	PRINT 'Creating HostAdmins Role'
	
	INSERT [<dbUser,varchar,dbo>].[subtext_Roles]
		SELECT (SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Host]), newId(), 'HostAdmins', 'hostadmins', 'Administrators of the installation of Subtext'
END
GO

/* Create the initial HostAdmin User */
IF(0 = (SELECT COUNT(1) 
		FROM [<dbUser,varchar,dbo>].[subtext_Users] 
		WHERE ApplicationId = (SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Host])
		)
	)
BEGIN
	PRINT 'Creating HostAdmin User'
	
	DECLARE @UserId UNIQUEIDENTIFIER
	SET @UserId = NEWID()

	INSERT [<dbUser,varchar,dbo>].[subtext_Users]
		SELECT ApplicationId = (SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Host])
			, UserId = @UserId
			, UserName = HostUserName
			, LoweredUserName = LOWER(HostUserName)
			, MobileAlias = LOWER(HostUserName)
			, 0
			, getdate()
		FROM subtext_Host
		
	PRINT 'Creating HostAdmin Membership'
	
	INSERT [<dbUser,varchar,dbo>].[subtext_Membership]
		SELECT ApplicationId = (SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Host])
			, UserId = @UserId
			, Password
			, PasswordFormat = 1
			, PasswordSalt = ''
			, MobilePIN = ''
			, Email = 'unknown'
			, LoweredEmail = 'unknown'
			, PasswordQuestion = ''
			, PasswordAnswer = ''
			, IsApproved = 1
			, IsLockedOut = 0
			, CreateDate = DateCreated
			, LastLoginDate = getdate()
			, LastPasswordChangedDate = getdate()
			, LastLockoutDate = CONVERT( datetime, '17540101', 112 )  
			, FailedPasswordAttemptCount = 0
			, FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )  
			, FailedPasswordAnswerAttemptCount = 0
			, FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )  
			, Comment = 'Host Admin - Installation Owner'
		FROM subtext_Host
		
		PRINT 'Adding Host To HostAdmins Role'
		INSERT [<dbUser,varchar,dbo>].[subtext_UsersInRoles]
		SELECT @UserId, RoleId FROM [<dbUser,varchar,dbo>].[subtext_Roles] WHERE RoleName = 'HostAdmins'
		
		PRINT 'Setting the Host Owner'
		
		UPDATE [<dbUser,varchar,dbo>].[subtext_Host]
		SET OwnerId = @UserId
END
GO


IF(0 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Applications] WHERE ApplicationID IN (SELECT ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Config])))
BEGIN
	PRINT 'Creating an Application for every entry in subtext_Config'

	INSERT [<dbUser,varchar,dbo>].[subtext_Applications]
	SELECT ApplicationName = Host + '/' + Application 
			, LoweredApplicationName = LOWER(Host + '/' + Application)
			, ApplicationID
			, Description = Title
	FROM [<dbUser,varchar,dbo>].[subtext_Config]
END
GO

--Now, for each application (if any exist) we need to create all the roles
IF(0 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Roles] WHERE RoleName = 'Administrators'))
BEGIN
	PRINT 'Creating Roles For Each Blog'	

	INSERT INTO [<dbUser,varchar,dbo>].[subtext_Roles]
	SELECT ApplicationId
		, RoleId = NEWID()
		, RoleName = 'Administrators'
		, LoweredRoleName = 'administrators'
		, Description = 'Administrators have full control over a blog.  They cannot create new blogs though.'
	FROM [<dbUser,varchar,dbo>].[subtext_Applications]
	UNION
	SELECT ApplicationId
		, RoleId = NEWID()
		, RoleName = 'PowerUsers'
		, LoweredRoleName = 'powerusers'
		, Description = 'PowerUsers can maintain a blog and change most settings.  Cannot change users or the owner of the blog.'
	FROM [<dbUser,varchar,dbo>].[subtext_Applications]
	UNION
	SELECT ApplicationId
		, RoleId = NEWID()
		, RoleName = 'Authors'
		, LoweredRoleName = 'authors'
		, Description = 'Authors may add and edit blog posts.'
	FROM [<dbUser,varchar,dbo>].[subtext_Applications]
	UNION
	SELECT ApplicationId
		, RoleId = NEWID()
		, RoleName = 'Commenters'
		, LoweredRoleName = 'commenters'
		, Description = 'Commenters may leave a comment on a blog post, if a blog restricts comments to this role.'
	FROM [<dbUser,varchar,dbo>].[subtext_Applications]
	UNION
	SELECT ApplicationId
		, RoleId = NEWID()
		, RoleName = 'Anonymous'
		, LoweredRoleName = 'anonymous'
		, Description = 'Role for all other users'
	FROM [<dbUser,varchar,dbo>].[subtext_Applications]
END
GO

/* Now, take the user in the Config table and add them as new Users
	and add them into the Administrators role */
IF(0 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Users] WHERE ApplicationId != (SELECT ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Applications] WHERE ApplicationName = '/')))
BEGIN
	PRINT 'Creating Blog Users'
	
	INSERT [<dbUser,varchar,dbo>].[subtext_Users]
		SELECT ApplicationId
			, newID()
			, UserName
			, LOWER(UserName)
			, LOWER(UserName)
			, 0
			, getdate()
		FROM [<dbUser,varchar,dbo>].[subtext_Config]
	
	PRINT 'Creating Blog Members'
	INSERT [<dbUser,varchar,dbo>].[subtext_Membership]
		SELECT cf.ApplicationId
			, u.UserId
			, cf.Password
			, PasswordFormat = 1
			, PasswordSalt = ''
			, MobilePIN = ''
			, cf.Email
			, LoweredEmail = LOWER(cf.Email)
			, PasswordQuestion = ''
			, PasswordAnswer = ''
			, IsApproved = 1
			, IsLockedOut = 0
			, CreateDate = cf.LastUpdated
			, LastLoginDate = getdate()
			, LastPasswordChangedDate = getdate()
			, LastLockoutDate = CONVERT( datetime, '17540101', 112 )  
			, FailedPasswordAttemptCount = 0
			, FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )  
			, FailedPasswordAnswerAttemptCount = 0
			, FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )  
			, Comment = CAST(cf.BlogId AS VARCHAR(64))
		FROM [<dbUser,varchar,dbo>].[subtext_Users] u
			INNER JOIN [<dbUser,varchar,dbo>].[subtext_Config] cf ON cf.UserName = u.UserName
		WHERE u.ApplicationId NOT IN (SELECT ApplicationId FROM subtext_Membership)

	PRINT 'Adding Blog Users To Admin Role'
	INSERT [<dbUser,varchar,dbo>].[subtext_UsersInRoles]
		SELECT u.UserId
			, r.RoleId
		FROM [<dbUser,varchar,dbo>].[subtext_Users] u
			INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r ON r.ApplicationId = u.ApplicationId
		WHERE 
			u.UserId NOT IN (SELECT UserId FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles])
			AND r.RoleName = 'Administrators'

	PRINT 'Setting Blog Owner'
	UPDATE cf
	SET OwnerId = 
		(
			SELECT u.UserId
			FROM [<dbUser,varchar,dbo>].[subtext_Applications] a 
				INNER JOIN [<dbUser,varchar,dbo>].[subtext_Users] u ON a.ApplicationId = u.ApplicationId
			WHERE u.UserName = UserName
				AND a.ApplicationName != '/'
		)
	FROM [<dbUser,varchar,dbo>].[subtext_Config] cf
END
GO

/* make the OwnerId unique */
IF NOT EXISTS(
	SELECT * 
	FROM [information_schema].[constraint_column_usage]
	WHERE constraint_name = 'IX_subtext_Config_UniqueOwner'
	AND constraint_schema = '<dbUser,varchar,dbo>'
	)
BEGIN
	PRINT 'Setting Blog Owner To Be Unique'
	ALTER TABLE subtext_Config 
		ADD CONSTRAINT
		IX_subtext_Config_UniqueOwner UNIQUE NONCLUSTERED 
		(
		OwnerId
		) ON [PRIMARY]
END
GO
	
/* tie the field to the Users field */
IF NOT EXISTS(
    SELECT * 
    FROM [information_schema].[referential_constraints] 
    WHERE constraint_name = 'FK_subtext_Users_subtext_Config' 
      AND constraint_schema = '<dbUser,varchar,dbo>'
)
BEGIN
	PRINT 'Setting Blog Owner To Be A Foreign Key to subtext_Users.UserId'
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] WITH NOCHECK 
	ADD CONSTRAINT FK_subtext_Users_subtext_Config FOREIGN KEY
	(
	OwnerId
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Users]
	(
	UserId
	)
END
GO

/* Update the content, adding the OwnerId to the AuthorId field */
IF EXISTS (select * from [<dbUser,varchar,dbo>].[subtext_content])
BEGIN
	PRINT 'Updating subtext_Content with owners'
	UPDATE [<dbUser,varchar,dbo>].[subtext_content]
	SET AuthorId = cf.[OwnerId]
	FROM    [<dbUser,varchar,dbo>].[subtext_Content] c
		INNER JOIN
			[<dbUser,varchar,dbo>].[subtext_Config] cf ON c.BlogId = cf.BlogId
END

/* Now set the FK so that it always points to Users */
IF NOT EXISTS(
    SELECT * 
    FROM [information_schema].[referential_constraints] 
    WHERE constraint_name = 'FK_subtext_Users_subtext_Content' 
      AND constraint_schema = '<dbUser,varchar,dbo>'
)
BEGIN
	PRINT 'Updating subtext_Content.AuthorId to be a Foreign Key to subtext_Users.UserId'
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content] WITH NOCHECK 
	ADD CONSTRAINT FK_subtext_Users_subtext_Content FOREIGN KEY
	(
	AuthorId
	) REFERENCES [<dbUser,varchar,dbo>].[subtext_Users]
	(
	UserId
	)
END
GO

/* Need to set the password questions and answers */
UPDATE [<dbUser,varchar,dbo>].[subtext_Membership] 
	SET PasswordQuestion = 'No Question Specified. Please type the word "subtext"', PasswordAnswer='subtext'
	WHERE PasswordQuestion IS NULL OR PasswordQuestion = ''