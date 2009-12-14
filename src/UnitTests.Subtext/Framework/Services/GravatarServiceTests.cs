using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class GravatarServiceTests
    {
        [Test]
        public void GenerateUrlHashesEmailAddress()
        {
            //arrange
            var service = new GravatarService("{0}", GravatarEmailFormat.Md5, true);

            //act
            string url = service.GenerateUrl("test@example.com", (string)null);

            //assert
            Assert.Contains(url, "55502f40dc8b7c769880b10874abc9d0");
        }

        [Test]
        public void GenerateUrlFormatsEmail()
        {
            //arrange
            var service = new GravatarService("http://example.com/{0}/sike", GravatarEmailFormat.Plain, true);

            //act
            string url = service.GenerateUrl("test@example.com", (string)null);

            //assert
            Assert.Contains(url, "http://example.com/test%40example.com/sike");
        }

        [Test]
        public void GenerateUrlEncodesEmail()
        {
            //arrange
            var service = new GravatarService("{0}", GravatarEmailFormat.Plain, true);

            //act
            string url = service.GenerateUrl("te st@example.com", (string)null);

            //assert
            Assert.Contains(url, "te+st%40example.com");
        }

        [Test]
        public void GenerateUrlWithNullDefaultUsesIdenticon()
        {
            //arrange
            var service = new GravatarService("{0}/{1}", GravatarEmailFormat.Plain, true);

            //act
            string url = service.GenerateUrl("test@example.com", (string)null);

            //assert
            Assert.AreEqual("test%40example.com/identicon", url);
        }

        [Test]
        public void GenerateUrlWithDefaultIncludesDefault()
        {
            //arrange
            var service = new GravatarService("{0}/{1}", GravatarEmailFormat.Plain, true);

            //act
            string url = service.GenerateUrl("test@example.com", "http://localhost/test.gif");

            //assert
            Assert.AreEqual("test%40example.com/http://localhost/test.gif", url);
        }

        [Test]
        public void GenerateUrlWithDefaultUriIncludesDefaultUri()
        {
            //arrange
            var service = new GravatarService("{0}/{1}", GravatarEmailFormat.Plain, true);

            //act
            string url = service.GenerateUrl("test@example.com", new Uri("http://localhost/test.gif"));

            //assert
            Assert.AreEqual("test%40example.com/http://localhost/test.gif", url);
        }

        [Test]
        public void GenerateUrlWithEmptyEmailReturnsEmptyString()
        {
            //arrange
            var service = new GravatarService("{0}/{1}", GravatarEmailFormat.Plain, true);

            //act
            string url = service.GenerateUrl(string.Empty, (string)null);

            //assert
            Assert.AreEqual(string.Empty, url);
        }

        [Test]
        public void CanCreateGravatarServiceWithNameValueCollection()
        {
            //arrange
            var settings = new NameValueCollection
            {
                {"GravatarEnabled", "true"},
                {"GravatarUrlFormatString", "{0}/{1}"},
                {"GravatarEmailFormat", "md5"}
            };

            //act
            var service = new GravatarService(settings);

            //assert
            Assert.IsTrue(service.Enabled);
            Assert.AreEqual("{0}/{1}", service.UrlFormatString);
            Assert.AreEqual(GravatarEmailFormat.Md5, service.EmailFormat);
        }

        [Test]
        public void WhenCreatingGravatarServiceWithNullBoolean_NoExceptionIsThrown()
        {
            //arrange
            var settings = new NameValueCollection
            {
                {"GravatarEnabled", null},
                {"GravatarUrlFormatString", "{0}/{1}"},
                {"GravatarEmailFormat", "md5"}
            };

            //act
            var service = new GravatarService(settings);

            //assert
            Assert.IsFalse(service.Enabled);
            Assert.AreEqual("{0}/{1}", service.UrlFormatString);
            Assert.AreEqual(GravatarEmailFormat.Md5, service.EmailFormat);
        }

        [Test]
        public void WhenCreatingGravatarServiceWithNonsensicalBoolean_NoExceptionIsThrown()
        {
            //arrange
            var settings = new NameValueCollection
            {
                {"GravatarEnabled", "Blablabla"},
                {"GravatarUrlFormatString", "{0}/{1}"},
                {"GravatarEmailFormat", "PLAIN"}
            };

            //act
            var service = new GravatarService(settings);

            //assert
            Assert.IsFalse(service.Enabled);
            Assert.AreEqual("{0}/{1}", service.UrlFormatString);
            Assert.AreEqual(GravatarEmailFormat.Plain, service.EmailFormat);
        }
    }
}