using System;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Data
{
	[TestFixture]
	public class DatabaseObjectProviderTests
	{
		string hostName;
		/// <summary>
		/// Creates some entries and makes sure that the proper 
		/// number of pages and entries per page are created 
		/// for various page sizes.
		/// </summary>
		[RowTest]
		[Row(11, 10, 2, 1)]
		[Row(11, 5, 3, 1)]
		[Row(12, 5, 3, 2)]
		[Row(10, 5, 2, 5)]
		[Row(10, 20, 1, 10)]
		[RollBack]
		public void GetPagedEntriesHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			//Create entries
			for(int i = 0; i < total; i++)
			{
				Entries.Create(UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Title" + i, "Who rocks the party that rocks the party?"));
			}

			int pageCount = 0;
			int totalSeen = 0;
			for (int pageIndex = 0; pageIndex < expectedPageCount; pageIndex++)
			{
				IPagedCollection<Entry> entries = Entries.GetPagedEntries(PostType.BlogPost, -1, pageIndex, pageSize);
				Assert.AreEqual(total, entries.MaxItems, "The paged collection got the max items wrong)");

				if (pageIndex < expectedPageCount - 1)
				{
					//Expect to see pageSize number of entries.
					Assert.AreEqual(pageSize, entries.Count, "The page at index " + pageIndex + "Did not have the correct number of records.");
				}
				else
				{
					Assert.AreEqual(itemsCountOnLastPage, entries.Count, "The last page did not have the correct number of records.");					
				}
				totalSeen += entries.Count;

				pageCount++;
			}
			
			Assert.AreEqual(expectedPageCount, pageCount, "We did not see the expected number of pages.");
			Assert.AreEqual(total, totalSeen, "We did not see the expected number of records.");
		}

		/// <summary>
		/// Creates some comments and makes sure that the proper 
		/// number of pages and entries per page are created 
		/// for various page sizes.
		/// </summary>
		[RowTest]
		[Row(11, 10, 2, 1)]
		[Row(11, 5, 3, 1)]
		[Row(12, 5, 3, 2)]
		[Row(10, 5, 2, 5)]
		[Row(10, 20, 1, 10)]
		[RollBack]
		public void GetPagedFeedbackHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			//Create entries
			for (int i = 0; i < total; i++)
			{
				Entry comment = UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Title" + i, "Who rocks the party that rocks the party? " + i);
				comment.PostType = PostType.Comment;
				comment.AlternativeTitleUrl = "blah";
				Entries.InsertComment(comment);
			}

			int pageCount = 0;
			int totalSeen = 0;
			for (int pageIndex = 0; pageIndex < expectedPageCount; pageIndex++)
			{
				IPagedCollection<Entry> entries = Entries.GetPagedFeedback(pageIndex, pageSize);
				Assert.AreEqual(total, entries.MaxItems, "The paged collection got the max items wrong)");

				if (pageIndex < expectedPageCount - 1)
				{
					//Expect to see pageSize number of entries.
					Assert.AreEqual(pageSize, entries.Count, "The page at index " + pageIndex + "Did not have the correct number of records.");
				}
				else
				{
					Assert.AreEqual(itemsCountOnLastPage, entries.Count, "The last page did not have the correct number of records.");
				}
				totalSeen += entries.Count;

				pageCount++;
			}

			Assert.AreEqual(expectedPageCount, pageCount, "We did not see the expected number of pages.");
			Assert.AreEqual(total, totalSeen, "We did not see the expected number of records.");
		}

		/// <summary>
		/// Creates some links and makes sure that the proper 
		/// number of pages and entries per page are created 
		/// for various page sizes.
		/// </summary>
		[RowTest]
		[Row(11, 10, 2, 1)]
		[Row(11, 5, 3, 1)]
		[Row(12, 5, 3, 2)]
		[Row(10, 5, 2, 5)]
		[Row(10, 20, 1, 10)]
		[RollBack]
		public void GetPagedLinksHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			LinkCategory category = new LinkCategory();
			category.BlogId = Config.CurrentBlog.Id;
			category.IsActive = true;
			category.Title = "Foobar";
			category.Description = "Unit Test";
			
			int categoryId = Links.CreateLinkCategory(category);
			//Create entries
			for (int i = 0; i < total; i++)
			{
				Link link = new Link();
				link.BlogId = Config.CurrentBlog.Id;
				link.IsActive = true;
				link.CategoryID = categoryId;
				link.Title = "Blah " + i;
				link.Url = "http://noneofyourbusiness.com/";
				Links.CreateLink(link);
			}

			int pageCount = 0;
			int totalSeen = 0;
			
			for (int pageIndex = 0; pageIndex < expectedPageCount; pageIndex++)
			{
				//TODO: Unfortunately, GetPagedLinks takes in a one-based index.  Wasn't my idea.
				//I'll refactor it later.
				IPagedCollection<Link> entries = Links.GetPagedLinks(categoryId, pageIndex + 1, pageSize, true);
				Assert.AreEqual(total, entries.MaxItems, "The paged collection got the max items wrong)");

				if (pageIndex < expectedPageCount - 1)
				{
					//Expect to see pageSize number of entries.
					Assert.AreEqual(pageSize, entries.Count, "The page at index " + pageIndex + "Did not have the correct number of records.");
				}
				else
				{
					Assert.AreEqual(itemsCountOnLastPage, entries.Count, "The last page did not have the correct number of records.");
				}
				totalSeen += entries.Count;

				pageCount++;
			}

			Assert.AreEqual(expectedPageCount, pageCount, "We did not see the expected number of pages.");
			Assert.AreEqual(total, totalSeen, "We did not see the expected number of records.");
		}

		[SetUp]
		public void SetUp()
		{
			this.hostName = UnitTestHelper.GenerateRandomString();
			Assert.IsTrue(Config.CreateBlog("", "username", "password", this.hostName, "blog"));
			UnitTestHelper.SetHttpContextWithBlogRequest(this.hostName, "blog");
			CommentFilter.ClearCommentCache();	
		}

		[TearDown]
		public void TearDown()
		{
			Config.ConfigurationProvider = null;
		}
	}
}
