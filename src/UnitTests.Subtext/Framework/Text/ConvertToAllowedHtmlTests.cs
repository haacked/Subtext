using System;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
    /// <summary>
    /// Unit tests of the ConvertToAllowedHtml method and 
    /// just that method (plus its overrides).
    /// </summary>
    [TestClass]
    public class ConvertToAllowedHtmlTests
    {
        [TestMethod]
        public void Ctor_WithNullText_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() =>
                                                             HtmlHelper.ConvertToAllowedHtml(null));
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("How now brown cow.", "How now brown cow.")]
        [DataRow("How now brown cow.", "How now brown cow.")]
        [DataRow("&", "&amp;")]
        [DataRow("<", "&lt;")]
        [DataRow(">", "&gt;")]
        [DataRow("\r\r\n", "<br />")]
        public void StripsDefaultHtmlWhenNoAllowedTagsSpecified(string text, string expected)
        {
            Assert.AreEqual(expected, HtmlHelper.ConvertToAllowedHtml(null, text));
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("<", "&lt;")]
        [DataRow(">", "&gt;")]
        [DataRow("<>", "&lt;&gt;")]
        [DataRow("How now brown cow.", "How now brown cow.")]
        [DataRow("How <strong>now</strong> brown cow.", "How <strong>now</strong> brown cow.")]
        [DataRow("How <strong>now</strong> brown <cow.", "How <strong>now</strong> brown &lt;cow.")]
        [DataRow("<How <strong>now</strong>", "&lt;How <strong>now</strong>")]
        [DataRow("Text Before <a href=\"test\">a</a> Text After", "Text Before <a href=\"test\">a</a> Text After")]
        [DataRow("<a href=\"test\">a</a>", "<a href=\"test\">a</a>")]
        [DataRow("<a href=\"test\" rel=\"notallowed\">a</a>", "<a href=\"test\">a</a>")]
        [DataRow("<a title=\">\">a</a>", "<a title=\"&gt;\">a</a>")]
        [DataRow("<A TITLE=\">\">a</a>", "<a title=\"&gt;\">a</a>")]
        [DataRow("<a\r\ntitle=\">\">a</a>", "<a title=\"&gt;\">a</a>")]
        [DataRow("<a href='test'></a>", "<a href=\"test\"></a>")]
        [DataRow("<a href=test></a>", "<a href=\"test\"></a>")]
        [DataRow("<a href=test title=\"cool\"></a>", "<a href=\"test\" title=\"cool\"></a>")]
        [DataRow("<a href=test title=cool></a>", "<a href=\"test\" title=\"cool\"></a>")]
        [DataRow("<a title></a>", "<a></a>")]
        [DataRow("<a title href=\"test\"></a>", "<a href=\"test\"></a>")]
        [DataRow("<a title href=\"test\" title></a>", "<a href=\"test\"></a>")]
        [DataRow("<a title href=\"test\" title title title></a>", "<a href=\"test\"></a>")]
        [DataRow("<a title=\"one\" title=\"two\"></a>", "<a title=\"one,two\"></a>")]
        [DataRow("<a title=\"one\" title=\"two\"></a>", "<a title=\"one,two\"></a>")]
        [DataRow("This is a comment <em>That forgets to close the <em> tag.",
            "This is a comment <em>That forgets to close the <em> tag.</em></em>")]
        public void StripsNonAllowedHtml(string text, string expected)
        {
            var allowedTags = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            allowedTags.Add("a", "href,title");
            allowedTags.Add("strong", "");
            allowedTags.Add("em", "");
            UnitTestHelper.AssertStringsEqualCharacterByCharacter(expected,
                                                                  HtmlHelper.ConvertToAllowedHtml(allowedTags, text));
        }

        [TestMethod]
        public void ClosesOpenTags()
        {
            var allowedTags = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            allowedTags.Add("u", "");
            string result = HtmlHelper.ConvertToAllowedHtml(allowedTags,
                                                            "This is <u>Underlined. But I forgot to close it.");
            Assert.AreEqual("This is <u>Underlined. But I forgot to close it.</u>", result,
                            "Expected that the tag would get closed");
        }
    }
}