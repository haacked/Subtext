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
using System.Web.UI.WebControls;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Exceptions;
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
		[Test]
		public void EnableUrlsFormatsUrlsWithAnchorTags()
		{
			string html = "this is text with http://haacked.com/ one url.";
			string expected = "this is text with <a rel=\"nofollow external\" href=\"http://haacked.com/\">http://haacked.com/</a> one url.";

			Assert.AreEqual(expected, HtmlHelper.ConvertUrlsToHyperLinks(html));

			html = "this is text with http://haacked.com/ two http://localhost/someplace/some.page.aspx urls.";
			expected = "this is text with <a rel=\"nofollow external\" href=\"http://haacked.com/\">http://haacked.com/</a> two <a rel=\"nofollow external\" href=\"http://localhost/someplace/some.page.aspx\">http://localhost/someplace/some.page.aspx</a> urls.";

			Assert.AreEqual(expected, HtmlHelper.ConvertUrlsToHyperLinks(html));
		}

		/// <summary>
		/// Makes sure that IsValidXHTML recognizes valid markup.
		/// </summary>
		[RowTest]
		[Row("This is some text", "This is some text")]
		[Row("<span>This is some text</span>", "<span>This is some text</span>")]
		[Row("<p><span>This is some text</span> <span>this is more text</span></p>", "<p><span>This is some text</span> <span>this is more text</span></p>")]
		[Row("<img src=\"blah\" />", "<img src=\"blah\" />")]
		[Row("<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>", "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
		public void ConvertHtmlToXHtmlLeavesValidMarkupAlone(string goodMarkup, string expected)
		{
			Entry entry = new Entry(PostType.BlogPost);
			entry.Body = goodMarkup;
			HtmlHelper.ConvertHtmlToXHtml(entry);
			Assert.AreEqual(expected, entry.Body);
		}

		/// <summary>
		/// Makes sure that IsValidXHTML recognizes invalid markup.
		/// </summary>
		[RowTest]
		[Row("This <br /><br />is bad <p> XHTML.", "This <br /><br />is bad <p> XHTML.</p>")]
		[Row("This <P>is bad </P> XHTML.", "This <p>is bad </p> XHTML.")]
		[Row("<style type=\"text/css\">\r\n<![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>", "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
		[Row("<style type=\"text/css\">\r\n\r\n<![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>", "<style type=\"text/css\"><![CDATA[\r\n.blah\r\n{\r\n  font-size: small;\r\n}\r\n]]></style>")]
		public void ConvertHtmlToXHtmlCorrectsInvalidMarkup(string badMarkup, string corrected)
		{
			Entry entry = new Entry(PostType.BlogPost);
			entry.Body = badMarkup;
			HtmlHelper.ConvertHtmlToXHtml(entry);
			Assert.AreEqual(corrected, entry.Body);
		}

		/// <summary>
		/// HasIllegalContent throws exception when encountering script tag.
		/// </summary>
		[Test, ExpectedException(typeof(IllegalPostCharactersException))]
		public void HasIllegalContentThrowsExceptionWithScriptTag()
		{
			HtmlHelper.HasIllegalContent("blah <script ");
		}

		/// <summary>
		/// HasIllegalContent throws exception when encountering encoded tag.
		/// </summary>
		[Test, ExpectedException(typeof(IllegalPostCharactersException))]
		public void HasIllegalContentThrowsExceptionWithEncodedScriptTag()
		{
			try
			{
				HtmlHelper.HasIllegalContent("blah &#60script ");
				Assert.Fail("Method should have thrown an exception");
			}
			catch(IllegalPostCharactersException)
			{
				
			}
			catch(Exception)
			{
				Assert.Fail("Method should have thrown an IllegalPostCharactersException exception");
			}

			try
			{
				HtmlHelper.HasIllegalContent("blah &60script ");
				Assert.Fail("Method should have thrown an exception");
			}
			catch(IllegalPostCharactersException)
			{
				
			}
			catch(Exception)
			{
				Assert.Fail("Method should have thrown an IllegalPostCharactersException exception");
			}

			HtmlHelper.HasIllegalContent("blah %60script ");
		}

		[Test]
		public void CanParseTag()
		{
			IList<string> tags = HtmlHelper.ParseTags("blah blah <a href=\"http://blah.com/subdir/mytag/\" rel=\"tag\">test1</a> goo goo");
			Assert.AreEqual(1, tags.Count, "Should have found one tag.");
			Assert.AreEqual("mytag", tags[0], "Should have found one tag.");

		}

		[Test]
		public void ParseTagsDoesNotParseDuplicates()
		{
			IList<string> tags = HtmlHelper.ParseTags("<a href=\"http://blah.com/subdir/mytag/\" rel=\"tag\">test1</a><a href=\"http://blah.com/another-dir/mytag/\" rel=\"tag\">test2</a>");
			Assert.AreEqual(1, tags.Count, "The same tag exists twice, should only count as one.");
		}

		[Test]
		public void ParseTagsDoesNotMatchRelOfAnotherTag()
		{
			IList<string> tags = HtmlHelper.ParseTags("<a title=\"blah\" href=\"http://blah.com/subdir/mytag1/\" " + Environment.NewLine + " rel=\"lightbox\">mytag1</a>other junk " + Environment.NewLine + "<a href=\"http://blah.com/another-dir/mytag2/\" rel=\"tag\">mytag2</a>");
			Assert.AreEqual(1, tags.Count, "The first anchor is not a tag.");
			Assert.AreEqual("mytag2", tags[0]);
		}

        [Test]
        public void ParseTagsWithWhitespaceAttributes()
        {
            IList<string> tags = HtmlHelper.ParseTags("<a title=\"blah\" href = " + Environment.NewLine + " \"http://blah.com/subdir/mytag1/\" rel = " + Environment.NewLine + " \"tag\">mytag1</a>");
            Assert.AreEqual(1, tags.Count, "The attributes contain whitespace but should be recognized as valid");
            Assert.AreEqual("mytag1", tags[0]);
        }

		[Test]
		public void ParseTagsWithWeirdWhiteSpace()
		{
			IList<string> tags = HtmlHelper.ParseTags("<a title=\"Programmer's Bill of Rights\" href=\"http://www.codinghorror.com/blog/archives/000666.html\">Programmer&rsquo;s Bill of Rights</a> that <a rel=\"friend met\" href=\"http://www.codinghorror.com/blog/\">Jeff Atwood</a>" + Environment.NewLine + "<div class=\"tags\">Technorati tags: <a rel=\"tag\" href=\"http://technorati.com/tag/Programming\">Programming</a>");
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

		private IDisposable blogRequest;

		[SetUp]
		public void SetUp()
		{			
			BlogInfo blogInfo = new BlogInfo();
			blogInfo.Host = "localhost";
			blogInfo.Subfolder = "MyBlog";

			blogRequest = BlogRequestSimulator.SimulateRequest(blogInfo, "localhost", "", "MyBlog");
		}

		[TearDown]
		public void TearDown()
		{
			if(blogRequest != null)
				blogRequest.Dispose();
		}
	}
}


