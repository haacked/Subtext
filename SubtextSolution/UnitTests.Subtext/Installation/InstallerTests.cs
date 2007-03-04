using System;
using MbUnit.Framework;
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
			Assert.AreEqual(InstallationState.Complete, Installer.InstallationStatus);
		}
	}
}
