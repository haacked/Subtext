using System;
using MbUnit.Framework;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Data
{
	[TestFixture]
	public class DatabaseObjectProviderTests
	{
		[Test]
		[ExpectedArgumentNullException]
		public void CreateFeedbackThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.CreateFeedback(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertCategoryThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.CreateLinkCategory(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertEntryThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.InsertEntry(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertImageThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.InsertImage(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void InsertKeyWordThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.InsertKeyWord(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void CreateLinkThrowsArgumentNullException()
		{
            DatabaseObjectProvider provider = new DatabaseObjectProvider();
			provider.CreateLink(null);
		}
	}
}
