using System;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Provides constants for the common SQL messages as 
	/// listed in master.dbo.sysmessages.
	/// </summary>
	public enum SqlErrorMessages
	{
		/// <summary>
		/// Specified SQL server not found:
		/// </summary>
		SpecifiedSqlServerNotFound = 6,
		
		/// <summary>
		/// Could not find the stored procedure.
		/// </summary>
		CouldNotFindStoredProcedure = 2812,

		/// <summary>
		/// Cannot open database requested in login '%.*ls'. Login fails.
		/// </summary>
		LoginFailsCannotOpenDatabase = 4060,
		
		/// <summary>
		/// Login failed for user '%ls'. Reason: Not defined as a valid user 
		/// of a trusted SQL Server connection.
		/// </summary>
		LoginFailedInvalidUserOfTrustedConnection = 18450,
		
		/// <summary>
		/// Login failed for user '%ls'. Reason: Not associated with a 
		/// trusted SQL Server connection.
		/// </summary>
		LoginFailedNotAssociatedWithTrustedConnection = 18452,

		/// <summary>
		/// Login failed for user '%ls'.
		/// </summary>
		LoginFailed = 18456,

		/// <summary>
		/// Login failed for user '%ls'. Reason: User name contains a 
		/// mapping character or is longer than 30 characters.
		/// </summary>
		LoginFailedUserNameInvalid = 18457,


	}
}
