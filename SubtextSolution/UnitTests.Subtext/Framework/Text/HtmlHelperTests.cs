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
		/// <summary>
		/// Tests that EnableUrls formats urls with anchor tags.
		/// </summary>
		[Test]
		public void EnableUrlsFormatsUrlsWithAnchorTags()
		{
			string html = "this is text with http://haacked.com/ one url.";
			string expected = "this is text with <a rel=\"nofollow\" target=\"_new\" href=\"http://haacked.com/\">http://haacked.com/</a> one url.";

			Assert.AreEqual(expected, HtmlHelper.EnableUrls(html));

			html = "this is text with http://haacked.com/ two http://localhost/someplace/some.page.aspx urls.";
			expected = "this is text with <a rel=\"nofollow\" target=\"_new\" href=\"http://haacked.com/\">http://haacked.com/</a> two <a rel=\"nofollow\" target=\"_new\" href=\"http://localhost/someplace/some.page.aspx\">http://localhost/someplace/some.page.aspx</a> urls.";

			Assert.AreEqual(expected, HtmlHelper.EnableUrls(html));
		}

		/// <summary>
		/// Makes sure that IsValidXHTML recognizes valid markup.
		/// </summary>
		[RowTest]
		[Row("This is some text", "This is some text")]
		[Row("<span>This is some text</span>", "<span>This is some text</span>")]
		[Row("<span>&#8220;</span>", "<span>&#8220;</span>")]
		public void ConvertHtmlToXHtmlLeavesValidMarkupAlone(string goodMarkup, string expected)
		{
			Entry entry = new Entry(PostType.BlogPost);
			entry.Body = goodMarkup;
			Assert.IsTrue(HtmlHelper.ConvertHtmlToXHtml(entry));
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
			Assert.IsTrue(HtmlHelper.ConvertHtmlToXHtml(entry));
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

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
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
