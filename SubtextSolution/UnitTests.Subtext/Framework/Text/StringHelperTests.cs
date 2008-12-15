using System;
using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
	/// <summary>
	/// Summary description for StringHelperTests.
	/// </summary>
	[TestFixture]
	public class StringHelperTests
	{
		[RowTest]
		[Row(null, char.MinValue, null, ExpectedException = typeof(ArgumentNullException))]
		[Row("Blah..Blah", '.', "Blah.Blah")]
		[Row("Blah...Blah", '.', "Blah.Blah")]
		[Row("Blah....Blah", '.', "Blah.Blah")]
		[Row("Blah- -Blah", '-', "Blah- -Blah")]
		[Row("Blah--Blah", '.', "Blah--Blah")]
		public void CanRemoveDoubleCharacter(string text, char character, string expected)
		{
			Assert.AreEqual(expected, StringHelper.RemoveDoubleCharacter(text, character));
		}

		/// <summary>
		/// Tests that we can properly pascal case text.
		/// </summary>
		/// <remarks>
		/// Does not remove punctuation.
		/// </remarks>
		/// <param name="original"></param>
		/// <param name="expected"></param>
		[RowTest]
		[Row("", "")]
		[Row("a", "A")]
		[Row("A", "A")]
		[Row("A B", "AB")]
		[Row("a bee keeper's dream.", "ABeeKeeper'sDream.")]
		public void PascalCaseTests(string original, string expected)
		{
			Assert.AreEqual(expected, original.ToPascalCase());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PascalCaseThrowsArgumentNullException()
		{
			StringHelper.ToPascalCase(null);
		}
		
		[RowTest]
		[Row("BLAH Tast", "a", 6, StringComparison.Ordinal)]
		[Row("BLAH Tast", "a", 2, StringComparison.InvariantCultureIgnoreCase)]
		public void IndexOfHandlesCaseSensitivity(string source, string search, int expectedIndex, StringComparison comparison)
		{
			Assert.AreEqual(expectedIndex, source.IndexOf(search, comparison), "Did not find the string '{0}' at the index {1}", search, expectedIndex);
		}
		
		[RowTest]
		[Row("Blah/Default.aspx", "Default.aspx", "Blah/", StringComparison.Ordinal)]
		[Row("Blah/Default.aspx", "default.aspx", "Blah/", StringComparison.InvariantCultureIgnoreCase)]
		[Row("Blah/Default.aspx", "default.aspx", "Blah/Default.aspx", StringComparison.Ordinal)]
		public void LeftBeforeOfHandlesCaseSensitivity(string source, string search, string expected, StringComparison comparison)
		{
			Assert.AreEqual(expected, StringHelper.LeftBefore(source, search, comparison), "Truncating did not return the correct result.");
		}
	}
}
