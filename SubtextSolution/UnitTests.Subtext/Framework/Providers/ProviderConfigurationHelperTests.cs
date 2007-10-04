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

using System.Collections.Specialized;
using System.Configuration.Provider;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Providers
{
	/// <summary>
	/// Tests a few methods of the <see cref="ProviderBase"/> class.
	/// </summary>
	[TestFixture]
    public class ProviderConfigurationHelperTests
	{
	    [Test]
	    public void CanFindConnectionString()
	    {
			Assert.IsNotNull(Config.ConnectionString);
	    }
	    
		/// <summary>
		/// Makes sure that we can recognize false setting pointers.
		/// </summary>
		[Test]
		public void GetConnectionStringSettingValueFindsConnectionString()
		{
			/*
			NameValueCollection configValue = new NameValueCollection();
			configValue.Add("connectionStringName", "subtextData");	
            Assert.AreEqual("Server=localhost;Database=SubtextData;Trusted_Connection=True;", ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName", configValue));
			 */
		}

		[Test]
		[ExpectedException(typeof(ProviderException))]
		public void LoadProviderCollectionThrowsProviderExceptionWhenDefaultProviderIsNull()
		{
			FakeProvider provider;
			ProviderConfigurationHelper.LoadProviderCollection("FakeProvider", out provider);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void GetConnectionStringSettingByValueThrowsArgumentNullExceptionForNullSettingKey()
		{
			ProviderConfigurationHelper.GetConnectionStringSettingValue(null, new NameValueCollection());
		}

		[Test]
		[ExpectedArgumentNullException]
		public void GetConnectionStringSettingByValueThrowsArgumentNullExceptionForNullConfigValues()
		{
			ProviderConfigurationHelper.GetConnectionStringSettingValue("Test", null);
		}

		[Test]
		[ExpectedArgumentException]
		public void GetConnectionStringSettingByValueThrowsArgumentExceptionForNonExistentConnectionString()
		{
			NameValueCollection config = new NameValueCollection();
			config.Add("connectionStringName", "nonexistentConnectionString");
			ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName", config);
		}

		[Test]
		public void GetConnectionStringSettingByValueReturnsNullIfSettingKeyIsNotConnectionStringName()
		{
			NameValueCollection config = new NameValueCollection();
			config.Add("blah", "nonexistentConnectionString");
			Assert.IsNull(ProviderConfigurationHelper.GetConnectionStringSettingValue("blah", config));
		}

		[Test]
		public void CanSetDefaultProvider()
		{
			ProviderSectionHandler handler = new ProviderSectionHandler();
			handler.DefaultProvider = "Test";
			Assert.AreEqual("Test", handler.DefaultProvider);
		}
	}

	public class FakeProvider : ProviderBase
	{ }
}
