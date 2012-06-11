using MbUnit.Framework;
using Moq;
using Ninject;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestFixture]
    public class SubtextPageBuilderTests
    {
        [Test]
        public void CtorSetsKernel()
        {
            //arrange
            IKernel kernel = new Mock<IKernel>().Object;

            //act
            var builder = new SubtextPageBuilder(kernel);

            //assert
            Assert.AreSame(kernel, builder.Kernel);
        }
    }
}