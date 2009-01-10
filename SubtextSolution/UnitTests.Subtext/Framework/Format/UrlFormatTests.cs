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
		string _hostName;

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
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subfolder);
			Blog info = new Blog();
			info.Host = "localhost";
			info.Subfolder = subfolder; 

			Assert.AreEqual(info.UrlFormats.AdminUrl(url), expected);

		}

		/// <summary>
		/// Makes sure the method GetEditLink distringuishes between a post and article.
		/// </summary>
		[Test]
		[RollBack]
		public void GetEditLinkDistringuishesBetweenPostAndArticle()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "");
			Config.CreateBlog("", "username", "password", _hostName, string.Empty);

			Entry postEntry = new Entry(PostType.BlogPost);
			postEntry.Id = 123;

			string editPostUrl = UrlFormats.GetEditLink(postEntry);
			Assert.AreEqual("~/Admin/Posts/Edit.aspx?PostID=123&return-to-post=true", editPostUrl, "Expected blog post to go to Posts/Edit.aspx");

			Entry articleEntry = new Entry(PostType.Story);
			articleEntry.Id = 456;
			string editArticleUrl = UrlFormats.GetEditLink(articleEntry);
			Assert.AreEqual("~/Admin/Articles/Edit.aspx?PostID=456&return-to-post=true", editArticleUrl, "Expected blog post to go to EditArticles.aspx");
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
			Assert.AreEqual(expected, actual, "Did not properly shorten url");
		}

		[Test]
		[RollBack]
		public void GetBlogNameReturnsBlogNameForEmptyVirtualDir()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog", "");
			string blogName = UrlFormats.GetBlogSubfolderFromRequest(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.ApplicationPath);
			Assert.AreEqual("MyBlog", blogName, "Wasn't able to parse request properly.");
		}

		[Test]
		[RollBack]
		public void GetBlogNameReturnsBlogNameForNonEmptyVirtualDir()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog2", "Subtext.Web");
			string blogName = UrlFormats.GetBlogSubfolderFromRequest(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.ApplicationPath);
			Assert.AreEqual("MyBlog2", blogName, "Wasn't able to parse request properly.");
		}

		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateUniqueString();
			
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
