using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
    [TestClass]
    public class TagTests
    {
        [TestMethod]
        public void CanSetAndGetSimpleProperties()
        {
            var tag = new Tag(new KeyValuePair<string, int>());
            UnitTestHelper.AssertSimpleProperties(tag);
        }
    }
}