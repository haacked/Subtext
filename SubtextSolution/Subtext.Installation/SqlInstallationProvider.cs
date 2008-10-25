#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Data;
using Subtext.Web.Controls;

namespace Subtext.Installation
{
	/// <summary>
	/// Summary description for SqlInstallationProvider.
	/// </summary>
	public class SqlInstallationProvider : Extensibility.Providers.Installation
	{
		string _connectionString = string.Empty;
		SqlInstaller installer;

		public SqlInstallationProvider()
		{
		}

		public SqlInstallationProvider(string connectionString)
		{
			this.installer = new SqlInstaller(connectionString);
			this._connectionString = connectionString;
		}

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
            _connectionString = ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName", configValue);

			this.installer = new SqlInstaller(_connectionString);
			if (!String.IsNullOrEmpty(configValue["dbUser"]))
				this.installer.DbUser = configValue["dbUser"];
            base.Initialize(name, configValue);
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
			ConnectionStringBuilder builder = new ConnectionStringBuilder();
			builder.AllowWebConfigOverride = true;
			builder.Description = "A SQL Connection String with the rights to create SQL Database objects such as Stored Procedures, Table, and Views.";
			builder.Title = "Connection String";
		
			return builder;
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
			return string.Empty;
		}

		/// <summary>
		/// Gets the installation status based on the current assembly Version.
		/// </summary>
		/// <param name="currentAssemblyVersion">The version of the assembly that represents this installation.</param>
		/// <returns></returns>
		public override InstallationState GetInstallationStatus(Version currentAssemblyVersion)
		{
			Version installationVersion = GetCurrentInstallationVersion();
			if (installationVersion == null)
				return InstallationState.NeedsInstallation;
			
			if (NeedsUpgrade(installationVersion))
			{
				return InstallationState.NeedsUpgrade;
			}
		
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
            bool isSqlException = exception is SqlException;

			if(isSqlException && tableRegex.IsMatch(exception.Message))
				return true;

			Regex spRegex = new Regex("'Could not find stored procedure '.*?'", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			if(isSqlException && spRegex.IsMatch(exception.Message))
				return true;

			return false;
		}

		/// <summary>
		/// Upgrades this instance. Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public override void Upgrade()
		{
			this.installer.Upgrade();
		}

		/// <summary>
		/// Installs this instance.  Returns true if it was successful.
		/// </summary>
		/// <param name="assemblyVersion">The version of the assembly being installed.</param>
		/// <returns></returns>
		public override void Install(Version assemblyVersion)
		{
			this.installer.Install(assemblyVersion);
		}

		/// <summary>
		/// Gets the <see cref="Version"/> of the current Subtext data store (ie. SQL Server). 
		/// This is the value stored in the database. If it does not match the actual 
		/// assembly version, we may need to run an upgrade.
		/// </summary>
		/// <returns></returns>
		public override Version GetCurrentInstallationVersion()
		{
			string sql = "subtext_VersionGetCurrent";
		
			try 
			{
				using(IDataReader reader = SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, sql))
				{
					if(reader.Read())
					{
						Version version = new Version((int)reader["Major"], (int)reader["Minor"], (int)reader["Build"]);
						reader.Close();
						return version;
					}
					reader.Close();
				}
			}
			catch(SqlException exception) 
			{
				if (exception.Number != (int)SqlErrorMessage.CouldNotFindStoredProcedure)
					throw;
			}
			return null;
		}

		/// <summary>
		/// Gets a value indicating whether the subtext installation needs an upgrade 
		/// to occur.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [needs upgrade]; otherwise, <c>false</c>.
		/// </value>
		public bool NeedsUpgrade(Version installationVersion)
		{
			return installer.NeedsUpgrade(installationVersion);
		}		

		/// <summary>
		/// Attempts to repair this instance. Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public override bool Repair()
		{
			throw new NotImplementedException();
		}

		internal class InstallationScriptInfo
		{
            //Have the compiled regex as static to get the full benefit of compilation
            private static Regex _ScriptParseRegex = 
                new Regex(@"(?<ScriptName>Installation\.(?<version>\d+\.\d+\.\d+)\.sql)$", 
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);

			private InstallationScriptInfo(string scriptName, Version version)
			{
				this.version = version;
				this.scriptName = scriptName;
			}

			internal static InstallationScriptInfo Parse(string resourceName)
			{
                Match match = _ScriptParseRegex.Match(resourceName);
				if(!match.Success)
				{
					return null;
				}
				Version version = new Version(match.Groups["version"].Value);
				string scriptName = match.Groups["ScriptName"].Value;
				return new InstallationScriptInfo(scriptName, version);
			}

			public string ScriptName
			{
				get { return this.scriptName; }
				set { this.scriptName = value; }
			}

			string scriptName;

			public Version Version
			{
				get { return this.version; }
				set { this.version = value; }
			}

			Version version;
		}

		/// <summary>
		/// Determines whether the specified exception is due to a permission 
		/// denied error.
		/// </summary>
		/// <param name="exception"></param>
		/// <returns></returns>
		public override bool IsPermissionDeniedException(Exception exception)
		{
			SqlException sqlexc = exception.InnerException as SqlException;
			return sqlexc != null
				&&
				(
				sqlexc.Number == (int)SqlErrorMessage.PermissionDeniedInDatabase
				|| sqlexc.Number == (int)SqlErrorMessage.PermissionDeniedOnProcedure
				|| sqlexc.Number == (int)SqlErrorMessage.PermissionDeniedInOnColumn
				|| sqlexc.Number == (int)SqlErrorMessage.PermissionDeniedInOnObject
				);
		}

		/// <summary>
		/// Updates the current installed version.
		/// </summary>
		/// <param name="newVersion">The new version that is now current.</param>
		/// <param name="transaction">The transaction to perform this upgrade within.</param>
		/// <returns></returns>
		public override void UpdateInstallationVersionNumber(Version newVersion, SqlTransaction transaction)
		{
			this.installer.UpdateInstallationVersionNumber(newVersion, transaction);
		}
	}
}
