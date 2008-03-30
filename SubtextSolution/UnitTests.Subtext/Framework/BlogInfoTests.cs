using System;
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
	    [RowTest]
		[Row("example.com", "example.com", "Should not have altered the host because it doesn't start with www.")]
		[Row("example.com:1234", "example.com:1234", "should not strip the port number")]
		[Row("www.example.com:1234", "example.com:1234", "should not strip the port number, but should strip www.")]
		[Row(null, null, "Expect an exception", ExpectedException = typeof(ArgumentException))]
	    public void StripWwwPrefixFromHostFunctionsProperly(string host, string expected, string message)
	    {
	        Assert.AreEqual(expected, BlogInfo.StripWwwPrefixFromHost(host), message);
	    }

	    [RowTest]
		[Row("example.com", "example.com", "Should not have altered the host because it doesn't have the port.")]
		[Row("example.com:1234", "example.com", "should strip the port number")]
		[Row("www.example.com:12345678910", "www.example.com", "should strip the port number.")]
		[Row(null, null, "Expect an exception", ExpectedException = typeof(ArgumentException))]
		public void StripPortFromHostFunctionsProperly(string host, string expected, string message)
	    {
	        Assert.AreEqual(expected, BlogInfo.StripPortFromHost(host), message);
	    }

		[RowTest]
		[Row("example.com", "www.example.com", "Should have prefixed with www.")]
		[Row("example.com:1234", "www.example.com:1234", "should not strip the port number and add prefix")]
		[Row("www.example.com:12345678910", "example.com:12345678910", "should strip the www prefix.")]
		[Row("www.example.com", "example.com", "should strip the www prefix.")]
		[Row(null, null, "Expect an exception", ExpectedException = typeof(ArgumentException))]
		public void CanGetAlternativeHostAlias(string host, string expected, string message)
		{
			Assert.AreEqual(expected, BlogInfo.GetAlternateHostAlias(host), message);
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
			string host = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(host, port, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestVirtualUrlPropertySetCorrectly", "username", "password", host, subfolder));

			Assert.AreEqual("http://" + host + expected, Config.CurrentBlog.HostFullyQualifiedUrl.ToString(), "Did not set the HostFullyQualifiedUrl correctly.");
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
            string host = UnitTestHelper.GenerateRandomString();
            UnitTestHelper.SetHttpContextWithBlogRequest(host, port, subfolder, virtualDir);
            Assert.IsTrue(Config.CreateBlog("TestVirtualUrlPropertySetCorrectly", "username", "password", host, subfolder));
            
            Entry entry = new Entry(PostType.BlogPost);
			entry.DateCreated = DateTime.Parse("January 23, 1975");
            entry.Id = 987123;
            Assert.AreEqual("http://" + host + expected, Config.CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(entry), "Did not set the entry url correctly.");
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
			string host = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestVirtualUrlPropertySetCorrectly", "username", "password", host, subfolder));
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
			string host = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestVirtualUrlPropertySetCorrectly", "username", "password", host, subfolder));
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
			string host = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestVirtualUrlPropertySetCorrectly", "username", "password", host, subfolder));
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
			string host = UnitTestHelper.GenerateRandomString();
			string expectedUrl = string.Format("http://{0}/{1}", host, expected);
			
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestRootUrlPropertySetCorrectly", "username", "password", host, subfolder));

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
			string host = UnitTestHelper.GenerateRandomString();
			string expectedUrl = string.Format("http://{0}/{1}", host, expected);
			
			Assert.IsTrue(Config.CreateBlog("TestBlogHomeUrlPropertySetCorrectly", "username", "password", host, subfolder));
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);

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
			string host = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("TestBlogHomeVirtualUrlPropertySetCorrectly", "username", "password", host, subfolder));
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);

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
			string host = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(host, subfolder, virtualDir);
			Assert.IsTrue(Config.CreateBlog("TestVirtualDirectoryRootPropertySetCorrectly", "username", "password", host, subfolder));

			Assert.AreEqual(expected, Config.CurrentBlog.VirtualDirectoryRoot, "Did not set the VirtualDirectoryRoot correctly.");
		}
	}
}
