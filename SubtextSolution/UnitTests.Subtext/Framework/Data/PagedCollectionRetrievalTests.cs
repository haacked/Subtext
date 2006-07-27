using System;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using UnitTests.Subtext;

namespace UnitTests.Subtext.Framework.Data
{
	[TestFixture]
	public class PagedCollectionRetrievalTests
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
			IPagedCollectionTester[] collectionTesters = {
			                           	new PagedEntryCollectionTester()
										, new FeedbackCollectionTester()
										, new LinkCollectionTester()
			                           };

			foreach (IPagedCollectionTester factory in collectionTesters)
			{
				AssertPagedCollection(factory, expectedPageCount, itemsCountOnLastPage, pageSize, total);
			}
		}

		private static void AssertPagedCollection(IPagedCollectionTester pagedCollectionTester, int expectedPageCount, int itemsCountOnLastPage, int pageSize, int total)
		{
			//Create entries
			for(int i = 0; i < total; i++)
			{
				pagedCollectionTester.Create(i);
			}

			int pageCount = 0;
			int totalSeen = 0;
			for (int pageIndex = 0; pageIndex < expectedPageCount; pageIndex++)
			{
				IPagedCollection items = pagedCollectionTester.GetPagedItems(pageIndex, pageSize);
				Assert.AreEqual(total, items.MaxItems, "The paged collection got the max items wrong)");

				if (pageIndex < expectedPageCount - 1)
				{
					//Expect to see pageSize number of entries.
					Assert.AreEqual(pageSize, pagedCollectionTester.GetCount(items), "The page at index " + pageIndex + "Did not have the correct number of records.");
				}
				else
				{
					Assert.AreEqual(itemsCountOnLastPage, pagedCollectionTester.GetCount(items), "The last page did not have the correct number of records.");					
				}
				totalSeen += pagedCollectionTester.GetCount(items);

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
	
	internal interface IPagedCollectionTester
	{
		void Create(int index);
		IPagedCollection GetPagedItems(int pageIndex, int pageSize);
		int GetCount(IPagedCollection collection);
	}
	
	internal class PagedEntryCollectionTester : IPagedCollectionTester
	{
		public void Create(int index)
		{
			Entries.Create(UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Title" + index, "Who rocks the party that rocks the party?"));
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			return Entries.GetPagedEntries(PostType.BlogPost, -1, pageIndex, pageSize);
		}
		
		public int GetCount(IPagedCollection collection)
		{
			return ((IPagedCollection<Entry>)collection).Count;
		}		
	}
	
	internal class FeedbackCollectionTester : IPagedCollectionTester
	{
		public void Create(int index)
		{
			Entry comment = UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Title" + index, "Who rocks the party that rocks the party? " + index);
			comment.PostType = PostType.Comment;
			comment.AlternativeTitleUrl = "blah";
			Entries.InsertComment(comment);
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			return Entries.GetPagedFeedback(pageIndex, pageSize);
		}

		public int GetCount(IPagedCollection collection)
		{
			return ((IPagedCollection<Entry>)collection).Count;
		}	
	}

	internal class LinkCollectionTester : IPagedCollectionTester
	{
		int categoryId;

		public LinkCollectionTester()
		{
			LinkCategory category = new LinkCategory();
			category.BlogId = Config.CurrentBlog.Id;
			category.IsActive = true;
			category.Title = "Foobar";
			category.Description = "Unit Test";
			this.categoryId = Links.CreateLinkCategory(category);
		}

		public void Create(int index)
		{
			Link link = new Link();
			link.BlogId = Config.CurrentBlog.Id;
			link.IsActive = true;
			link.CategoryID = this.categoryId;
			link.Title = "Blah " + index;
			link.Url = "http://noneofyourbusiness.com/";
			Links.CreateLink(link);
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			//TODO: Unfortunately, GetPagedLinks takes in a one-based index.  Wasn't my idea.
			//I'll refactor it later.
			return Links.GetPagedLinks(categoryId, pageIndex + 1, pageSize, true);
		}

		public int GetCount(IPagedCollection collection)
		{
			return ((IPagedCollection<Link>)collection).Count;
		}
	}
}
