using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class TagTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            Tag tag = new Tag(new KeyValuePair<string,int>());
            UnitTestHelper.AssertSimpleProperties(tag);
        }
    }
}
