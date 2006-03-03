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
		[RollBack]
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
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}
	}
}
