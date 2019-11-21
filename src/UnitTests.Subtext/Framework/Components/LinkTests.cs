using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestClass]
    public class LinkTests
    {
        [TestMethod]
        public void CanSetAndGetSimpleProperties()
        {
            var link = new Link();
            UnitTestHelper.AssertSimpleProperties(link);
        }

        [TestMethod]
        public void CanSetAndGetSimpleLinkCategoryProperties()
        {
            var category = new LinkCategory();
            UnitTestHelper.AssertSimpleProperties(category);
        }
    }
}