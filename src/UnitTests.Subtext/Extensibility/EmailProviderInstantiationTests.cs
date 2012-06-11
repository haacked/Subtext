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
        public void Initialize_WithNullPort_UsesDefaultPort(EmailProvider provider)
        {
            // arrange
            var configValue = new NameValueCollection();
            configValue["port"] = null;
            
            // act
            provider.Initialize("providerTest", configValue);

            // assert
            Assert.AreEqual(25, provider.Port);
        }

        [Test]
        public void Initialize_WithValuesFromConfig_SetsConfigProperties(EmailProvider provider)
        {
            // arrange
            var configValue = new NameValueCollection();
            configValue["adminEmail"] = "admin@example.com";
            configValue["smtpServer"] = "smtp.example.com";
            configValue["password"] = "abracadabra";
            configValue["username"] = "haacked";

            // act
            provider.Initialize("providerTest", configValue);

            // assert
            Assert.AreEqual("admin@example.com", provider.AdminEmail, "Did not initialize the admin email properly.");
            Assert.AreEqual("smtp.example.com", provider.SmtpServer, "Did not initialize the SMTP server properly.");
            Assert.AreEqual("abracadabra", provider.Password, "Did not initialize the password properly.");
            Assert.AreEqual("haacked", provider.UserName, "Did not initialize the username properly.");
        }
    }

    internal class EmailProviderFactory
    {
        [Factory]
        public EmailProvider SystemMailProvider
        {
            get { return new SystemMailProvider(); }
        }
    }
}