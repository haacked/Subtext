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

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class UrlHelperTests
    {
        [Test]
        public void EntryUrl_WithSubfolderAndEntryHavingEntryName_RendersVirtualPathToEntryWithDateAndSlugInUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            UrlHelper helper = SetupUrlHelper("/", routeData);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
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
        public void EntryUrl_WithEntryHavingEntryName_RendersVirtualPathToEntryWithDateAndSlugInUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
            {
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
            UrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
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
            UrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
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
            UrlHelper helper = SetupUrlHelper("/App");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
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
            var helper = new UrlHelper(requestContext, new RouteCollection());

            //act
            try
            {
                helper.EntryUrl(null);
            }
            catch(ArgumentNullException)
            {
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
            var helper = new UrlHelper(requestContext, new RouteCollection());

            //act
            try
            {
                helper.EntryUrl(new Entry(PostType.None));
            }
            catch(ArgumentException)
            {
                return;
            }

            //assert
            Assert.Fail();
        }


        [Test]
        public void FeedbackUrl_WithEntryHavingEntryName_RendersVirtualPathWithFeedbackIdInFragment()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var comment = new FeedbackItem(FeedbackType.Comment)
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
        public void FeedbackUrl_WithContactPageFeedback_ReturnsNullUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var comment = new FeedbackItem(FeedbackType.ContactPage)
            {
                Id = 321,
                Entry = new Entry(PostType.BlogPost)
            };

            //act
            string url = helper.FeedbackUrl(comment);

            //assert
            Assert.IsNull(url);
        }

        [Test]
        public void FeedbackUrl_WithNullEntry_ReturnsNullUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var comment = new FeedbackItem(FeedbackType.ContactPage)
            {
                Id = 321,
                Entry = null
            };

            //act
            string url = helper.FeedbackUrl(comment);

            //assert
            Assert.IsNull(url);
        }

        [Test]
        public void FeedbackUrl_WithEntryIdEqualToIntMinValue_ReturnsNull()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var comment = new FeedbackItem(FeedbackType.Comment)
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
            UrlHelper helper = SetupUrlHelper("/App");

            //act
            try
            {
                helper.FeedbackUrl(null);
            }
            catch(ArgumentNullException)
            {
                return;
            }

            //assert
            Assert.Fail();
        }

        [Test]
        public void IdenticonUrl_WithAppPathWithoutSubfolder_ReturnsRootedUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");

            //act
            string url = helper.IdenticonUrl(123);

            //assert
            Assert.AreEqual("/Subtext.Web/images/IdenticonHandler.ashx?code=123", url);
        }

        [Test]
        public void IdenticonUrl_WithEmptyAppPathWithoutSubfolder_ReturnsRootedUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.IdenticonUrl(123);

            //assert
            Assert.AreEqual("/images/IdenticonHandler.ashx?code=123", url);
        }

        [Test]
        public void IdenticonUrl_WithEmptyPathWithSubfolder_IgnoresSubfolderInUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "foobar");
            UrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.IdenticonUrl(123);

            //assert
            Assert.AreEqual("/images/IdenticonHandler.ashx?code=123", url);
        }

        [Test]
        public void ImageUrl_WithoutBlogWithAppPathWithoutSubfolderAndImage_ReturnsRootedImageUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");

            //act
            string url = helper.ImageUrl("random.gif");

            //assert
            Assert.AreEqual("/Subtext.Web/images/random.gif", url);
        }

        [Test]
        public void ImageUrl_WithoutBlogWithEmptyAppPathWithoutSubfolderAndImage_ReturnsRootedImageUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.ImageUrl("random.gif");

            //assert
            Assert.AreEqual("/images/random.gif", url);
        }

        [Test]
        public void ImageUrl_WithoutBlogWithSubfolderAndImage_IgnoresSubfolderInUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "foobar");
            UrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.ImageUrl("random.gif");

            //assert
            Assert.AreEqual("/images/random.gif", url);
        }

        [Test]
        public void ImageUrl_WithBlogWithAppPathWithoutSubfolderAndImage_ReturnsUrlForImageUploadDirectory()
        {
            //arrange
            var blog = new Blog {Host = "localhost", Subfolder = "sub"};
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");

            //act
            string url = helper.ImageUrl(blog, "random.gif");

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/sub/random.gif", url);
        }

        [Test]
        public void ImageUrl_WithBlogWithEmptyAppPathWithoutSubfolderAndImage_ReturnsUrlForImageUploadDirectory()
        {
            //arrange
            var blog = new Blog { Host = "localhost", Subfolder = "" };
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.ImageUrl(blog, "random.gif");

            //assert
            Assert.AreEqual("/images/localhost/random.gif", url);
        }

        [Test]
        public void ImageUrl_WithBlogWithSubfolderAndImage_IgnoresSubfolderInUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "foobar");
            UrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.ImageUrl("random.gif");

            //assert
            Assert.AreEqual("/images/random.gif", url);
        }

        [Test]
        public void GalleryUrl_WithId_ReturnsGalleryUrlWithId()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.GalleryUrl(1234);

            //assert
            Assert.AreEqual("/gallery/1234.aspx", url);
        }

        [Test]
        public void GalleryUrl_WithImageAndBlogWithSubfolder_ReturnsGalleryUrlWithSubfolder()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var image = new Image {CategoryID = 1234, Blog = new Blog {Subfolder = "subfolder"}};

            //act
            string url = helper.GalleryUrl(image);

            //assert
            Assert.AreEqual("/subfolder/gallery/1234.aspx", url);
        }

        [Test]
        public void GalleryImageUrl_WithNullImage_ThrowsArgumentNullException()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => helper.GalleryImagePageUrl(null));
        }

        [Test]
        public void GalleryImageUrl_WithId_ReturnsGalleryUrlWithId()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.GalleryImagePageUrl(new Image {ImageID = 1234, Blog = new Blog()});

            //assert
            Assert.AreEqual("/gallery/image/1234.aspx", url);
        }

        [Test]
        public void GalleryImageUrl_WithImageInBlogWithSubfolder_ReturnsGalleryUrlWithId()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url =
                helper.GalleryImagePageUrl(new Image {ImageID = 1234, Blog = new Blog {Subfolder = "subfolder"}});

            //assert
            Assert.AreEqual("/subfolder/gallery/image/1234.aspx", url);
        }

        [Test]
        public void GalleryImageUrl_WithImageHavingUrlAndFileName_ReturnsUrlToImage()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var image = new Image {Url = "~/images/localhost/blog1/1234/", FileName = "close.gif"};

            //act
            string url = helper.GalleryImageUrl(image, image.OriginalFile);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/blog1/1234/o_close.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithBlogHavingSubfolderAndVirtualPathAndImageHavingNullUrlAndFileName_ReturnsUrlToImage()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog {Host = "localhost", Subfolder = "blog1"};
            var image = new Image {Blog = blog, Url = null, FileName = "open.gif", CategoryID = 1234};

            //act
            string url = helper.GalleryImageUrl(image, image.OriginalFile);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/blog1/1234/o_open.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithBlogHavingSubfolderAndImageHavingNullUrlAndFileName_ReturnsUrlToImage()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "localhost", Subfolder = "blog1"};
            var image = new Image {Blog = blog, Url = null, FileName = "open.gif", CategoryID = 1234};

            //act
            string url = helper.GalleryImageUrl(image, image.OriginalFile);

            //assert
            Assert.AreEqual("/images/localhost/blog1/1234/o_open.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithBlogHavingNoSubfolderAndImageHavingNullUrlAndFileName_ReturnsUrlToImage()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "localhost", Subfolder = ""};
            var image = new Image {Blog = blog, Url = null, FileName = "open.gif", CategoryID = 1234};

            //act
            string url = helper.GalleryImageUrl(image, image.OriginalFile);

            //assert
            Assert.AreEqual("/images/localhost/1234/o_open.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog {Host = "localhost", Subfolder = "blog1"};
            var image = new Image {CategoryID = 1234, FileName = "close.gif", Blog = blog};
            //act
            string url = helper.GalleryImageUrl(image);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/blog1/1234/o_close.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithoutAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "localhost", Subfolder = "blog1"};
            var image = new Image {CategoryID = 1234, FileName = "close.gif", Blog = blog};

            //act
            string url = helper.GalleryImageUrl(image);

            //assert
            Assert.AreEqual("/images/localhost/blog1/1234/o_close.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog {Host = "localhost", Subfolder = ""};
            var image = new Image {CategoryID = 1234, FileName = "close.gif", Blog = blog};

            //act
            string url = helper.GalleryImageUrl(image);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/1234/o_close.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithoutAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "localhost", Subfolder = ""};
            var image = new Image {CategoryID = 1234, FileName = "close.gif", Blog = blog};
            //act
            string url = helper.GalleryImageUrl(image);

            //assert
            Assert.AreEqual("/images/localhost/1234/o_close.gif", url);
        }

        [Test]
        public void ImageGalleryDirectoryUrl_WithAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog {Host = "localhost", Subfolder = "blog1"};

            //act
            string url = helper.ImageGalleryDirectoryUrl(blog, 1234);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/blog1/1234/", url);
        }

        [Test]
        public void ImageGalleryDirectoryUrl_WithoutAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "localhost", Subfolder = "blog1"};

            //act
            string url = helper.ImageGalleryDirectoryUrl(blog, 1234);

            //assert
            Assert.AreEqual("/images/localhost/blog1/1234/", url);
        }

        [Test]
        public void ImageGalleryDirectoryUrl_WithAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog {Host = "localhost", Subfolder = ""};

            //act
            string url = helper.ImageGalleryDirectoryUrl(blog, 1234);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/1234/", url);
        }

        [Test]
        public void ImageGalleryDirectoryUrl_WithoutAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "localhost", Subfolder = ""};

            //act
            string url = helper.ImageGalleryDirectoryUrl(blog, 1234);

            //assert
            Assert.AreEqual("/images/localhost/1234/", url);
        }

        [Test]
        public void ImageDirectoryUrl_WithAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog {Host = "localhost", Subfolder = "blog1"};

            //act
            string url = helper.ImageDirectoryUrl(blog);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/blog1/", url);
        }

        [Test]
        public void ImageDirectoryUrl_WithoutAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "localhost", Subfolder = "blog1"};

            //act
            string url = helper.ImageDirectoryUrl(blog);

            //assert
            Assert.AreEqual("/images/localhost/blog1/", url);
        }

        [Test]
        public void ImageDirectoryUrl_WithAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog {Host = "localhost", Subfolder = ""};

            //act
            string url = helper.ImageDirectoryUrl(blog);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/", url);
        }

        [Test]
        public void ImageDirectoryUrl_WithoutAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "localhost", Subfolder = ""};

            //act
            string url = helper.ImageDirectoryUrl(blog);

            //assert
            Assert.AreEqual("/images/localhost/", url);
        }

        [Test]
        public void AggBugUrl_WithId_ReturnsAggBugUrlWithId()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.AggBugUrl(1234);

            //assert
            Assert.AreEqual("/aggbug/1234.aspx", url);
        }

        [Test]
        public void BlogUrl_WithoutSubfolder_ReturnsVirtualPathToBlog()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.BlogUrl();

            //assert
            Assert.AreEqual("/default.aspx", url);
        }

        [Test]
        public void BlogUrl_WithSubfolder_ReturnsVirtualPathToBlogWithSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            UrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.BlogUrl();

            //assert
            Assert.AreEqual("/subfolder/default.aspx", url);
        }

        [Test]
        public void BlogUrlWithExplicitBlogNotHavingSubfolderAndVirtualPath_WithoutSubfolderInRouteData_ReturnsSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.BlogUrl(new Blog { Subfolder = null });

            //assert
            Assert.AreEqual("/Subtext.Web/default.aspx", url);
        }

        [Test]
        public void BlogUrlWithExplicitBlogHavingSubfolderAndVirtualPath_WithoutSubfolderInRouteData_ReturnsSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.BlogUrl(new Blog { Subfolder = "subfolder" });

            //assert
            Assert.AreEqual("/Subtext.Web/subfolder/default.aspx", url);
        }

        [Test]
        public void BlogUrlWithExplicitBlogHavingSubfolder_WithoutSubfolderInRouteData_ReturnsSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            UrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.BlogUrl(new Blog {Subfolder = "subfolder"});

            //assert
            Assert.AreEqual("/subfolder/default.aspx", url);
        }

        [Test]
        public void BlogUrl_WithSubfolderAndAppPath_ReturnsSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            UrlHelper helper = SetupUrlHelper("/App", routeData);

            //act
            string url = helper.BlogUrl();

            //assert
            Assert.AreEqual("/App/subfolder/default.aspx", url);
        }

        [Test]
        public void CategoryUrl_ReturnsURlWithCategoryId()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.CategoryUrl(new LinkCategory {Id = 1234});

            //assert
            Assert.AreEqual("/category/1234.aspx", url);
        }

        [Test]
        public void CategoryRssUrl_ReturnsURlWithCategoryIdInQueryString()
        {
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.CategoryRssUrl(new LinkCategory {Id = 1234});

            //assert
            Assert.AreEqual("/rss.aspx?catId=1234", url);
        }

        [Test]
        public void AdminUrl_WithoutSubfolder_ReturnsCorrectUrl()
        {
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.AdminUrl("Feedback.aspx", new {status = 2});

            //assert
            Assert.AreEqual("/admin/Feedback.aspx?status=2", url);
        }

        [Test]
        public void AdminUrl_WithSubfolderAndApplicationPath_ReturnsCorrectUrl()
        {
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.AdminUrl("Feedback.aspx", new {status = 2});

            //assert
            Assert.AreEqual("/Subtext.Web/subfolder/admin/Feedback.aspx?status=2", url);
        }

        [Test]
        public void DayUrl_WithDate_ReturnsUrlWithDateInIt()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            //Make sure date isn't midnight.
            DateTime dateTime = DateTime.ParseExact("2009/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            dateTime.AddMinutes(231);

            //act
            string url = helper.DayUrl(dateTime);


            //assert
            Assert.AreEqual("/archive/2009/01/23.aspx", url);
        }

        [Test]
        public void RssProxyUrl_WithBlogHavingFeedBurnerName_ReturnsFeedburnerUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {RssProxyUrl = "test"};

            //act
            Uri url = helper.RssProxyUrl(blog);


            //assert
            Assert.AreEqual("http://feedproxy.google.com/test", url.ToString());
        }

        [Test]
        public void RssProxyUrl_WithBlogHavingSyndicationProviderUrl_ReturnsFullUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {RssProxyUrl = "http://feeds.example.com/"};

            //act
            Uri url = helper.RssProxyUrl(blog);


            //assert
            Assert.AreEqual("http://feeds.example.com/", url.ToString());
        }

        [Test]
        public void RssUrl_WithoutRssProxy_ReturnsRssUri()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "example.com"};

            //act
            Uri url = helper.RssUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/rss.aspx", url.ToString());
        }

        [Test]
        public void RssUrl_ForBlogWithSubfolderWithoutRssProxy_ReturnsRssUri()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "example.com", Subfolder = "blog"};

            //act
            Uri url = helper.RssUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/Subtext.Web/blog/rss.aspx", url.ToString());
        }

        [Test]
        public void RssUrl_WithRssProxy_ReturnsProxyUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "example.com", RssProxyUrl = "http://feeds.example.com/feed"};

            //act
            Uri url = helper.RssUrl(blog);

            //assert
            Assert.AreEqual("http://feeds.example.com/feed", url.ToString());
        }

        [Test]
        public void AtomUrl_WithoutRssProxy_ReturnsRssUri()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "example.com"};

            //act
            Uri url = helper.AtomUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/atom.aspx", url.ToString());
        }

        [Test]
        public void AtomUrl_WithRssProxy_ReturnsRssUri()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog {Host = "example.com", RssProxyUrl = "http://atom.example.com/atom"};

            //act
            Uri url = helper.AtomUrl(blog);

            //assert
            Assert.AreEqual("http://atom.example.com/atom", url.ToString());
        }

        [Test]
        public void AdminUrl_WithPage_RendersAdminUrlToPage()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.AdminUrl("log.aspx");

            //assert
            Assert.AreEqual("/admin/log.aspx", url);
        }

        [Test]
        public void AdminUrl_WithBlogHavingSubfolder_RendersAdminUrlToPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.AdminUrl("log.aspx");

            //assert
            Assert.AreEqual("/sub/admin/log.aspx", url);
        }

        [Test]
        public void AdminUrl_WithBlogHavingSubfolderAndVirtualPath_RendersAdminUrlToPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.AdminUrl("log.aspx");

            //assert
            Assert.AreEqual("/Subtext.Web/sub/admin/log.aspx", url);
        }

        [Test]
        public void AdminRssUrl_WithFeednameAndSubfolderAndApp_ReturnsAdminRssUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            VirtualPath url = helper.AdminRssUrl("Referrers");

            //assert
            Assert.AreEqual("/Subtext.Web/sub/admin/ReferrersRss.axd", url.ToString());
        }

        [Test]
        public void LoginUrl_WithSubfolderAndApp_ReturnsLoginUrlInSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.LoginUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/login.aspx", url);
        }

        [Test]
        public void LogoutUrl_WithSubfolderAndApp_ReturnsLoginUrlInSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.LogoutUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/account/logout.ashx", url);
        }

        [Test]
        public void LogoutUrl_WithoutSubfolderAndApp_ReturnsLoginUrlInSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.LogoutUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/account/logout.ashx", url);
        }

        [Test]
        public void ArchivesUrl_WithSubfolderAndApp_ReturnsUrlWithAppAndSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.ArchivesUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/archives.aspx", url);
        }

        [Test]
        public void ContactFormUrl_WithSubfolderAndApp_ReturnsUrlWithAppAndSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.ContactFormUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/contact.aspx", url);
        }

        [Test]
        public void WlwManifestUrl_WithoutSubfolderWithoutApp_ReturnsPerBlogManifestUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/");

            //act
            string manifestUrl = helper.WlwManifestUrl();

            //assert
            Assert.AreEqual("/wlwmanifest.xml.ashx", manifestUrl);
        }

        [Test]
        public void WlwManifestUrl_WithoutSubfolderAndApp_ReturnsPerBlogManifestUrl()
        {
            //arrange
            UrlHelper helper = SetupUrlHelper("/Subtext.Web");

            //act
            string manifestUrl = helper.WlwManifestUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/wlwmanifest.xml.ashx", manifestUrl);
        }

        [Test]
        public void WlwManifestUrl_WithSubfolderAndApp_ReturnsPerBlogManifestUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string manifestUrl = helper.WlwManifestUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/wlwmanifest.xml.ashx", manifestUrl);
        }

        [Test]
        public void MetaWeblogApiUrl_WithSubfolderAndApp_ReturnsFullyQualifiedUrl()
        {
            //arrange
            var blog = new Blog {Host = "example.com", Subfolder = "sub"};
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            Uri url = helper.MetaWeblogApiUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/Subtext.Web/sub/services/metablogapi.aspx", url.ToString());
        }

        [Test]
        public void RsdUrl_WithSubfolderAndApp_ReturnsFullyQualifiedUrl()
        {
            //arrange
            var blog = new Blog {Host = "example.com", Subfolder = "sub"};
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            Uri url = helper.RsdUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/Subtext.Web/sub/rsd.xml.ashx", url.ToString());
        }

        [Test]
        public void CustomCssUrl_WithSubfolderAndApp_ReturnsFullyQualifiedUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            VirtualPath url = helper.CustomCssUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/customcss.aspx", url.ToString());
        }

        [Test]
        public void TagUrl_WithSubfolderAndApp_ReturnsTagUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            VirtualPath url = helper.TagUrl("tagName");

            //assert
            Assert.AreEqual("/Subtext.Web/sub/tags/tagName/default.aspx", url.ToString());
        }

        [Test]
        public void TagUrl_CorrectlyEncodesPoundCharacter()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            VirtualPath url = helper.TagUrl("C#");

            //assert
            Assert.AreEqual("/Subtext.Web/sub/tags/C%23/default.aspx", url.ToString());
        }

        [Test]
        public void TagCloudUrl_WithSubfolderAndApp_ReturnsTagCloudUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            VirtualPath url = helper.TagCloudUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/tags/default.aspx", url.ToString());
        }

        [Test]
        public void AppRootUrl_WithSubfolder_ReturnsAppRootAndIgnoresSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            VirtualPath url = helper.AppRoot();

            //assert
            Assert.AreEqual("/", url.ToString());
        }

        [Test]
        public void AppRootUrl_WithSubfolderAndApp_ReturnsAppRootAndIgnoresSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            VirtualPath url = helper.AppRoot();

            //assert
            Assert.AreEqual("/Subtext.Web/", url.ToString());
        }

        [Test]
        public void EditIcon_WithSubfolderAndApp_ReturnsAppRootAndIgnoresSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            VirtualPath url = helper.EditIconUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/images/icons/edit.gif", url.ToString());
        }

        [Test]
        public void HostAdminUrl_WithBlogHavingSubfolder_RendersUrlToHostAdmin()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.HostAdminUrl("default.aspx");

            //assert
            Assert.AreEqual("/hostadmin/default.aspx", url);
        }

        [Test]
        public void HostAdminUrl_WithAppPathAndBlogHavingSubfolder_RendersUrlToHostAdmin()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            UrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.HostAdminUrl("default.aspx");

            //assert
            Assert.AreEqual("/Subtext.Web/hostadmin/default.aspx", url);
        }

        private static UrlHelper SetupUrlHelper(string appPath)
        {
            return SetupUrlHelper(appPath, new RouteData());
        }

        private static UrlHelper SetupUrlHelper(string appPath, RouteData routeData)
        {
            return UnitTestHelper.SetupUrlHelper(appPath, routeData);
        }
    }
}