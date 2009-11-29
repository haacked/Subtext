using System.Drawing;
using MbUnit.Framework;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
    [TestFixture]
    public class MathHelperTests
    {
        [Test]
        public void ScaleToFit_WithImageAlreadyFitting_ReturnsOriginalImage()
        {
            // arrange
            var original = new Size(8, 4); // aspect = 2.00

            // act
            Size resized = original.ScaleToFit(new Size(12, 15));

            // assert
            Assert.AreEqual(new Size(8, 4), resized);
        }

        [Test]
        public void ScaleToFit_ScaledToSameAspectRatio_ScalesExactlyToMaxSize()
        {
            // arrange
            var original = new Size(8, 4); // aspect = 2.00

            // act
            Size resized = original.ScaleToFit(new Size(4, 2));

            // assert
            Assert.AreEqual(new Size(4, 2), resized);
        }

        [Test]
        public void ScaleToFit_WithImageHavingAspectRatioGreaterThanOneScaledToAspectRatioLessThanOne_ScalesCorrectly()
        {
            // arrange
            var original = new Size(7, 5); // aspect = 1.40
            var maxSize = new Size(2, 3); // aspect = 0.67

            // act
            Size resized = original.ScaleToFit(maxSize);

            // assert
            Assert.AreEqual(new Size(2, 1), resized); // aspect = 2
        }

        [Test]
        public void ScaleToFit_WithImageHavingAspectRatioLessThanOneScaledToAspectRatioGreaterThanOne_ScalesCorrectly()
        {
            // arrange
            var original = new Size(5, 7); // aspect = 0.71
            var maxSize = new Size(4, 3); // aspect = 1.33

            // act
            Size resized = original.ScaleToFit(maxSize);

            // assert
            Assert.AreEqual(new Size(2, 3), resized); // aspect = 0.67
        }
    }
}