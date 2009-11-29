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

using System;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.InstallationTests
{
    /// <summary>
    /// Tests of the InstallationManager class.
    /// </summary>
    [TestFixture]
    public class InstallationManagerTests
    {
        [Test]
        public void IsInstallationActionRequired_WithInstallerReturningNull_ReturnsTrue()
        {
            //arrange
            var installer = new Mock<IInstaller>();
            installer.Setup(i => i.GetCurrentInstallationVersion()).Returns((Version)null);
            var cache = new TestCache();
            cache["NeedsInstallation"] = null;
            var manager = new InstallationManager(installer.Object, cache);

            //act
            bool result = manager.InstallationActionRequired(new Version(), null);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsInstallationActionRequired_WithCachedInstallationStatusOfNeedsInstallation_ReturnsTrue()
        {
            //arrange
            var installer = new Mock<IInstaller>();
            installer.Setup(i => i.GetCurrentInstallationVersion()).Throws(new InvalidOperationException());
            var cache = new TestCache();
            cache["NeedsInstallation"] = InstallationState.NeedsInstallation;
            var manager = new InstallationManager(installer.Object, cache);

            //act
            bool result = manager.InstallationActionRequired(new Version(), null);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsInstallationActionRequired_WithInstallerReturningSameVersionAsAssembly_ReturnsFalse()
        {
            //arrange
            var installer = new Mock<IInstaller>();
            installer.Setup(i => i.GetCurrentInstallationVersion()).Returns(new Version(1, 0, 0, 0));
            var installManager = new InstallationManager(installer.Object, new TestCache());

            //act
            bool result = installManager.InstallationActionRequired(new Version(1, 0, 0, 0), null);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsInstallationActionRequired_WithHostDataDoesNotExistException_ReturnsTrue()
        {
            //arrange
            var installer = new Mock<IInstaller>();
            var installManager = new InstallationManager(installer.Object, new TestCache());

            //act
            bool result = installManager.InstallationActionRequired(new Version(), new HostDataDoesNotExistException());

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Install_ResetsInstallationStatusCache()
        {
            // arrange
            var cache = new TestCache();
            cache["NeedsInstallation"] = InstallationState.NeedsInstallation;
            var installer = new Mock<IInstaller>();
            installer.Setup(i => i.Install(It.IsAny<Version>()));
            var installManager = new InstallationManager(installer.Object, cache);

            // act
            installManager.Install(new Version());

            // assert
            Assert.IsNull(cache["NeedsInstallation"]);
        }

        [Test]
        public void Upgrade_ResetsInstallationStatusCache()
        {
            // arrange
            var cache = new TestCache();
            cache["NeedsInstallation"] = InstallationState.NeedsInstallation;
            var installer = new Mock<IInstaller>();
            installer.Setup(i => i.Upgrade(It.IsAny<Version>()));
            var installManager = new InstallationManager(installer.Object, cache);

            // act
            installManager.Upgrade(new Version());

            // assert
            Assert.IsNull(cache["NeedsInstallation"]);
        }

        [Test]
        public void ResetInstallationStatusCache_WithApplicationNeedingInstallation_SetsStatusToNull()
        {
            // arrange
            var cache = new TestCache();
            cache["NeedsInstallation"] = InstallationState.NeedsInstallation;
            var installManager = new InstallationManager(null, cache);

            // act
            installManager.ResetInstallationStatusCache();

            // assert
            Assert.IsNull(cache["NeedsInstallation"]);
        }

        [Test]
        public void CreateWelcomeContent_CreatesIntroBlogPostAndCategories()
        {
            // arrange
            var installationManager = new InstallationManager(new Mock<IInstaller>().Object, null);
            var repository = new Mock<ObjectProvider>();
            var entryPublisher = new Mock<IEntryPublisher>();
            Entry entry = null;
            entryPublisher.Setup(p => p.Publish(It.Is<Entry>(e => e.PostType == PostType.BlogPost))).Callback<Entry>(e => entry = e);
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.AdminUrl("")).Returns("/admin/default.aspx");
            urlHelper.Setup(u => u.EntryUrl(It.Is<Entry>(e => e.PostType == PostType.Story))).Returns<Entry>(e => "/articles/" + e.EntryName + ".aspx");
            urlHelper.Setup(u => u.HostAdminUrl("default.aspx")).Returns("/hostadmin/default.aspx");
            var context = new Mock<ISubtextContext>();
            context.SetupUrlHelper(urlHelper);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var blog = new Blog {Id = 123, Author = "TestAuthor"};

            // act
            installationManager.CreateWelcomeContent(context.Object, entryPublisher.Object, blog);

            // assert
            Assert.AreEqual(entry.Title, "Welcome to Subtext!");
            Assert.AreEqual(entry.EntryName, "welcome-to-subtext");
            Assert.Contains(entry.Body, @"<a href=""/admin/default.aspx");
            Assert.Contains(entry.Body, @"<a href=""/articles/welcome-to-subtext-article.aspx");
            Assert.Contains(entry.Body, @"<a href=""/hostadmin/default.aspx");
            Assert.IsTrue(entry.AllowComments);
            Assert.IsTrue(!entry.Body.Contains(@"<a href=""{0}"));
            Assert.IsTrue(!entry.Body.Contains(@"<a href=""{1}"));
            Assert.IsTrue(!entry.Body.Contains(@"<a href=""{2}"));
        }

        [Test]
        public void CreateWelcomeContent_CreatesIntroArticle()
        {
            // arrange
            var installationManager = new InstallationManager(new Mock<IInstaller>().Object, null);
            var repository = new Mock<ObjectProvider>();
            var entryPublisher = new Mock<IEntryPublisher>();
            Entry article = null;
            entryPublisher.Setup(p => p.Publish(It.Is<Entry>(e => e.PostType == PostType.Story))).Callback<Entry>(e => article = e);
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.AdminUrl("articles")).Returns("/admin/articles/default.aspx");
            var context = new Mock<ISubtextContext>();
            context.SetupUrlHelper(urlHelper);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var blog = new Blog { Id = 123, Author = "TestAuthor" };
            
            // act
            installationManager.CreateWelcomeContent(context.Object, entryPublisher.Object, blog);

            // assert
            Assert.AreEqual(article.Title, "Welcome to Subtext!");
            Assert.Contains(article.Body, @"<a href=""/admin/articles/");
            Assert.IsTrue(!article.Body.Contains(@"<a href=""{0}"));
        }

        [Test]
        public void CreateWelcomeContent_CreatesIntroComment()
        {
            // arrange
            var installationManager = new InstallationManager(new Mock<IInstaller>().Object, null);
            var repository = new Mock<ObjectProvider>();
            var entryPublisher = new Mock<IEntryPublisher>();
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.AdminUrl("feedback")).Returns("/admin/feedback/default.aspx");
            var context = new Mock<ISubtextContext>();
            context.SetupUrlHelper(urlHelper);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var blog = new Blog { Id = 123, Author = "TestAuthor" };
            FeedbackItem comment = null;
            repository.Setup(r => r.Create(It.IsAny<FeedbackItem>())).Callback<FeedbackItem>(c => comment = c);

            // act
            installationManager.CreateWelcomeContent(context.Object, entryPublisher.Object, blog);

            // assert
            Assert.IsTrue(comment.Approved);
            Assert.AreEqual(comment.Title, "re: Welcome to Subtext!");
            Assert.Contains(comment.Body, @"<a href=""/admin/feedback/");
            Assert.IsTrue(!comment.Body.Contains(@"<a href=""{1}"));
        }

        /// <summary>
        /// Called before each unit test.
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //Confirm app settings
            UnitTestHelper.AssertAppSettings();
        }
    }
}