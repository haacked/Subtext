using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class KeywordTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            var keyword = new KeyWord();
            UnitTestHelper.AssertSimpleProperties(keyword);
        }
    }
}