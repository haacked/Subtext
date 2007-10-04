using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using SubSonic;
using Subtext.Data;

namespace Subtext.Installation
{
	public class SqlInstaller
	{
		private readonly string connectionString;
		
		public SqlInstaller(string connectionString)
		{
			this.connectionString = connectionString;
		}

		private string _dbUser = string.Empty;

		public string DbUser
		{
			get { return _dbUser; }
			set { _dbUser = value; }
		}
		/// <summary>
		/// Gets the framework version.
		/// </summary>
		/// <value></value>
		public Version SubtextAssemblyVersion
		{
			get
			{
				if (subtextAssemblyVersion == null)
				{
					subtextAssemblyVersion = this.GetType().Assembly.GetName().Version;
				}

				return subtextAssemblyVersion;
			}
		}
		private Version subtextAssemblyVersion;

		public void Install(Version assemblyVersion)
		{
			using (SqlConnection connection = new SqlConnection(this.connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						string[] scripts = ListInstallationScripts(this.CurrentInstallationVersion, SubtextAssemblyVersion);
						foreach (string scriptName in scripts)
						{
							ScriptHelper.ExecuteScript(scriptName, transaction, _dbUser);
						}

						ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction, _dbUser);
						transaction.Commit();

						// putting this update inside the transaction causes a timeout during a clean install 
						// b/c the SP hasn't been added yet.
						UpdateInstallationVersionNumber(assemblyVersion);
					}
					catch (Exception)
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Upgrades this instance. Returns true if it was successful.
		/// </summary>
		/// <returns></returns>
		public void Upgrade()
		{
			using (SqlConnection connection = new SqlConnection(this.connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						Version installationVersion = this.CurrentInstallationVersion;
						if (installationVersion == null)
						{
							//This is the base version.  We need to hardcode this 
							//because Subtext 1.0 didn't write the assembly version 
							//into the database.
							installationVersion = new Version(1, 0, 0, 0);
						}
						string[] scripts = ListInstallationScripts(installationVersion, SubtextAssemblyVersion);
						foreach (string scriptName in scripts)
						{
							ScriptHelper.ExecuteScript(scriptName, transaction, _dbUser);
						}
						ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction, _dbUser);

						UpdateInstallationVersionNumber(SubtextAssemblyVersion);
						transaction.Commit();
					}
					catch (Exception)
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Returns a collection of installation script names with a version 
		/// less than or equal to the max version.
		/// </summary>
		/// <param name="minVersionExclusive">The min verison exclusive.</param>
		/// <param name="maxVersionInclusive">The max version inclusive.</param>
		/// <returns></returns>
		public string[] ListInstallationScripts(Version minVersionExclusive, Version maxVersionInclusive)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string[] resourceNames = assembly.GetManifestResourceNames();
			StringCollection collection = new StringCollection();
			foreach (string resourceName in resourceNames)
			{
				SqlInstallationProvider.InstallationScriptInfo scriptInfo = SqlInstallationProvider.InstallationScriptInfo.Parse(resourceName);
				if (scriptInfo == null) continue;

				if ((minVersionExclusive == null || scriptInfo.Version > minVersionExclusive)
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
		/// Updates the value of the current installed version within the subtext_Version table.
		/// </summary>
		/// <param name="newVersion">New version.</param>
		public void UpdateInstallationVersionNumber(Version newVersion)
		{
			StoredProcedure proc = new StoredProcedure("subtext_VersionAdd");
			proc.Command.AddParameter("@Major", newVersion.Major);
			proc.Command.AddParameter("@Minor", newVersion.Minor);
			proc.Command.AddParameter("@Build", newVersion.Build);
			proc.Execute();
		}

		/// <summary>
		/// Gets the <see cref="Version"/> of the current Subtext data store (ie. SQL Server). 
		/// This is the value stored in the database. If it does not match the actual 
		/// assembly version, we may need to run an upgrade.
		/// </summary>
		/// <returns></returns>
		public Version CurrentInstallationVersion
		{
			get
			{
				StoredProcedure proc = new StoredProcedure("subtext_VersionGetCurrent");

				try
				{
					using (IDataReader reader = proc.GetReader())
					{
						if (reader.Read())
						{
							Version version = new Version((int) reader["Major"], (int) reader["Minor"], (int) reader["Build"]);
							reader.Close();
							return version;
						}
						reader.Close();
					}
				}
				catch (SqlException exception)
				{
					const int CouldNotFindStoredProcedure = 2812;
					if (exception.Number != CouldNotFindStoredProcedure)
						throw;

					if (exception.Number != (int)SqlErrorMessage.CouldNotFindStoredProcedure)
						throw;
				}
				return null;
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
			if (installationVersion >= SubtextAssemblyVersion)
			{
				return false;
			}

			if (installationVersion == null)
			{
				//This is the base version.  We need to hardcode this 
				//because Subtext 1.0 didn't write the assembly version 
				//into the database.
				installationVersion = new Version(1, 0, 0, 0);
			}
			string[] scripts = ListInstallationScripts(installationVersion, SubtextAssemblyVersion);
			return scripts.Length > 0;
		}
	}
}
