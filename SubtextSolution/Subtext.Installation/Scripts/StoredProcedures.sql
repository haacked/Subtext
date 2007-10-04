/*
WARNING: This SCRIPT USES SQL TEMPLATE PARAMETERS.
Be sure to hit CTRL+SHIFT+M in Query Analyzer if running manually.

When generating drop and create from SQL Query Analyzer, you can 
use the following search and replace expressions to convert the 
script to use INFORMATION_SCHEMA.

SEARCH:  IF:b* EXISTS \(SELECT \* FROM dbo\.sysobjects WHERE id = OBJECT_ID\(N'\[[^\]]+\]\.\[{[^\]]+}\]'\) AND OBJECTPROPERTY\(id,:b*N'IsProcedure'\) = 1\)
REPLACE: IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = '\1' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')

*/

/* DROPPED STORED PROCS.  
	These are stored procs that used to be in the system but are no longer needed.
	The statements will only drop the procs if they exist as a form of cleanup.
*/
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertViewStats' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertViewStats]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UTILITY_AddBlog' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UTILITY_AddBlog]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateHost' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateHost]

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetAllCategories]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetAllCategories]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetCategoryByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetCategoryByName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableReferrersByEntryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableReferrersByEntryID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetConfig' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetConfig] -- RENAMED subtext_GetConfig to subtext_GetBlog. So we're making sure to drop the old one.
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetHost' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetHost]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Utility_GetUnHashedPasswords' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Utility_GetUnHashedPasswords]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Utility_UpdateToHashedPassword' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Utility_UpdateToHashedPassword]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableReferrersByEntryID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableReferrersByEntryID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetBlogsByHost' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetBlogsByHost]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetConditionalEntriesByDateUpdated' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetConditionalEntriesByDateUpdated]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetEntryCollectionByDateUpdated' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetEntryCollectionByDateUpdated]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByCategoryNameByDateUpdated' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryNameByDateUpdated]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByCategoryIDByDateUpdated' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryIDByDateUpdated]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetEntryWithCategoryTitlesByEntryName' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetEntryWithCategoryTitlesByEntryName]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByCategoryName' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryName]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetRecentEntriesByDateUpdated' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetRecentEntriesByDateUpdated]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetRecentEntriesWithCategoryTitles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetRecentEntriesWithCategoryTitles]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetRecentEntries' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetRecentEntries]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetSingleEntryByName' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetSingleEntryByName]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetEntryWithCategoryTitles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetEntryWithCategoryTitles]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertPostCategoryByName' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertPostCategoryByName]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableLinksByCategoryID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]
GO

/* The Rest of the script */

-- Views
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[VIEWS] WHERE TABLE_NAME = 'vw_subtext_Profiles' AND TABLE_SCHEMA = '<dbUser,varchar,dbo>')
DROP VIEW [<dbUser,varchar,dbo>].[vw_subtext_Profiles]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[VIEWS] WHERE TABLE_NAME = 'vw_subtext_Roles' AND TABLE_SCHEMA = '<dbUser,varchar,dbo>')
DROP VIEW [<dbUser,varchar,dbo>].[vw_subtext_Roles]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[VIEWS] WHERE TABLE_NAME = 'vw_subtext_UsersInRoles' AND TABLE_SCHEMA = '<dbUser,varchar,dbo>')
DROP VIEW [<dbUser,varchar,dbo>].[vw_subtext_UsersInRoles]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[VIEWS] WHERE TABLE_NAME = 'vw_subtext_WebPartState_Paths' AND TABLE_SCHEMA = '<dbUser,varchar,dbo>')
DROP VIEW [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Paths]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[VIEWS] WHERE TABLE_NAME = 'vw_subtext_WebPartState_Shared' AND TABLE_SCHEMA = '<dbUser,varchar,dbo>')
DROP VIEW [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Shared]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_VersionAdd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_VersionAdd]

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[VIEWS] WHERE TABLE_NAME = 'vw_subtext_WebPartState_User' AND TABLE_SCHEMA = '<dbUser,varchar,dbo>')
DROP VIEW [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_User]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[VIEWS] WHERE TABLE_NAME = 'vw_subtext_Applications' AND TABLE_SCHEMA = '<dbUser,varchar,dbo>')
DROP VIEW [<dbUser,varchar,dbo>].[vw_subtext_Applications]
GO

-- Membership Provider Stored Procs
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_RegisterSchemaVersion' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_CheckSchemaVersion' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UnRegisterSchemaVersion' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_AnyDataInTables' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_AnyDataInTables]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_CreateUser' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_CreateUser]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_GetUserByName' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByName]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_GetUserByUserId' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByUserId]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_GetUserByEmail' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByEmail]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_GetPasswordWithFormat' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetPasswordWithFormat]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_UpdateUserInfo' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_UpdateUserInfo]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_GetPassword' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetPassword]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_SetPassword' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_SetPassword]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_ResetPassword' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_ResetPassword]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_UnlockUser' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_UnlockUser]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_UpdateUser' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_UpdateUser]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_ChangePasswordQuestionAndAnswer' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_ChangePasswordQuestionAndAnswer]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_GetAllUsers' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetAllUsers]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_GetNumberOfUsersOnline' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetNumberOfUsersOnline]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Membership_FindUsersByNameOrEmail' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_FindUsersByNameOrEmail]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Profile_GetProperties' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_GetProperties]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Profile_SetProperties' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_SetProperties]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Profile_DeleteProfiles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_DeleteProfiles]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Profile_DeleteInactiveProfiles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_DeleteInactiveProfiles]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Profile_GetNumberOfInactiveProfiles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_GetNumberOfInactiveProfiles]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Profile_GetProfiles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_GetProfiles]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UsersInRoles_IsUserInRole' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_IsUserInRole]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UsersInRoles_GetRolesForUser' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_GetRolesForUser]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Roles_CreateRole' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Roles_CreateRole]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Roles_DeleteRole' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Roles_DeleteRole]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Roles_RoleExists' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Roles_RoleExists]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UsersInRoles_AddUsersToRoles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_AddUsersToRoles]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UsersInRoles_RemoveUsersFromRoles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_RemoveUsersFromRoles]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UsersInRoles_GetUsersInRoles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_GetUsersInRoles]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UsersInRoles_FindUsersInRole' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_FindUsersInRole]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Roles_GetAllRoles' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Roles_GetAllRoles]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationAllUsers_GetPageSettings' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_GetPageSettings]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationAllUsers_ResetPageSettings' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_ResetPageSettings]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationAllUsers_SetPageSettings' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_SetPageSettings]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationPerUser_GetPageSettings' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_GetPageSettings]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationPerUser_ResetPageSettings' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_ResetPageSettings]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationPerUser_SetPageSettings' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_SetPageSettings]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationAdministration_DeleteAllState' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_DeleteAllState]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationAdministration_ResetSharedState' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_ResetSharedState]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationAdministration_ResetUserState' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_ResetUserState]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationAdministration_FindState' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_FindState]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_PersonalizationAdministration_GetCountOfState' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_GetCountOfState]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_WebEvent_LogEvent' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_WebEvent_LogEvent]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Setup_RestorePermissions' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Setup_RestorePermissions]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Setup_RemoveAllRoleMembers' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Setup_RemoveAllRoleMembers]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Applications_CreateApplication' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Applications_CreateApplication]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Users_DeleteUser' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Users_DeleteUser]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Personalization_GetApplicationId' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Personalization_GetApplicationId]
GO
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_Paths_CreatePath' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_Paths_CreatePath]

/* Note: DNW_* are the aggregate blog procs */
IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'DNW_GetRecentPosts' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[DNW_GetRecentPosts]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'DNW_HomePageData' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[DNW_HomePageData]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'DNW_Stats' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[DNW_Stats]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'DNW_Total_Stats' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[DNW_Total_Stats]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'iter_charlist_to_table' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP FUNCTION [<dbUser,varchar,dbo>].[iter_charlist_to_table]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_CreateHost' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_CreateHost]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_VersionAdd' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_VersionAdd]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_VersionGetCurrent' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_VersionGetCurrent]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetCommentByChecksumHash' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetCommentByChecksumHash]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableBlogs' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetBlogById' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetBlogById]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeleteCategory' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteCategory]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeleteImage' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteImage]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeleteImageCategory' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteImageCategory]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeleteKeyWord' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteKeyWord]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeleteLink' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteLink]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeleteLinksByPostID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteLinksByPostID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeletePost' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeletePost]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeleteFeedbackByStatus' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteFeedbackByStatus]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeleteFeedback' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteFeedback]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetActiveCategoriesWithLinkCollection' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetAllCategories' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetAllCategories]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetBlogKeyWords' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetCategory' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetCategory]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetCategoryByName' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetCategoryByName]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetConditionalEntries' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetConditionalEntries]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetBlog' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetBlog]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetEntriesByDayRange' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetEntriesByDayRange]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetFeedbackCollection' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetFeedbackCollection]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetFeedbackCountsByStatus' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetFeedbackCountsByStatus]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetFeedback' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetFeedback]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetImageCategory' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetImageCategory]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetKeyWord' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetKeyWord]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetLinkCollectionByPostID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetLinkCollectionByPostID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetLinksByActiveCategoryID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetLinksByActiveCategoryID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetLinksByCategoryID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableEntries' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableEntries]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetEntriesForBlogMl' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableEntriesByCategoryID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableFeedback' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableFeedback]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableLogEntries' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableLogEntries]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableKeyWords' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableKeyWords]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableLinks' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableLinks]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPageableReferrers' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPageableReferrers]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByCategoryID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByDayRange' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByDayRange]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByMonth' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByMonth]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByMonthArchive' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByMonthArchive]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByYearArchive' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByYearArchive]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetSingleDay' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetSingleDay]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetSingleEntry' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetSingleEntry]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetSingleImage' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetSingleImage]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetSingleLink' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetSingleLink]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetUrlID' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetUrlID]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertCategory' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertEntry' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertEntry]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertEntryViewCount' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_UpdateBlogStats')
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateFeedback]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateFeedback]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertImage' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertImage]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertKeyWord' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertKeyWord]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertLink' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertLink]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertLinkCategoryList' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertLinkCategoryList]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertFeedback' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertFeedback]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateFeedbackCount' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_UpdateBlogStats')
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateFeedback' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateFeedback]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertReferral' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertReferral]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_StatsSummary' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_StatsSummary]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_TrackEntry' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_TrackEntry]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateCategory' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateCategory]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateConfig' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateConfig]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateConfigUpdateTime' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateEntry' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateEntry]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateImage' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateImage]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateKeyWord' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateKeyWord]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdateLink' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateLink]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_AddLogEntry' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_AddLogEntry]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPostsByTag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPostsByTag]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertEntryTagList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertEntryTagList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetTopTags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetTopTags]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_InsertMetaTag')
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertMetaTag]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_UpdateMetaTag')
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateMetaTag]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_GetMetaTagsForBlog')
drop procedure [<dbUser,varchar,dbo>].[subtext_GetMetaTagsForBlog]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_GetMetaTagsForEntry')
drop procedure [<dbUser,varchar,dbo>].[subtext_GetMetaTagsForEntry]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_DeleteMetaTag')
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteMetaTag]
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 
	and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_ClearBlogContent')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_ClearBlogContent]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_LogClear' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_LogClear]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_SearchEntries' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_SearchEntries]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetRelatedLinks' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetRelatedLinks]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetTop10byBlogId' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPostsByCategoriesArchive' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].subtext_GetPostsByCategoriesArchive
GO

if exists (select ROUTINE_NAME from INFORMATION_SCHEMA.ROUTINES where ROUTINE_TYPE = 'PROCEDURE' and OBJECTPROPERTY(OBJECT_ID(ROUTINE_NAME), 'IsMsShipped') = 0 and ROUTINE_SCHEMA = '<dbUser,varchar,dbo>' AND ROUTINE_NAME = 'subtext_ClearBlogContent')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_ClearBlogContent]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertPluginData' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertPluginData]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_UpdatePluginData' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdatePluginData]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_DeletePluginBlog' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeletePluginBlog]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPluginBlog' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPluginBlog]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_InsertPluginBlog' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertPluginBlog]
GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[ROUTINES] WHERE ROUTINE_NAME = 'subtext_GetPluginData' AND ROUTINE_SCHEMA = '<dbUser,varchar,dbo>')
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPluginData]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogByDomainAlias]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogByDomainAlias]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetPageableDomainAliases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetPageableDomainAliases]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_CreateDomainAlias]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_CreateDomainAlias]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteDomainAlias]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteDomainAlias]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateDomainAlias]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateDomainAlias]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetDomainAliasById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetDomainAliasById]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DeleteBlogGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_DeleteBlogGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetBlogGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetBlogGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_InsertBlogGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_InsertBlogGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_UpdateBlogGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_UpdateBlogGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_ListBlogGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_ListBlogGroups]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[DNW_GetRecentImages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[DNW_GetRecentImages]
GO



SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

--Found at: http://www.algonet.se/~sommar/arrays-in-sql.html
  CREATE FUNCTION [<dbUser,varchar,dbo>].[iter_charlist_to_table]
                    (@list      ntext,
                     @delimiter nchar(1) = N',')
         RETURNS @tbl TABLE (listpos int IDENTITY(1, 1) NOT NULL,
                             str     varchar(4000),
                             nstr    nvarchar(2000)) AS

   BEGIN
      DECLARE @pos      int,
              @textpos  int,
              @chunklen smallint,
              @tmpstr   nvarchar(4000),
              @leftover nvarchar(4000),
              @tmpval   nvarchar(4000)

      SET @textpos = 1 
           SET @leftover = ''
                 WHILE @textpos <= datalength(@list) / 2
                       BEGIN
         SET @chunklen = 4000 - datalength(@leftover) / 2
         SET @tmpstr = @leftover + substring(@list, @textpos, @chunklen)
         SET @textpos = @textpos + @chunklen

         SET @pos = charindex(@delimiter, @tmpstr)

         WHILE @pos > 0
         BEGIN
            SET @tmpval = ltrim(rtrim(left(@tmpstr, @pos - 1)))
            INSERT @tbl (str, nstr) VALUES(@tmpval, @tmpval)
            SET @tmpstr = substring(@tmpstr, @pos + 1, len(@tmpstr))
            SET @pos = charindex(@delimiter, @tmpstr)
         END

         SET @leftover = @tmpstr
      END

      INSERT @tbl(str, nstr) VALUES (ltrim(rtrim(@leftover)), ltrim(rtrim(@leftover)))
   RETURN
   END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/*
Returns a single blog within the subtext_Config table by id.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetBlogById]
(
	@BlogId int
)
AS

SELECT	blog.BlogId 
		, blog.OwnerId
		, blog.Title
		, blog.SubTitle
		, blog.Skin
		, blog.Subfolder
		, blog.Host
		, blog.TimeZone
		, blog.ItemCount
		, blog.[Language]
		, blog.News
		, blog.SecondaryCss
		, blog.LastUpdated
		, blog.PostCount
		, blog.StoryCount
		, blog.PingTrackCount
		, blog.CommentCount
		, blog.Flag
		, blog.SkinCssFile 
		, blog.BlogGroupId
		, blog.LicenseUrl
		, blog.DaysTillCommentsClose
		, blog.CommentDelayInMinutes
		, blog.NumberOfRecentComments
		, blog.RecentCommentsLength
		, blog.AkismetAPIKey
		, blog.FeedBurnerName
		, bgroup.Title AS BlogGroupTitle
FROM [<dbUser,varchar,dbo>].[subtext_Config] blog
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_BlogGroup] bgroup ON
bgroup.Id = blog.BlogGroupId
WHERE	blog.BlogId = @BlogId
GO


GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetBlogById]  TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats]
(
	@BlogId int
)
AS
	-- Update the blog comment count.
	UPDATE [<dbUser,varchar,dbo>].[subtext_Config] 
	SET CommentCount = 
		(
			SELECT COUNT(1) 
			FROM  [<dbUser,varchar,dbo>].[subtext_FeedBack] f WITH (NOLOCK)
			    INNER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c WITH (NOLOCK) ON c.ID = f.EntryId
			WHERE f.BlogId = @BlogId
				AND f.StatusFlag & 1 = 1
				AND f.FeedbackType = 1
				AND c.PostConfig & 1 = 1
		)
	WHERE BlogId = @BlogId
	
	-- Update the blog trackback count.
	UPDATE [<dbUser,varchar,dbo>].[subtext_Config] 
	SET PingTrackCount = 
		(
			SELECT COUNT(1) 
			FROM  [<dbUser,varchar,dbo>].[subtext_FeedBack] f WITH (NOLOCK)
			    INNER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c WITH (NOLOCK) ON c.ID = f.EntryId
			WHERE f.BlogId = @BlogId
				AND f.StatusFlag & 1 = 1
				AND f.FeedbackType = 2
				AND c.PostConfig & 1 = 1
		)
	WHERE BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats] TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats]
(
	@BlogId int
)
AS

    UPDATE [<dbUser,varchar,dbo>].[subtext_Config]  
    SET PostCount = 
	    (
		    SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = @BlogId AND PostType = 1 AND PostConfig & 1 = 1
	    ),
	    StoryCount = 
	    (
	        SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = @BlogId AND PostType = 2 AND PostConfig & 1 = 1
	    )
    WHERE BlogId = @BlogId
    
    EXEC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats] @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount]
(
	@BlogId int
	,@EntryId int
)
AS
	-- Update the entry comment count.
	UPDATE [<dbUser,varchar,dbo>].[subtext_Content] 
	SET [<dbUser,varchar,dbo>].[subtext_Content].FeedbackCount = 
		(
			SELECT COUNT(1) 
			FROM  [<dbUser,varchar,dbo>].[subtext_FeedBack] f  WITH (NOLOCK)
			WHERE f.EntryId = @EntryId 
				AND f.StatusFlag & 1 = 1
		)
	WHERE Id = @EntryId

	-- Update the blog comment count.
	EXEC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackStats] @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteCategory]
(
	@CategoryID int,
	@BlogId int
)
AS
DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE CategoryID = @CategoryID AND BlogId = @BlogId
DELETE [<dbUser,varchar,dbo>].[subtext_LinkCategories] FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories] WHERE subtext_LinkCategories.CategoryID = @CategoryID AND subtext_LinkCategories.BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteImage]
(
	@BlogId int,
	@ImageID int
)
AS
DELETE [<dbUser,varchar,dbo>].[subtext_Images] 
FROM [<dbUser,varchar,dbo>].[subtext_Images] 
WHERE	ImageID = @ImageID 
	AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteImageCategory]
(
	@CategoryID int,
	@BlogId int
)
AS
DELETE [<dbUser,varchar,dbo>].[subtext_Images] FROM [<dbUser,varchar,dbo>].[subtext_Images] WHERE subtext_Images.CategoryID = @CategoryID AND subtext_Images.BlogId = @BlogId
DELETE [<dbUser,varchar,dbo>].[subtext_LinkCategories] FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories] WHERE subtext_LinkCategories.CategoryID = @CategoryID AND subtext_LinkCategories.BlogId = @BlogId



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteImageCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteKeyWord]
(
	@KeyWordID int,
	@BlogId int
)

AS

DELETE FROM [<dbUser,varchar,dbo>].[subtext_KeyWords] WHERE BlogId = @BlogId AND KeyWordID = @KeyWordID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteLink]
(
	@LinkID int,
	@BlogId int
)
AS
DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE [LinkID] = @LinkID AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteLinksByPostID]
(
	@PostID int,
	@BlogId int
)
AS
Set NoCount ON
DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE PostID = @PostID AND BlogId = @BlogId



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteLinksByPostID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Returns a count of feedback for the various statuses.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetFeedbackCountsByStatus]
(
	@BlogId int,
	@ApprovedCount int out,
	@NeedsModerationCount int out,
	@FlaggedSpam int out,
	@Deleted int out	
)
AS

SELECT @ApprovedCount = COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE BlogId = @BlogId AND StatusFlag & 1 = 1
SELECT @NeedsModerationCount = COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE BlogId = @BlogId AND StatusFlag & 2 = 2 AND StatusFlag & 8 != 8 AND StatusFlag & 1 != 1
SELECT @FlaggedSpam = COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE BlogId = @BlogId AND StatusFlag & 4 = 4 AND StatusFlag & 8 != 8 AND StatusFlag & 1 != 1
SELECT @Deleted = COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE BlogId = @BlogId AND StatusFlag & 8 = 8 AND StatusFlag & 1 != 1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetFeedbackCountsByStatus] TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Fully deletes a Feedback item from the db.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteFeedback]
(
	@Id int
)
AS

DECLARE @EntryId int
DECLARE @BlogId int

SELECT @EntryId = EntryId, @BlogId = BlogId FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE [Id] = @Id

DELETE [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE [Id] = @Id

exec [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] @BlogId, @EntryId
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteFeedback] TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Fully deletes a Feedback item from the db.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetHost]
AS
SELECT TOP 1 ApplicationId, OwnerId, DateCreated FROM [<dbUser,varchar,dbo>].[subtext_Host]
GO
GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetHost] TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Fully deletes a Feedback item from the db.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteFeedbackByStatus]
(
	@BlogId int
	, @StatusFlag int
)
AS

DELETE [<dbUser,varchar,dbo>].[subtext_FeedBack] 
WHERE [BlogId] = @BlogId 
	AND StatusFlag & @StatusFlag = @StatusFlag
	AND StatusFlag & 1 != 1 -- Do not delete approved.
	AND (
			(@StatusFlag = 4 AND StatusFlag & 8 != 8)
			OR
			@StatusFlag != 4
		)	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeleteFeedbackByStatus] TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Deletes a record FROM [<dbUser,varchar,dbo>].[subtext_Content], whether it be a post, a comment, etc..
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeletePost]
(
	@ID int
)
AS

DECLARE @blogId int
SET @blogId = (select BlogId from [<dbUser,varchar,dbo>].[subtext_Content] where [ID] = @ID)

DELETE FROM [<dbUser,varchar,dbo>].[subtext_EntryTag] WHERE EntryId = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE PostID = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_EntryViewCount] WHERE EntryID = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Referrals] WHERE EntryID = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE EntryId = @ID
DELETE FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE [ID] = @ID

EXEC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] @blogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_DeletePost]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]
(
	@BlogId int = NULL
)
AS
SELECT CategoryID
	, Title
	, Active
	, CategoryType
	, [Description]
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories]
WHERE	
		Active= 1 
	AND	(BlogId = @BlogId OR @BlogId IS NULL)
	AND	CategoryType = 5
ORDER BY 
	Title;

SELECT l.LinkID
	, l.Title
	, l.Url
	, l.Rss
	, l.Active
	, l.NewWindow
	, l.CategoryID
	, l.PostID
FROM [<dbUser,varchar,dbo>].[subtext_Links] l
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID
WHERE 
		l.Active = 1 
	AND c.Active = 1
	AND (c.BlogId = @BlogId OR @BlogId IS NULL)
	AND l.BlogId = @BlogId 
	AND c.CategoryType = 5
ORDER BY 
	l.Title



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetActiveCategoriesWithLinkCollection]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetCategory]
(
	@CategoryName nvarchar(150) = NULL
	, @CategoryID int = NULL
	, @IsActive bit
	, @BlogId int = NULL
	, @CategoryType tinyint = NULL
)
AS
SELECT	c.CategoryID
		, c.Title
		, c.Active
		, c.CategoryType
		, c.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories] c
WHERE (c.CategoryID = @CategoryID OR @CategoryID IS NULL)
	AND (c.Title = @CategoryName OR @CategoryName IS NULL)
	AND (c.CategoryType = @CategoryType OR @CategoryType IS NULL)
	AND (c.BlogId = @BlogId OR @BlogId IS NULL)
	AND c.Active <> CASE @IsActive WHEN 0 THEN -1 else 0 END
	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetConditionalEntries]
(
	@ItemCount int 
	, @PostType int
	, @PostConfig int
	, @BlogId int = NULL
	, @IncludeCategories bit = 0
)
AS
/* 
//TODO: This proc is being used to populate home page 
and feed. But it should sort on different dates for each.
*/
CREATE Table #IDs  
(  
	 TempId int IDENTITY (0, 1) NOT NULL,  
	 Id int not NULL  
)
-- Create a temp table to store the Post IDs 
-- of the posts we're interested in.
SET ROWCOUNT @ItemCount

INSERT #IDs (Id)  
SELECT [Id]   
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	PostType = @PostType 
	AND BlogId = COALESCE(@BlogId, BlogId)
	AND PostConfig & @PostConfig = @PostConfig
ORDER BY ISNULL([DateSyndicated], [DateAdded]) DESC

SET ROWCOUNT 0

-- Now select the content etc... for the posts 
-- in the temp table.
SELECT BlogId
	, AuthorId
	, [<dbUser,varchar,dbo>].[subtext_Content].[Id]
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, PostType
	, DateUpdated
	, FeedbackCount = ISNULL(FeedbackCount, 0)
	, PostConfig
	, EntryName 
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
	INNER JOIN #IDs ON #IDs.[Id] = [<dbUser,varchar,dbo>].[subtext_Content].[Id]
ORDER BY #IDs.TempId

IF @IncludeCategories = 1
BEGIN
	-- Select the category title and the associated post id
	SELECT	c.Title  
			, p.[Id]
	FROM [<dbUser,varchar,dbo>].[subtext_Links] l
		INNER JOIN #IDs p ON l.[PostID] = p.[ID]  
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID
	ORDER BY p.[TempID] DESC
END
DROP TABLE #IDs

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetConditionalEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
/*
Returns the blog that matches the given host/application combination.

@Strict -- If 0, then we return the one and only blog if there's one and only blog.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetBlog]
(
	@Host nvarchar(100)
	, @Subfolder nvarchar(50)
	, @Strict bit = 1 
)
AS

DECLARE @BlogId INT

SELECT
	@BlogID = BlogId
FROM [<dbUser,varchar,dbo>].[subtext_Config]
WHERE
	(
		-- try to find an exact match.
			Host = @Host
		AND Subfolder = @Subfolder
	)
	OR
	(
		-- we couldn't find an exact match, so next see if there is only one blog in the system 
		-- with the given Host name, and return it.
			(@Strict = 0) 
		AND (1 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_config] WHERE Host = @Host))
		AND Host = @Host
	)
	OR
	(
		-- we couldn't find an exact match, nor a match for the HostName, so next see is 
		-- only ONE blog in the system, and we haven't found a more exact match, return the blog.
			(@Strict = 0) 
		AND (1 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].[subtext_config]))
	)

EXEC [<dbUser,varchar,dbo>].[subtext_GetBlogById] @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetBlog]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetEntriesByDayRange]
(
	@StartDate datetime,
	@StopDate datetime,
	@PostType int,
	@IsActive bit,
	@BlogId int
)
AS
SELECT	BlogId
	, [ID]
	, AuthorId
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, PostType
	, DateUpdated
	, FeedbackCount = ISNULL(FeedbackCount, 0)
	, PostConfig
	, EntryName 
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE 
	(
		DateSyndicated > @StartDate 
		AND DateSyndicated < DateAdd(day, 1, @StopDate)
	)
	AND PostType=@PostType 
	AND BlogId = @BlogId 
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY DateSyndicated DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetEntriesByDayRange]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/* Gets all the ACTIVE Feedback (comments, pingbacks/trackbacks) for the entry */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetFeedbackCollection]
(
	@EntryId int
)
AS
	SELECT f.Id 
		, f.Title
		, f.Body
		, f.BlogId
		, f.EntryId
		, f.Author
		, f.IsBlogAuthor
		, f.Email
		, f.Url
		, f.FeedbackType
		, f.StatusFlag
		, f.CommentAPI
		, f.Referrer
		, f.IpAddress
		, f.UserAgent
		, f.FeedbackChecksumHash
		, f.DateCreated
		, f.DateModified
		, ParentEntryCreateDate = c.DateAdded
		, ParentEntryName = c.EntryName
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c 
		ON c.Id = f.EntryId
WHERE f.EntryId = @EntryId
	AND f.StatusFlag & 1 = 1
ORDER BY f.[Id]


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetFeedbackCollection]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/* Returns a single Feedback by id */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetFeedback]
(
	@Id int
)
AS
	SELECT f.Id 
		, f.Title
		, f.Body
		, f.BlogId
		, f.EntryId
		, f.Author
		, f.IsBlogAuthor
		, f.Email
		, f.Url
		, f.FeedbackType
		, f.StatusFlag
		, f.CommentAPI
		, f.Referrer
		, f.IpAddress
		, f.UserAgent
		, f.FeedbackChecksumHash
		, f.DateCreated
		, f.DateModified
		, ParentEntryCreateDate = c.DateAdded
		, ParentEntryName = c.EntryName
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c 
		ON c.Id = f.EntryId
WHERE f.Id = @Id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetFeedback] TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetImageCategory]
(
	@CategoryID int
	, @IsActive bit
	, @BlogId int
)
AS
EXEC [<dbUser,varchar,dbo>].[subtext_GetCategory] @CategoryID=@CategoryID, @IsActive=@IsActive, @BlogId=@BlogId


SELECT	Title
		, CategoryID
		, Height
		, Width
		, [File]
		, Active
		, ImageID 
FROM [<dbUser,varchar,dbo>].[subtext_Images]  
WHERE CategoryID = @CategoryID 
	AND BlogId = @BlogId 
	AND Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY Title, ImageID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetImageCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetKeyWord]
(
	@KeyWordID int
	, @BlogId int
)
AS

SELECT 
	KeyWordID, Word,Rel,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogId
FROM
	subtext_keywords
WHERE 
	KeyWordID = @KeyWordID AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetLinkCollectionByPostID]
(
	@PostID int,
	@BlogId int
)
AS

IF @PostID = -1
	SET @PostID = NULL

SELECT	LinkID
	, Title
	, Url
	, Rss
	, Active
	, CategoryID
	, PostID = ISNULL(PostID, -1)
	, NewWindow 
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE PostID = @PostID 
	AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetLinkCollectionByPostID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]
(
	@CategoryID int
	, @BlogId int
)
AS
EXEC [<dbUser,varchar,dbo>].[subtext_GetCategory] @CategoryID, @BlogId
SELECT	LinkID
		, Title
		, Url
		, Rss
		, Active
		, NewWindow
		, CategoryID
		, PostId = ISNULL(PostID, -1)
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE	CategoryID = @CategoryID 
	AND BlogId = @BlogId
ORDER BY Title


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetLinksByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Selects a page of blog posts within the admin section.
Updated this to use a more efficient paging technique:
http://www.4guysfromrolla.com/webtech/041206-1.shtml
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableEntries]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
	, @PostType int
)
AS

DECLARE @FirstDate datetime
DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstDate = DateAdded 
	, @FirstId = ID
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	BlogId = @BlogId 
	AND PostType = @PostType 
ORDER BY DateAdded DESC, ID DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	c.BlogId 
		, c.[ID] 
		, c.AuthorId 
		, c.Title 
		, c.DateAdded 
		, c.[Text] 
		, c.[Description]
		, c.PostType 
		, c.DateUpdated 
		, FeedbackCount = ISNULL(c.FeedbackCount, 0)
		, c.PostConfig
		, c.EntryName
		, c.DateSyndicated
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] c
	LEFT JOIN  [<dbUser,varchar,dbo>].subtext_EntryViewCount vc ON (c.[ID] = vc.EntryID AND vc.BlogId = @BlogId)
WHERE 	c.BlogId = @BlogId 
	AND c.DateAdded <= @FirstDate
	AND c.ID <= @FirstId
	AND PostType = @PostType
ORDER BY c.DateAdded DESC
 
SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE 	BlogId = @BlogId 
	AND PostType = @PostType 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableEntries]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Selects a page of blog posts within the admin section, when a category 
is selected.
Updated this to use a more efficient paging technique:
http://www.4guysfromrolla.com/webtech/041206-1.shtml
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]
(
	@BlogId int
	, @CategoryID int
	, @PageIndex int
	, @PageSize int
	, @PostType int
)
AS

DECLARE @FirstDate datetime
DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstDate = c.DateAdded 
	, @FirstId = ID
FROM [<dbUser,varchar,dbo>].[subtext_Content] c
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] links ON c.[ID] = ISNULL(links.PostID, -1)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] cats ON (links.CategoryID = cats.CategoryID)
WHERE	c.BlogId = @BlogId 
	AND c.PostType = @PostType 
	AND cats.CategoryID = @CategoryID
ORDER BY c.DateAdded DESC, c.ID DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	c.BlogId 
		, c.[ID] 
		, c.AuthorId
		, c.Title 
		, c.DateAdded 
		, c.[Text] 
		, c.[Description]
		, c.PostType 
		, c.DateUpdated 
		, FeedbackCount = ISNULL(c.FeedbackCount, 0)
		, c.PostConfig
		, c.EntryName
		, c.DateSyndicated
		, vc.WebCount
		, vc.AggCount
		, vc.WebLastUpdated
		, vc.AggLastUpdated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] c
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] l ON c.[ID] = ISNULL(l.PostID, -1)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] cats ON (l.CategoryID = cats.CategoryID)
	Left JOIN  subtext_EntryViewCount vc ON (c.[ID] = vc.EntryID AND vc.BlogId = @BlogId)
WHERE 	c.BlogId = @BlogId 
	AND c.DateAdded <= @FirstDate
	AND c.ID <= @FirstId
	AND c.PostType = @PostType
	AND cats.CategoryID = @CategoryID
ORDER BY c.DateAdded DESC, c.ID DESC
 
SELECT COUNT(c.[ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Content] c
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] links ON c.[ID] = ISNULL(links.PostID, -1)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] cats ON (links.CategoryID = cats.CategoryID)
WHERE 	c.BlogId = @BlogId 
	AND c.PostType = @PostType 
	AND cats.CategoryID = @CategoryID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableEntriesByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
For the admin section. Gets a page of Feedback for the specified blog.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableFeedback]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
	, @StatusFlag int
	, @ExcludeFeedbackStatusMask int = NULL
	, @FeedbackType int = NULL -- Null for all feedback.
)
AS

IF @ExcludeFeedbackStatusMask IS NULL
	SET @ExcludeFeedbackStatusMask = ~0

DECLARE @FirstDate datetime
DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT @FirstDate = DateCreated,
	@FirstId = f.Id
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
WHERE 	f.BlogId = @BlogId 
	AND (f.StatusFlag & @StatusFlag = @StatusFlag)
	AND (f.StatusFlag & @ExcludeFeedbackStatusMask = 0) -- Make sure the status doesn't have any of the excluded statuses set
	AND (f.FeedbackType = @FeedbackType OR @FeedbackType IS NULL)
ORDER BY DateCreated DESC, f.Id DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT  f.Id
		, f.Title
		, f.Body
		, f.BlogId
		, f.EntryId
		, f.Author
		, f.IsBlogAuthor
		, f.Email
		, f.Url
		, f.FeedbackType
		, f.StatusFlag
		, f.CommentAPI
		, f.Referrer
		, f.IpAddress
		, f.UserAgent
		, f.FeedbackChecksumHash
		, f.DateCreated
		, f.DateModified
		, ParentEntryCreateDate = c.DateAdded
		, ParentEntryName = c.EntryName
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c 
		ON c.Id = f.EntryId
WHERE 	f.BlogId = @BlogId 
	AND f.DateCreated <= @FirstDate
	AND f.Id <= @FirstId
	AND f.StatusFlag & @StatusFlag = @StatusFlag
	AND (f.StatusFlag & @ExcludeFeedbackStatusMask = 0) -- Make sure the status doesn't have any of the excluded statuses set
	AND (f.FeedbackType = @FeedbackType OR @FeedbackType IS NULL)
ORDER BY DateCreated DESC
 
SELECT COUNT(f.[Id]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
WHERE 	f.BlogId = @BlogId 
	AND f.StatusFlag & @StatusFlag = @StatusFlag
	AND (f.StatusFlag & @ExcludeFeedbackStatusMask = 0) -- Make sure the status doesn't have any of the excluded statuses set
	AND (f.FeedbackType = @FeedbackType OR @FeedbackType IS NULL)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableFeedback]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
Selects a page of log posts within the admin section.
Updated this to use a more efficient paging technique:
http://www.4guysfromrolla.com/webtech/041206-1.shtml
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableLogEntries]
(
	@BlogId int = NULL
	, @PageIndex int
	, @PageSize int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = [ID] FROM [<dbUser,varchar,dbo>].[subtext_Log] 
WHERE	BlogId = @BlogId OR @BlogId IS NULL
ORDER BY [ID] DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	[log].[Id]
		, [log].[BlogId]
		, [log].[Date]
		, [log].[Thread]
		, [log].[Context]
		, [log].[Level]
		, [log].[Logger]
		, [log].[Message]
		, [log].[Exception]
		, [log].[Url]
FROM [<dbUser,varchar,dbo>].[subtext_Log] [log]
WHERE 	([log].BlogId = @BlogId or @BlogId IS NULL)
	AND [log].[ID] <= @FirstId
ORDER BY [log].[ID] DESC
 
SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Log] 
WHERE 	BlogId = @BlogId 
	OR 	@BlogId IS NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableLogEntries]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Selects a page of keywords within the admin section.
Updated this to use a more efficient paging technique:
http://www.4guysfromrolla.com/webtech/041206-1.shtml
*/

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableKeyWords]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
)
AS
DECLARE @FirstWord nvarchar(100)
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstWord = [Word] FROM [<dbUser,varchar,dbo>].[subtext_KeyWords]
WHERE	BlogId = @BlogId 
ORDER BY [Word] ASC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT 	words.KeyWordID
		, words.Word
		, words.Rel
		, words.[Text]
		, words.ReplaceFirstTimeOnly
		, words.OpenInNewWindow
		, words.CaseSensitive
		, words.Url
		, words.Title
		, words.BlogId
FROM 	
	[<dbUser,varchar,dbo>].[subtext_KeyWords] words
WHERE 	
		words.BlogId = @BlogId 
	AND words.Word >= @FirstWord
ORDER BY
		words.Word ASC
 
SELECT 	COUNT([KeywordId]) AS 'TotalRecords'
FROM [<dbUser,varchar,dbo>].[subtext_KeyWords] 
WHERE 	BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableKeyWords]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Returns a page of links for the admin section */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableLinks]
(
	@BlogId int
	, @CategoryId int = NULL
	, @PageIndex int
	, @PageSize int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT @FirstId = LinkID
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE 	BlogId = @BlogId 
	AND (CategoryID = @CategoryID OR @CategoryId IS NULL)
	AND PostID IS NULL
ORDER BY [LinkID] DESC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT links.LinkID 
	, links.Title 
	, links.Url
	, links.Rss 
	, links.Active 
	, links.NewWindow 
	, links.CategoryID
	, PostID = ISNULL(links.PostID, -1)
FROM [<dbUser,varchar,dbo>].[subtext_Links] links
WHERE 	links.BlogId = @BlogId 
	AND links.[LinkId] <= @FirstId
	AND (CategoryID = @CategoryId OR @CategoryId IS NULL)
	AND PostID IS NULL
ORDER BY links.[LinkID] DESC
 
SELECT COUNT([LinkID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Links] 
WHERE 	BlogId = @BlogId 
	AND (CategoryID = @CategoryId OR @CategoryId IS NULL)
	AND PostID IS NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableLinks]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]
(
	@BlogId int
	, @CategoryID int = NULL
	, @PageIndex int
	, @PageSize int
	, @SortDesc bit
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

SET @PageLowerBound = @PageSize * @PageIndex - @PageSize
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

CREATE TABLE #TempPagedLinkIDs 
(
	TempID int IDENTITY (1, 1) NOT NULL,
	LinkID int NOT NULL
)	

IF NOT (@SortDesc = 1)
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM [<dbUser,varchar,dbo>].[subtext_Links] 
	WHERE 	BlogId = @BlogId 
		AND CategoryID = @CategoryID
		AND PostID IS NULL
	ORDER BY Title
END
ELSE
BEGIN
	INSERT INTO #TempPagedLinkIDs (LinkID)
	SELECT	LinkID
	FROM [<dbUser,varchar,dbo>].[subtext_Links]
	WHERE 	BlogId = @BlogId 
		AND CategoryID = @CategoryID
		AND PostID IS NULL
	ORDER BY Title DESC
END

SELECT 	links.LinkID
		, links.Title
		, links.Url
		, links.Rss 
		, links.Active 
		, links.NewWindow 
		, links.CategoryID  
		, PostId = ISNULL(links.PostID, -1)
FROM 	
	subtext_Links links
	INNER JOIN #TempPagedLinkIDs tmp ON (links.LinkID = tmp.LinkID)
WHERE 	
		links.BlogId = @BlogId 
	AND links.CategoryID = @CategoryID
	AND tmp.TempID > @PageLowerBound
	AND tmp.TempID < @PageUpperBound
ORDER BY
	tmp.TempID
 
DROP TABLE #TempPagedLinkIDs


SELECT  COUNT([LinkID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Links] 
WHERE 	BlogId = @BlogId 
	AND CategoryID = @CategoryID 
	AND PostID IS NULL


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableLinksByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableReferrers] 
(
	@BlogId INT,
	@EntryID int = NULL,
	@PageIndex INT,
	@PageSize INT
)
AS

DECLARE @FirstDate DateTime
DECLARE @FirstEntryId int
DECLARE @FirstUrlId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
SELECT	@FirstDate = [LastUpdated] 
	, @FirstEntryId = [EntryID]
	, @FirstUrlId = [UrlID]
FROM [<dbUser,varchar,dbo>].[subtext_Referrals]
WHERE	BlogId = @BlogId 
	AND (EntryID = @EntryID OR @EntryID IS NULL)
ORDER BY [LastUpdated] DESC, [EntryID] DESC, UrlID DESC

SET ROWCOUNT @PageSize

SELECT	
	u.URL
	, c.Title
	, c.EntryName
	, r.[EntryId]
	, [Count]
	, r.LastUpdated
FROM [<dbUser,varchar,dbo>].[subtext_Referrals] r
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_URLs] u ON u.UrlID = r.UrlID
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c ON c.ID = r.EntryID
WHERE 
		r.LastUpdated <= @FirstDate
	AND r.EntryID <= @FirstEntryId
	AND r.UrlID <= @FirstUrlId
	AND (r.EntryID = @EntryID OR @EntryID IS NULL)
	AND r.BlogId = @BlogId
ORDER BY r.[LastUpdated] DESC

SELECT COUNT([UrlID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Referrals] 
WHERE 	BlogId = @BlogId 
	AND (EntryID = @EntryID OR @EntryID IS NULL)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableReferrers]  TO [public]
GO



SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryID]
(
	@ItemCount int
	, @CategoryID int
	, @IsActive bit
	, @BlogId int
)
AS
SET ROWCOUNT @ItemCount
SELECT	content.BlogId
	, content.[ID]
	, content.AuthorId
	, content.Title
	, content.DateAdded
	, content.[Text]
	, content.[Description]
	, content.PostType
	, content.DateUpdated
	, FeedbackCount = ISNULL(content.FeedbackCount, 0)
	, content.PostConfig
	, content.EntryName 
	, content.DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content] content WITH (NOLOCK)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] links WITH (NOLOCK) ON content.ID = ISNULL(links.PostID, -1)
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] categories WITH (NOLOCK) ON links.CategoryID = categories.CategoryID
WHERE  content.BlogId = @BlogId 
	AND content.PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END AND categories.CategoryID = @CategoryID
ORDER BY content.DateSyndicated DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoryID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByDayRange]
(
	@StartDate datetime,
	@StopDate datetime,
	@BlogId int
)
AS
SELECT	BlogId
		, [ID]
		, AuthorId
		, Title
		, DateAdded
		, [Text]
		, [Description]
		, PostType
		, DateUpdated
		, FeedbackCount = ISNULL(FeedbackCount, 0)
		, PostConfig
		, EntryName 
		, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE 
	(
			DateSyndicated > @StartDate 
		AND DateSyndicated < DateAdd(day,1,@StopDate)
	)
	AND PostType=1 
	AND BlogId = @BlogId
ORDER BY DateSyndicated DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByDayRange]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByMonth]
(
	@Month int
	, @Year int
	, @BlogId int = NULL
)
AS
SELECT	BlogId
	, [ID]
	, AuthorId
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, PostType
	, DateUpdated
	, FeedbackCount = ISNULL(FeedbackCount, 0)
	, PostConfig
	, EntryName 
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	PostType=1 
	AND (BlogId = @BlogId OR @BlogId IS NULL)
	AND PostConfig & 1 = 1 
	AND Month(DateSyndicated) = @Month 
	AND Year(DateSyndicated)  = @Year
ORDER BY DateSyndicated DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByMonth]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByMonthArchive]
(
	@BlogId int = NULL
)
AS
SELECT Month(DateSyndicated) AS [Month]
	, Year(DateSyndicated) AS [Year]
	, 1 AS Day, Count(*) AS [Count] 
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE PostType = 1 AND PostConfig & 1 = 1 AND (BlogId = @BlogId OR @BlogId IS NULL)
GROUP BY Year(DateSyndicated), Month(DateSyndicated) ORDER BY [Year] DESC, [Month] DESC



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByMonthArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByYearArchive] 
(
	@BlogId int
)
AS
SELECT 1 AS [Month], Year(DateSyndicated) AS [Year], 1 AS Day, Count(*) AS [Count] FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE PostType = 1 AND PostConfig & 1 = 1 AND BlogId = @BlogId 
GROUP BY Year(DateSyndicated) ORDER BY [Year] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByYearArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleDay]
(
	@Date datetime
	,@BlogId int
)
AS
SELECT	BlogId
	, [ID]
	, AuthorId
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, PostType
	, DateUpdated
	, FeedbackCount = ISNULL(FeedbackCount, 0)
	, PostConfig
	, EntryName 
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE Year(DateSyndicated) = Year(@Date) 
	AND Month(DateSyndicated) = Month(@Date)
    AND Day(DateSyndicated) = Day(@Date) 
    And PostType=1
    AND BlogId = @BlogId 
    AND PostConfig & 1 = 1 
ORDER BY DateSyndicated DESC;


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetSingleDay]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleEntry]
(
	@ID int = NULL
	, @EntryName nvarchar(150) = NULL
	, @IsActive bit
	, @BlogId int = NULL
	, @IncludeCategories bit = 0
)
AS
SELECT	BlogId
	, [ID]
	, AuthorId
	, Title
	, DateAdded
	, [Text]
	, [Description]
	, PostType
	, DateUpdated
	, FeedbackCount = ISNULL(FeedbackCount, 0)
	, PostConfig
	, EntryName 
	, DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE ID = COALESCE(@ID, ID)
	AND (EntryName = @EntryName OR @EntryName IS NULL) 
	AND (BlogId = @BlogId OR  @BlogId IS NULL)
	AND PostConfig & 1 <> CASE @IsActive WHEN 1 THEN 0 Else -1 END
ORDER BY [ID] DESC

IF @IncludeCategories = 1
BEGIN
	SELECT c.Title
		, PostID = l.PostID  
	FROM [<dbUser,varchar,dbo>].[subtext_Links] l  
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID  
	WHERE l.PostID = @Id
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetSingleEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleImage]
(
	@ImageID int
	, @IsActive bit
	, @BlogId int
)
AS
SELECT Title
	, CategoryID
	, Height
	, Width
	, [File]
	, Active
	, ImageID 
FROM [<dbUser,varchar,dbo>].[subtext_Images]  
WHERE ImageID = @ImageID 
	AND BlogId = @BlogId 
	AND  Active <> CASE @IsActive WHEN 1 THEN 0 Else -1 END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetSingleImage]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetSingleLink]
(
	@LinkID int
	, @BlogId int
)
AS
SELECT	LinkID
		, Title
		, Url
		, Rss
		, Active
		, NewWindow
		, CategoryID
		, PostId
FROM [<dbUser,varchar,dbo>].[subtext_Links]
WHERE LinkID = @LinkID AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetSingleLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetUrlID]
(
	@Url nvarchar(255)
	, @UrlID int OUTPUT
)
AS
IF EXISTS(SELECT UrlID FROM [<dbUser,varchar,dbo>].[subtext_Urls] WHERE Url = @Url AND Url != '')
BEGIN
	SELECT @UrlID = UrlID FROM [<dbUser,varchar,dbo>].[subtext_Urls] WHERE Url = @Url
END
Else
BEGIN
	IF(@Url != '' AND NOT @Url IS NULL)
		INSERT subtext_Urls VALUES (@Url)
		SELECT @UrlID = SCOPE_IDENTITY()
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetUrlID]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertCategory]
(
	@Title nvarchar(150)
	, @Active bit
	, @BlogId int
	, @CategoryType tinyint
	, @Description nvarchar(1000)
	, @CategoryID int OUTPUT
)
AS
Set NoCount ON
INSERT INTO subtext_LinkCategories 
( 
	Title
	, Active
	, CategoryType
	, [Description]
	, BlogId )
VALUES 
(
	@Title
	, @Active
	, @CategoryType
	, @Description
	, @BlogId
)
SELECT @CategoryID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount]-- 1, 0, 1
(
	@EntryID int,
	@BlogId int,
	@IsWeb bit
)

AS

BEGIN
	--Do we have an existing entry in the subtext_InsertEntryViewCount table?
	IF EXISTS(SELECT EntryID FROM [<dbUser,varchar,dbo>].[subtext_EntryViewCount] WHERE EntryID = @EntryID AND BlogId = @BlogId)
	BEGIN
		if(@IsWeb = 1) -- Is this a web view?
		BEGIN
			UPDATE [<dbUser,varchar,dbo>].[subtext_EntryViewCount]
			Set [WebCount] = [WebCount] + 1
				, WebLastUpdated = getdate()
			WHERE EntryID = @EntryID 
				AND BlogId = @BlogId
		END
		ELSE
		BEGIN
			UPDATE [<dbUser,varchar,dbo>].[subtext_EntryViewCount]
			Set [AggCount] = [AggCount] + 1
				, AggLastUpdated = getdate()
			WHERE EntryID = @EntryID 
				AND BlogId = @BlogId
		END
	END
	else
	BEGIN
		if(@IsWeb = 1) -- Is this a web view
		BEGIN
			INSERT subtext_EntryViewCount (EntryID, BlogId, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
			VALUES(@EntryID, @BlogId, 1, 0, getdate(), NULL)
		END
		else
		BEGIN
			Insert subtext_EntryViewCount (EntryID, BlogId, WebCount, AggCount, WebLastUpdated, AggLastUpdated)
		       values (@EntryID, @BlogId, 0, 1, NULL, getdate())
		END
	END


END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertImage]
(
	@Title nvarchar(250),
	@CategoryID int,
	@Width int,
	@Height int,
	@File nvarchar(50),
	@Active bit,
	@BlogId int,
	@ImageID int OUTPUT
)
AS
Insert subtext_Images
(
	Title, CategoryID, Width, Height, [File], Active, BlogId
)
Values
(
	@Title, @CategoryID, @Width, @Height, @File, @Active, @BlogId
)
Set @ImageID = SCOPE_IDENTITY()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertKeyWord]
(
	@Word nvarchar(100),
	@Rel nvarchar(100) = NULL,
	@Text nvarchar(100),
	@ReplaceFirstTimeOnly bit,
	@OpenInNewWindow bit,
	@CaseSensitive bit,
	@Url nvarchar(255),
	@Title nvarchar(100),
	@BlogId int,
	@KeyWordID int OUTPUT
)

AS

Insert [<dbUser,varchar,dbo>].[subtext_KeyWords]
	(Word,Rel,[Text],ReplaceFirstTimeOnly,OpenInNewWindow, CaseSensitive,Url,Title,BlogId)
Values
	(@Word,@Rel,@Text,@ReplaceFirstTimeOnly,@OpenInNewWindow, @CaseSensitive,@Url,@Title,@BlogId)

SELECT @KeyWordID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertLink]
(
	@Title nvarchar(150),
	@Url nvarchar(255),
	@Rss nvarchar(255),
	@Active bit,
	@NewWindow bit,
	@CategoryID int,
	@PostID int = NULL,
	@BlogId int,
	@LinkID int OUTPUT
)
AS

IF @PostID = -1
	SET @PostID = NULL

INSERT INTO subtext_Links 
( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogId )
VALUES 
(@Title, @Url, @Rss, @Active, @NewWindow, @PostID, @CategoryID, @BlogId);
SELECT @LinkID = SCOPE_IDENTITY()


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertLinkCategoryList]
(
	@CategoryList nvarchar(4000)
	, @PostID int
	, @BlogId int
)
AS

IF @PostID = -1
	SET @PostID = NULL

IF @CategoryList = ''
BEGIN
	DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links]
	WHERE 
		BlogId = @BlogId AND (PostID = @PostID)
END
ELSE
BEGIN

	--DELETE categories that have been removed
	DELETE [<dbUser,varchar,dbo>].[subtext_Links] FROM [<dbUser,varchar,dbo>].[subtext_Links]
	WHERE 
		CategoryID not in (SELECT str FROM iter_charlist_to_table(@CategoryList,','))
	And 
		BlogId = @BlogId AND (PostID = @PostID)

	--Add updated/new categories
	INSERT INTO subtext_Links ( Title, Url, Rss, Active, NewWindow, PostID, CategoryID, BlogId )
	SELECT NULL, NULL, NULL, 1, 0, @PostID, Convert(int, [str]), @BlogId
	FROM iter_charlist_to_table(@CategoryList,',')
	WHERE 
		Convert(int, [str]) not in (SELECT CategoryID FROM [<dbUser,varchar,dbo>].[subtext_Links] WHERE PostID = @PostID AND BlogId = @BlogId)
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertLinkCategoryList]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertReferral]
(
	@EntryID int,
	@BlogId int,
	@Url nvarchar(255)
)
AS

DECLARE @UrlID int

if(@Url is not NULL)
BEGIN
	EXEC [<dbUser,varchar,dbo>].[subtext_GetUrlID] @Url, @UrlID = @UrlID OUTPUT
END

if(@UrlID is not NULL)
BEGIN

	IF EXISTS(SELECT EntryID FROM [<dbUser,varchar,dbo>].[subtext_Referrals] WHERE EntryID = @EntryID AND BlogId = @BlogId AND UrlID = @UrlID)
	BEGIN
		UPDATE [<dbUser,varchar,dbo>].[subtext_Referrals]
		Set [Count] = [Count] + 1, LastUpdated = getdate()
		WHERE EntryID = @EntryID AND BlogId = @BlogId AND UrlID = @UrlID
	END
	else
	BEGIN
		Insert [<dbUser,varchar,dbo>].[subtext_Referrals] (EntryID, BlogId, UrlID, [Count], LastUpdated)
		       values (@EntryID, @BlogId, @UrlID, 1, getdate())
	END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertReferral]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertViewStats]
(
	@BlogId int,
	@PageType tinyint,
	@PostID int,
	@Day datetime,
	@Url nvarchar(255)
)
AS

DECLARE @UrlID int

if(@Url is not NULL)
BEGIN
	EXEC [<dbUser,varchar,dbo>].[subtext_GetUrlID] @Url, @UrlID = @UrlID OUTPUT
END
if(@UrlID is NULL)
	set @UrlID = NULL


IF EXISTS (SELECT BlogId FROM [<dbUser,varchar,dbo>].[subtext_ViewStats] WHERE BlogId = @BlogId AND PageType = @PageType AND PostID = @PostID AND [Day] = @Day AND UrlID = @UrlID AND NOT @UrlID IS NULL)
BEGIN
	UPDATE [<dbUser,varchar,dbo>].[subtext_ViewStats]
	Set [Count] = [Count] + 1
	WHERE BlogId = @BlogId AND PageType = @PageType AND PostID = @PostID AND [Day] = @Day AND UrlID = @UrlID
END
Else
BEGIN
	Insert subtext_ViewStats (BlogId, PageType, PostID, [Day], UrlID, [Count])
	Values (@BlogId, @PageType, @PostID, @Day, @UrlID, 1)
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertViewStats]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_StatsSummary]
(
	@BlogId int
)
AS
DECLARE @ReferralCount int
DECLARE @WebCount int
DECLARE @AggCount int

SELECT @ReferralCount = Sum([Count]) FROM [<dbUser,varchar,dbo>].[subtext_Referrals] WHERE BlogId = @BlogId

SELECT @WebCount = Sum(WebCount), @AggCount = Sum(AggCount) FROM [<dbUser,varchar,dbo>].[subtext_EntryViewCount] WHERE BlogId = @BlogId

SELECT @ReferralCount AS 'ReferralCount', @WebCount AS 'WebCount', @AggCount AS 'AggCount'


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_StatsSummary]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_TrackEntry]
(
	@EntryID int,
	@BlogId int,
	@Url nvarchar(255) = NULL,
	@IsWeb bit
)

AS

if(@Url is not NULL AND @IsWeb = 1)
BEGIN
	EXEC [<dbUser,varchar,dbo>].[subtext_InsertReferral] @EntryID, @BlogId, @Url
END

EXEC [<dbUser,varchar,dbo>].[subtext_InsertEntryViewCount] @EntryID, @BlogId, @IsWeb





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_TrackEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateCategory]
(
	@CategoryID int,
	@Title nvarchar(150),
	@Active bit,
	@CategoryType tinyint,
	@Description nvarchar(1000),
	@BlogId int
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_LinkCategories] 
SET 
	[Title] = @Title, 
	[Active] = @Active,
	[CategoryType] = @CategoryType,
	[Description] = @Description
WHERE   
		[CategoryID] = @CategoryID 
	AND [BlogId] = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateCategory]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateConfig]
(
	@OwnerId uniqueidentifier
	, @Title nvarchar(100)
	, @SubTitle nvarchar(250)
	, @Skin nvarchar(50)
	, @Subfolder nvarchar(50)
	, @Host nvarchar(100)
	, @Language nvarchar(10)
	, @TimeZone int = NULL
	, @ItemCount int
	, @CategoryListPostCount int
	, @News nText = NULL
	, @CustomMetaTags nText = NULL
	, @TrackingCode nText = NULL
	, @LastUpdated datetime = NULL
	, @SecondaryCss nText = NULL
	, @SkinCssFile varchar(100) = NULL
	, @Flag int = NULL
	, @BlogId int
	, @LicenseUrl nvarchar(64) = NULL
	, @DaysTillCommentsClose int = NULL
	, @CommentDelayInMinutes int = NULL
	, @NumberOfRecentComments int = NULL
	, @RecentCommentsLength int = NULL
	, @AkismetAPIKey varchar(16) = NULL
	, @FeedBurnerName nvarchar(64) = NULL
	, @BlogGroupId int
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Config]
Set
	OwnerId = @OwnerId      
	, Title	   =   @Title        
	, SubTitle   =   @SubTitle     
	, Skin	  =    @Skin         
	, Subfolder =  @Subfolder
	, Host	  =    @Host         
	, [Language] = @Language
	, TimeZone   = @TimeZone
	, ItemCount = @ItemCount
	, CategoryListPostCount = @CategoryListPostCount
	, News      = @News
	, CustomMetaTags      = @CustomMetaTags
	, TrackingCode      = @TrackingCode
	, LastUpdated = @LastUpdated
	, Flag = @Flag
	, SecondaryCss = @SecondaryCss
	, SkinCssFile = @SkinCssFile
	, LicenseUrl = @LicenseUrl
	, DaysTillCommentsClose = @DaysTillCommentsClose
	, CommentDelayInMinutes = @CommentDelayInMinutes
	, NumberOfRecentComments = @NumberOfRecentComments
	, RecentCommentsLength = @RecentCommentsLength
	, AkismetAPIKey = @AkismetAPIKey
	, FeedBurnerName = @FeedBurnerName
	, BlogGroupId =  @BlogGroupId
WHERE BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateConfig]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime]
(
	@BlogId int,
	@LastUpdated datetime
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Config]
SET LastUpdated = @LastUpdated
WHERE BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateEntry]
(
	@ID int
	, @Title nvarchar(255)
	, @Text ntext = NULL
	, @PostType int
	, @AuthorId uniqueidentifier
	, @Description nvarchar(500) = NULL
	, @DateUpdated datetime = NULL
	, @PostConfig int
	, @EntryName nvarchar(150) = NULL
	, @DateSyndicated DateTime = NULL
	, @BlogId int
)
AS

IF(LEN(RTRIM(LTRIM(@EntryName))) = 0)
	SET @EntryName = NULL

IF(@EntryName IS NOT NULL)
BEGIN
	IF EXISTS(SELECT EntryName FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = @BlogId AND EntryName = @EntryName AND [ID] <> @ID)
	BEGIN
		RAISERROR('The EntryName of your entry is already in use with in this Blog. Please pick a unique EntryName.', 11, 1) 
		RETURN 1
	END
END
IF(LTRIM(RTRIM(@Description)) = '')
SET @Description = NULL

UPDATE [<dbUser,varchar,dbo>].[subtext_Content] 
SET 
	Title = @Title 
	, [Text] = @Text 
	, PostType = @PostType
	, AuthorId = @AuthorId
	, [Description] = @Description
	, DateUpdated = @DateUpdated
	, PostConfig = @PostConfig
	, EntryName = @EntryName
	, DateSyndicated = @DateSyndicated
WHERE 	
		[ID] = @ID 
	AND BlogId = @BlogId
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime] @BlogId, @DateUpdated
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] @BlogId, @ID
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateEntry]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateImage]
(
	@Title nvarchar(250),
	@CategoryID int,
	@Width int,
	@Height int,
	@File nvarchar(50),
	@Active bit,
	@BlogId int,
	@ImageID int
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Images]
Set
	Title = @Title,
	CategoryID = @CategoryID,
	Width = @Width,
	Height = @Height,
	[File] = @File,
	Active = @Active
WHERE
	ImageID = @ImageID AND BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateImage]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateKeyWord]
(
	@KeyWordID int,
	@Word nvarchar(100),
	@Rel nvarchar(100),
	@Text nvarchar(100),
	@ReplaceFirstTimeOnly bit,
	@OpenInNewWindow bit,
	@CaseSensitive bit,
	@Url nvarchar(255),
	@Title nvarchar(100),
	@BlogId int
)

AS

UPDATE [<dbUser,varchar,dbo>].[subtext_keywords] 
	Set
		Word = @Word,
		Rel = @Rel,
		[Text] = @Text,
		ReplaceFirstTimeOnly = @ReplaceFirstTimeOnly,
		OpenInNewWindow = @OpenInNewWindow,
		CaseSensitive = @CaseSensitive,
		Url = @Url,
		Title = @Title
	WHERE
		BlogId = @BlogId AND KeyWordID = @KeyWordID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateKeyWord]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateLink]
(
	@LinkID int,
	@Title nvarchar(150),
	@Url nvarchar(255),
	@Rss nvarchar(255),
	@Active bit,
	@NewWindow bit,
	@CategoryID int,
	@BlogId int
	
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_Links] 
SET 
	Title = @Title, 
	Url = @Url, 
	Rss = @Rss, 
	Active = @Active,
	NewWindow = @NewWindow, 
	CategoryID = @CategoryID
WHERE  
		LinkID = @LinkID 
	AND BlogId = @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateLink]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Returns a page of blogs within subtext_config table
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]
(
	@PageIndex int
	, @PageSize int
	, @Host nvarchar(100) = NULL
	, @ConfigurationFlags int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
SELECT	@FirstId = [BlogId] FROM [<dbUser,varchar,dbo>].[subtext_Config]
WHERE @ConfigurationFlags & Flag = @ConfigurationFlags
	AND (Host = @Host OR @Host IS NULL)
ORDER BY [BlogId] ASC

SET ROWCOUNT @PageSize

SELECT	blog.BlogId 
		, blog.OwnerId
		, blog.Title
		, blog.SubTitle
		, blog.Skin
		, blog.Subfolder
		, blog.Host
		, blog.TimeZone
		, blog.ItemCount
		, blog.[Language]
		, blog.News
		, blog.SecondaryCss
		, blog.LastUpdated
		, blog.PostCount
		, blog.StoryCount
		, blog.PingTrackCount
		, blog.CommentCount
		, blog.Flag
		, blog.SkinCssFile 
		, blog.BlogGroupId
		, blog.LicenseUrl
		, blog.DaysTillCommentsClose
		, blog.CommentDelayInMinutes
		, blog.NumberOfRecentComments
		, blog.RecentCommentsLength
		, blog.AkismetAPIKey
		, blog.FeedBurnerName
		, bgroup.Title AS BlogGroupTitle
		
FROM [<dbUser,varchar,dbo>].[subtext_Config] blog
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_BlogGroup] bgroup ON
bgroup.Id = blog.BlogGroupId
WHERE blog.BlogId >= @FirstId
	AND @ConfigurationFlags & Flag = @ConfigurationFlags
	AND (Host = @Host OR @Host IS NULL)
ORDER BY BlogId ASC

SELECT COUNT([BlogId]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_config]
WHERE @ConfigurationFlags & Flag = @ConfigurationFlags
	AND (Host = @Host OR @Host IS NULL)
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPageableBlogs]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
SET ANSI_WARNINGS OFF
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertFeedback]
(
	@Title nvarchar(256)
	, @Body ntext = NULL
	, @BlogId int
	, @EntryId int = NULL
	, @Author nvarchar(64) = NULL
	, @IsBlogAuthor bit = 0
	, @Email varchar(128) = NULL
	, @Url varchar(256) = NULL
	, @FeedbackType int
	, @StatusFlag int
	, @CommentAPI bit
	, @Referrer varchar(256) = NULL
	, @IpAddress varchar(16) = NULL
	, @UserAgent nvarchar(128) = NULL
	, @FeedbackChecksumHash varchar(32)
	, @DateCreated datetime
	, @DateModified datetime = NULL
	, @Id int OUTPUT	
)
AS

IF @DateModified IS NULL
    SET @DateModified = getdate()
    
INSERT INTO [<dbUser,varchar,dbo>].[subtext_FeedBack]
( 
	Title
	, Body
	, BlogId
	, EntryId
	, Author
	, IsBlogAuthor
	, Email
	, Url
	, FeedbackType
	, StatusFlag
	, CommentAPI
	, Referrer
	, IpAddress
	, UserAgent
	, FeedbackChecksumHash
	, DateCreated
	, DateModified
)
VALUES 
(
	@Title
	, @Body
	, @BlogId
	, @EntryId
	, @Author
	, @IsBlogAuthor
	, @Email
	, @Url
	, @FeedbackType
	, @StatusFlag
	, @CommentAPI
	, @Referrer
	, @IpAddress
	, @UserAgent
	, @FeedbackChecksumHash
	, @DateCreated
	, @DateModified
)

SELECT @Id = SCOPE_IDENTITY()

exec [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] @BlogId, @EntryId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertFeedback]  TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateFeedback]
(
	@ID int
	, @Title nvarchar(256)
	, @Body ntext = NULL
	, @Author nvarchar(64) = NULL
	, @Email varchar(128) = NULL
	, @Url varchar(256) = NULL
	, @StatusFlag int
	, @FeedbackChecksumHash varchar(32)
	, @DateModified datetime
)
AS

DECLARE @EntryId int
DECLARE @BlogId int
SELECT @EntryId = EntryId, @BlogId = BlogId FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] WHERE Id = @Id

UPDATE [<dbUser,varchar,dbo>].[subtext_FeedBack]
SET	Title = @Title
	, Body = @Body
	, Author = @Author
	, Email = @Email
	, Url = @Url
	, StatusFlag = @StatusFlag
	, FeedbackChecksumHash = @FeedbackChecksumHash
	, DateModified = @DateModified
WHERE Id = @Id

exec [<dbUser,varchar,dbo>].[subtext_UpdateFeedbackCount] @BlogId, @EntryId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UpdateFeedback]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertEntry]
(
	@Title nvarchar(255)
	, @AuthorId uniqueidentifier
	, @Text ntext = NULL
	, @PostType int
	, @Description nvarchar(500) = NULL
	, @BlogId int
	, @DateAdded datetime
	, @PostConfig int
	, @EntryName nvarchar(150) = NULL
	, @DateSyndicated DateTime = NULL
	, @ID int OUTPUT
)
AS

IF(LEN(RTRIM(LTRIM(@EntryName))) = 0)
	SET @EntryName = NULL

IF(@EntryName IS NOT NULL)
BEGIN
	IF EXISTS(SELECT EntryName FROM [<dbUser,varchar,dbo>].[subtext_Content] WHERE BlogId = @BlogId AND EntryName = @EntryName)
	BEGIN
		RAISERROR('The EntryName of your entry is already in use with in this Blog. Please pick a unique EntryName.', 11, 1) 
		RETURN 1
	END
END
IF(LTRIM(RTRIM(@Description)) = '')
SET @Description = NULL

INSERT INTO subtext_Content 
(
	Title
	, AuthorId
	, [Text]
	, PostType
	, DateAdded
	, DateUpdated
	, [Description]
	, PostConfig
	, FeedbackCount
	, BlogId
	, EntryName 
	, DateSyndicated
)
VALUES 
(
	@Title
	, @AuthorId
	, @Text
	, @PostType
	, @DateAdded
	, @DateAdded
	, @Description
	, @PostConfig
	, 0 -- Feedback Count
	, @BlogId
	, @EntryName
	, @DateSyndicated
)
SELECT @ID = SCOPE_IDENTITY()

EXEC [<dbUser,varchar,dbo>].[subtext_UpdateConfigUpdateTime] @BlogId, @DateAdded
EXEC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] @BlogId


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertEntry]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/*
Retrieves a comment (or pingback) that has the specified 
FeedbackChecksumHash.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetCommentByChecksumHash]
(
	@FeedbackChecksumHash varchar(32)
	, @BlogId int
)
AS
SELECT TOP 1 f.Title
		, f.Body
		, f.BlogId
		, f.EntryId
		, f.Author
		, f.IsBlogAuthor
		, f.Email
		, f.Url
		, f.FeedbackType
		, f.StatusFlag
		, f.CommentAPI
		, f.Referrer
		, f.IpAddress
		, f.UserAgent
		, f.FeedbackChecksumHash
		, f.DateCreated
		, f.DateModified
		, ParentEntryCreateDate = c.DateAdded
		, ParentEntryName = c.EntryName
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Content] c ON f.EntryId = c.ID
WHERE 
	f.FeedbackChecksumHash = @FeedbackChecksumHash
	AND f.BlogId = @BlogId
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetCommentByChecksumHash]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[DNW_GetRecentPosts]
	@Host nvarchar(100)
	, @GroupID int

AS
SELECT Top 35 Host
	, Subfolder
	, [EntryName] = IsNull(content.EntryName, content.[ID])
	, content.[ID]
	, content.AuthorId
	, content.Title
	, content.DateAdded
	, content.PostType
	, content.FeedbackCount
	, content.EntryName
	, [IsXHTML] = Convert(bit,CASE WHEN content.PostConfig & 2 = 2 THEN 1 else 0 END) 
	, [BlogTitle] = content.Title
	, content.PostConfig
	, config.TimeZone
	, [Description] = IsNull(CASE WHEN PostConfig & 32 = 32 THEN content.[Description] else content.[Text] END, '')
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
INNER JOIN	[<dbUser,varchar,dbo>].[subtext_Config] config ON content.BlogId = config.BlogId
WHERE  content.PostType = 1 
	AND content.PostConfig & 1 = 1 
	AND content.PostConfig & 64 = 64 
	AND config.Flag & 2 = 2 
	AND config.Host = @Host
	AND (BlogGroupId = @GroupID or @GroupID = 0)
ORDER BY [ID] DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[DNW_GetRecentPosts]  TO [public]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[DNW_Stats]
(
	@Host nvarchar(100),
	@GroupID int
)
AS
SELECT blog.BlogId
	, blog.OwnerId
	, blog.Subfolder
	, blog.Host
	, blog.Title
	, blog.PostCount
	, blog.CommentCount
	, blog.StoryCount
	, blog.PingTrackCount
	, blog.LastUpdated
	, blog.BlogGroupId
	, bgroup.Title AS BlogGroupTitle
FROM [<dbUser,varchar,dbo>].[subtext_Config] blog
	LEFT OUTER JOIN [<dbUser,varchar,dbo>].[subtext_BlogGroup] bgroup ON
bgroup.Id = blog.BlogGroupId
WHERE PostCount > 0		
	AND blog.Flag & 2 = 2 
	AND blog.Flag & 1 = 1
	AND blog.Host = @Host
	AND bgroup.Active = 1
	AND blog.IsActive = 1
	AND (blog.BlogGroupId = @GroupID
	OR @GroupID = 0)
ORDER BY bgroup.DisplayOrder, bgroup.Id,  blog.PostCount DESC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[DNW_Stats]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[DNW_Total_Stats]
(
	@Host nvarchar(100),
	@GroupID int
)
AS
SELECT Count(*) AS [BlogCount]
	, Sum(PostCount) AS PostCount
	, Sum(CommentCount) AS CommentCount
	, Sum(StoryCount) AS StoryCount
	, Sum(PingTrackCount) AS PingTrackCount 
FROM [<dbUser,varchar,dbo>].[subtext_Config] 
	WHERE subtext_Config.Flag & 2 = 2 
		AND Host = @Host 
		AND (BlogGroupId = @GroupID OR @GroupID = 0)

SET QUOTED_IDENTIFIER ON


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [DNW_Total_Stats]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC [<dbUser,varchar,dbo>].[DNW_HomePageData]
(
	@Host nvarchar(100),
	@GroupID int
)
AS 
EXEC [<dbUser,varchar,dbo>].[DNW_Stats] @Host, @GroupID
EXEC [<dbUser,varchar,dbo>].[DNW_GetRecentPosts] @Host, @GroupID
EXEC [<dbUser,varchar,dbo>].[DNW_Total_Stats] @Host, @GroupID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[DNW_HomePageData]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Gets the most recent version in the Version table */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_VersionGetCurrent]
AS
SELECT	TOP 1
		[Id]
		, [Major]
		, [Minor]
		, [Build]
		, [DateCreated]
FROM	[<dbUser,varchar,dbo>].[subtext_Version]
ORDER BY [DateCreated] DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_VersionGetCurrent]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Gets the most recent version in the Version table */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_VersionAdd]
(
	 @Major	INT
	, @Minor INT
	, @Build INT
	, @DateCreated DATETIME = NULL
	, @Id INT = NULL OUTPUT
)
AS

IF @DateCreated IS NULL
	SET @DateCreated = getdate()

INSERT [<dbUser,varchar,dbo>].[subtext_Version]
SELECT	@Major, @Minor, @Build, @DateCreated

SELECT @Id = SCOPE_IDENTITY()

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_VersionAdd]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Creates a record in the subtext_log table */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_LogClear]
(
	@BlogId int = NULL
)
AS

IF(@BlogId IS NULL)
	TRUNCATE TABLE [<dbUser,varchar,dbo>].[subtext_Log]
ELSE
	DELETE [<dbUser,varchar,dbo>].[subtext_Log] WHERE [BlogId] = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_LogClear]  TO [public]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/* Creates a record in the subtext_log table */
CREATE PROC [<dbUser,varchar,dbo>].[subtext_AddLogEntry]
(
	 @Date DateTime
	 , @BlogId int = NULL
	 , @Thread varchar(255)
	 , @Context varchar(512)
	 , @Level varchar(20)
	 , @Logger nvarchar(256)
	 , @Message nvarchar(2000)
	 , @Exception nvarchar(1000)
	 , @Url varchar(255)
)
AS

if @BlogId < 0
	SET @BlogId = NULL

INSERT [<dbUser,varchar,dbo>].[subtext_Log]
SELECT	@BlogId, @Date, @Thread, @Context, @Level, @Logger, @Message, @Exception, @Url

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_AddLogEntry]  TO [public]
GO

/*Search Entries-GY*/
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].subtext_SearchEntries
(
	@BlogId int
	, @SearchStr nvarchar(30)
)
as

Set @SearchStr = '%' + @SearchStr + '%'

Select [ID]
	, Title
	, DateAdded
	, EntryName
	, PostType
From [<dbUser,varchar,dbo>].[subtext_Content]
Where (PostType = 1 OR PostType = 2)
	AND PostConfig & 1 = 1 -- IsActive!
	AND ([Text] LIKE @SearchStr OR Title LIKE @SearchStr)
	AND BlogId = @BlogId
	
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_SearchEntries]  TO [public]
GO

/*Previous Next*/
if exists (select * from dbo.sysobjects where id = object_id(N'[<dbUser,varchar,dbo>].[subtext_GetEntry_PreviousNext]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [<dbUser,varchar,dbo>].[subtext_GetEntry_PreviousNext]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetEntry_PreviousNext]
(
	@ID int
	, @PostType int = 1
	, @BlogId int
)
AS

DECLARE @DateSyndicated DateTime
SELECT @DateSyndicated = DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE ID = @ID

-- If DateSyndicated is NULL, then this entry hasn't been published yet.
SELECT * FROM
(
	SELECT Top 1 BlogId
		, [ID]
		, Title
		, DateAdded
		, PostType
		, PostConfig
		, EntryName 
		, DateSyndicated
		, CardinalityDate = DateSyndicated 	FROM [<dbUser,varchar,dbo>].[subtext_Content]
	WHERE DateSyndicated >= @DateSyndicated
		AND subtext_Content.BlogId = @BlogId 
		AND subtext_Content.PostConfig & 1 = 1 
		AND PostType = @PostType
		AND [ID] != @ID
	ORDER BY DateSyndicated ASC
) [Previous]
UNION
SELECT * FROM
(
	SELECT Top 1 BlogId
		, [ID]
		, Title
		, DateAdded
		, PostType
		, PostConfig
		, EntryName 
		, DateSyndicated
		, CardinalityDate = DateSyndicated
	FROM [<dbUser,varchar,dbo>].[subtext_Content]
	WHERE DateSyndicated <= @DateSyndicated
		AND subtext_Content.BlogId = @BlogId 
		AND subtext_Content.PostConfig & 1 = 1 
		AND PostType = @PostType
		AND [ID] != @ID
	ORDER BY DateSyndicated DESC
) [Next]

ORDER BY CardinalityDate DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetEntry_PreviousNext]  TO [public]
GO


/*Get Related Links (called from RelatedLinks.ascx) - GY*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetRelatedLinks] 
@BlogId int,
@EntryID int
AS

Select Distinct Top 10 c.ID EntryID, c.Title, c.DateAdded 
From [<dbUser,varchar,dbo>].subtext_LinkCategories lc, [<dbUser,varchar,dbo>].subtext_Links l, [<dbUser,varchar,dbo>].subtext_Content c 
Where lc.CategoryType = 1 
And lc.Active = 1
And l.CategoryID = lc.CategoryID
And l.CategoryID In (Select CategoryID From [<dbUser,varchar,dbo>].subtext_links Where PostID = @EntryID)
And l.PostID = c.ID
And c.BlogId = @BlogId --param
And c.ID <> @EntryID --param --do not list the same entry in related links
Order By c.DateAdded Desc


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetRelatedLinks]  TO [public]
GO

/*Top10Posts - (called from Top10Module.ascx) - GY*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]  
@BlogId int
AS
Select Distinct top 10 evc.EntryId, (evc.WebCount + evc.AggCount) As mcount, c.title, c.DateAdded
From [<dbUser,varchar,dbo>].subtext_EntryViewCount evc, [<dbUser,varchar,dbo>].subtext_Content c
Where evc.EntryId = c.Id
And c.BlogId = @BlogId --param
Order By mcount desc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetTop10byBlogId]  TO [public]
GO

/*
Selects a page of blog posts for export to blogml. These are 
sorted ascending by id to map to the database.
*/
CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl]
(
	@BlogId int
	, @PageIndex int
	, @PageSize int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = [ID] FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	BlogId = @BlogId 
	AND (PostType = 1 OR PostType = 2) -- PostType 1 = BlogPost, 2 = Story
ORDER BY [ID] ASC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

CREATE Table #IDs  
(  
	 TempId int IDENTITY (0, 1) NOT NULL,  
	 Id int not NULL  
)

-- Store the IDs for this page in a temp table.
INSERT #IDs (Id)  
SELECT [Id]   
FROM [<dbUser,varchar,dbo>].[subtext_Content]
WHERE	(PostType = 1 OR PostType = 2)
	AND BlogId = @BlogId
	AND [ID] >= @FirstId
ORDER BY Id ASC

SET ROWCOUNT 0

SELECT	content.BlogId 
		, idTable.[ID] 
		, content.AuthorId
		, content.Title 
		, content.DateAdded 
		, content.[Text] 
		, content.[Description]
		, content.PostType 
		, content.DateUpdated 
		, FeedbackCount = ISNULL(content.FeedbackCount, 0)
		, content.PostConfig
		, content.EntryName
		, content.DateSyndicated
		
FROM [<dbUser,varchar,dbo>].[subtext_Content] content
	INNER JOIN #IDs idTable ON idTable.Id = content.[ID]
ORDER BY idTable.[ID] ASC
 
SELECT COUNT([ID]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_Content] 
WHERE 	BlogId = @BlogId 
	AND PostType = 1 OR PostType = 2

-- Select associated categories
SELECT	p.[Id]
		, c.CategoryID
	FROM [<dbUser,varchar,dbo>].[subtext_Links] l
		INNER JOIN #IDs p ON l.[PostID] = p.[ID]  
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_LinkCategories] c ON l.CategoryID = c.CategoryID
	ORDER BY p.[ID] ASC

-- Select associated comments
SELECT	f.[Id]
		, Title
		, Body
		, BlogId
		, EntryId
		, Author
		, IsBlogAuthor
		, Email
		, Url
		, FeedbackType
		, StatusFlag
		, CommentAPI
		, Referrer
		, IpAddress
		, UserAgent
		, FeedbackChecksumHash
		, DateCreated
		, DateModified
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	INNER JOIN #IDs idTable ON idTable.Id = f.[EntryId]
	WHERE f.FeedbackType = 1 -- Comment
ORDER BY idTable.[ID] ASC

-- Select associated track/ping backs.
SELECT	f.[Id]
		, Title
		, Body
		, BlogId
		, EntryId
		, Author
		, IsBlogAuthor
		, Email
		, Url
		, FeedbackType
		, StatusFlag
		, CommentAPI
		, Referrer
		, IpAddress
		, UserAgent
		, FeedbackChecksumHash
		, DateCreated
		, DateModified
FROM [<dbUser,varchar,dbo>].[subtext_FeedBack] f
	INNER JOIN #IDs idTable ON idTable.Id = f.[EntryId]
	WHERE f.FeedbackType = 2 -- Trackback/Pingback
ORDER BY idTable.[ID] ASC

-- Get the Post's author(s)
SELECT	p.[Id]
		, @BlogId AS AuthorId-- Hardcoding the AuthorId since right now we only have one.
	FROM #IDs p
	ORDER BY p.[ID] ASC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetEntriesForBlogMl] TO [public]
GO




/*
	subtext_GetPostsByCategoriesArchive - (called from CategoryCloud.ascx) - SCH
	retrieves all active categories with realative post number
*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoriesArchive]  
(
	@BlogId int = NULL
)
AS

SELECT	[Id] = c.CategoryID
		, c.Title 
		, [Count] = COUNT(1)
		, [Month] = 1
		, [Year] = 1
		, [Day] = 1
	
FROM [<dbUser,varchar,dbo>].[subtext_LinkCategories] c 
	INNER JOIN [<dbUser,varchar,dbo>].[subtext_Links] l on c.CategoryID = l.CategoryID
WHERE	c.Active= 1 
	AND	(c.BlogId = @BlogId OR @BlogId IS NULL)
	AND	c.CategoryType = 1 -- post category

GROUP BY c.CategoryID, c.Title
ORDER BY c.Title

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByCategoriesArchive]  TO [public]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]
(
	@BlogId int
)
AS

SELECT 
	KeyWordID
	, Word
	, Rel
	, [Text]
	, ReplaceFirstTimeOnly
	, OpenInNewWindow
	, CaseSensitive
	, Url
	, Title
	, BlogId
FROM
	[<dbUser,varchar,dbo>].[subtext_keywords]
WHERE 
	BlogId = @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetBlogKeyWords]  TO [public]
GO


/*	ClearBlogContent - used to delete all content (Entries, Comments, Track/Ping-backs, Statistics, etc...)
	for a given blog (sans the Image Galleries). Used from the Admin -> Import/Export Page.
*/
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_ClearBlogContent]
	@BlogId int
AS
DELETE FROM [<dbUser,varchar,dbo>].subtext_Referrals WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_Log WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_Links WHERE BlogId = @BlogId
--DELETE FROM [<dbUser,varchar,dbo>].subtext_Images WHERE BlogId = @BlogId  -- Don't want to wipe out the images this way b/c that would leave them on the disk.
DELETE FROM [<dbUser,varchar,dbo>].subtext_LinkCategories WHERE BlogId = @BlogId AND CategoryType <> 3 -- We're not doing Image Galleries.
DELETE FROM [<dbUser,varchar,dbo>].subtext_KeyWords WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_EntryViewCount WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_FeedBack WHERE BlogId = @BlogId
DELETE FROM [<dbUser,varchar,dbo>].subtext_Content WHERE BlogId = @BlogId

EXEC [<dbUser,varchar,dbo>].[subtext_UpdateBlogStats] @BlogId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_ClearBlogContent]  TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPostsByTag]
(
	@ItemCount int
	, @Tag nvarchar(256)
	, @BlogId int
)
AS
DECLARE @TagId int
SELECT @TagId = Id FROM subtext_Tag WHERE BlogId = @BlogId AND [Name] = @Tag

SET ROWCOUNT @ItemCount
SELECT	content.BlogId
	, content.[ID]
	, content.Title
	, content.DateAdded
	, content.[Text]
	, content.[Description]
	, content.PostType
	, content.AuthorId
	, content.DateUpdated
	, FeedbackCount = ISNULL(content.FeedbackCount, 0)
	, content.PostConfig
	, content.EntryName 
	, content.DateSyndicated
FROM [<dbUser,varchar,dbo>].[subtext_Content] content WITH (NOLOCK)
WHERE  content.BlogId = @BlogId 
	AND content.PostConfig & 1 = 1
	AND content.ID IN 
	(
		SELECT EntryId 
		FROM [<dbUser,varchar,dbo>].[subtext_EntryTag] 
		WHERE BlogId = @BlogId 
			AND TagId = @TagId
	)
ORDER BY content.DateAdded DESC
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetPostsByTag]  TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertEntryTagList] 
	(
	@EntryId int,
	@BlogId int,
	@TagList ntext
	)
AS

-- When taglist is empty, delete any potentially existing tags.
IF CONVERT(nvarchar(1),@TagList) = ''
BEGIN
	DELETE FROM subtext_EntryTag 
		WHERE BlogId = @BlogId AND EntryId = @EntryId
END
ELSE
BEGIN
	DECLARE @Tags TABLE (tagId int,
						 tag nvarchar(2000))
	-- Populate the in-memory table with the @TagList string broken out into rows.
	INSERT INTO @Tags 
		SELECT t.Id, c.nstr
		FROM iter_charlist_to_table(@TagList, ',') c
		LEFT OUTER JOIN subtext_Tag t ON BlogId = @BlogId AND t.[Name] = c.nstr

	-- If a tag doesn't exist, it needs to be created in subtext_Tag.
	INSERT INTO subtext_Tag (BlogId, [Name])
		SELECT @BlogId, tag  FROM @Tags 
		WHERE tag NOT IN (SELECT [Name] FROM subtext_Tag WHERE BlogId = @BlogID)
	
	-- If tags were created above, we need to update @Tags with their Ids.
	UPDATE @Tags SET tagId = s.Id FROM @Tags t, subtext_Tag s 
		WHERE s.BlogId = @BlogId AND s.[Name] = t.tag AND t.TagId IS NULL

	-- If tags exist for an entry that have been removed, remove the link.
	DELETE FROM subtext_EntryTag 
		WHERE BlogId = @BlogId AND EntryId = @EntryId 
		AND TagId NOT IN (SELECT tagId FROM @Tags)

	-- Now add any tags that aren't already linked.
	INSERT INTO subtext_EntryTag (BlogId, EntryId, TagId)
	SELECT @BlogId, @EntryId, tagId FROM @Tags
		WHERE tagId NOT IN 
			(SELECT TagId FROM subtext_EntryTag 
				WHERE BlogId = @BlogId AND EntryId = @EntryId)
END
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_InsertEntryTagList] TO [public]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetTopTags] 
	(
	@ItemCount int
	, @BlogId int
	)
AS
SET ROWCOUNT @ItemCount
SELECT t.[Name], COUNT(*) AS TagCount FROM [<dbUser,varchar,dbo>].subtext_Tag t, [<dbUser,varchar,dbo>].subtext_EntryTag e
WHERE t.BlogId = @BlogId AND t.Id = e.TagId
GROUP BY t.[Name]
ORDER BY Count(*) DESC
GO 

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_GetTopTags] TO [public]
GO




SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdatePluginData]
(
	@PluginID uniqueidentifier,
	@BlogID int,
	@EntryID int,
	@Key nvarchar(256),
	@Value ntext
)
AS

UPDATE [<dbUser,varchar,dbo>].[subtext_PluginData]
SET
	[Value]=@Value
FROM [<dbUser,varchar,dbo>].[subtext_PluginBlog]
WHERE [<dbUser,varchar,dbo>].[subtext_PluginBlog].PluginID=@PluginID AND [<dbUser,varchar,dbo>].[subtext_PluginBlog].BlogID=@BlogID AND [<dbUser,varchar,dbo>].[subtext_PluginBlog].[Id]=[<dbUser,varchar,dbo>].[subtext_PluginData].BlogPluginID AND [<dbUser,varchar,dbo>].[subtext_PluginBlog].Enabled=1 AND [Key]=@Key AND EntryID=@EntryID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertPluginData]
(
	@PluginID uniqueidentifier,
	@BlogID int,
	@EntryID int,
	@Key nvarchar(256),
	@Value ntext
)
AS

DECLARE @BlogPluginID int

SELECT @BlogPluginID=[Id] FROM [<dbUser,varchar,dbo>].[subtext_PluginBlog]
WHERE PluginID=@PluginID AND BlogID=@BlogID

INSERT INTO [<dbUser,varchar,dbo>].[subtext_PluginData]
(
	BlogPluginID,
	EntryID,
	[Key],
	[Value]
)
VALUES
(
	@BlogPluginID,
	@EntryID,
	@Key,
	@Value
)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeletePluginBlog]
(
	@PluginID uniqueidentifier,
	@BlogId int
)
as

UPDATE  [<dbUser,varchar,dbo>].[subtext_PluginBlog]
SET Enabled=0
WHERE PluginID=@PluginID AND BlogId=@BlogId 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPluginBlog]
(
	@BlogId int
)

AS

SELECT PluginID FROM [<dbUser,varchar,dbo>].[subtext_PluginBlog]
WHERE
BlogID=@BlogId AND Enabled=1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertPluginBlog]
(
	@PluginID uniqueidentifier,
	@BlogId int
)
as

IF NOT EXISTS 
	(
		SELECT * FROM [<dbUser,varchar,dbo>].[subtext_PluginBlog]
		WHERE PluginID=@PluginID AND BlogId=@BlogId
	)

	BEGIN
	
		INSERT INTO [<dbUser,varchar,dbo>].[subtext_PluginBlog]
		(
			PluginID,
			BlogID,
			Enabled
		)
		VALUES
		(
			@PluginID,
			@BlogId,
			1
		)
	END
ELSE

	BEGIN
		UPDATE  [<dbUser,varchar,dbo>].[subtext_PluginBlog]
		SET Enabled=1
		WHERE PluginID=@PluginID AND BlogId=@BlogId 
	END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetPluginData]
(
	@PluginID uniqueidentifier,
	@BlogId int,
	@EntryId int
)
as

SELECT [Key], [Value]
FROM [<dbUser,varchar,dbo>].[subtext_PluginData] INNER JOIN [<dbUser,varchar,dbo>].[subtext_PluginBlog] ON [<dbUser,varchar,dbo>].[subtext_PluginBlog].[Id]=[<dbUser,varchar,dbo>].[subtext_PluginData].BlogPluginID
WHERE [<dbUser,varchar,dbo>].[subtext_PluginBlog].PluginID=@PluginID and [<dbUser,varchar,dbo>].[subtext_PluginBlog].BlogId=@BlogId and EntryId=@EntryId AND [<dbUser,varchar,dbo>].[subtext_PluginBlog].Enabled=1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/* Membership SPs */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Setup_RestorePermissions]
    @name   sysname
AS
BEGIN
    DECLARE @object sysname
    DECLARE @protectType char(10)
    DECLARE @action varchar(20)
    DECLARE @grantee sysname
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT Object, ProtectType, [Action], Grantee FROM #subtext_Permissions where Object = @name

    OPEN c1

    FETCH c1 INTO @object, @protectType, @action, @grantee
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = @protectType + ' ' + @action + ' on ' + @object + ' TO [' + @grantee + ']'
        EXEC (@cmd)
        FETCH c1 INTO @object, @protectType, @action, @grantee
    END

    CLOSE c1
    DEALLOCATE c1
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Setup_RemoveAllRoleMembers]
    @name   sysname
AS
BEGIN
    CREATE TABLE #subtext_RoleMembers
    (
        Group_name      sysname,
        Group_id        smallint,
        Users_in_group  sysname,
        User_id         smallint
    )

    INSERT INTO #subtext_RoleMembers
    EXEC sp_helpuser @name

    DECLARE @user_id smallint
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT User_id FROM #subtext_RoleMembers

    OPEN c1

    FETCH c1 INTO @user_id
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = 'EXEC sp_droprolemember ' + '''' + @name + ''', ''' + USER_NAME(@user_id) + ''''
        EXEC (@cmd)
        FETCH c1 INTO @user_id
    END

    CLOSE c1
    DEALLOCATE c1
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128),
    @IsCurrentVersion          bit,
    @RemoveIncompatibleSchema  bit
AS
BEGIN
    IF( @RemoveIncompatibleSchema = 1 )
    BEGIN
        DELETE FROM [<dbUser,varchar,dbo>].[subtext_SchemaVersions] WHERE Feature = LOWER( @Feature )
    END
    ELSE
    BEGIN
        IF( @IsCurrentVersion = 1 )
        BEGIN
            UPDATE [<dbUser,varchar,dbo>].subtext_SchemaVersions
            SET IsCurrentVersion = 0
            WHERE Feature = LOWER( @Feature )
        END
    END

    INSERT  [<dbUser,varchar,dbo>].subtext_SchemaVersions( Feature, CompatibleSchemaVersion, IsCurrentVersion )
    VALUES( LOWER( @Feature ), @CompatibleSchemaVersion, @IsCurrentVersion )
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    IF (EXISTS( SELECT  *
                FROM    [<dbUser,varchar,dbo>].subtext_SchemaVersions
                WHERE   Feature = LOWER( @Feature ) AND
                        CompatibleSchemaVersion = @CompatibleSchemaVersion ))
        RETURN 0

    RETURN 1
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    DELETE FROM [<dbUser,varchar,dbo>].subtext_SchemaVersions
        WHERE   Feature = LOWER(@Feature) AND @CompatibleSchemaVersion = CompatibleSchemaVersion
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByName]
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier
   
    SELECT TOP 1 UserName
		, Email
		, PasswordQuestion
		, Comment
		, IsApproved
		, CreateDate
        , LastLoginDate
        , LastActivityDate = @CurrentTimeUtc
        , LastPasswordChangedDate
        , UserId
        , IsLockedOut
        , LastLockoutDate
    FROM	[<dbUser,varchar,dbo>].[subtext_Users]
    WHERE    
            LOWER(@UserName) = LoweredUserName 

    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN -1

	IF (@UpdateLastActivity = 1)
    BEGIN
    
        UPDATE   [<dbUser,varchar,dbo>].[subtext_Users]
        SET      LastActivityDate = @CurrentTimeUtc
        WHERE    @UserId = UserId
    END

    RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByUserId]
    @UserId               uniqueidentifier,
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    SELECT  UserId
			, Email
			, PasswordQuestion
			, Comment
			, IsApproved
			, CreateDate
			, LastLoginDate
			, LastActivityDate
			, LastPasswordChangedDate
			, UserName
			, IsLockedOut
			, LastLockoutDate
    FROM    [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE   @UserId = UserId

    IF ( @@ROWCOUNT = 0 ) -- User ID not found
       RETURN -1

    IF ( @UpdateLastActivity = 1 )
    BEGIN
        UPDATE   [<dbUser,varchar,dbo>].subtext_Users
        SET      LastActivityDate = @CurrentTimeUtc
        FROM     [<dbUser,varchar,dbo>].[subtext_Users]
        WHERE    @UserId = UserId

        IF ( @@ROWCOUNT = 0 ) -- User ID not found
            RETURN -1
    END

    RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByEmail]
    @Email            nvarchar(256)
AS
BEGIN
    
    SELECT  UserName
    FROM    [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE   (@Email IS NULL AND LoweredEmail IS NULL)
		OR  (NOT @Email IS NULL AND LOWER(@Email) = LoweredEmail)

    IF (@@rowcount = 0)
        RETURN(1)
    RETURN(0)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetPasswordWithFormat]
    @UserName                       nvarchar(256),
    @UpdateLastLoginActivityDate    bit,
    @CurrentTimeUtc                 datetime
AS
BEGIN
    DECLARE @IsLockedOut                        bit
    DECLARE @UserId                             uniqueidentifier
    DECLARE @Password                           nvarchar(128)
    DECLARE @PasswordSalt                       nvarchar(128)
    DECLARE @PasswordFormat                     int
    DECLARE @FailedPasswordAttemptCount         int
    DECLARE @FailedPasswordAnswerAttemptCount   int
    DECLARE @IsApproved                         bit
    DECLARE @LastActivityDate                   datetime
    DECLARE @LastLoginDate                      datetime

    SELECT  @UserId          = NULL

    SELECT  @UserId = UserId
			, @IsLockedOut = IsLockedOut
			, @Password=Password
			, @PasswordFormat=PasswordFormat
			, @PasswordSalt=PasswordSalt
			, @FailedPasswordAttemptCount=FailedPasswordAttemptCount
			, @FailedPasswordAnswerAttemptCount=FailedPasswordAnswerAttemptCount
			, @IsApproved=IsApproved
			, @LastActivityDate = LastActivityDate
			, @LastLoginDate = LastLoginDate
    FROM    [<dbUser,varchar,dbo>].subtext_Users u
    WHERE   
            LOWER(@UserName) = LoweredUserName

    IF (@UserId IS NULL)
        RETURN 1

    IF (@IsLockedOut = 1)
        RETURN 99

    SELECT   Password = @Password
		, PasswordFormat = @PasswordFormat
		, PasswordSalt = @PasswordSalt
		, FailedPasswordAttemptCount = @FailedPasswordAttemptCount
		, FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount
		, IsApproved = @IsApproved
		, LastLoginDate = @LastLoginDate
		, LastActivityDate = @LastActivityDate

    IF (@UpdateLastLoginActivityDate = 1 AND @IsApproved = 1)
    BEGIN
        UPDATE  [<dbUser,varchar,dbo>].subtext_Users
        SET     LastLoginDate = @CurrentTimeUtc
			, LastActivityDate = @CurrentTimeUtc
        WHERE   UserId = @UserId
    END

    RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_UpdateUserInfo]
    @UserName                       nvarchar(256),
    @IsPasswordCorrect              bit,
    @UpdateLastLoginActivityDate    bit,
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @LastLoginDate                  datetime,
    @LastActivityDate               datetime
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @IsApproved                             bit
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = UserId
            , @IsApproved = IsApproved
            , @IsLockedOut = IsLockedOut
            , @LastLockoutDate = LastLockoutDate
            , @FailedPasswordAttemptCount = FailedPasswordAttemptCount
            , @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart
            , @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount
            , @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM    [<dbUser,varchar,dbo>].[subtext_Users] WITH ( UPDLOCK )
    WHERE   
			LOWER(@UserName) = LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        GOTO Cleanup
    END

    IF( @IsPasswordCorrect = 0 )
    BEGIN
        IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAttemptWindowStart ) )
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = 1
        END
        ELSE
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1
        END

        BEGIN
            IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
            BEGIN
                SET @IsLockedOut = 1
                SET @LastLockoutDate = @CurrentTimeUtc
            END
        END
    END
    ELSE
    BEGIN
        IF( @FailedPasswordAttemptCount > 0 OR @FailedPasswordAnswerAttemptCount > 0 )
        BEGIN
            SET @FailedPasswordAttemptCount = 0
            SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @FailedPasswordAnswerAttemptCount = 0
            SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )
        END
    END

    IF( @UpdateLastLoginActivityDate = 1 )
    BEGIN
        UPDATE  [<dbUser,varchar,dbo>].[subtext_Users]
        SET     LastActivityDate = @LastActivityDate
			, LastLoginDate = @LastLoginDate
        WHERE   @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END


    UPDATE [<dbUser,varchar,dbo>].[subtext_Users]
		SET IsLockedOut = @IsLockedOut
			, LastLockoutDate = @LastLockoutDate
			, FailedPasswordAttemptCount = @FailedPasswordAttemptCount
			, FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart
			, FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount
			, FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
		WHERE @UserId = UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetPassword]
    @UserName                       nvarchar(256),
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @PasswordAnswer                 nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @PasswordFormat                         int
    DECLARE @Password                               nvarchar(128)
    DECLARE @passAns                                nvarchar(128)
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = UserId,
            @Password = Password,
            @passAns = PasswordAnswer,
            @PasswordFormat = PasswordFormat,
            @IsLockedOut = IsLockedOut,
            @LastLockoutDate = LastLockoutDate,
            @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM    [<dbUser,varchar,dbo>].[subtext_Users] WITH ( UPDLOCK )
    WHERE   
            LOWER(@UserName) = LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    IF ( NOT( @PasswordAnswer IS NULL ) )
    BEGIN
        IF( ( @passAns IS NULL ) OR ( LOWER( @passAns ) <> LOWER( @PasswordAnswer ) ) )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
        ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

        UPDATE [<dbUser,varchar,dbo>].[subtext_Users]
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    IF( @ErrorCode = 0 )
        SELECT Password = @Password, PasswordFormat = @PasswordFormat

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetNumberOfUsersOnline]
    @MinutesSinceLastInActive   int,
    @CurrentTimeUtc             datetime,
    @OnlineUserCount			int output
AS
BEGIN
    DECLARE @DateActive datetime
    SELECT  @DateActive = DATEADD(minute, -(@MinutesSinceLastInActive), @CurrentTimeUtc)

    SELECT  @OnlineUserCount = COUNT(1)
    FROM    [<dbUser,varchar,dbo>].[subtext_Users] (NOLOCK)
    WHERE   LastActivityDate > @DateActive
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_SetPassword]
    @UserName         nvarchar(256),
    @NewPassword      nvarchar(128),
    @PasswordSalt     nvarchar(128),
    @CurrentTimeUtc   datetime,
    @PasswordFormat   int = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = UserId
    FROM    [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE   LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN(1)

    UPDATE [<dbUser,varchar,dbo>].[subtext_Users]
    SET Password = @NewPassword
		, PasswordFormat = @PasswordFormat
		, PasswordSalt = @PasswordSalt
		, LastPasswordChangedDate = @CurrentTimeUtc
    WHERE @UserId = UserId
    
    RETURN(0)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_ResetPassword]
    @UserName                    nvarchar(256),
    @NewPassword                 nvarchar(128),
    @MaxInvalidPasswordAttempts  int,
    @PasswordAttemptWindow       int,
    @PasswordSalt                nvarchar(128),
    @CurrentTimeUtc              datetime,
    @PasswordFormat              int = 0,
    @PasswordAnswer              nvarchar(128) = NULL
AS
BEGIN
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @UserId                                 uniqueidentifier
    SET     @UserId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = UserId
    FROM    [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE   LoweredUserName = LOWER(@UserName)       

    IF ( @UserId IS NULL )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    SELECT @IsLockedOut = IsLockedOut,
           @LastLockoutDate = LastLockoutDate,
           @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
           @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
           @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
           @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM [<dbUser,varchar,dbo>].subtext_Users WITH ( UPDLOCK )
    WHERE @UserId = UserId

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    UPDATE [<dbUser,varchar,dbo>].[subtext_Users]
    SET    Password = @NewPassword
		, LastPasswordChangedDate = @CurrentTimeUtc
		, PasswordFormat = @PasswordFormat
		, PasswordSalt = @PasswordSalt
    WHERE  UserId = @UserId
		AND
		( 
			( @PasswordAnswer IS NULL ) 
			OR 
			( LOWER(PasswordAnswer) = LOWER(@PasswordAnswer) ) 
		)

    IF ( @@ROWCOUNT = 0 )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
    ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

    IF( NOT ( @PasswordAnswer IS NULL ) )
    BEGIN
        UPDATE [<dbUser,varchar,dbo>].[subtext_Users]
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_UnlockUser]
    @UserName                                nvarchar(256)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = UserId
    FROM    [<dbUser,varchar,dbo>].subtext_Users
    WHERE   LoweredUserName = LOWER(@UserName)

    IF ( @UserId IS NULL )
        RETURN 1

    UPDATE [<dbUser,varchar,dbo>].[subtext_Users]
    SET IsLockedOut = 0,
        FailedPasswordAttemptCount = 0,
        FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        FailedPasswordAnswerAttemptCount = 0,
        FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        LastLockoutDate = CONVERT( datetime, '17540101', 112 )
    WHERE @UserId = UserId

    RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_UpdateUser]
    @UserName             nvarchar(256),
    @Email                nvarchar(256),
    @Comment              ntext = NULL,
    @IsApproved           bit,
    @LastLoginDate        datetime,
    @LastActivityDate     datetime,
    @UniqueEmail          int,
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = UserId
    FROM    [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE   LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN(1)

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  [<dbUser,varchar,dbo>].[subtext_Users] WITH (UPDLOCK, HOLDLOCK)
                    WHERE @UserId <> UserId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            RETURN(7)
        END
    END

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    UPDATE [<dbUser,varchar,dbo>].[subtext_Users] WITH (ROWLOCK)
    SET
         Email            = @Email
         , LoweredEmail     = LOWER(@Email)
         , Comment          = @Comment
         , IsApproved       = @IsApproved
         , LastLoginDate    = @LastLoginDate
         , LastActivityDate = @LastActivityDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_ChangePasswordQuestionAndAnswer]
    @UserName              nvarchar(256),
    @NewPasswordQuestion   nvarchar(256) = NULL,
    @NewPasswordAnswer     nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = UserId
    FROM    [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE   LoweredUserName = LOWER(@UserName) 
    
    IF (@UserId IS NULL)
    BEGIN
        RETURN(1)
    END

    UPDATE [<dbUser,varchar,dbo>].[subtext_Users]
    SET    PasswordQuestion = @NewPasswordQuestion
		, PasswordAnswer = @NewPasswordAnswer
    WHERE  UserId = @UserId
    RETURN(0)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_GetProperties]
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT @UserId = UserId
    FROM   [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE  LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN
    SELECT TOP 1 PropertyNames, PropertyValuesString, PropertyValuesBinary
    FROM         [<dbUser,varchar,dbo>].[subtext_Profile]
    WHERE        UserId = @UserId

    IF (@@ROWCOUNT > 0)
    BEGIN
        UPDATE [<dbUser,varchar,dbo>].[subtext_Users]
        SET    LastActivityDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_DeleteInactiveProfiles]
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DELETE
    FROM    [<dbUser,varchar,dbo>].[subtext_Profile]
    WHERE   UserId IN
            (   SELECT  UserId
                FROM    [<dbUser,varchar,dbo>].subtext_Users
                WHERE   
					(LastActivityDate <= @InactiveSinceDate)
                        AND (
                                (@ProfileAuthOptions = 2)
                             OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                             OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                            )
            )

    SELECT  @@ROWCOUNT
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_GetNumberOfInactiveProfiles]
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    SELECT  COUNT(*)
    FROM    [<dbUser,varchar,dbo>].[subtext_Users] u
		INNER JOIN [<dbUser,varchar,dbo>].subtext_Profile p ON u.UserId = p.UserId
    WHERE   
        (LastActivityDate <= @InactiveSinceDate)
        AND (
                (@ProfileAuthOptions = 2)
                OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
            )
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_IsUserInRole]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(2)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    DECLARE @RoleId uniqueidentifier
    SELECT  @RoleId = NULL

    SELECT  @UserId = UserId
    FROM    [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE   LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN(2)

    SELECT  @RoleId = RoleId
    FROM    [<dbUser,varchar,dbo>].[subtext_Roles]
    WHERE   LoweredRoleName = LOWER(@RoleName) 
		AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
        RETURN(3)

    IF (EXISTS( SELECT * FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] WHERE UserId = @UserId AND RoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_GetRolesForUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256)
AS
BEGIN
	DECLARE @DefaultAppId uniqueidentifier
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    SELECT  @ApplicationId = ApplicationId 
	FROM [<dbUser,varchar,dbo>].[subtext_Applications ]
	WHERE LOWER(@ApplicationName) = LoweredApplicationName

	SELECT @DefaultAppId = ApplicationId
	FROM [<dbUser,varchar,dbo>].[subtext_Applications]
	WHERE LoweredApplicationName = "/"

    IF (@ApplicationId IS NULL)
        RETURN(1)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT  @UserId = UserId
    FROM    [<dbUser,varchar,dbo>].[subtext_Users]
    WHERE   LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN(1)

    SELECT r.RoleName
    FROM   [<dbUser,varchar,dbo>].[subtext_Roles] r
		INNER JOIN [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur ON r.RoleId = ur.RoleId
    WHERE  (r.ApplicationId = @ApplicationId AND ur.UserId = @UserId) OR
		(r.ApplicationId = @DefaultAppId AND ur.UserId = @UserID AND r.RoleName = "HostAdmins")
    ORDER BY r.RoleName
    RETURN (0)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Roles_DeleteRole]
    @ApplicationName            nvarchar(256),
    @RoleName                   nvarchar(256),
    @DeleteOnlyIfRoleIsEmpty    bit
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    DECLARE @RoleId   uniqueidentifier
    SELECT  @RoleId = NULL
    SELECT  @RoleId = RoleId FROM [<dbUser,varchar,dbo>].subtext_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
    BEGIN
        SELECT @ErrorCode = 1
        GOTO Cleanup
    END
    IF (@DeleteOnlyIfRoleIsEmpty <> 0)
    BEGIN
        IF (EXISTS (SELECT RoleId FROM [<dbUser,varchar,dbo>].subtext_UsersInRoles  WHERE @RoleId = RoleId))
        BEGIN
            SELECT @ErrorCode = 2
            GOTO Cleanup
        END
    END


    DELETE FROM [<dbUser,varchar,dbo>].subtext_UsersInRoles  WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DELETE FROM [<dbUser,varchar,dbo>].subtext_Roles WHERE @RoleId = RoleId  AND ApplicationId = @ApplicationId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Roles_RoleExists]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(0)
    IF (EXISTS (SELECT RoleName FROM [<dbUser,varchar,dbo>].subtext_Roles WHERE LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId ))
        RETURN(1)
    ELSE
        RETURN(0)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_AddUsersToRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000),
	@CurrentTimeUtc   datetime
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)
	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames	table(Name nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL PRIMARY KEY)
	DECLARE @tbRoles	table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers	table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num		int
	DECLARE @Pos		int
	DECLARE @NextPos	int
	DECLARE @Name		nvarchar(256)

	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   [<dbUser,varchar,dbo>].[subtext_Roles] r
		INNER JOIN @tbNames t ON LOWER(t.Name) = r.LoweredRoleName 
	  WHERE  r.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		SELECT TOP 1 Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN 
			(
				SELECT sr.LoweredRoleName 
				FROM [<dbUser,varchar,dbo>].[subtext_Roles] sr 
					INNER JOIN @tbRoles r ON r.RoleId = sr.RoleId
			)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END

	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1

	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   [<dbUser,varchar,dbo>].[subtext_Users] u 
		INNER JOIN @tbNames t 
			ON LOWER(t.Name) = u.LoweredUserName

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		DELETE FROM @tbNames
		WHERE LOWER(Name) IN 
		(
			SELECT LoweredUserName 
				FROM [<dbUser,varchar,dbo>].[subtext_Users] su 
					INNER JOIN @tbUsers u 
						ON su.UserId = u.UserId
		)

		INSERT [<dbUser,varchar,dbo>].[subtext_Users]
		(
			UserId
			, UserName
			, LoweredUserName
			, IsAnonymous
			, LastActivityDate
		)
		SELECT 
			NEWID()
			, [Name]
			, LOWER([Name])
			, 0
			, @CurrentTimeUtc
		  FROM   @tbNames

		INSERT INTO @tbUsers
		  SELECT  UserId
		  FROM	[<dbUser,varchar,dbo>].subtext_Users su
			INNER JOIN @tbNames t ON LOWER(t.Name) = su.LoweredUserName 
	END
	
	IF (EXISTS 
			(
				SELECT * 
				FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur
					INNER JOIN @tbUsers tu ON tu.UserId = ur.UserId
					INNER JOIN @tbRoles tr ON tr.RoleId = ur.RoleId
			)
	)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 [<dbUser,varchar,dbo>].subtext_UsersInRoles ur, @tbUsers tu, @tbRoles tr, subtext_Users u, subtext_Roles r
		WHERE		u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	INSERT INTO [<dbUser,varchar,dbo>].subtext_UsersInRoles (UserId, RoleId)
	SELECT UserId, RoleId
	FROM @tbUsers, @tbRoles

	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_RemoveUsersFromRoles]
	@ApplicationName  nvarchar(256)
	, @UserNames		  nvarchar(4000)
	, @RoleNames		  nvarchar(4000)
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)


	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames  table(Name nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL PRIMARY KEY)
	DECLARE @tbRoles  table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers  table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num	  int
	DECLARE @Pos	  int
	DECLARE @NextPos  int
	DECLARE @Name	  nvarchar(256)
	DECLARE @CountAll int
	DECLARE @CountU	  int
	DECLARE @CountR	  int


	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   [<dbUser,varchar,dbo>].subtext_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId
	SELECT @CountR = @@ROWCOUNT

	IF (@CountR <> @Num)
	BEGIN
		SELECT TOP 1 N'', Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM [<dbUser,varchar,dbo>].subtext_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END


	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1


	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   [<dbUser,varchar,dbo>].subtext_Users sr, @tbNames t
	  WHERE  LOWER(t.Name) = sr.LoweredUserName

	SELECT @CountU = @@ROWCOUNT
	IF (@CountU <> @Num)
	BEGIN
		SELECT TOP 1 Name, N''
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT au.LoweredUserName FROM [<dbUser,varchar,dbo>].subtext_Users au,  @tbUsers u WHERE u.UserId = au.UserId)

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(1)
	END

	SELECT  @CountAll = COUNT(*)
	FROM	[<dbUser,varchar,dbo>].subtext_UsersInRoles ur, @tbUsers u, @tbRoles r
	WHERE   ur.UserId = u.UserId AND ur.RoleId = r.RoleId

	IF (@CountAll <> @CountU * @CountR)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 @tbUsers tu, @tbRoles tr, [<dbUser,varchar,dbo>].subtext_Users u, [<dbUser,varchar,dbo>].subtext_Roles r
		WHERE		 u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND
					 tu.UserId NOT IN (SELECT ur.UserId FROM [<dbUser,varchar,dbo>].subtext_UsersInRoles ur WHERE ur.RoleId = tr.RoleId) AND
					 tr.RoleId NOT IN (SELECT ur.RoleId FROM [<dbUser,varchar,dbo>].subtext_UsersInRoles ur WHERE ur.UserId = tu.UserId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	DELETE FROM [<dbUser,varchar,dbo>].subtext_UsersInRoles
	WHERE UserId IN (SELECT UserId FROM @tbUsers)
	  AND RoleId IN (SELECT RoleId FROM @tbRoles)
	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Roles_GetAllRoles] (
    @ApplicationName           nvarchar(256))
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN
    SELECT RoleName
    FROM   [<dbUser,varchar,dbo>].subtext_Roles WHERE ApplicationId = @ApplicationId
    ORDER BY RoleName
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_GetUsersInRoles]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    [<dbUser,varchar,dbo>].subtext_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName 
		AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   [<dbUser,varchar,dbo>].subtext_Users u
		INNER JOIN [<dbUser,varchar,dbo>].subtext_UsersInRoles ur ON u.UserId = ur.UserId
    WHERE  @RoleId = ur.RoleId
    ORDER BY u.UserName
    RETURN(0)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UsersInRoles_FindUsersInRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256),
    @UserNameToMatch  nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    [<dbUser,varchar,dbo>].subtext_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   [<dbUser,varchar,dbo>].subtext_Users u
		INNER JOIN [<dbUser,varchar,dbo>].subtext_UsersInRoles ur ON u.UserId = ur.UserId
    WHERE  @RoleId = ur.RoleId AND LoweredUserName LIKE LOWER(@UserNameToMatch)
    ORDER BY u.UserName
    RETURN(0)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Paths_CreatePath]
    @ApplicationId UNIQUEIDENTIFIER,
    @Path           NVARCHAR(256),
    @PathId         UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    BEGIN TRANSACTION
    IF (NOT EXISTS(SELECT * FROM [<dbUser,varchar,dbo>].subtext_Paths WHERE LoweredPath = LOWER(@Path) AND ApplicationId = @ApplicationId))
    BEGIN
        INSERT [<dbUser,varchar,dbo>].subtext_Paths (ApplicationId, Path, LoweredPath) VALUES (@ApplicationId, @Path, LOWER(@Path))
    END
    COMMIT TRANSACTION
    SELECT @PathId = PathId FROM [<dbUser,varchar,dbo>].subtext_Paths WHERE LOWER(@Path) = LoweredPath AND ApplicationId = @ApplicationId
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_WebEvent_LogEvent]
        @EventId         char(32),
        @EventTimeUtc    datetime,
        @EventTime       datetime,
        @EventType       nvarchar(256),
        @EventSequence   decimal(19,0),
        @EventOccurrence decimal(19,0),
        @EventCode       int,
        @EventDetailCode int,
        @Message         nvarchar(1024),
        @ApplicationPath nvarchar(256),
        @ApplicationVirtualPath nvarchar(256),
        @MachineName    nvarchar(256),
        @RequestUrl      nvarchar(1024),
        @ExceptionType   nvarchar(256),
        @Details         ntext
AS
BEGIN
    INSERT
        [<dbUser,varchar,dbo>].subtext_WebEvent_Events
        (
            EventId,
            EventTimeUtc,
            EventTime,
            EventType,
            EventSequence,
            EventOccurrence,
            EventCode,
            EventDetailCode,
            Message,
            ApplicationPath,
            ApplicationVirtualPath,
            MachineName,
            RequestUrl,
            ExceptionType,
            Details
        )
    VALUES
    (
        @EventId,
        @EventTimeUtc,
        @EventTime,
        @EventType,
        @EventSequence,
        @EventOccurrence,
        @EventCode,
        @EventDetailCode,
        @Message,
        @ApplicationPath,
        @ApplicationVirtualPath,
        @MachineName,
        @RequestUrl,
        @ExceptionType,
        @Details
    )
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Personalization_GetApplicationId] (
    @ApplicationName NVARCHAR(256),
    @ApplicationId UNIQUEIDENTIFIER OUT)
AS
BEGIN
    SELECT @ApplicationId = ApplicationId FROM [<dbUser,varchar,dbo>].subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_GetProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @PageIndex              int,
    @PageSize               int,
    @UserNameToMatch        nvarchar(256) = NULL,
    @InactiveSinceDate      datetime      = NULL
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT  u.UserId
        FROM    [<dbUser,varchar,dbo>].subtext_Users u, [<dbUser,varchar,dbo>].subtext_Profile p
        WHERE   u.UserId = p.UserId
            AND (@InactiveSinceDate IS NULL OR LastActivityDate <= @InactiveSinceDate)
            AND (     (@ProfileAuthOptions = 2)
                   OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                   OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                 )
            AND (@UserNameToMatch IS NULL OR LoweredUserName LIKE LOWER(@UserNameToMatch))
        ORDER BY UserName

    SELECT  u.UserName, u.IsAnonymous, u.LastActivityDate, p.LastUpdatedDate,
            DATALENGTH(p.PropertyNames) + DATALENGTH(p.PropertyValuesString) + DATALENGTH(p.PropertyValuesBinary)
    FROM    [<dbUser,varchar,dbo>].subtext_Users u, [<dbUser,varchar,dbo>].subtext_Profile p, #PageIndexForUsers i
    WHERE   u.UserId = p.UserId AND p.UserId = i.UserId AND i.IndexId >= @PageLowerBound AND i.IndexId <= @PageUpperBound

    SELECT COUNT(*)
    FROM   #PageIndexForUsers

    DROP TABLE #PageIndexForUsers
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_FindUsersByNameOrEmail]
    @ApplicationName	nvarchar(256) = NULL
    , @UserNameToMatch  nvarchar(256) = NULL
    , @EmailToMatch	    nvarchar(256) = NULL
    , @PageIndex		int
    , @PageSize         int
    , @TotalCount		int output
AS
BEGIN
    DECLARE @FirstUserName nvarchar(256)
	DECLARE @StartRow int
	DECLARE @StartRowIndex int

	SET @StartRowIndex = @PageIndex * @PageSize + 1

	SET ROWCOUNT @StartRowIndex
	

	SELECT	@FirstUserName = UserName
		FROM [<dbUser,varchar,dbo>].[subtext_Users]
		WHERE
		( 
			(@UserNameToMatch IS NOT NULL AND UserName LIKE '%' + @UserNameToMatch + '%')
			OR
			(@EmailToMatch IS NOT NULL AND Email LIKE '%' + @EmailToMatch + '%')
		)
		AND
		(
			@ApplicationName IS NULL 
			OR
			UserId IN 
			(
				SELECT UserId 
				FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur
					INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r
						ON r.RoleId = ur.RoleId
					INNER JOIN [<dbUser,varchar,dbo>].[subtext_Applications] a
						ON r.ApplicationId = a.ApplicationId
				WHERE a.ApplicationName = @ApplicationName
			)
		)
	ORDER BY UserName ASC

	SET ROWCOUNT @PageSize

	SELECT  UserName
		, Email
		, PasswordQuestion
		, Comment
		, IsApproved
		, CreateDate
		, LastLoginDate
		, LastActivityDate
		, LastPasswordChangedDate
		, UserId
		, IsLockedOut
		, LastLockoutDate
	FROM   [<dbUser,varchar,dbo>].[subtext_Users]
	WHERE  UserName >= @FirstUserName
		AND
		(
			(@UserNameToMatch IS NOT NULL AND UserName LIKE '%' + @UserNameToMatch + '%')
		OR
			(@EmailToMatch IS NOT NULL AND Email LIKE '%' + @EmailToMatch + '%')
		)	
		AND 
		(
			@ApplicationName IS NULL 
			OR
			UserId IN 
			(
				SELECT UserId 
				FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur
					INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r
						ON r.RoleId = ur.RoleId
					INNER JOIN [<dbUser,varchar,dbo>].[subtext_Applications] a
						ON r.ApplicationId = a.ApplicationId
				WHERE a.ApplicationName = @ApplicationName
			)
		)
	ORDER BY UserName ASC


	SELECT @TotalCount = COUNT(1)
	FROM [<dbUser,varchar,dbo>].[subtext_Users]
	WHERE 	(@UserNameToMatch IS NOT NULL AND UserName LIKE '%' + @UserNameToMatch + '%')
		OR
			(@EmailToMatch IS NOT NULL AND Email LIKE '%' + @EmailToMatch + '%')
		AND
		(
			@ApplicationName IS NULL 
			OR
			UserId IN 
			(
				SELECT UserId 
				FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur
					INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r
						ON r.RoleId = ur.RoleId
					INNER JOIN [<dbUser,varchar,dbo>].[subtext_Applications] a
						ON r.ApplicationId = a.ApplicationId
				WHERE a.ApplicationName = @ApplicationName
			)
		)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_GetAllUsers]
    @ApplicationName  nvarchar(256) = NULL
    , @PageIndex		int
    , @PageSize		int
    , @TotalCount	int output
AS
BEGIN
	DECLARE @FirstUserName nvarchar(256)
	DECLARE @StartRow int
	DECLARE @StartRowIndex int
	DECLARE @ApplicationId UniqueIdentifier

	SET @StartRowIndex = @PageIndex * @PageSize + 1
	SET @ApplicationId = NULL
	SET ROWCOUNT @StartRowIndex
	
	IF (NOT @ApplicationName IS NULL)
		SELECT @ApplicationId = ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Applications] WHERE ApplicationName = @ApplicationName

	SELECT	@FirstUserName = UserName
	FROM [<dbUser,varchar,dbo>].[subtext_Users]
	WHERE
		@ApplicationName IS NULL 
		OR
		UserId IN 
		(
			SELECT UserId 
			FROM [<dbUser,varchar,dbo>].[subtext_Users] us
			WHERE UserId IN (
				SELECT UserId
				FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur
					INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r
						ON r.RoleId = ur.RoleId
				WHERE r.ApplicationId = @ApplicationId)
			OR UserId IN (
				SELECT OwnerId FROM [<dbUser,varchar,dbo>].[subtext_Config] WHERE ApplicationId = @ApplicationId)
		)
	ORDER BY UserName ASC
	
	SET ROWCOUNT @PageSize

	
	SELECT  UserName
		, Email
		, PasswordQuestion
		, Comment
		, IsApproved
		, CreateDate
		, LastLoginDate
		, LastActivityDate
		, LastPasswordChangedDate
		, UserId
		, IsLockedOut
		, LastLockoutDate
	FROM   [<dbUser,varchar,dbo>].[subtext_Users]
	WHERE   UserName >= @FirstUserName
		AND 
		(
			@ApplicationName IS NULL 
			OR
			UserId IN 
			(
				SELECT UserId 
				FROM [<dbUser,varchar,dbo>].[subtext_Users] us
				WHERE UserId IN (
					SELECT UserId
					FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur
						INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r
							ON r.RoleId = ur.RoleId
					WHERE r.ApplicationId = @ApplicationId)
				OR UserId IN (
					SELECT OwnerId FROM [<dbUser,varchar,dbo>].[subtext_Config] WHERE ApplicationId = @ApplicationId)
			)
		)
	ORDER BY UserName ASC

	SELECT @TotalCount = COUNT(1)
	FROM   [<dbUser,varchar,dbo>].[subtext_Users]
	WHERE
		@ApplicationName IS NULL 
		OR
		UserId IN 
		(
			SELECT UserId 
			FROM [<dbUser,varchar,dbo>].[subtext_Users] us
			WHERE UserId IN (
				SELECT UserId
				FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles] ur
					INNER JOIN [<dbUser,varchar,dbo>].[subtext_Roles] r
						ON r.RoleId = ur.RoleId
				WHERE r.ApplicationId = @ApplicationId)
			OR UserId IN (
				SELECT OwnerId FROM [<dbUser,varchar,dbo>].[subtext_Config] WHERE ApplicationId = @ApplicationId)
		)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Users_DeleteUser]
    @UserName         nvarchar(256),
    @TablesToDeleteFrom int,
    @NumTablesDeletedFrom int OUTPUT
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @NumTablesDeletedFrom = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
		SET @TranStarted = 0

    DECLARE @ErrorCode   int
    DECLARE @RowCount    int

    SET @ErrorCode = 0
    SET @RowCount  = 0

    SELECT  @UserId = UserId
    FROM    [<dbUser,varchar,dbo>].subtext_Users
    WHERE   LoweredUserName       = LOWER(@UserName)

    IF (@UserId IS NULL)
    BEGIN
        GOTO Cleanup
    END

    -- Delete from subtext_UsersInRoles table if (@TablesToDeleteFrom & 2) is set
    IF ((@TablesToDeleteFrom & 2) <> 0)
    BEGIN
        DELETE FROM [<dbUser,varchar,dbo>].subtext_UsersInRoles WHERE UserId = @UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from subtext_Profile table if (@TablesToDeleteFrom & 4) is set
    IF ((@TablesToDeleteFrom & 4) <> 0)
    BEGIN
        DELETE FROM [<dbUser,varchar,dbo>].subtext_Profile WHERE UserId = @UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from subtext_PersonalizationPerUser table if (@TablesToDeleteFrom & 8) is set
    IF ((@TablesToDeleteFrom & 8) <> 0)
    BEGIN
        DELETE FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser WHERE UserId = @UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from subtext_Users table if (@TablesToDeleteFrom & 1,2,4 & 8) are all set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (@TablesToDeleteFrom & 2) <> 0 AND
        (@TablesToDeleteFrom & 4) <> 0 AND
        (@TablesToDeleteFrom & 8) <> 0 AND
        (EXISTS (SELECT UserId FROM [<dbUser,varchar,dbo>].subtext_Users WHERE UserId = @UserId)))
    BEGIN
        DELETE FROM [<dbUser,varchar,dbo>].subtext_Users WHERE UserId = @UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:
    SET @NumTablesDeletedFrom = 0

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
	    ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Applications_CreateApplication]
    @ApplicationName      nvarchar(256),
    @ApplicationId        uniqueidentifier OUTPUT
AS
BEGIN
    SELECT  @ApplicationId = ApplicationId FROM [<dbUser,varchar,dbo>].subtext_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName

    IF(@ApplicationId IS NULL)
    BEGIN
        DECLARE @TranStarted   bit
        SET @TranStarted = 0

        IF( @@TRANCOUNT = 0 )
        BEGIN
	        BEGIN TRANSACTION
	        SET @TranStarted = 1
        END
        ELSE
    	    SET @TranStarted = 0

        SELECT  @ApplicationId = ApplicationId
        FROM [<dbUser,varchar,dbo>].subtext_Applications WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(@ApplicationName) = LoweredApplicationName

        IF(@ApplicationId IS NULL)
        BEGIN
            SELECT  @ApplicationId = NEWID()
            INSERT  [<dbUser,varchar,dbo>].subtext_Applications (ApplicationId, ApplicationName, LoweredApplicationName)
            VALUES  (@ApplicationId, @ApplicationName, LOWER(@ApplicationName))
        END


        IF( @TranStarted = 1 )
        BEGIN
            IF(@@ERROR = 0)
            BEGIN
	        SET @TranStarted = 0
	        COMMIT TRANSACTION
            END
            ELSE
            BEGIN
                SET @TranStarted = 0
                ROLLBACK TRANSACTION
            END
        END
    END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_AnyDataInTables]
    @TablesToCheck int
AS
BEGIN
    -- Check subtext_Roles table if (@TablesToCheck & 2) is set
    IF ((@TablesToCheck & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_subtext_Roles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 RoleId FROM [<dbUser,varchar,dbo>].subtext_Roles))
        BEGIN
            SELECT N'subtext_Roles'
            RETURN
        END
    END

    -- Check subtext_Profile table if (@TablesToCheck & 4) is set
    IF ((@TablesToCheck & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_subtext_Profiles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM [<dbUser,varchar,dbo>].subtext_Profile))
        BEGIN
            SELECT N'subtext_Profile'
            RETURN
        END
    END

    -- Check subtext_PersonalizationPerUser table if (@TablesToCheck & 8) is set
    IF ((@TablesToCheck & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_subtext_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser))
        BEGIN
            SELECT N'subtext_PersonalizationPerUser'
            RETURN
        END
    END

    -- Check subtext_PersonalizationPerUser table if (@TablesToCheck & 16) is set
    IF ((@TablesToCheck & 16) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'subtext_WebEvent_LogEvent') AND (type = 'P'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 * FROM [<dbUser,varchar,dbo>].subtext_WebEvent_Events))
        BEGIN
            SELECT N'subtext_WebEvent_Events'
            RETURN
        END
    END

    -- Check subtext_Users table if (@TablesToCheck & 1,2,4 & 8) are all set
    IF ((@TablesToCheck & 1) <> 0 AND
        (@TablesToCheck & 2) <> 0 AND
        (@TablesToCheck & 4) <> 0 AND
        (@TablesToCheck & 8) <> 0 AND
        (@TablesToCheck & 32) <> 0 AND
        (@TablesToCheck & 128) <> 0 AND
        (@TablesToCheck & 256) <> 0 AND
        (@TablesToCheck & 512) <> 0 AND
        (@TablesToCheck & 1024) <> 0)
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM [<dbUser,varchar,dbo>].subtext_Users))
        BEGIN
            SELECT N'subtext_Users'
            RETURN
        END
        IF (EXISTS(SELECT TOP 1 ApplicationId FROM [<dbUser,varchar,dbo>].subtext_Applications))
        BEGIN
            SELECT N'subtext_Applications'
            RETURN
        END
    END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC [<dbUser,varchar,dbo>].subtext_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM [<dbUser,varchar,dbo>].subtext_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC [<dbUser,varchar,dbo>].subtext_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    IF (EXISTS(SELECT PathId FROM [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers WHERE PathId = @PathId))
        UPDATE [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE PathId = @PathId
    ELSE
        INSERT INTO [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers(PathId, PageSettings, LastUpdatedDate) VALUES (@PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Roles_CreateRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    EXEC [<dbUser,varchar,dbo>].subtext_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS(SELECT RoleId FROM [<dbUser,varchar,dbo>].subtext_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId))
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    INSERT INTO [<dbUser,varchar,dbo>].subtext_Roles
                (ApplicationId, RoleName, LoweredRoleName)
         VALUES (@ApplicationId, @RoleName, LOWER(@RoleName))

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_SetProperties]
    @ApplicationName        nvarchar(256),
    @PropertyNames          ntext,
    @PropertyValuesString   ntext,
    @PropertyValuesBinary   image,
    @UserName               nvarchar(256),
    @IsUserAnonymous        bit,
    @CurrentTimeUtc         datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
       BEGIN TRANSACTION
       SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC [<dbUser,varchar,dbo>].subtext_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DECLARE @UserId uniqueidentifier
    DECLARE @LastActivityDate datetime
    SELECT  @UserId = NULL
    SELECT  @LastActivityDate = @CurrentTimeUtc

    SELECT @UserId = UserId
    FROM   [<dbUser,varchar,dbo>].subtext_Users
    WHERE  LoweredUserName = LOWER(@UserName)
    
    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    UPDATE [<dbUser,varchar,dbo>].subtext_Users
    SET    LastActivityDate=@CurrentTimeUtc
    WHERE  UserId = @UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS( SELECT *
               FROM   [<dbUser,varchar,dbo>].[subtext_Profile]
               WHERE  UserId = @UserId))
        UPDATE [<dbUser,varchar,dbo>].[subtext_Profile]
        SET    PropertyNames=@PropertyNames
			, PropertyValuesString = @PropertyValuesString
			, PropertyValuesBinary = @PropertyValuesBinary
			, LastUpdatedDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    ELSE
        INSERT INTO [<dbUser,varchar,dbo>].subtext_Profile
        (
			UserId
			, PropertyNames
			, PropertyValuesString
			, PropertyValuesBinary
			, LastUpdatedDate
		)
        VALUES 
        (
			@UserId
			, @PropertyNames
			, @PropertyValuesString
			, @PropertyValuesBinary
			, @CurrentTimeUtc
		)

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Membership_CreateUser]
    @UserName                               nvarchar(256),
    @Password                               nvarchar(128) = NULL,
    @PasswordSalt                           nvarchar(128) = NULL,
    @Email                                  nvarchar(256) = NULL,
    @PasswordQuestion                       nvarchar(256) = NULL,
    @PasswordAnswer                         nvarchar(128) = NULL,
    @IsApproved                             bit,
    @CurrentTimeUtc                         datetime,
    @CreateDate                             datetime = NULL,
    @UniqueEmail                            bit      = 0,
    @PasswordFormat                         int      = 0,
    @UserId                                 uniqueidentifier OUTPUT
AS
BEGIN
    DECLARE @NewUserId uniqueidentifier
    SELECT @NewUserId = NULL

    DECLARE @IsLockedOut bit
    SET @IsLockedOut = 0

    DECLARE @LastLockoutDate  datetime
    SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAttemptCount int
    SET @FailedPasswordAttemptCount = 0

    DECLARE @FailedPasswordAttemptWindowStart  datetime
    SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAnswerAttemptCount int
    SET @FailedPasswordAnswerAttemptCount = 0

    DECLARE @FailedPasswordAnswerAttemptWindowStart  datetime
    SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @NewUserCreated bit
    DECLARE @ReturnValue   int
    SET @ReturnValue = 0

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SET @CreateDate = @CurrentTimeUtc

    SELECT  @NewUserId = UserId FROM [<dbUser,varchar,dbo>].[subtext_Users] WHERE LOWER(@UserName) = LoweredUserName
    IF ( NOT @NewUserId IS NULL )
    BEGIN
        SET @NewUserCreated = 0
        IF( @NewUserId <> @UserId AND @UserId IS NOT NULL )
        BEGIN
            SET @ErrorCode = 6
            GOTO Cleanup
        END
    END

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @ReturnValue = -1 )
    BEGIN
        SET @ErrorCode = 10
        GOTO Cleanup
    END
	
	SELECT @NewUserId = NEWID()
	
	IF ( EXISTS ( SELECT UserId
                  FROM   [<dbUser,varchar,dbo>].[subtext_Users]
                  WHERE  @NewUserId = UserId ) )
    BEGIN
		-- This should never happen.
        SET @ErrorCode = 6
        GOTO Cleanup
    END

    SET @UserId = @NewUserId

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  [<dbUser,varchar,dbo>].[subtext_Users] WITH ( UPDLOCK, HOLDLOCK )
                    WHERE LoweredEmail = LOWER(@Email)))
        BEGIN
            SET @ErrorCode = 7
            GOTO Cleanup
        END
    END


    INSERT INTO [<dbUser,varchar,dbo>].[subtext_Users]
	( 
		UserId
		, UserName
		, LoweredUserName
		, IsAnonymous
		, Password
		, PasswordSalt
		, Email
		, LoweredEmail
		, PasswordQuestion
		, PasswordAnswer
		, PasswordFormat
		, IsApproved
		, IsLockedOut
		, CreateDate
		, LastLoginDate
		, LastPasswordChangedDate
		, LastActivityDate
		, LastLockoutDate
		, FailedPasswordAttemptCount
		, FailedPasswordAttemptWindowStart
		, FailedPasswordAnswerAttemptCount
		, FailedPasswordAnswerAttemptWindowStart 
	)
	VALUES 
	( 
		@UserId
		, @UserName
		, LOWER(@UserName)
		, 0 -- IsAnonymous
		, @Password
		, @PasswordSalt
		, @Email
		, LOWER(@Email)
		, @PasswordQuestion
		, @PasswordAnswer
		, @PasswordFormat
		, @IsApproved
		, @IsLockedOut
		, @CreateDate
		, @CreateDate
		, @CreateDate
		, @CurrentTimeUtc -- LastActivityDate
		, @LastLockoutDate
		, @FailedPasswordAttemptCount
		, @FailedPasswordAttemptWindowStart
		, @FailedPasswordAnswerAttemptCount
		, @FailedPasswordAnswerAttemptWindowStart 
	)

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC [<dbUser,varchar,dbo>].subtext_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM [<dbUser,varchar,dbo>].subtext_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC [<dbUser,varchar,dbo>].subtext_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    SELECT @UserId = UserId FROM [<dbUser,varchar,dbo>].[subtext_Users] WHERE LoweredUserName = LOWER(@UserName)
    
    UPDATE   [<dbUser,varchar,dbo>].subtext_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    IF (EXISTS(SELECT PathId FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser WHERE UserId = @UserId AND PathId = @PathId))
        UPDATE [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE UserId = @UserId AND PathId = @PathId
    ELSE
        INSERT INTO [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser(UserId, PathId, PageSettings, LastUpdatedDate) VALUES (@UserId, @PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_Profile_DeleteProfiles]
    @UserNames              nvarchar(4000)
AS
BEGIN
    DECLARE @UserName     nvarchar(256)
    DECLARE @CurrentPos   int
    DECLARE @NextPos      int
    DECLARE @NumDeleted   int
    DECLARE @DeletedUser  int
    DECLARE @TranStarted  bit
    DECLARE @ErrorCode    int

    SET @ErrorCode = 0
    SET @CurrentPos = 1
    SET @NumDeleted = 0
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    WHILE (@CurrentPos <= LEN(@UserNames))
    BEGIN
        SELECT @NextPos = CHARINDEX(N',', @UserNames,  @CurrentPos)
        IF (@NextPos = 0 OR @NextPos IS NULL)
            SELECT @NextPos = LEN(@UserNames) + 1

        SELECT @UserName = SUBSTRING(@UserNames, @CurrentPos, @NextPos - @CurrentPos)
        SELECT @CurrentPos = @NextPos+1

        IF (LEN(@UserName) > 0)
        BEGIN
            SELECT @DeletedUser = 0
            EXEC [<dbUser,varchar,dbo>].[subtext_Users_DeleteUser] @UserName, 4, @DeletedUser OUTPUT
            IF( @@ERROR <> 0 )
            BEGIN
                SET @ErrorCode = -1
                GOTO Cleanup
            END
            IF (@DeletedUser <> 0)
                SELECT @NumDeleted = @NumDeleted + 1
        END
    END
    SELECT @NumDeleted
    IF (@TranStarted = 1)
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END
    SET @TranStarted = 0

    RETURN 0

Cleanup:
    IF (@TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END
    RETURN @ErrorCode
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC [<dbUser,varchar,dbo>].[subtext_Personalization_GetApplicationId] @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM [<dbUser,varchar,dbo>].[subtext_Paths] u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT p.PageSettings FROM [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers p WHERE p.PathId = @PathId
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC [<dbUser,varchar,dbo>].subtext_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM [<dbUser,varchar,dbo>].subtext_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    DELETE FROM [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers WHERE PathId = @PathId
    RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC [<dbUser,varchar,dbo>].subtext_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM [<dbUser,varchar,dbo>].subtext_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = UserId FROM [<dbUser,varchar,dbo>].subtext_Users WHERE LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   [<dbUser,varchar,dbo>].subtext_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    SELECT p.PageSettings FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser p WHERE p.PathId = @PathId AND p.UserId = @UserId
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC [<dbUser,varchar,dbo>].subtext_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM [<dbUser,varchar,dbo>].subtext_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = UserId FROM [<dbUser,varchar,dbo>].subtext_Users WHERE LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   [<dbUser,varchar,dbo>].subtext_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    DELETE FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser WHERE PathId = @PathId AND UserId = @UserId
    RETURN 0
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_DeleteAllState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Count int OUT)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC [<dbUser,varchar,dbo>].subtext_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        IF (@AllUsersScope = 1)
            DELETE FROM subtext_PersonalizationAllUsers
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM [<dbUser,varchar,dbo>].subtext_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)
        ELSE
            DELETE FROM subtext_PersonalizationPerUser
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM [<dbUser,varchar,dbo>].subtext_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)

        SELECT @Count = @@ROWCOUNT
    END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_ResetSharedState] (
    @Count int OUT,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC [<dbUser,varchar,dbo>].subtext_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers
        WHERE PathId IN
            (SELECT AllUsers.PathId
             FROM [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers AllUsers, [<dbUser,varchar,dbo>].subtext_Paths Paths
             WHERE Paths.ApplicationId = @ApplicationId
                   AND AllUsers.PathId = Paths.PathId
                   AND Paths.LoweredPath = LOWER(@Path))

        SELECT @Count = @@ROWCOUNT
    END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_ResetUserState] (
    @Count                  int                 OUT,
    @ApplicationName        NVARCHAR(256),
    @InactiveSinceDate      DATETIME            = NULL,
    @UserName               NVARCHAR(256)       = NULL,
    @Path                   NVARCHAR(256)       = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC [<dbUser,varchar,dbo>].subtext_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser
        WHERE Id IN (SELECT PerUser.Id
                     FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser PerUser, [<dbUser,varchar,dbo>].subtext_Users Users, [<dbUser,varchar,dbo>].subtext_Paths Paths
                     WHERE Paths.ApplicationId = @ApplicationId
                           AND PerUser.UserId = Users.UserId
                           AND PerUser.PathId = Paths.PathId
                           AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
                           AND (@UserName IS NULL OR Users.LoweredUserName = LOWER(@UserName))
                           AND (@Path IS NULL OR Paths.LoweredPath = LOWER(@Path)))

        SELECT @Count = @@ROWCOUNT
    END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_GetCountOfState] (
    @Count int OUT,
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN

    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC [<dbUser,varchar,dbo>].subtext_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
        IF (@AllUsersScope = 1)
            SELECT @Count = COUNT(*)
            FROM [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers AllUsers, [<dbUser,varchar,dbo>].subtext_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND AllUsers.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
        ELSE
            SELECT @Count = COUNT(*)
            FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser PerUser, [<dbUser,varchar,dbo>].subtext_Users Users, [<dbUser,varchar,dbo>].subtext_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND PerUser.UserId = Users.UserId
                  AND PerUser.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
                  AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
                  AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_FindState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @PageIndex              INT,
    @PageSize               INT,
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC [<dbUser,varchar,dbo>].subtext_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound INT
    DECLARE @PageUpperBound INT
    DECLARE @TotalRecords   INT
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table to store the selected results
    CREATE TABLE #PageIndex (
        IndexId int IDENTITY (0, 1) NOT NULL,
        ItemId UNIQUEIDENTIFIER
    )

    IF (@AllUsersScope = 1)
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT Paths.PathId
        FROM [<dbUser,varchar,dbo>].subtext_Paths Paths,
             ((SELECT Paths.PathId
               FROM [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers AllUsers, [<dbUser,varchar,dbo>].subtext_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND AllUsers.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT DISTINCT Paths.PathId
               FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser PerUser, [<dbUser,varchar,dbo>].subtext_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND PerUser.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path,
               SharedDataPerPath.LastUpdatedDate,
               SharedDataPerPath.SharedDataLength,
               UserDataPerPath.UserDataLength,
               UserDataPerPath.UserCount
        FROM [<dbUser,varchar,dbo>].subtext_Paths Paths,
             ((SELECT PageIndex.ItemId AS PathId,
                      AllUsers.LastUpdatedDate AS LastUpdatedDate,
                      DATALENGTH(AllUsers.PageSettings) AS SharedDataLength
               FROM [<dbUser,varchar,dbo>].subtext_PersonalizationAllUsers AllUsers, #PageIndex PageIndex
               WHERE AllUsers.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT PageIndex.ItemId AS PathId,
                      SUM(DATALENGTH(PerUser.PageSettings)) AS UserDataLength,
                      COUNT(*) AS UserCount
               FROM subtext_PersonalizationPerUser PerUser, #PageIndex PageIndex
               WHERE PerUser.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
               GROUP BY PageIndex.ItemId
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC
    END
    ELSE
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT PerUser.Id
        FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser PerUser, [<dbUser,varchar,dbo>].subtext_Users Users, [<dbUser,varchar,dbo>].subtext_Paths Paths
        WHERE Paths.ApplicationId = @ApplicationId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
              AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
        ORDER BY Paths.Path ASC, Users.UserName ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path, PerUser.LastUpdatedDate, DATALENGTH(PerUser.PageSettings), Users.UserName, Users.LastActivityDate
        FROM [<dbUser,varchar,dbo>].subtext_PersonalizationPerUser PerUser, [<dbUser,varchar,dbo>].subtext_Users Users, [<dbUser,varchar,dbo>].subtext_Paths Paths, #PageIndex PageIndex
        WHERE PerUser.Id = PageIndex.ItemId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
        ORDER BY Paths.Path ASC, Users.UserName ASC
    END

    RETURN @TotalRecords
END

GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_RegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CheckSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UnRegisterSchemaVersion] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByName] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByName] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByUserId] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByUserId] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByEmail] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetUserByEmail] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetPasswordWithFormat] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_UpdateUserInfo] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetPassword] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetNumberOfUsersOnline] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetNumberOfUsersOnline] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_SetPassword] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_ResetPassword] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_UnlockUser] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_UpdateUser] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_ChangePasswordQuestionAndAnswer] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Profile_GetProperties] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Profile_DeleteInactiveProfiles] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Profile_GetNumberOfInactiveProfiles] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UsersInRoles_IsUserInRole] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UsersInRoles_IsUserInRole] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UsersInRoles_GetRolesForUser] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UsersInRoles_GetRolesForUser] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Roles_DeleteRole] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Roles_RoleExists] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UsersInRoles_AddUsersToRoles] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UsersInRoles_RemoveUsersFromRoles] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Roles_GetAllRoles] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UsersInRoles_GetUsersInRoles] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UsersInRoles_FindUsersInRole] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Paths_CreatePath] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_WebEvent_LogEvent] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Personalization_GetApplicationId] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Profile_GetProfiles] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_FindUsersByNameOrEmail] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Membership_GetAllUsers] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Users_DeleteUser] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_SetPageSettings] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Roles_CreateRole] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Profile_SetProperties] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_SetPageSettings] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_Profile_DeleteProfiles] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_GetPageSettings] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers_ResetPageSettings] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_GetPageSettings] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser_ResetPageSettings] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_DeleteAllState] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_ResetSharedState] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_ResetUserState] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_GetCountOfState] TO [public]
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_PersonalizationAdministration_FindState] TO [public]

/* Views */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


  CREATE VIEW [<dbUser,varchar,dbo>].[vw_subtext_Profiles]
  AS SELECT [<dbUser,varchar,dbo>].[subtext_Profile].[UserId], [<dbUser,varchar,dbo>].[subtext_Profile].[LastUpdatedDate],
      [DataSize]=  DATALENGTH([<dbUser,varchar,dbo>].[subtext_Profile].[PropertyNames])
                 + DATALENGTH([<dbUser,varchar,dbo>].[subtext_Profile].[PropertyValuesString])
                 + DATALENGTH([<dbUser,varchar,dbo>].[subtext_Profile].[PropertyValuesBinary])
  FROM [<dbUser,varchar,dbo>].[subtext_Profile]
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [<dbUser,varchar,dbo>].[vw_subtext_Roles]
  AS SELECT [<dbUser,varchar,dbo>].[subtext_Roles].[ApplicationId], [<dbUser,varchar,dbo>].[subtext_Roles].[RoleId], [<dbUser,varchar,dbo>].[subtext_Roles].[RoleName], [<dbUser,varchar,dbo>].[subtext_Roles].[LoweredRoleName], [<dbUser,varchar,dbo>].[subtext_Roles].[Description]
  FROM [<dbUser,varchar,dbo>].[subtext_Roles]
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [<dbUser,varchar,dbo>].[vw_subtext_UsersInRoles]
  AS SELECT [<dbUser,varchar,dbo>].[subtext_UsersInRoles].[UserId], [<dbUser,varchar,dbo>].[subtext_UsersInRoles].[RoleId]
  FROM [<dbUser,varchar,dbo>].[subtext_UsersInRoles]
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Paths]
  AS SELECT [<dbUser,varchar,dbo>].[subtext_Paths].[ApplicationId], [<dbUser,varchar,dbo>].[subtext_Paths].[PathId], [<dbUser,varchar,dbo>].[subtext_Paths].[Path], [<dbUser,varchar,dbo>].[subtext_Paths].[LoweredPath]
  FROM [<dbUser,varchar,dbo>].[subtext_Paths]
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Shared]
  AS SELECT [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers].[PathId], [DataSize]=DATALENGTH([<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers].[PageSettings]), [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers].[LastUpdatedDate]
  FROM [<dbUser,varchar,dbo>].[subtext_PersonalizationAllUsers]
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_User]
  AS SELECT [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser].[PathId], [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser].[UserId], [DataSize]=DATALENGTH([<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser].[PageSettings]), [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser].[LastUpdatedDate]
  FROM [<dbUser,varchar,dbo>].[subtext_PersonalizationPerUser]
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [<dbUser,varchar,dbo>].[vw_subtext_Applications]
  AS SELECT [<dbUser,varchar,dbo>].[subtext_Applications].[ApplicationName], [<dbUser,varchar,dbo>].[subtext_Applications].[LoweredApplicationName], [<dbUser,varchar,dbo>].[subtext_Applications].[ApplicationId], [<dbUser,varchar,dbo>].[subtext_Applications].[Description]
  FROM [<dbUser,varchar,dbo>].[subtext_Applications]
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

/* permission grants for VIEWS */
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Profiles] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Profiles] ([UserId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Profiles] ([LastUpdatedDate]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Profiles] ([DataSize]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Roles] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Roles] ([ApplicationId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Roles] ([RoleId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Roles] ([RoleName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Roles] ([LoweredRoleName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Roles] ([Description]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_UsersInRoles] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_UsersInRoles] ([UserId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_UsersInRoles] ([RoleId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Paths] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Paths] ([ApplicationId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Paths] ([PathId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Paths] ([Path]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Paths] ([LoweredPath]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Shared] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Shared] ([PathId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Shared] ([DataSize]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_Shared] ([LastUpdatedDate]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_User] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_User] ([PathId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_User] ([UserId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_User] ([DataSize]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_WebPartState_User] ([LastUpdatedDate]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([ApplicationName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([ApplicationName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([ApplicationName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([ApplicationName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([LoweredApplicationName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([LoweredApplicationName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([LoweredApplicationName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([LoweredApplicationName]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([ApplicationId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([ApplicationId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([ApplicationId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([ApplicationId]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([Description]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([Description]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([Description]) TO [public]
GO
GRANT SELECT ON [<dbUser,varchar,dbo>].[vw_subtext_Applications] ([Description]) TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UTILITY_AddBlog]
(
	@Title nvarchar(100)
	, @Host nvarchar(50)
	, @Subfolder nvarchar(50)
	, @OwnerID uniqueidentifier
	, @CurrentTimeUtc DateTime
)

AS

IF NOT EXISTS(SELECT * FROM [<dbUser,varchar,dbo>].[subtext_config] WHERE Host = @Host AND Subfolder = @Subfolder)
BEGIN
	
	DECLARE @ApplicationId UNIQUEIDENTIFIER
	DECLARE @ApplicationName nvarchar(256)
	SET @ApplicationName = @Host + '/' + @Subfolder
	SET @ApplicationId = newid()

	/* Create the Membership Application for this blog */
	INSERT [<dbUser,varchar,dbo>].[subtext_Applications]
		SELECT @ApplicationName, LOWER(@ApplicationName), @ApplicationId, 'New Blog'
	
	DECLARE @CreateDate DateTime
	SELECT @CreateDate = getdate()
	
	/* Create this blog */
	INSERT subtext_Config  
	(
		LastUpdated
		, ApplicationId
		, OwnerId
		, Title
		, SubTitle
		, Skin
		, SkinCssFile
		, Subfolder
		, Host
		, TimeZone
		, [Language]
		, ItemCount
		, Flag
	)
	Values             
	(
		getdate()
		, @ApplicationId
		, @OwnerID
		, @Title
		, 'Another Subtext Powered Blog'
		, 'RedBook'
		, 'blue.css'
		, @Subfolder
		, @Host
		, -1188006249
		,'en-US'
		, 10
		, 55 -- Flag
	)

	DECLARE @newBlogId int
	SET @newBlogId = SCOPE_IDENTITY()

	EXEC subtext_GetBlogById @newBlogId
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_UTILITY_AddBlog]  TO [public]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_CreateHost]
(
	@OwnerId uniqueidentifier
)
AS

IF NOT EXISTS(SELECT * FROM [<dbUser,varchar,dbo>].[subtext_Host])
BEGIN
	/* Create the Membership Application for this blog */
	DECLARE @ApplicationId UNIQUEIDENTIFIER
	
	SELECT @ApplicationId = ApplicationId FROM [<dbUser,varchar,dbo>].[subtext_Applications] WHERE ApplicationName = '/'
	IF(@ApplicationID IS NULL)
	BEGIN
		EXEC subtext_Applications_CreateApplication '/', @ApplicationId OUTPUT
	END
	
	DECLARE @CreateDate DateTime
	SELECT @CreateDate = getdate()

	/* Create the host record */
	INSERT [<dbUser,varchar,dbo>].[subtext_Host]
	(
		ApplicationId
		, OwnerId
		, DateCreated
	)
	Values             
	(
		@ApplicationId
		, @OwnerId
		, @CreateDate
	)
	
	EXEC [<dbUser,varchar,dbo>].[subtext_GetHost]
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [<dbUser,varchar,dbo>].[subtext_CreateHost]  TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_InsertMetaTag] 
	(
		@Content nvarchar(512),
		@Name nvarchar(100) = NULL,
		@HttpEquiv nvarchar(100) = NULL,
		@BlogId int,
		@EntryId int = NULL,
		@DateCreated datetime = NULL,
		@Id int OUTPUT
	)
AS
	IF @DateCreated IS NULL 
		SET @DateCreated = getdate()
		
	IF LEN(@Name) = 0
		SET @Name = NULL
	IF LEN(@HttpEquiv) = 0
		SET @HttpEquiv = NULL

	INSERT INTO [<dbUser,varchar,dbo>].subtext_MetaTag
		([Content], [Name], HttpEquiv, BlogId, EntryId, DateCreated)
	VALUES
		(@Content, @Name, @HttpEquiv, @BlogId, @EntryId, @DateCreated)

	SELECT @Id = SCOPE_IDENTITY()

GO 

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_InsertMetaTag] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateMetaTag] 
	(
		@Id int,
		@Content nvarchar(512),
		@Name nvarchar(100) = NULL,
		@HttpEquiv nvarchar(100) = NULL,
		@BlogId int,
		@EntryId int = NULL
	)
AS
		
	IF LEN(RTRIM(LTRIM(@Name))) = 0
		SET @Name = NULL
	IF LEN(RTRIM(LTRIM(@HttpEquiv))) = 0
		SET @HttpEquiv = NULL

	UPDATE [<dbUser,varchar,dbo>].subtext_MetaTag
	SET
		[Content] = @Content,
		[Name] = @Name,
		HttpEquiv = @HttpEquiv,
		BlogId = @BlogId,
		EntryId = @EntryId
	WHERE
		[Id] = @Id

GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UpdateMetaTag] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetMetaTagsForBlog] 
	(
		@BlogId int
	)
AS
	SELECT Id, [Content], [Name], HttpEquiv, BlogId, EntryId, DateCreated FROM [<dbUser,varchar,dbo>].subtext_MetaTag
	WHERE BlogId = @BlogId
	ORDER BY DateCreated DESC
GO 

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetMetaTagsForBlog] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetMetaTagsForEntry] 
	(
		@BlogId int,
		@EntryId int
	)
AS
	SELECT Id, [Content], [Name], HttpEquiv, BlogId, EntryId, DateCreated FROM [<dbUser,varchar,dbo>].subtext_MetaTag
	WHERE BlogId = @BlogId
		AND EntryId = @EntryId
	ORDER BY DateCreated DESC
GO 

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetMetaTagsForEntry] TO [public]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteMetaTag] 
	(
		@Id int
	)
AS
	DELETE FROM [<dbUser,varchar,dbo>].[subtext_MetaTag] WHERE [Id] = @Id

GO

GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetBlogByDomainAlias]
	(
		  @Host	VARCHAR(100)
		, @Application nvarchar(50)
		, @Strict bit = 1 
	)
AS
DECLARE
	@BlogId int

	IF @Strict = 0 
		AND NOT EXISTS(SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].subtext_Config WHERE Host = @Host)
		AND (1 = (SELECT COUNT(1) FROM [<dbUser,varchar,dbo>].subtext_DomainAlias WHERE Host = @Host))
	BEGIN
		SELECT @BlogId = BlogId FROM [<dbUser,varchar,dbo>].subtext_DomainAlias WHERE (Host = @Host OR Host = 'www.' + @Host) AND IsActive = 1
	END
	ELSE
	BEGIN
		SELECT @BlogId = BlogId FROM [<dbUser,varchar,dbo>].subtext_DomainAlias WHERE (Host = @Host OR Host = 'www.' + @Host) AND Application = @Application AND IsActive = 1
	END
	EXEC [<dbUser,varchar,dbo>].[subtext_GetBlogById]  @BlogId = @BlogId
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetBlogByDomainAlias] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetPageableDomainAliases]
(
	  @PageIndex int
	, @PageSize int
	, @BlogId int
)
AS

DECLARE @FirstId int
DECLARE @StartRow int
DECLARE @StartRowIndex int

SET @StartRowIndex = @PageIndex * @PageSize + 1

SET ROWCOUNT @StartRowIndex
-- Get the first entry id for the current page.
SELECT	@FirstId = Id FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias]
WHERE BlogId = @BlogId
ORDER BY [BlogId] ASC

-- Now, set the row count to MaximumRows and get
-- all records >= @first_id
SET ROWCOUNT @PageSize

SELECT	
		  Id
		, BlogId
		, Host
		, Application
		, IsActive
FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] 
WHERE Id >= @FirstId
	AND BlogId = @BlogId
ORDER BY Id ASC

SELECT COUNT([BlogId]) AS TotalRecords
FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias]
WHERE BlogId = @BlogId
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetPageableDomainAliases] TO [public]
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_CreateDomainAlias]
	(
		  @BlogId int
		, @Host	nvarchar(100)
		, @Application nvarchar(50)
		, @Active bit = 1
		, @Id int = NULL OUTPUT
	)
AS
IF NOT EXISTS(SELECT * FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] WHERE Host = @Host AND Application = @Application)
BEGIN
	INSERT INTO [<dbUser,varchar,dbo>].[subtext_DomainAlias]		
	(
		 BlogId
		,Host
		,Application
		,IsActive
	)
	VALUES
	(
		 @BlogId
		,@Host
		,@Application
		,@Active
	)

	SELECT @Id = SCOPE_IDENTITY()
END
GO
GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_CreateDomainAlias] TO [public]
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_DeleteDomainAlias]
	(
		  @Id	INT
	)
AS
	DELETE 
	FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] 
	WHERE Id = @Id
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_DeleteDomainAlias] TO [public]
GO
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_UpdateDomainAlias]
	(
		  @Id int
		, @BlogId int
		, @Host	nvarchar(100)
		, @Application nvarchar(50)
		, @Active bit = 1
	)
AS
	UPDATE [<dbUser,varchar,dbo>].[subtext_DomainAlias]		
	SET  BlogId			=@BlogId
		,Host			=@Host
		,Application	=@Application
		,IsActive		=@Active
	WHERE Id = @Id	
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UpdateDomainAlias] TO [public]
GO

CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_GetDomainAliasById]
	(
		  @Id	INT
	)
AS
	SELECT Id, BlogId, Host, Application, IsActive
	FROM [<dbUser,varchar,dbo>].[subtext_DomainAlias] 
	WHERE Id = @Id
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetDomainAliasById] TO [public]
GO
CREATE PROC [<dbUser,varchar,dbo>].[subtext_DeleteBlogGroup]
(
	@Id int
)
AS
DELETE FROM [<dbUser,varchar,dbo>].[subtext_BlogGroup] WHERE Id = @Id
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_DeleteBlogGroup] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_GetBlogGroup]
(
	@Id int
	, @Active bit
)
AS
SELECT	c.Id
		, c.Title
		, c.Active
		, c.DisplayOrder
		, c.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_BlogGroup] c
WHERE c.Id = @Id AND c.Active <> CASE @Active WHEN 0 THEN -1 else 0 END
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_GetBlogGroup] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_InsertBlogGroup]
(
	@Title nvarchar(150)
	, @Active bit
	, @DisplayOrder int = NULL
	, @Description nvarchar(1000) = NULL
	, @Id int OUTPUT
)
AS
Set NoCount ON
INSERT INTO [<dbUser,varchar,dbo>].[subtext_BlogGroup]
( 
	Title
	, Active
	, [Description]
	, DisplayOrder 
)
VALUES 
(
	@Title
	, @Active
	, @Description
	, @DisplayOrder
)
SELECT @Id = SCOPE_IDENTITY()
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_InsertBlogGroup] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_UpdateBlogGroup]
(
	@Id int,
	@Title nvarchar(150),
	@Active bit,
	@Description nvarchar(1000) = NULL,
	@DisplayOrder int = NULL
)
AS
UPDATE [<dbUser,varchar,dbo>].[subtext_BlogGroup] 
SET 
	[Title] = @Title, 
	[Active] = @Active,
	[Description] = @Description,
	[DisplayOrder] = @DisplayOrder
WHERE   
		[Id] = @Id
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_UpdateBlogGroup] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[subtext_ListBlogGroups]
(
	@Active bit
)
AS
SELECT	c.Id
		, c.Title
		, c.Active
		, c.DisplayOrder
		, c.[Description]
FROM [<dbUser,varchar,dbo>].[subtext_BlogGroup] c
WHERE c.Active <> CASE @Active WHEN 0 THEN -1 else 0 END
ORDER BY DisplayOrder, Title
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[subtext_ListBlogGroups] TO [public]
GO

CREATE PROC [<dbUser,varchar,dbo>].[DNW_GetRecentImages]
	@Host nvarchar(100)
	, @GroupID int

AS
SELECT Top 35 Host
	, [Subfolder]
	, images.ImageID
	, [ImageTitle] = images.Title
	, [ImageFile] = images.[File]
	, config.TimeZone
	, [BlogTitle] = config.Title
	, [CategoryTitle] = categories.Title
	, categories.CategoryID
FROM [<dbUser,varchar,dbo>].[subtext_Images] images
INNER JOIN	[<dbUser,varchar,dbo>].[subtext_Config] config ON images.BlogId = config.BlogId
INNER JOIN  [<dbUser,varchar,dbo>].[subtext_LinkCategories] categories ON categories.CategoryID = images.CategoryID
WHERE  images.Active > 0
	AND config.Host = @Host
	AND (config.BlogGroupId = @GroupID OR @GroupID = 0)
ORDER BY [ImageID] DESC
GO

GRANT EXECUTE ON [<dbUser,varchar,dbo>].[DNW_GetRecentImages] TO [public]
GO
