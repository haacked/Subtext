using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Configuration
{
    [TestClass]
    public class BlogGroupTests
    {
        [DatabaseIntegrationTestMethod]
        public void CanListBlogGroups()
        {
            Assert.IsTrue(
                new DatabaseObjectProvider().ListBlogGroups(true).Count > 0,
                "Expected at least one blog group");
        }

        [DatabaseIntegrationTestMethod]
        public void CanGetBlogGroup()
        {
            Assert.IsNotNull(new DatabaseObjectProvider().GetBlogGroup(1, true), "Expected the default blog group");
        }
    }
}