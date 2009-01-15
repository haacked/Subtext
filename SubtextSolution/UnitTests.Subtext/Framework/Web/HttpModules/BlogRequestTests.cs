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
            var module = new BlogRequestModule();
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
            var module = new BlogRequestModule();
            var request = CreateRequest("example.com", "/", "/foo/bar", true);

            //act
            var blogRequest = new BlogRequest(request.Object);

            //assert
            Assert.AreEqual("example.com", blogRequest.Host);
        }

        private static Mock<HttpRequestBase> CreateRequest(string host, string applicationPath, string rawUrl, bool useParametersForHost)
        {
            var request = new Mock<HttpRequestBase>();
            request.Expect(r => r.RawUrl).Returns(rawUrl);
            request.Expect(r => r.ApplicationPath).Returns(applicationPath);
            request.Expect(r => r.IsLocal).Returns(true);
            request.Expect(r => r.Url).Returns(new Uri("http://" + host + rawUrl));

            var parameters = new NameValueCollection();
            parameters["HTTP_HOST"] = useParametersForHost ? host : null;
            request.Expect(r => r.Params).Returns(parameters);
            return request;
        }
    }
}
