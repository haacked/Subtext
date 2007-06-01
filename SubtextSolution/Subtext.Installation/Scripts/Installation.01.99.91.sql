/*
SCRIPT: Installation.01.99.91

NOTE:	This script does not correspond to a released version of Subtext.
		It is the second of three scripts used to upgrade Subtext from 1.9 to 2.0

ACTION: This script creates the Subtext Membership data based on data from 
		the pre-existing tables, if any.
*/


/* Create the Host Application */
IF(0 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Applications] WHERE ApplicationName = '/'))
BEGIN
	PRINT 'Creating Host Application'
	
	DECLARE @ApplicationId uniqueidentifier
	SET @ApplicationId = NEWID()
	
	INSERT [<dbUser,varchar,dbo>].[subtext_Applications]
	SELECT Application_Name = '/'
		, LoweredApplicationName = '/'
		, @ApplicationID
		, 'Host Admin Application'
END
GO

/* Create the HostAdmins Role */
IF(0 = (SELECT COUNT(1) 
		FROM [<dbUser,varchar,dbo>].[subtext_Roles] 
		WHERE LoweredRoleName = 'hostadmins' 
			AND ApplicationId = (SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Applications])
		)
	)
BEGIN
	PRINT 'Creating HostAdmins Role'
	
	INSERT [<dbUser,varchar,dbo>].[subtext_Roles]
		SELECT (SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Applications]), newId(), 'HostAdmins', 'hostadmins', 'Administrators of the installation of Subtext'
END
GO

/* Create the initial HostAdmin User if Upgrading */
IF(0 != (
			SELECT COUNT(1) 
			FROM [<dbUser,varchar,dbo>].[subtext_Host]
		)
	)
BEGIN
	-- Get Application Id
	
	DECLARE @HostAdminApplicationId uniqueidentifier
	SELECT @HostAdminApplicationId = ApplicationId 
		FROM [<dbUser,varchar,dbo>].[subtext_Applications]
		WHERE ApplicationName = '/'
		
	-- Get Host Admin Role
	DECLARE @HostAdminRoleId uniqueidentifier
	SELECT @HostAdminRoleId = RoleID 
	FROM [<dbUser,varchar,dbo>].[subtext_Roles]
	WHERE 
			ApplicationId = @HostAdminApplicationId
		AND LoweredRoleName = 'hostadmins'
	
	-- If we don't already have a host admin, add one.
	IF(0 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] WHERE RoleId = @HostAdminRoleId))
	BEGIN
		PRINT 'Creating HostAdmin User'

		DECLARE @UserId uniqueidentifier
		SET @UserId = NEWID()

		INSERT [<dbUser,varchar,dbo>].[subtext_Users]
			SELECT 
				UserId = @UserId
				, UserName = HostUserName
				, LoweredUserName = LOWER(HostUserName)
				, 0 --IsAnonymous
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
				, LastActivityDate = getdate()
				, LastLoginDate = getdate()
				, LastPasswordChangedDate = getdate()
				, LastLockoutDate = CONVERT( datetime, '17540101', 112 )  
				, FailedPasswordAttemptCount = 0
				, FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )  
				, FailedPasswordAnswerAttemptCount = 0
				, FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )  
				, Comment = 'Host Admin - Installation Owner'
			FROM [<dbUser,varchar,dbo>].[subtext_Host]
			
			PRINT 'Adding Host To HostAdmins Role'
			INSERT [<dbUser,varchar,dbo>].[subtext_UsersInRoles]
			SELECT @UserId, @HostAdminRoleId
			
			PRINT 'Setting the Host Owner'
			UPDATE [<dbUser,varchar,dbo>].[subtext_Host]
			SET OwnerId = @UserId
	END
END
ELSE
BEGIN
	UPDATE  [<dbUser,varchar,dbo>].[subtext_Host]
	SET OwnerId = CONVERT(uniqueIdentifier , '00000000-0000-0000-0000-000000000000')
	WHERE 1 = 0
END
GO


IF(0 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Applications] WHERE ApplicationId IN (SELECT ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Config])))
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
ELSE
BEGIN
	UPDATE  [<dbUser,varchar,dbo>].[subtext_Host]
	SET OwnerId = CONVERT(uniqueIdentifier , '00000000-0000-0000-0000-000000000000')
	WHERE 1 = 0
END
GO

/* Now, take the user in the Config table and add them as new Users
	and add them into the Administrators role */
IF(0 = (SELECT COUNT(1) 
	FROM [<dbUser,varchar,dbo>].[subtext_Users] u
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur ON u.UserId = ur.UserId
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r ON r.RoleId = ur.RoleId
	WHERE
		r.loweredrolename != 'hostadmins'	
	)
)
BEGIN
	PRINT 'Creating Blog Admin Users - 1 Per Blog'
	
	INSERT [<dbUser,varchar,dbo>].[subtext_Users]
		SELECT 
			newID()
			, UserName
			, LOWER(UserName)
			, 0 -- IsAnonymous
			, Password
			, PasswordFormat = 1
			, PasswordSalt = ''
			, MobilePIN = ''
			, Email
			, LoweredEmail = LOWER(Email)
			, PasswordQuestion = ''
			, PasswordAnswer = ''
			, IsApproved = 1
			, IsLockedOut = 0
			, CreateDate = LastUpdated
			, LastActivityDate = getdate()
			, LastLoginDate = getdate()
			, LastPasswordChangedDate = getdate()
			, LastLockoutDate = CONVERT( datetime, '17540101', 112 )  
			, FailedPasswordAttemptCount = 0
			, FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )  
			, FailedPasswordAnswerAttemptCount = 0
			, FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )  
			, Comment = CAST(ApplicationId AS VARCHAR(64))
		FROM [<dbUser,varchar,dbo>].[subtext_Config]
	
	PRINT 'Adding Blog Users To Admin Role'
	INSERT [<dbUser,varchar,dbo>].[subtext_UsersInRoles]
		SELECT u.UserId
			, r.RoleId
		FROM [<dbUser,varchar,dbo>].[subtext_Users] u
			INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r 
				ON CAST(r.ApplicationId as VARCHAR(64)) = CAST(u.Comment as VARCHAR(64))
		WHERE 
			u.UserId NOT IN (SELECT UserId FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles])
			AND r.RoleName = 'Administrators'

	PRINT 'Setting Blog Owner'
	UPDATE cf
	SET OwnerId = u.UserId
	FROM [<dbUser,varchar,dbo>].[subtext_Config] cf
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_Users] u 
			ON CAST(cf.ApplicationId as VARCHAR(64)) = CAST(u.Comment as VARCHAR(64))
END
GO

/* tie the field to the Users field */
IF NOT EXISTS(
    SELECT * 
    FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
    WHERE CONSTRAINT_NAME = 'FK_subtext_Users_subtext_Config' 
      AND CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
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
ELSE
BEGIN
	UPDATE  [<dbUser,varchar,dbo>].[subtext_Host]
	SET OwnerId = CONVERT(uniqueIdentifier , '00000000-0000-0000-0000-000000000000')
	WHERE 1 = 0
END
GO

/* Now set the FK so that it always points to Users */
IF NOT EXISTS(
    SELECT * 
    FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
    WHERE CONSTRAINT_NAME = 'FK_subtext_Users_subtext_Content' 
      AND CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
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
