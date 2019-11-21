using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestClass]
    public class ReferrerTests
    {
        [TestMethod]
        public void CanSetAndGetSimpleProperties()
        {
            var referrer = new Referrer();
            referrer.ReferrerUrl = "localhost";
            Assert.AreEqual("http://localhost", referrer.ReferrerUrl);
            UnitTestHelper.AssertSimpleProperties(referrer, "ReferrerUrl");
        }
    }
}