using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class BlogGroupTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            var group = new BlogGroup();
            UnitTestHelper.AssertSimpleProperties(group);
        }
    }
}