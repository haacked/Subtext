using System;
using NUnit.Framework;
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
		/// Makes sure that a <see cref="BlogDoesNotExistException"/> indicates that 
		/// an installation action is required.
		/// </summary>
		[Test]
		[Rollback]
		public void IsInstallationActionRequiredReturnsTrueForBlogDoesNotExistException()
		{
			Assert.IsTrue(InstallationManager.GetIsInstallationActionRequired(new BlogDoesNotExistException("host", "app", false), VersionInfo.FrameworkVersion));
		}

		/// <summary>
		/// Determines whether [is in install directory reports correct result].
		/// </summary>
		[Test]
		[Rollback]
		public void IsInInstallDirectoryReportsTrueCorrectly()
		{
			AssertIsInInstallDirectory(System.Guid.NewGuid().ToString().Replace("-", ""), "VirtDir");
			AssertIsInInstallDirectory(System.Guid.NewGuid().ToString().Replace("-", ""), "");
			AssertIsInInstallDirectory("", "");
		}

		/// <summary>
		/// Determines whether [is in install directory reports correct result].
		/// </summary>
		[Test]
		[Rollback]
		public void IsInInstallDirectoryReportsFalseCorrectly()
		{
			AssertNotInInstallDirectory(System.Guid.NewGuid().ToString().Replace("-", ""), "");
			AssertNotInInstallDirectory(System.Guid.NewGuid().ToString().Replace("-", ""), "VirtDir");
			AssertNotInInstallDirectory("", "");
		}

		void AssertIsInInstallDirectory(string virtualDirectory, string blogName)
		{
			string host = System.Guid.NewGuid().ToString().Replace("-", "");
			Config.CreateBlog("Title", "username", "thePassword", host, blogName);

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
			//This file needs to be there already.
			UnitTestHelper.UnpackEmbeddedResource("App.config", "UnitTests.Subtext.dll.config");
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}
	}
}
