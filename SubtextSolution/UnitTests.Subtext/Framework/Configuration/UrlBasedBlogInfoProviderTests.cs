using System;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Configuration
{
	[TestFixture]
	public class UrlBasedBlogInfoProviderTests
	{
		/// <summary>
		/// If the user specifies "www.example.com" as the primary host and 
		/// a request comes in for "example.com", make sure we redirect to 
		/// the primary.
		/// </summary>
		[Test]
		[RollBack2]
		public void AlternateHostNameRequestRedirectsToPrimaryHost()
		{
			Config.CreateBlog("title", "username", "password", "subtextproject.com", string.Empty);
			Config.CreateBlog("title", "username", "password", "www.example.com", string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest("example.com", string.Empty);
			var blogRequest = new BlogRequest("example.com", string.Empty, new Uri("http://example.com/2007/01/23/some-post.aspx"), false);
            Assert.IsNull(UrlBasedBlogInfoProvider.Instance.GetBlogInfo(blogRequest), "Should return null");
			Assert.AreEqual(301, HttpContext.Current.Response.StatusCode, "Expected a 301 status code");
			Assert.AreEqual("http://www.example.com:80/2007/01/23/some-post.aspx", HttpContext.Current.Response.RedirectLocation, "Expected the url to change");
		}

		/// <summary>
		/// A request for a domain alias will redirect to the primary host.
		/// </summary>
		[Test]
		[RollBack2]
		public void RequestForAliasRedirectsToPrimaryHost()
		{
			Config.CreateBlog("title", "username", "password", "subtextproject.com", string.Empty);
			Config.CreateBlog("title", "username", "password", "example.com", string.Empty);
			BlogAlias alias = new BlogAlias();
			alias.BlogId = Config.GetBlog("example.com", string.Empty).Id;
			alias.Host = "alias.example.com";
			alias.IsActive = true;
			Config.AddBlogAlias(alias);
			UnitTestHelper.SetHttpContextWithBlogRequest("alias.example.com", string.Empty);
			var blogRequest = new BlogRequest("alias.example.com", string.Empty, new Uri("http://alias.example.com/2007/01/23/some-post.aspx"), false);
            Assert.IsNull(UrlBasedBlogInfoProvider.Instance.GetBlogInfo(blogRequest), "Should return null");
			Assert.AreEqual(301, HttpContext.Current.Response.StatusCode, "Expected a 301 status code");
			Assert.AreEqual("http://example.com:80/2007/01/23/some-post.aspx", HttpContext.Current.Response.RedirectLocation, "Expected the url to change");
		}

		/// <summary>
		/// A request for a domain alias will redirect to the primary host.
		/// </summary>
		[Test]
		[RollBack2]
		public void AliasRedirectHandlesSubfolder()
		{
            // arrange
			Config.CreateBlog("title", "username", "password", "subtextproject.com", string.Empty);
			Config.CreateBlog("title", "username", "password", "example.com", string.Empty);
			BlogAlias alias = new BlogAlias();
			alias.BlogId = Config.GetBlog("example.com", string.Empty).Id;
			alias.Host = "alias.example.com";
			alias.Subfolder = "blog";
			alias.IsActive = true;
			
            // act
            Config.AddBlogAlias(alias);
			UnitTestHelper.SetHttpContextWithBlogRequest("alias.example.com", "blog");
			var blogRequest = new BlogRequest("alias.example.com", "blog", new Uri("http://alias.example.com/blog/2007/01/23/some-post.aspx"), false);
			
            // assert
            Assert.IsNull(UrlBasedBlogInfoProvider.Instance.GetBlogInfo(blogRequest), "Should return null");
			Assert.AreEqual(301, HttpContext.Current.Response.StatusCode, "Expected a 301 status code");
			Assert.AreEqual("http://example.com:80/2007/01/23/some-post.aspx", HttpContext.Current.Response.RedirectLocation, "Expected the url to change");
		}

		[Test]
		[RollBack2]
		public void CanGetAggregateBlog()
		{
			Config.CreateBlog("title", "username", "password", "blog1.example.com", string.Empty);
			Config.CreateBlog("title", "username", "password", "blog2.example.com", string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest("example.com", string.Empty);
            var blogRequest = new BlogRequest("example.com", string.Empty, new Uri("http://example.com/"), false);
            Blog info = UrlBasedBlogInfoProvider.Instance.GetBlogInfo(blogRequest);
			Assert.AreEqual(Blog.AggregateBlog, info, "Should have received the aggregate blog");
		}

		/// <summary>
		/// This test makes sure we deal gracefully with a common deployment problem. 
		/// A user sets up the blog on his/her local machine (aka "localhost"), then 
		/// deploys the database to their production server. The hostname in the db 
		/// should be changed to the new domain.
		/// </summary>
		[Test]
		[RollBack2]
		public void GetBlogInfoChangesHostForOnlyLocalHostBlog()
		{
            UnitTestHelper.ClearAllBlogData();
			string subfolder = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("title", "username", "password", "localhost", subfolder);
			Assert.AreEqual(1, Blog.GetBlogs(0, 10, ConfigurationFlags.None).Count, "Need to make sure there's only one blog in the system.");

			UnitTestHelper.SetHttpContextWithBlogRequest("example.com", subfolder);
            var blogRequest = new BlogRequest("example.com", subfolder, new Uri("http://example.com/"), false);
            Blog info = UrlBasedBlogInfoProvider.Instance.GetBlogInfo(blogRequest);
			Assert.IsNotNull(info, "Expected to find a blog.");
			Assert.AreEqual(subfolder, info.Subfolder, "The subfolder has not changed.");
			Assert.AreEqual("example.com", info.Host, "The host should have changed.");
		}

		[Test]
		[RollBack2]
		public void GetBlogInfoFindsBlogIfItIsOnlyBlogInSystem()
		{
            UnitTestHelper.ClearAllBlogData();
			string hostName = UnitTestHelper.GenerateUniqueString();
			string subfolder = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("title", "username", "password", hostName, subfolder);
			Assert.AreEqual(1, Blog.GetBlogs(0, 10, ConfigurationFlags.None).Count, "Need to make sure there's only one blog in the system.");
			
			UnitTestHelper.SetHttpContextWithBlogRequest("example.com", subfolder);
            var blogRequest = new BlogRequest("example.com", subfolder, new Uri("http://example.com/" + subfolder + "/"), false);
            Blog info = UrlBasedBlogInfoProvider.Instance.GetBlogInfo(blogRequest);
			
			Assert.IsNotNull(info, "Expected to find a blog.");
			Assert.AreEqual(subfolder, info.Subfolder, "The subfolder should not have changed.");
			Assert.AreEqual(hostName, info.Host, "The hostName should not have changed.");
		}
	}
}
