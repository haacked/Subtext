using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Extensibility;
using Subtext.Framework.Routing;

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
        [RollBack]
        public void GetEntryFromRequest_WithIdInRouteDataMatchingEntryInRepository_ReturnsEntry()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Expect(r => r.GetEntry(123, true, true)).Returns(new Entry(PostType.BlogPost) { Id = 123, Title = "Testing 123" });
            UnitTestHelper.SetupBlog();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/123.aspx");
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Expect(c => c.RequestContext).Returns(requestContext);
            subtextContext.Expect(c => c.Repository).Returns(repository.Object);

            //act
            Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short, true, subtextContext.Object);

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
            repository.Expect(r => r.GetEntry(123, true, true)).Returns(entry);
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Expect(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/archive/testing-slug.aspx");
            UnitTestHelper.SetupBlog();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/testing-slug.aspx");
            httpContext.Expect(c => c.Response.End());
            httpContext.ExpectSet(c => c.Response.StatusCode, 302).Verifiable();
            httpContext.ExpectSet(c => c.Response.Status, "301 Moved Permanently").Verifiable();
            string redirectLocation = null;
            httpContext.ExpectSet(c => c.Response.RedirectLocation).Callback(s => redirectLocation = s);
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Expect(c => c.UrlHelper).Returns(urlHelper.Object);
            subtextContext.Expect(c => c.RequestContext).Returns(requestContext);
            subtextContext.Expect(c => c.Repository).Returns(repository.Object);
            subtextContext.Expect(c => c.Blog).Returns(new Blog { Host = "localhost" });

            //act
            Entry cachedEntry = Cacher.GetEntryFromRequest(CacheDuration.Short, true /* allowRedirect */, subtextContext.Object);

            //assert
            Assert.AreEqual(123, cachedEntry.Id);
            Assert.AreEqual("Testing 123", cachedEntry.Title);
            Assert.AreEqual("http://localhost/archive/testing-slug.aspx", redirectLocation);
        }

        /// <summary>
        /// This test is to make sure a bug I introduced never happens again.
        /// </summary>
        [Test]
        [RollBack]
        public void GetEntryFromRequest_WithSlugInRouteDataMatchingEntryInRepository_ReturnsEntry()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Expect(r => r.GetEntry("the-slug", true, true)).Returns(new Entry(PostType.BlogPost) { Id = 123, EntryName = "the-slug", Title = "Testing 123" });
            UnitTestHelper.SetupBlog();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/the-slug.aspx");
            var routeData = new RouteData();
            routeData.Values.Add("slug", "the-slug");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Expect(c => c.RequestContext).Returns(requestContext);
            subtextContext.Expect(c => c.Repository).Returns(repository.Object);
            subtextContext.Expect(c => c.Blog).Returns(new Blog { Id = 1, TimeZoneId = -2037797565 /* pacific */ });

            //act
            Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short, true, subtextContext.Object);

            //assert
            Assert.AreEqual(123, entry.Id);
            Assert.AreEqual("Testing 123", entry.Title);
            Assert.AreEqual("the-slug", entry.EntryName);
        }

		/// <summary>
		/// This test is to make sure a bug I introduced never happens again.
		/// </summary>
		[Test]
		[RollBack]
		public void GetEntryFromRequest_WithNonExistentEntry_DoesNotThrowNullReferenceException()
		{
            //arrange
            UnitTestHelper.SetupBlog();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/99999.aspx");
            var routeData = new RouteData();
            routeData.Values.Add("id", "999999");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Expect(c => c.RequestContext).Returns(requestContext);
            
            //act
            Cacher.GetEntryFromRequest(CacheDuration.Short, true, subtextContext.Object);
            
            //assert
            //None needed.
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SingleCategoryThrowsExceptionIfContextNull()
		{
			HttpContext.Current = null;
			Cacher.SingleCategory(CacheDuration.Short);
		}

		[Test]
		[RollBack]
		public void SingleCategoryReturnsNullForNonExistentCategory()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/99.aspx");
			Assert.IsNull(Cacher.SingleCategory(CacheDuration.Short));
		}

		[Test]
		[RollBack]
		public void CanGetCategoryByIdRequest()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/");
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "This Is a Test");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/" + categoryId + ".aspx");

			LinkCategory category = Cacher.SingleCategory(CacheDuration.Short);
			Assert.AreEqual("This Is a Test", category.Title);
		}

		[Test]
		[RollBack]
		public void CanGetCategoryByNameRequest()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/This Is a Test.aspx");
			UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "This Is a Test");

			LinkCategory category = Cacher.SingleCategory(CacheDuration.Short);
			Assert.AreEqual("This Is a Test", category.Title);
		}

		[Test]
		[RollBack]
		public void CanGetCategoryByNameWithWordDelimitersRequest()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/This_Is_a_Test.aspx");
			UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "This Is a Test");

			LinkCategory category = Cacher.SingleCategory(CacheDuration.Short);
			Assert.AreEqual("This Is a Test", category.Title);
		}

		/// <summary>
		/// Makes sure that the GetActiveCategories method handles user 
		/// Locale correctly.
		/// </summary>
		[Test]
		[RollBack]
		public void GetActiveCategoriesHandlesLocale()
		{
			string hostName = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "");
			Config.CreateBlog("", "username", "thePassword", hostName, "");

			//Start with en-US
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

			// Add categories to cache.
			ICollection<LinkCategory> cachedCategories = new List<LinkCategory>();
			cachedCategories.Add(new LinkCategory(1, "Test 1"));
			cachedCategories.Add(new LinkCategory(2, "Test 2"));

			//Note, this corresponds to a private var in Cacher.
			string ActiveLCCKey = "ActiveLinkCategoryCollection:Blog{0}";
			ActiveLCCKey = String.Format(ActiveLCCKey, Config.CurrentBlog.Id);
			ContentCache cache = ContentCache.Instantiate();
			cache[ActiveLCCKey] = cachedCategories;

			ICollection<LinkCategory> categories = Cacher.GetActiveCategories(CacheDuration.Short);
			Assert.AreEqual(2, categories.Count, "Expected to get the cached categories.");
			Assert.AreSame(cachedCategories, categories, "Categories should have been pulled from cache.");

			//Change to spanish
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es");
			ICollection<LinkCategory> spanishCachedCategories = new List<LinkCategory>();
			spanishCachedCategories.Add(new LinkCategory(1, "prueba numero uno"));
			cache[ActiveLCCKey] = spanishCachedCategories;

            ICollection<LinkCategory> spanishCategories = Cacher.GetActiveCategories(CacheDuration.Short);
			Assert.AreEqual(1, spanishCategories.Count, "Only expected one category for spanish.");
		}
	}
}
