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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Format;

namespace UnitTests.Subtext.Framework.Format
{
    /// <summary>
    /// Unit tests of the <see cref="UrlFormats"/> class which 
    /// is used to format Subtext specific urls.
    /// </summary>
    [TestClass]
    public class UrlFormatTests
    {
        /// <summary>
        /// Makes sure that UrlFormats.GetBlogAppFromRequest does the right thing.
        /// </summary>
        [DataTestMethod]
        [DataRow("/Subtext.Web/MyBlog/default.aspx", "/Subtext.Web", "MyBlog")]
        [DataRow("/subtext.web/MyBlog/default.aspx", "/Subtext.Web", "MyBlog")]
        [DataRow("/subtext.web/default.aspx", "/Subtext.Web", "")]
        [DataRow("/subtext.web", "/Subtext.Web", "")]
        [DataRow("/subtext.web/myBLOG/", "/Subtext.Web", "myBLOG")]
        [DataRow("/subtext.web/myblog", "/Subtext.Web", "myblog")]
        [DataRow("/foo/bar", "/", "foo")]
        public void GetBlogAppFromRequestDoesTheRightThing(string rawUrl, string subfolder, string expected)
        {
            Assert.AreEqual(expected, UrlFormats.GetBlogSubfolderFromRequest(rawUrl, subfolder));
        }
    }
}