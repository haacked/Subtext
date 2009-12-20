using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
    /// <summary>
    /// Unit tests of the ConvertToAllowedHtml method and 
    /// just that method (plus its overrides).
    /// </summary>
    [TestFixture]
    public class ConvertToAllowedHtmlTests
    {
        [Test]
        public void Ctor_WithNullText_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() =>
                                                             HtmlHelper.ConvertToAllowedHtml(null));
        }

        [RowTest]
        [Row("", "")]
        [Row("How now brown cow.", "How now brown cow.")]
        [Row("How now brown cow.", "How now brown cow.")]
        [Row("&", "&amp;")]
        [Row("<", "&lt;")]
        [Row(">", "&gt;")]
        [Row("\r\r\n", "<br />")]
        public void StripsDefaultHtmlWhenNoAllowedTagsSpecified(string text, string expected)
        {
            Assert.AreEqual(expected, HtmlHelper.ConvertToAllowedHtml(null, text));
        }

        [RowTest]
        [Row("", "")]
        [Row("<", "&lt;")]
        [Row(">", "&gt;")]
        [Row("<>", "&lt;&gt;")]
        [Row("How now brown cow.", "How now brown cow.")]
        [Row("How <strong>now</strong> brown cow.", "How <strong>now</strong> brown cow.")]
        [Row("How <strong>now</strong> brown <cow.", "How <strong>now</strong> brown &lt;cow.")]
        [Row("<How <strong>now</strong>", "&lt;How <strong>now</strong>")]
        [Row("Text Before <a href=\"test\">a</a> Text After", "Text Before <a href=\"test\">a</a> Text After")]
        [Row("<a href=\"test\">a</a>", "<a href=\"test\">a</a>")]
        [Row("<a href=\"test\" rel=\"notallowed\">a</a>", "<a href=\"test\">a</a>")]
        [Row("<a title=\">\">a</a>", "<a title=\"&gt;\">a</a>")]
        [Row("<A TITLE=\">\">a</a>", "<a title=\"&gt;\">a</a>")]
        [Row("<a\r\ntitle=\">\">a</a>", "<a title=\"&gt;\">a</a>")]
        [Row("<a href='test'></a>", "<a href=\"test\"></a>")]
        [Row("<a href=test></a>", "<a href=\"test\"></a>")]
        [Row("<a href=test title=\"cool\"></a>", "<a href=\"test\" title=\"cool\"></a>")]
        [Row("<a href=test title=cool></a>", "<a href=\"test\" title=\"cool\"></a>")]
        [Row("<a title></a>", "<a></a>")]
        [Row("<a title href=\"test\"></a>", "<a href=\"test\"></a>")]
        [Row("<a title href=\"test\" title></a>", "<a href=\"test\"></a>")]
        [Row("<a title href=\"test\" title title title></a>", "<a href=\"test\"></a>")]
        [Row("<a title=\"one\" title=\"two\"></a>", "<a title=\"one,two\"></a>")]
        [Row("<a title=\"one\" title=\"two\"></a>", "<a title=\"one,two\"></a>")]
        [Row("This is a comment <em>That forgets to close the <em> tag.",
            "This is a comment <em>That forgets to close the <em> tag.</em></em>")]
        public void StripsNonAllowedHtml(string text, string expected)
        {
            var allowedTags = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
            allowedTags.Add("a", "href,title");
            allowedTags.Add("strong", "");
            allowedTags.Add("em", "");
            UnitTestHelper.AssertStringsEqualCharacterByCharacter(expected,
                                                                  HtmlHelper.ConvertToAllowedHtml(allowedTags, text));
        }

        [Test]
        public void ClosesOpenTags()
        {
            var allowedTags = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
            allowedTags.Add("u", "");
            string result = HtmlHelper.ConvertToAllowedHtml(allowedTags,
                                                            "This is <u>Underlined. But I forgot to close it.");
            Assert.AreEqual("This is <u>Underlined. But I forgot to close it.</u>", result,
                            "Expected that the tag would get closed");
        }
    }
}