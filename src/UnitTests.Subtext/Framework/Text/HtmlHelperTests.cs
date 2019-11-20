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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework;
using Subtext.Framework.Text;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Text
{
    /// <summary>
    /// Tests of the <see cref="HtmlHelper"/> class.
    /// </summary>
    [TestClass]
    public class HtmlHelperTests
    {
        [DataTestMethod]
        [DataRow("", 10, "")]
        [DataRow("http://example.com/", 50, "http://example.com/")]
        [DataRow("http://example.com/testxtest.aspx", 25, "example.com")]
        [DataRow("http://example.com/", 10, "example...")]
        [DataRow("http://example.com/", 11, "example.com")]
        [DataRow("http://example.com", 11, "example.com")]
        [DataRow("http://example.com", 5, "ex...")]
        public void CanShortenUrl(string url, int max, string expected)
        {
            Assert.AreEqual(expected, url.ShortenUrl(max));
        }

        [TestMethod]
        public void ShortenUrl_WithNullUrl_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => ((string)null).ShortenUrl(10));
        }

        [TestMethod]
        public void ShortenUrl_WithTwoSegmentsEndingWithFileName_OnlyCompressesMiddleSegment()
        {
            const string url = "http://example.com/test/test.aspx";

            string shorty = url.ShortenUrl(25);

            Assert.AreEqual("example.com/.../test.aspx", shorty);
        }

        [TestMethod]
        public void ShortenUrl_WithTwoSegmentsAndTrailingSlash_OnlyCompressesMiddleSegment()
        {
            const string url = "http://example.com/test/testagain/";

            string shorty = url.ShortenUrl(26);

            Assert.AreEqual("example.com/.../testagain/", shorty);
        }

        [TestMethod]
        public void ShortenUrl_WithMaxLessThanFive_ThrowsArgumentOutOfRangeException()
        {
            // arrange
            const string url = "http://subtextproject.com/";

            // act, assert
            UnitTestHelper.AssertThrows<ArgumentOutOfRangeException>(() => url.ShortenUrl(4));
        }

        [TestMethod]
        public void ShortenUrl_WithQueryParamsMakingUrlTooLong_RemovesQueryParams()
        {
            // arrange
            const string url = "http://do.com/?foo=bar";

            // act
            string shorty = url.ShortenUrl(6);

            // assert
            Assert.AreEqual("do.com", shorty);
        }

        [DataTestMethod]
        [DataRow("http://example.com", "www.example.com", "http://www.example.com")]
        [DataRow("http://example.com", "example.com", "http://example.com")]
        [DataRow("http://example.com/", "example.com", "http://example.com/")]
        [DataRow("http://example.com/example.com/", "example.com", "http://example.com/example.com/")]
        [DataRow("http://www.example.com", "example.com", "http://example.com")]
        [DataRow("http://example.com/", "www.example.com", "http://www.example.com/")]
        [DataRow("http://example.com:8080/", "www.example.com", "http://www.example.com:8080/")]
        [DataRow("http://example.com:8080/example.com/blah.html", "www.example.com",
            "http://www.example.com:8080/example.com/blah.html")]
        [DataRow("http://example.com/example.com/blah.html", "www.example.com",
            "http://www.example.com/example.com/blah.html")]
        [DataRow("http://example.com/example.com/", "www.example.com", "http://www.example.com/example.com/")]
        public void CanReplaceHostInUrl(string url, string host, string expected)
        {
            Assert.AreEqual(expected, HtmlHelper.ReplaceHost(url, host));
        }

        [TestMethod]
        public void AppendNullClassThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => HtmlHelper.AppendCssClass(new TextBox(), null));
        }

        [TestMethod]
        public void AppendClassToNullControlThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => HtmlHelper.AppendCssClass(null, "blah"));
        }

        [TestMethod]
        public void RemoveNullClassThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => HtmlHelper.RemoveCssClass(new TextBox(), null));
        }

        [TestMethod]
        public void RemoveClassFromNullControlThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => HtmlHelper.RemoveCssClass(null, "blah"));
        }

        [TestMethod]
        public void RemoveClassFromControlWithNoClasHasNoEffect()
        {
            var textbox = new TextBox();
            HtmlHelper.RemoveCssClass(textbox, "blah");
            Assert.AreEqual(string.Empty, textbox.CssClass);
        }

        [TestMethod]
        public void CanAppendCssClassToControl()
        {
            var textbox = new TextBox();
            HtmlHelper.AppendCssClass(textbox, "testclass");
            Assert.AreEqual("testclass", textbox.CssClass);

            HtmlHelper.AppendCssClass(textbox, "testclass");
            Assert.AreEqual("testclass", textbox.CssClass);

            HtmlHelper.AppendCssClass(textbox, "blah");
            Assert.AreEqual("testclass blah", textbox.CssClass);

            HtmlHelper.AppendCssClass(textbox, "BLAH");
            Assert.AreEqual("testclass blah BLAH", textbox.CssClass);
        }

        [TestMethod]
        public void CanRemoveCssClassToControl()
        {
            var textbox = new TextBox();
            HtmlHelper.AppendCssClass(textbox, "testclass");
            HtmlHelper.AppendCssClass(textbox, "blah");
            HtmlHelper.AppendCssClass(textbox, "BLAH");
            Assert.AreEqual("testclass blah BLAH", textbox.CssClass);

            HtmlHelper.RemoveCssClass(textbox, "blah");
            Assert.AreEqual("testclass BLAH", textbox.CssClass);

            HtmlHelper.RemoveCssClass(textbox, "BLAH");
            HtmlHelper.RemoveCssClass(textbox, "testclass");
            Assert.AreEqual(string.Empty, textbox.CssClass);
        }

        /// <summary>
        /// Tests that EnableUrls formats urls with anchor tags.
        /// </summary>
        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("http://haacked.com/one/two/three/four/five/six/seven/eight/nine/ten.aspx",
            "<a rel=\"nofollow external\" href=\"http://haacked.com/one/two/three/four/five/six/seven/eight/nine/ten.aspx\" title=\"http://haacked.com/one/two/three/four/five/six/seven/eight/nine/ten.aspx\">haacked.com/.../ten.aspx</a>"
            )]
        [DataRow("begin http://haacked.com/ end.",
            "begin <a rel=\"nofollow external\" href=\"http://haacked.com/\" title=\"http://haacked.com/\">http://haacked.com/</a> end."
            )]
        [DataRow("begin http://haacked.com/ two http://localhost/someplace/some.page.aspx end.",
            "begin <a rel=\"nofollow external\" href=\"http://haacked.com/\" title=\"http://haacked.com/\">http://haacked.com/</a> two <a rel=\"nofollow external\" href=\"http://localhost/someplace/some.page.aspx\" title=\"http://localhost/someplace/some.page.aspx\">http://localhost/someplace/some.page.aspx</a> end."
            )]
        [DataRow("this www.haacked.com",
            "this <a rel=\"nofollow external\" href=\"http://www.haacked.com\" title=\"www.haacked.com\">www.haacked.com</a>"
            )]
        [DataRow("<p>www.haacked.com</p>",
            "<p><a rel=\"nofollow external\" href=\"http://www.haacked.com\" title=\"www.haacked.com\">www.haacked.com</a></p>"
            )]
        [DataRow("<b>www.haacked.com</b>",
            "<b><a rel=\"nofollow external\" href=\"http://www.haacked.com\" title=\"www.haacked.com\">www.haacked.com</a></b>"
            )]
        [DataRow("subtextproject.com", "subtextproject.com")]
        [DataRow("www.subtextproject.com?test=test&blah=blah",
            "<a rel=\"nofollow external\" href=\"http://www.subtextproject.com?test=test&amp;blah=blah\" title=\"www.subtextproject.com?test=test&amp;blah=blah\">www.subtextproject.com?test=test&amp;blah=blah</a>"
            )]
        [DataRow("<a href=\"http://example.com/\">Test</a>", "<a href=\"http://example.com/\">Test</a>")]
        [DataRow("<img src=\"http://example.com/\" />", "<img src=\"http://example.com/\" />")]
        [DataRow("<a href='http://example.com/'>Test</a>", "<a href=\"http://example.com/\">Test</a>")]
        [DataRow("<a href=http://example.com/>Test</a>", "<a href=\"http://example.com/\">Test</a>")]
        [DataRow("<b title=\"blah http://example.com/ blah\" />", "<b title=\"blah http://example.com/ blah\" />")]
        [DataRow("a < b blah http://example.com/",
            "a &lt; b blah <a rel=\"nofollow external\" href=\"http://example.com/\" title=\"http://example.com/\">http://example.com/</a>"
            )]
        [DataRow("www.haacked.com<a href=\"test\">test</a>",
            "<a rel=\"nofollow external\" href=\"http://www.haacked.com\" title=\"www.haacked.com\">www.haacked.com</a><a href=\"test\">test</a>"
            )]
        public void ConvertUrlsToHyperLinksConvertsUrlsToAnchorTags(string html, string expected)
        {
            Assert.AreEqual(expected, HtmlHelper.ConvertUrlsToHyperLinks(html));
        }

        [TestMethod]
        public void ConvertUrlsToHyperLinks_WithNullHtml_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => HtmlHelper.ConvertUrlsToHyperLinks(null));
        }

        [TestMethod]
        public void ConvertUrlToHyperlinksIgnoreAnchorContents()
        {
            string html = "<a href=\"/\"><b>http://example.com/</b></a>";
            Assert.AreEqual(html, HtmlHelper.ConvertUrlsToHyperLinks(html));
        }

        [TestMethod]
        public void Html_WithEncodedMarkup_IsNotUnencoded()
        {
            string html = "&lt;script /&gt;";
            Assert.AreEqual(html, HtmlHelper.ConvertUrlsToHyperLinks(html));
        }

        /// <summary>
        /// HasIllegalContent throws exception when encountering encoded tag.
        /// </summary>
        [DataTestMethod]
        [DataRow("blah &#60script ", true)]
        [DataRow("blah <script ", true)]
        [DataRow("blah script ", false)]
        public void HasIllegalContentReturnsExpectedAnswer(string html, bool expected)
        {
            Assert.AreEqual(expected, HtmlHelper.HasIllegalContent(html));
        }

        [TestMethod]
        public void CanParseTag()
        {
            IList<string> tags =
                "blah blah <a href=\"http://blah.com/subdir/mytag/\" rel=\"tag\">test1</a> goo goo".ParseTags();
            Assert.AreEqual(1, tags.Count, "Should have found one tag.");
            Assert.AreEqual("mytag", tags[0], "Should have found one tag.");
        }

        [TestMethod]
        public void ParseTags_WithDuplicateTags_DoesNotParseDuplicate()
        {
            IList<string> tags =
                "<a href=\"http://blah.com/subdir/mytag/\" rel=\"tag\">test1</a><a href=\"http://blah.com/another-dir/mytag/\" rel=\"tag\">test2</a>"
                    .ParseTags();
            Assert.AreEqual(1, tags.Count, "The same tag exists twice, should only count as one.");
        }

        [TestMethod]
        public void ParseTagsDoesNotMatchRelOfAnotherTag()
        {
            IList<string> tags =
                ("<a title=\"blah\" href=\"http://blah.com/subdir/mytag1/\" " + Environment.NewLine +
                 " rel=\"lightbox\">mytag1</a>other junk " + Environment.NewLine +
                 "<a href=\"http://blah.com/another-dir/mytag2/\" rel=\"tag\">mytag2</a>").ParseTags();
            Assert.AreEqual(1, tags.Count, "The first anchor is not a tag.");
            Assert.AreEqual("mytag2", tags[0]);
        }

        [TestMethod]
        public void ParseTags_WithWhitespaceBetweenAttributes_ParsesTagCorrectly()
        {
            IList<string> tags =
                ("<a title=\"blah\" href = " + Environment.NewLine + " \"http://blah.com/subdir/mytag1/\" rel = " +
                 Environment.NewLine + " \"tag\">mytag1</a>").ParseTags();
            Assert.AreEqual(1, tags.Count, "The attributes contain whitespace but should be recognized as valid");
            Assert.AreEqual("mytag1", tags[0]);
        }

        [TestMethod]
        public void ParseTags_WithWeirdWhiteSpace_ParsesTagCorrectly()
        {
            IList<string> tags =
                ("<a title=\"Programmer's Bill of Rights\" href=\"http://www.codinghorror.com/blog/archives/000666.html\">Programmer&rsquo;s Bill of Rights</a> that <a rel=\"friend met\" href=\"http://www.codinghorror.com/blog/\">Jeff Atwood</a>" +
                 Environment.NewLine +
                 "<div class=\"tags\">Technorati tags: <a rel=\"tag\" href=\"http://technorati.com/tag/Programming\">Programming</a>")
                    .ParseTags();
            Assert.AreEqual(1, tags.Count, "The attributes contain whitespace but should be recognized as valid");
            Assert.AreEqual("Programming", tags[0]);
        }

        [TestMethod]
        public void ParseTags_WithUrlEndingWithDefaultAspx_WeirdWhiteSpace()
        {
            // arrange
            string html =
                "<a title=\"Programmer's Bill of Rights\" href=\"http://www.codinghorror.com/blog/archives/000666.html\">Programmer&rsquo;s Bill of Rights</a> that <a rel=\"friend met\" href=\"http://www.codinghorror.com/blog/\">Jeff Atwood</a>" +
                Environment.NewLine +
                "<div class=\"tags\">Technorati tags: <a rel=\"tag\" href=\"http://technorati.com/tag/Programming/default.aspx\">Programming</a>";

            // act
            IList<string> tags = html.ParseTags();

            // assert
            Assert.AreEqual(1, tags.Count, "The attributes contain whitespace but should be recognized as valid");
            Assert.AreEqual("Programming", tags[0]);
        }

        [TestMethod]
        public void ParseTags_WithMultipleRelAttributeValues_ParsesTag()
        {
            // arrange
            string html = "<a href=\"http://blah/yourtag\" rel=\"tag friend\">nothing</a>";

            // act
            IList<string> tags = html.ParseTags();

            // assert
            Assert.AreEqual("yourtag", tags.First());
        }

        [TestMethod]
        public void ParseTags_WithEncodedTag_ParsesTag()
        {
            // arrange
            string html = "<a href=\"http://blah/tags/c%23/default.aspx\" rel=\"tag\">C#</a>";

            // act
            IList<string> tags = html.ParseTags();

            // assert
            Assert.AreEqual("c#", tags.First());
        }

        [DataTestMethod]
        [DataRow("http://blah.com/blah/", "blah")]
        [DataRow("http://blah.com/foo-bar", "foo-bar")]
        [DataRow("http://blah.com/query?someparm=somevalue", "query")]
        [DataRow("http://blah.com/query/?someparm=somevalue", "query")]
        [DataRow("http://blah.com/decode+test", "decode test")]
        [DataRow("http://blah.com/decode%20test2", "decode test2")]
        [DataRow("http://blah.com/another+decode%20test", "another decode test")]
        public void CanParseEntryTags(string url, string expectedTag)
        {
            // arrange
            string html = "<a href=\"" + url + "\" rel=\"tag\">nothing</a>";

            // act
            IList<string> tags = html.ParseTags();

            // assert;
            Assert.AreEqual(1, tags.Count);
            Assert.AreEqual(expectedTag, tags.First());
        }

        [DataTestMethod]
        [DataRow(" rel = \"tag\" ", " rel = \"tag\"", true)]
        [DataRow(" xrel = \"tag\" ", null, false)]
        [DataRow(" rel = \"friend tag\" ", " rel = \"friend tag\"", true)]
        [DataRow(" rel = \"friend tag met\" ", " rel = \"friend tag met\"", true)]
        [DataRow(" rel = \"tag met\" ", " rel = \"tag met\"", true)]
        [DataRow(" rel=\"friend met\"> rel=\"tag\" ", " rel=\"tag\"", true)]
        [DataRow(" rel = \'tag\' ", " rel = \'tag\'", true)]
        [DataRow(" xrel = \'tag\' ", null, false)]
        [DataRow(" rel = \'friend tag\' ", " rel = \'friend tag\'", true)]
        [DataRow(" rel = \'friend tag met\' ", " rel = \'friend tag met\'", true)]
        [DataRow(" rel = \'tag met\' ", " rel = \'tag met\'", true)]
        [DataRow(" rel=\'friend met\'> rel=\'tag\' ", " rel=\'tag\'", true)]
        public void CanParseRelTag(string original, string matched, bool expected)
        {
            var relRegex = new Regex(@"\s+rel\s*=\s*(""[^""]*?\btag\b.*?""|'[^']*?\btag\b.*?')",
                                     RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match match = relRegex.Match(original);
            Assert.AreEqual(expected, match.Success);
            if (match.Success)
            {
                Assert.AreEqual(matched, match.Value);
            }
        }

        [DataTestMethod]
        [DataRow("  <a href=\"foo\">test</a>  ", "<a href=\"foo\">test</a>", true)]
        [DataRow("  <a href=\"foo\" title=\"blah\">test</a>  ", "<a href=\"foo\" title=\"blah\">test</a>", true)]
        [DataRow("  <a href = \"foo\" >test</a>  ", "<a href = \"foo\" >test</a>", true)]
        [DataRow("  <span title=\"test <a href=\"> <a href=\"foo2\">test2</a>", "<a href=\"foo2\">test2</a>", true)]
        public void CanParseAnchorTags(string original, string expectedMatchValue, bool expectedMatch)
        {
            var regex = new Regex(@"<a(\s+\w+\s*=\s*(?:""[^""]*?""|'[^']*?')(?!\w))+\s*>.*?</a>",
                                  RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match match = regex.Match(original);
            Assert.AreEqual(expectedMatch, match.Success);
            if (match.Success)
            {
                string matchValue = match.Value;
                Assert.AreEqual(expectedMatchValue, matchValue);
            }
        }

        [TestMethod]
        public void ParseUri_WithValidUri_ReturnsNull()
        {
            // arrange
            string notUri = "http://haacked.com/";

            // act
            Uri parsed = notUri.ParseUri();

            // assert
            Assert.AreEqual("haacked.com", parsed.Host);
        }

        [TestMethod]
        public void ParseUri_WithInvalidUri_ReturnsNull()
        {
            // arrange
            string notUri = "blah@example.com";

            // act
            Uri parsed = notUri.ParseUri();

            // assert
            Assert.IsNull(parsed);
        }

        [TestMethod]
        public void EnsureUrl_WithoutHttp_PrependsHttp()
        {
            // arrange
            string text = "subtextproject.com";

            // act
            Uri url = text.EnsureUrl();

            // assert
            Assert.IsNotNull(url);
            Assert.AreEqual("subtextproject.com", url.Host);
        }

        [TestMethod]
        public void EnsureUrl_WithNull_ReturnsNull()
        {
            // arrange
            string text = null;

            // act
            Uri url = text.EnsureUrl();

            // assert
            Assert.IsNull(url);
        }

        [TestMethod]
        public void EnsureUrl_WithStringHavingOnlyWhitespace_ReturnsNull()
        {
            // arrange
            string text = "     ";

            // act
            Uri url = text.EnsureUrl();

            // assert
            Assert.IsNull(url);
        }

        [TestMethod]
        public void GetAttributeValues_WithHtmlContainingAttributeValues_ReturnsAttributeValues()
        {
            // arrange
            string html =
                @"<html>
                <img src=""test.jpg"" />
                <img src=""test2.jpg""></img>
            </html>";

            // act
            IEnumerable<string> imageSources = html.GetAttributeValues("img", "src");

            // assert
            Assert.AreEqual(2, imageSources.Count());
            Assert.AreEqual("test.jpg", imageSources.First());
            Assert.AreEqual("test2.jpg", imageSources.ElementAt(1));
        }

        [TestMethod]
        public void GetAttributeValues_WithNonBalancedQuoteInMiddle_ReturnsAttributeValuesContainingQuoteCharacter()
        {
            // arrange
            string html =
                @"<html>
                <img src=""test's.jpg"" />
                <img src='test2"".jpg'></img>
            </html>";

            // act
            IEnumerable<string> imageSources = html.GetAttributeValues("img", "src");

            // assert
            Assert.AreEqual(2, imageSources.Count());
            Assert.AreEqual("test's.jpg", imageSources.First());
            Assert.AreEqual("test2\".jpg", imageSources.ElementAt(1));
        }

        [TestMethod]
        public void
            GetAttributeValues_WithHtmlHavingDuplicateHtmlTagsAndContainingAttributeValues_ReturnsAttributeValues()
        {
            // arrange
            string html =
                @"<html><html>
                <img src=""test.jpg"" />
                <img src=""test2.jpg""></img>
            </html></html>";

            // act
            IEnumerable<string> imageSources = html.GetAttributeValues("img", "src");

            // assert
            Assert.AreEqual(2, imageSources.Count());
            Assert.AreEqual("test.jpg", imageSources.First());
            Assert.AreEqual("test2.jpg", imageSources.ElementAt(1));
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            //Confirm app settings
            UnitTestHelper.AssertAppSettings();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "MyBlog");
            var blogInfo = new Blog();
            blogInfo.Host = "localhost";
            blogInfo.Subfolder = "MyBlog";

            BlogRequest.Current.Blog = blogInfo;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            HttpContext.Current = null;
        }
    }
}