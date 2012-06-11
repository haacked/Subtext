using System.Collections.Generic;
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
            var tag = new Tag(new KeyValuePair<string, int>());
            UnitTestHelper.AssertSimpleProperties(tag);
        }
    }
}