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
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                DateCreated = dateCreated,
                DateSyndicated = dateCreated,
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
            BlogUrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                DateSyndicated = dateCreated,
                DateCreated = dateCreated,
                EntryName = "post-slug"
            };

            //act
            string url = helper.EntryUrl(entry);

            //assert
            Assert.AreEqual("/archive/2008/01/23/post-slug.aspx", url);
        }

        [Test]
        public void EntryUrl_WithEntryHavingEntryNameAndPublishedInTheFuture_RendersVirtualPathToEntryWithDateAndSlugInUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            DateTime dateSyndicated = DateTime.ParseExact("2008/02/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                DateCreated = dateCreated,
                DateSyndicated = dateSyndicated,
                EntryName = "post-slug"
            };

            //act
            string url = helper.EntryUrl(entry);

            //assert
            Assert.AreEqual("/archive/2008/02/23/post-slug.aspx", url);
        }


        [Test]
        public void EntryUrl_WithEntryWhichIsReallyAnArticle_ReturnsArticleLink()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                DateCreated = dateCreated,
                DateSyndicated = dateCreated,
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
            BlogUrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
            {
                DateCreated = dateCreated,
                DateSyndicated = dateCreated,
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
            BlogUrlHelper helper = SetupUrlHelper("/App");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var entry = new Entry(PostType.BlogPost)
            {
                Id = 123,
                DateCreated = dateCreated,
                DateSyndicated = dateCreated,
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
            var helper = new BlogUrlHelper(requestContext, new RouteCollection());

            //act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => helper.EntryUrl(null));
        }

        [Test]
        public void EntryUrl_WithEntryHavingPostTypeOfNone_ThrowsArgumentException()
        {
            //arrange
            var httpContext = new Mock<HttpContextBase>();
            var requestContext = new RequestContext(httpContext.Object, new RouteData());
            var helper = new BlogUrlHelper(requestContext, new RouteCollection());

            //act
            UnitTestHelper.AssertThrows<ArgumentException>(() => helper.EntryUrl(new Entry(PostType.None)));
        }


        [Test]
        public void FeedbackUrl_WithEntryHavingEntryName_RendersVirtualPathWithFeedbackIdInFragment()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            DateTime dateCreated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var comment = new FeedbackItem(FeedbackType.Comment)
            {
                Id = 321,
                Entry = new Entry(PostType.BlogPost)
                {
                    Id = 123,
                    DateCreated = dateCreated,
                    DateSyndicated = dateCreated,
                    EntryName = "post-slug"
                }
            };

            //act
            string url = helper.FeedbackUrl(comment);

            //assert
            Assert.AreEqual("/archive/2008/01/23/post-slug.aspx#321", url);
        }

        [Test]
        public void FeedbackUrl_WithEntryHavingNoEntryName_RendersVirtualPathWithFeedbackIdInFragment()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            DateTime dateSyndicated = DateTime.ParseExact("2008/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var comment = new FeedbackItem(FeedbackType.Comment)
            {
                Id = 321,
                EntryId = 1234,
                ParentDateSyndicated = dateSyndicated
            };

            //act
            string url = helper.FeedbackUrl(comment);

            //assert
            Assert.AreEqual("/archive/2008/01/23/1234.aspx#321", url);
        }

        [Test]
        public void FeedbackUrl_WithContactPageFeedback_ReturnsNullUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
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
            BlogUrlHelper helper = SetupUrlHelper("/");
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
            BlogUrlHelper helper = SetupUrlHelper("/");
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
        public void FeedbackUrl_WithNullFeedback_ThrowsArgumentNullException()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/App");

            //act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => helper.FeedbackUrl(null));
        }

        [Test]
        public void IdenticonUrl_WithAppPathWithoutSubfolder_ReturnsRootedUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");

            //act
            string url = helper.IdenticonUrl(123);

            //assert
            Assert.AreEqual("/Subtext.Web/images/services/IdenticonHandler.ashx?code=123", url);
        }

        [Test]
        public void IdenticonUrl_WithEmptyAppPathWithoutSubfolder_ReturnsRootedUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.IdenticonUrl(123);

            //assert
            Assert.AreEqual("/images/services/IdenticonHandler.ashx?code=123", url);
        }

        [Test]
        public void IdenticonUrl_WithEmptyPathWithSubfolder_IgnoresSubfolderInUrl()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "foobar");
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.IdenticonUrl(123);

            //assert
            Assert.AreEqual("/images/services/IdenticonHandler.ashx?code=123", url);
        }

        [Test]
        public void ImageUrl_WithoutBlogWithAppPathWithoutSubfolderAndImage_ReturnsRootedImageUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");

            //act
            string url = helper.ImageUrl("random.gif");

            //assert
            Assert.AreEqual("/Subtext.Web/images/random.gif", url);
        }

        [Test]
        public void ImageUrl_WithoutBlogWithEmptyAppPathWithoutSubfolderAndImage_ReturnsRootedImageUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

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
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.ImageUrl("random.gif");

            //assert
            Assert.AreEqual("/images/random.gif", url);
        }

        [Test]
        public void ImageUrl_WithBlogWithAppPathWithoutSubfolderAndImage_ReturnsUrlForImageUploadDirectory()
        {
            //arrange
            var blog = new Blog { Host = "localhost", Subfolder = "sub" };
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");

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
            BlogUrlHelper helper = SetupUrlHelper("/");

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
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.ImageUrl("random.gif");

            //assert
            Assert.AreEqual("/images/random.gif", url);
        }

        [Test]
        public void GalleryUrl_WithId_ReturnsGalleryUrlWithId()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.GalleryUrl(1234);

            //assert
            Assert.AreEqual("/gallery/1234.aspx", url);
        }

        [Test]
        public void GalleryUrl_WithImageAndBlogWithSubfolder_ReturnsGalleryUrlWithSubfolder()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var image = new Image { CategoryID = 1234, Blog = new Blog { Subfolder = "subfolder" } };

            //act
            string url = helper.GalleryUrl(image);

            //assert
            Assert.AreEqual("/subfolder/gallery/1234.aspx", url);
        }

        [Test]
        public void GalleryImageUrl_WithNullImage_ThrowsArgumentNullException()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => helper.GalleryImagePageUrl(null));
        }

        [Test]
        public void GalleryImageUrl_WithId_ReturnsGalleryUrlWithId()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.GalleryImagePageUrl(new Image { ImageID = 1234, Blog = new Blog() });

            //assert
            Assert.AreEqual("/gallery/image/1234.aspx", url);
        }

        [Test]
        public void GalleryImageUrl_WithImageInBlogWithSubfolder_ReturnsGalleryUrlWithId()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string url =
                helper.GalleryImagePageUrl(new Image { ImageID = 1234, Blog = new Blog { Subfolder = "subfolder" } });

            //assert
            Assert.AreEqual("/subfolder/gallery/image/1234.aspx", url);
        }

        [Test]
        public void GalleryImageUrl_WithImageHavingUrlAndFileName_ReturnsUrlToImage()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var image = new Image { Url = "~/images/localhost/blog1/1234/", FileName = "close.gif" };

            //act
            string url = helper.GalleryImageUrl(image, image.OriginalFile);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/blog1/1234/o_close.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithBlogHavingSubfolderAndVirtualPathAndImageHavingNullUrlAndFileName_ReturnsUrlToImage()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "localhost", Subfolder = "blog1" };
            var image = new Image { Blog = blog, Url = null, FileName = "open.gif", CategoryID = 1234 };

            //act
            string url = helper.GalleryImageUrl(image, image.OriginalFile);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/blog1/1234/o_open.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithBlogHavingSubfolderAndImageHavingNullUrlAndFileName_ReturnsUrlToImage()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "localhost", Subfolder = "blog1" };
            var image = new Image { Blog = blog, Url = null, FileName = "open.gif", CategoryID = 1234 };

            //act
            string url = helper.GalleryImageUrl(image, image.OriginalFile);

            //assert
            Assert.AreEqual("/images/localhost/blog1/1234/o_open.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithBlogHavingNoSubfolderAndImageHavingNullUrlAndFileName_ReturnsUrlToImage()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "localhost", Subfolder = "" };
            var image = new Image { Blog = blog, Url = null, FileName = "open.gif", CategoryID = 1234 };

            //act
            string url = helper.GalleryImageUrl(image, image.OriginalFile);

            //assert
            Assert.AreEqual("/images/localhost/1234/o_open.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "localhost", Subfolder = "blog1" };
            var image = new Image { CategoryID = 1234, FileName = "close.gif", Blog = blog };
            //act
            string url = helper.GalleryImageUrl(image);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/blog1/1234/o_close.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithoutAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "localhost", Subfolder = "blog1" };
            var image = new Image { CategoryID = 1234, FileName = "close.gif", Blog = blog };

            //act
            string url = helper.GalleryImageUrl(image);

            //assert
            Assert.AreEqual("/images/localhost/blog1/1234/o_close.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "localhost", Subfolder = "" };
            var image = new Image { CategoryID = 1234, FileName = "close.gif", Blog = blog };

            //act
            string url = helper.GalleryImageUrl(image);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/1234/o_close.gif", url);
        }

        [Test]
        public void GalleryImageUrl_WithoutAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "localhost", Subfolder = "" };
            var image = new Image { CategoryID = 1234, FileName = "close.gif", Blog = blog };
            //act
            string url = helper.GalleryImageUrl(image);

            //assert
            Assert.AreEqual("/images/localhost/1234/o_close.gif", url);
        }

        [Test]
        public void ImageGalleryDirectoryUrl_WithAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "localhost", Subfolder = "blog1" };

            //act
            string url = helper.ImageGalleryDirectoryUrl(blog, 1234);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/blog1/1234/", url);
        }

        [Test]
        public void ImageGalleryDirectoryUrl_WithoutAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "localhost", Subfolder = "blog1" };

            //act
            string url = helper.ImageGalleryDirectoryUrl(blog, 1234);

            //assert
            Assert.AreEqual("/images/localhost/blog1/1234/", url);
        }

        [Test]
        public void ImageGalleryDirectoryUrl_WithAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "localhost", Subfolder = "" };

            //act
            string url = helper.ImageGalleryDirectoryUrl(blog, 1234);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/1234/", url);
        }

        [Test]
        public void ImageGalleryDirectoryUrl_WithoutAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "localhost", Subfolder = "" };

            //act
            string url = helper.ImageGalleryDirectoryUrl(blog, 1234);

            //assert
            Assert.AreEqual("/images/localhost/1234/", url);
        }

        [Test]
        public void ImageDirectoryUrl_WithAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "localhost", Subfolder = "blog1" };

            //act
            string url = helper.ImageDirectoryUrl(blog);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/blog1/", url);
        }

        [Test]
        public void ImageDirectoryUrl_WithoutAppPathWithSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "localhost", Subfolder = "blog1" };

            //act
            string url = helper.ImageDirectoryUrl(blog);

            //assert
            Assert.AreEqual("/images/localhost/blog1/", url);
        }

        [Test]
        public void ImageDirectoryUrl_WithAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "localhost", Subfolder = "" };

            //act
            string url = helper.ImageDirectoryUrl(blog);

            //assert
            Assert.AreEqual("/Subtext.Web/images/localhost/Subtext_Web/", url);
        }

        [Test]
        public void ImageDirectoryUrl_WithoutAppPathWithoutSubfolderAndImage_ReturnsUrlToImageFile()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "localhost", Subfolder = "" };

            //act
            string url = helper.ImageDirectoryUrl(blog);

            //assert
            Assert.AreEqual("/images/localhost/", url);
        }

        [Test]
        public void AggBugUrl_WithId_ReturnsAggBugUrlWithId()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.AggBugUrl(1234);

            //assert
            Assert.AreEqual("/aggbug/1234.aspx", url);
        }

        [Test]
        public void BlogUrl_WithoutSubfolder_ReturnsVirtualPathToBlog()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

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
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.BlogUrl(new Blog { Subfolder = "subfolder" });

            //assert
            Assert.AreEqual("/subfolder/default.aspx", url);
        }

        [Test]
        public void BlogUrl_WithSubfolderAndAppPath_ReturnsSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            BlogUrlHelper helper = SetupUrlHelper("/App", routeData);

            //act
            string url = helper.BlogUrl();

            //assert
            Assert.AreEqual("/App/subfolder/default.aspx", url);
        }

        [Test]
        public void CategoryUrl_ReturnsURlWithCategoryId()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.CategoryUrl(new LinkCategory { Id = 1234, Title = "my-category" });

            //assert
            Assert.AreEqual("/category/my-category.aspx", url);
        }

        [Test]
        public void CategoryRssUrl_ReturnsURlWithCategoryIdInQueryString()
        {
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.CategoryRssUrl(new LinkCategory { Id = 1234, Title = "MyCategory" });

            //assert
            Assert.AreEqual("/category/MyCategory/rss", url);
        }

        [Test]
        public void AdminUrl_WithoutSubfolder_ReturnsCorrectUrl()
        {
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.AdminUrl("Feedback.aspx", new { status = 2 });

            //assert
            Assert.AreEqual("/admin/Feedback.aspx?status=2", url);
        }

        [Test]
        public void AdminUrl_WithSubfolderAndApplicationPath_ReturnsCorrectUrl()
        {
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "subfolder");
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.AdminUrl("Feedback.aspx", new { status = 2 });

            //assert
            Assert.AreEqual("/Subtext.Web/subfolder/admin/Feedback.aspx?status=2", url);
        }

        [Test]
        public void DayUrl_WithDate_ReturnsUrlWithDateInIt()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
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
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { RssProxyUrl = "test" };

            //act
            Uri url = helper.RssProxyUrl(blog);


            //assert
            Assert.AreEqual("http://feedproxy.google.com/test", url.ToString());
        }

        [Test]
        public void RssProxyUrl_WithBlogHavingSyndicationProviderUrl_ReturnsFullUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { RssProxyUrl = "http://feeds.example.com/" };

            //act
            Uri url = helper.RssProxyUrl(blog);


            //assert
            Assert.AreEqual("http://feeds.example.com/", url.ToString());
        }

        [Test]
        public void RssUrl_WithoutRssProxy_ReturnsRssUri()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "example.com" };

            //act
            Uri url = helper.RssUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/rss.aspx", url.ToString());
        }

        [Test]
        public void RssUrl_ForBlogWithSubfolderWithoutRssProxy_ReturnsRssUri()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");
            var blog = new Blog { Host = "example.com", Subfolder = "blog" };

            //act
            Uri url = helper.RssUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/Subtext.Web/blog/rss.aspx", url.ToString());
        }

        [Test]
        public void RssUrl_WithRssProxy_ReturnsProxyUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "example.com", RssProxyUrl = "http://feeds.example.com/feed" };

            //act
            Uri url = helper.RssUrl(blog);

            //assert
            Assert.AreEqual("http://feeds.example.com/feed", url.ToString());
        }

        [Test]
        public void AtomUrl_WithoutRssProxy_ReturnsRssUri()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "example.com" };

            //act
            Uri url = helper.AtomUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/atom.aspx", url.ToString());
        }

        [Test]
        public void AtomUrl_WithRssProxy_ReturnsRssUri()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");
            var blog = new Blog { Host = "example.com", RssProxyUrl = "http://atom.example.com/atom" };

            //act
            Uri url = helper.AtomUrl(blog);

            //assert
            Assert.AreEqual("http://atom.example.com/atom", url.ToString());
        }

        [Test]
        public void AdminUrl_WithPage_RendersAdminUrlToPage()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

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
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.LoginUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/login.aspx", url);
        }

        [Test]
        public void LoginUrl_WithSubfolderAndAppAndReturnUrl_ReturnsLoginUrlWithReturnUrlInQueryString()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.LoginUrl("/Subtext.Web/AdminPage.aspx").ToString().ToLowerInvariant();

            //assert
            Assert.AreEqual(("/Subtext.Web/sub/login.aspx?ReturnUrl=" + HttpUtility.UrlEncode("/Subtext.Web/AdminPage.aspx")).ToLowerInvariant(), url);
        }

        [Test]
        public void LogoutUrl_WithSubfolderAndApp_ReturnsLoginUrlInSubfolder()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.ContactFormUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/contact.aspx", url);
        }

        [Test]
        public void WlwManifestUrl_WithoutSubfolderWithoutApp_ReturnsPerBlogManifestUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/");

            //act
            string manifestUrl = helper.WlwManifestUrl();

            //assert
            Assert.AreEqual("/wlwmanifest.xml.ashx", manifestUrl);
        }

        [Test]
        public void WlwManifestUrl_WithoutSubfolderAndApp_ReturnsPerBlogManifestUrl()
        {
            //arrange
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web");

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string manifestUrl = helper.WlwManifestUrl();

            //assert
            Assert.AreEqual("/Subtext.Web/sub/wlwmanifest.xml.ashx", manifestUrl);
        }

        [Test]
        public void MetaWeblogApiUrl_WithSubfolderAndApp_ReturnsFullyQualifiedUrl()
        {
            //arrange
            var blog = new Blog { Host = "example.com", Subfolder = "sub" };
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            Uri url = helper.MetaWeblogApiUrl(blog);

            //assert
            Assert.AreEqual("http://example.com/Subtext.Web/sub/services/metablogapi.aspx", url.ToString());
        }

        [Test]
        public void RsdUrl_WithSubfolderAndApp_ReturnsFullyQualifiedUrl()
        {
            //arrange
            var blog = new Blog { Host = "example.com", Subfolder = "sub" };
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/", routeData);

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
            BlogUrlHelper helper = SetupUrlHelper("/Subtext.Web", routeData);

            //act
            string url = helper.HostAdminUrl("default.aspx");

            //assert
            Assert.AreEqual("/Subtext.Web/hostadmin/default.aspx", url);
        }

        private static BlogUrlHelper SetupUrlHelper(string appPath)
        {
            return SetupUrlHelper(appPath, new RouteData());
        }

        private static BlogUrlHelper SetupUrlHelper(string appPath, RouteData routeData)
        {
            return UnitTestHelper.SetupUrlHelper(appPath, routeData);
        }

        [RowTest]
        [Row("http://www.google.com/search?q=asp.net+mvc&ie=utf-8&oe=utf-8&aq=t&rls=org.mozilla:en-US:official&client=firefox-a", "asp.net mvc")]
        [Row("http://it.search.yahoo.com/search;_ylt=A03uv8bsRjNLZ0ABugAbDQx.?p=asp.net+mvc&fr2=sb-top&fr=yfp-t-709&rd=r1&sao=1", "asp.net mvc")]
        [Row("http://www.google.com/#hl=en&source=hp&q=asp.net+mvc&btnG=Google+Search&aq=0p&aqi=g-p3g7&oq=as&fp=cbc2f75bf9d43a8f", "asp.net mvc")]
        [Row("http://www.bing.com/search?q=asp.net+mvc&go=&form=QBLH&filt=all", "asp.net mvc")]
        [Row("http://www.google.com/search?hl=en&safe=off&client=firefox-a&rls=org.mozilla%3Aen-US%3Aofficial&hs=MUl&q=%22asp.net+mvc%22&aq=f&oq=&aqi=g-p3g7", "\"asp.net mvc\"")]
        [Row("http://codeclimber.net.nz/search.aspx?q=%22asp.net%20mvc%22", "")]
        [Row("http://www.google.it/search?rlz=1C1GGLS_enIT354IT354&sourceid=chrome&ie=UTF-8&q=site:http://haacked.com/+water+birth", "water birth")]
        [Row("http://www.google.it/search?rlz=1C1GGLS_enIT354IT354&sourceid=chrome&ie=UTF-8&q=site:https://haacked.com/+water+birth", "water birth")]
        [Row("http://www.google.it/search?rlz=1C1GGLS_enIT354IT354&sourceid=chrome&ie=UTF-8&q=water+birth+site:https://haacked.com/", "water birth")]
        public void UrlHelper_ExtractKeywordsFromReferrer_ParsesCorrectly(string referralUrl, string expectedResult)
        {
            Uri referrer = new Uri(referralUrl);
            Uri currentPath = new Uri("http://codeclimber.net.nz/archive/2009/05/20/book-review-asp.net-mvc-1.0-quickly.aspx");
            string query = BlogUrlHelper.ExtractKeywordsFromReferrer(referrer, currentPath);
            Assert.AreEqual(expectedResult, query);
        }
    }
}