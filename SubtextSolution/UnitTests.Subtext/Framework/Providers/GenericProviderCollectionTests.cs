using System;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Providers
{
	[TestFixture]
	public class GenericProviderCollectionTests
	{
		[Test]
		public void CanAddProvider()
		{
			GenericProviderCollection<DatabaseObjectProvider> providers = new GenericProviderCollection<DatabaseObjectProvider>();
			Assert.AreEqual(0, providers.Count);
			providers.Add(new DatabaseObjectProvider());
			Assert.AreEqual(1, providers.Count);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void AddThrowsArgumentNullException()
		{
			GenericProviderCollection<EmailProvider> providers = new GenericProviderCollection<EmailProvider>();
			providers.Add(null);
		}

		[Test]
		[ExpectedArgumentException]
		public void AddThrowsArgumentExceptionIfProviderDoesNotMatchT()
		{
			GenericProviderCollection<EmailProvider> providers = new GenericProviderCollection<EmailProvider>();
			providers.Add(new DatabaseObjectProvider());
		}
	}
}
