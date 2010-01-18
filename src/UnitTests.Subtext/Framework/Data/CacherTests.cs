using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Infrastructure;
using UnitTests.Subtext.Framework.Util;
using System.Globalization;

namespace UnitTests.Subtext.Framework.Data
{
    /// <summary>
    /// Unit tests of the <see cref="Cacher"/> class.
    /// </summary>
    [TestFixture]
    public class CacherTests
    {
        [Test]
        public void GetOrInsert_WithItemNotInCache_InsertsItemReturnedByDelegate()
        {
            // arrange
            var cachedItem = new { Title = "Test" };
            var cache = new Mock<ICache>();
            cache.SetupGet(c => c["key"]).Returns(null);

            // act
            var cached = cache.Object.GetOrInsert("key", () => cachedItem);

            // assert
            Assert.AreEqual(cachedItem, cached);
            cache.Verify(c => c["key"]);
            cache.Verify(c => c.Insert("key",
                                      cachedItem,
                                      null,
                                      It.IsAny<DateTime>(),
                                      TimeSpan.Zero,
                                      CacheItemPriority.Normal,
                                      null));
        }

        [Test]
        public void GetOrInsert_WithItemNotInCache_ReturnsNullIfDelegateNullAndDoesNotTryToCache()
        {
            // arrange
            var cache = new Mock<ICache>();
            cache.SetupGet(c => c["key"]).Returns(null);
            cache.Setup(c => c.Insert("key", null, It.IsAny<CacheDependency>(), It.IsAny<DateTime>(), TimeSpan.Zero, CacheItemPriority.Normal, null)).Throws(new ArgumentNullException());

            // act
            var cached = cache.Object.GetOrInsert<Entry>("key", () => null);

            // assert
            Assert.IsNull(cached);
        }

        [Test]
        public void GetOrInsertSliding_WithItemNotInCache_ReturnsNullIfDelegateNullAndDoesNotTryToCache()
        {
            // arrange
            var cache = new Mock<ICache>();
            cache.SetupGet(c => c["key"]).Returns(null);
            cache.Setup(c => c.Insert("key", null, It.IsAny<CacheDependency>(), DateTime.MaxValue, It.IsAny<TimeSpan>(), CacheItemPriority.Normal, null)).Throws(new ArgumentNullException());

            // act
            var cached = cache.Object.GetOrInsertSliding<Entry>("key", () => null, null, 10);

            // assert
            Assert.IsNull(cached);
        }



        [Test]
        public void GetEntriesForMonth_WithEntriesInCache_RetrievesFromCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Testing Cacher" };
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["EntryMonth:Date200901Blog1001"]).Returns(new List<Entry> { entry });
            context.Setup(c => c.Repository.GetPostsByMonth(1, 2009)).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedEntries = Cacher.GetEntriesForMonth(dateTime, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntries.First());
        }

        [Test]
        public void GetEntriesForMonth_WithEntriesNotInCache_RetrievesFromRepositoryAndInsertsInCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Testing Cacher" };
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["EntryMonth:Date200901Blog1001"]).Returns(null);
            context.Setup(c => c.Repository.GetPostsByMonth(1, 2009)).Returns(new List<Entry> { entry });

            // act
            var cachedEntries = Cacher.GetEntriesForMonth(dateTime, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntries.First());
            context.Verify(c => c.Cache["EntryMonth:Date200901Blog1001"]);
        }

        [Test]
        public void GetEntriesForDay_WithEntriesInCache_RetrievesFromCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Testing Cacher" };
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["EntryDay:Date20090123Blog1001"]).Returns(new EntryDay(dateTime, new List<Entry> { entry }));
            context.Setup(c => c.Repository.GetEntryDay(dateTime)).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedEntries = Cacher.GetEntriesForDay(dateTime, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntries.First());
        }

        [Test]
        public void GetEntriesForDay_WithEntriesNotInCache_RetrievesFromRepositoryAndInsertsInCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Testing Cacher" };
            var dateTime = DateTime.ParseExact("20090123", "yyyyMMdd", CultureInfo.InvariantCulture);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["EntryDay:Date20090123Blog1001"]).Returns(null);
            context.Setup(c => c.Repository.GetEntryDay(dateTime)).Returns(new EntryDay(dateTime, new List<Entry> { entry }));

            // act
            var cachedEntries = Cacher.GetEntriesForDay(dateTime, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntries.First());
            context.Verify(c => c.Cache["EntryDay:Date20090123Blog1001"]);
        }

        [Test]
        public void GetEntriesByCategory_WithEntriesInCache_RetrievesFromCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Testing Cacher" };
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["EC:Count10Category1BlogId1001"]).Returns(new List<Entry> { entry });
            context.Setup(c => c.Repository.GetEntriesByCategory(10, 1, true /*activeOnly*/)).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedEntries = Cacher.GetEntriesByCategory(10 /*count*/, 1 /*categoryId*/, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntries.First());
        }

        [Test]
        public void GetEntriesByCategory_WithEntriesNotInCache_RetrievesFromRepositoryAndInsertsInCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Testing Cacher" };
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["EC:Count10Category1BlogId1001"]).Returns(null);
            context.Setup(c => c.Repository.GetEntriesByCategory(10, 1, true /*activeOnly*/)).Returns(new List<Entry> { entry });

            // act
            var cachedEntries = Cacher.GetEntriesByCategory(10 /*count*/, 1 /*categoryId*/, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntries.First());
            context.Verify(c => c.Cache["EC:Count10Category1BlogId1001"]);
        }

        [Test]
        public void GetEntriesByTag_WithEntriesInCache_RetrievesFromCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Testing Cacher" };
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["ET:Count10TagTestTagBlogId1001"]).Returns(new List<Entry> { entry });
            context.Setup(c => c.Repository.GetEntriesByTag(10, "TestTag")).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedEntries = Cacher.GetEntriesByTag(10 /*count*/, "TestTag" /*tag*/, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntries.First());
        }

        [Test]
        public void GetEntriesByTag_WithEntriesNotInCache_RetrievesFromRepositoryAndInsertsInCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Title = "Testing Cacher" };
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["ET:Count10TagTestTagBlogId1001"]).Returns(null);
            context.Setup(c => c.Repository.GetEntriesByTag(10, "TestTag")).Returns(new List<Entry> { entry });

            // act
            var cachedEntries = Cacher.GetEntriesByTag(10 /*count*/, "TestTag" /*tag*/, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntries.First());
            context.Verify(c => c.Cache["ET:Count10TagTestTagBlogId1001"]);
        }

        [Test]
        public void GetFeedback_WithFeedbackInCache_RetrievesFromCache()
        {
            // arrange
            var feedback = new FeedbackItem(FeedbackType.Comment) {Title = "Testing Cacher"};
            var parentEntry = new Entry(PostType.BlogPost) {Id = 322};
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog {Id = 1001});
            context.Setup(c => c.Cache["ParentEntry:Comments:EntryId322:BlogId1001"]).Returns(new List<FeedbackItem> {feedback});
            context.Setup(c => c.Repository.GetFeedbackForEntry(parentEntry)).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedFeedback = Cacher.GetFeedback(parentEntry, context.Object);

            // assert
            Assert.AreEqual(feedback, cachedFeedback.First());
        }

        [Test]
        public void GetFeedback_WithFeedbackNotInCache_RetrievesFromRepositoryAndInsertsInCache()
        {
            // arrange
            var feedback = new FeedbackItem(FeedbackType.Comment) {Title = "Testing Cacher"};
            var parentEntry = new Entry(PostType.BlogPost) {Id = 322};
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog {Id = 1001});
            context.Setup(c => c.Cache["ParentEntry:Comments:EntryId322:BlogId1001"]).Returns(null);
            context.Setup(c => c.Repository.GetFeedbackForEntry(parentEntry)).Returns(new List<FeedbackItem> {feedback});

            // act
            var cachedFeedback = Cacher.GetFeedback(parentEntry, context.Object);

            // assert
            Assert.AreEqual(feedback, cachedFeedback.First());
            context.Verify(c => c.Cache["ParentEntry:Comments:EntryId322:BlogId1001"]);
        }

        [Test]
        public void GetEntry_WithEntryIdAndEntryInCache_RetrievesFromCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Id = 111, Title = "Testing Cacher" };
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["Entry111BlogId1001"]).Returns(entry);
            context.Setup(c => c.Repository.GetEntry(111, true /*activeOnly*/, true /*includeCategories*/)).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedEntry = Cacher.GetEntry(111, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntry);
        }

        [Test]
        public void GetEntry_WithEntryIdAndEntryNotInCache_RetrievesFromRepositoryAndInsertsInCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Id = 111, Title = "Testing Cacher" };
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["Entry111BlogId1001"]).Returns(null);
            context.Setup(c => c.Repository.GetEntry(111, true /*activeOnly*/, true /*includeCategories*/)).Returns(entry);

            // act
            var cachedEntry = Cacher.GetEntry(111, context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntry);
            context.Verify(c => c.Cache["Entry111BlogId1001"]);
        }

        [Test]
        public void GetEntry_WithEntryNameAndEntryInCache_RetrievesFromCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Id = 111, EntryName = "entry-slug", DateSyndicated = DateTime.Now.AddDays(-1)};
            var timeZone = new Mock<ITimeZone>();
            timeZone.Setup(tz => tz.Now).Returns(DateTime.Now);
            var blog = new Mock<Blog>();
            blog.Setup(b => b.TimeZone).Returns(timeZone.Object);
            blog.Object.Id = 1001;
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(blog.Object);
            context.Setup(c => c.Cache["EntryNameentry-slugBlogId1001"]).Returns(entry);
            context.Setup(c => c.Repository.GetEntry("entry-slug", true /*activeOnly*/, true /*includeCategories*/)).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedEntry = Cacher.GetEntry("entry-slug", context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntry);
        }

        [Test]
        public void GetEntry_WithEntryNameAndEntryInCacheWithPublishDateInFuture_ReturnsNull()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Id = 111, EntryName = "entry-slug", DateSyndicated = DateTime.Now.AddDays(2) };
            var timeZone = new Mock<ITimeZone>();
            timeZone.Setup(tz => tz.Now).Returns(DateTime.Now);
            var blog = new Mock<Blog>();
            blog.Setup(b => b.TimeZone).Returns(timeZone.Object);
            blog.Object.Id = 1001;
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(blog.Object);
            context.Setup(c => c.Cache["EntryNameentry-slugBlogId1001"]).Returns(entry);
            context.Setup(c => c.Repository.GetEntry("entry-slug", true /*activeOnly*/, true /*includeCategories*/)).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedEntry = Cacher.GetEntry("entry-slug", context.Object);

            // assert
            Assert.IsNull(cachedEntry);
        }

        [Test]
        public void GetEntry_WithEntryNameAndEntryNotInCacheAndHasPublishDateInFuture_ReturnsNull()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Id = 111, EntryName = "entry-slug", DateSyndicated = DateTime.Now.AddDays(2) };
            var timeZone = new Mock<ITimeZone>();
            timeZone.Setup(tz => tz.Now).Returns(DateTime.Now);
            var blog = new Mock<Blog>();
            blog.Setup(b => b.TimeZone).Returns(timeZone.Object);
            blog.Object.Id = 1001;
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(blog.Object);
            context.Setup(c => c.Cache["EntryNameentry-slugBlogId1001"]).Returns(null);
            context.Setup(c => c.Repository.GetEntry("entry-slug", true /*activeOnly*/, true /*includeCategories*/)).Returns(entry);

            // act
            var cachedEntry = Cacher.GetEntry("entry-slug", context.Object);

            // assert
            Assert.IsNull(cachedEntry);
            context.Verify(c => c.Cache["EntryNameentry-slugBlogId1001"]);
        }

        [Test]
        public void GetEntry_WithEntryNameAndEntryNotInCache_RetrievesFromRepositoryAndInsertsInCache()
        {
            // arrange
            var entry = new Entry(PostType.BlogPost) { Id = 111, EntryName = "entry-slug", DateSyndicated = DateTime.Now.AddDays(-1) };
            var timeZone = new Mock<ITimeZone>();
            timeZone.Setup(tz => tz.Now).Returns(DateTime.Now);
            var blog = new Mock<Blog>();
            blog.Setup(b => b.TimeZone).Returns(timeZone.Object);
            blog.Object.Id = 1001;
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(blog.Object);
            context.Setup(c => c.Cache["EntryNameentry-slugBlogId1001"]).Returns(null);
            context.Setup(c => c.Repository.GetEntry("entry-slug", true /*activeOnly*/, true /*includeCategories*/)).Returns(entry);

            // act
            var cachedEntry = Cacher.GetEntry("entry-slug", context.Object);

            // assert
            Assert.AreEqual(entry, cachedEntry);
            context.Verify(c => c.Cache["EntryNameentry-slugBlogId1001"]);
        }

        [Test]
        public void GetTopTags_WithTagsInCache_RetrievesFromCache()
        {
            // arrange
            var tag = new Tag(new KeyValuePair<string, int>("Test", 123));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["TagsCount10BlogId1001"]).Returns(new[]  { tag });
            context.Setup(c => c.Repository.GetTopTags(10)).Throws(new Exception("Repository should not have been accessed"));

            // act
            var cachedTags = Cacher.GetTopTags(10, context.Object);

            // assert
            Assert.AreEqual(tag, cachedTags.First());
        }

        [Test]
        public void GetTopTags_WithTagsNotInCache_RetrievesFromRepositoryAndInsertsInCache()
        {
            // arrange
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog).Returns(new Blog { Id = 1001 });
            context.Setup(c => c.Cache["TagsCount10BlogId1001"]).Returns(null);
            context.Setup(c => c.Repository.GetTopTags(10)).Returns(new Dictionary<string, int>{{"test", 123}});

            // act
            var cachedTags = Cacher.GetTopTags(10, context.Object);

            // assert
            Assert.AreEqual("test", cachedTags.First().TagName);
            context.Verify(c => c.Cache["TagsCount10BlogId1001"]);
        }


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
            subtextContext.SetupRequestContext(httpContext, routeData, new Blog {Id = 123})
                .Setup(c => c.Repository.GetEntry(123, true, true)).Returns(new Entry(PostType.BlogPost)
                {Id = 123, Title = "Testing 123"});

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

            var entry = new Entry(PostType.BlogPost) {Id = 123, EntryName = "testing-slug", Title = "Testing 123"};
            repository.Setup(r => r.GetEntry(123, true, true)).Returns(entry);
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/archive/testing-slug.aspx");
            UnitTestHelper.SetupBlog();
            var response = new Mock<HttpResponseBase>();
            response.Setup(r => r.End());
            response.SetupSet(r => r.StatusCode, 301);
            response.SetupSet(r => r.StatusDescription, "301 Moved Permanently");
            response.SetupSet(r => r.RedirectLocation, "http://localhost/archive/testing-slug.aspx");
            var httpContext = new Mock<HttpContextBase>();
            httpContext.FakeRequest("~/archive/testing-slug.aspx");
            httpContext.Setup(c => c.Response).Returns(response.Object);
            
            var routeData = new RouteData();
            routeData.Values.Add("id", "123");
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext(httpContext, routeData)
                .SetupUrlHelper(urlHelper)
                .SetupRepository(repository.Object)
                .SetupBlog(new Blog {Host = "localhost"});
            subtextContext.Setup(c => c.HttpContext).Returns(httpContext.Object);

            //act
            Entry cachedEntry = Cacher.GetEntryFromRequest(true /* allowRedirect */, subtextContext.Object);

            //assert
            response.VerifySet(r => r.StatusCode, 301);
            response.VerifySet(r => r.StatusDescription, "301 Moved Permanently");
            response.VerifySet(r => r.RedirectLocation, "http://localhost/archive/testing-slug.aspx");
            Assert.AreEqual(123, cachedEntry.Id);
            Assert.AreEqual("Testing 123", cachedEntry.Title);
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
                .SetupBlog(new Blog {Id = 1, TimeZoneId = TimeZonesTest.PacificTimeZoneId /* pacific */})
                .Setup(c => c.Repository.GetEntry("the-slug", true, true)).Returns(new Entry(PostType.BlogPost)
                {Id = 123, EntryName = "the-slug", Title = "Testing 123"});

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
            UnitTestHelper.AssertThrowsArgumentNullException(
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
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Id = 123});
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
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Id = 123});
            subtextContext.Setup(c => c.Repository.GetLinkCategory(99, true))
                .Returns(new LinkCategory {Id = 99, Title = "this is a test"});
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
            subtextContext.Setup(c => c.Blog).Returns(new Blog {Id = 123});
            subtextContext.Setup(c => c.Repository.GetLinkCategory("this-is-a-test", true))
                .Returns(new LinkCategory {Id = 99, Title = "this is a test"});
            subtextContext.Setup(c => c.Cache[It.IsAny<string>()]).Returns(null);

            //act
            LinkCategory category = Cacher.SingleCategory(subtextContext.Object);

            //assert
            Assert.AreEqual(99, category.Id);
        }
    }
}