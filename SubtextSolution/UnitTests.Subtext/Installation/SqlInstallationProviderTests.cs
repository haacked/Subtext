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
using MbUnit.Framework;
using Subtext.Installation;

namespace UnitTests.Subtext.Installation
{
	/// <summary>
	/// Tests of the <see cref="InstallerProvider"/> class.
	/// </summary>
	[TestFixture]
	public class SqlInstallerProviderTests
	{
		/// <summary>
		/// Make sure that the process in which we gather installation information 
		/// properly gathers the information.
		/// </summary>
		[Test]
		[RollBack2]
		public void InstallationInformationGatheringProcessGathersCorrectInfo()
		{
			InstallerProvider provider = Installer.Provider;
			Assert.IsNotNull(provider, "The provider instance should not be null.");
			SqlInstallationProvider sqlProvider = provider as SqlInstallationProvider;
			Assert.IsNotNull(sqlProvider, "The sql provider instance should not be null.");
			Assert.AreEqual("SqlInstallationProvider", provider.Name);

		
			//Ok, no way to really check this just yet.
		}

		/// <summary>
		/// Tests that we can properly list the installation scripts.
		/// </summary>
		[Test]
		public void ListInstallationScriptsReturnsCorrectScripts()
		{
			SqlInstaller installer = new SqlInstaller("null");
			string[] scripts = installer.ListInstallationScripts(null, new Version(1, 5, 0, 0));
			Assert.AreEqual(2, scripts.Length, "We expected to see two scripts.");
			Assert.AreEqual("Installation.01.00.00.sql", scripts[0], "Expected the initial 1.0 installation file.");
			Assert.AreEqual("Installation.01.05.00.sql", scripts[1], "Expected the bugfix 1.5 installation file.");

			scripts = installer.ListInstallationScripts(null, new Version(1, 0, 3, 0));
			Assert.AreEqual(1, scripts.Length, "We expected to see one script.");
			Assert.AreEqual("Installation.01.00.00.sql", scripts[0], "Expected the initial 1.0 installation file.");

			scripts = installer.ListInstallationScripts(null, new Version(0, 0, 3, 0));
			Assert.AreEqual(0, scripts.Length, "We expected to see no scripts.");
			
			scripts = installer.ListInstallationScripts(new Version(1, 1, 0, 0), new Version(1, 5, 0, 0));
			Assert.AreEqual(1, scripts.Length, "We expected to see one script.");
			Assert.AreEqual("Installation.01.05.00.sql", scripts[0], "Expected the bugfix 1.5.0 installation file.");

			scripts = installer.ListInstallationScripts(new Version(1, 1, 0, 0), new Version(1, 9, 0, 0));
			Assert.AreEqual(2, scripts.Length, "We expected to see two script.");
			Assert.AreEqual("Installation.01.09.00.sql", scripts[1], "Expected the 1.9.0 installation file.");
		}

		/// <summary>
		/// Called before each unit test.
		/// </summary>
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			//Confirm app settings
            UnitTestHelper.AssertAppSettings();
		}
	}
}
