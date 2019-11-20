using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
    /// <summary>
    /// Summary description for StringHelperTests.
    /// </summary>
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void Remove_PassingInTextWithRepeatingSequenceAndOccurrenceCountOfOne_RemovesFirstOccurrence()
        {
            //act
            string result = "foo/bar/foo".Remove("Foo", 1, StringComparison.OrdinalIgnoreCase);

            //assert
            Assert.AreEqual("/bar/foo", result);
        }

        [TestMethod]
        public void Remove_PassingInTextWithRepeatingSequenceAndOccurrenceCountOfTwo_RemovesAllOccurrences()
        {
            //act
            string result = "foo/bar/foo".Remove("Foo", 2, StringComparison.OrdinalIgnoreCase);

            //assert
            Assert.AreEqual("/bar/", result);
        }

        [TestMethod]
        public void Remove_PassingInTextWithRepeatingSequenceAndOccurrenceCountOfFour_RemovesAllOccurrences()
        {
            //act
            string result = "foo/bar/foo".Remove("Foo", 4, StringComparison.OrdinalIgnoreCase);

            //assert
            Assert.AreEqual("/bar/", result);
        }

        [DataTestMethod]
        [DataRow("Blah..Blah", '.', "Blah.Blah")]
        [DataRow("Blah...Blah", '.', "Blah.Blah")]
        [DataRow("Blah....Blah", '.', "Blah.Blah")]
        [DataRow("Blah- -Blah", '-', "Blah- -Blah")]
        [DataRow("Blah--Blah", '.', "Blah--Blah")]
        public void CanRemoveDoubleCharacter(string text, char character, string expected)
        {
            Assert.AreEqual(expected, text.RemoveDoubleCharacter(character));
        }

        [TestMethod]
        public void RemoveDoubleCharacter_WithNullCharacter_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(
                () => "6 bdy.RemoveDoubleCharacter(e)".RemoveDoubleCharacter(Char.MinValue)
            );
        }


        /// <summary>
        /// Tests that we can properly pascal case text.
        /// </summary>
        /// <remarks>
        /// Does not remove punctuation.
        /// </remarks>
        /// <param name="original"></param>
        /// <param name="expected"></param>
        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("a", "A")]
        [DataRow("A", "A")]
        [DataRow("A B", "AB")]
        [DataRow("a bee keeper's dream.", "ABeeKeeper'sDream.")]
        public void PascalCaseTests(string original, string expected)
        {
            Assert.AreEqual(expected, original.ToPascalCase());
        }

        [TestMethod]
        public void PascalCaseThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() =>
                StringHelper.ToPascalCase(null)
            );
        }

        [DataTestMethod]
        [DataRow("BLAH Tast", "a", 6, StringComparison.Ordinal)]
        [DataRow("BLAH Tast", "a", 2, StringComparison.OrdinalIgnoreCase)]
        public void IndexOfHandlesCaseSensitivity(string source, string search, int expectedIndex,
                                                  StringComparison comparison)
        {
            Assert.AreEqual(expectedIndex, source.IndexOf(search, comparison),
                            "Did not find the string '{0}' at the index {1}", search, expectedIndex);
        }

        [DataTestMethod]
        [DataRow("Blah/Default.aspx", "Default.aspx", "Blah/", StringComparison.Ordinal)]
        [DataRow("Blah/Default.aspx", "default.aspx", "Blah/", StringComparison.OrdinalIgnoreCase)]
        [DataRow("Blah/Default.aspx", "default.aspx", "Blah/Default.aspx", StringComparison.Ordinal)]
        public void LeftBeforeOfHandlesCaseSensitivity(string source, string search, string expected,
                                                       StringComparison comparison)
        {
            Assert.AreEqual(expected, source.LeftBefore(search, comparison),
                            "Truncating did not return the correct result.");
        }

        [TestMethod]
        public void SplitIntoWords_WithStringContainingSpaces_SplitsIntoWords()
        {
            //arrange, act
            IEnumerable<string> words = "this is a test".SplitIntoWords().ToList();

            //assert
            Assert.AreEqual(4, words.Count());
            Assert.AreEqual("this", words.First());
            Assert.AreEqual("is", words.ElementAt(1));
            Assert.AreEqual("a", words.ElementAt(2));
            Assert.AreEqual("test", words.ElementAt(3));
        }

        [TestMethod]
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


        /*
               "string\r\n".chop   #=> "string"
               "string\n\r".chop   #=> "string\n"
               "string\n".chop     #=> "string"
               "string".chop       #=> "strin"
               "x".chop.chop       #=> ""
             */

        [TestMethod]
        public void Chop_WithStringEndingWithWindowsNewLine_ReturnsStringWithoutNewline()
        {
            Assert.AreEqual("string", "string\r\n".Chop());
        }

        [TestMethod]
        public void Chop_WithStringEndingWithSlashR_OnlyChopsSlashR()
        {
            Assert.AreEqual("string\n", "string\n\r".Chop());
        }

        [TestMethod]
        public void Chop_WithStringEndingWithNewline_ChopsNewline()
        {
            Assert.AreEqual("string", "string\n".Chop());
        }

        [TestMethod]
        public void Chop_WithStringEndingWithLetter_ReturnsStringWithoutLastLetter()
        {
            Assert.AreEqual("strin", "string".Chop());
        }

        [TestMethod]
        public void Chop_WithOneLetter_ReturnsEmptyString()
        {
            Assert.AreEqual(string.Empty, "x".Chop());
        }

        /*
         "hello".chomp            #=> "hello"
         "hello\n".chomp          #=> "hello"
         "hello\r\n".chomp        #=> "hello"
         "hello\n\r".chomp        #=> "hello\n"
         "hello\r".chomp          #=> "hello"
         "hello \n there".chomp   #=> "hello \n there"
         "hello".chomp("llo")     #=> "he"
         */

        [TestMethod]
        public void Chomp_WithStringNotEndingWithDefaultSeparator_ReturnsString()
        {
            Assert.AreEqual("hello", "hello".Chomp());
        }

        [TestMethod]
        public void Chomp_WithStringEndingWithNewline_ChopsNewline()
        {
            Assert.AreEqual("hello", "hello\n".Chop());
        }

        [TestMethod]
        public void Chomp_WithStringEndingWithWindowsNewLine_ReturnsStringWithoutNewline()
        {
            Assert.AreEqual("hello", "hello\r\n".Chomp());
        }

        [TestMethod]
        public void Chomp_WithStringEndingWithSlashNSlashR_OnlyChopsSlashR()
        {
            Assert.AreEqual("hello\n", "hello\n\r".Chop());
        }

        [TestMethod]
        public void Chomp_WithStringEndingWithSlashR_OnlyChopsSlashR()
        {
            Assert.AreEqual("hello", "hello\r".Chop());
        }

        [TestMethod]
        public void Chomp_WithSeparator_ChopsSeparator()
        {
            Assert.AreEqual("he", "hello".Chomp("llo", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Chomp_WithSeparatorButStringNotEndingWithSeparator_LeavesStringAlone()
        {
            Assert.AreEqual("hello world", "hello world".Chomp("llo", StringComparison.Ordinal));
        }
    }
}