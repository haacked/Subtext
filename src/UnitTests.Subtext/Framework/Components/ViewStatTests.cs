using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class ViewStatTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            var stat = new ViewStat();
            UnitTestHelper.AssertSimpleProperties(stat);
        }
    }
}