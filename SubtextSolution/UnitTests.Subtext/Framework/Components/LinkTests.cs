using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class LinkTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            Link link = new Link();
            UnitTestHelper.AssertSimpleProperties(link);
        }

        [Test]
        public void CanSetAndGetSimpleLinkCategoryProperties()
        {
            LinkCategory category = new LinkCategory();
            UnitTestHelper.AssertSimpleProperties(category);
        }
    }
}
