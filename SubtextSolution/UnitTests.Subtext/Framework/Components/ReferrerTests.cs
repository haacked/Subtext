using System;
using System.Collections.Generic;
using System.Text;
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
            Referrer referrer = new Referrer();
            referrer.ReferrerURL = "localhost";
            Assert.AreEqual("http://localhost", referrer.ReferrerURL);
            UnitTestHelper.AssertSimpleProperties(referrer, "ReferrerURL");
        }

    }
}
