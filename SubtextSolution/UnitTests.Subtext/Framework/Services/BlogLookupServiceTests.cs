using System;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class BlogLookupServiceTests
    {
        [Test]
        public void Request_WithMatchingHost_ReturnsCorrespondingBlog() {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>(), true)).Returns(new Blog());
            var service = new BlogLookupService(repository.Object, new HostInfo());
            
            //act
            BlogLookupResult result = service.Lookup(new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false));

            //assert
            Assert.IsNotNull(result.Blog);
            Assert.IsNull(result.AlternateUrl);
        }

        [Test]
        public void Request_WithNonMatchingHostButAlternativeHostMatches_ReturnsAlternativeHost()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>(), true)).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("www.example.com", It.IsAny<string>(), true)).Returns(new Blog { });
            var service = new BlogLookupService(repository.Object, new HostInfo());

            //act
            BlogLookupResult result = service.Lookup(new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false));

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://www.example.com/foo/bar", result.AlternateUrl.ToString());
        }

        [Test]
        public void Request_MatchingActiveAlias_RedirectsToPrimary()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("blog.example.com", It.IsAny<string>(), true)).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("www.example.com", It.IsAny<string>(), true)).Returns(new Blog { });
            repository.Setup(r => r.GetBlogByDomainAlias("blog.example.com", It.IsAny<string>(), It.IsAny<bool>())).Returns(new Blog { Host = "www.example.com" });
            var service = new BlogLookupService(repository.Object, new HostInfo());

            //act
            BlogLookupResult result = service.Lookup(new BlogRequest("blog.example.com", string.Empty, new Uri("http://blog.example.com/foo/bar"), false));

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://www.example.com/foo/bar", result.AlternateUrl.ToString());
        }

        [Test]
        public void Request_MatchingActiveAliasWithSubfolder_RedirectsToPrimaryWithoutSubfolder()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("blog.example.com", "sub", true)).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("www.example.com", string.Empty, true)).Returns(new Blog { });
            repository.Setup(r => r.GetBlogByDomainAlias("blog.example.com", "sub", It.IsAny<bool>())).Returns(new Blog { Host = "www.example.com", Subfolder = string.Empty });
            var service = new BlogLookupService(repository.Object, new HostInfo());

            //act
            BlogLookupResult result = service.Lookup(new BlogRequest("blog.example.com", "sub", new Uri("http://blog.example.com/sub/foo/bar"), false));

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://www.example.com/foo/bar", result.AlternateUrl.ToString());
        }

        [Test]
        public void Request_MatchingActiveAliasWithoutSubfolder_RedirectsToPrimaryWithSubfolder()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("blog.example.com", string.Empty, true)).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("www.example.com", "sub", true)).Returns(new Blog { });
            repository.Setup(r => r.GetBlogByDomainAlias("blog.example.com", string.Empty, It.IsAny<bool>())).Returns(new Blog { Host = "www.example.com", Subfolder = "sub" });
            var service = new BlogLookupService(repository.Object, new HostInfo());

            //act
            BlogLookupResult result = service.Lookup(new BlogRequest("blog.example.com", string.Empty, new Uri("http://blog.example.com/foo/bar"), false));

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://www.example.com/sub/foo/bar", result.AlternateUrl.ToString());
        }

        [Test]
        public void Request_MatchingActiveAliasWithSubfolder_RedirectsToPrimaryWithDifferentSubfolder()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("blog.example.com", "notsub", true)).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("www.example.com", "sub", true)).Returns(new Blog { });
            repository.Setup(r => r.GetBlogByDomainAlias("blog.example.com", "notsub", It.IsAny<bool>())).Returns(new Blog { Host = "www.example.com", Subfolder = "sub" });
            var service = new BlogLookupService(repository.Object, new HostInfo());

            //act
            BlogLookupResult result = service.Lookup(new BlogRequest("blog.example.com", "notsub", new Uri("http://blog.example.com/notsub/foo/bar"), false));

            //assert
            Assert.IsNull(result.Blog);
            Assert.AreEqual("http://www.example.com/sub/foo/bar", result.AlternateUrl.ToString());
        }

        [Test]
        public void Request_NotMatchingAnyBlog_ReturnsNull()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>(), true)).Returns((Blog)null);
            var pagedCollection = new Mock<IPagedCollection<Blog>>();
            pagedCollection.Setup(p => p.MaxItems).Returns(0);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(pagedCollection.Object); ;
            var service = new BlogLookupService(repository.Object, new HostInfo { BlogAggregationEnabled = false });

            //act
            BlogLookupResult result = service.Lookup(new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false));

            //assert
            Assert.IsNull(result);
        }

        [Test]
        public void RequestNotMatchingAnyBlog_ButWithAggregateBlogsEnabledAndActiveBlogsInTheSystem_ReturnsAggregateBlog()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>(), true)).Returns((Blog)null);
            var pagedCollection = new Mock<IPagedCollection<Blog>>();
            pagedCollection.Setup(p => p.MaxItems).Returns(1);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(pagedCollection.Object);
            Blog aggregateBlog = new Blog();
            var service = new BlogLookupService(repository.Object, new HostInfo { BlogAggregationEnabled = true, AggregateBlog = aggregateBlog });
            var blogRequest = new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false);

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.AreSame(aggregateBlog, result.Blog);
        }

        [Test]
        public void RequestNotMatchingAnyBlog_ButWithASingleBlogInSystem_ReturnsThatBlog()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>(), true)).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>(), false)).Returns(new Blog());
            var pagedCollection = new Mock<IPagedCollection<Blog>>();
            pagedCollection.Setup(p => p.MaxItems).Returns(1);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(pagedCollection.Object);
            var service = new BlogLookupService(repository.Object, new HostInfo { BlogAggregationEnabled = false });
            var blogRequest = new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false);

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNotNull(result.Blog);
            Assert.IsNull(result.AlternateUrl);
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
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>(), true)).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("example.com", It.IsAny<string>(), false)).Returns(new Blog { Host = "localhost"});
            var pagedCollection = new Mock<IPagedCollection<Blog>>();
            pagedCollection.Setup(p => p.MaxItems).Returns(1);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(pagedCollection.Object);
            var service = new BlogLookupService(repository.Object, new HostInfo { BlogAggregationEnabled = false });
            var blogRequest = new BlogRequest("example.com", string.Empty, new Uri("http://example.com/foo/bar"), false);

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNotNull(result.Blog);
            Assert.IsNull(result.AlternateUrl);
            Assert.AreEqual("example.com", result.Blog.Host);
            repository.Verify(r => r.UpdateBlog(It.IsAny<Blog>()));
        }

        /// <summary>
        /// This test makes sure we deal gracefully with a common deployment problem. 
        /// A user sets up the blog on his/her local machine (aka "localhost"), then 
        /// deploys the database to their production server. The hostname in the db 
        /// should be changed to the new domain.
        /// </summary>
        [Test]
        public void RequestForLocalHostNotMatchingAnyBlog_ButWithASingleBlogInSystemWithLocalHost_ReturnsThatBlogAndDoesNotUpdateItsHost()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetBlog("localhost", string.Empty, true)).Returns((Blog)null);
            repository.Setup(r => r.GetBlog("localhost", string.Empty, false)).Returns(new Blog { Host = "localhost", Subfolder = "sub" });
            repository.Setup(r => r.UpdateBlog(It.IsAny<Blog>())).Throws(new InvalidOperationException("Should not be called"));
            var pagedCollection = new Mock<IPagedCollection<Blog>>();
            pagedCollection.Setup(p => p.MaxItems).Returns(1);
            repository.Setup(r => r.GetPagedBlogs(null, 0, It.IsAny<int>(), ConfigurationFlags.None)).Returns(pagedCollection.Object);
            var service = new BlogLookupService(repository.Object, new HostInfo { BlogAggregationEnabled = false });
            var blogRequest = new BlogRequest("localhost", string.Empty, new Uri("http://localhost/foo/bar"), false);

            //act
            BlogLookupResult result = service.Lookup(blogRequest);

            //assert
            Assert.IsNotNull(result.Blog);
            Assert.IsNull(result.AlternateUrl);
            Assert.AreEqual("localhost", result.Blog.Host);
        }
    }
}