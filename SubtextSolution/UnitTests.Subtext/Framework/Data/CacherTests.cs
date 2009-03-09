using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.HttpModules;

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
            repository.Setup(r => r.GetEntry(123, true, true)).Returns(entry);
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/archive/testing-slug.aspx");
            UnitTestHelper.SetupBlog();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/testing-slug.aspx");
            httpContext.Setup(c => c.Response.End());
            httpContext.SetupSet(c => c.Response.StatusCode, 301);
            httpContext.SetupSet(c => c.Response.Status, "301 Moved Permanently");
            string redirectLocation = null;
            httpContext.SetupSet(c => c.Response.RedirectLocation).Callback(s => redirectLocation = s);
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext, routeData)
                .SetupUrlHelper(urlHelper)
                .SetupRepository(repository.Object)
                .SetupBlog(new Blog { Host = "localhost" });

            //act
            Entry cachedEntry = Cacher.GetEntryFromRequest(CacheDuration.Short, true /* allowRedirect */, subtextContext.Object);

            //assert
            httpContext.VerifySet(c => c.Response.StatusCode, 301);
            httpContext.VerifySet(c => c.Response.Status, "301 Moved Permanently");
            Assert.AreEqual(123, cachedEntry.Id);
            Assert.AreEqual("Testing 123", cachedEntry.Title);
            Assert.AreEqual("http://localhost/archive/testing-slug.aspx", redirectLocation);
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
                .SetupBlog(new Blog { Id = 1, TimeZoneId = -2037797565 /* pacific */ })
                .Setup(c => c.Repository.GetEntry("the-slug", true, true)).Returns(new Entry(PostType.BlogPost) { Id = 123, EntryName = "the-slug", Title = "Testing 123" });

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
            Cacher.GetEntryFromRequest(CacheDuration.Short, true, subtextContext.Object);
            
            //assert
            //None needed.
		}

		[Test]
		public void SingleCategoryThrowsExceptionIfContextNull()
		{
            UnitTestHelper.AssertThrows<ArgumentNullException>(
                () => Cacher.SingleCategory(CacheDuration.Short, null)
            );
		}

		[Test]
		public void SingleCategoryReturnsNullForNonExistentCategory()
		{
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("/category/99.aspx");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext)
                .Setup(c => c.Repository.GetLinkCategory(It.IsAny<int>(), true)).Returns((LinkCategory)null);
            
            //act
            LinkCategory category = Cacher.SingleCategory(CacheDuration.Short, subtextContext.Object);

            //assert
            Assert.IsNull(category);
		}

		[Test]
		public void CanGetCategoryByIdRequest()
		{
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("/category/99.aspx");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext)
                .Setup(c => c.Repository.GetLinkCategory(99, true)).Returns(new LinkCategory { Id = 99, Title = "this is a test"});
			
            //act
            LinkCategory category = Cacher.SingleCategory(CacheDuration.Short, subtextContext.Object);
			     
            //assert
            Assert.AreEqual("this is a test", category.Title);
		}

		[Test]
		public void CanGetCategoryByNameRequest()
		{
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("/category/this-is-a-test.aspx");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext)
                .Setup(c => c.Repository.GetLinkCategory("this-is-a-test", true)).Returns(new LinkCategory { Id = 99, Title = "this is a test" });

            //act
			LinkCategory category = Cacher.SingleCategory(CacheDuration.Short, subtextContext.Object);

            //assert
			Assert.AreEqual(99, category.Id);
		}

		/// <summary>
		/// Makes sure that the GetActiveCategories method handles user 
		/// Locale correctly.
		/// </summary>
		[Test]
		public void GetActiveCategoriesHandlesLocale()
		{
            //arrange
            int blogId = 123;
			var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext();
            subtextContext.SetupBlog(new Blog { Id = blogId });

			//Start with en-US
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

			// Add categories to cache.
			ICollection<LinkCategory> cachedCategories = new List<LinkCategory>();
			cachedCategories.Add(new LinkCategory(1, "Test 1"));
			cachedCategories.Add(new LinkCategory(2, "Test 2"));

			//Note, this corresponds to a private var in Cacher.
			string ActiveLCCKey = "ActiveLinkCategoryCollection:Blog{0}";
            ActiveLCCKey = String.Format(ActiveLCCKey, blogId);
			ContentCache cache = ContentCache.Instantiate(subtextContext.Object);
			cache[ActiveLCCKey] = cachedCategories;

            ICollection<LinkCategory> categories = Cacher.GetActiveCategories(CacheDuration.Short, subtextContext.Object);
			Assert.AreEqual(2, categories.Count, "Expected to get the cached categories.");
			Assert.AreSame(cachedCategories, categories, "Categories should have been pulled from cache.");

			//Change to spanish
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es");
			ICollection<LinkCategory> spanishCachedCategories = new List<LinkCategory>();
			spanishCachedCategories.Add(new LinkCategory(1, "prueba numero uno"));
			cache[ActiveLCCKey] = spanishCachedCategories;

            ICollection<LinkCategory> spanishCategories = Cacher.GetActiveCategories(CacheDuration.Short, subtextContext.Object);
			Assert.AreEqual(1, spanishCategories.Count, "Only expected one category for spanish.");
		}
	}
}
