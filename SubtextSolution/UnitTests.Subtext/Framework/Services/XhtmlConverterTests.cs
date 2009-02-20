using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class XhtmlConverterTests
    {
        [RowTest]
        [Row("This is some text", "This is some text")]
        [Row("<span>This is some text</span>", "<span>This is some text</span>")]
        [Row("<img src=\"blah\" />", "<img src=\"blah\" />")]
        [Row("<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>", "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
        public void Transform_WithValidMarkup_DoesNotChangeIt(string markup, string expected) { 
            //arrange
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(markup);

            //assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Transform_WithConverter_AppliesConverterWhileConvertingHtml()
        {
            string html = "<p title=\"blah blah\"> blah blah </p>";
            string expected = "<p title=\"blah blah\"> yadda yadda </p>";

            //arrange
            var converter = new XhtmlConverter(input => input.Replace("blah", "yadda"));

            //act
            string result = converter.Transform(html);

            //assert
            Assert.AreEqual(expected, result);
        }

        [Test]
		public void ConvertHtmlToXHtmlLeavesNestedMarkupAlone()
		{
            //arrange
			string expected = "<p><span>This is some text</span> <span>this is more text</span></p>";
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(expected);

            //assert
            Assert.AreEqual(expected, result);
		}

        /// <summary>
        /// Makes sure that IsValidXHTML recognizes invalid markup.
        /// </summary>
        [RowTest]
        [Row("<a href=\"xyz\">test<b>Test</b>", "<a href=\"xyz\">test<b>Test</b></a>")]
        [Row("This <br /><br />is bad <p> XHTML.", "This <br /><br />is bad <p> XHTML.</p>")]
        [Row("This <br /><br style=\"blah\" />is bad <p> XHTML.", "This <br /><br style=\"blah\" />is bad <p> XHTML.</p>")]
        [Row("This <P>is bad </P> XHTML.", "This <p>is bad </p> XHTML.")]
        [Row("<style type=\"text/css\">\r\n<![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>", "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
        [Row("<style type=\"text/css\">\r\n\r\n<![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>", "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
        public void ConvertHtmlToXHtmlCorrectsInvalidMarkup(string badMarkup, string corrected)
        {
            //arrange
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(badMarkup);

            //assert
            Assert.AreEqual(corrected, result);
        }

        [RowTest]
        [Row("<a name=\"test\"></a>", "<a name=\"test\"></a>", "Anchor tags should not be self-closed.")]
        [Row("<a name=\"test\" />", "<a name=\"test\"></a>", "Anchor tags should not be self-closed.")]
        [Row("<script src=\"test\" />", "<script src=\"test\"></script>", "Script tags should not be self-closed.")]
        [Row("<script src=\"test\"></script>", "<script src=\"test\"></script>", "Script tags should not be self-closed.")]
        public void ConvertHtmlToXhtmlEnsuresSomeTagsMustNotBeSelfClosed(string html, string expected, string message)
        {
            //arrange
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(html);

            //assert
            Assert.AreEqual(expected, result);
        }

        [RowTest]
        [Row("br")]
        [Row("hr")]
        [Row("meta")]
        [Row("link")]
        [Row("input")]
        [Row("img")]
        public void ConvertHtmlToXhtmlEnsuresSomeTagsMustBeSelfClosed(string tag)
        {
            //arrange
            string html = string.Format("<{0} src=\"blah-blah\"></{0}>", tag);
            string expected = string.Format("<{0} src=\"blah-blah\" />", tag);
            var converter = new XhtmlConverter();

            //act
            string result = converter.Transform(html);

            //assert
            Assert.AreEqual(expected, result);
        }
    }
}
