using System;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;

namespace UnitTests.Subtext.Extensibility
{
	/// <summary>
	/// Summary description for ProviderBaseTests.
	/// </summary>
	[TestFixture]
	public class ProviderBaseTests
	{
		[Test]
		[ExpectedException(typeof(ProviderInstantiationException))]
		public void ThrowsProviderInstantiationExceptionWhenInvalidTypeSpecified()
		{
			ProviderInfo providerInfo = UnitTestHelper.CreateProviderInfoInstance("test", "test");
			ProviderBase.Instance("Section", providerInfo);
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullSectionNameThrowsException()
		{
			ProviderInfo providerInfo = UnitTestHelper.CreateProviderInfoInstance("test", "test");
			ProviderBase.Instance(null, providerInfo);
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullProviderInfoThrowsException()
		{
			ProviderBase.Instance("Section", null);
		}
	}
}
