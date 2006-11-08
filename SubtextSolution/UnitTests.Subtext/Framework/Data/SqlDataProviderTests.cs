using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Data
{
	[TestFixture]
	public class DatabaseObjectProviderTests
	{
		[Test]
		[RollBack]
		public void CanClearBlogContent()
		{
			string host = UnitTestHelper.GenerateRandomString();
			Config.CreateBlog("test title", "test", "testaoeu!123", host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "");
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("author", "Ttitle", "Some body");
			int entryId = Entries.Create(entry);
			entry.BlogId = Config.CurrentBlog.Id;
			Entry loaded = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.IsNotNull(loaded);
			Assert.AreEqual(Config.CurrentBlog.Id, loaded.BlogId);

			DatabaseObjectProvider provider = new DatabaseObjectProvider();
			NameValueCollection config = new NameValueCollection();
			config.Add("connectionStringName", "subtextData");
			provider.Initialize("data", config);
			provider.ClearBlogContent();
			loaded = Entries.GetEntry(entryId, PostConfig.None, false);
			Assert.IsNull(loaded);
		}
		
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
