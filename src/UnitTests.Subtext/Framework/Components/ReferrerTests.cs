using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class ReferrerTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            var referrer = new Referrer();
            referrer.ReferrerUrl = "localhost";
            Assert.AreEqual("http://localhost", referrer.ReferrerUrl);
            UnitTestHelper.AssertSimpleProperties(referrer, "ReferrerUrl");
        }
    }
}