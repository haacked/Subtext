using System;
using NUnit.Framework;
using Subtext.Framework;
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
		public void IsInstallationActionRequiredReturnsTrueForBlogDoesNotExistException()
		{
			Assert.IsTrue(InstallationManager.GetIsInstallationActionRequired(new BlogDoesNotExistException("host", "app", false), VersionInfo.FrameworkVersion));
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
