using MbUnit.Framework;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class AggregateEnabledConstraintTests
    {
        [Test]
        public void ConstraintWithTrue_WithSettingSetToTrue_ReturnsTrue()
        {
            //arrange
            var constraint = new AggregateEnabledConstraint(null, true);

            //act
            bool result = constraint.Match(true);

            //assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ConstraintWithFalse_WithSettingSetToTrue_ReturnsFalse()
        {
            //arrange
            var constraint = new AggregateEnabledConstraint(null, false);

            //act
            bool result = constraint.Match(true);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ConstraintWithTrue_WithSettingSetToFalse_ReturnsFalse()
        {
            //arrange
            var constraint = new AggregateEnabledConstraint(null, true);

            //act
            bool result = constraint.Match(false);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ConstraintWithFalse_WithSettingSetToFalse_ReturnsTrue()
        {
            //arrange
            var constraint = new AggregateEnabledConstraint(null, false);

            //act
            bool result = constraint.Match(false);

            //assert
            Assert.IsTrue(result);
        }
    }
}