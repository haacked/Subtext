using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestClass]
    public class ViewStatTests
    {
        [TestMethod]
        public void CanSetAndGetSimpleProperties()
        {
            var stat = new ViewStat();
            UnitTestHelper.AssertSimpleProperties(stat);
        }
    }
}