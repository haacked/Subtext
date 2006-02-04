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
using System.Web;
using NUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

namespace UnitTests.Subtext.Framework.Format
{
	/// <summary>
	/// Tests of the <see cref="UrlFormats"/> class.
	/// </summary>
	[TestFixture]
	public class UrlFormatsTests
	{
		/// <summary>
		/// Makes sure that UrlFormats.GetBlogAppFromRequest does the right thing.
		/// </summary>
		[Test]
		public void GetBlogAppFromRequestDoesTheRightThing()
		{
			string rawUrl = "/Subtext.Web/MyBlog/default.aspx";
			string app = "/Subtext.Web";

			Assert.AreEqual("MyBlog", UrlFormats.GetBlogApplicationNameFromRequest(rawUrl, app));
		
			rawUrl = "/subtext.web/MyBlog/default.aspx";
			app = "/Subtext.Web";

			Assert.AreEqual("MyBlog", UrlFormats.GetBlogApplicationNameFromRequest(rawUrl, app));

			rawUrl = "/subtext.web/MyBlog/default.aspx";
			app = "Subtext.Web";

			Assert.AreEqual("MyBlog", UrlFormats.GetBlogApplicationNameFromRequest(rawUrl, app));

			rawUrl = "/subtext.web/default.aspx";
			app = "/Subtext.Web";

			Assert.AreEqual(string.Empty, UrlFormats.GetBlogApplicationNameFromRequest(rawUrl, app));

			rawUrl = "/subtext.web";
			app = "/Subtext.Web";

			Assert.AreEqual(string.Empty, UrlFormats.GetBlogApplicationNameFromRequest(rawUrl, app));

			rawUrl = "/subtext.web/myBLOG/";
			app = "/Subtext.Web";

			Assert.AreEqual("myBLOG", UrlFormats.GetBlogApplicationNameFromRequest(rawUrl, app));
		}

		/// <summary>
		/// Makes sure an entry url with no application is formatted correctly.
		/// </summary>
		[Test]
		public void EntryArchiveUrlWithNoApplication()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", string.Empty);
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Application = string.Empty;

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);
			UrlFormats formats = new UrlFormats(Config.CurrentBlog.RootUrl);
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.Parse("January 23, 1975");
			entry.EntryID = 123;
			Assert.AreEqual("http://localhost/archive/1975/01/23/123.aspx", formats.EntryUrl(entry));
		}

		/// <summary>
		/// Makes sure an entry url with no application is formatted correctly.
		/// </summary>
		[Test]
		public void EntryArchiveUrlWithApplication()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "MyBlog");
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Application = "MyBlog";

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);
			UrlFormats formats = new UrlFormats(Config.CurrentBlog.RootUrl);
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.Parse("January 23, 1975");
			entry.EntryID = 123;
			Assert.AreEqual("http://localhost/MyBlog/archive/1975/01/23/123.aspx", formats.EntryUrl(entry));
		}

		/// <summary>
		/// Makes sure an entry url with no application is formatted correctly.
		/// </summary>
		[Test]
		public void EntryArchiveUrlWithApplicationAndVirtualDir()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "MyBlog", "Subtext.Web");
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Application = "MyBlog";

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);
			UrlFormats formats = new UrlFormats(Config.CurrentBlog.RootUrl);
			Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.Parse("January 23, 1975");
			entry.EntryID = 123;
			Assert.AreEqual("http://localhost/Subtext.Web/MyBlog/archive/1975/01/23/123.aspx", formats.EntryUrl(entry));
		}
	}
}
