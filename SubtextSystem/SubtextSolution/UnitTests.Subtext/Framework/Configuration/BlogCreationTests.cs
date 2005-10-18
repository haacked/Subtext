using System;
using System.Globalization;
using NUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;

namespace UnitTests.Subtext.Framework.Configuration
{
	/// <summary>
	/// These are unit tests specifically of the blog creation process, 
	/// as there are many validation rules involved.
	/// </summary>
	[TestFixture]
	public class BlogCreationTests
	{
		string _hostName;

		/// <summary>
		/// Ensures that creating a blog will hash the password 
		/// if UseHashedPassword is set in web.config (as it should be).
		/// </summary>
		[Test]
		[Rollback]
		public void CreatingBlogHashesPassword()
		{
			string password = "MyPassword";
			string hashedPassword = Security.HashPassword(password);
            
			Assert.IsTrue(Config.CreateBlog("", "username", password, _hostName, "MyBlog1"));
			BlogInfo info = Config.GetBlogInfo(_hostName, "MyBlog1");
			Assert.IsNotNull(info, "We tried to get blog at " + _hostName + "/MyBlog1 but it was null");

			Config.Settings.UseHashedPasswords = true;
			Assert.IsTrue(Config.Settings.UseHashedPasswords, "This test is voided because we're not hashing passwords");
			Assert.AreEqual(hashedPassword, info.Password, "The password wasn't hashed.");
		}

		/// <summary>
		/// Ran into a problem where saving changes to a blog would rehash the password. 
		/// We need a separate method for changing passwords.
		/// </summary>
		[Test]
		[Rollback]
		public void ModifyingBlogShouldNotChangePassword()
		{
			Config.Settings.UseHashedPasswords = true;
			Config.CreateBlog("", "username", "thePassword", _hostName, "MyBlog1");
			BlogInfo info = Config.GetBlogInfo(_hostName.ToUpper(CultureInfo.InvariantCulture), "MyBlog1");
			string password = info.Password;
			info.LicenseUrl = "http://subtextproject.com/";
			Config.UpdateConfigData(info);
			
			info = Config.GetBlogInfo(_hostName.ToUpper(CultureInfo.InvariantCulture), "MyBlog1");
			Assert.AreEqual(password, info.Password);
		}

		/// <summary>
		/// If a blog already exists with a domain name and application, one 
		/// cannot create a blog with the same domain name and no application.
		/// </summary>
		[Test]
		//[ExpectedException(typeof(BlogRequiresApplicationException))]
		[Rollback(typeof(BlogRequiresApplicationException))]
		public void CreatingBlogWithDuplicateHostNameRequiresApplicationName()
		{
			Config.CreateBlog("", "username", "password", _hostName, "MyBlog1");
			Config.CreateBlog("", "username", "password", _hostName, string.Empty);
		}

		/// <summary>
		/// Make sure adding two distinct blogs doesn't raise an exception.
		/// </summary>
		[Test]
		[Rollback]
		public void AddingDistinctBlogsIsFine()
		{
			Config.CreateBlog("title", "username", "password", UnitTestHelper.GenerateUniqueHost(), string.Empty);
			Config.CreateBlog("title", "username", "password", "www2." + UnitTestHelper.GenerateUniqueHost(), string.Empty);
			Config.CreateBlog("title", "username", "password", UnitTestHelper.GenerateUniqueHost(), string.Empty);
			Config.CreateBlog("title", "username", "password", _hostName, "Blog1");
			Config.CreateBlog("title", "username", "password", _hostName, "Blog2");
			Config.CreateBlog("title", "username", "password", _hostName, "Blog3");
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no application specified.
		/// </summary>
		[Test]
		[Rollback(typeof(BlogDuplicationException))]
		//[ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndNoApplication()
		{
			Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
			Config.CreateBlog("title", "username2", "password2", _hostName, string.Empty);
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate application and host 
		/// as another blog.
		/// </summary>
		[Test]
		[Rollback(typeof(BlogDuplicationException))]
		//[ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndApplication()
		{
			Config.CreateBlog("title", "username", "password", _hostName, "MyBlog");
			Config.CreateBlog("title", "username2", "password2", _hostName, "MyBlog");
		}

		/// <summary>
		/// Ensures that one cannot update a blog to have a duplicate application and host 
		/// as another blog.
		/// </summary>
		[Test]
		[Rollback(typeof(BlogDuplicationException))]
		//[ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHostAndApplication()
		{
			string secondHost = UnitTestHelper.GenerateUniqueHost();
			Config.CreateBlog("title", "username", "password", _hostName, "MyBlog");
			Config.CreateBlog("title", "username2", "password2", secondHost, "MyBlog");
			BlogInfo info = Config.GetBlogInfo(secondHost, "MyBlog");
			info.Host = _hostName;
			
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Ensures that one update a blog to have a duplicate host 
		/// as another blog when both have no application specified.
		/// </summary>
		[Test]
		[Rollback(typeof(BlogDuplicationException))]
		//[ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHost()
		{
			string anotherHost = UnitTestHelper.GenerateUniqueHost();
			Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
			Config.CreateBlog("title", "username2", "password2", anotherHost, string.Empty);
			BlogInfo info = Config.GetBlogInfo(anotherHost, string.Empty);
			info.Host = _hostName;
			
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
		[Test]
		[Rollback(typeof(BlogHiddenException))]
		//[ExpectedException(typeof(BlogHiddenException))]
		public void CreateBlogCannotHideAnotherBlog()
		{
			Config.CreateBlog("title", "username", "password", "www." + _hostName, string.Empty);
			Config.CreateBlog("title", "username", "password", _hostName, "MyBlog");
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
		[Rollback]
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
		[Test]
		[Rollback(typeof(BlogRequiresApplicationException))]
		//[ExpectedException(typeof(BlogRequiresApplicationException))]
		public void UpdatingBlogWithDuplicateHostNameRequiresApplicationName()
		{
			string anotherHost = UnitTestHelper.GenerateUniqueHost();
			Config.CreateBlog("title", "username", "password", _hostName, "MyBlog1");
			Config.CreateBlog("title", "username", "password", anotherHost, string.Empty);

			BlogInfo info = Config.GetBlogInfo(anotherHost, string.Empty);
			info.Host = _hostName;
			info.Application = string.Empty;
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// This really tests that looking for duplicates doesn't 
		/// include the blog being edited.
		/// </summary>
		[Test]
		[Rollback]
		public void UpdatingBlogIsFine()
		{
			Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
			BlogInfo info = Config.GetBlogInfo(_hostName.ToUpper(CultureInfo.InvariantCulture), string.Empty);
			info.Author = "Phil";
			Assert.IsTrue(Config.UpdateConfigData(info), "Updating blog config should return true.");
		}

		/// <summary>
		/// Makes sure that every invalid character is checked 
		/// within the application name.
		/// </summary>
		[Test]
		[Rollback]
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
		[Test, Rollback(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameBin()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "bin");
		}

		/// <summary>
		/// Tests that modifying a blog with a reserved keyword (bin) is not allowed.
		/// </summary>
		[Test]
		//[ExpectedException(typeof(InvalidApplicationNameException))]
		[Rollback(typeof(InvalidApplicationNameException))]
		public void CannotRenameBlogToHaveApplicationNameBin()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "Anything");
			BlogInfo info = Config.GetBlogInfo(_hostName, "Anything");
			info.Application = "bin";

			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Tests that creating a blog with a reserved keyword (archive) is not allowed.
		/// </summary>
		[Test]
		//[ExpectedException(typeof(InvalidApplicationNameException))]
		[Rollback(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameArchive()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "archive");
			BlogInfo info = Config.GetBlogInfo(_hostName, "archive");
			info.Application = "archive";

			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Tests that creating a blog that ends with . is not allowed
		/// </summary>
		[Test]
		//[ExpectedException(typeof(InvalidApplicationNameException))]
		[Rollback(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameEndingWithDot()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "archive.");
		}

		/// <summary>
		/// Tests that creating a blog that starts with . is not allowed
		/// </summary>
		[Test]
		//[ExpectedException(typeof(InvalidApplicationNameException))]
		[Rollback(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameStartingWithDot()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, ".archive");
		}

		/// <summary>
		/// Tests that creating a blog that contains invalid characters is not allowed.
		/// </summary>
		[Test]
		//[ExpectedException(typeof(InvalidApplicationNameException))]
		[Rollback(typeof(InvalidApplicationNameException))]
		public void CannotCreateBlogWithApplicationNameWithInvalidCharacters()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "My!Blog");
		}
		#endregion

		/// <summary>
		/// Sets the up test fixture.  This is called once for 
		/// this test fixture before all the tests run.  It 
		/// essentially copies the App.config file to the 
		/// run directory.
		/// </summary>
		[TestFixtureSetUp]
		public void SetUpTestFixture()
		{
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}
		
		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateUniqueHost();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog");
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
