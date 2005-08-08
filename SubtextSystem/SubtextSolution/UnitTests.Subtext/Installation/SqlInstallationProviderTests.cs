using System;
using NUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Installation;

namespace UnitTests.Subtext.Installation
{
	/// <summary>
	/// Tests of the <see cref="SqlInstallationProvider"/> class.
	/// </summary>
	[TestFixture]
	public class SqlInstallationProviderTests
	{
		/// <summary>
		/// Make sure that the process in which we gather installation information 
		/// properly gathers the information.
		/// </summary>
		[Test]
		[Rollback]
		public void InstallationInformationGatheringProcessGathersCorrectInfo()
		{
			InstallationProvider provider = InstallationProvider.Instance();
			Assert.IsNotNull(provider, "The provider instance should not be null.");
			SqlInstallationProvider sqlProvider = provider as SqlInstallationProvider;
			Assert.IsNotNull(sqlProvider, "The sql provider instance should not be null.");
			Assert.AreEqual("SqlInstallationProvider", provider.Name);

		
			//Ok, no way to really check this just yet.
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
