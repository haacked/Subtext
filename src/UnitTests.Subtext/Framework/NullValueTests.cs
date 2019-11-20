using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework
{
    /// <summary>
    /// Tests of the NullValue helper class
    /// </summary>
    [TestClass]
    public class NullValueTests
    {
        [TestMethod]
        public void IsNullReturnsTrueForNullInt()
        {
            Assert.IsTrue(int.MinValue.IsNull());
        }

        [TestMethod]
        public void IsNullReturnsTrueForNullDateTime()
        {
            Assert.IsTrue(DateTime.MinValue.IsNull());
        }
    }
}