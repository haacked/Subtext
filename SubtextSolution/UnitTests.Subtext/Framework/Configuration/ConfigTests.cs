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
            string anotherHost = UnitTestHelper.GenerateUniqueString();
            string subfolder = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("title", "username", "password", hostName, subfolder);
            Config.CreateBlog("title", "username", "password", anotherHost, string.Empty);

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
            string subfolder1 = UnitTestHelper.GenerateUniqueString();
            string subfolder2 = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("title", "username", "password", hostName, subfolder1);
            Config.CreateBlog("title", "username", "password", hostName, subfolder2);

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
            string subfolder1 = UnitTestHelper.GenerateUniqueString();
            string subfolder2 = UnitTestHelper.GenerateUniqueString();
            Config.CreateBlog("title", "username", "password", hostName, subfolder1);
            Config.CreateBlog("title", "username", "password", hostName, subfolder2);

            BlogInfo info = Config.GetBlogInfo(hostName, string.Empty);
            Assert.IsNull(info, "Hmm... Looks like found a blog using too generic of search criteria.");
        }

        [Test]
        [RollBack2]
        public void GetBlogInfoLoadsOpenIDSettings()
        {
            Config.CreateBlog("title", "username", "password", hostName, string.Empty);

            BlogInfo info = Config.GetBlogInfo(hostName, string.Empty);
            info.OpenIDServer = "http://server.example.com/";
            info.OpenIDDelegate = "http://delegate.example.com/";
            Config.UpdateConfigData(info);
            info = Config.GetBlogInfo(hostName, string.Empty);

            Assert.AreEqual("http://server.example.com/", info.OpenIDServer);
            Assert.AreEqual("http://delegate.example.com/", info.OpenIDDelegate);
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
