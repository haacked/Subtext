using System;
using System.Web;
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
		[Row("XXX.example.com", "", "", "/")]
		[Row("XXX.example.com", "", "/", "/")]
		[Row("XXX.example.com", "Blog", "", "/Blog/")]
		[Row("XXX.example.com", "Blog", "Subtext.Web", "/Subtext.Web/Blog/")]
		[Row("XXX.example.com", "Blog", "Subtext.Web/AnotherFolder", "/Subtext.Web/AnotherFolder/Blog/")]
		[Row("XXX.example.com", "Blog", "/Subtext.Web/AnotherFolder/", "/Subtext.Web/AnotherFolder/Blog/")]
		[Row("XXX.example.com", "", "Subtext.Web", "/Subtext.Web/")]
		[RollBack]
		public void TestVirtualUrlPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestVirtualUrlPropertySetCorrectly", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.VirtualUrl, "Did not set the Virtual Dir correctly.");
		}

		[RowTest]
		[Row("XXX.example.com", "", "Subtext.Web", "http://XXX.example.com/Subtext.Web/")]
		[Row("XXX.example.com", "", "", "http://XXX.example.com/")]
		[Row("XXX.example.com", "Blog", "", "http://XXX.example.com/Blog/")]
		[Row("XXX.example.com", "Blog", "Subtext.Web", "http://XXX.example.com/Subtext.Web/Blog/")]
		[RollBack]
		public void TestRootUrlPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestRootUrlPropertySetCorrectly", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.RootUrl, "Did not set the Virtual Dir correctly.");
		}

		[RowTest]
		[Row("XXX.example.com", "", "", "http://XXX.example.com/Default.aspx")]
		[Row("XXX.example.com", "Blog", "", "http://XXX.example.com/Blog/Default.aspx")]
		[Row("XXX.example.com", "Blog", "Subtext.Web", "http://XXX.example.com/Subtext.Web/Blog/Default.aspx")]
		[Row("XXX.example.com", "", "Subtext.Web", "http://XXX.example.com/Subtext.Web/Default.aspx")]
		[RollBack]
		public void TestBlogHomeUrlPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestBlogHomeUrlPropertySetCorrectly", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.BlogHomeUrl, "Did not set the BlogHomeUrl correctly.");
		}

		[RowTest]
		[Row("XXX.example.com", "", "", "/Default.aspx")]
		[Row("XXX.example.com", "Blog", "", "/Blog/Default.aspx")]
		[Row("XXX.example.com", "Blog", "Subtext.Web", "/Subtext.Web/Blog/Default.aspx")]
		[Row("XXX.example.com", "", "Subtext.Web", "/Subtext.Web/Default.aspx")]
		[RollBack]
		public void TestBlogHomeVirtualUrlPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestBlogHomeVirtualUrlPropertySetCorrectly", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.BlogHomeVirtualUrl, "Did not set the BlogHomeVirtualUrl correctly.");
		}

		/// <summary>
		/// Tests the virtual directory root property set correctly.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="subfolder">The subfolder.</param>
		/// <param name="virtualDir">The virtual dir.</param>
		/// <param name="expected">The expected.</param>
		[RowTest]
		[Row("XXX.example.com", "", "", "/")]
		[Row("XXX.example.com", "Blog", "", "/")]
		[Row("XXX.example.com", "Blog", "Subtext.Web", "/Subtext.Web/")]
		[Row("XXX.example.com", "", "Subtext.Web", "/Subtext.Web/")]
		[RollBack]
		public void TestVirtualDirectoryRootPropertySetCorrectly(string host, string subfolder, string virtualDir, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestVirtualDirectoryRootPropertySetCorrectly", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.VirtualDirectoryRoot, "Did not set the VirtualDirectoryRoot correctly.");
		}

		[SetUp]
		public void SetUp()
		{
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current = null;
		}
	}
}
