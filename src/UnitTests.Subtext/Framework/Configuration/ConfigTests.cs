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

using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Configuration
{
    /// <summary>
    /// These are unit tests specifically for the Config class.
    /// </summary>
    [TestFixture]
    public class ConfigTests
    {
        string hostName;

        /// <summary>
        /// Make sure we can correctly find a blog based on it's HostName and
        /// subfolder when the system has multiple blogs with the same Host.
        /// </summary>
        [Test]
        [RollBack2]
        public void GetBlogInfoFindsBlogWithUniqueHostAndSubfolder()
        {
            string subfolder1 = UnitTestHelper.GenerateUniqueString();
            string subfolder2 = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("title", "username", "password", hostName, subfolder1);
            Config.CreateBlog("title", "username", "password", hostName, subfolder2);

            Blog info = Config.GetBlog(hostName, subfolder1);
            Assert.IsNotNull(info, "Could not find the blog with the unique hostName & subfolder combination.");
            Assert.AreEqual(info.Subfolder, subfolder1, "Oops! Looks like we found the wrong Blog!");

            info = Config.GetBlog(hostName, subfolder2);
            Assert.IsNotNull(info, "Could not find the blog with the unique hostName & subfolder combination.");
            Assert.AreEqual(info.Subfolder, subfolder2, "Oops! Looks like we found the wrong Blog!");
        }

        [Test]
        [RollBack2]
        public void GetBlogInfoDoesNotFindBlogWithWrongSubfolderInMultiBlogSystem()
        {
            string subfolder1 = UnitTestHelper.GenerateUniqueString();
            string subfolder2 = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("title", "username", "password", hostName, subfolder1);
            Config.CreateBlog("title", "username", "password", hostName, subfolder2);

            Blog info = Config.GetBlog(hostName, string.Empty);
            Assert.IsNull(info, "Hmm... Looks like found a blog using too generic of search criteria.");
        }


        [Test]
        [RollBack2]
        public void SettingShowEmailAddressInRssFlagDoesntChangeOtherFlags()
        {
            Config.CreateBlog("title", "username", "password", hostName, string.Empty);
            Blog info = Config.GetBlog(hostName, string.Empty);
            bool test = info.IsAggregated;
            info.ShowEmailAddressInRss = false;
            ObjectProvider.Instance().UpdateConfigData(info);
            info = Config.GetBlog(hostName, string.Empty);

            Assert.AreEqual(test, info.IsAggregated);
        }


        [Test]
        [RollBack2]
        public void GetBlogInfoLoadsOpenIDSettings()
        {
            Config.CreateBlog("title", "username", "password", hostName, string.Empty);

            Blog info = Config.GetBlog(hostName, string.Empty);
            info.OpenIdServer = "http://server.example.com/";
            info.OpenIdDelegate = "http://delegate.example.com/";
            ObjectProvider.Instance().UpdateConfigData(info);
            info = Config.GetBlog(hostName, string.Empty);

            Assert.AreEqual("http://server.example.com/", info.OpenIdServer);
            Assert.AreEqual("http://delegate.example.com/", info.OpenIdDelegate);
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
            hostName = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "MyBlog");
        }
    }
}