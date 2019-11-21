using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestClass]
    public class XhtmlConverterTests
    {
        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("This is some text", "This is some text")]
        [DataRow("<span>This is some text</span>", "<span>This is some text</span>")]
        [DataRow("<img src=\"blah\" />", "<img src=\"blah\" />")]
        [DataRow("<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>",
            "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
        public void Transform_WithValidMarkup_DoesNotChangeIt(string markup, string expected)
        {
            //arrange
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(markup);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Transform_WithAngleBracketInAttributeValue_EncodesAttribute()
        {
            const string html = @"<a title="">"">b</a>";
            const string expected = @"<a title=""&gt;"">b</a>";

            //arrange
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(html);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [Ignore("Need to follow up with the SgmlReader on this")]
        public void Transform_WithStyleTag_DoesNotWrapStyleInCdata()
        {
            const string html = "<style>.test {color: blue;}</style>";
            const string expected = html;

            //arrange
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(html);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Transform_WithConverter_AppliesConverterWhileConvertingHtml()
        {
            const string html = "<p title=\"blah blah\"> blah blah </p>";
            const string expected = "<p title=\"blah blah\"> yadda yadda </p>";

            //arrange
            var converter = new XhtmlConverter(input => input.Replace("blah", "yadda"));

            //act
            string result = converter.Transform(html);

            //assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertHtmlToXHtmlLeavesNestedMarkupAlone()
        {
            //arrange
            const string expected = "<p><span>This is some text</span> <span>this is more text</span></p>";
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(expected);

            //assert
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Makes sure that IsValidXHTML recognizes invalid markup.
        /// </summary>
        [DataTestMethod]
        [DataRow("<a href=\"xyz\">test<b>Test</b>", "<a href=\"xyz\">test<b>Test</b></a>")]
        [DataRow("This <br /><br />is bad <p> XHTML.", "This <br /><br />is bad <p> XHTML.</p>")]
        [DataRow("This <br /><br style=\"blah\" />is bad <p> XHTML.",
            "This <br /><br style=\"blah\" />is bad <p> XHTML.</p>")]
        [DataRow("This <P>is bad </P> XHTML.", "This <p>is bad </p> XHTML.")]
        [DataRow("<style type=\"text/css\">\r\n<![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>",
            "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
        [DataRow("<style type=\"text/css\">\r\n\r\n<![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>",
            "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
        public void ConvertHtmlToXHtmlCorrectsInvalidMarkup(string badMarkup, string corrected)
        {
            //arrange
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(badMarkup);

            //assert
            Assert.AreEqual(corrected, result);
        }

        [DataTestMethod]
        [DataRow("<a name=\"test\"></a>", "<a name=\"test\"></a>", "Anchor tags should not be self-closed.")]
        [DataRow("<a name=\"test\" />", "<a name=\"test\"></a>", "Anchor tags should not be self-closed.")]
        [DataRow("<script src=\"test\" />", "<script src=\"test\"></script>", "Script tags should not be self-closed.")]
        [DataRow("<script src=\"test\"></script>", "<script src=\"test\"></script>",
            "Script tags should not be self-closed.")]
        public void ConvertHtmlToXhtmlEnsuresSomeTagsMustNotBeSelfClosed(string html, string expected, string message)
        {
            //arrange
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(html);

            //assert
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("br")]
        [DataRow("hr")]
        [DataRow("meta")]
        [DataRow("link")]
        [DataRow("input")]
        [DataRow("img")]
        public void ConvertHtmlToXhtmlEnsuresSomeTagsMustBeSelfClosed(string tag)
        {
            //arrange
            string html = string.Format(CultureInfo.InvariantCulture, "<{0} src=\"blah-blah\"></{0}>", tag);
            string expected = string.Format(CultureInfo.InvariantCulture, "<{0} src=\"blah-blah\" />", tag);
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(html);

            //assert
            Assert.AreEqual(expected, result);
        }
    }
}