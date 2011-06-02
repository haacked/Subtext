#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Globalization;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Providers;
using Subtext.Framework.Security;

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
        [RollBack2]
        public void CreatingBlogHashesPassword()
        {
            const string password = "MyPassword";
            string hashedPassword = SecurityHelper.HashPassword(password);

            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("", "username", password, _hostName, "MyBlog1");
            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName, "MyBlog1");
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
        [RollBack2]
        public void ModifyingBlogShouldNotChangePassword()
        {
            Config.Settings.UseHashedPasswords = true;
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("", "username", "thePassword", _hostName, "MyBlog1");
            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName.ToUpper(CultureInfo.InvariantCulture), "MyBlog1");
            string password = info.Password;
            info.LicenseUrl = "http://subtextproject.com/";
            ObjectProvider.Instance().UpdateConfigData(info);

            info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName.ToUpper(CultureInfo.InvariantCulture), "MyBlog1");
            Assert.AreEqual(password, info.Password);
        }

        /// <summary>
        /// If a blog already exists with a domain name and subfolder, one 
        /// cannot create a blog with the same domain name and no subfolder.
        /// </summary>
        [Test]
        [RollBack2]
        public void CreatingBlogWithDuplicateHostNameRequiresSubfolderName()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("", "username", "password", _hostName, "MyBlog1");


            UnitTestHelper.AssertThrows<BlogRequiresSubfolderException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("", "username", "password", _hostName, string.Empty));
        }

        /// <summary>
        /// Make sure adding two distinct blogs doesn't raise an exception.
        /// </summary>
        [Test]
        [RollBack2]
        public void CreatingMultipleBlogs_WithDistinctProperties_DoesNotThrowException()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", UnitTestHelper.GenerateUniqueString(), string.Empty);
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", "www2." + UnitTestHelper.GenerateUniqueString(),
                              string.Empty);
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", UnitTestHelper.GenerateUniqueString(), string.Empty);
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, "Blog1");
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, "Blog2");
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, "Blog3");
        }

        /// <summary>
        /// Ensures that one cannot create a blog with a duplicate host 
        /// as another blog when both have no subfolder specified.
        /// </summary>
        [Test]
        [RollBack2]
        public void CreateBlogCannotCreateOneWithDuplicateHostAndNoSubfolder()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, string.Empty);

            UnitTestHelper.AssertThrows<BlogDuplicationException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username2", "password2", _hostName, string.Empty));
        }

        /// <summary>
        /// Ensures that one cannot create a blog with a duplicate host 
        /// as another blog when both have no subfolder specified.
        /// </summary>
        [Test]
        [RollBack2]
        public void CreateBlogCannotCreateBlogWithHostThatIsDuplicateOfAnotherBlogAlias()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, string.Empty);
            var alias = new BlogAlias { Host = "example.com", IsActive = true, BlogId = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName, string.Empty).Id };
            new global::Subtext.Framework.Data.DatabaseObjectProvider().AddBlogAlias(alias);

            UnitTestHelper.AssertThrows<BlogDuplicationException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username2", "password2", "example.com", string.Empty));
        }

        /// <summary>
        /// Ensures that one cannot create a blog with a duplicate host 
        /// as another blog when both have no subfolder specified.
        /// </summary>
        [Test]
        [RollBack2]
        public void CreateBlogCannotAddAliasThatIsDuplicateOfAnotherBlog()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, string.Empty);
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username2", "password2", "example.com", string.Empty);

            var alias = new BlogAlias { Host = "example.com", IsActive = true, BlogId = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName, string.Empty).Id };
            UnitTestHelper.AssertThrows<BlogDuplicationException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().AddBlogAlias(alias));
        }

        /// <summary>
        /// Ensures that one cannot create a blog with a duplicate subfolder and host 
        /// as another blog.
        /// </summary>
        [Test]
        [RollBack2]
        public void CreateBlogCannotCreateOneWithDuplicateHostAndSubfolder()
        {
            var repository = new DatabaseObjectProvider();
            repository.CreateBlog("title", "username", "password", _hostName, "MyBlog");

            UnitTestHelper.AssertThrows<BlogDuplicationException>(() => repository.CreateBlog("title", "username2", "password2", _hostName, "MyBlog"));
        }

        /// <summary>
        /// Ensures that one cannot update a blog to have a duplicate subfolder and host 
        /// as another blog.
        /// </summary>
        [Test]
        [RollBack2]
        public void UpdateBlogCannotConflictWithDuplicateHostAndSubfolder()
        {
            string secondHost = UnitTestHelper.GenerateUniqueString();
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, "MyBlog");
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username2", "password2", secondHost, "MyBlog");
            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(secondHost, "MyBlog");
            info.Host = _hostName;

            UnitTestHelper.AssertThrows<BlogDuplicationException>(() => ObjectProvider.Instance().UpdateConfigData(info));
        }

        /// <summary>
        /// Ensures that one update a blog to have a duplicate host 
        /// as another blog when both have no subfolder specified.
        /// </summary>
        [Test]
        [RollBack2]
        public void UpdateBlogCannotConflictWithDuplicateHost()
        {
            string anotherHost = UnitTestHelper.GenerateUniqueString();
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, string.Empty);
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username2", "password2", anotherHost, string.Empty);
            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(anotherHost, string.Empty);
            info.Host = _hostName;

            UnitTestHelper.AssertThrows<BlogDuplicationException>(() => ObjectProvider.Instance().UpdateConfigData(info));
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
        [RollBack2]
        public void CreateBlogCannotHideAnotherBlog()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, string.Empty);

            UnitTestHelper.AssertThrows<BlogHiddenException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, "MyBlog"));
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
        [RollBack2]
        public void UpdatingBlogCannotHideAnotherBlog()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", "www.mydomain.com", string.Empty);

            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog("www.mydomain.com", string.Empty);
            info.Host = "mydomain.com";
            info.Subfolder = "MyBlog";
            ObjectProvider.Instance().UpdateConfigData(info);
        }

        /// <summary>
        /// If a blog already exists with a domain name and subfolder, one 
        /// cannot modify another blog to have the same domain name, but with no subfolder.
        /// </summary>
        [Test]
        [RollBack2]
        public void UpdatingBlogWithDuplicateHostNameRequiresSubfolderName()
        {
            string anotherHost = UnitTestHelper.GenerateUniqueString();
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, "MyBlog1");
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", anotherHost, string.Empty);

            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(anotherHost, string.Empty);
            info.Host = _hostName;
            info.Subfolder = string.Empty;

            UnitTestHelper.AssertThrows<BlogRequiresSubfolderException>(() => ObjectProvider.Instance().UpdateConfigData(info));
        }

        /// <summary>
        /// This really tests that looking for duplicates doesn't 
        /// include the blog being edited.
        /// </summary>
        [Test]
        [RollBack2]
        public void UpdatingBlogIsFine()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, string.Empty);
            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName.ToUpper(CultureInfo.InvariantCulture), string.Empty);
            info.Author = "Phil";
            ObjectProvider.Instance().UpdateConfigData(info); //Make sure no exception is thrown.
        }

        [Test]
        [RollBack2]
        public void CanUpdateMobileSkin()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "username", "password", _hostName, string.Empty);
            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName.ToUpper(CultureInfo.InvariantCulture), string.Empty);
            info.MobileSkin = new SkinConfig { TemplateFolder = "Mobile", SkinStyleSheet = "Mobile.css" };
            ObjectProvider.Instance().UpdateConfigData(info);
            Blog blog = ObjectProvider.Instance().GetBlogById(info.Id);
            Assert.AreEqual("Mobile", blog.MobileSkin.TemplateFolder);
            Assert.AreEqual("Mobile.css", blog.MobileSkin.SkinStyleSheet);
        }

        /// <summary>
        /// Makes sure that every invalid character is checked 
        /// within the subfolder name.
        /// </summary>
        [Test]
        [RollBack2]
        public void EnsureInvalidCharactersMayNotBeUsedInSubfolderName()
        {
            string[] badNames = {
                                    "a{b", "a}b", "a[e", "a]e", "a/e", @"a\e", "a@e", "a!e", "a#e", "a$e", "a'e", "a%",
                                    ":e", "a^", "ae&", "*ae", "a(e", "a)e", "a?e", "+a", "e|", "a\"", "e=", "a'", "e<",
                                    "a>e", "a;", ",e", "a e"
                                };
            foreach (string badName in badNames)
            {
                Assert.IsFalse(Config.IsValidSubfolderName(badName), badName + " is not a valid app name.");
            }
        }

        /// <summary>
        /// Makes sure that every invalid character is checked 
        /// within the subfolder name.
        /// </summary>
        [Test]
        [RollBack2]
        public void ReservedSubtextWordsAreNotValidForSubfolders()
        {
            string[] badSubfolders = {
                                         "name.", "tags", "Admin", "bin", "ExternalDependencies", "HostAdmin", "Images",
                                         "Install", "Properties", "Providers", "Scripts", "Skins", "SystemMessages", "UI",
                                         "Modules", "Services", "Category", "Archive", "Archives", "Comments", "Articles",
                                         "Posts", "Story", "Stories", "Gallery", "aggbug", "Sitemap"
                                     };
            foreach (string subfolderCandidate in badSubfolders)
            {
                Assert.IsFalse(Config.IsValidSubfolderName(subfolderCandidate),
                               subfolderCandidate + " is not a valid app name.");
            }
        }

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
            _hostName = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "MyBlog");
        }

        [TearDown]
        public void TearDown()
        {
        }

        /// <summary>
        /// Tests that creating a blog with a reserved keyword (bin) is not allowed.
        /// </summary>
        [Test]
        [RollBack2]
        public void CannotCreateBlogWithSubfolderNameBin()
        {
            UnitTestHelper.AssertThrows<InvalidSubfolderNameException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "blah", "blah", _hostName, "bin"));
        }

        /// <summary>
        /// Tests that modifying a blog with a reserved keyword (bin) is not allowed.
        /// </summary>
        [Test]
        [RollBack2]
        public void CannotRenameBlogToHaveSubfolderNameBin()
        {
            new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "blah", "blah", _hostName, "Anything");
            Blog info = new global::Subtext.Framework.Data.DatabaseObjectProvider().GetBlog(_hostName, "Anything");
            info.Subfolder = "bin";

            UnitTestHelper.AssertThrows<InvalidSubfolderNameException>(() => ObjectProvider.Instance().UpdateConfigData(info));
        }

        /// <summary>
        /// Tests that creating a blog with a reserved keyword (archive) is not allowed.
        /// </summary>
        [Test]
        [RollBack2]
        public void CannotCreateBlogWithSubfolderNameArchive()
        {
            UnitTestHelper.AssertThrows<InvalidSubfolderNameException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "blah", "blah", _hostName, "archive"));
        }

        /// <summary>
        /// Tests that creating a blog that ends with . is not allowed
        /// </summary>
        [Test]
        [RollBack2]
        public void CannotCreateBlogWithSubfolderNameEndingWithDot()
        {
            UnitTestHelper.AssertThrows<InvalidSubfolderNameException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "blah", "blah", _hostName, "archive."));
        }

        /// <summary>
        /// Tests that creating a blog that contains invalid characters is not allowed.
        /// </summary>
        [Test]
        [RollBack2]
        public void CannotCreateBlogWithSubfolderNameWithInvalidCharacters()
        {
            UnitTestHelper.AssertThrows<InvalidSubfolderNameException>(() => new global::Subtext.Framework.Data.DatabaseObjectProvider().CreateBlog("title", "blah", "blah", _hostName, "My!Blog"));
        }
    }
}