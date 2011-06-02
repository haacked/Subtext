using MbUnit.Framework;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Configuration
{
    [TestFixture]
    public class BlogGroupTests
    {
        [Test]
        [RollBack2]
        public void CanListBlogGroups()
        {
            Assert.Greater(new DatabaseObjectProvider().ListBlogGroups(true).Count, 0, "Expected at least one blog group");
        }

        [Test]
        [RollBack2]
        public void CanGetBlogGroup()
        {
            Assert.IsNotNull(new DatabaseObjectProvider().GetBlogGroup(1, true), "Expected the default blog group");
        }
    }
}