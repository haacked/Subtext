using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestClass]
    public class KeywordTests
    {
        [TestMethod]
        public void CanSetAndGetSimpleProperties()
        {
            var keyword = new KeyWord();
            UnitTestHelper.AssertSimpleProperties(keyword);
        }
    }
}