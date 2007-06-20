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
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Configuration
{
    /// <summary>
    /// These are unit tests specifically for the Config class.
    /// </summary>
    [TestFixture]
    public class ConfigTests
    {
        string hostName;

        #region GetBlogInfo tests

		[Test]
		[RollBack2]
		public void CurrentBlogReturnsNullWhenContextIsNull()
		{
			HttpContext.Current = null;
			Assert.IsNull(Config.CurrentBlog);
		}

		/// <summary>
		/// When making a request for the Install directory w/zero (0) blogs in the system, 
		/// Config.CurrentBlog SHOULD always return NULL.
		/// </summary>
		/// <remarks>
		/// Do we really need to ensure that there are no blogs in the system? If so, is there a 
		/// way to remove all blogs from the system (using the Rollback2 attribute to revert chagnes 
		/// after the test is done)?
		/// </remarks>
		[Test]
		[RollBack2]
		public void CurrentBlogReturnsNullWhenNoBlogsExistAndInInstallDirectory()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(UnitTestHelper.GenerateRandomString(), "Install", "", "Default.aspx");
			UnitTestHelper.ClearAllBlogData();
			Assert.AreEqual(Config.BlogCount, 0, "This test requires that there be no blogs in the system.");
			Assert.IsNull(Config.CurrentBlog, "Should not have been able to find a BlogInfo object when in the Install directory");
		}

    	/// <summary>
		/// When making a request for the HostAdmin directory, Config.CurrentBlog SHOULD always return NULL
		/// </summary>
		/// <remarks>
		/// Because there is a quirk with the subtext_GetBlog SP where it will ALWAYS return
		/// a record if the system has EXACTLY ONE (1) blog in the system. Let's make sure 
		/// we still return null when in the context of the HostAdmin request.
		/// </remarks>
		[Test]
		[RollBack2]
		public void CurrentBlogReturnNsullWhenInHostAdminDirectory()
		{
			UnitTestHelper.ClearAllBlogData();
			UnitTestHelper.SetupBlog(); //Create one blog.
			UnitTestHelper.SetHttpContextWithBlogRequest(UnitTestHelper.GenerateRandomString(), "HostAdmin", "", "Default.aspx");

			Assert.IsNull(Config.CurrentBlog, "Should not have been able to find a BlogInfo object for a request to the HostAdmin directory.");
		}

    	/// <summary>
        /// If we have two or more blogs in the system we want to be sure that 
        /// we can find a blog if it has a unique HostName in the system, despite 
        /// what it's subfolder is.
        /// </summary>
        /// <remarks>
        /// Basically, we need to be able support the following setup:
        /// Blog1 has a HostName "mydomain.com" and any (or no) subfolder name.
        /// Blog2 has a HostName "example.com" and any (or no) subfolder name.
        /// 
        /// When a request comes in for "http://mydomain.com/" we want to make sure 
        /// that we find Blog1 because it is the ONLY blog in the system with the 
        /// hostName "mydomain.com".
        /// </remarks>
        [Test]
        [RollBack2]
        public void GetBlogInfoFindsBlogWithUniqueHostName()
        {
            string anotherHost = UnitTestHelper.GenerateRandomString();
            string subfolder = UnitTestHelper.GenerateRandomString();
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", hostName, subfolder);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", anotherHost, string.Empty);

            BlogInfo info = Config.GetBlogInfo(hostName, string.Empty);
            Assert.IsNotNull(info, "Could not find the blog with the unique hostName.");
            Assert.AreEqual(info.Host, hostName, "Oops! Looks like we found the wrong Blog!");
            Assert.AreEqual(info.Subfolder, subfolder, "Oops! Looks like we found the wrong Blog!");
        }
        
        /// <summary>
        /// Make sure we can correctly find a blog based on it's HostName and
        /// subfolder when the system has multiple blogs with the same Host.
        /// </summary>
        [Test]
        [RollBack2]
        public void GetBlogInfoFindsBlogWithUniqueHostAndSubfolder()
        {
            string subfolder1 = UnitTestHelper.GenerateRandomString();
            string subfolder2 = UnitTestHelper.GenerateRandomString();
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", hostName, subfolder1);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", hostName, subfolder2);

            BlogInfo info = Config.GetBlogInfo(hostName, subfolder1);
            Assert.IsNotNull(info, "Could not find the blog with the unique hostName & subfolder combination.");
            Assert.AreEqual(info.Subfolder, subfolder1, "Oops! Looks like we found the wrong Blog!");

            info = Config.GetBlogInfo(hostName, subfolder2);
            Assert.IsNotNull(info, "Could not find the blog with the unique hostName & subfolder combination.");
            Assert.AreEqual(info.Subfolder, subfolder2, "Oops! Looks like we found the wrong Blog!");
        }
        
        [Test]
        [RollBack2]
        public void GetBlogInfoDoesNotFindBlogWithWrongSubfolderInMultiBlogSystem()
        {
            string subfolder1 = UnitTestHelper.GenerateRandomString();
            string subfolder2 = UnitTestHelper.GenerateRandomString();
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", hostName, subfolder1);
            UnitTestHelper.CreateBlog("title", UnitTestHelper.MembershipTestUsername, UnitTestHelper.MembershipTestEmail, "password", hostName, subfolder2);

            BlogInfo info = Config.GetBlogInfo(hostName, string.Empty);
            Assert.IsNull(info, "Hmm... Looks like found a blog using too generic of search criteria.");
        }

        #endregion

        #region IsValidSubfolderName tests

        /// <summary>
        /// Makes sure that every invalid character is checked 
        /// within the subfolder name.
        /// </summary>
        [Test]
        public void EnsureInvalidCharactersMayNotBeUsedInSubfolderName()
        {
            string[] badNames = { "name.", "a{b", "a}b", "a[e", "a]e", "a/e", @"a\e", "a@e", "a!e", "a#e", "a$e", "a'e", "a%", ":e", "a^", "ae&", "*ae", "a(e", "a)e", "a?e", "+a", "e|", "a\"", "e=", "a'", "e<", "a>e", "a;", ",e", "a e" };
            foreach (string badName in badNames)
            {
                Assert.IsFalse(Config.IsValidSubfolderName(badName), badName + " is not a valid app name.");
            }
        }

        /// <summary>
        /// Makes sure that every invalid character is checked 
        /// within the subfolder name.
        /// </summary>
        [RowTest]
        [Row("Admin")]
        [Row("bin")]
        [Row("Admin")]
        [Row("bin")]
        [Row("ExternalDependencies")]
        [Row("HostAdmin")]
        [Row("Images")]
        [Row("Install")]
        [Row("Modules")]
        [Row("Services")]
        [Row("Skins")]
        [Row("UI")]
        [Row("Category")]
        [Row("Archive")]
        [Row("Archives")]
        [Row("Comments")]
        [Row("Articles")]
        [Row("Posts")]
        [Row("Story")]
        [Row("Stories")]
        [Row("Gallery")]
        [Row("Providers")]
        [Row("aggbug")]
        [Row(".")]
        [Row("end.")]
        [Row("sub:folder")]
        public void ReservedSubtextWordsAreNotValidForSubfolders(string badSubfolderName)
        {
            Assert.IsFalse(Config.IsValidSubfolderName(badSubfolderName), "'" + badSubfolderName + "' was considered a valid subfolder name, but shouldn't have been.");
        }
        
        [Test]
        [ExpectedArgumentNullException]
        public void NullSubfolderThrowsArgumentNullException()
        {
            Config.IsValidSubfolderName(null);
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

    	private IDisposable blogRequest;
        [SetUp]
        public void SetUp()
        {
            hostName = UnitTestHelper.GenerateRandomString();
        	blogRequest = BlogRequestSimulator.SimulateRequest(hostName, "", "MyBlog");
        }

		[TearDown]
		public void TearDown()
		{
			if(blogRequest != null)
				blogRequest.Dispose();
		}
    }
}
