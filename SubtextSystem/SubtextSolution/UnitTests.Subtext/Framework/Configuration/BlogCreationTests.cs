using System;
using NUnit.Framework;
using Subtext.Framework;
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
		/// Ensures that creating a blog will hash the password 
		/// if UseHashedPassword is set in web.config (as it should be).
		/// </summary>
		[Test]
		public void CreatingBlogHashesPassword()
		{
			string password = "MyPassword";
			string hashedPassword = Security.HashPassword(password);
            
			Assert.IsTrue(Config.CreateBlog("", "username", password, "LocaLhost", "MyBlog1"));
			BlogInfo info = Config.GetBlogInfo("localhost", "MyBlog1");
			Assert.IsNotNull(info, "We tried to get blog at localhost/MyBlog1 but it was null");

			Config.Settings.UseHashedPasswords = true;
			Assert.IsTrue(Config.Settings.UseHashedPasswords, "This test is voided because we're not hashing passwords");
			Assert.AreEqual(hashedPassword, info.Password, "The password wasn't hashed.");
		}

		/// <summary>
		/// Ensures that creating a blog will hash the password 
		/// if UseHashedPassword is set in web.config (as it should be).
		/// </summary>
		[Test]
		public void ModifyingBlogHashesPassword()
		{
			string password = "My Password";
			string hashedPassword = Security.HashPassword(password);
            
			Config.CreateBlog("", "username", "something", "LocaLhost", "MyBlog1");
			BlogInfo info = Config.GetBlogInfo("localhost", "MyBlog1");
			Config.Settings.UseHashedPasswords = true;
			
			info.Password = password;
			Assert.AreEqual(password, info.Password, "Passwords aren't hashed till they're saved. Otherwise loading a config would hash the hash.");
		
			Config.UpdateConfigData(info);

			Assert.AreEqual(hashedPassword, info.Password, "The password wasn't hashed.");
		}

		/// <summary>
		/// If a blog already exists with a domain name and application, one 
		/// cannot create a blog with the same domain name and no application.
		/// </summary>
		[Test, ExpectedException(typeof(BlogRequiresApplicationException))]
		public void CreatingBlogWithDuplicateHostNameRequiresApplicationName()
		{
			Config.CreateBlog("", "username", "password", "LocaLhost", "MyBlog1");
			Config.CreateBlog("", "username", "password", "LocaLhost", string.Empty);
		}

		/// <summary>
		/// Make sure adding two distinct blogs doesn't raise an exception.
		/// </summary>
		[Test]
		public void AddingDistinctBlogsIsFine()
		{
			Config.CreateBlog("title", "username", "password", "www.example.com", string.Empty);
			Config.CreateBlog("title", "username", "password", "www2.example.com", string.Empty);
			Config.CreateBlog("title", "username", "password", "example.org", string.Empty);
			Config.CreateBlog("title", "username", "password", "localhost", "Blog1");
			Config.CreateBlog("title", "username", "password", "localhost", "Blog2");
			Config.CreateBlog("title", "username", "password", "localhost", "Blog3");
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no application specified.
		/// </summary>
		[Test, ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndNoApplication()
		{
			Config.CreateBlog("title", "username", "password", "LocaLhost", string.Empty);
			Config.CreateBlog("title", "username2", "password2", "localhost", string.Empty);
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate application and host 
		/// as another blog.
		/// </summary>
		[Test, ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndApplication()
		{
			Config.CreateBlog("title", "username", "password", "localhost", "MyBlog");
			Config.CreateBlog("title", "username2", "password2", "Localhost", "MyBlog");
		}

		/// <summary>
		/// Ensures that one cannot update a blog to have a duplicate application and host 
		/// as another blog.
		/// </summary>
		[Test, ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHostAndApplication()
		{
			Config.CreateBlog("title", "username", "password", "localhost", "MyBlog");
			Config.CreateBlog("title", "username2", "password2", "example.com", "MyBlog");
			BlogInfo info = Config.GetBlogInfo("example.com", "MyBlog");
			info.Host = "localhost";
			
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Ensures that one update a blog to have a duplicate host 
		/// as another blog when both have no application specified.
		/// </summary>
		[Test, ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHost()
		{
			Config.CreateBlog("title", "username", "password", "localhost", string.Empty);
			Config.CreateBlog("title", "username2", "password2", "example.com", string.Empty);
			BlogInfo info = Config.GetBlogInfo("example.com", string.Empty);
			info.Host = "localhost";
			
			Config.UpdateConfigData(info);
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
			Config.CreateBlog("title", "username", "password", "www.example.com", string.Empty);
			Config.CreateBlog("title", "username", "password", "Example.com", "MyBlog");
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
			Config.CreateBlog("title", "username", "password", "www.mydomain.com", string.Empty);
			
			BlogInfo info = Config.GetBlogInfo("www.mydomain.com", string.Empty);
			info.Host = "mydomain.com";
			info.Application = "MyBlog";
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// If a blog already exists with a domain name and application, one 
		/// cannot modify another blog to have the same domain name, but with no application.
		/// </summary>
		[Test, ExpectedException(typeof(BlogRequiresApplicationException))]
		public void UpdatingBlogWithDuplicateHostNameRequiresApplicationName()
		{
			Config.CreateBlog("title", "username", "password", "LocaLhost", "MyBlog1");
			Config.CreateBlog("title", "username", "password", "example.com", string.Empty);

			BlogInfo info = Config.GetBlogInfo("www.example.com", string.Empty);
			info.Host = "localhost";
			info.Application = string.Empty;
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// This really tests that looking for duplicates doesn't 
		/// include the blog being edited.
		/// </summary>
		[Test]
		public void UpdatingBlogIsFine()
		{
			Config.CreateBlog("title", "username", "password", "www.example.com", string.Empty);
			BlogInfo info = Config.GetBlogInfo("www.EXAMPLE.com", string.Empty);
			info.BlogID = 1;			
			Assert.IsTrue(Config.UpdateConfigData(info), "Updating blog config should return true.");
		}

		/// <summary>
		/// Makes sure that every invalid character is checked 
		/// within the application name.
		/// </summary>
		[Test]
		public void EnsureInvalidCharactersMayNotBeUsedInApplicationName()
		{
			string[] badNames = {".name", "a{b", "a}b", "a[e", "a]e", "a/e",@"a\e", "a@e", "a!e", "a#e", "a$e", "a'e", "a%", ":e", "a^", "ae&", "*ae", "a(e", "a)e", "a?e", "+a", "e|", "a\"", "e=", "a'", "e<", "a>e", "a;", ",e", "a e"};
			foreach(string badName in badNames)
			{
				Assert.IsFalse(Config.IsValidApplicationName(badName), badName + " is not a valid app name.");
			}
		}

		#region Invalid Application Name Tests... There's a bunch...
		/// <summary>
		/// Tests that creating a blog with a reserved keyword (bin) is not allowed.
		/// </summary>
		[Test, ExpectedException(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameBin()
		{
			Config.CreateBlog("title", "blah", "blah", "localhost", "bin");
		}

		/// <summary>
		/// Tests that modifying a blog with a reserved keyword (bin) is not allowed.
		/// </summary>
		[Test, ExpectedException(typeof(InvalidApplicationNameException))]
		public void CannotRenameBlogToHaveApplicationNameBin()
		{
			Config.CreateBlog("title", "blah", "blah", "localhost", "Anything");
			BlogInfo info = Config.GetBlogInfo("localhost", "Anything");
			info.Application = "bin";

			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Tests that creating a blog with a reserved keyword (archive) is not allowed.
		/// </summary>
		[Test, ExpectedException(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameArchive()
		{
			Config.CreateBlog("title", "blah", "blah", "localhost", "archive");
			BlogInfo info = Config.GetBlogInfo("localhost", "archive");
			info.Application = "archive";

			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Tests that creating a blog that ends with . is not allowed
		/// </summary>
		[Test, ExpectedException(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameEndingWithDot()
		{
			Config.CreateBlog("title", "blah", "blah", "localhost", "archive.");
		}

		/// <summary>
		/// Tests that creating a blog that starts with . is not allowed
		/// </summary>
		[Test, ExpectedException(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameStartingWithDot()
		{
			Config.CreateBlog("title", "blah", "blah", "localhost", ".archive");
		}

		/// <summary>
		/// Tests that creating a blog that contains invalid characters is not allowed.
		/// </summary>
		[Test, ExpectedException(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameWithInvalidCharacters()
		{
			Config.CreateBlog("title", "blah", "blah", "localhost", "My!Blog");
		}
		#endregion

		[SetUp]
		public void SetUp()
		{
			//This file needs to be there already.
			UnitTestHelper.UnpackEmbeddedResource("App.config", "UnitTests.Subtext.dll.config");
			
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;

			UnitTestObjectProvider objectProvider = (UnitTestObjectProvider)ObjectProvider.Instance();
			objectProvider.ClearBlogs();
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
