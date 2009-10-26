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
using System.Collections.ObjectModel;
using MbUnit.Framework;
using Subtext.Framework.Infrastructure.Installation;

namespace UnitTests.Subtext.InstallationTests
{
    /// <summary>
    /// Tests of the <see cref="SqlInstallationProvider"/> class.
    /// </summary>
    [TestFixture]
    public class SqlInstallationProviderTests
    {
        /// <summary>
        /// Tests that we can properly list the installation scripts.
        /// </summary>
        [Test]
        public void ListInstallationScriptsReturnsCorrectScripts()
        {
            ReadOnlyCollection<string> scripts = SqlInstaller.ListInstallationScripts(null, new Version(1, 5, 0, 0));
            Assert.AreEqual(2, scripts.Count, "We expected to see two scripts.");
            Assert.AreEqual("Installation.01.00.00.sql", scripts[0], "Expected the initial 1.0 installation file.");
            Assert.AreEqual("Installation.01.05.00.sql", scripts[1], "Expected the bugfix 1.5 installation file.");

            scripts = SqlInstaller.ListInstallationScripts(null, new Version(1, 0, 3, 0));
            Assert.AreEqual(1, scripts.Count, "We expected to see one script.");
            Assert.AreEqual("Installation.01.00.00.sql", scripts[0], "Expected the initial 1.0 installation file.");

            scripts = SqlInstaller.ListInstallationScripts(null, new Version(0, 0, 3, 0));
            Assert.AreEqual(0, scripts.Count, "We expected to see no scripts.");

            scripts = SqlInstaller.ListInstallationScripts(new Version(1, 1, 0, 0), new Version(1, 5, 0, 0));
            Assert.AreEqual(1, scripts.Count, "We expected to see one script.");
            Assert.AreEqual("Installation.01.05.00.sql", scripts[0], "Expected the bugfix 1.5.0 installation file.");

            scripts = SqlInstaller.ListInstallationScripts(new Version(1, 1, 0, 0), new Version(1, 9, 0, 0));
            Assert.AreEqual(2, scripts.Count, "We expected to see two script.");
            Assert.AreEqual("Installation.01.09.00.sql", scripts[1], "Expected the 1.9.0 installation file.");
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