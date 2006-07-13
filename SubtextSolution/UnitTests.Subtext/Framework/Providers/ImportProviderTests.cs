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

namespace UnitTests.Subtext.Framework.Providers
{
	/// <summary>
	/// Summary description for ImportProviderTests.
	/// </summary>
	[TestFixture]
	public class ImportProviderTests
	{
		/// <summary>
		/// Makes sure that the static <see cref="ImportProvider.Providers"/> property 
		/// returns all the configured providers.
		/// </summary>
		[Test]
		public void ProvidersPropertyReturnsAllImportProviders()
		{
            System.Configuration.Provider.ProviderCollection providers = ImportProvider.Providers;
			Assert.IsNotNull(providers, "That didn't work out too well. It's null.");
			Assert.AreEqual(2, providers.Count, "Not the number of providers expected.");
		}

		/// <summary>
		/// Make sure that we can instantiate the non-default provider.
		/// </summary>
		[Test]
		public void InstanceCanReturnNonDefaultProvider()
		{
            System.Configuration.Provider.ProviderCollection providers = ImportProvider.Providers;

			UnitTestImportProvider provider = (UnitTestImportProvider)providers["UnitTestImportProvider"];
			Assert.AreEqual("UnitTestImportProvider", provider.Name);
		}
	}
}