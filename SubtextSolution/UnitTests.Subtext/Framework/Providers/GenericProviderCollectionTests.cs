using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Providers
{
	[TestFixture]
	public class GenericProviderCollectionTests
	{
		[Test]
		public void CanAddProvider()
		{
			GenericProviderCollection<SystemMailProvider> providers = new GenericProviderCollection<SystemMailProvider>();
			Assert.AreEqual(0, providers.Count);
			SystemMailProvider provider = new SystemMailProvider();
			provider.Initialize("EmailProvider", new NameValueCollection());
			providers.Add(provider);
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
