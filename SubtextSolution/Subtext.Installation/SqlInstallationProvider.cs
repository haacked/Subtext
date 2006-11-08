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
using System.Reflection;
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
	public class SqlInstallationProvider : InstallationProvider
	{
		Version _version;
		string _connectionString = string.Empty;
		
		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="config">Config value.</param>
		public override void Initialize(string name, NameValueCollection config)
		{
            _connectionString = ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName", config);
            base.Initialize(name, config);
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
			if (exception == null)
				throw new ArgumentNullException("exception", "Cannot determine whether a null exception is an installation exception.  This exception should never be thrown.");
			
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
			using(SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						Version installationVersion = this.GetCurrentInstallationVersion();
						if(installationVersion == null)
						{
							//This is the base version.  We need to hardcode this 
							//because Subtext 1.0 didn't write the assembly version 
							//into the database.
							installationVersion = new Version(1, 0, 0, 0);
						}
						string[] scripts = ListInstallationScripts(installationVersion, this.CurrentAssemblyVersion);
						foreach(string scriptName in scripts)
						{
							ScriptHelper.ExecuteScript(scriptName, transaction);	
						}
						ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction);

						UpdateInstallationVersionNumber(this.CurrentAssemblyVersion, transaction);
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
		/// Installs this instance.  Returns true if it was successful.
		/// </summary>
		/// <param name="assemblyVersion">The version of the assembly being installed.</param>
		/// <returns></returns>
		public override void Install(Version assemblyVersion)
		{
			using(SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						string[] scripts = ListInstallationScripts(this.GetCurrentInstallationVersion(), this.CurrentAssemblyVersion);
						foreach(string scriptName in scripts)
						{
							ScriptHelper.ExecuteScript(scriptName, transaction);	
						}

						ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction);
						UpdateInstallationVersionNumber(assemblyVersion, transaction);
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
		/// Updates the value of the current installed version within the subtext_Version table.
		/// </summary>
		/// <param name="newVersion">New version.</param>
		public override void UpdateInstallationVersionNumber(Version newVersion, SqlTransaction transaction)
		{
			if (newVersion == null)
				throw new ArgumentNullException("newVersion", "Cannot update the version to a null version.");
			
			string sql = "subtext_VersionAdd";
			SqlParameter[] p =
			{
				CreateParameter("@Major", SqlDbType.Int, 4, newVersion.Major), 
				CreateParameter("@Minor", SqlDbType.Int, 4, newVersion.Minor), 
				CreateParameter("@Build", SqlDbType.Int, 4, newVersion.Build)
			};
			SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, sql, p);
		}

		static SqlParameter CreateParameter(string name, SqlDbType dbType, int size, object value)
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
		public bool NeedsUpgrade(Version installationVersion)
		{
			if(installationVersion >= CurrentAssemblyVersion)
			{
				return false;
			}

			if(installationVersion == null)
			{
				//This is the base version.  We need to hardcode this 
				//because Subtext 1.0 didn't write the assembly version 
				//into the database.
				installationVersion = new Version(1, 0, 0, 0);
			}
			string[] scripts = ListInstallationScripts(installationVersion, CurrentAssemblyVersion);
			return scripts.Length > 0;
		}

		/// <summary>
		/// Returns a collection of installation script names with a version 
		/// less than or equal to the max version.
		/// </summary>
		/// <param name="maxVersionInclusive">The max version inclusive.</param>
		/// <returns></returns>
		public static string[] ListInstallationScripts(Version minVersionExclusive, Version maxVersionInclusive)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string[] resourceNames = assembly.GetManifestResourceNames();
			StringCollection collection = new StringCollection();
			foreach(string resourceName in resourceNames)
			{
				InstallationScriptInfo scriptInfo = InstallationScriptInfo.Parse(resourceName);
				if(scriptInfo == null) continue;
				
				if((minVersionExclusive == null || scriptInfo.Version > minVersionExclusive)
					&& (maxVersionInclusive == null || scriptInfo.Version <= maxVersionInclusive))
				{
					collection.Add(scriptInfo.ScriptName);
				}
			}

			string[] scripts = new string[collection.Count];
			collection.CopyTo(scripts, 0);
			Array.Sort(scripts);

			return scripts;
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
			private InstallationScriptInfo(string scriptName, Version version)
			{
				this.version = version;
				this.scriptName = scriptName;
			}

			internal static InstallationScriptInfo Parse(string resourceName)
			{
				Regex regex = new Regex(@"(?<ScriptName>Installation\.(?<version>\d+\.\d+\.\d+)\.sql)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
				Match match = regex.Match(resourceName);
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
			}

			string scriptName;

			public Version Version
			{
				get { return this.version; }
			}

			Version version;
		}
	}
}
