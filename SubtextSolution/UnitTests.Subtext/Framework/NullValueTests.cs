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
		public void IsNullReturnsTrueForNullDouble()
		{
            Assert.IsTrue(NullValue.IsNull(double.NaN));
		}

		[Test]
		public void IsNullReturnsTrueForNullGuid()
		{
			Assert.IsTrue(NullValue.IsNull(Guid.Empty));
		}

		[Test]
		public void IsNullReturnsTrueForNullInt()
		{
			Assert.IsTrue(NullValue.IsNull(int.MinValue));
		}

		[Test]
		public void IsNullReturnsTrueForNullDateTime()
		{
			Assert.IsTrue(NullValue.IsNull(DateTime.MinValue));
		}
	}
}
