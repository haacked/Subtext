using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class LinkTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            var link = new Link();
            UnitTestHelper.AssertSimpleProperties(link);
        }

        [Test]
        public void CanSetAndGetSimpleLinkCategoryProperties()
        {
            var category = new LinkCategory();
            UnitTestHelper.AssertSimpleProperties(category);
        }
    }
}