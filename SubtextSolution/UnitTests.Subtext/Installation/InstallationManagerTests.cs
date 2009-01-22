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
using Subtext.Framework.Exceptions;
using Moq;
using Subtext.Extensibility.Providers;

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
            installProvider.Setup(p => p.GetInstallationStatus(It.IsAny<Version>())).Returns(InstallationState.NeedsInstallation);
            InstallationManager manager = new InstallationManager(installProvider.Object);

            //act
            bool result = manager.IsInstallationActionRequired(new Version());

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsInstallationActionRequired_WithProviderReturningComplete_ReturnsFalse()
        {
            //arrange
            var installProvider = new Mock<Installation>();
            installProvider.Setup(p => p.GetInstallationStatus(It.IsAny<Version>())).Returns(InstallationState.Complete);
            InstallationManager installManager = new InstallationManager(installProvider.Object);

            //act
            bool result = installManager.IsInstallationActionRequired(new Version());

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsInstallationActionRequired_WithBlogDoesNotExistException_ReturnsTrue()
        {
            //arrange
            var installProvider = new Mock<Installation>();
            InstallationManager installManager = new InstallationManager(installProvider.Object);

            //act
            bool result = installManager.InstallationActionRequired(new BlogDoesNotExistException(123), new Version());

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsInstallationActionRequired_WithHostDataDoesNotExistException_ReturnsTrue()
        {
            //arrange
            var installProvider = new Mock<Installation>();
            InstallationManager installManager = new InstallationManager(installProvider.Object);

            //act
            bool result = installManager.InstallationActionRequired(new HostDataDoesNotExistException(), new Version());

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
