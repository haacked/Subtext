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
	/// Tests of the <see cref="BlogInfo"/> class.
	/// </summary>
	[TestFixture]
	public class BlogInfoTests
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
		public void DefaultPortIs80()
		{
			HttpContext.Current = null;
			Assert.IsNull(HttpContext.Current);
			Assert.AreEqual(80, Blog.Port);
		}

		[Test]
		public void CanSetupFeedbackSpamService()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				Blog blog = new Blog();
				blog.Host = "http://subtextproject.com/";
				blog.FeedbackSpamServiceKey = null;
				Assert.IsNull(blog.FeedbackSpamService);
				Assert.IsFalse(blog.FeedbackSpamServiceEnabled);

				blog.FeedbackSpamServiceKey = "abc123";
				Assert.IsNotNull(blog.FeedbackSpamService);
				Assert.IsTrue(blog.FeedbackSpamServiceEnabled);
			}
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

			Assert.AreEqual(-1988688221, blog.GetHashCode());
		}

		[Test]
		public void CanSetFeedBurnerName()
		{
			Blog blog = new Blog();
			blog.FeedBurnerName = null;
			Assert.IsFalse(blog.FeedBurnerEnabled);

			blog.FeedBurnerName = "Subtext";
			Assert.IsTrue(blog.FeedBurnerEnabled);
		}
	    
		/// <summary>
		/// Test makes sure that the port number is included in fully qualified 
		/// urls.
		/// </summary>
		/// <param name="subfolder"></param>
		/// <param name="virtualDir"></param>
		/// <param name="port"></param>
		/// <param name="expected"></param>
		[RowTest]
		[Row("", "", 8080, ":8080/")]
		[Row("", "", 80, "/")]
		[Row("", "Subtext.Web", 8080, ":8080/Subtext.Web/")]
		[Row("blog", "Subtext.Web", 8080, ":8080/Subtext.Web/")]
		[Row("blog", "", 8080, ":8080/")]
		[RollBack2]
		public void HostFullyQualifiedUrlPropertySetCorrectly(string subfolder, string virtualDir, int port, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir, port);
			
			Assert.AreEqual("http://" + Config.CurrentBlog.Host + expected, Config.CurrentBlog.HostFullyQualifiedUrl.ToString(), "Did not set the HostFullyQualifiedUrl correctly.");
		}

	    /// <summary>
	    /// Test makes sure that the port number is included in fully qualified 
	    /// urls.
	    /// </summary>
	    /// <param name="subfolder"></param>
	    /// <param name="virtualDir"></param>
	    /// <param name="port"></param>
	    /// <param name="expected"></param>
        [RowTest]
        [Row("", "", 8080, ":8080/archive/1975/01/23/987123.aspx")]
        [Row("", "", 80, "/archive/1975/01/23/987123.aspx")]
        [Row("", "Subtext.Web", 8080, ":8080/Subtext.Web/archive/1975/01/23/987123.aspx")]
        [Row("blog", "Subtext.Web", 8080, ":8080/Subtext.Web/blog/archive/1975/01/23/987123.aspx")]
        [Row("blog", "", 8080, ":8080/blog/archive/1975/01/23/987123.aspx")]
        [RollBack2]
        public void FullyQualifiedUrlPropertySetCorrectly(string subfolder, string virtualDir, int port, string expected)
        {
			UnitTestHelper.SetupBlog(subfolder, virtualDir, port);
            
            Entry entry = new Entry(PostType.BlogPost);
            entry.DateCreated = entry.DateSyndicated = DateTime.ParseExact("1/23/1975", "M/d/yyyy", CultureInfo.InvariantCulture);
            entry.Id = 987123;
            Assert.AreEqual("http://" + Config.CurrentBlog.Host + expected, Config.CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(entry), "Did not set the entry url correctly.");
        }
	    
		[RowTest]
		[Row("", "", "/")]
		[Row("", "/", "/")]
		[Row("Blog", "", "/Blog/")]
		[Row("Blog", "Subtext.Web", "/Subtext.Web/Blog/")]
		[Row("Blog", "Subtext.Web/AnotherFolder", "/Subtext.Web/AnotherFolder/Blog/")]
		[Row("Blog", "/Subtext.Web/AnotherFolder/", "/Subtext.Web/AnotherFolder/Blog/")]
		[Row("", "Subtext.Web/", "/Subtext.Web/")]
		[Row("", "/Subtext.Web/", "/Subtext.Web/")]
		[Row("", "Subtext.Web", "/Subtext.Web/")]
		[RollBack2]
		public void TestVirtualUrlPropertySetCorrectly(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);
			
			Assert.AreEqual(expected, Config.CurrentBlog.VirtualUrl, "Did not set the Virtual Dir correctly.");
		}

		[RowTest]
		[Row("", "", "/Admin/")]
		[Row("", "/", "/Admin/")]
		[Row("Blog", "", "/Blog/Admin/")]
		[Row("Blog", "Subtext.Web", "/Subtext.Web/Blog/Admin/")]
		[Row("Blog", "Subtext.Web/AnotherFolder", "/Subtext.Web/AnotherFolder/Blog/Admin/")]
		[Row("Blog", "/Subtext.Web/AnotherFolder/", "/Subtext.Web/AnotherFolder/Blog/Admin/")]
		[Row("", "Subtext.Web/", "/Subtext.Web/Admin/")]
		[Row("", "/Subtext.Web/", "/Subtext.Web/Admin/")]
		[Row("", "Subtext.Web", "/Subtext.Web/Admin/")]
		[RollBack2]
		public void TestAdminDirectoryVirtualUrlPropertySetCorrectly(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);
			
			Assert.AreEqual(expected, Config.CurrentBlog.AdminDirectoryVirtualUrl, "Did not set the Admin Virtual Dir correctly.");
		}
		
		[RowTest]
		[Row("", "", "/Admin/Default.aspx")]
		[Row("", "/", "/Admin/Default.aspx")]
		[Row("Blog", "", "/Blog/Admin/Default.aspx")]
		[Row("Blog", "Subtext.Web", "/Subtext.Web/Blog/Admin/Default.aspx")]
		[Row("Blog", "Subtext.Web/AnotherFolder", "/Subtext.Web/AnotherFolder/Blog/Admin/Default.aspx")]
		[Row("Blog", "/Subtext.Web/AnotherFolder/", "/Subtext.Web/AnotherFolder/Blog/Admin/Default.aspx")]
		[Row("", "Subtext.Web/", "/Subtext.Web/Admin/Default.aspx")]
		[Row("", "/Subtext.Web/", "/Subtext.Web/Admin/Default.aspx")]
		[Row("", "Subtext.Web", "/Subtext.Web/Admin/Default.aspx")]
		[RollBack2]
		public void TestAdminVirtualUrlPropertySetCorrectly(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);
			
			Assert.AreEqual(expected, Config.CurrentBlog.AdminHomeVirtualUrl, "Did not set the Admin Virtual Dir correctly.");
		}

		[RowTest]
		[Row("", "Subtext.Web", "Subtext.Web/")]
		[Row("", "", "")]
		[Row("Blog", "", "Blog/")]
		[Row("Blog", "Subtext.Web", "Subtext.Web/Blog/")]
		[RollBack2]
		public void RootUrlPropertyReturnsCorrectValue(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);
			
			string expectedUrl = string.Format("http://{0}/{1}", Config.CurrentBlog.Host, expected);
			Assert.AreEqual(expectedUrl, Config.CurrentBlog.RootUrl.ToString(), "Did not set the Virtual Dir correctly.");
		}

		[RowTest]
		[Row("", "", "Default.aspx")]
		[Row("Blog", "", "Blog/Default.aspx")]
		[Row("Blog", "Subtext.Web", "Subtext.Web/Blog/Default.aspx")]
		[Row("", "Subtext.Web", "Subtext.Web/Default.aspx")]
		[RollBack2]
		public void TestBlogHomeFullyQualifiedUrlPropertySetCorrectly(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);
			string expectedUrl = string.Format("http://{0}/{1}", Config.CurrentBlog.Host, expected);
			
			Assert.AreEqual(expectedUrl, Config.CurrentBlog.HomeFullyQualifiedUrl.ToString(), "Did not set the BlogHomeUrl correctly.");
		}

		[RowTest]
		[Row("", "", "/Default.aspx")]
		[Row("Blog", "", "/Blog/Default.aspx")]
		[Row("Blog", "Subtext.Web", "/Subtext.Web/Blog/Default.aspx")]
		[Row("", "Subtext.Web", "/Subtext.Web/Default.aspx")]
		[RollBack2]
		public void TestBlogHomeVirtualUrlPropertySetCorrectly(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);

			Assert.AreEqual(expected, Config.CurrentBlog.HomeVirtualUrl, "Did not set the BlogHomeVirtualUrl correctly.");
		}

		/// <summary>
		/// Tests the virtual directory root property set correctly.
		/// </summary>
		/// <param name="subfolder">The subfolder.</param>
		/// <param name="virtualDir">The virtual dir.</param>
		/// <param name="expected">The expected.</param>
		[RowTest]
		[Row("", "", "/")]
		[Row("Blog", "", "/")]
		[Row("Blog", "Subtext.Web", "/Subtext.Web/")]
		[Row("", "Subtext.Web", "/Subtext.Web/")]
		[RollBack2]
		public void TestVirtualDirectoryRootPropertySetCorrectly(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);

			Assert.AreEqual(expected, Blog.VirtualDirectoryRoot, "Did not set the VirtualDirectoryRoot correctly.");
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
			new Blog().FeedBurnerName = "\\";
		}
	}
}
