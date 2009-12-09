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
using Subtext.Framework.Format;

namespace UnitTests.Subtext.Framework.Format
{
    /// <summary>
    /// Unit tests of the <see cref="UrlFormats"/> class which 
    /// is used to format Subtext specific urls.
    /// </summary>
    [TestFixture]
    public class UrlFormatTests
    {
        /// <summary>
        /// Makes sure that UrlFormats.GetBlogAppFromRequest does the right thing.
        /// </summary>
        [RowTest]
        [Row("/Subtext.Web/MyBlog/default.aspx", "/Subtext.Web", "MyBlog")]
        [Row("/subtext.web/MyBlog/default.aspx", "/Subtext.Web", "MyBlog")]
        [Row("/subtext.web/default.aspx", "/Subtext.Web", "")]
        [Row("/subtext.web", "/Subtext.Web", "")]
        [Row("/subtext.web/myBLOG/", "/Subtext.Web", "myBLOG")]
        [Row("/subtext.web/myblog", "/Subtext.Web", "myblog")]
        [Row("/foo/bar", "/", "foo")]
        public void GetBlogAppFromRequestDoesTheRightThing(string rawUrl, string subfolder, string expected)
        {
            Assert.AreEqual(expected, UrlFormats.GetBlogSubfolderFromRequest(rawUrl, subfolder));
        }
    }
}