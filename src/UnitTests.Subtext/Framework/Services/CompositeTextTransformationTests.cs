using MbUnit.Framework;
using Moq;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class CompositeTextTransformationTests
    {
        [Test]
        public void Transform_WithMultipleTransformations_RunsThemAll()
        {
            //arrange
            var transform1 = new Mock<ITextTransformation>();
            transform1.Setup(t => t.Transform(It.IsAny<string>())).Returns<string>(s => s + "t1");
            var transform2 = new Mock<ITextTransformation>();
            transform2.Setup(t => t.Transform(It.IsAny<string>())).Returns<string>(s => s + "t2");
            var composite = new CompositeTextTransformation();
            composite.Add(transform1.Object);
            composite.Add(transform2.Object);

            //act
            string transformed = composite.Transform("This is a test. ");

            //assert
            Assert.AreEqual("This is a test. t1t2", transformed);
        }
    }
}