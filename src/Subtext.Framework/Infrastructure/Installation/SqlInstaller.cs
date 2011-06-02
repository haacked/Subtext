#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Reflection;
using Subtext.Framework.Data;

namespace Subtext.Framework.Infrastructure.Installation
{
    public class SqlInstaller : IInstaller
    {
        private readonly string _connectionString;

        public SqlInstaller(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string DBUser { get; set; }

        public void Install(Version assemblyVersion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        ReadOnlyCollection<string> scripts = ListInstallationScripts(GetCurrentInstallationVersion(),
                                                                                     VersionInfo.CurrentAssemblyVersion);
                        foreach (string scriptName in scripts)
                        {
                            ScriptHelper.ExecuteScript(scriptName, transaction, DBUser);
                        }

                        ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction, DBUser);
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
        public void Upgrade(Version currentAssemblyVersion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Version installationVersion = GetCurrentInstallationVersion() ?? new Version(1, 0, 0, 0);
                        ReadOnlyCollection<string> scripts = ListInstallationScripts(installationVersion, currentAssemblyVersion);
                        foreach (string scriptName in scripts)
                        {
                            ScriptHelper.ExecuteScript(scriptName, transaction, DBUser);
                        }
                        ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction, DBUser);

                        UpdateInstallationVersionNumber(currentAssemblyVersion, transaction);
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
        public static ReadOnlyCollection<string> ListInstallationScripts(Version minVersionExclusive,
                                                                         Version maxVersionInclusive)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resourceNames = assembly.GetManifestResourceNames();
            var collection = new List<string>();
            foreach (string resourceName in resourceNames)
            {
                InstallationScriptInfo scriptInfo = InstallationScriptInfo.Parse(resourceName);
                if (scriptInfo == null)
                {
                    continue;
                }

                if ((minVersionExclusive == null || scriptInfo.Version > minVersionExclusive)
                   && (maxVersionInclusive == null || scriptInfo.Version <= maxVersionInclusive))
                {
                    collection.Add(scriptInfo.ScriptName);
                }
            }

            var scripts = new string[collection.Count];
            collection.CopyTo(scripts, 0);
            Array.Sort(scripts);

            return new ReadOnlyCollection<string>(new List<string>(scripts));
        }

        /// <summary>
        /// Updates the value of the current installed version within the subtext_Version table.
        /// </summary>
        /// <param name="newVersion">New version.</param>
        /// <param name="transaction">The transaction to perform this action within.</param>
        public static void UpdateInstallationVersionNumber(Version newVersion, SqlTransaction transaction)
        {
            var procedures = new StoredProcedures(transaction);
            procedures.VersionAdd(newVersion.Major, newVersion.Minor, newVersion.Build, DateTime.UtcNow);
        }

        /// <summary>
        /// Gets the <see cref="Version"/> of the current Subtext data store (ie. SQL Server). 
        /// This is the value stored in the database. If it does not match the actual 
        /// assembly version, we may need to run an upgrade.
        /// </summary>
        /// <returns></returns>
        public Version GetCurrentInstallationVersion()
        {
            var procedures = new StoredProcedures(_connectionString);
            try
            {
                using (var reader = procedures.VersionGetCurrent())
                {
                    if (reader.Read())
                    {
                        var version = new Version((int)reader["Major"], (int)reader["Minor"], (int)reader["Build"]);
                        reader.Close();
                        return version;
                    }
                }
            }
            catch (SqlException exception)
            {
                if (exception.Number != (int)SqlErrorMessage.CouldNotFindStoredProcedure)
                {
                    throw;
                }
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
        public bool NeedsUpgrade(Version installationVersion, Version currentAssemblyVersion)
        {
            if (installationVersion >= currentAssemblyVersion)
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
            ReadOnlyCollection<string> scripts = ListInstallationScripts(installationVersion, currentAssemblyVersion);
            return scripts.Count > 0;
        }
    }
}