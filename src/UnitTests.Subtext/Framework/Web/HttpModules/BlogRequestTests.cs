using System;
using System.Collections.Specialized;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Web.HttpModules
{
    [TestFixture]
    public class BlogRequestTests
    {
        [Test]
        public void Ctor_WithRequestWithSubfolder_CreatesBlogRequestWithSubfolder()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com", "/", "/foo/bar", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual("foo", blogRequest.Subfolder);
        }

        [Test]
        public void Ctor_WithHostHavingPort_StripsPort()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com:1234", "/", "/foo/bar", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual("example.com", blogRequest.Host);
        }

        [Test]
        public void Ctor_WithRequestHavingNoHostInParameters_CreatesBlogRequestWithHostAuthority()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com", "/", "/foo/bar", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual("example.com", blogRequest.Host);
        }

        [Test]
        public void Ctor_WithRequestForLoginPage_SetsRequestLocationToLogin()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com", "/", "/login.aspx", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.LoginPage, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForSystemMessage_SetsRequestLocationToSystemMessages()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com", "/", "/SystemMessages/anything.aspx", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.SystemMessages, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForHostAdmin_SetsRequestLocationToHostAdmin()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com", "/", "/HostAdmin/anything.aspx", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.HostAdmin, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForInstallDirectoryRoot_SetsRequestLocationToInstallDirectory()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com", "/", "/Install", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.Installation, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForInstallDirectory_SetsRequestLocationToInstallDirectory()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com", "/", "/Install/anything.aspx", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.Installation, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForStaticFile_SetsRequestLocationToStaticFile()
        {
            //arrange
            Mock<HttpRequestBase> request = CreateRequest("example.com", "/", "/Install/anything.css", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.StaticFile, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForBlog_SetsBlogNotRequiredFalse()
        {
            //arrange, act
            var blogRequest = new BlogRequest(null, null, new Uri("http://example.com"), false, RequestLocation.Blog,
                                              "/");

            //assert
            Assert.IsFalse(blogRequest.BlogNotRequired);
        }

        [Test]
        public void Ctor_WithRequestForHostAdmin_SetsBlogNotRequiredTrue()
        {
            //arrange, act
            var blogRequest = new BlogRequest(null, null, new Uri("http://example.com"), false,
                                              RequestLocation.HostAdmin, "/");

            //assert
            Assert.IsTrue(blogRequest.BlogNotRequired);
        }

        [Test]
        public void Ctor_WithRequestForUpgrade_SetsBlogNotRequiredTrue()
        {
            //arrange, act
            var blogRequest = new BlogRequest(null, null, new Uri("http://example.com"), false, RequestLocation.Upgrade,
                                              "/");

            //assert
            Assert.IsTrue(blogRequest.BlogNotRequired);
        }

        [Test]
        public void Ctor_WithRequestForSkins_SetsBlogNotRequiredTrue()
        {
            //arrange, act
            var blogRequest = new BlogRequest(null, null, new Uri("http://example.com"), false, RequestLocation.Skins,
                                              "/");

            //assert
            Assert.IsTrue(blogRequest.BlogNotRequired);
        }

        [Test]
        public void Ctor_WithRequestForStaticFile_SetsBlogNotRequiredTrue()
        {
            //arrange, act
            var blogRequest = new BlogRequest(null, null, new Uri("http://example.com"), false,
                                              RequestLocation.StaticFile, "/");

            //assert
            Assert.IsTrue(blogRequest.BlogNotRequired);
        }

        [Test]
        public void Ctor_WithRequestForSystemMessages_SetsBlogNotRequiredTrue()
        {
            //arrange, act
            var blogRequest = new BlogRequest(null, null, new Uri("http://example.com"), false,
                                              RequestLocation.SystemMessages, "/");

            //assert
            Assert.IsTrue(blogRequest.BlogNotRequired);
        }

        [Test]
        public void Ctor_WithRequestForLoginPage_SetsBlogNotRequiredFalse()
        {
            //arrange, act
            var blogRequest = new BlogRequest(null, null, new Uri("http://example.com"), false,
                                              RequestLocation.LoginPage, "/");

            //assert
            Assert.IsFalse(blogRequest.BlogNotRequired);
        }

        [Test]
        public void Ctor_WithRequestForInstallation_SetsBlogNotRequiredTrue()
        {
            //arrange, act
            var blogRequest = new BlogRequest(null, null, new Uri("http://example.com"), false,
                                              RequestLocation.Installation, "/");

            //assert
            Assert.IsTrue(blogRequest.BlogNotRequired);
        }

        private static Mock<HttpRequestBase> CreateRequest(string host, string applicationPath, string rawUrl,
                                                           bool useParametersForHost)
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.RawUrl).Returns(rawUrl);
            request.Setup(r => r.Path).Returns(rawUrl);
            request.Setup(r => r.FilePath).Returns(rawUrl);
            request.Setup(r => r.ApplicationPath).Returns(applicationPath);
            request.Setup(r => r.IsLocal).Returns(host == "localhost");
            request.Setup(r => r.Url).Returns(new Uri("http://" + host + rawUrl));

            var parameters = new NameValueCollection();
            parameters["HTTP_HOST"] = useParametersForHost ? host : null;
            request.Setup(r => r.Params).Returns(parameters);
            return request;
        }
    }
}