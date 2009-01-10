using System;
using Subtext.Framework.Text;
using MbUnit.Framework;
using UnitTests.Subtext;

namespace UnitTests
{
    [TestFixture]
    public class NamedFormatTests
    {
        [Test]
        public void StringFormat_WithMultipleExpressions_FormatsThemAll()
        {
            //arrange
            var o = new { foo = 123.45, bar = 42, baz = "hello" };

            //act
            string result = "{foo} {foo} {bar}{baz}".NamedFormat(o);

            //assert
            Assert.AreEqual("123.45 123.45 42hello", result);
        }

        [Test]
        public void StringFormat_WithDoubleEscapedCurlyBraces_DoesNotFormatString()
        {
            //arrange
            var o = new { foo = 123.45 };

            //act
            string result = "{{{{foo}}}}".NamedFormat(o);

            //assert
            Assert.AreEqual("{{foo}}", result);
        }

        [Test]
        public void StringFormat_WithFormatSurroundedByDoubleEscapedBraces_FormatsString()
        {
            //arrange
            var o = new { foo = 123.45 };

            //act
            string result = "{{{{{foo}}}}}".NamedFormat(o);

            //assert
            Assert.AreEqual("{{123.45}}", result);
        }

        [Test]
        public void Format_WithEscapeSequence_EscapesInnerCurlyBraces()
        {
            var o = new { foo = 123.45 };

            //act
            string result = "{{{foo}}}".NamedFormat(o);

            //assert
            Assert.AreEqual("{123.45}", result);
        }

        [Test]
        public void Format_WithEmptyString_ReturnsEmptyString()
        {
            var o = new { foo = 123.45 };

            //act
            string result = string.Empty.NamedFormat(o);

            //assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Format_WithNoFormats_ReturnsFormatStringAsIs()
        {
            var o = new { foo = 123.45 };

            //act
            string result = "a b c".NamedFormat(o);

            //assert
            Assert.AreEqual("a b c", result);
        }

        [Test]
        public void Format_WithFormatType_ReturnsFormattedExpression()
        {
            var o = new { foo = 123.45 };

            //act
            string result = "{foo:#.#}".NamedFormat(o);

            //assert
            Assert.AreEqual("123.5", result);
        }

        [Test]
        public void Format_WithSubProperty_ReturnsValueOfSubProperty()
        {
            var o = new { foo = new { bar = 123.45 } };

            //act
            string result = "{foo.bar:#.#}ms".NamedFormat(o);

            //assert
            Assert.AreEqual("123.5ms", result);
        }

        [Test]
        public void Format_WithFormatNameNotInObject_ThrowsFormatException()
        {
            //arrange
            var o = new { foo = 123.45 };

            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{bar}".NamedFormat(o));
        }

        [Test]
        public void Format_WithNoEndFormatBrace_ThrowsFormatException()
        {
            //arrange
            var o = new { foo = 123.45 };

            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{bar".NamedFormat(o));
        }

        [Test]
        public void Format_WithEscapedEndFormatBrace_ThrowsFormatException()
        {
            //arrange
            var o = new { foo = 123.45 };

            
            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{foo}}".NamedFormat(o));
        }

        [Test]
        public void Format_WithDoubleEscapedEndFormatBrace_ThrowsFormatException()
        {
            //arrange
            var o = new { foo = 123.45 };

            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{foo}}}}bar".NamedFormat(o));
        }

        [Test]
        public void Format_WithDoubleEscapedEndFormatBraceWhichTerminatesString_ThrowsFormatException()
        {
            //arrange
            var o = new { foo = 123.45 };

            //act, assert
            UnitTestHelper.AssertThrows<FormatException>(() => "{foo}}}}".NamedFormat(o));
        }

        [Test]
        public void Format_WithEndBraceFollowedByEscapedEndFormatBraceWhichTerminatesString_FormatsCorrectly()
        {
            var o = new { foo = 123.45 };

            //act
            string result = "{foo}}}".NamedFormat(o);

            //assert
            Assert.AreEqual("123.45}", result);
        }

        [Test]
        public void Format_WithEndBraceFollowedByEscapedEndFormatBrace_FormatsCorrectly()
        {
            var o = new { foo = 123.45 };

            //act
            string result = "{foo}}}bar".NamedFormat(o);
            
            //assert
            Assert.AreEqual("123.45}bar", result);
        }

        [Test]
        public void Format_WithEndBraceFollowedByDoubleEscapedEndFormatBrace_FormatsCorrectly()
        {
            var o = new { foo = 123.45 };

            //act
            string result = "{foo}}}}}bar".NamedFormat(o);

            //assert
            Assert.AreEqual("123.45}}bar", result);
        }

        [Test]
        public void Format_WithNullFormatString_ThrowsArgumentNullException()
        {
            //arrange, act, assert
            UnitTestHelper.AssertThrows<ArgumentNullException>(() => ((string)null).NamedFormat(123));
        }
    }
}
