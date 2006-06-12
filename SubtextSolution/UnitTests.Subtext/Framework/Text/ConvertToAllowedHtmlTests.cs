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
		[Row("<a title=\">\">a</a>", "<a title=\">\">a</a>")]
		[Row("<A TITLE=\">\">a</a>", "<a title=\">\">a</a>")]
		[Row("<a\r\ntitle=\">\">a</a>", "<a title=\">\">a</a>")]
		[Row("<a href='test'></a>", "<a href=\"test\"></a>")]
		[Row("<a href=test></a>", "<a href=\"test\"></a>")]
		[Row("<a href=test title=\"cool\"></a>", "<a href=\"test\" title=\"cool\"></a>")]
		[Row("<a href=test title=cool></a>", "<a href=\"test\" title=\"cool\"></a>")]
		[Row("<a title></a>", "<a></a>")]
		[Row("<a title href=\"test\"></a>", "<a href=\"test\"></a>")]
		[Row("<a title href=\"test\" title></a>", "<a href=\"test\"></a>")]
		[Row("<a title href=\"test\" title title title></a>", "<a href=\"test\"></a>")]
		[Row("<a title=\"one\" title=\"two\"></a>", "<a title=\"one,two\"></a>")]
		public void StripsNonAllowedHtml(string text, string expected)
		{
			//NameValueCollection allowedTags = new NameValueCollection(new System.Collections.CaseInsensitiveHashCodeProvider(), new System.Collections.CaseInsensitiveComparer());
            NameValueCollection allowedTags = new NameValueCollection(StringComparer.InvariantCultureIgnoreCase);
			allowedTags.Add("a", "href,title");
			allowedTags.Add("strong", "");
			UnitTestHelper.AssertStringsEqualCharacterByCharacter(expected, HtmlHelper.ConvertToAllowedHtml(allowedTags, text));
		}

	}
}