using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Email;

namespace UnitTests.Subtext.Extensibility
{
    /// <summary>
    /// Summary description for ProviderInstantiationTests.
    /// </summary>
    [TestClass]
    public class EmailProviderInstantiationTests
    {
        [TestMethod]
        public void Initialize_WithNullPort_UsesDefaultPort()
        {
            // arrange
            var provider = new SystemMailProvider();
            var configValue = new NameValueCollection();
            configValue["port"] = null;
            
            // act
            provider.Initialize("providerTest", configValue);

            // assert
            Assert.AreEqual(25, provider.Port);
        }

        [TestMethod]
        public void Initialize_WithValuesFromConfig_SetsConfigProperties()
        {
            // arrange
            var provider = new SystemMailProvider();
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
}
