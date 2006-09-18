using System;
using MbUnit.Framework;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Data
{
	[TestFixture]
	public class SqlDataProviderTests
	{
		[Test]
		[ExpectedArgumentNullException]
		public void InsertFeedbackThrowsArgumentNullException()
		{
			SqlDataProvider provider = new SqlDataProvider();
			provider.InsertFeedback(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertCategoryThrowsArgumentNullException()
		{
			SqlDataProvider provider = new SqlDataProvider();
			provider.InsertCategory(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertEntryThrowsArgumentNullException()
		{
			SqlDataProvider provider = new SqlDataProvider();
			provider.InsertEntry(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertImageThrowsArgumentNullException()
		{
			SqlDataProvider provider = new SqlDataProvider();
			provider.InsertImage(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertKeyWordThrowsArgumentNullException()
		{
			SqlDataProvider provider = new SqlDataProvider();
			provider.InsertKeyWord(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertLinkThrowsArgumentNullException()
		{
			SqlDataProvider provider = new SqlDataProvider();
			provider.InsertLink(null);
		}
	}
}
