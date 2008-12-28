using System;
using System.Linq;
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
			string hostname = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("empty title", "username", "password", hostname, string.Empty);

			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, "/");

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Me", "Unit Test Entry", "Body");
			int id = Entries.Create(entry);

			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "My Subtext UnitTest Category");

			Entries.SetEntryCategoryList(id, new int[] { categoryId });

			Entry loaded = Entries.GetEntry(id, PostConfig.None, true);
			Assert.AreEqual("My Subtext UnitTest Category", loaded.Categories.First(), "Expected a category for this entry");

			Entries.SetEntryCategoryList(id, new int[]{});

			loaded = Entries.GetEntry(id, PostConfig.None, true);
			Assert.AreEqual(0, loaded.Categories.Count, "Expected that our category would be removed.");
		}
	}
}
