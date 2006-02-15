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
		/// Makes sure that url formatting is culture invariant.
		/// </summary>
		[Test]
		public void FormattingEntryUrlIsCultureInvariant()
		{
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.Parse("2006/01/23");
			entry.EntryName = "test";

			UrlFormats formats = new UrlFormats("http://localhost/");
			string url = formats.EntryUrl(entry);
			Assert.AreEqual("http://localhost/archive/2006/01/23/test.aspx", url, "Expected a normally formatted url.");

			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("tr");
			url = formats.EntryUrl(entry);
			Assert.AreEqual("http://localhost/archive/2006/01/23/test.aspx", url, "Expected a normally formatted url.");
		}

		/// <summary>
		/// Makes sure the method GetEditLink distringuishes between a post and article.
		/// </summary>
		[Test]
		[RollBack]
		public void GetEditLinkDistringuishesBetweenPostAndArticle()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "");
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));

			Entry postEntry = new Entry(PostType.BlogPost);
			postEntry.EntryID = 123;

			string editPostUrl = UrlFormats.GetEditLink(postEntry);
			Assert.AreEqual("~/Admin/EditPosts.aspx?PostID=123", editPostUrl, "Expected blog post to go to EditPosts.aspx");

			Entry articleEntry = new Entry(PostType.Story);
			articleEntry.EntryID = 456;
			string editArticleUrl = UrlFormats.GetEditLink(articleEntry);
			Assert.AreEqual("~/Admin/EditArticles.aspx?PostID=456", editArticleUrl, "Expected blog post to go to EditPosts.aspx");
		}

		[Test]
		[RollBack]
		public void GetBlogNameReturnsBlogNameForEmptyVirtualDir()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog", "");
			Console.WriteLine("HttpContext.Current.Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
			string blogName = UrlFormats.GetBlogNameFromRequest(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.ApplicationPath);
			Assert.AreEqual("MyBlog", blogName, "Wasn't able to parse request properly.");
		}

		[Test]
		[RollBack]
		public void GetBlogNameReturnsBlogNameForNonEmptyVirtualDir()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog2", "Subtext.Web");
			Console.WriteLine("HttpContext.Current.Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);
			string blogName = UrlFormats.GetBlogNameFromRequest(HttpContext.Current.Request.RawUrl, HttpContext.Current.Request.ApplicationPath);
			Assert.AreEqual("MyBlog2", blogName, "Wasn't able to parse request properly.");
		}

		[SetUp]
		public void SetUp()
		{
			_hostName = System.Guid.NewGuid().ToString().Replace("-", "") + ".com";
			
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
