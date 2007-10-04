using System;
using System.Configuration;
using System.Data.SqlClient;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Installation;

namespace UnitTests.Subtext.Installation
{
	[TestFixture]
	public class InstallerTests
	{
		[Test]
		[ExpectedArgumentNullException]
		public void IsInstallationExceptionThrowsArgumentNullException()
		{
			Installer.IsInstallationException(null);
		}

		[Test]
		public void IsInstallationExceptionReturnsFalseForNonSqlException()
		{
			Assert.IsFalse(Installer.IsInstallationException(new Exception("Test")));
		}

		[Test]
		public void CanGetInstallationStatus()
		{
			using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
			{
				connection.Open();
				SqlInstaller installer = new SqlInstaller(Config.ConnectionString);
				Console.WriteLine("UPDATING INSTALLATION VERSION TO '{0}'!", installer.SubtextAssemblyVersion);
				installer.UpdateInstallationVersionNumber(installer.SubtextAssemblyVersion);
			}
			Assert.AreEqual(InstallationState.Complete, Installer.InstallationStatus);
		}
	}
}
