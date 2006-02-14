using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
	/// <summary>
	/// Unit tests of the <see cref="HtmlHelper.ConvertToAllowedHtml"/> method and 
	/// just that method.
	/// </summary>
	[TestFixture]
	public class ConvertToAllowedHtmlTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ThrowsArgumentNullExceptionWhenNullTextIsPassed()
		{
			HtmlHelper.ConvertToAllowedHtml(null);
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
		[Row(">", "&gt;")]
		[Row("\r\r\n", "<br />")]
		[Row("How now brown cow.", "How now brown cow.")]
		[Row("How now brown cow.", "How now brown cow.")]
		[Row("<a href='test'>a</a>", "<a href='test'>a</a>")]
		[Row("<a href=\"test\">a</a>", "<a href=\"test\">a</a>")]
		[Row("<a href=\"test\" rel=\"notallowed\">a</a>", "<a href=\"test\">a</a>")]
		[Row("<a title=\">\">a</a>", "<a title=\">\">a</a>")]
		[Row("<a\r\ntitle=\">\">a</a>", "<a\r\ntitle=\">\">a</a>")]
		public void StripsNonAllowedHtml(string text, string expected)
		{
			NameValueCollection allowedTags = new NameValueCollection();
			allowedTags.Add("a", "href,title");
			Assert.AreEqual(expected, HtmlHelper.ConvertToAllowedHtml(allowedTags, text));
		}

	}
}
