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
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;

namespace UnitTests.Subtext.Installation
{
	/// <summary>
	/// Tests of the InstallationManager class.
	/// </summary>
	[TestFixture]
	public class InstallationManagerTests
	{
		/// <summary>
		/// Determines whether [is in host admin directory returns true result].
		/// </summary>
		[Test]
		[RollBack]
		public void IsInHostAdminDirectoryReturnsTrueResult()
		{
			string host = System.Guid.NewGuid().ToString().Replace("-", "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "Subtext.Web", "HostAdmin/Import/BlahBlah.aspx");
			Assert.IsTrue(InstallationManager.IsInHostAdminDirectory, "This request should be within the hostadmin/import directory.");	
		}

		/// <summary>
		/// Makes sure that a <see cref="BlogDoesNotExistException"/> indicates that 
		/// an installation action is required.
		/// </summary>
		[Test]
		[RollBack]
		public void IsInstallationActionRequiredReturnsTrueForBlogDoesNotExistException()
		{
			Assert.IsTrue(InstallationManager.InstallationActionRequired(new BlogDoesNotExistException("host", "app", false), VersionInfo.FrameworkVersion));
		}

		/// <summary>
		/// Determines whether [is in install directory reports correct result].
		/// </summary>
		[Test]
		[RollBack]
		public void IsInInstallDirectoryReportsTrueCorrectly()
		{
			AssertIsInInstallDirectory("VirtDir", System.Guid.NewGuid().ToString().Replace("-", ""));
			AssertIsInInstallDirectory("", System.Guid.NewGuid().ToString().Replace("-", ""));
			AssertIsInInstallDirectory("", "");
		}

		/// <summary>
		/// Determines whether [is in install directory reports correct result].
		/// </summary>
		[Test]
		[RollBack]
		public void IsInInstallDirectoryReportsFalseCorrectly()
		{
			AssertNotInInstallDirectory(System.Guid.NewGuid().ToString().Replace("-", ""), "");
			AssertNotInInstallDirectory(System.Guid.NewGuid().ToString().Replace("-", ""), "VirtDir");
			AssertNotInInstallDirectory("", "");
		}

		void AssertIsInInstallDirectory(string virtualDirectory, string blogName)
		{
			string host = System.Guid.NewGuid().ToString().Replace("-", "");
			Config.CreateBlog("AssertIsInInstallDirectory", "username", "thePassword", host, blogName);
			UnitTestHelper.SetHttpContextWithBlogRequest(host, blogName, virtualDirectory, "Install/InstallationComplete.aspx");
			Assert.IsTrue(InstallationManager.IsInInstallDirectory, "This request should be within the installation directory.");	
		}

		void AssertNotInInstallDirectory(string virtualDirectory, string blogName)
		{
			string host = System.Guid.NewGuid().ToString().Replace("-", "");
			Config.CreateBlog("Title", "username", "thePassword", host, blogName);

			UnitTestHelper.SetHttpContextWithBlogRequest(host, blogName, virtualDirectory, "Admin/InstallationComplete.aspx");
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
