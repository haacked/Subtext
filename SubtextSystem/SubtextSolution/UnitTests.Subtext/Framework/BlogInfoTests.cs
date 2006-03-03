using System;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework
{
	/// <summary>
	/// Tests of the <see cref="BlogInfo"/> class.
	/// </summary>
	[TestFixture]
	public class BlogInfoTests
	{
		[RowTest]
		[Row("example.com", "", "", "/")]
		[Row("example.com", "", "/", "/")]
		[Row("example.com", "Blog", "", "/Blog/")]
		[Row("example.com", "Blog", "Subtext.Web", "/Subtext.Web/Blog/")]
		[Row("example.com", "Blog", "Subtext.Web/AnotherFolder", "/Subtext.Web/AnotherFolder/Blog/")]
		[Row("example.com", "Blog", "/Subtext.Web/AnotherFolder/", "/Subtext.Web/AnotherFolder/Blog/")]
		[Row("example.com", "", "Subtext.Web", "/Subtext.Web/")]
		[RollBack]
		public void TestVirtualUrlPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.VirtualUrl, "Did not set the Virtual Dir correctly.");
		}

		[RowTest]
		[Row("example.com", "", "", "http://example.com/")]
		[Row("example.com", "Blog", "", "http://example.com/Blog/")]
		[Row("example.com", "Blog", "Subtext.Web", "http://example.com/Subtext.Web/Blog/")]
		[Row("example.com", "", "Subtext.Web", "http://example.com/Subtext.Web/")]
		[RollBack]
		public void TestRootUrlPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.RootUrl, "Did not set the Virtual Dir correctly.");
		}

		[RowTest]
		[Row("example.com", "", "", "http://example.com/Default.aspx")]
		[Row("example.com", "Blog", "", "http://example.com/Blog/Default.aspx")]
		[Row("example.com", "Blog", "Subtext.Web", "http://example.com/Subtext.Web/Blog/Default.aspx")]
		[Row("example.com", "", "Subtext.Web", "http://example.com/Subtext.Web/Default.aspx")]
		[RollBack]
		public void TestBlogHomeUrlPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.BlogHomeUrl, "Did not set the BlogHomeUrl correctly.");
		}

		[RowTest]
		[Row("example.com", "", "", "/Default.aspx")]
		[Row("example.com", "Blog", "", "/Blog/Default.aspx")]
		[Row("example.com", "Blog", "Subtext.Web", "/Subtext.Web/Blog/Default.aspx")]
		[Row("example.com", "", "Subtext.Web", "/Subtext.Web/Default.aspx")]
		[RollBack]
		public void TestBlogHomeVirtualUrlPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.BlogHomeVirtualUrl, "Did not set the BlogHomeVirtualUrl correctly.");
		}
	}
}
