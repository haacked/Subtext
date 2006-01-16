using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;
using Subtext.Scripting;

namespace Subtext.Installation
{
	/// <summary>
	/// Summary description for SqlInstallationProvider.
	/// </summary>
	public class SqlInstallationProvider : InstallationProvider
	{
		Version _version = null;
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
			_defaultConnectionString = GetSettingValue("defaultConnectionString", configValue);
			// Can specify this in web.config but it's better not to.
			_adminConnectionString = GetSettingValue("adminConnectionString", configValue);
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
		/// This method returns the <see cref="Control"/> used to gather 
		/// the required installation information.  This will be returned 
		/// back to the provider after the user provides the information.
		/// </p>
		/// </summary>
		/// <returns></returns>
		public override Control GatherInstallationInformation()
		{
			HtmlTable table = new HtmlTable();
			table.ID = "installationQuestionTable";
			table.CellPadding = 3;
			table.CellSpacing = 0;

			// Create header row
			HtmlTableRow headerRow = new HtmlTableRow();
			headerRow.BgColor = "#EEEEEE";
			HtmlTableCell fieldCell = new HtmlTableCell("th");
			fieldCell.ColSpan = 2;
			fieldCell.Controls.Add(new LiteralControl("<strong>Connection String</strong>"));
			headerRow.Cells.Add(fieldCell);
			table.Rows.Add(headerRow);

			// Create Text Input Row
			HtmlTableRow row = new HtmlTableRow();
			row.VAlign = "top";
			HtmlTableCell questionCell = new HtmlTableCell();
			TextBox textbox = new TextBox();
			textbox.ID = "txtAdminConnectionString";
			questionCell.Controls.Add(textbox);

			//Checkbox to use connection string in web.config
			CheckBox checkbox = new CheckBox();
			checkbox.ID = "chkUseConnectionStringInWebConfig";
			checkbox.Text = "Use Connection String In Web.config";
			checkbox.Attributes["onclick"] = "if(this.checked) {txtAdminConnectionString.disabled = true;} else {txtAdminConnectionString.disabled = false;} ;";
			questionCell.Controls.Add(new LiteralControl("<br />"));
			questionCell.Controls.Add(checkbox);
			
			row.Cells.Add(questionCell);

			HtmlTableCell descriptionCell = new HtmlTableCell();
			descriptionCell.Controls.Add(new LiteralControl("A SQL Connection String with the rights to create SQL Database objects such as Stored Procedures, Table, and Views."));
			row.Cells.Add(descriptionCell);
			
			table.Rows.Add(row);
			
			return table;
		}

		/// <summary>
		/// Provides the installation information as provided by the user. 
		/// The control passed in should be the same as that provided in 
		/// <see cref="GatherInstallationInformation"/>, but with user values 
		/// supplied within it.
		/// </summary>
		/// <param name="populatedControl">Populated control.</param>
		public override void ProvideInstallationInformation(Control populatedControl)
		{
			this._adminConnectionString = GetConnectionStringFromControl(populatedControl);
		}

		string GetConnectionStringFromControl(Control populatedControl)
		{
			if(populatedControl != null)
			{
				CheckBox chkUseWebConfig = populatedControl.FindControl("chkUseConnectionStringInWebConfig") as CheckBox;
				if(chkUseWebConfig != null && chkUseWebConfig.Checked)
					return ConfigurationSettings.AppSettings["ConnectionString"];

				TextBox textbox = populatedControl.FindControl("txtAdminConnectionString") as TextBox;
				if(textbox != null)
					return textbox.Text;
			}
			return string.Empty;
		}

		/// <summary>
		/// Validates the installation information provided by the user.  
		/// Returns a NameValueCollection of any fields that are incorrect 
		/// with an explanation of why it is incorrect.
		/// </summary>
		/// <param name="control">Information.</param>
		/// <returns></returns>
		public override string ValidateInstallationInformation(Control control)
		{
			string connectionString = GetConnectionStringFromControl(control);
			string errorMessages = string.Empty;
			if(connectionString == null || connectionString.Length == 0)
			{
				errorMessages = "The Connection String was not specified.";
				return errorMessages;
			}

			if(!TestAdminConnection(connectionString))
			{
				//TODO: Improve this error handling.
				errorMessages = "The connection string is invalid or does not have sufficient rights.";
				return errorMessages;
			}
			return string.Empty;
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
		/// Gets the installation status based on the current assembly Version.
		/// </summary>
		/// <param name="currentAssemblyVersion">The version of the assembly that represents this installation.</param>
		/// <returns></returns>
		public override InstallationState GetInstallationStatus(Version currentAssemblyVersion)
		{
			if(!VersionTableExists)
				return InstallationState.NeedsInstallation;
			
			if(NeedsUpgrade)
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
		/// <param name="assemblyVersion">Version of the assembly upgrading to.</param>
		/// <returns></returns>
		public override bool Upgrade(Version assemblyVersion)
		{
			using(SqlConnection connection = new SqlConnection(this._adminConnectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					//TODO: Calculate the script name.
					try
					{
						bool result = false; //ScriptHelper.ExecuteScript("UpgradeDotText095Script.sql", transaction);
						if(result)
						{
							UpdateCurrentInstalledVersion(assemblyVersion, transaction);
							transaction.Commit();
						}
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
		/// <param name="assemblyVersion">The version of the assembly being installed.</param>
		/// <returns></returns>
		public override void Install(Version assemblyVersion)
		{
			using(SqlConnection connection = new SqlConnection(this._adminConnectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						//TODO: Calculate the script name.
						ScriptHelper.ExecuteScript("Installation.01.00.00.sql", transaction);
						ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction);
						UpdateCurrentInstalledVersion(assemblyVersion, transaction);
						transaction.Commit();
					}
					catch(Exception)
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Gets the <see cref="Version"/> of the current Subtext installation.
		/// </summary>
		/// <returns></returns>
		public override Version GetCurrentInstalledVersion()
		{
			string sql = "subtext_VersionGetCurrent";
		
			using(IDataReader reader = SqlHelper.ExecuteReader(_defaultConnectionString, CommandType.StoredProcedure, sql))
			{
				if(reader.Read())
				{
					Version version = new Version((int)reader["Major"], (int)reader["Minor"], (int)reader["Build"]);
					reader.Close();
					return version;
				}
				reader.Close();
			}
			return null;
		}

		/// <summary>
		/// Updates the value of the current installed version within the subtext_Version table.
		/// </summary>
		/// <param name="newVersion">New version.</param>
		public override void UpdateCurrentInstalledVersion(Version newVersion, SqlTransaction transaction)
		{
			string sql = "subtext_VersionAdd";
			SqlParameter[] p =
			{
				CreateParameter("@Major", SqlDbType.Int, 4, newVersion.Major), 
				CreateParameter("@Minor", SqlDbType.Int, 4, newVersion.Minor), 
				CreateParameter("@Build", SqlDbType.Int, 4, newVersion.Build)
			};
			SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, sql, p);
		}

		SqlParameter CreateParameter(string name, SqlDbType dbType, int size, object value)
		{
			SqlParameter param = new SqlParameter(name, dbType, size);
			param.Value = value;
			return param;
		}

		/// <summary>
		/// Gets the framework version.
		/// </summary>
		/// <value></value>
		public Version CurrentAssemblyVersion
		{
			get
			{
				if(_version == null)
				{
					_version = this.GetType().Assembly.GetName().Version;
				}
				return _version;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the subtext installation needs an upgrade 
		/// to occur.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [needs upgrade]; otherwise, <c>false</c>.
		/// </value>
		bool NeedsUpgrade
		{
			get
			{
				Version installedVersion = GetCurrentInstalledVersion();
				if(installedVersion > CurrentAssemblyVersion)
				{
					//TODO: check if we have any scripts between the current 
					//		installed version and the current assemly version.
				}
				return false;
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
		bool VersionTableExists
		{
			get
			{
				return DoesTableExist("subtext_Version");
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
