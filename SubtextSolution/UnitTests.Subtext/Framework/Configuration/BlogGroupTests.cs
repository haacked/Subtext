using System;
using MbUnit.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Configuration
{
	[TestFixture]
	public class BlogGroupTests
	{
		[Test]
		[RollBack2]
		public void CanListBlogGroups()
		{
			Assert.Greater(Config.ListBlogGroups(true).Count, 0, "Expected at least one blog group");
		}

		[Test]
		[RollBack2]
		public void CanGetBlogGroup()
		{
			Assert.IsNotNull(Config.GetBlogGroup(1, true), "Expected the default blog group");
		}
	}
}
