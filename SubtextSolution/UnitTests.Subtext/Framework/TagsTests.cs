using System;
using MbUnit.Framework;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework
{
	[TestFixture]
	public class TagsTests
	{
		[RowTest]
		[Row(-1, 1, 1)]
		[Row(0, 1, 2)]
		[Row(.25, 1, 3)]
		[Row(.49, 1, 4)]
		[Row(.9, 1, 5)]
		[Row(1.9, 1, 6)]
		public void CanComputeWeight(double factor, double stdDev, int expected)
		{
			Assert.AreEqual(expected, Tags.ComputeWeight(factor, stdDev));
		}
	}
}
