using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Net;
using Subtext.Framework.Providers;

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
		public void CanSetAdminEmail(EmailProvider provider)
		{
			provider.AdminEmail = "unittest@example.com";
			Assert.AreEqual("unittest@example.com", provider.AdminEmail);
		}

		[Test]
		public void DefaultSmtpServerSetIfNull(EmailProvider provider)
		{
			provider.SmtpServer = null;
			Assert.AreEqual("localhost", provider.SmtpServer);
		}

		[Test]
		public void CanSetPort(EmailProvider provider)
		{
			provider.Port = 99;
			Assert.AreEqual(99, provider.Port);
		}

		[Test]
		public void CanSetUserName(EmailProvider provider)
		{
			provider.UserName = "Me";
			Assert.AreEqual("Me", provider.UserName);
		}

		[Test]
		public void CanSetPassword(EmailProvider provider)
		{
			provider.Password = "Super-Secret";
			Assert.AreEqual("Super-Secret", provider.Password);
		}

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

		#region Exception Tests
		[Test]
		[ExpectedArgumentNullException]
		public void InitializeThrowsArgumentNullExceptionForNullName(EmailProvider provider)
		{
			provider.Initialize(null, new NameValueCollection());
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InitializeThrowsArgumentNullExceptionForNullNameValueCollection(EmailProvider provider)
		{
			provider.Initialize("Email", null);
		}
		#endregion
	}
	
	internal class EmailProviderFactory
	{
		[Factory]
		public EmailProvider DotNetOpenMailProvider
		{
			get
			{
				return new DotNetOpenMailProvider();
			}
		}
		
		[Factory]
		public EmailProvider SystemMailProvider
		{
			get
			{
				return new SystemMailProvider();
			}
		}
	}

	[TestFixture]
	public class OtherEmailProviderTests
	{
		[Test]
		public void CanGetProviders()
		{
			Assert.AreEqual(3, Email.Providers.Count);
		}

		[Test]
		public void CanGetProvider()
		{
			Assert.AreEqual(typeof(UnitTestEmailProvider), Email.Provider.GetType());
		}

		[Test]
		public void SendDelegatesToProvider()
		{
			Assert.IsTrue(Email.Send("to", "from", "subject", "message"));
		}
	}
}
