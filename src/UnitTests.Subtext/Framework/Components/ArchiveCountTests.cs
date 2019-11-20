using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestClass]
    public class ArchiveCountTests
    {
        [TestMethod]
        public void CanSetAndGetSimpleProperties()
        {
            var archive = new ArchiveCount();
            UnitTestHelper.AssertSimpleProperties(archive);
        }
    }
}