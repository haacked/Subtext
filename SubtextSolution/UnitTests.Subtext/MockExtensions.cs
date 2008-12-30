using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.HttpModules;
using Subtext.Extensibility;

namespace UnitTests.Subtext
{
    public static class MockExtensions
    {
        public static void FakeRequest(this Mock<HttpContextBase> httpContextMock, string virtualPath) {
            httpContextMock.FakeRequest(virtualPath, null);
        }

        public static StringWriter FakeSitemapHandlerRequest(this Mock<ISubtextContext> subtextContext, Mock<ObjectProvider> repository)
        {
            subtextContext.Expect(c => c.Repository).Returns(repository.Object);
            StringWriter writer = subtextContext.FakeSubtextContextRequest(new Blog { Host = "localhost" }, "/sitemap.xml", "/", string.Empty);
            var urlHelper = Mock.Get<UrlHelper>(subtextContext.Object.UrlHelper);
            urlHelper.Expect(u => u.EntryUrl(It.IsAny<Entry>())).Returns<Entry>(e => e.PostType == PostType.BlogPost ? "/some-blogpost-with-id-of-" + e.Id : "some-article-with-id-of-" + e.Id);
            urlHelper.Expect(u => u.BlogUrl()).Returns("/");
            urlHelper.Expect(u => u.ContactFormUrl()).Returns("/contact.aspx");
            return writer;
        }

        public static StringWriter FakeRequest(this Mock<HttpContextBase> httpContextMock, string virtualPath, string subfolder)
        {
            httpContextMock.Expect(h => h.Request.HttpMethod).Returns("GET");
            httpContextMock.Expect(context => context.Request.AppRelativeCurrentExecutionFilePath).Returns(virtualPath);
            httpContextMock.Expect(context => context.Request.Path).Returns(virtualPath);
            httpContextMock.ExpectGet(c => c.Items[BlogRequest.BlogRequestKey]).Returns(new BlogRequest("localhost", subfolder, new Uri("http://localhost/"), true));
            var writer = new StringWriter();
            httpContextMock.Expect(c => c.Response.Output).Returns(writer);
            return writer;
        }

        public static void FakeSyndicationContext(this Mock<ISubtextContext> subtextContextMock, Blog blog, string virtualPath, string applicationPath, Action<string> callback) {
            subtextContextMock.FakeSyndicationContext(blog, virtualPath, applicationPath, null, callback);
        }

        public static StringWriter FakeSubtextContextRequest(this Mock<ISubtextContext> subtextContextMock, Blog blog, string virtualPath, string applicationPath, string subfolder) {
            var httpContext = new Mock<HttpContextBase>();
            var writer = httpContext.FakeRequest(virtualPath, subfolder);
            httpContext.Expect(c => c.Request.ApplicationPath).Returns(applicationPath);

            var urlHelper = new Mock<UrlHelper>();
            
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", subfolder);
            
            var requestContext = new RequestContext(httpContext.Object, routeData);
            
            subtextContextMock.Expect(c => c.Blog).Returns(blog);
            subtextContextMock.Expect(c => c.UrlHelper).Returns(urlHelper.Object);
            subtextContextMock.Expect(c => c.RequestContext).Returns(requestContext);

            return writer;
        }
        
        public static void FakeSyndicationContext(this Mock<ISubtextContext> subtextContextMock, Blog blog, string virtualPath, string applicationPath, string subfolder, Action<string> callback) {
            var urlHelper = new Mock<UrlHelper>();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeSyndicationRequest(virtualPath, applicationPath, callback);

            string imagePath = "/images/RSS2Image.gif";
            if (applicationPath != "/") {
                imagePath = applicationPath + imagePath;
            }

            urlHelper.Expect(url => url.ResolveUrl(It.IsAny<string>())).Returns(new VirtualPath(imagePath));
            urlHelper.Expect(u => u.AggBugUrl(It.IsAny<int>())).Returns<int>(id => "/Subtext.Web/aggbug/" + id + ".aspx");
            urlHelper.Expect(u => u.CommentRssUrl(It.IsAny<int>())).Returns<int>(id => "/Subtext.Web/comments/commentRss/" + id + ".aspx");
            urlHelper.Expect(u => u.TrackbacksUrl(It.IsAny<int>())).Returns<int>(id => "/Subtext.Web/services/trackbacks/" + id + ".aspx");

            var routeData = new RouteData();
            routeData.Values.Add("subfolder", subfolder);
            var requestContext = new RequestContext(httpContext.Object, routeData);
            subtextContextMock.Expect(c => c.Blog).Returns(blog);
            subtextContextMock.Expect(c => c.UrlHelper).Returns(urlHelper.Object);
            subtextContextMock.Expect(c => c.RequestContext).Returns(requestContext);
        }

        public static void FakeSyndicationContext(this Mock<ISubtextContext> subtextContextMock, Blog blog, string virtualPath, Action<string> callback) {
            subtextContextMock.FakeSyndicationContext(blog, virtualPath, "/", callback);
        }

        public static void FakeSyndicationRequest(this Mock<HttpContextBase> httpContextMock, string virtualPath, string applicationPath, Action<string> callback) {
            var headers = new NameValueCollection();
            headers.Add("If-Modified-Since", null);
            httpContextMock.Expect(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(virtualPath);
            httpContextMock.Expect(c => c.Request.Path).Returns(virtualPath);
            httpContextMock.Expect(c => c.Request.ApplicationPath).Returns(applicationPath);
            httpContextMock.Expect(c => c.Response.Output).Returns(new StringWriter());
            httpContextMock.Expect(c => c.Request.Headers).Returns(headers);
            httpContextMock.ExpectSet(c => c.Response.ContentType, "text/xml").Verifiable();
            httpContextMock.Expect(c => c.Response.Cache.SetCacheability(HttpCacheability.Public)).Verifiable();
            httpContextMock.Expect(c => c.Response.Cache.SetLastModified(It.IsAny<DateTime>()));
            httpContextMock.Expect(c => c.Response.Cache.SetETag(It.IsAny<string>()));
            httpContextMock.Expect(c => c.Response.AddHeader(It.IsAny<string>(), It.IsAny<string>()));
            httpContextMock.ExpectSet(c => c.Response.StatusCode, It.IsAny<int>());
            if (callback != null) {
                httpContextMock.Expect(c => c.Response.Write(It.IsAny<string>())).Callback<string>(callback);
            }
            httpContextMock.Expect(c => c.Cache).Returns((Cache)null);
        }
    }
}
