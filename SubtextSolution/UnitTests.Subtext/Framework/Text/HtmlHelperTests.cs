#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
	/// <summary>
	/// Tests of the <see cref="HtmlHelper"/> class.
	/// </summary>
	[TestFixture]
	public class HtmlHelperTests
	{
		[RowTest]
		[Row(null, 10, null, ExpectedException = typeof(ArgumentNullException))]
		[Row("", 10, "")]
		[Row("http://example.com/", 50, "http://example.com/")]
		[Row("http://example.com/test/testagain/", 26, "example.com/.../testagain/")]
		[Row("http://example.com/test/test.aspx", 25, "example.com/.../test.aspx")]
		[Row("http://example.com/", 10, "example...")]
		[Row("http://example.com/", 11, "example.com")]
		[Row("http://example.com", 11, "example.com")]
		[Row("http://example.com", 5, "ex...")]
		public void CanShortenUrl(string url, int max, string expected)
		{
			Assert.AreEqual(expected, HtmlHelper.ShortenUrl(url, max));
		}

		[RowTest]
		[Row("http://example.com", "www.example.com", "http://www.example.com")]
		[Row("http://example.com", "example.com", "http://example.com")]
		[Row("http://example.com/", "example.com", "http://example.com/")]
		[Row("http://example.com/example.com/", "example.com", "http://example.com/example.com/")]
		[Row("http://www.example.com", "example.com", "http://example.com")]
		[Row("http://example.com/", "www.example.com", "http://www.example.com/")]
		[Row("http://example.com:8080/", "www.example.com", "http://www.example.com:8080/")]
		[Row("http://example.com:8080/example.com/blah.html", "www.example.com", "http://www.example.com:8080/example.com/blah.html")]
		[Row("http://example.com/example.com/blah.html", "www.example.com", "http://www.example.com/example.com/blah.html")]
		[Row("http://example.com/example.com/", "www.example.com", "http://www.example.com/example.com/")]
		public void CanReplaceHostInUrl(string url, string host, string expected)
		{
			Assert.AreEqual(expected, HtmlHelper.ReplaceHost(url, host));
		}

		[Test]
		[ExpectedArgumentNullException]
		public void AppendNullClassThrowsArgumentNullException()
		{
			HtmlHelper.AppendCssClass(new TextBox(), null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void AppendClassToNullControlThrowsArgumentNullException()
		{
			HtmlHelper.AppendCssClass(null, "blah");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void RemoveNullClassThrowsArgumentNullException()
		{
			HtmlHelper.RemoveCssClass(new TextBox(), null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void RemoveClassFromNullControlThrowsArgumentNullException()
		{
			HtmlHelper.RemoveCssClass(null, "blah");
		}
		
		[Test]
		public void RemoveClassFromControlWithNoClasHasNoEffect()
		{
			TextBox textbox = new TextBox();
			HtmlHelper.RemoveCssClass(textbox, "blah");
			Assert.AreEqual(string.Empty, textbox.CssClass);
		}
		
		[Test]
		public void CanAppendCssClassToControl()
		{
			TextBox textbox = new TextBox();
			HtmlHelper.AppendCssClass(textbox, "testclass");
			Assert.AreEqual("testclass", textbox.CssClass);

			HtmlHelper.AppendCssClass(textbox, "testclass");
			Assert.AreEqual("testclass", textbox.CssClass);

			HtmlHelper.AppendCssClass(textbox, "blah");
			Assert.AreEqual("testclass blah", textbox.CssClass);

			HtmlHelper.AppendCssClass(textbox, "BLAH");
			Assert.AreEqual("testclass blah BLAH", textbox.CssClass);
		}

		[Test]
		public void CanRemoveCssClassToControl()
		{
			TextBox textbox = new TextBox();
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
		[RowTest]
		[Row("", "")]
		[Row(null, null, ExpectedException=typeof(ArgumentNullException))]
		[Row("http://haacked.com/one/two/three/four/five/six/seven/eight/nine/ten.aspx", "<a rel=\"nofollow external\" href=\"http://haacked.com/one/two/three/four/five/six/seven/eight/nine/ten.aspx\" title=\"http://haacked.com/one/two/three/four/five/six/seven/eight/nine/ten.aspx\">haacked.com/.../ten.aspx</a>")]
		[Row("begin http://haacked.com/ end.", "begin <a rel=\"nofollow external\" href=\"http://haacked.com/\" title=\"http://haacked.com/\">http://haacked.com/</a> end.")]
		[Row("begin http://haacked.com/ two http://localhost/someplace/some.page.aspx end.", "begin <a rel=\"nofollow external\" href=\"http://haacked.com/\" title=\"http://haacked.com/\">http://haacked.com/</a> two <a rel=\"nofollow external\" href=\"http://localhost/someplace/some.page.aspx\" title=\"http://localhost/someplace/some.page.aspx\">http://localhost/someplace/some.page.aspx</a> end.")]
		[Row("this www.haacked.com", "this <a rel=\"nofollow external\" href=\"http://www.haacked.com\" title=\"www.haacked.com\">www.haacked.com</a>")]
		[Row("<p>www.haacked.com</p>", "<p><a rel=\"nofollow external\" href=\"http://www.haacked.com\" title=\"www.haacked.com\">www.haacked.com</a></p>")]
		[Row("<b>www.haacked.com</b>", "<b><a rel=\"nofollow external\" href=\"http://www.haacked.com\" title=\"www.haacked.com\">www.haacked.com</a></b>")]
		[Row("subtextproject.com", "subtextproject.com")]
		[Row("www.subtextproject.com?test=test&blah=blah", "<a rel=\"nofollow external\" href=\"http://www.subtextproject.com?test=test&amp;blah=blah\" title=\"www.subtextproject.com?test=test&amp;blah=blah\">www.subtextproject.com?test=test&amp;blah=blah</a>")]
		[Row("<a href=\"http://example.com/\">Test</a>", "<a href=\"http://example.com/\">Test</a>")]
		[Row("<img src=\"http://example.com/\" />", "<img src=\"http://example.com/\" />")]
		[Row("<a href='http://example.com/'>Test</a>", "<a href=\"http://example.com/\">Test</a>")]
		[Row("<a href=http://example.com/>Test</a>", "<a href=\"http://example.com/\">Test</a>")]
		[Row("<b title=\"blah http://example.com/ blah\" />", "<b title=\"blah http://example.com/ blah\" />")]
        [Row("a < b blah http://example.com/", "a &lt; b blah <a rel=\"nofollow external\" href=\"http://example.com/\" title=\"http://example.com/\">http://example.com/</a>")]
		[Row("www.haacked.com<a href=\"test\">test</a>", "<a rel=\"nofollow external\" href=\"http://www.haacked.com\" title=\"www.haacked.com\">www.haacked.com</a><a href=\"test\">test</a>")]
		public void ConvertUrlsToHyperLinksConvertsUrlsToAnchorTags(string html, string expected)
		{
			Assert.AreEqual(expected, HtmlHelper.ConvertUrlsToHyperLinks(html));
		}

		[Test]
		public void ConvertUrlToHyperlinksIgnoreAnchorContents()
		{
			string html = "<a href=\"/\"><b>http://example.com/</b></a>";
			Assert.AreEqual(html, HtmlHelper.ConvertUrlsToHyperLinks(html));
		}

        [Test]
        public void Html_WithEncodedMarkup_IsNotUnencoded()
        {
            string html = "&lt;script /&gt;";
            Assert.AreEqual(html, HtmlHelper.ConvertUrlsToHyperLinks(html));
        }

		[Test]
		public void CanApplyConverterWhileConvertingHtmlToXhtml()
		{
			string html = "<p title=\"blah blah\"> blah blah </p>";
			string expected = "<p title=\"blah blah\"> yadda yadda </p>";
			Converter<string, string> converter = delegate(string input)
			                                      	{
			                                      		return input.Replace("blah", "yadda");
			                                      	};

			
			Assert.AreEqual(expected, HtmlHelper.ConvertHtmlToXHtml(html, converter));
		}

		/// <summary>
		/// Makes sure that IsValidXHTML recognizes valid markup.
		/// </summary>
		[RowTest]
		[Row("This is some text", "This is some text")]
		[Row("<span>This is some text</span>", "<span>This is some text</span>")]
		[Row("<img src=\"blah\" />", "<img src=\"blah\" />")]
		[Row("<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>", "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
		public void ConvertHtmlToXHtmlLeavesValidMarkupAlone(string goodMarkup, string expected)
		{
			Entry entry = new Entry(PostType.BlogPost);
			entry.Body = goodMarkup;
			Assert.IsTrue(HtmlHelper.ConvertHtmlToXHtml(entry));
			Assert.AreEqual(expected, entry.Body);
		}

		[Test]
		public void ConvertHtmlToXHtmlLeavesNestedMarkupAlone()
		{
			string expected = "<p><span>This is some text</span> <span>this is more text</span></p>";
			Assert.AreEqual(expected, HtmlHelper.ConvertHtmlToXHtml(expected, null), "markup should not have changed");
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
			Assert.AreEqual(corrected, HtmlHelper.ConvertHtmlToXHtml(badMarkup, null));
		}

		[RowTest]
		[Row("<a name=\"test\"></a>", "<a name=\"test\"></a>", "Anchor tags should not be self-closed.")]
		[Row("<a name=\"test\" />", "<a name=\"test\"></a>", "Anchor tags should not be self-closed.")]
		[Row("<script src=\"test\" />", "<script src=\"test\"></script>", "Script tags should not be self-closed.")]
		[Row("<script src=\"test\"></script>", "<script src=\"test\"></script>", "Script tags should not be self-closed.")]
		public void ConvertHtmlToXhtmlEnsuresSomeTagsMustNotBeSelfClosed(string html, string expected, string message)
		{
			Assert.AreEqual(expected, HtmlHelper.ConvertHtmlToXHtml(html, null), message);
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
			string html = string.Format("<{0} src=\"blah-blah\"></{0}>", tag);
			string expected = string.Format("<{0} src=\"blah-blah\" />", tag);

			Assert.AreEqual(expected, HtmlHelper.ConvertHtmlToXHtml(html, null), tag + " tags must be self-closed");
			Assert.AreEqual(expected, HtmlHelper.ConvertHtmlToXHtml(expected, null), tag + " tags must be self-closed. We shouldn't change already closed.");
		}

		/// <summary>
		/// HasIllegalContent throws exception when encountering encoded tag.
		/// </summary>
		[RowTest]
		[Row("blah &#60script ", true)]
		[Row("blah <script ", true)]
		[Row("blah script ", false)]
		public void HasIllegalContentReturnsExpectedAnswer(string html, bool expected)
		{
			Assert.AreEqual(expected, HtmlHelper.HasIllegalContent(html));
		}

		[Test]
		public void CanParseTag()
		{
			List<string> tags = HtmlHelper.ParseTags("blah blah <a href=\"http://blah.com/subdir/mytag/\" rel=\"tag\">test1</a> goo goo");
			Assert.AreEqual(1, tags.Count, "Should have found one tag.");
			Assert.AreEqual("mytag", tags[0], "Should have found one tag.");

		}

		[Test]
		public void ParseTagsDoesNotParseDuplicates()
		{
			List<string> tags = HtmlHelper.ParseTags("<a href=\"http://blah.com/subdir/mytag/\" rel=\"tag\">test1</a><a href=\"http://blah.com/another-dir/mytag/\" rel=\"tag\">test2</a>");
			Assert.AreEqual(1, tags.Count, "The same tag exists twice, should only count as one.");
		}

		[Test]
		public void ParseTagsDoesNotMatchRelOfAnotherTag()
		{
			List<string> tags = HtmlHelper.ParseTags("<a title=\"blah\" href=\"http://blah.com/subdir/mytag1/\" " + Environment.NewLine + " rel=\"lightbox\">mytag1</a>other junk " + Environment.NewLine + "<a href=\"http://blah.com/another-dir/mytag2/\" rel=\"tag\">mytag2</a>");
			Assert.AreEqual(1, tags.Count, "The first anchor is not a tag.");
			Assert.AreEqual("mytag2", tags[0]);
		}

        [Test]
        public void ParseTagsWithWhitespaceAttributes()
        {
            List<string> tags = HtmlHelper.ParseTags("<a title=\"blah\" href = " + Environment.NewLine + " \"http://blah.com/subdir/mytag1/\" rel = " + Environment.NewLine + " \"tag\">mytag1</a>");
            Assert.AreEqual(1, tags.Count, "The attributes contain whitespace but should be recognized as valid");
            Assert.AreEqual("mytag1", tags[0]);
        }

		[Test]
		public void ParseTagsWithWeirdWhiteSpace()
		{
			List<string> tags = HtmlHelper.ParseTags("<a title=\"Programmer's Bill of Rights\" href=\"http://www.codinghorror.com/blog/archives/000666.html\">Programmer&rsquo;s Bill of Rights</a> that <a rel=\"friend met\" href=\"http://www.codinghorror.com/blog/\">Jeff Atwood</a>" + Environment.NewLine + "<div class=\"tags\">Technorati tags: <a rel=\"tag\" href=\"http://technorati.com/tag/Programming\">Programming</a>");
			Assert.AreEqual(1, tags.Count, "The attributes contain whitespace but should be recognized as valid");
			Assert.AreEqual("Programming", tags[0]);
		}

		[RowTest]
		[Row(" rel = \"tag\" ", " rel = \"tag\"", true)]
		[Row(" xrel = \"tag\" ", null, false)]
		[Row(" rel = \"friend tag\" ", " rel = \"friend tag\"", true)]
		[Row(" rel = \"friend tag met\" ", " rel = \"friend tag met\"", true)]
		[Row(" rel = \"tag met\" ", " rel = \"tag met\"", true)]
		[Row(" rel=\"friend met\"> rel=\"tag\" ", " rel=\"tag\"", true)]
		[Row(" rel = \'tag\' ", " rel = \'tag\'", true)]
		[Row(" xrel = \'tag\' ", null, false)]
		[Row(" rel = \'friend tag\' ", " rel = \'friend tag\'", true)]
		[Row(" rel = \'friend tag met\' ", " rel = \'friend tag met\'", true)]
		[Row(" rel = \'tag met\' ", " rel = \'tag met\'", true)]
		[Row(" rel=\'friend met\'> rel=\'tag\' ", " rel=\'tag\'", true)]
		public void CanParseRelTag(string original, string matched, bool expected)
		{
			Regex relRegex = new Regex(@"\s+rel\s*=\s*(""[^""]*?\btag\b.*?""|'[^']*?\btag\b.*?')", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = relRegex.Match(original);
			Assert.AreEqual(expected, match.Success);
			if(match.Success)
			{
				Assert.AreEqual(matched, match.Value);
			}
		}

		[RowTest]
		[Row("  <a href=\"foo\">test</a>  ", "<a href=\"foo\">test</a>", true)]
		[Row("  <a href=\"foo\" title=\"blah\">test</a>  ", "<a href=\"foo\" title=\"blah\">test</a>", true)]
		[Row("  <a href = \"foo\" >test</a>  ", "<a href = \"foo\" >test</a>", true)]
		[Row("  <span title=\"test <a href=\"> <a href=\"foo2\">test2</a>", "<a href=\"foo2\">test2</a>", true)]
		public void CanParseAnchorTags(string original, string expectedMatchValue, bool expectedMatch)
		{
			Regex regex = new Regex(@"<a(\s+\w+\s*=\s*(?:""[^""]*?""|'[^']*?')(?!\w))+\s*>.*?</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = regex.Match(original);
			Assert.AreEqual(expectedMatch, match.Success);
			if(match.Success)
			{
				string matchValue = match.Value;
				Assert.AreEqual(expectedMatchValue, matchValue);
			}
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			//Confirm app settings
            UnitTestHelper.AssertAppSettings();
		}

		[SetUp]
		public void SetUp()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "MyBlog");
			Blog blogInfo = new Blog();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "MyBlog";

			HttpContext.Current.Items.Add("BlogInfo-", blogInfo);
		}

		[TearDown]
		public void TearDown()
		{
			HttpContext.Current = null;
		}
	}
}


