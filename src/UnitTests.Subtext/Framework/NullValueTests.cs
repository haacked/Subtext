using System;
using MbUnit.Framework;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework
{
    /// <summary>
    /// Tests of the NullValue helper class
    /// </summary>
    [TestFixture]
    public class NullValueTests
    {
        [Test]
        public void IsNullReturnsTrueForNullInt()
        {
            Assert.IsTrue(int.MinValue.IsNull());
        }

        [Test]
        public void IsNullReturnsTrueForNullDateTime()
        {
            Assert.IsTrue(DateTime.MinValue.IsNull());
        }
    }
}