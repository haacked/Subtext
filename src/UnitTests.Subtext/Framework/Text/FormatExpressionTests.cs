using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
    [TestFixture]
    public class FormatExpressionTests
    {
        [Test]
        public void Format_WithExpressionReturningNull_ReturnsEmptyString()
        {
            //arrange
            var expr = new FormatExpression("{foo}");

            //assert
            Assert.AreEqual(string.Empty, expr.Eval(new {foo = (string)null}));
        }

        [Test]
        public void Format_WithoutColon_ReadsWholeExpression()
        {
            //arrange
            var expr = new FormatExpression("{foo}");

            //assert
            Assert.AreEqual("foo", expr.Expression);
        }

        [Test]
        public void Format_WithColon_ParsesoutFormat()
        {
            //arrange
            var expr = new FormatExpression("{foo:#.##}");

            //assert
            Assert.AreEqual("#.##", expr.Format);
        }

        [Test]
        public void Eval_WithNamedExpression_EvalsPropertyOfExpression()
        {
            //arrange
            var expr = new FormatExpression("{foo}");

            //act
            string result = expr.Eval(new {foo = 123});

            //assert
            Assert.AreEqual("123", result);
        }

        [Test]
        [MultipleCulture("en-US,en-NZ,it-IT")]
        public void Eval_WithNamedExpressionAndFormat_EvalsPropertyOfExpression()
        {
            //arrange
            var expr = new FormatExpression("{foo:#.##}");

            //act
            string result = expr.Eval(new {foo = 1.23456});

            //assert
            float expected = 1.23f;
            Assert.AreEqual(expected.ToString(), result);
        }
    }
}