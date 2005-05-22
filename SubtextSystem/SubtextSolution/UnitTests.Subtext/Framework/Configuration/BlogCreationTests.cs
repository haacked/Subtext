using System;
using NUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Configuration
{
	/// <summary>
	/// These are unit tests specifically of the blog creation process, 
	/// as there are many validation rules involved.
	/// </summary>
	[TestFixture]
	public class BlogCreationTests
	{
		/// <summary>
		/// If a blog already exists with a domain name and application, one 
		/// cannot create a blog with the same domain name and no application.
		/// </summary>
		[Test, ExpectedException(typeof(BlogRequiresApplicationException))]
		public void CreatingBlogWithDuplicateHostNameRequiresApplicationName()
		{
			Config.AddBlogConfiguration("username", "password", "LocaLhost", "MyBlog1");
			Config.AddBlogConfiguration("username", "password", "LocaLhost", string.Empty);
		}

		/// <summary>
		/// Make sure adding two distinct blogs doesn't raise an exception.
		/// </summary>
		[Test]
		public void AddingDistinctBlogsIsFine()
		{
			Config.AddBlogConfiguration("username", "password", "www.example.com", string.Empty);
			Config.AddBlogConfiguration("username", "password", "www2.example.com", string.Empty);
			Config.AddBlogConfiguration("username", "password", "example.org", string.Empty);
			Config.AddBlogConfiguration("username", "password", "localhost", "Blog1");
			Config.AddBlogConfiguration("username", "password", "localhost", "Blog2");
			Config.AddBlogConfiguration("username", "password", "localhost", "Blog3");
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no application specified.
		/// </summary>
		[Test, ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndNoApplication()
		{
			Config.AddBlogConfiguration("username", "password", "LocaLhost", string.Empty);
			Config.AddBlogConfiguration("username2", "password2", "localhost", string.Empty);
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate application and host 
		/// as another blog.
		/// </summary>
		[Test, ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndApplication()
		{
			Config.AddBlogConfiguration("username", "password", "localhost", "MyBlog");
			Config.AddBlogConfiguration("username2", "password2", "Localhost", "MyBlog");
		}

		/// <summary>
		/// Ensures that one cannot update a blog to have a duplicate application and host 
		/// as another blog.
		/// </summary>
		[Test, ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHostAndApplication()
		{
			Config.AddBlogConfiguration("username", "password", "localhost", "MyBlog");
			Config.AddBlogConfiguration("username2", "password2", "example.com", "MyBlog");
			BlogConfig config = Config.GetConfig("example.com", "MyBlog");
			config.Host = "localhost";
			
			Config.UpdateConfigData(config);
		}

		/// <summary>
		/// Ensures that one update a blog to have a duplicate host 
		/// as another blog when both have no application specified.
		/// </summary>
		[Test, ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHost()
		{
			Config.AddBlogConfiguration("username", "password", "localhost", string.Empty);
			Config.AddBlogConfiguration("username2", "password2", "example.com", string.Empty);
			BlogConfig config = Config.GetConfig("example.com", string.Empty);
			config.Host = "localhost";
			
			Config.UpdateConfigData(config);
		}

		/// <summary>
		/// Ensures that creating a blog cannot "hide" another blog. Read the 
		/// remarks for more details.
		/// </summary>
		/// <remarks>
		/// <p>This exception occurs when adding a blog with the same hostname as another blog, 
		/// but the original blog does not have an application name defined.</p>  
		/// <p>For example, if there exists a blog with the host "www.example.com" with no 
		/// application defined, and the admin adds another blog with the host "www.example.com" 
		/// and application as "MyBlog", this creates a multiple blog situation in the example.com 
		/// domain.  In that situation, each example.com blog MUST have an application name defined. 
		/// The URL to the example.com with no application becomes the aggregate blog.
		/// </p>
		/// </remarks>
		[Test, ExpectedException(typeof(BlogHiddenException))]
		public void CreateBlogCannotHideAnotherBlog()
		{
			Config.AddBlogConfiguration("username", "password", "www.example.com", string.Empty);
			Config.AddBlogConfiguration("username", "password", "Example.com", "MyBlog");
		}

		/// <summary>
		/// Ensures that updating a blog cannot "hide" another blog. Read the 
		/// remarks for more details.
		/// </summary>
		/// <remarks>
		/// <p>This exception occurs when adding a blog with the same hostname as another blog, 
		/// but the original blog does not have an application name defined.</p>  
		/// <p>For example, if there exists a blog with the host "www.example.com" with no 
		/// application defined, and the admin adds another blog with the host "www.example.com" 
		/// and application as "MyBlog", this creates a multiple blog situation in the example.com 
		/// domain.  In that situation, each example.com blog MUST have an application name defined. 
		/// The URL to the example.com with no application becomes the aggregate blog.
		/// </p>
		/// </remarks>
		[Test]
		public void UpdatingBlogCannotHideAnotherBlog()
		{
			Config.AddBlogConfiguration("username", "password", "www.mydomain.com", string.Empty);
			
			BlogConfig config = Config.GetConfig("www.mydomain.com", string.Empty);
			config.Host = "mydomain.com";
			config.Application = "MyBlog";
			Config.UpdateConfigData(config);
		}

		/// <summary>
		/// If a blog already exists with a domain name and application, one 
		/// cannot modify another blog to have the same domain name, but with no application.
		/// </summary>
		[Test, ExpectedException(typeof(BlogRequiresApplicationException))]
		public void UpdatingBlogWithDuplicateHostNameRequiresApplicationName()
		{
			Config.AddBlogConfiguration("username", "password", "LocaLhost", "MyBlog1");
			Config.AddBlogConfiguration("username", "password", "example.com", string.Empty);

			BlogConfig config = Config.GetConfig("www.example.com", string.Empty);
			config.Host = "localhost";
			config.Application = string.Empty;
			Config.UpdateConfigData(config);
		}

		/// <summary>
		/// This really tests that looking for duplicates doesn't 
		/// include the blog being edited.
		/// </summary>
		[Test]
		public void UpdatingBlogIsFine()
		{
			Config.AddBlogConfiguration("username", "password", "www.example.com", string.Empty);
			BlogConfig config = Config.GetConfig("www.EXAMPLE.com", string.Empty);
			config.BlogID = 1;			
			Assert.IsTrue(Config.UpdateConfigData(config), "Updating blog config should return true.");
		}

		[SetUp]
		public void SetUp()
		{
			//This file needs to be there already.
			UnitTestHelper.UnpackEmbeddedResource("App.config", "UnitTests.Subtext.dll.config");
			
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;

			UnitTestDTOProvider dtoProvider = (UnitTestDTOProvider)DTOProvider.Instance();
			dtoProvider.ClearBlogs();
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
