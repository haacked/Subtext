#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Globalization;
using System.Threading;
using System.Web;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

namespace UnitTests.Subtext.Framework.Format
{
	/// <summary>
	/// Unit tests of the <see cref="UrlFormats"/> class which 
	/// is used to format Subtext specific urls.
	/// </summary>
	[TestFixture]
	public class UrlFormatTests
	{
	    /// <summary>
		/// Makes sure that UrlFormats.GetBlogAppFromRequest does the right thing.
		/// </summary>
		[RowTest]
		[Row("/Subtext.Web/MyBlog/default.aspx", "/Subtext.Web", "MyBlog")]
		[Row("/subtext.web/MyBlog/default.aspx", "/Subtext.Web", "MyBlog")]
		[Row("/subtext.web/MyBlog/default.aspx", "Subtext.Web", "MyBlog")]
		[Row("/subtext.web/default.aspx", "/Subtext.Web", "")]
		[Row("/subtext.web", "/Subtext.Web", "")]
		[Row("/subtext.web/myBLOG/", "/Subtext.Web", "myBLOG")]
		public void GetBlogAppFromRequestDoesTheRightThing(string rawUrl, string subfolder, string expected)
		{
			Assert.AreEqual(expected, UrlFormats.GetBlogSubfolderFromRequest(rawUrl, subfolder));
		}

		[RowTest]		
		[Row("Feedback.aspx","test", "http://localhost/test/Admin/Feedback.aspx")]
		[Row("Referrers.aspx", "test", "http://localhost/test/Admin/Referrers.aspx")]
		[Row("Referrers.aspx", "", "http://localhost/Admin/Referrers.aspx")]
		public void FormatAdminUrl(string url, string subfolder, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subfolder, string.Empty, string.Empty);
			BlogInfo info = new BlogInfo();
			info.Host = "localhost";
			info.Subfolder = subfolder; 


			Assert.AreEqual(info.UrlFormats.AdminUrl(url), expected);

		}
		/// <summary>
		/// Makes sure an entry url with no application is formatted correctly.
		/// </summary>
		[Test]
		public void EntryArchiveUrlWithNoApplication()
		{
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = string.Empty;

			using (BlogRequestSimulator.SimulateRequest(blogInfo, "localhost", string.Empty, string.Empty))
			{
				UrlFormats formats = new UrlFormats(Config.CurrentBlog.RootUrl);
				Entry entry = new Entry(PostType.BlogPost);
				entry.DateCreated = DateTime.Parse("January 23, 1974");
				entry.DateSyndicated = DateTime.Parse("January 23, 1975");
				entry.Id = 123;
				Assert.AreEqual("/archive/1975/01/23/123.aspx", formats.EntryUrl(entry));
			}
		}

		/// <summary>
		/// Makes sure an entry url with no application is formatted correctly.
		/// </summary>
		[Test]
		public void EntryArchiveUrlWithApplication()
		{
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "MyBlog";

			using (BlogRequestSimulator.SimulateRequest(blogInfo, "localhost", string.Empty, "MyBlog"))
			{
				UrlFormats formats = new UrlFormats(Config.CurrentBlog.RootUrl);
				Entry entry = new Entry(PostType.BlogPost);
				entry.DateCreated = DateTime.Parse("January 23, 1974");
				entry.DateSyndicated = DateTime.Parse("January 23, 1975");
				entry.Id = 123;
				Assert.AreEqual("/MyBlog/archive/1975/01/23/123.aspx", formats.EntryUrl(entry));
			}
		}

		/// <summary>
		/// Makes sure an entry url with no application is formatted correctly.
		/// </summary>
		[Test]
		public void EntryArchiveUrlWithApplicationAndVirtualDir()
		{
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "MyBlog";

			using (BlogRequestSimulator.SimulateRequest(blogInfo, "localhost", "Subtext.Web", "MyBlog"))
			{
				UrlFormats formats = new UrlFormats(Config.CurrentBlog.RootUrl);
				Entry entry = new Entry(PostType.BlogPost);
				entry.DateCreated = DateTime.Parse("January 23, 1974");
				entry.DateSyndicated = DateTime.Parse("January 23, 1975");
				entry.Id = 123;
				Assert.AreEqual("/Subtext.Web/MyBlog/archive/1975/01/23/123.aspx", formats.EntryUrl(entry));
			}
		}
		/// <summary>
		/// Makes sure that url formatting is culture invariant.
		/// </summary>
		[Test]
		[RollBack2]
		public void FormattingEntryUrlIsCultureInvariant()
		{
			UnitTestHelper.SetupBlog("MyBlog", "Subtext.Web");
			Entry entry = new Entry(PostType.BlogPost);
			entry.Id = 123;
            entry.DateCreated = DateTime.ParseExact("2005/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            entry.DateSyndicated = DateTime.ParseExact("2006/01/23", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			entry.EntryName = "test";
			entry.BlogId = Config.CurrentBlog.Id;

			UrlFormats formats = new UrlFormats(new Uri("http://localhost/"));
			string url = formats.EntryUrl(entry);
			Assert.AreEqual("/Subtext.Web/MyBlog/archive/2006/01/23/test.aspx", url, "Expected a normally formatted url.");

			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("tr");
			url = formats.EntryUrl(entry);
			Assert.AreEqual("/Subtext.Web/MyBlog/archive/2006/01/23/test.aspx", url, "Expected a normally formatted url.");
			Assert.AreEqual("/Subtext.Web/MyBlog/archive/2006/01.aspx", formats.MonthUrl(entry.DateSyndicated), "Expected a normally formatted url.");
		}

		/// <summary>
		/// Makes sure the method GetEditLink distringuishes between a post and article.
		/// </summary>
		[Test]
		[RollBack2]
		public void GetEditLinkDistringuishesBetweenPostAndArticle()
		{
			UnitTestHelper.SetupBlog();
			
			Entry postEntry = new Entry(PostType.BlogPost);
			postEntry.Id = 123;

			string editPostUrl = UrlFormats.GetEditLink(postEntry);
			Assert.AreEqual("~/Admin/EditPosts.aspx?PostID=123", editPostUrl, "Expected blog post to go to EditPosts.aspx");

			Entry articleEntry = new Entry(PostType.Story);
			articleEntry.Id = 456;
			string editArticleUrl = UrlFormats.GetEditLink(articleEntry);
			Assert.AreEqual("~/Admin/EditArticles.aspx?PostID=456", editArticleUrl, "Expected blog post to go to EditPosts.aspx");
		}

		[RowTest]
		[Row("http://localhost/Subtext.Web/MyBlog/archive/2006/01.aspx", 50, "localhost/Subtext.Web/MyBlog/archive/2006/01.aspx")]
		[Row("http://localhost/Subtext.Web/MyBlog/archive/2006/01.aspx", 20, "localhost/.../01.aspx")]
		public void CheckUrlShortener(string source, int maxLength, string expected)
		{
			string actual = UrlFormats.ShortenUrl(source, maxLength);
			Assert.AreEqual(expected,actual,"Shortened Url is not correct.");
		}

		[RowTest]
		[Row("Test url to shorten http://localhost/Subtext.Web/MyBlog/archive/2006/01.aspx with shortener", "Test url to shorten <a href=\"http://localhost/Subtext.Web/MyBlog/archive/2006/01.aspx\">localhost/Subtext.Web/MyBlog/archive/2006/01.aspx</a> with shortener")]
		public void CheckUrlShortenerInText(string source, string expected)
		{
			string actual = UrlFormats.ResolveLinks(source);
			Console.WriteLine("Source: " + source);
			Console.WriteLine("Expected: " + expected);
			Console.WriteLine("Actual: " + actual);
			Assert.AreEqual(expected, actual, "Did not properly shorten url");
		}

		[Test]
		[RollBack2]
		public void GetBlogNameReturnsBlogNameForEmptyVirtualDir()
		{
			UnitTestHelper.SetupBlog("MyBlog");
			
			Console.WriteLine("HttpContext.Current.Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
			string blogName = UrlFormats.GetBlogSubfolderFromRequest(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.ApplicationPath);
			Assert.AreEqual("MyBlog", blogName, "Wasn't able to parse request properly.");
		}

		[Test]
		[RollBack2]
		public void GetBlogNameReturnsBlogNameForNonEmptyVirtualDir()
		{
			UnitTestHelper.SetupBlog("MyBlog2", "Subtext.Web");

			Console.WriteLine("HttpContext.Current.Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
			string blogName = UrlFormats.GetBlogSubfolderFromRequest(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.ApplicationPath);
			Assert.AreEqual("MyBlog2", blogName, "Wasn't able to parse request properly.");
		}
	}
}
