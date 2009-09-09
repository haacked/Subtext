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
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Exceptions;

namespace UnitTests.Subtext.InstallationTests
{
    /// <summary>
    /// Tests of the InstallationManager class.
    /// </summary>
    [TestFixture]
    public class InstallationManagerTests
    {
        [Test]
        public void IsInstallationActionRequired_WithProviderReturningInstallRequired_ReturnsTrue()
        {
            //arrange
            var installProvider = new Mock<Installation>();
            installProvider.Setup(p => p.GetInstallationStatus(It.IsAny<Version>())).Returns(
                InstallationState.NeedsInstallation);
            var manager = new InstallationManager(installProvider.Object);

            //act
            bool result = manager.InstallationActionRequired(new Version());

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsInstallationActionRequired_WithProviderReturningComplete_ReturnsFalse()
        {
            //arrange
            var installProvider = new Mock<Installation>();
            installProvider.Setup(p => p.GetInstallationStatus(It.IsAny<Version>())).Returns(InstallationState.Complete);
            var installManager = new InstallationManager(installProvider.Object);

            //act
            bool result = installManager.InstallationActionRequired(new Version());

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsInstallationActionRequired_WithHostDataDoesNotExistException_ReturnsTrue()
        {
            //arrange
            var installProvider = new Mock<Installation>();
            var installManager = new InstallationManager(installProvider.Object);

            //act
            bool result = installManager.InstallationActionRequired(new Version(), new HostDataDoesNotExistException());

            //assert
            Assert.IsTrue(result);
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