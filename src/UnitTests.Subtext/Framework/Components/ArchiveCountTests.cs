using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class ArchiveCountTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            var archive = new ArchiveCount();
            UnitTestHelper.AssertSimpleProperties(archive);
        }
    }
}