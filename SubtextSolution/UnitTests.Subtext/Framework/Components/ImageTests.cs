using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class ImageTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            Image image = new Image();
            UnitTestHelper.AssertSimpleProperties(image);
        }

    }
}
