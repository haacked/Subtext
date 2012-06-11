using System;
using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
	/// <summary>
	/// Unit tests of the <see cref="HtmlHelper.RemoveHtmlComments"/> method and 
	/// just that method.
	/// </summary>
	[TestFixture]
	public class RemoveCommentsTests
	{

		/// <summary>
		/// Passes in each row to the test. Calls <see cref="HtmlHelper.RemoveHtmlComments"/> on 
		/// the input and compares against the expected value.
		/// </summary>
		/// <param name="input">text to strip comments from.</param>
		/// <param name="expected">Expected return value.</param>
		[RowTest]
		[Row("", "")]
		[Row(null, null)]
		[Row("<!--", "<!--")]
		[Row("-->", "-->")]
		[Row("&lt;!--", "&lt;!--")]
		[Row("--&gt;", "--&gt;")]
		[Row("aBc", "aBc")]
		[Row("<!--Comment text-->String","String")]
		[Row("<!--Comment --&gt; text-->String", "String")]
		[Row("&lt;!--Comment text-->String", "&lt;!--Comment text-->String")]
		[Row("<!--Comment text--&gt;String", "<!--Comment text--&gt;String")]
		public void RemoveCommentsStripsHtmlCommentsProperly(string input, string expected)
		{
			Assert.AreEqual(expected, HtmlHelper.RemoveHtmlComments(input), "Did not strip comments from [" + input + "] properly.");
		}
	}
}
