using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework;
using Subtext.Framework.Data;

namespace Subtext.Installation
{
	public class SqlInstaller
	{
		private readonly string connectionString;
		
		public SqlInstaller(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public string DbUser
		{
			get;
			set;
		}

		public void Install(Version assemblyVersion)
		{
			using (SqlConnection connection = new SqlConnection(this.connectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						string[] scripts = ListInstallationScripts(this.GetCurrentInstallationVersion(), VersionInfo.FrameworkVersion);
						foreach (string scriptName in scripts)
						{
                            ScriptHelper.ExecuteScript(scriptName, transaction, DbUser);
						}

						ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction, DbUser);
						UpdateInstallationVersionNumber(assemblyVersion, transaction);
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
						Version installationVersion = this.GetCurrentInstallationVersion();
						if (installationVersion == null)
						{
							//This is the base version.  We need to hardcode this 
							//because Subtext 1.0 didn't write the assembly version 
							//into the database.
							installationVersion = new Version(1, 0, 0, 0);
						}
						string[] scripts = ListInstallationScripts(installationVersion, VersionInfo.FrameworkVersion);
						foreach (string scriptName in scripts)
						{
							ScriptHelper.ExecuteScript(scriptName, transaction, DbUser);
						}
						ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction, DbUser);

						UpdateInstallationVersionNumber(VersionInfo.FrameworkVersion, transaction);
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
		/// <param name="transaction">The transaction to perform this action within.</param>
		public void UpdateInstallationVersionNumber(Version newVersion, SqlTransaction transaction)
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

		static SqlParameter CreateParameter(string name, SqlDbType dbType, int size, object value)
		{
			SqlParameter param = new SqlParameter(name, dbType, size);
			param.Value = value;
			return param;
		}

		/// <summary>
		/// Gets the <see cref="Version"/> of the current Subtext data store (ie. SQL Server). 
		/// This is the value stored in the database. If it does not match the actual 
		/// assembly version, we may need to run an upgrade.
		/// </summary>
		/// <returns></returns>
		public Version GetCurrentInstallationVersion()
		{
			string sql = "subtext_VersionGetCurrent";

			try
			{
				using (IDataReader reader = SqlHelper.ExecuteReader(this.connectionString, CommandType.StoredProcedure, sql))
				{
					if (reader.Read())
					{
						Version version = new Version((int)reader["Major"], (int)reader["Minor"], (int)reader["Build"]);
						reader.Close();
						return version;
					}
					reader.Close();
				}
			}
			catch (SqlException exception)
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
			if (installationVersion >= VersionInfo.FrameworkVersion)
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
			string[] scripts = ListInstallationScripts(installationVersion, VersionInfo.FrameworkVersion);
			return scripts.Length > 0;
		}
	}
}
