using System;
using System.Configuration;
using MbUnit.Framework;
using Subtext;
using UnitTests.Subtext;

[assembly: AssemblyCleanup(typeof(AssemblySetUpAndCleanUp))]
namespace UnitTests.Subtext
{
	public static class AssemblySetUpAndCleanUp
	{
		[SetUp]
		public static void SetUp()
		{
			Console.WriteLine("Rebuilding Database for unit tests...");
			string connectionString = ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString;
			Arguments arguments = new Arguments("install /recreate-db /connect \"" + connectionString + "\"");
			InstallCommand installer = new InstallCommand();
			installer.Execute(arguments);
			Console.WriteLine("Rebuild complete!");
		}
		
		[TearDown]
		public static void TearDown()
		{
			//Not sure we need anything here yet.
		}
	}
}
