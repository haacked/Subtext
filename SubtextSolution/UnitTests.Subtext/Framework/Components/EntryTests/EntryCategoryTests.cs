using System;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	[TestFixture]
	public class EntryCategoryTests
	{
		[Test]
		[RollBack]
		public void CanAddAndRemoveAllCategories()
		{
			string hostname = UnitTestHelper.GenerateRandomString();
			Config.CreateBlog("", hostname, string.Empty, null);

			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, "/", string.Empty);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Unit Test Entry", "Body");
			int id = Entries.Create(entry);

			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "My Subtext UnitTest Category");

			Entries.SetEntryCategoryList(id, new int[] { categoryId });

			Entry loaded = Entries.GetEntry(id, PostConfig.None, true);
			Assert.AreEqual("My Subtext UnitTest Category", loaded.Categories[0], "Expected a category for this entry");

			Entries.SetEntryCategoryList(id, new int[]{});

			loaded = Entries.GetEntry(id, PostConfig.None, true);
			Assert.AreEqual(0, loaded.Categories.Count, "Expected that our category would be removed.");
		}
	}
}
