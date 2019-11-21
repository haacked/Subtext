using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestClass]
    public class GravatarServiceTests
    {
        [TestMethod]
        public void GenerateUrlHashesEmailAddress()
        {
            //arrange
            var service = new GravatarService("{0}", true);

            //act
            string url = service.GenerateUrl("test@example.com");

            //assert
            StringAssert.Contains(url, "55502f40dc8b7c769880b10874abc9d0");
        }

        [TestMethod]
        public void GenerateUrlUsesIdenticonForDefaultImage()
        {
            //arrange
            var service = new GravatarService(
                "http://gravatar.com/avatar/{0}?d={1}",
                true);

            //act
            string url = service.GenerateUrl("test@example.com");

            //assert
            Assert.AreEqual(
                url,
                "http://gravatar.com/avatar/55502f40dc8b7c769880b10874abc9d0"
                    + "?d=identicon");
        }

        [TestMethod]
        public void GenerateUrlSupportsCustomDefaultImage()
        {
            //arrange
            var service = new GravatarService(
                "http://gravatar.com/avatar/{0}?d={1}",
                true);

            const string defaultImage = "https://technologytoolbox.com"
                + "/blog/Skins/TechnologyToolbox1"
                + "/Images/Silhouette-1.jpg";

            //act
            string url = service.GenerateUrl("test@example.com", defaultImage);

            //assert
            Assert.AreEqual(
                url,
                "http://gravatar.com/avatar/55502f40dc8b7c769880b10874abc9d0"
                    + "?d="
                    + "https%3a%2f%2ftechnologytoolbox.com"
                        + "%2fblog%2fSkins%2fTechnologyToolbox1"
                        + "%2fImages%2fSilhouette-1.jpg");
        }

        [TestMethod]
        public void CanCreateGravatarServiceWithNameValueCollection()
        {
            //arrange
            var settings = new NameValueCollection
            {
                {"GravatarEnabled", "true"},
                {"GravatarUrlFormatString", "{0}/{1}"}
            };

            //act
            var service = new GravatarService(settings);

            //assert
            Assert.IsTrue(service.Enabled);
            Assert.AreEqual("{0}/{1}", service.UrlFormatString);
        }

        [TestMethod]
        public void WhenCreatingGravatarServiceWithNullBoolean_NoExceptionIsThrown()
        {
            //arrange
            var settings = new NameValueCollection
            {
                {"GravatarEnabled", null},
                {"GravatarUrlFormatString", "{0}/{1}"}
            };

            //act
            var service = new GravatarService(settings);

            //assert
            Assert.IsFalse(service.Enabled);
            Assert.AreEqual("{0}/{1}", service.UrlFormatString);
        }

        [TestMethod]
        public void WhenCreatingGravatarServiceWithNonsensicalBoolean_NoExceptionIsThrown()
        {
            //arrange
            var settings = new NameValueCollection
            {
                {"GravatarEnabled", "Blablabla"},
                {"GravatarUrlFormatString", "{0}/{1}"}
            };

            //act
            var service = new GravatarService(settings);

            //assert
            Assert.IsFalse(service.Enabled);
            Assert.AreEqual("{0}/{1}", service.UrlFormatString);
        }
    }
}