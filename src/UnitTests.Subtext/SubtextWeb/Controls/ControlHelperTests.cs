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

using System.Web.UI.WebControls;
using MbUnit.Framework;
using Subtext.Web.Controls;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
    [TestFixture]
    public class ControlHelperTests
    {
        [RowTest]
        [Row("a tooltip", "trying this title", "a tooltip")]
        [Row((string)null, "this is my title", "this is my title")]
        [Row("", "", "")]
        public void OnlyAddTitleWhenNotAlreadyThere(string toolTip, string title, string expectedTitle)
        {
            var link = new HyperLink();
            link.ToolTip = toolTip;

            ControlHelper.SetTitleIfNone(link, title);

            Assert.AreEqual(expectedTitle, link.ToolTip, "Didn't set the tooltip correctly.");
            Assert.IsNull(link.Attributes["title"], "Oops, looks like we set the title attribute too!");
        }

        [Test]
        public void OnlyAddCssClassWhenNotAlreadyThere()
        {
            var label = new Label();
            label.CssClass = "foo bar baz quux";

            ControlHelper.AddCssClass(label, "baz");
            Assert.AreEqual("foo bar baz quux", label.CssClass);

            ControlHelper.AddCssClass(label, "click");
            Assert.AreEqual("foo bar baz quux click", label.CssClass);

            label.CssClass = "click";
            ControlHelper.AddCssClass(label, "click");
            Assert.AreEqual("click", label.CssClass);

            label.CssClass = "";
            ControlHelper.AddCssClass(label, "click");
            Assert.AreEqual("click", label.CssClass);

            label.CssClass = "click";
            ControlHelper.AddCssClass(label, "cluck");
            Assert.AreEqual("click cluck", label.CssClass);
        }
    }
}