using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class ArchiveCountTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            ArchiveCount archive = new ArchiveCount();
            UnitTestHelper.AssertSimpleProperties(archive);
        }
    }
}
