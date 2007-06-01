using System;
using System.Configuration;
using System.Data.SqlClient;
using MbUnit.Framework;
using Subtext.Installation;
using UnitTests.Subtext;

[assembly: AssemblyCleanup(typeof(AssemblySetUpAndCleanUp))]
namespace UnitTests.Subtext
{
	public static class AssemblySetUpAndCleanUp
	{
		[SetUp]
		public static void SetUp()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString;
			SqlInstaller installer = new SqlInstaller(connectionString);
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				using (SqlTransaction transaction = conn.BeginTransaction())
				{
					installer.UpdateInstallationVersionNumber(installer.SubtextAssemblyVersion);
					transaction.Commit();
				}
			}
/*
Console.WriteLine("Rebuilding Database for unit tests...");

Arguments arguments = new Arguments("install /connect \"" + connectionString + "\"");
InstallCommand installer = new InstallCommand();
installer.Execute(arguments);
Console.WriteLine("Rebuild complete!");
*/			
		}
		
		[TearDown]
		public static void TearDown()
		{
			//Not sure we need anything here yet.
		}
	}
}
