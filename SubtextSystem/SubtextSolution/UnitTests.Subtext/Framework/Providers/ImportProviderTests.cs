using System;
using NUnit.Framework;
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
			ProviderCollection providers = ImportProvider.Providers;
			Assert.IsNotNull(providers, "That didn't work out too well. It's null.");
			Assert.AreEqual(1, providers.Count, "Not the number of providers expected.");
		}
	}
}