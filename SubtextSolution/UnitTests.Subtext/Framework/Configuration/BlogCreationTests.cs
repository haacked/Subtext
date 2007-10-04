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

using System;
using System.Globalization;
using System.Web.Security;
using MbUnit.Framework;
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
		private string _hostName;

		/// <summary>
		/// make sure we cannot pass in null for any of the arguments.
		/// </summary>
		[RowTest]
		[Row("Title", "localhost0", null)]
		[Row("Title", "localhost1", "")]
		[Row("Title", "localhost2", "subfolder")]
		[Row("Title", null, null, ExpectedException = typeof(ArgumentNullException))]
		[Row(null, "Host", null, ExpectedException = typeof(ArgumentNullException))]
		[RollBack2]
		public void CreateBlogWithOwnerTest(string title, string host, string subfolder)
		{
			MembershipUser owner = Membership.CreateUser(UnitTestHelper.MembershipTestUsername, "test", UnitTestHelper.MembershipTestEmail);
			Assert.IsNotNull(owner, "Test is pointless without a valid user");
			Config.CreateBlog(title, host, subfolder, owner);
		}

		/// <summary>
		/// If a blog already exists with a domain name and subfolder, one 
		/// cannot create a blog with the same domain name and no subfolder.
		/// </summary>
		[Test]
		[RollBack2]
		[ExpectedException(typeof(BlogRequiresSubfolderException))]
		public void CreatingBlogWithDuplicateHostNameRequiresSubfolderName()
		{
			string host = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.CreateBlog("", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, "MyBlog1");
			UnitTestHelper.CreateBlog("", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, string.Empty);
		}

		/// <summary>
		/// Make sure adding two distinct blogs doesn't raise an exception.
		/// </summary>
		[Test]
		[RollBack2]
		public void AddingDistinctBlogsIsFine()
		{
			string host = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password1", UnitTestHelper.MembershipTestUsername, string.Empty);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password1", "www0." + UnitTestHelper.MembershipTestUsername, string.Empty);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password1", UnitTestHelper.MembershipTestUsername, string.Empty);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password1", host, "Blog1");
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password1", host, "Blog2");
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password1", host, "Blog3");
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no subfolder specified.
		/// </summary>
		[Test]
		[RollBack2]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndNoSubfolder()
		{
			string host = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, string.Empty);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password2", host, string.Empty);
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no subfolder specified.
		/// </summary>
		[Test]
		[RollBack2]
		[Ignore("Need to fix the blog alias conflict detection code")]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateBlogWithHostThatIsDuplicateOfAnotherBlogAlias()
		{
			UnitTestHelper.SetupBlog();
			BlogAlias alias = new BlogAlias();
			alias.Host = "example.com";
			alias.IsActive = true;
			alias.BlogId = Config.GetBlogInfo(_hostName, string.Empty).Id;
			Config.AddBlogAlias(alias);

			Config.CreateBlog("title", "example.com", string.Empty, Config.CurrentBlog.Owner);
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate host 
		/// as another blog when both have no subfolder specified.
		/// </summary>
		[Test]
		[RollBack]
		[Ignore("Need to fix the blog alias conflict detection code")]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotAddAliasThatIsDuplicateOfAnotherBlog()
		{
			UnitTestHelper.SetupBlog();
			Config.CreateBlog("title", _hostName, string.Empty, Config.CurrentBlog.Owner);
			Config.CreateBlog("title", "example.com", string.Empty, Config.CurrentBlog.Owner);
			
			BlogAlias alias = new BlogAlias();
			alias.Host = "example.com";
			alias.IsActive = true;
			alias.BlogId = Config.GetBlogInfo(_hostName, string.Empty).Id;
			Config.AddBlogAlias(alias);
		}

		/// <summary>
		/// Ensures that one cannot create a blog with a duplicate subfolder and host 
		/// as another blog.
		/// </summary>
		[Test]
		[RollBack2]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void CreateBlogCannotCreateOneWithDuplicateHostAndSubfolder()
		{
			string host = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, "MyBlog");
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password2", host, "MyBlog");
		}

		/// <summary>
		/// Ensures that one cannot update a blog to have a duplicate subfolder and host 
		/// as another blog.
		/// </summary>
		[Test]
		[RollBack2]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHostAndSubfolder()
		{
			string host = UnitTestHelper.MembershipTestUsername;
			
			string secondHost = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, "MyBlog");
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password2", secondHost, "MyBlog");
			BlogInfo info = Config.GetBlogInfo(secondHost, "MyBlog");
			info.Host = host;
			
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Ensures that one update a blog to have a duplicate host 
		/// as another blog when both have no subfolder specified.
		/// </summary>
		[Test]
		[RollBack2]
		[ExpectedException(typeof(BlogDuplicationException))]
		public void UpdateBlogCannotConflictWithDuplicateHost()
		{
			string host = UnitTestHelper.MembershipTestUsername;
			
			string anotherHost = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, string.Empty);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password2", anotherHost, string.Empty);
			BlogInfo info = Config.GetBlogInfo(anotherHost, string.Empty);
			info.Host = host;
			
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
		[RollBack2]
		[ExpectedException(typeof(BlogHiddenException))]
		public void CreateBlogCannotHideAnotherBlog()
		{
			string host = UnitTestHelper.MembershipTestUsername;

            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, string.Empty);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, "MyBlog");
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
		[ExpectedException(typeof(BlogHiddenException))]
		public void UpdatingBlogCannotHideAnotherBlog()
		{
			string host1 = UnitTestHelper.MembershipTestUsername;
            string host2 = UnitTestHelper.MembershipTestUsername;
            string subfolder = UnitTestHelper.MembershipTestUsername;

		    // add the first blog
		    UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host1, string.Empty);
		    
		    // now add a 2nd blog, with different Host name, then get the 2nd blog and change the hostname.
			UnitTestHelper.CreateBlog("title2", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host2, subfolder);
			BlogInfo blog2Info = Config.GetBlogInfo(host2, subfolder);
            blog2Info.Host = host1;
		    
			Config.UpdateConfigData(blog2Info);
		}

		/// <summary>
		/// If a blog already exists with a domain name and subfolder, one 
		/// cannot modify another blog to have the same domain name, but with no subfolder.
		/// </summary>
		[Test]
		[RollBack2]
		[ExpectedException(typeof(BlogRequiresSubfolderException))]
		public void UpdatingBlogWithDuplicateHostNameRequiresSubfolderName()
		{
			string host = UnitTestHelper.MembershipTestUsername;
			
			string anotherHost = UnitTestHelper.MembershipTestUsername;
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, "MyBlog1");
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", anotherHost, string.Empty);

			BlogInfo info = Config.GetBlogInfo(anotherHost, string.Empty);
			info.Host = host;
			info.Subfolder = string.Empty;
			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// This really tests that looking for duplicates doesn't 
		/// include the blog being edited.
		/// </summary>
		[Test]
		[RollBack2]
		public void UpdatingBlogIsFine()
		{
			string host = UnitTestHelper.MembershipTestUsername;

            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", host, string.Empty);
			BlogInfo info = Config.GetBlogInfo(host.ToUpper(CultureInfo.InvariantCulture), string.Empty);
			info.News = "This just in";
			Config.UpdateConfigData(info);
			info = Config.GetBlogInfo(host.ToUpper(CultureInfo.InvariantCulture), string.Empty);
			Assert.AreEqual("This just in", info.News);
		}

		/// <summary>
		/// Makes sure that every invalid character is checked 
		/// within the subfolder name.
		/// </summary>
		[Test]
		[RollBack2]
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
		[RollBack2]
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
		/// Tests that modifying a blog with a reserved keyword (bin) is not allowed.
		/// </summary>
		[RowTest]
		[Row("bin", ExpectedException = typeof(InvalidSubfolderNameException))]
		[Row("archive", ExpectedException = typeof(InvalidSubfolderNameException))]
		[Row("archive.", ExpectedException = typeof(InvalidSubfolderNameException))]
		[Row("archive.", ExpectedException = typeof(InvalidSubfolderNameException))]
		[Row("My!Blog", ExpectedException = typeof(InvalidSubfolderNameException))]
		[RollBack2]
		public void CannotRenameBlogToHaveSubfolderNameBin(string badSubfolderName)
		{
			UnitTestHelper.SetupBlog("OkSubfolder");
			BlogInfo info = Config.CurrentBlog;
			info.Subfolder = badSubfolderName;

			Config.UpdateConfigData(info);
		}

		/// <summary>
		/// Tests that creating a blog with a reserved (or invalid) keyword (archive) is not allowed.
		/// </summary>
		[RowTest]
		[Row("bin", ExpectedException = typeof(InvalidSubfolderNameException))]
		[Row("archive", ExpectedException = typeof(InvalidSubfolderNameException))]
		[Row("archive.", ExpectedException = typeof(InvalidSubfolderNameException))]
		[Row("archive.", ExpectedException = typeof(InvalidSubfolderNameException))]
		[Row("My!Blog", ExpectedException = typeof(InvalidSubfolderNameException))]
		[RollBack2]
		public void CannotCreateBlogWithReservedOrInvalidSubfolder(string subfolder)
		{
			UnitTestHelper.SetupBlog(subfolder);
		}
		#endregion

		[TearDown]
		public void TearDown()
		{
		}

		[SetUp]
		public void SetUp()
		{
			_hostName = UnitTestHelper.GenerateRandomString();
		}
	}
}
