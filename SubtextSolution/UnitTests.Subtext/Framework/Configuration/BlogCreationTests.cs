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

using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Security;
using System;

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
		[RollBack]
		public void CreatingBlogHashesPassword()
		{
			string password = "MyPassword";
			string hashedPassword = SecurityHelper.HashPassword(password);
            
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
		[RollBack]
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
		/// If a blog already exists with a domain name and subfolder, one 
		/// cannot create a blog with the same domain name and no subfolder.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(BlogRequiresSubfolderException))]
		public void CreatingBlogWithDuplicateHostNameRequiresSubfolderName()
		{
			Config.CreateBlog("", "username", "password", _hostName, "MyBlog1");
			Config.CreateBlog("", "username", "password", _hostName, string.Empty);
		}

		/// <summary>
		/// Make sure adding two distinct blogs doesn't raise an exception.
		/// </summary>
		[Test]
		[RollBack]
		public void AddingDistinctBlogsIsFine()
		{
			Config.CreateBlog("title", "username", "password", UnitTestHelper.GenerateRandomString(), string.Empty);
			Config.CreateBlog("title", "username", "password", "www2." + UnitTestHelper.GenerateRandomString(), string.Empty);
			Config.CreateBlog("title", "username", "password", UnitTestHelper.GenerateRandomString(), string.Empty);
			Config.CreateBlog("title", "username", "password", _hostName, "Blog1");
			Config.CreateBlog("title", "username", "password", _hostName, "Blog2");
			Config.CreateBlog("title", "username", "password", _hostName, "Blog3");
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no subfolder specified.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndNoSubfolder()
		{
			Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
			Config.CreateBlog("title", "username2", "password2", _hostName, string.Empty);
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no subfolder specified.
		/// </summary>
		[Test]
		[RollBack]
		[Ignore("Need to fix the blog alias conflict detection code")]
		public void CreateBlogCannotCreateBlogWithHostThatIsDuplicateOfAnotherBlogAlias()
		{
			Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
			BlogAlias alias = new BlogAlias();
			alias.Host = "example.com";
			alias.IsActive = true;
			alias.BlogId = Config.GetBlogInfo(_hostName, string.Empty).Id;
			Config.AddBlogAlias(alias);

            try
            {
                Config.CreateBlog("title", "username2", "password2", "example.com", string.Empty);
            }
            catch (BlogDuplicationException)
            {
                return;
            }
            catch (Exception e)
            {
                Assert.Fail("Expected BlogDuplicationException but got " + e);
            }
            Assert.Fail("Expected BlogDuplicationException");
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no subfolder specified.
		/// </summary>
		[Test]
		[RollBack]
		[Ignore("Need to fix the blog alias conflict detection code")]
		public void CreateBlogCannotAddAliasThatIsDuplicateOfAnotherBlog()
		{
			Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
			Config.CreateBlog("title", "username2", "password2", "example.com", string.Empty);
			
			BlogAlias alias = new BlogAlias();
			alias.Host = "example.com";
			alias.IsActive = true;
			alias.BlogId = Config.GetBlogInfo(_hostName, string.Empty).Id;
            try
            {
                Config.AddBlogAlias(alias);

            }
            catch (BlogDuplicationException)
            {
                return;
            }
            catch (Exception e)
            {
                Assert.Fail("Expected BlogDuplicationException but got " + e);
            }
            Assert.Fail("Expected BlogDuplicationException, but got nothing");
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate subfolder and host 
		/// as another blog.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndSubfolder()
		{
			Config.CreateBlog("title", "username", "password", _hostName, "MyBlog");
			Config.CreateBlog("title", "username2", "password2", _hostName, "MyBlog");
		}

		/// <summary>
		/// Ensures that one cannot update a blog to have a duplicate subfolder and host 
		/// as another blog.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHostAndSubfolder()
		{
			string secondHost = UnitTestHelper.GenerateRandomString();
			Config.CreateBlog("title", "username", "password", _hostName, "MyBlog");
			Config.CreateBlog("title", "username2", "password2", secondHost, "MyBlog");
			BlogInfo info = Config.GetBlogInfo(secondHost, "MyBlog");
			info.Host = _hostName;
			
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Ensures that one update a blog to have a duplicate host 
		/// as another blog when both have no subfolder specified.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHost()
		{
			string anotherHost = UnitTestHelper.GenerateRandomString();
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
		/// but the original blog does not have an subfolder name defined.</p>  
		/// <p>For example, if there exists a blog with the host "www.example.com" with no 
		/// subfolder defined, and the admin adds another blog with the host "www.example.com" 
		/// and subfolder as "MyBlog", this creates a multiple blog situation in the example.com 
		/// domain.  In that situation, each example.com blog MUST have an subfolder name defined. 
		/// The URL to the example.com with no subfolder becomes the aggregate blog.
		/// </p>
		/// </remarks>
		[Test]
		[RollBack]
		[ExpectedException(typeof(BlogHiddenException))]
		public void CreateBlogCannotHideAnotherBlog()
		{
			Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
			Config.CreateBlog("title", "username", "password", _hostName, "MyBlog");
		}

		/// <summary>
		/// Ensures that updating a blog cannot "hide" another blog. Read the 
		/// remarks for more details.
		/// </summary>
		/// <remarks>
		/// <p>This exception occurs when adding a blog with the same hostname as another blog, 
		/// but the original blog does not have an subfolder name defined.</p>  
		/// <p>For example, if there exists a blog with the host "www.example.com" with no 
		/// subfolder defined, and the admin adds another blog with the host "www.example.com" 
		/// and subfolder as "MyBlog", this creates a multiple blog situation in the example.com 
		/// domain.  In that situation, each example.com blog MUST have an subfolder name defined. 
		/// The URL to the example.com with no subfolder becomes the aggregate blog.
		/// </p>
		/// </remarks>
		[Test]
		[RollBack]
		public void UpdatingBlogCannotHideAnotherBlog()
		{
			Config.CreateBlog("title", "username", "password", "www.mydomain.com", string.Empty);
			
			BlogInfo info = Config.GetBlogInfo("www.mydomain.com", string.Empty);
			info.Host = "mydomain.com";
			info.Subfolder = "MyBlog";
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// If a blog already exists with a domain name and subfolder, one 
		/// cannot modify another blog to have the same domain name, but with no subfolder.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(BlogRequiresSubfolderException))]
		public void UpdatingBlogWithDuplicateHostNameRequiresSubfolderName()
		{
			string anotherHost = UnitTestHelper.GenerateRandomString();
			Config.CreateBlog("title", "username", "password", _hostName, "MyBlog1");
			Config.CreateBlog("title", "username", "password", anotherHost, string.Empty);

			BlogInfo info = Config.GetBlogInfo(anotherHost, string.Empty);
			info.Host = _hostName;
			info.Subfolder = string.Empty;
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// This really tests that looking for duplicates doesn't 
		/// include the blog being edited.
		/// </summary>
		[Test]
		[RollBack]
		public void UpdatingBlogIsFine()
		{
			Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
			BlogInfo info = Config.GetBlogInfo(_hostName.ToUpper(CultureInfo.InvariantCulture), string.Empty);
			info.Author = "Phil";
            Config.UpdateConfigData(info); //Make sure no exception is thrown.
		}

        [Test]
        [RollBack]
        public void CanUpdateMobileSkin()
        {
            Config.CreateBlog("title", "username", "password", _hostName, string.Empty);
            BlogInfo info = Config.GetBlogInfo(_hostName.ToUpper(CultureInfo.InvariantCulture), string.Empty);
            info.MobileSkin = new SkinConfig();
            info.MobileSkin.TemplateFolder = "Mobile";
            info.MobileSkin.SkinStyleSheet = "Mobile.css";
            Config.UpdateConfigData(info);
            BlogInfo blog = BlogInfo.GetBlogById(info.Id);
            Assert.AreEqual("Mobile", blog.MobileSkin.TemplateFolder);
            Assert.AreEqual("Mobile.css", blog.MobileSkin.SkinStyleSheet);
        }

		/// <summary>
		/// Makes sure that every invalid character is checked 
		/// within the subfolder name.
		/// </summary>
		[Test]
		[RollBack]
		public void EnsureInvalidCharactersMayNotBeUsedInSubfolderName()
		{
			string[] badNames = {"a{b", "a}b", "a[e", "a]e", "a/e",@"a\e", "a@e", "a!e", "a#e", "a$e", "a'e", "a%", ":e", "a^", "ae&", "*ae", "a(e", "a)e", "a?e", "+a", "e|", "a\"", "e=", "a'", "e<", "a>e", "a;", ",e", "a e"};
			foreach(string badName in badNames)
			{
				Assert.IsFalse(Config.IsValidSubfolderName(badName), badName + " is not a valid app name.");
			}
		}

		/// <summary>
		/// Makes sure that every invalid character is checked 
		/// within the subfolder name.
		/// </summary>
		[Test]
		[RollBack]
		public void ReservedSubtextWordsAreNotValidForSubfolders()
		{
            string[] badSubfolders = { "name.", "tags", "Admin", "bin", "ExternalDependencies", "HostAdmin", "Images", "Install", "Properties", "Providers", "Scripts", "Skins", "SystemMessages", "UI", "Modules", "Services", "Category", "Archive", "Archives", "Comments", "Articles", "Posts", "Story", "Stories", "Gallery", "aggbug", "Sitemap" };
			foreach (string subfolderCandidate in badSubfolders)
			{
				Assert.IsFalse(Config.IsValidSubfolderName(subfolderCandidate), subfolderCandidate + " is not a valid app name.");
			}
		}

		#region Invalid Subfolder Name Tests... There's a bunch...
		/// <summary>
		/// Tests that creating a blog with a reserved keyword (bin) is not allowed.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(InvalidSubfolderNameException))]
		public void CannotCreateBlogWithSubfolderNameBin()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "bin");
		}

		/// <summary>
		/// Tests that modifying a blog with a reserved keyword (bin) is not allowed.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(InvalidSubfolderNameException))]
		public void CannotRenameBlogToHaveSubfolderNameBin()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "Anything");
			BlogInfo info = Config.GetBlogInfo(_hostName, "Anything");
			info.Subfolder = "bin";

			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Tests that creating a blog with a reserved keyword (archive) is not allowed.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(InvalidSubfolderNameException))]
		public void CannotCreateBlogWithSubfolderNameArchive()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "archive");
			BlogInfo info = Config.GetBlogInfo(_hostName, "archive");
			info.Subfolder = "archive";

			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Tests that creating a blog that ends with . is not allowed
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(InvalidSubfolderNameException))]
		public void CannotCreateBlogWithSubfolderNameEndingWithDot()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "archive.");
		}

		/// <summary>
		/// Tests that creating a blog that contains invalid characters is not allowed.
		/// </summary>
		[Test]
		[RollBack]
		[ExpectedException(typeof(InvalidSubfolderNameException))]
		public void CannotCreateBlogWithSubfolderNameWithInvalidCharacters()
		{
			Config.CreateBlog("title", "blah", "blah", _hostName, "My!Blog");
		}
		#endregion

		/// <summary>
		/// Sets the up test fixture.  This is called once for 
		/// this test fixture before all the tests run.
		/// </summary>
		[TestFixtureSetUp]
		public void SetUpTestFixture()
		{
			//Confirm app settings
            UnitTestHelper.AssertAppSettings();
		}
		
		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateRandomString();
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog");
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
