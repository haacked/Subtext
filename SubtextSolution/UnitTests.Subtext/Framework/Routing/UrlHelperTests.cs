using System;
using System.Globalization;
using System.Web;
using System.Web.Routing;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;
using Subtext.Web;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class UrlHelperTests
    {
        [Test]
        public void EntryUrl_WithSubfolderAndEntryHavingEntryName_RendersVirtualPathToEntryWithDateAndSlugInUrl()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            Entry entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                DateCreated = dateCreated,
                EntryName = "post-slug"
            };

            //act
            string url = helper.EntryUrl(entry);

            //assert
            Assert.AreEqual("/subfolder/archive/2008/01/23/post-slug.aspx", url);
        }

        [Test]
        public void EntryUrl_WithEntryHavingEntryName_RendersVirtualPathToEntryWithDateAndSlugInUrl() {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            Entry entry = new Entry(PostType.BlogPost) { 
                Id = 123,
                DateCreated = dateCreated, 
                EntryName = "post-slug"
            };

            //act
            string url = helper.EntryUrl(entry);

            //assert
            Assert.AreEqual("/archive/2008/01/23/post-slug.aspx", url);
        }

        [Test]
        public void EntryUrl_WithEntryWhichIsReallyAnArticle_ReturnsArticleLink()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            Entry entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                DateCreated = dateCreated,
                EntryName = "post-slug",
                PostType = PostType.Story
            };

            //act
            string url = helper.EntryUrl(entry);

            //assert
            Assert.AreEqual("/articles/post-slug.aspx", url);
        }


        [Test]
        public void EntryUrl_WithEntryNotHavingEntryName_RendersVirtualPathWithId()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            Entry entry = new Entry(PostType.BlogPost)
            {
                DateCreated = dateCreated,
                EntryName = string.Empty,
                Id = 123
            };

            //act
            string url = helper.EntryUrl(entry);

            //assert
            Assert.AreEqual("/archive/2008/01/23/123.aspx", url);
        }

        [Test]
        public void EntryUrlWithAppPath_WithEntryHavingEntryName_RendersVirtualPathToEntryWithDateAndSlugInUrl()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/App");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            Entry entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                DateCreated = dateCreated,
                EntryName = "post-slug"
            };

            //act
            string url = helper.EntryUrl(entry);

            //assert
            Assert.AreEqual("/App/archive/2008/01/23/post-slug.aspx", url);
        }

        [Test]
        public void EntryUrl_WithNullEntry_ThrowsArgumentNullException()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, new RouteCollection());
            
            //act
            try {
                helper.EntryUrl(null);
            }
            catch (ArgumentNullException) {
                return;
            }

            //assert
            Assert.Fail();
        }

        [Test]
        public void EntryUrl_WithEntryHavingPostTypeOfNone_ThrowsArgumentException()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, new RouteCollection());

            //act
            try {
                helper.EntryUrl(new Entry(PostType.None));
            }
            catch (ArgumentException) {
                return;
            }

            //assert
            Assert.Fail();
        }


        [Test]
        public void FeedbackUrl_WithEntryHavingEntryName_RendersVirtualPathWithFeedbackIdInFragment()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            FeedbackItem comment = new FeedbackItem(FeedbackType.Comment)
            {
                Id = 321,
                Entry = new Entry(PostType.BlogPost)
                {
                    Id = 123,
                    DateCreated = dateCreated,
                    EntryName = "post-slug"
                }
            };

            //act
            string url = helper.FeedbackUrl(comment);

            //assert
            Assert.AreEqual("/archive/2008/01/23/post-slug.aspx#321", url);
        }

        [Test]
        public void FeedbackUrl_WithEntryIdEqualToIntMinValue_ReturnsNull()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            FeedbackItem comment = new FeedbackItem(FeedbackType.Comment)
            {
                Id = 123,
                Entry = new Entry(PostType.BlogPost)
                {
                    Id = NullValue.NullInt32,
                    DateCreated = dateCreated,
                    EntryName = "post-slug"
                }
            };

            //act
            string url = helper.FeedbackUrl(comment);

            //assert
            Assert.IsNull(url);
        }

        [Test]
        public void FeedbackUrl_WithNulFeedback_ThrowsArgumentNullException()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/App");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);

            //act
            try
            {
                helper.FeedbackUrl(null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            //assert
            Assert.Fail();
        }

        [Test]
        public void AggBugUrl_WithId_ReturnsAggBugUrlWithId()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);
            
            //act
            string url = helper.AggBugUrl(1234);

            //assert
            Assert.AreEqual("/aggbug/1234.aspx", url);
        }

        [Test]
        public void BlogUrl_WithoutSubfolder_ReturnsSlash() {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            UrlHelper helper = new UrlHelper(requestContext, routes);

            //act
            string url = helper.BlogUrl();

            //assert
            Assert.AreEqual("/", url);
        }

        [Test]
        public void BlogUrl_WithSubfolder_ReturnsSubfolder()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);

            //act
            string url = helper.BlogUrl();

            //assert
            Assert.AreEqual("/subfolder/", url);
        }

        [Test]
        public void BlogUrl_WithSubfolderAndAppPath_ReturnsSubfolder()
        {
            //arrange
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/App");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);

            //act
            string url = helper.BlogUrl();

            //assert
            Assert.AreEqual("/App/subfolder/", url);
        }

        [Test]
        public void CategoryUrl_ReturnsURlWithCategoryId() {
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);

            //act
            string url = helper.CategoryUrl(new LinkCategory { Id = 1234 });

            //assert
            Assert.AreEqual("/category/1234.aspx", url.ToString());
        }

        [Test]
        public void CategoryRssUrl_ReturnsURlWithCategoryIdInQueryString()
        {
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);

            //act
            string url = helper.CategoryRssUrl(new LinkCategory { Id = 1234 });

            //assert
            Assert.AreEqual("/rss.aspx?catId=1234", url.ToString());
        }

        [Test]
        public void AdminUrl_WithoutSubfolder_ReturnsCorrectUrl()
        {
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContext.Object, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);

            //act
            string url = helper.AdminUrl("Feedback.aspx", new {status = 2});

            //assert
            Assert.AreEqual("/admin/Feedback.aspx?status=2", url.ToString());
        }

        [Test]
        public void AdminUrl_WithSubfolderAndApplicationPath_ReturnsCorrectUrl()
        {
            var routes = new RouteCollection();
            Global.RegisterRoutes(routes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Expect(c => c.Request.ApplicationPath).Returns("/Subtext.Web");
            httpContext.Expect(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            var requestContext = new RequestContext(httpContext.Object, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);

            //act
            string url = helper.AdminUrl("Feedback.aspx", new { status = 2 });

            //assert
            Assert.AreEqual("/Subtext.Web/subfolder/admin/Feedback.aspx?status=2", url.ToString());
        }
    }
}
