using System;
using System.Collections.Generic;
using System.Text;
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
            ViewStat stat = new ViewStat();
            UnitTestHelper.AssertSimpleProperties(stat);
        }
    }
}
