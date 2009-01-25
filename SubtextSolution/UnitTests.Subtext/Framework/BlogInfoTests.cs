using System;
using System.Web;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using System.Globalization;
using Subtext.Extensibility.Interfaces;

namespace UnitTests.Subtext.Framework
{
	/// <summary>
	/// Tests of the <see cref="Blog"/> class.
	/// </summary>
	[TestFixture]
	public class BlogTests
	{
		[RowTest]
		[Row("example.com", "example.com", "Should not have altered the host because it doesn't start with www.")]
		[Row("example.com:1234", "example.com:1234", "should not strip the port number")]
		[Row("www.example.com:1234", "example.com:1234", "should not strip the port number, but should strip www.")]
		[Row(null, null, "Expect an exception", ExpectedException = typeof(ArgumentException))]
		public void StripWwwPrefixFromHostFunctionsProperly(string host, string expected, string message)
		{
			Assert.AreEqual(expected, Blog.StripWwwPrefixFromHost(host), message);
		}

		[RowTest]
		[Row("example.com", "example.com", "Should not have altered the host because it doesn't have the port.")]
		[Row("example.com:1234", "example.com", "should strip the port number")]
		[Row("www.example.com:12345678910", "www.example.com", "should strip the port number.")]
		[Row(null, null, "Expect an exception", ExpectedException = typeof(ArgumentException))]
		public void StripPortFromHostFunctionsProperly(string host, string expected, string message)
		{
			Assert.AreEqual(expected, Blog.StripPortFromHost(host), message);
		}

		[RowTest]
		[Row("example.com", "www.example.com", "Should have prefixed with www.")]
		[Row("example.com:1234", "www.example.com:1234", "should not strip the port number and add prefix")]
		[Row("www.example.com:12345678910", "example.com:12345678910", "should strip the www prefix.")]
		[Row("www.example.com", "example.com", "should strip the www prefix.")]
		[Row(null, null, "Expect an exception", ExpectedException = typeof(ArgumentException))]
		public void CanGetAlternativeHostAlias(string host, string expected, string message)
		{
			Assert.AreEqual(expected, Blog.GetAlternateHostAlias(host), message);
		}
	    
		/// <summary>
	    /// Makes sure we can setup the fake HttpContext.
	    /// </summary>
	    [Test]
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
		[Test]
		public void PropertyGetSetTests()
		{
			Blog blog = new Blog();

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
            Assert.IsTrue((blog.Flag & ConfigurationFlags.ShowAuthorEmailAddressinRss) == ConfigurationFlags.ShowAuthorEmailAddressinRss);
            blog.ShowEmailAddressInRss = false;
            Assert.IsTrue((blog.Flag & ConfigurationFlags.ShowAuthorEmailAddressinRss) != ConfigurationFlags.ShowAuthorEmailAddressinRss);

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
		}

		[Test]
		[RollBack2]
		public void CanGetBlogs()
		{
			UnitTestHelper.SetupBlog();
			IPagedCollection<Blog> blogs = Blog.GetBlogs(0, int.MaxValue, ConfigurationFlags.None);
			Assert.GreaterEqualThan(blogs.Count, 1);
			foreach(Blog blog in blogs)
			{
				if (blog.Id == Config.CurrentBlog.Id)
					return;
			}
			Assert.Fail("Did not find the blog we created");
		}

		[Test]
		public void CanTestForEquality()
		{
			Blog blog = new Blog();
			blog.Id = 12;
			Assert.IsFalse(blog.Equals(null), "Blog should not equal null");
			Assert.IsFalse(blog.Equals("Something Not A Blog"), "Blog should not equal a string");

			Blog blog2 = new Blog();
			blog2.Id = 12;
			Assert.IsTrue(blog.Equals(blog2));
		}

		[Test]
		public void CanGetDefaultTimeZone()
		{
			Blog blog = new Blog();
			blog.TimeZoneId = int.MinValue;
			Assert.IsNotNull(blog.TimeZone);
		}

		[Test]
		public void CanGetLanguageAndLanguageCode()
		{
			Blog blog = new Blog();
			blog.Language = null;
			Assert.AreEqual("en-US", blog.Language, "By default, the language is en-US");
			Assert.AreEqual("en", blog.LanguageCode);

			blog.Language = "fr-FR";
			Assert.AreEqual("fr-FR", blog.Language, "The language should have changed.");
			Assert.AreEqual("fr", blog.LanguageCode);
		}
		
		[Test]
		public void HasNewsReturnsProperResult()
		{
			Blog blog = new Blog();
			Assert.IsFalse(blog.HasNews);
			blog.News = "You rock! Story at eleven";
			Assert.IsTrue(blog.HasNews);
		}

		[Test]
		public void CanGetHashCode()
		{
			Blog blog = new Blog();
			blog.Host = "http://subtextproject.com";
			blog.Subfolder = "blog";

            Assert.AreEqual(158795427, blog.GetHashCode());
		}

		[Test]
		public void CanSetFeedBurnerName()
		{
			Blog blog = new Blog();
			blog.RssProxyUrl = null;
			Assert.IsFalse(blog.RssProxyEnabled);

			blog.RssProxyUrl = "Subtext";
			Assert.IsTrue(blog.RssProxyEnabled);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void GetBlogsByHostThrowsArgumentNullException()
		{
			Blog.GetBlogsByHost(null, 0, 10, ConfigurationFlags.IsActive);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void FeedBurnerNameThrowsInvalidOperationException()
		{
			new Blog().RssProxyUrl = "\\";
		}
	}
}
