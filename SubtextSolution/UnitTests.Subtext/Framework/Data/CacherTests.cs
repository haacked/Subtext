using System;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Moq.Stub;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using UnitTests.Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Data
{
	/// <summary>
	/// Unit tests of the <see cref="Cacher"/> class.
	/// </summary>
	[TestFixture]
	public class CacherTests
	{
        /// <summary>
        /// This test is to make sure a bug I introduced never happens again.
        /// </summary>
        [Test]
        public void GetEntryFromRequest_WithIdInRouteDataMatchingEntryInRepository_ReturnsEntry()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/123.aspx");
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext, routeData, new Blog { Id = 123 })
                .Setup(c => c.Repository.GetEntry(123, true, true)).Returns(new Entry(PostType.BlogPost) { Id = 123, Title = "Testing 123" });

            //act
            Entry entry = Cacher.GetEntryFromRequest(true, subtextContext.Object);

            //assert
            Assert.AreEqual(123, entry.Id);
            Assert.AreEqual("Testing 123", entry.Title);
        }

        /// <summary>
        /// This test is to make sure a bug I introduced never happens again.
        /// </summary>
        [Test]
        [RollBack]
        public void GetEntryFromRequest_WithEntryHavingEntryNameButIdInRouteDataMatchingEntryInRepository_RedirectsToUrlWithSlug()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            
            Entry entry = new Entry(PostType.BlogPost) { Id = 123, EntryName = "testing-slug", Title = "Testing 123" };
            repository.Setup(r => r.GetEntry(123, true, true)).Returns(entry);
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/archive/testing-slug.aspx");
            UnitTestHelper.SetupBlog();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/testing-slug.aspx");
            httpContext.Setup(c => c.Response.End());
            httpContext.SetupSet(c => c.Response.StatusCode, 301);
            httpContext.SetupSet(c => c.Response.Status, "301 Moved Permanently");
            httpContext.Stub(c => c.Response.RedirectLocation);
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext, routeData)
                .SetupUrlHelper(urlHelper)
                .SetupRepository(repository.Object)
                .SetupBlog(new Blog { Host = "localhost" });

            //act
            Entry cachedEntry = Cacher.GetEntryFromRequest(true /* allowRedirect */, subtextContext.Object);

            //assert
            httpContext.VerifySet(c => c.Response.StatusCode, 301);
            httpContext.VerifySet(c => c.Response.Status, "301 Moved Permanently");
            Assert.AreEqual(123, cachedEntry.Id);
            Assert.AreEqual("Testing 123", cachedEntry.Title);
            Assert.AreEqual("http://localhost/archive/testing-slug.aspx", httpContext.Object.Response.RedirectLocation);
        }

        /// <summary>
        /// This test is to make sure a bug I introduced never happens again.
        /// </summary>
        [Test]
        public void GetEntryFromRequest_WithSlugInRouteDataMatchingEntryInRepository_ReturnsEntry()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/the-slug.aspx");
            var routeData = new RouteData();
            routeData.Values.Add("slug", "the-slug");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext, routeData)
                .SetupBlog(new Blog { Id = 1, TimeZoneId = TimeZonesTest.PacificTimeZoneId /* pacific */ })
                .Setup(c => c.Repository.GetEntry("the-slug", true, true)).Returns(new Entry(PostType.BlogPost) { Id = 123, EntryName = "the-slug", Title = "Testing 123" });

            //act
            Entry entry = Cacher.GetEntryFromRequest(true, subtextContext.Object);

            //assert
            Assert.AreEqual(123, entry.Id);
            Assert.AreEqual("Testing 123", entry.Title);
            Assert.AreEqual("the-slug", entry.EntryName);
        }

		/// <summary>
		/// This test is to make sure a bug I introduced never happens again.
		/// </summary>
		[Test]
		public void GetEntryFromRequest_WithNonExistentEntry_DoesNotThrowNullReferenceException()
		{
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/99999.aspx");
            var routeData = new RouteData();
            routeData.Values.Add("id", "999999");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext, routeData)
                .Setup(c => c.Repository.GetEntry(It.IsAny<int>(), true, true)).Returns((Entry)null);
    
            //act
            Cacher.GetEntryFromRequest(true, subtextContext.Object);
            
            //assert
            //None needed.
		}

		[Test]
		public void SingleCategoryThrowsExceptionIfContextNull()
		{
            UnitTestHelper.AssertThrows<ArgumentNullException>(
                () => Cacher.SingleCategory(null)
            );
		}

		[Test]
		public void SingleCategoryReturnsNullForNonExistentCategory()
		{
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("slug", "99");
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, routeData);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.RequestContext).Returns(requestContext);
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Id = 123 });
            subtextContext.Setup(c => c.Repository.GetLinkCategory(99, true)).Returns((LinkCategory)null);
            subtextContext.Setup(c => c.Cache[It.IsAny<string>()]).Returns(null);

            //act
            LinkCategory category = Cacher.SingleCategory(subtextContext.Object);

            //assert
            Assert.IsNull(category);
		}

		[Test]
		public void CanGetCategoryByIdRequest()
		{
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("slug", "99");
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, routeData);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.RequestContext).Returns(requestContext);
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Id = 123 });
            subtextContext.Setup(c => c.Repository.GetLinkCategory(99, true))
                .Returns(new LinkCategory { Id = 99, Title = "this is a test"});
            subtextContext.Setup(c => c.Cache[It.IsAny<string>()]).Returns(null);
			
            //act
            LinkCategory category = Cacher.SingleCategory(subtextContext.Object);
			     
            //assert
            Assert.AreEqual("this is a test", category.Title);
		}

		[Test]
		public void CanGetCategoryByNameRequest()
		{
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("slug", "this-is-a-test");
            var requestContext = new RequestContext(new Mock<HttpContextBase>().Object, routeData);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.RequestContext).Returns(requestContext);
            subtextContext.Setup(c => c.Blog).Returns(new Blog { Id = 123 });
            subtextContext.Setup(c => c.Repository.GetLinkCategory("this-is-a-test", true))
                .Returns(new LinkCategory { Id = 99, Title = "this is a test" });
            subtextContext.Setup(c => c.Cache[It.IsAny<string>()]).Returns(null);

            //act
			LinkCategory category = Cacher.SingleCategory(subtextContext.Object);

            //assert
			Assert.AreEqual(99, category.Id);
		}
	}
}
