using System;
using System.Collections.Generic;
using System.Text;
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
            KeyWord keyword = new KeyWord();
            UnitTestHelper.AssertSimpleProperties(keyword);
        }

    }
}
