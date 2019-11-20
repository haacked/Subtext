using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestClass]
    public class BlogGroupTests
    {
        [TestMethod]
        public void CanSetAndGetSimpleProperties()
        {
            var group = new BlogGroup();
            UnitTestHelper.AssertSimpleProperties(group);
        }
    }
}