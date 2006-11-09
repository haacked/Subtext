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

			Assert.AreEqual(expected, HtmlHelper.EnableUrls(html));

			html = "this is text with http://haacked.com/ two http://localhost/someplace/some.page.aspx urls.";
			expected = "this is text with <a rel=\"nofollow external\" href=\"http://haacked.com/\">http://haacked.com/</a> two <a rel=\"nofollow external\" href=\"http://localhost/someplace/some.page.aspx\">http://localhost/someplace/some.page.aspx</a> urls.";

			Assert.AreEqual(expected, HtmlHelper.EnableUrls(html));
		}

		/// <summary>
		/// Makes sure that IsValidXHTML recognizes valid markup.
		/// </summary>
		[RowTest]
		[Row("This is some text", "This is some text")]
		[Row("<span>This is some text</span>", "<span>This is some text</span>")]
		[Row("<p><span>This is some text</span> <span>this is more text</span></p>", "<p><span>This is some text</span> <span>this is more text</span></p>")]
		[Row("<img src=\"blah\" />", "<img src=\"blah\" />")]
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
			HtmlHelper.CheckForIllegalContent("blah <script ");
		}

		/// <summary>
		/// HasIllegalContent throws exception when encountering encoded tag.
		/// </summary>
		[Test, ExpectedException(typeof(IllegalPostCharactersException))]
		public void HasIllegalContentThrowsExceptionWithEncodedScriptTag()
		{
			try
			{
				HtmlHelper.CheckForIllegalContent("blah &#60script ");
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
				HtmlHelper.CheckForIllegalContent("blah &60script ");
				Assert.Fail("Method should have thrown an exception");
			}
			catch(IllegalPostCharactersException)
			{
				
			}
			catch(Exception)
			{
				Assert.Fail("Method should have thrown an IllegalPostCharactersException exception");
			}

			HtmlHelper.CheckForIllegalContent("blah %60script ");
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
			BlogInfo blogInfo = new BlogInfo();
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
