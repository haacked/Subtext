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
using System.Data.Common;
using System.Web;
using MbUnit.Framework;
using Rhino.Mocks;
using SubSonic;
using Subtext.Framework;
using Subtext.Framework.Exceptions;
using Subtext.Installation;
using Subtext.TestLibrary;

namespace UnitTests.Subtext.Installation
{
	/// <summary>
	/// Tests of the InstallationManager class.
	/// </summary>
	[TestFixture]
	public class InstallationManagerTests
	{
		[Test]
		[RollBack2]
		public void CanGetCurrentInstallationState()
		{
			QueryCommand command = new QueryCommand("Delete subtext_Version");
			DataService.ExecuteQuery(command);
			Assert.AreEqual(InstallationState.NeedsInstallation, InstallationManager.CurrentInstallationState);
		}

		/// <summary>
		/// Determines whether [is in host admin directory returns true result].
		/// </summary>
		[Test]
		[RollBack2]
		public void IsInHostAdminDirectoryReturnsTrueResult()
		{
			UnitTestHelper.SetupBlog(string.Empty, "Subtext.Web", "HostAdmin/Import/BlahBlah.aspx");
			Assert.IsTrue(InstallationManager.IsInHostAdminDirectory, "This request should be within the hostadmin/import directory.");	
		}

		/// <summary>
		/// Determines whether [is in host admin directory returns true result].
		/// </summary>
		[Test]
		[RollBack2]
		public void IsInUpgradeDirectoryReturnsTrueResult()
		{
			UnitTestHelper.SetupBlog(string.Empty, "Subtext.Web", "HostAdmin/Upgrade/BlahBlah.aspx");
			Assert.IsTrue(InstallationManager.IsInUpgradeDirectory, "This request should be within the hostadmin/upgrade directory.");
		}

		/// <summary>
		/// Determines whether [is in host admin directory returns true result].
		/// </summary>
		[Test]
		[RollBack2]
		public void IsInSystemMessageDirectoryReturnsTrueResult()
		{
			UnitTestHelper.SetupBlog(string.Empty, "Subtext.Web", "SystemMessages/BlahBlah.aspx");
			Assert.IsTrue(InstallationManager.IsInSystemMessageDirectory, "This request should be within the SystemMessages directory.");
		}

		[Test]
		public void IsIntstallationActionRequiredReturnsContextVariable()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				HttpContext.Current.Application["NeedsInstallation"] = false;
				Assert.IsFalse(InstallationManager.IsInstallationActionRequired());
			}
		}

		[Test]
		public void CanResetInstallationStatusCache()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				HttpContext.Current.Application["NeedsInstallation"] = true;

				Assert.IsTrue((bool) HttpContext.Current.Application["NeedsInstallation"]);
				InstallationManager.ResetInstallationStatusCache();
				Assert.IsNull(HttpContext.Current.Application["NeedsInstallation"]);
			}
		}

		[Test]
		[RollBack2]
		public void IsIntstallationActionRequiredReturnsTrue()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				Assert.IsNull(HttpContext.Current.Application["NeedsInstallation"]);
				QueryCommand command = new QueryCommand("Delete subtext_Version");
				DataService.ExecuteQuery(command);
				Assert.IsTrue(InstallationManager.IsInstallationActionRequired());
				Assert.IsNotNull(HttpContext.Current.Application["NeedsInstallation"]);
			}
		}

		/// <summary>
		/// Makes sure that a <see cref="BlogDoesNotExistException"/> indicates that 
		/// an installation action is required.
		/// </summary>
		[Test]
		[RollBack2]
		public void IsInstallationActionRequiredReturnsCorrectAnswerForExceptions()
		{
			Assert.IsFalse(InstallationManager.InstallationActionRequired(new Exception("host"), VersionInfo.FrameworkVersion));
			Assert.IsTrue(InstallationManager.InstallationActionRequired(new BlogDoesNotExistException("host", "app", false), VersionInfo.FrameworkVersion));
			Assert.IsTrue(InstallationManager.InstallationActionRequired(new HostDataDoesNotExistException("message"), VersionInfo.FrameworkVersion));
			Assert.IsTrue(InstallationManager.InstallationActionRequired(new HostNotConfiguredException("message"), VersionInfo.FrameworkVersion));

			MockRepository mocks = new MockRepository();
			DbException invalidObjectException = mocks.DynamicMock<DbException>();
			DbException storedProcException = mocks.DynamicMock<DbException>();
			
			using (mocks.Record())
			{
				SetupResult.For(invalidObjectException.Message).Return("Invalid object name 'blah'");
				SetupResult.For(storedProcException.Message).Return("'Could not find stored procedure 'blah'");
			}
			using (mocks.Playback())
			{
				Assert.IsTrue(InstallationManager.InstallationActionRequired(invalidObjectException, VersionInfo.FrameworkVersion));
				Assert.IsTrue(InstallationManager.InstallationActionRequired(storedProcException, VersionInfo.FrameworkVersion));
			}

			QueryCommand command = new QueryCommand("Delete subtext_Version");
			DataService.ExecuteQuery(command);
			Assert.IsTrue(InstallationManager.InstallationActionRequired(new Exception("host"), VersionInfo.FrameworkVersion));
		}

		/// <summary>
		/// Determines whether [is in install directory reports correct result].
		/// </summary>
		[Test]
		[RollBack2]
		public void IsInInstallDirectoryReportsTrueCorrectly()
		{
			AssertIsInInstallDirectory("VirtDir", UnitTestHelper.GenerateRandomString());
			AssertIsInInstallDirectory("", UnitTestHelper.GenerateRandomString());
			AssertIsInInstallDirectory("", "");
		}

		/// <summary>
		/// Determines whether [is in install directory reports correct result].
		/// </summary>
		[Test]
		[RollBack2]
		public void IsInInstallDirectoryReportsFalseCorrectly()
		{
			AssertNotInInstallDirectory(UnitTestHelper.GenerateRandomString(), "");
			AssertNotInInstallDirectory(UnitTestHelper.GenerateRandomString(), "VirtDir");
			AssertNotInInstallDirectory("", "");
		}

		static void AssertIsInInstallDirectory(string virtualDirectory, string subfolder)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDirectory, "Install/Default.aspx");
			Assert.IsTrue(InstallationManager.IsInInstallDirectory, "This request should be within the installation directory.");	
		}

		static void AssertNotInInstallDirectory(string virtualDirectory, string subfolder)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDirectory, "Admin/Default.aspx");
			Assert.IsFalse(InstallationManager.IsInInstallDirectory, "This request is indeed within the installation directory.");	
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
