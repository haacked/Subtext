using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
    [TestFixture]
    public class LiteralFormatTests
    {
        [Test]
        public void Literal_WithEscapedCloseBraces_CollapsesDoubleBraces()
        {
            //arrange
            var literal = new LiteralFormat("hello}}world");
            //act
            string result = literal.Eval(null);
            //assert
            Assert.AreEqual("hello}world", result);
        }

        [Test]
        public void Literal_WithEscapedOpenBraces_CollapsesDoubleBraces()
        {
            //arrange
            var literal = new LiteralFormat("hello{{world");
            //act
            string result = literal.Eval(null);
            //assert
            Assert.AreEqual("hello{world", result);
        }
    }
}