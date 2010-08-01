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

using System.Collections.Specialized;
using System.Configuration.Provider;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Configuration;
using Subtext.Providers.BlogEntryEditor.FCKeditor;
using Subtext.Web.Providers.BlogEntryEditor.FTB;
using Subtext.Web.Admin;
using Subtext.Web.Providers.BlogEntryEditor.PlainText;

namespace UnitTests.Subtext.Framework.Providers
{
    /// <summary>
    /// Tests a few methods of the <see cref="ProviderBase"/> class.
    /// </summary>
    [TestFixture]
    public class ProviderConfigurationHelperTests
    {
        [Test]
        public void CanFindConnectionString()
        {
            Assert.IsNotNull(Config.ConnectionString);
        }

        /// <summary>
        /// Makes sure that we can recognize false setting pointers.
        /// </summary>
        [Test]
        public void GetConnectionStringSettingValueFindsConnectionString()
        {
            var configValue = new NameValueCollection {{"connectionStringName", "subtextData"}};
            Assert.AreEqual("Server=localhost;Database=SubtextData;Trusted_Connection=True;",
                            ProviderConfigurationHelper.GetConnectionStringSettingValue("connectionStringName",
                                                                                        configValue));
        }

        [Test]
        public void ProvideEditor_Default_Is_FtbEditor()
        {
            using (var httpRequest = new HttpSimulator("http://localhost").SimulateRequest())
            {
                //arrange
                BlogEntryEditorProvider provider;
                
                //act
                ProviderConfigurationHelper.LoadProviderCollection<BlogEntryEditorProvider>("BlogEntryEditor", out provider);

                //post (in test app.config the FtbBlogEntryEditorProvider is selected by default)
                Assert.IsTrue(provider is FtbBlogEntryEditorProvider, "FtbBlogEntryEditorProvider is created by default.");
            }
        }
    }
}