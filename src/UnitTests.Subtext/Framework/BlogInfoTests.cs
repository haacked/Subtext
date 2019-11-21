using System;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework
{
    /// <summary>
    /// Tests of the <see cref="Blog"/> class.
    /// </summary>
    [TestClass]
    public class BlogTests
    {
        [DataTestMethod]
        [DataRow("example.com", "example.com", "Should not have altered the host because it doesn't start with www.")]
        [DataRow("example.com:1234", "example.com:1234", "should not strip the port number")]
        [DataRow("www.example.com:1234", "example.com:1234", "should not strip the port number, but should strip www.")]
        [DataRow("www.example.com", "example.com", "Should strip www.")]
        public void StripWwwPrefixFromHostFunctionsProperly(string host, string expected, string message)
        {
            Assert.AreEqual(expected, Blog.StripWwwPrefixFromHost(host), message);
        }

        [TestMethod]
        public void StripWwwPrefixFromHost_WithNullHost_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Blog.StripWwwPrefixFromHost(null));
        }

        [DataTestMethod]
        [DataRow("example.com", "example.com", "Should not have altered the host because it doesn't have the port.")]
        [DataRow("example.com:1234", "example.com", "should strip the port number")]
        [DataRow("www.example.com:12345678910", "www.example.com", "should strip the port number.")]
        public void StripPortFromHostFunctionsProperly(string host, string expected, string message)
        {
            Assert.AreEqual(expected, Blog.StripPortFromHost(host), message);
        }

        [TestMethod]
        public void StripPortFromHost_WithNullHost_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => Blog.StripPortFromHost(null));
        }

        /// <summary>
        /// Makes sure we can setup the fake HttpContext.
        /// </summary>
        [TestMethod]
        public void SetHttpContextWithBlogRequestDoesADecentSimulation()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "", "");
            Assert.AreEqual(HttpContext.Current.Request.Url.Host, "localhost");
            Assert.AreEqual(HttpContext.Current.Request.ApplicationPath, "/");

            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", "Subtext.Web");

            Assert.AreEqual(HttpContext.Current.Request.Url.Host, "localhost");
            Assert.AreEqual(HttpContext.Current.Request.ApplicationPath, "/Subtext.Web");

            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "", "Subtext.Web");

            Assert.AreEqual(HttpContext.Current.Request.Url.Host, "localhost");
            Assert.AreEqual(HttpContext.Current.Request.ApplicationPath, "/Subtext.Web");
        }

        [TestMethod]
        public void PropertyGetSetTests()
        {
            var blog = new Blog();

            Assert.AreEqual("Subtext Weblog", blog.Author, "Expected the default author name.");

            //blog.CustomMetaTags = "Test";
            //Assert.AreEqual("Test", blog.CustomMetaTags);

            //blog.TrackingCode = "Test";
            //Assert.AreEqual("Test", blog.TrackingCode);

            //blog.TrackbackNoficationEnabled = true;
            //Assert.IsTrue(blog.TrackbackNoficationEnabled);
            //blog.TrackbackNoficationEnabled = false;
            //Assert.IsFalse(blog.TrackbackNoficationEnabled);

            blog.CaptchaEnabled = true;
            Assert.IsTrue((blog.Flag & ConfigurationFlags.CaptchaEnabled) == ConfigurationFlags.CaptchaEnabled);
            Assert.IsTrue(blog.CaptchaEnabled);
            blog.CaptchaEnabled = false;
            Assert.IsTrue((blog.Flag & ConfigurationFlags.CaptchaEnabled) != ConfigurationFlags.CaptchaEnabled);

            blog.CoCommentsEnabled = true;
            Assert.IsTrue(blog.CoCommentsEnabled);
            Assert.IsTrue((blog.Flag & ConfigurationFlags.CoCommentEnabled) == ConfigurationFlags.CoCommentEnabled);
            blog.CoCommentsEnabled = false;
            Assert.IsTrue((blog.Flag & ConfigurationFlags.CoCommentEnabled) != ConfigurationFlags.CoCommentEnabled);

            blog.IsActive = true;
            Assert.IsTrue(blog.IsActive);
            Assert.IsTrue((blog.Flag & ConfigurationFlags.IsActive) == ConfigurationFlags.IsActive);
            blog.IsActive = false;
            Assert.IsTrue((blog.Flag & ConfigurationFlags.IsActive) != ConfigurationFlags.IsActive);

            blog.ShowEmailAddressInRss = true;
            Assert.IsTrue(blog.ShowEmailAddressInRss);
            Assert.IsTrue((blog.Flag & ConfigurationFlags.ShowAuthorEmailAddressinRss) ==
                          ConfigurationFlags.ShowAuthorEmailAddressinRss);
            blog.ShowEmailAddressInRss = false;
            Assert.IsTrue((blog.Flag & ConfigurationFlags.ShowAuthorEmailAddressinRss) !=
                          ConfigurationFlags.ShowAuthorEmailAddressinRss);

            blog.IsAggregated = true;
            Assert.IsTrue(blog.IsAggregated);
            Assert.IsTrue((blog.Flag & ConfigurationFlags.IsAggregated) == ConfigurationFlags.IsAggregated);
            blog.IsAggregated = false;
            Assert.IsTrue((blog.Flag & ConfigurationFlags.IsAggregated) != ConfigurationFlags.IsAggregated);

            blog.CommentCount = 42;
            Assert.AreEqual(42, blog.CommentCount);

            blog.PingTrackCount = 8;
            Assert.AreEqual(8, blog.PingTrackCount);

            blog.NumberOfRecentComments = 2006;
            Assert.AreEqual(2006, blog.NumberOfRecentComments);

            blog.PostCount = 1997;
            Assert.AreEqual(1997, blog.PostCount);

            blog.RecentCommentsLength = 1993;
            Assert.AreEqual(1993, blog.RecentCommentsLength);

            blog.StoryCount = 1975;
            Assert.AreEqual(1975, blog.StoryCount);

            UnitTestHelper.AssertSimpleProperties(blog, "OpenIdUrl");
        }

        [DatabaseIntegrationTestMethod]
        public void CanGetBlogs()
        {
            // arrange
            UnitTestHelper.SetupBlog();

            // act
            IPagedCollection<Blog> blogs = new DatabaseObjectProvider().GetBlogs(0, int.MaxValue, ConfigurationFlags.None);

            // assert
            Assert.IsTrue(blogs.Count >= 1);
            var blog = blogs.First(b => b.Id == Config.CurrentBlog.Id);
            Assert.IsNotNull(blog);
        }

        [TestMethod]
        public void CanTestForEquality()
        {
            var blog = new Blog();
            blog.Id = 12;
            Assert.IsFalse(blog.Equals(null), "Blog should not equal null");
            Assert.IsFalse(blog.Equals("Something Not A Blog"), "Blog should not equal a string");

            var blog2 = new Blog();
            blog2.Id = 12;
            Assert.IsTrue(blog.Equals(blog2));
        }

        [TestMethod]
        public void CanGetDefaultTimeZone()
        {
            var blog = new Blog();
            blog.TimeZoneId = null;
            Assert.IsNotNull(blog.TimeZone);
        }

        [TestMethod]
        public void CanGetLanguageAndLanguageCode()
        {
            var blog = new Blog();
            blog.Language = null;
            Assert.AreEqual("en-US", blog.Language, "By default, the language is en-US");
            Assert.AreEqual("en", blog.LanguageCode);

            blog.Language = "fr-FR";
            Assert.AreEqual("fr-FR", blog.Language, "The language should have changed.");
            Assert.AreEqual("fr", blog.LanguageCode);
        }

        [TestMethod]
        public void HasNewsReturnsProperResult()
        {
            var blog = new Blog();
            Assert.IsFalse(blog.HasNews);
            blog.News = "You rock! Story at eleven";
            Assert.IsTrue(blog.HasNews);
        }

        [TestMethod]
        public void CanGetHashCode()
        {
            var blog = new Blog();
            blog.Host = "http://subtextproject.com";
            blog.Subfolder = "blog";

            Assert.AreNotEqual(0, blog.GetHashCode());
        }

        [TestMethod]
        public void CanSetFeedBurnerName()
        {
            var blog = new Blog();
            blog.RssProxyUrl = null;
            Assert.IsFalse(blog.RssProxyEnabled);

            blog.RssProxyUrl = "Subtext";
            Assert.IsTrue(blog.RssProxyEnabled);
        }

        [TestMethod]
        public void GetBlogsByHostThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() =>
                                                               new DatabaseObjectProvider().GetBlogsByHost(null, 0, 10,
                                                                                   ConfigurationFlags.IsActive));
        }

        [TestMethod]
        public void RssProxyUrl_WithInvalidCharacters_ThrowsInvalidOperationException()
        {
            UnitTestHelper.AssertThrows<InvalidOperationException>(() => new Blog().RssProxyUrl = "\\");
        }

        [TestMethod]
        public void OpenIdUrl_WhenSetToValueWithoutHttp_PrependsHttp()
        {
            // arrange
            var blog = new Blog();

            // act
            blog.OpenIdUrl = "openid.example.com";

            // assert
            Assert.AreEqual("http://openid.example.com", blog.OpenIdUrl);
        }

        [TestMethod]
        public void OpenIdUrl_WhenSetToValueWithHttp_SetsUrl()
        {
            // arrange
            var blog = new Blog();

            // act
            blog.OpenIdUrl = "http://openid.example.com";

            // assert
            Assert.AreEqual("http://openid.example.com", blog.OpenIdUrl);
        }

        [TestMethod]
        public void OpenIdUrl_WhenSetToNull_IsNull()
        {
            // arrange
            var blog = new Blog();

            // act
            blog.OpenIdUrl = null;

            // assert
            Assert.IsNull(blog.OpenIdUrl);
        }
    }
}