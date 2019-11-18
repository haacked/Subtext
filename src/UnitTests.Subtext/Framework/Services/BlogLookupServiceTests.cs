using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class BlogLookupServiceTests
    {
        private static HostInfo CreateHostInfo()
        {
            return new HostInfo(new NameValueCollection());
        }

        [Test]
        public void Request_WithMatchingHost_ReturnsCorrespondingBlog()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>())).Returns(new Blog { Host = "example.com" });
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(CreateHostInfo));

            //act
            BlogLookupResult result =
                service.Lookup(new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false));

            //assert
            Assert.IsNotNull(result.Blog);
            Assert.IsNull(result.AlternateUrl);
        }

        [Test]
        public void Request_WithNonMatchingHostButAlternativeHostMatches_ReturnsBlogWithSpecifiedHost()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>())).Returns(new Blog { Host = "www.example.com" });
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(CreateHostInfo));

            //act
            BlogLookupResult result =
                service.Lookup(new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false));

            //assert
            Assert.AreEqual(result.Blog.Host, "example.com");
            Assert.IsNull(result.AlternateUrl);
        }

        [Test]
        public void Request_MatchingActiveAlias_ReturnsBlogWithSpecifiedHost()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("blog.example.com", It.IsAny<string>())).Returns(new Blog { Host = "www.example.com" });
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(CreateHostInfo));

            //act
            BlogLookupResult result =
                service.Lookup(new BlogRequest("blog.example.com", string.Empty,
                                               new Uri("http://blog.example.com/foo/bar"), false));

            //assert
            Assert.AreEqual("blog.example.com", result.Blog.Host);
            Assert.IsNull(result.AlternateUrl);
        }

        [Test]
        public void Request_MatchingActiveAliasWithSubfolder_RedirectsToPrimaryWithoutSubfolder()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("blog.example.com", "sub")).Returns(new Blog { Host = "www.example.com", Subfolder = "" });
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(CreateHostInfo));

            //act
            BlogLookupResult result =
                service.Lookup(new BlogRequest("blog.example.com", "sub", new Uri("http://blog.example.com/sub/foo/bar"),
                                               false));

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://www.example.com/foo/bar", result.AlternateUrl.ToString());
        }

        [Test]
        public void Request_MatchingActiveAliasWithoutSubfolder_RedirectsToPrimaryWithSubfolder()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("blog.example.com", string.Empty)).Returns(new Blog { Host = "www.example.com", Subfolder = "sub" });
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(CreateHostInfo));

            //act
            BlogLookupResult result =
                service.Lookup(new BlogRequest("blog.example.com", string.Empty,
                                               new Uri("http://blog.example.com/foo/bar"), false));

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://www.example.com/sub/foo/bar", result.AlternateUrl.ToString());
        }

        [Test]
        public void Request_MatchingActiveAliasWithSubfolder_RedirectsToPrimaryWithDifferentSubfolder()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("blog.example.com", "notsub")).Returns(new Blog { Host = "www.example.com", Subfolder = "sub" });
            repository.Setup(r => r.GetBlogByDomainAlias("blog.example.com", "notsub", It.IsAny<bool>())).Returns(
                new Blog { Host = "www.example.com", Subfolder = "sub" });
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(CreateHostInfo));

            //act
            BlogLookupResult result =
                service.Lookup(new BlogRequest("blog.example.com", "notsub",
                                               new Uri("http://blog.example.com/notsub/foo/bar"), false));

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://www.example.com/sub/foo/bar", result.AlternateUrl.ToString());
        }

        [Test]
        public void Request_NotMatchingAnyBlog_ReturnsNull()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>())).Returns((Blog)null);
            var pagedCollection = new Mock<IPagedCollection<Blog>>();
            pagedCollection.Setup(p => p.MaxItems).Returns(0);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(
                pagedCollection.Object);
            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "false");
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(() => new HostInfo(appSettings)));

            //act
            BlogLookupResult result =
                service.Lookup(new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false));

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void RequestNotMatchingAnyBlog_ButWithAggregateBlogsEnabledAndActiveBlogsInTheSystem_ReturnsAggregateBlog()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>())).Returns((Blog)null);
            var onlyBlog = new Blog { Host = "example.com", Subfolder = "not-sub" };
            var pagedCollection = new PagedCollection<Blog> { onlyBlog };
            pagedCollection.MaxItems = 1;
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(
                pagedCollection);
            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "true");
            var hostInfo = new HostInfo(appSettings);
            var service = new BlogLookupService(repository.Object,
                                                new LazyNotNull<HostInfo>(() => hostInfo));
            var blogRequest = new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false);

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.AreSame(hostInfo.AggregateBlog, result.Blog);
        }

        [Test]
        public void RequestWithSubfolderNotMatchingAnyBlog_ButWithAggregateBlogsEnabledAndMoreThanOneActiveBlogsInTheSystem_ReturnsNull()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>())).Returns((Blog)null);
            var blog1 = new Blog { Host = "example.com", Subfolder = "not-sub" };
            var blog2 = new Blog { Host = "example.com", Subfolder = "not-sub-2" };
            var pagedCollection = new PagedCollection<Blog> { blog1, blog2 };
            pagedCollection.MaxItems = 2;
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(
                pagedCollection);
            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "true");
            var hostInfo = new HostInfo(appSettings);
            var service = new BlogLookupService(repository.Object,
                                                new LazyNotNull<HostInfo>(() => hostInfo));
            var blogRequest = new BlogRequest("example.com", "blog1234", new Uri("http://example.com/foo/bar"), false);

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// This test makes sure we deal gracefully with a common deployment problem. 
        /// A user sets up the blog on his/her local machine (aka "localhost"), then 
        /// deploys the database to their production server. The hostname in the db 
        /// should be changed to the new domain.
        /// </summary>
        [Test]
        public void
            RequestNotMatchingAnyBlog_ButWithASingleBlogInSystemWithMatchingHostButDifferentSubfolder_RedirectsToOnlyBlog
            ()
        {
            //arrange
            var onlyBlog = new Blog { Host = "example.com", Subfolder = "not-sub" };
            var pagedCollection = new PagedCollection<Blog> { onlyBlog };
            pagedCollection.MaxItems = 1;

            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", "sub")).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("example.com", "not-sub")).Returns(onlyBlog);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(
                pagedCollection);
            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "false");
            var hostInfo = new HostInfo(appSettings);
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(() => hostInfo));
            var blogRequest = new BlogRequest("example.com", "sub", new Uri("http://example.com/Subtext.Web/sub/bar"),
                                              false, RequestLocation.Blog, "/Subtext.Web");

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://example.com/Subtext.Web/not-sub/bar", result.AlternateUrl.ToString());
        }

        /// <summary>
        /// This test makes sure we deal gracefully with a common deployment problem. 
        /// A user sets up the blog on his/her local machine (aka "localhost"), then 
        /// deploys the database to their production server. The hostname in the db 
        /// should be changed to the new domain.
        /// </summary>
        [Test]
        public void RequestNotMatchingAnyBlog_ButWithASingleBlogInSystemWithLocalHost_ReturnsThatBlogAndUpdatesItsHost()
        {
            //arrange
            var onlyBlog = new Blog { Host = "localhost", Subfolder = "" };
            var pagedCollection = new PagedCollection<Blog> { onlyBlog };
            pagedCollection.MaxItems = 1;

            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>())).Returns((Blog)null);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(
                pagedCollection);
            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "false");
            var hostInfo = new HostInfo(appSettings);
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(() => hostInfo));
            var blogRequest = new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false);

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNotNull(result.Blog);
            Assert.IsNull(result.AlternateUrl);
            Assert.AreEqual("example.com", result.Blog.Host);
            Assert.AreEqual("example.com", onlyBlog.Host);
            repository.Verify(r => r.UpdateBlog(It.IsAny<Blog>()));
        }

        /// <summary>
        /// This test makes sure we deal gracefully with a common deployment problem. 
        /// A user sets up the blog on his/her local machine (aka "localhost"), then 
        /// deploys the database to their production server. The hostname in the db 
        /// should be changed to the new domain.
        /// </summary>
        [Test]
        public void
            RequestNotMatchingAnyBlog_ButWithASingleBlogInSystemWithLocalHostButNotMatchingSubfolder_ReturnsUpdatesItsHostThenRedirectsToSubfolder
            ()
        {
            //arrange
            var onlyBlog = new Blog { Host = "localhost", Subfolder = "sub" };
            var pagedCollection = new PagedCollection<Blog> { onlyBlog };
            pagedCollection.MaxItems = 1;

            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>())).Returns((Blog)null);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(
                pagedCollection);
            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "false");
            var hostInfo = new HostInfo(appSettings);
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(() => hostInfo));
            var blogRequest = new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false);

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNull(result.Blog);
            Assert.IsNotNull(result.AlternateUrl);
            Assert.AreEqual("http://example.com/sub/foo/bar", result.AlternateUrl.ToString());
            Assert.AreEqual("example.com", onlyBlog.Host);
            repository.Verify(r => r.UpdateBlog(It.IsAny<Blog>()));
        }

        [Test]
        public void
            RequestNotMatchingAnyBlog_ButWithASingleBlogInSystemAndSubfolderEqualsAppPath_RedirectsToSingleBlog
            ()
        {
            //arrange
            var onlyBlog = new Blog { Host = "example.com", Subfolder = "jjameson" };
            var pagedCollection = new PagedCollection<Blog> { onlyBlog };
            pagedCollection.MaxItems = 1;

            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", "blog")).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("example.com", "jjameson")).Returns(onlyBlog);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(
                pagedCollection);
            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "false");
            var hostInfo = new HostInfo(appSettings);
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(() => hostInfo));
            var blogRequest = new BlogRequest(
                "example.com",
                "blog",
                new Uri("http://example.com/blog/"),
                false,
                RequestLocation.Blog,
                "/blog");

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://example.com/blog/jjameson/", result.AlternateUrl.ToString());
        }

        [Test]
        public void
            RequestNotMatchingAnyBlog_ButWithASingleBlogInSystemAndSubfolderEqualsEmptyString_RedirectsToSingleBlog
            ()
        {
            //arrange
            var onlyBlog = new Blog { Host = "example.com", Subfolder = "jjameson" };
            var pagedCollection = new PagedCollection<Blog> { onlyBlog };
            pagedCollection.MaxItems = 1;

            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetBlog("example.com", "blog")).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("example.com", "jjameson")).Returns(onlyBlog);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(
                pagedCollection);
            var appSettings = new NameValueCollection();
            appSettings.Add("AggregateEnabled", "false");
            var hostInfo = new HostInfo(appSettings);
            var service = new BlogLookupService(repository.Object, new LazyNotNull<HostInfo>(() => hostInfo));
            var blogRequest = new BlogRequest(
                "example.com",
                string.Empty,
                new Uri("http://example.com/blog"),
                false,
                RequestLocation.Blog,
                "/blog");

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://example.com/blog/jjameson/", result.AlternateUrl.ToString());
        }
    }
}
