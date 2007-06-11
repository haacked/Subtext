using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Subtext.Extensibility.Providers;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Search;

namespace UnitTests.Subtext.Framework.Search
{
	[TestFixture]
	public class SearchTests
	{
		[Test]
		public void CanGetSearchProvider()
		{
			Assert.IsNotNull(SearchEngine.Provider);
			Assert.Greater(SearchEngine.Providers.Count, 0);
		}

		[Test]
		[RollBack2]
		public void CanSearch()
		{
			UnitTestHelper.SetupBlog();
			int blogId = Config.CurrentBlog.Id;
			IList<SearchResult> results = SearchEngine.Search(blogId, Guid.NewGuid().ToString());
			Assert.AreEqual(0, results.Count);

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Unit Tests Are Fun!", "Subtext kicks the llamas's ass!");
			Entries.Create(entry);
			results = SearchEngine.Search(blogId, "Tests");
			Assert.AreEqual(1, results.Count);
			Assert.AreEqual("Unit Tests Are Fun!", results[0].Title);
			StringAssert.Contains(results[0].Url.ToString(), "Unit_Tests_Are_Fun");
		}

		[Test]
		[RollBack2]
		[ExpectedArgumentNullException]
		public void SearchThrowsNullReferenceException()
		{
			UnitTestHelper.SetupBlog();
			SearchEngine.Search(Config.CurrentBlog.Id, null);
		}
	}
}
