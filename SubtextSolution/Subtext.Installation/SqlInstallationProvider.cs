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
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Subtext.Data;
using Subtext.Extensibility.Providers;

namespace Subtext.Installation
{
	/// <summary>
	/// Summary description for SqlInstallationProvider.
	/// </summary>
	public class SqlInstallationProvider : InstallerProvider
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
		/// Gets the installation status based on the current assembly Version.
		/// </summary>
		/// <returns></returns>
		public override InstallationState InstallationStatus
		{
			get
			{
				Version installationVersion = CurrentInstallationVersion;
				if (installationVersion == null)
					return InstallationState.NeedsInstallation;

				if (NeedsUpgrade(installationVersion))
				{
					return InstallationState.NeedsUpgrade;
				}

				return InstallationState.Complete;
			}
		}

		/// <summary>
		/// Gets the <see cref="Version"/> of the current Subtext data store (ie. SQL Server). 
		/// This is the value stored in the database. If it does not match the actual 
		/// assembly version, we may need to run an upgrade.
		/// </summary>
		/// <returns></returns>
		public override Version CurrentInstallationVersion
		{
			get
			{
				return this.installer.CurrentInstallationVersion;
			}
		}

		/// <summary>
		/// Initializes the specified provider.
		/// </summary>
		/// <param name="name">Friendly Name of the provider.</param>
		/// <param name="config">Config value.</param>
		public override void Initialize(string name, NameValueCollection config)
		{
            _connectionString = ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName", config);
            this.installer = new SqlInstaller(_connectionString);
			if (!String.IsNullOrEmpty(config["dbUser"]))
				this.installer.DbUser = config["dbUser"];
            base.Initialize(name, config);
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
				throw new ArgumentNullException("exception", "It's not an installation exception if it's null.");

			Regex tableRegex = new Regex("Invalid object name '.*?'", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			bool isDbException = exception is DbException;

			if (isDbException && tableRegex.IsMatch(exception.Message))
				return true;

			Regex spRegex = new Regex("'Could not find stored procedure '.*?'", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			if (isDbException && spRegex.IsMatch(exception.Message))
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
				if (!match.Success)
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
	}
}
