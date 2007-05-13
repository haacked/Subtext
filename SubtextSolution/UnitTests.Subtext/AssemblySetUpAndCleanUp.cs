using System;
using System.Configuration;
using System.Data.SqlClient;
using MbUnit.Framework;
using Subtext.Installation;

namespace UnitTests.Subtext
{
	public static class AssemblySetUpAndCleanUp
	{
		[SetUp]
		public static void SetUp()
		{
			using (	SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString))
			{
				connection.Open();
				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					Console.WriteLine("REFRESHING STORED PROCEDURES!");
					ScriptHelper.ExecuteScript("StoredProcedures.sql", transaction);
					SqlInstaller installer = new SqlInstaller(ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString);
					Console.WriteLine("UPDATING INSTALLATION VERSION TO '{0}'!", installer.SubtextAssemblyVersion);
					installer.UpdateInstallationVersionNumber(installer.SubtextAssemblyVersion, transaction);
					transaction.Commit();
				}
			}
		}
		
		[TearDown]
		public static void TearDown()
		{
			//Not sure we need anything here yet.
		}
	}
}
