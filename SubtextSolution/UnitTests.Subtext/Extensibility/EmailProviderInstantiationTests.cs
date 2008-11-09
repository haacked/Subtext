using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Email;

namespace UnitTests.Subtext.Extensibility
{
	/// <summary>
	/// Summary description for ProviderInstantiationTests.
	/// </summary>
	[TypeFixture(typeof(EmailProvider))]
	[ProviderFactory(typeof(EmailProviderFactory), typeof(EmailProvider))]
	public class EmailProviderInstantiationTests
	{
		[Test]
		public void CanInstantiateAndInitializeEmailProvider(EmailProvider provider)
		{
			Assert.IsNotNull(provider, "Well the provider should not be null.");
			NameValueCollection configValue = new NameValueCollection();
			configValue["adminEmail"] = "admin@example.com";
			configValue["smtpServer"] = "smtp.example.com";
			configValue["password"] = "abracadabra";;
			configValue["username"] = "haacked";
			provider.Initialize("providerTest", configValue);
			
			Assert.AreEqual("admin@example.com", provider.AdminEmail, "Did not initialize the admin email properly.");
			Assert.AreEqual("smtp.example.com", provider.SmtpServer, "Did not initialize the SMTP server properly.");
			Assert.AreEqual("abracadabra", provider.Password, "Did not initialize the password properly.");
			Assert.AreEqual("haacked", provider.UserName, "Did not initialize the username properly.");
		}
		
        //
        //Removed since this method doesn't exists any more (simo)
        //
        //[Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void GetSettingValueThrowsArgumentNullExceptionForNullSettingKey(EmailProvider provider)
        //{
        //    provider.GetSettingValue(null, new NameValueCollection());
        //}
		
        //[Test]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void GetSettingValueThrowsArgumentNullExceptionForNullConfigValues(EmailProvider provider)
        //{
        //    provider.GetSettingValue("Section", null);
        //}
	}
	
	internal class EmailProviderFactory
	{
		[Factory]
		public EmailProvider SystemMailProvider
		{
			get
			{
				return new SystemMailProvider();
			}
		}
	}
}
