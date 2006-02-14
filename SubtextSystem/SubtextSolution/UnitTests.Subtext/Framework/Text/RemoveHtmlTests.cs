using System;
using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
	/// <summary>
	/// Summary description for RemoveHtmlTests.
	/// </summary>
	[TestFixture]
	public class RemoveHtmlTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveHtmlThrowsArgumentNullException()
		{
			HtmlHelper.RemoveHtml(null);
		}

		/// <summary>
		/// Passes in each row to the test. Calls <see cref="HtmlHelper.RemoveHtml"/> on 
		/// the input and compares against the expected value.
		/// </summary>
		/// <param name="input">text to strip html from.</param>
		/// <param name="expected">Expected return value.</param>
		[RowTest]
		[Row("", "")]
		[Row("<", "<")]
		[Row("<>", "<>")]
		[Row("aBc", "aBc")]
		[Row("this <strong>rocks!</strong>", "this rocks!")]
		[Row("this <a title='>'>rocks!</a> and rolls", "this rocks! and rolls")]
		[Row("this <A title='>'>rocks!</A> and rolls", "this rocks! and rolls")]
		public void RemoveHtmlStripsHtmlProperly(string input, string expected)
		{
			Assert.AreEqual(expected, HtmlHelper.RemoveHtml(input), "Did not strip html from [" + input + "] properly.");
		}
	}
}
