using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
    [TestClass]
    public class NamedFormatTests
    {
        [MultipleCultureTestMethod("en-US,en-NZ,it-IT")]
        public void StringFormat_WithMultipleExpressions_FormatsThemAll()
        {
            //arrange
            var o = new {foo = 123.45, bar = 42, baz = "hello"};

            //act
            string result = "{foo} {foo} {bar}{baz}".NamedFormat(o);

            //assert
            float expectedNum = 123.45f;
            string expected = String.Format("{0} {1} 42hello", expectedNum, expectedNum);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StringFormat_WithDoubleEscapedCurlyBraces_DoesNotFormatString()
        {
            //arrange
            var o = new {foo = 123.45};

            //act
            string result = "{{{{foo}}}}".NamedFormat(o);

            //assert
            Assert.AreEqual("{{foo}}", result);
        }

        [MultipleCultureTestMethod("en-US,en-NZ,it-IT")]
        public void StringFormat_WithFormatSurroundedByDoubleEscapedBraces_FormatsString()
        {
            //arrange
            var o = new {foo = 123.45};

            //act
            string result = "{{{{{foo}}}}}".NamedFormat(o);

            //assert
            float expected = 123.45f;
            Assert.AreEqual("{{" + expected + "}}", result);
        }

        [MultipleCultureTestMethod("en-US,en-NZ,it-IT")]
        public void Format_WithEscapeSequence_EscapesInnerCurlyBraces()
        {
            var o = new {foo = 123.45};

            //act
            string result = "{{{foo}}}".NamedFormat(o);

            //assert
            float expected = 123.45f;
            Assert.AreEqual("{" + expected + "}", result);
        }

        [TestMethod]
        public void Format_WithEmptyString_ReturnsEmptyString()
        {
            var o = new {foo = 123.45};

            //act
            string result = string.Empty.NamedFormat(o);

            //assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Format_WithNoFormats_ReturnsFormatStringAsIs()
        {
            var o = new {foo = 123.45};

            //act
            string result = "a b c".NamedFormat(o);

            //assert
            Assert.AreEqual("a b c", result);
        }

        [MultipleCultureTestMethod("en-US,en-NZ,it-IT")]
        public void Format_WithFormatType_ReturnsFormattedExpression()
        {
            var o = new {foo = 123.45};

            //act
            string result = "{foo:#.#}".NamedFormat(o);

            //assert
            float expected = 123.5f;
            Assert.AreEqual(expected.ToString(), result);
        }

        [MultipleCultureTestMethod("en-US,en-NZ,it-IT")]
        public void Format_WithSubProperty_ReturnsValueOfSubProperty()
        {
            var o = new {foo = new {bar = 123.45}};

            //act
            string result = "{foo.bar:#.#}ms".NamedFormat(o);

            //assert
            float expected = 123.5f;
            Assert.AreEqual(expected + "ms", result);
        }

        [TestMethod]
        public void Format_WithFormatNameNotInObject_ThrowsFormatException()
        {
            //arrange
            var o = new {foo = 123.45};

            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{bar}".NamedFormat(o));
        }

        [TestMethod]
        public void Format_WithNoEndFormatBrace_ThrowsFormatException()
        {
            //arrange
            var o = new {foo = 123.45};

            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{bar".NamedFormat(o));
        }

        [TestMethod]
        public void Format_WithEscapedEndFormatBrace_ThrowsFormatException()
        {
            //arrange
            var o = new {foo = 123.45};


            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{foo}}".NamedFormat(o));
        }

        [TestMethod]
        public void Format_WithDoubleEscapedEndFormatBrace_ThrowsFormatException()
        {
            //arrange
            var o = new {foo = 123.45};

            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{foo}}}}bar".NamedFormat(o));
        }

        [TestMethod]
        public void Format_WithDoubleEscapedEndFormatBraceWhichTerminatesString_ThrowsFormatException()
        {
            //arrange
            var o = new {foo = 123.45};

            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{foo}}}}".NamedFormat(o));
        }

        [MultipleCultureTestMethod("en-US,en-NZ,it-IT")]
        public void Format_WithEndBraceFollowedByEscapedEndFormatBraceWhichTerminatesString_FormatsCorrectly()
        {
            var o = new {foo = 123.45};

            //act
            string result = "{foo}}}".NamedFormat(o);

            //assert
            float expected = 123.45f;
            Assert.AreEqual(expected + "}", result);
        }

        [MultipleCultureTestMethod("en-US,en-NZ,it-IT")]
        public void Format_WithEndBraceFollowedByEscapedEndFormatBrace_FormatsCorrectly()
        {
            var o = new {foo = 123.45};

            //act
            string result = "{foo}}}bar".NamedFormat(o);

            //assert
            float expected = 123.45f;
            Assert.AreEqual(expected + "}bar", result);
        }

        [MultipleCultureTestMethod("en-US,en-NZ,it-IT")]
        public void Format_WithEndBraceFollowedByDoubleEscapedEndFormatBrace_FormatsCorrectly()
        {
            var o = new {foo = 123.45};

            //act
            string result = "{foo}}}}}bar".NamedFormat(o);

            //assert
            float expected = 123.45f;
            Assert.AreEqual(expected + "}}bar", result);
        }

        [TestMethod]
        public void Format_WithNullFormatString_ThrowsArgumentNullException()
        {
            //arrange, act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => ((string)null).NamedFormat(123));
        }
    }
}