using System;
using NUnit.Framework;
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

			Assert.AreEqual("MyBlog", UrlFormats.GetBlogAppFromRequest(rawUrl, app));
		
			rawUrl = "/subtext.web/MyBlog/default.aspx";
			app = "/Subtext.Web";

			Assert.AreEqual("MyBlog", UrlFormats.GetBlogAppFromRequest(rawUrl, app));

			rawUrl = "/subtext.web/MyBlog/default.aspx";
			app = "Subtext.Web";

			Assert.AreEqual("MyBlog", UrlFormats.GetBlogAppFromRequest(rawUrl, app));

			rawUrl = "/subtext.web/default.aspx";
			app = "/Subtext.Web";

			Assert.AreEqual(string.Empty, UrlFormats.GetBlogAppFromRequest(rawUrl, app));

			rawUrl = "/subtext.web";
			app = "/Subtext.Web";

			Assert.AreEqual(string.Empty, UrlFormats.GetBlogAppFromRequest(rawUrl, app));

			rawUrl = "/subtext.web/myBLOG/";
			app = "/Subtext.Web";

			Assert.AreEqual("myBLOG", UrlFormats.GetBlogAppFromRequest(rawUrl, app));
		}
	}
}
