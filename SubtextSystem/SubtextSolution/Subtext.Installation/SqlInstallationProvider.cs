using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Subtext.Extensibility.Providers;

namespace Subtext.Installation
{
	/// <summary>
	/// Summary description for SqlInstallationProvider.
	/// </summary>
	public class SqlInstallationProvider : InstallationProvider
	{
		string _name = string.Empty;
		const string TableExistsSql = "SELECT COUNT(1) FROM dbo.sysobjects WHERE id = object_id(N'[{0}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
		string _adminConnectionString = string.Empty;
		string _defaultConnectionString = string.Empty;

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
			_name = name;
		}

		/// <summary>
		/// Returns the friendly name of the provider when the provider is initialized.
		/// </summary>
		/// <value></value>
		public override string Name
		{
			get
			{
				return _name;
			}
		}

		/// <summary>
		/// <p>
		/// This method is called by the installation engine in order to ask the 
		/// provider what pieces of information it needs from the user in order 
		/// to proceed with the installation.
		/// </p>
		/// <p>
		/// The <see cref="NameValueCollection"/> returned by this method should 
		/// contain entries with the Name being the piece of information being 
		/// requested and the Value being a description of the requested information.
		/// </p>
		/// <p>
		/// Upon gathering this information, the <see cref="Initialize"/> method 
		/// will be called passing in a <see cref="NameValueCollection"/> with the 
		/// values containing the user's input for each name.
		/// </p>
		/// </summary>
		/// <returns></returns>
		public override NameValueCollection QueryInstallationInformation()
		{
			NameValueCollection information = new NameValueCollection();
			information["Admin Connection String"] = "A SQL Connection String with the rights to create SQL Database objects such as Stored Procedures, Table, and Views.";
			return information;
		}

		/// <summary>
		/// Validates the installation information provided by the user.  
		/// Returns a NameValueCollection of any fields that are incorrect 
		/// with an explanation of why it is incorrect.
		/// </summary>
		/// <param name="information">Information.</param>
		/// <returns></returns>
		public override NameValueCollection ValidateInstallationInformation(NameValueCollection information)
		{
			NameValueCollection errors = new NameValueCollection();
		
			string connectionString = information["Admin Connection String"] as string;
			if(connectionString == null || connectionString.Length == 0)
			{
				errors["Admin Connection String"] = "Was not specified.";
				return errors;
			}

			if(!TestAdminConnection(connectionString))
			{
				//TODO: Improve this error handling.
				errors["Admin Connection String"] = "The connection string is invalid or does not have sufficient rights.";
				return errors;
			}

		
			return errors;
		}

		bool TestAdminConnection(string connectionString)
		{
			try
			{
				string tableName = "subtext_" + System.Guid.NewGuid().ToString();
				string testCreateSql = "CREATE TABLE [" + tableName + "] ([TestColumn] [int] NULL)";
				SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, testCreateSql);
				string testDropSql = "DROP TABLE [" + tableName + "]";
				SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, testDropSql);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Provides the installation information to this installation provider. 
		/// See <see cref="QueryInstallationInformation"/> for more information.
		/// </summary>
		/// <param name="information">Information.</param>
		public override void ProvideInstallationInformation(NameValueCollection information)
		{
			string adminConnectionString = information["Admin Connection String"] as string;
			if(adminConnectionString != null && adminConnectionString.Length > 0)
			{
				_adminConnectionString = adminConnectionString;
			}
			string defaultConnectionString = information["Default Connection String"] as string;
			if(defaultConnectionString != null && defaultConnectionString.Length > 0)
			{
				_defaultConnectionString = defaultConnectionString;
			}
		}

		/// <summary>
		/// Gets the installation status.
		/// </summary>
		/// <returns></returns>
		public override InstallationState GetInstallationStatus()
		{
			if(!BlogContentTableExists)
				return InstallationState.NeedsInstallation;
			
			else if(!BlogHostTableExists)
				return InstallationState.NeedsUpgrade;
			
			return InstallationState.Complete;
		}

		/// <summary>
		/// Determines whether the specified exception is due to 
		/// a problem with the installation.
		/// </summary>
		/// <param name="exception">exception.</param>
		/// <returns>
		/// 	<c>true</c> if this is an installation exception; otherwise, <c>false</c>.
		/// </returns>
		public override bool IsInstallationException(Exception exception)
		{
			Regex tableRegex = new Regex("Invalid object name '.*?'", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			if(exception is System.Data.SqlClient.SqlException && tableRegex.IsMatch(exception.Message))
				return true;

			Regex spRegex = new Regex("'Could not find stored procedure '.*?'", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			if(exception is System.Data.SqlClient.SqlException && spRegex.IsMatch(exception.Message))
				return true;

			return false;
		}

		/// <summary>
		/// Upgrades this instance. Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public override bool Upgrade()
		{
			using(SqlConnection connection = new SqlConnection(this._adminConnectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					//TODO: Calculate the script name.
					try
					{
						bool result = ScriptHelper.ExecuteScript("UpgradeDotText095Script.sql", transaction);
						if(result)
							transaction.Commit();
						else
							transaction.Rollback();
						return result;
					}
					catch(Exception)
					{
						transaction.Rollback();
						return false;
					}
				}
			}
		}

		/// <summary>
		/// Installs this instance.  Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public override bool Install()
		{
			using(SqlConnection connection = new SqlConnection(this._adminConnectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						//TODO: Calculate the script name.
						if(ScriptHelper.ExecuteScript("InstallationScript.v1.0.sql", transaction))
						{
							bool result = ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction);
							if(result)
								transaction.Commit();
							else
								transaction.Rollback();
							return result;
						}
						transaction.Rollback();
						return false;
					}
					catch(SqlException)
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Attempts to repair this instance. Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public override bool Repair()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets or sets a value indicating whether the blog content table exists.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the blog content table exists; otherwise, <c>false</c>.
		/// </value>
		bool BlogContentTableExists
		{
			get
			{
				return DoesTableExist("subtext_content");
			}
		}

		bool BlogHostTableExists
		{
			get 
			{ 
				return DoesTableExist("subtext_Host");
			}
			
		}

		bool DoesTableExist(string tableName)
		{
			
			return 0 < GetTableCount(tableName);
		}

		int GetTableCount(string tableName)
		{
			string blogContentTableSql = String.Format(TableExistsSql, tableName);			
			return (int)SqlHelper.ExecuteScalar(this._defaultConnectionString, CommandType.Text, blogContentTableSql);
		}
	}
}
