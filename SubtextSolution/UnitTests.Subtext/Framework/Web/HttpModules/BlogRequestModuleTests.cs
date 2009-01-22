using System;
using System.Collections.Specialized;
using System.Web;
using MbUnit.Framework;
using Moq;
using Moq.Stub;
using Subtext.Framework;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Web.HttpModules
{
    [TestFixture]
    public class BlogRequestModuleTests
    {
        [Test]
        public void ConvertRequestToBlogRequest_WithRequestForCorrectHost_ReturnsBlogRequest()
        {
            //arrange
            var service = new Mock<IBlogLookupService>();
            service.Setup(s => s.Lookup(It.IsAny<BlogRequest>())).Returns(new BlogLookupResult(new Blog { IsActive = true }, null));
            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Setup(r => r.End()).Throws(new InvalidOperationException("This method should not have been called"));
            var httpRequest = CreateRequest("example.com", "/", "/", true);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request).Returns(httpRequest.Object);
            httpContext.Setup(c => c.Response).Returns(httpResponse.Object);
            var module = new BlogRequestModule(service.Object);

            //act
            BlogRequest request = module.ConvertRequestToBlogRequest(httpContext.Object);

            //assert
            Assert.IsNotNull(request);
        }

        [Test]
        public void ConvertRequestToBlogRequest_WithRequestForAlternateHost_RedirectsToPrimaryHost()
        {
            //arrange
            var service = new Mock<IBlogLookupService>();
            service.Setup(s => s.Lookup(It.IsAny<BlogRequest>())).Returns(new BlogLookupResult(null, new Uri("http://www.example.com/")));
            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Stub(r => r.StatusCode);
            httpResponse.Stub(r => r.Status);
            httpResponse.Stub(r => r.RedirectLocation);
            var httpRequest = CreateRequest("example.com", "/", "/", true);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request).Returns(httpRequest.Object);
            httpContext.Setup(c => c.Response).Returns(httpResponse.Object);
            var module = new BlogRequestModule(service.Object);

            //act
            BlogRequest request = module.ConvertRequestToBlogRequest(httpContext.Object);

            //assert
            Assert.IsNull(request);
            httpResponse.Verify(r => r.End());
            Assert.AreEqual(301, httpResponse.Object.StatusCode);
            Assert.AreEqual("301 Moved Permanently", httpResponse.Object.Status);
            Assert.AreEqual("http://www.example.com/", httpResponse.Object.RedirectLocation);
        }

        [Test]
        public void ConvertRequestToBlogRequest_WithNoMatchingBlog_RedirectsToBlogNotConfiguredPage()
        {
            //arrange
            var service = new Mock<IBlogLookupService>();
            service.Setup(s => s.Lookup(It.IsAny<BlogRequest>())).Returns((BlogLookupResult)null);
            var httpResponse = new Mock<HttpResponseBase>();
            var httpRequest = CreateRequest("example.com", "/", "/", true);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request).Returns(httpRequest.Object);
            httpContext.Setup(c => c.Response).Returns(httpResponse.Object);
            var module = new BlogRequestModule(service.Object);

            //act
            BlogRequest request = module.ConvertRequestToBlogRequest(httpContext.Object);

            //assert
            Assert.IsNull(request);
            httpResponse.Verify(r => r.Redirect("~/Install/BlogNotConfiguredError.aspx", true));
        }

        [Test]
        public void ConvertRequestToBlogRequest_MatchingInactiveBlog_RedirectsToBlogInactivePage()
        {
            //arrange
            var service = new Mock<IBlogLookupService>();
            var result = new BlogLookupResult(new Blog { IsActive = false }, null);
            service.Setup(s => s.Lookup(It.IsAny<BlogRequest>())).Returns(result);
            var httpResponse = new Mock<HttpResponseBase>();
            var httpRequest = CreateRequest("example.com", "/", "/", true);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request).Returns(httpRequest.Object);
            httpContext.Setup(c => c.Response).Returns(httpResponse.Object);
            var module = new BlogRequestModule(service.Object);

            //act
            BlogRequest request = module.ConvertRequestToBlogRequest(httpContext.Object);

            //assert
            Assert.IsNull(request);
            httpResponse.Verify(r => r.Redirect("~/SystemMessages/BlogNotActive.aspx", true));
        }

        [Test]
        public void ConvertRequestToBlogRequestWithRequestForLoginPage_MatchingInactiveBlog_DoesNotRedirect()
        {
            //arrange
            var service = new Mock<IBlogLookupService>();
            var result = new BlogLookupResult(new Blog { IsActive = false }, null);
            service.Setup(s => s.Lookup(It.IsAny<BlogRequest>())).Returns(result);
            var httpResponse = new Mock<HttpResponseBase>();
            var httpRequest = CreateRequest("example.com", "/", "/login.aspx", true);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request).Returns(httpRequest.Object);
            httpContext.Setup(c => c.Response).Returns(httpResponse.Object);
            var module = new BlogRequestModule(service.Object);

            //act
            BlogRequest request = module.ConvertRequestToBlogRequest(httpContext.Object);

            //assert
            Assert.IsNotNull(request);
        }

        [Test]
        public void ConvertRequestToBlogRequest_WithNoMatchingBlogButWithRequestForLoginPage_SetsBlogRequestToNull()
        {
            //arrange
            var service = new Mock<IBlogLookupService>();
            service.Setup(s => s.Lookup(It.IsAny<BlogRequest>())).Returns((BlogLookupResult)null);
            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Setup(r => r.Redirect(It.IsAny<string>(), true)).Throws(new InvalidOperationException("Method should not have been called"));
            var httpRequest = CreateRequest("example.com", "/", "/login.aspx", true);
            httpRequest.Setup(r => r.FilePath).Returns("/Login.aspx");
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request).Returns(httpRequest.Object);
            httpContext.Setup(c => c.Response).Returns(httpResponse.Object);
            var module = new BlogRequestModule(service.Object);

            //act
            BlogRequest request = module.ConvertRequestToBlogRequest(httpContext.Object);

            //assert
            Assert.IsNull(request);
        }

        [Test]
        public void ConvertRequestToBlogRequest_WithRequestForInstallationDirectory_ReturnsNullBlog()
        {
            //arrange
            var service = new Mock<IBlogLookupService>();
            service.Setup(s => s.Lookup(It.IsAny<BlogRequest>())).Throws(new InvalidOperationException("Should not be called"));
            var httpResponse = new Mock<HttpResponseBase>();
            httpResponse.Setup(r => r.Redirect(It.IsAny<string>(), true)).Throws(new InvalidOperationException("Method should not have been called"));
            var httpRequest = CreateRequest("example.com", "/", "/Install/Anything.aspx", true);
            httpRequest.Setup(r => r.FilePath).Returns("/Install/Anything.aspx");
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request).Returns(httpRequest.Object);
            httpContext.Setup(c => c.Response).Returns(httpResponse.Object);
            var module = new BlogRequestModule(service.Object);

            //act
            BlogRequest request = module.ConvertRequestToBlogRequest(httpContext.Object);

            //assert
            Assert.IsNull(request.Blog);
        }

        private static Mock<HttpRequestBase> CreateRequest(string host, string applicationPath, string rawUrl, bool useParametersForHost)
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(r => r.RawUrl).Returns(rawUrl);
            request.Setup(r => r.Path).Returns(rawUrl);
            request.Setup(r => r.FilePath).Returns(rawUrl);
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
