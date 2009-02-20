using System;
using System.Linq;
using MbUnit.Framework;
using Subtext.Framework.Text;
using System.Collections;
using System.Collections.Generic;

namespace UnitTests.Subtext.Framework.Text
{
	/// <summary>
	/// Summary description for StringHelperTests.
	/// </summary>
	[TestFixture]
	public class StringHelperTests
	{
        [Test]
        public void Remove_PassingInTextWithRepeatingSequenceAndOccurrenceCountOfOne_RemovesFirstOccurrence()
        {
            //act
            string result = "foo/bar/foo".Remove("Foo", 1, StringComparison.OrdinalIgnoreCase);

            //assert
            Assert.AreEqual("/bar/foo", result);
        }

        [Test]
        public void Remove_PassingInTextWithRepeatingSequenceAndOccurrenceCountOfTwo_RemovesAllOccurrences()
        {
            //act
            string result = "foo/bar/foo".Remove("Foo", 2, StringComparison.OrdinalIgnoreCase);

            //assert
            Assert.AreEqual("/bar/", result);
        }

        [Test]
        public void Remove_PassingInTextWithRepeatingSequenceAndOccurrenceCountOfFour_RemovesAllOccurrences()
        {
            //act
            string result = "foo/bar/foo".Remove("Foo", 4, StringComparison.OrdinalIgnoreCase);

            //assert
            Assert.AreEqual("/bar/", result);
        }

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

        [Test]
        public void SplitIntoWords_WithStringContainingSpaces_SplitsIntoWords() { 
            //arrange, act
            IEnumerable<string> words = "this is a test".SplitIntoWords().ToList();
            
            //assert
            Assert.AreEqual(4, words.Count());
            Assert.AreEqual("this", words.First());
            Assert.AreEqual("is", words.ElementAt(1));
            Assert.AreEqual("a", words.ElementAt(2));
            Assert.AreEqual("test", words.ElementAt(3));
        }

        [Test]
        public void SplitIntoWords_WithStringContainingTabsAndDoubleSpaces_SplitsIntoWords()
        {
            //arrange, act
            IEnumerable<string> words = "  this \t is\ta  test  \t".SplitIntoWords().ToList();

            //assert
            Assert.AreEqual(4, words.Count());
            Assert.AreEqual("this", words.First());
            Assert.AreEqual("is", words.ElementAt(1));
            Assert.AreEqual("a", words.ElementAt(2));
            Assert.AreEqual("test", words.ElementAt(3));
        }
	}
}
