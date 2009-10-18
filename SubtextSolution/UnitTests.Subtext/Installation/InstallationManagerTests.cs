#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Infrastructure.Installation;

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