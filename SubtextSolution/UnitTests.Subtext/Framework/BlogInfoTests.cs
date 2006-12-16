using System;
using System.Globalization;
using System.Web;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	/// <summary>
	/// Tests of the <see cref="BlogInfo"/> class.
	/// </summary>
	[TestFixture]
	public class BlogInfoTests
	{
		[Test]
		public void PropertyGetSetTests()
		{
			BlogInfo blog = new BlogInfo();
			blog.CaptchaEnabled = true;
			Assert.IsTrue((blog.Flag & ConfigurationFlag.CaptchaEnabled) == ConfigurationFlag.CaptchaEnabled);
			blog.CaptchaEnabled = false;
			Assert.IsTrue((blog.Flag & ConfigurationFlag.CaptchaEnabled) != ConfigurationFlag.CaptchaEnabled);

			blog.CoCommentsEnabled = true;
			Assert.IsTrue(blog.CoCommentsEnabled);
			Assert.IsTrue((blog.Flag & ConfigurationFlag.CoCommentEnabled) == ConfigurationFlag.CoCommentEnabled);
			blog.CoCommentsEnabled = false;
			Assert.IsTrue((blog.Flag & ConfigurationFlag.CoCommentEnabled) != ConfigurationFlag.CoCommentEnabled);

			blog.IsActive = true;
			Assert.IsTrue(blog.IsActive);
			Assert.IsTrue((blog.Flag & ConfigurationFlag.IsActive) == ConfigurationFlag.IsActive);
			blog.IsActive = false;
			Assert.IsTrue((blog.Flag & ConfigurationFlag.IsActive) != ConfigurationFlag.IsActive);

			blog.IsAggregated = true;
			Assert.IsTrue(blog.IsAggregated);
			Assert.IsTrue((blog.Flag & ConfigurationFlag.IsAggregated) == ConfigurationFlag.IsAggregated);
			blog.IsAggregated = false;
			Assert.IsTrue((blog.Flag & ConfigurationFlag.IsAggregated) != ConfigurationFlag.IsAggregated);

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

			Assert.IsFalse(blog.Equals(null), "Blog should not equal null");
		}

		[Test]
		public void CanGetDefaultTimeZone()
		{
			BlogInfo blog = new BlogInfo();
			blog.TimeZoneId = int.MinValue;
			Assert.IsNotNull(blog.TimeZone);
		}

		[Test]
		public void CanGetLanguageAndLanguageCode()
		{
			BlogInfo blog = new BlogInfo();
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
			Assert.IsNull(HttpContext.Current);
			Assert.AreEqual(80, BlogInfo.Port);
		}

		[Test]
		public void CanSetupFeedbackSpamService()
		{
			UnitTestHelper.SetupHttpContextWithRequest("/");

			BlogInfo blog = new BlogInfo();
			blog.Host = "http://subtextproject.com/";
			blog.FeedbackSpamServiceKey = null;
			Assert.IsNull(blog.FeedbackSpamService);
			Assert.IsFalse(blog.FeedbackSpamServiceEnabled);

			blog.FeedbackSpamServiceKey = "abc123";
			Assert.IsNotNull(blog.FeedbackSpamService);
			Assert.IsTrue(blog.FeedbackSpamServiceEnabled);
		}

		[Test]
		public void HasNewsReturnsProperResult()
		{
			BlogInfo blog = new BlogInfo();
			Assert.IsFalse(blog.HasNews);
			blog.News = "You rock! Story at eleven";
			Assert.IsTrue(blog.HasNews);
		}

		[Test]
		public void CanGetHashCode()
		{
			BlogInfo blog = new BlogInfo();
			blog.Host = "http://subtextproject.com";
			blog.Subfolder = "blog";

			Assert.AreEqual(-1988688221, blog.GetHashCode());
		}

		[Test]
		public void CanSetFeedBurnerName()
		{
			BlogInfo blog = new BlogInfo();
			blog.FeedBurnerName = null;
			Assert.IsFalse(blog.FeedBurnerEnabled);

			blog.FeedBurnerName = "Subtext";
			Assert.IsTrue(blog.FeedBurnerEnabled);
		}

		[Test]
	    public void NormalizeHostNameFunctionsProperly()
	    {
            string host = UnitTestHelper.GenerateRandomString();
	        
	        Assert.AreEqual(host, BlogInfo.NormalizeHostName(host), "Should not have altered the host");
	        Assert.AreEqual(host, BlogInfo.NormalizeHostName("www."+host), "Did not strip the URL prefix");
	        Assert.AreEqual(host, BlogInfo.NormalizeHostName(host+":1234"), "Did not strip the port number");
	        Assert.AreEqual(host, BlogInfo.NormalizeHostName("www."+host+":2734"), "Need to strip both the prefix and port number");
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
		[RollBack]
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
        [RollBack]
        public void FullyQualifiedUrlPropertySetCorrectly(string subfolder, string virtualDir, int port, string expected)
        {
			UnitTestHelper.SetupBlog(subfolder, virtualDir, port);
            
            Entry entry = new Entry(PostType.BlogPost);
            entry.DateCreated = DateTime.ParseExact("1/23/1975", "M/d/yyyy", CultureInfo.InvariantCulture);
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
		[RollBack]
		public void TestVirtualUrlPropertySetCorrectly(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);
			
			Console.WriteLine("TEST: Subfolder: " + subfolder);
			Console.WriteLine("TEST: VirtualDir: " + virtualDir);
			Console.WriteLine("TEST: expected: " + expected);
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
		[RollBack]
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
		[RollBack]
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
		[RollBack]
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
		[RollBack]
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
		[RollBack]
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
		[RollBack]
		public void TestVirtualDirectoryRootPropertySetCorrectly(string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetupBlog(subfolder, virtualDir);

			Assert.AreEqual(expected, BlogInfo.VirtualDirectoryRoot, "Did not set the VirtualDirectoryRoot correctly.");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void GetBlogsByHostThrowsArgumentNullException()
		{
			BlogInfo.GetBlogsByHost(null, 0, 10, ConfigurationFlag.IsActive);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void FeedBurnerNameThrowsInvalidOperationException()
		{
			new BlogInfo().FeedBurnerName = "/";
		}
	}
}
