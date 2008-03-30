using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class BlogGroupTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            BlogGroup group = new BlogGroup();
            UnitTestHelper.AssertSimpleProperties(group);
        }
    }
}
