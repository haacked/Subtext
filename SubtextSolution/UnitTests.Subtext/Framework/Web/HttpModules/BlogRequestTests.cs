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
        public void Ctor_WithRequestWithSubfolder_CreatesBlogRequestWithSubfolder() {
            //arrange
            var request = CreateRequest("example.com", "/", "/foo/bar", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual("foo", blogRequest.Subfolder);
        }

        [Test]
        public void Ctor_WithRequestHavingNoHostInParameters_CreatesBlogRequestWithHostAuthority()
        {
            //arrange
            var request = CreateRequest("example.com", "/", "/foo/bar", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual("example.com", blogRequest.Host);
        }

        [Test]
        public void Ctor_WithRequestForLoginPage_SetsRequestLocationToLogin()
        {
            //arrange
            var request = CreateRequest("example.com", "/", "/login.aspx", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.LoginPage, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForSystemMessage_SetsRequestLocationToSystemMessages()
        {
            //arrange
            var request = CreateRequest("example.com", "/", "/SystemMessages/anything.aspx", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.SystemMessages, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForHostAdmin_SetsRequestLocationToHostAdmin()
        {
            //arrange
            var request = CreateRequest("example.com", "/", "/HostAdmin/anything.aspx", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.HostAdmin, blogRequest.RequestLocation);
        }

        [Test]
        public void Ctor_WithRequestForInstallDirectory_SetsRequestLocationToInstallDirectory()
        {
            //arrange
            var request = CreateRequest("example.com", "/", "/Install/anything.aspx", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual(RequestLocation.Installation, blogRequest.RequestLocation);
        }

        private static Mock<HttpRequestBase> CreateRequest(string host, string applicationPath, string rawUrl, bool useParametersForHost)
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.RawUrl).Returns(rawUrl);
            request.Setup(r => r.Path).Returns(rawUrl);
            request.Setup(r => r.ApplicationPath).Returns(applicationPath);
            request.Setup(r => r.IsLocal).Returns(true);
            request.Setup(r => r.Url).Returns(new Uri("http://" + host + rawUrl));

            var parameters = new NameValueCollection();
            parameters["HTTP_HOST"] = useParametersForHost ? host : null;
            request.Setup(r => r.Params).Returns(parameters);
            return request;
        }
    }
}
