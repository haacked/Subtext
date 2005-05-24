using System;
using NUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
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
		[Test]
		public void ConvertHtmlToXHtmlLeavesValidMarkupAlone()
		{
			Config.CurrentBlog.FullyQualifiedUrl = "http://localhost/";
			Entry entry = new Entry(PostType.BlogPost);
			entry.Body = "This is some text";
			Assert.IsTrue(HtmlHelper.ConvertHtmlToXHtml(ref entry));
			Assert.AreEqual("This is some text", entry.Body);
		}

		/// <summary>
		/// Makes sure that IsValidXHTML recognizes invalid markup.
		/// </summary>
		[Test]
		public void ConvertHtmlToXHtmlCorrectsInvalidMarkup()
		{
			Config.CurrentBlog.FullyQualifiedUrl = "http://localhost/";
			Entry entry = new Entry(PostType.BlogPost);
			entry.Body = "This <br><br>is bad <p> XHTML.";
			Assert.IsTrue(HtmlHelper.ConvertHtmlToXHtml(ref entry));
			Assert.AreEqual("This <br /><br />is bad <p> XHTML.</p>", entry.Body);
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

		[SetUp]
		public void SetUp()
		{
			Config.ConfigurationProvider = new UnitTestConfigProvider();

			//This file needs to be there already.
			UnitTestHelper.UnpackEmbeddedResource("App.config", "UnitTests.Subtext.dll.config");
			
			//Confirm app settings
			Assert.AreEqual("~/Admin/Resources/PageTemplate.ascx", System.Configuration.ConfigurationSettings.AppSettings["Admin.DefaultTemplate"]) ;
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
