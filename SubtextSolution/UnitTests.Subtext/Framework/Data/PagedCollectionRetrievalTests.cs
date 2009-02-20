using System;
using System.Data;
using System.Data.SqlClient;
using MbUnit.Framework;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Util;
using Subtext.Framework.Web.HttpModules;

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
			Config.CreateBlog("", "username", "password", this.hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, "blog");
			AssertPagedCollection(new PagedEntryCollectionTester(), expectedPageCount, itemsCountOnLastPage, pageSize, total);
		}

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
		public void GetPagedEntriesByCategoryHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			Config.CreateBlog("", "username", "password", this.hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, "blog");
			AssertPagedCollection(new PagedEntryByCategoryCollectionTester(), expectedPageCount, itemsCountOnLastPage, pageSize, total);
		}

		/// <summary>
		/// Creates some entries and makes sure that the proper 
		/// number of pages and entries per page are created 
		/// for various page sizes.
		/// </summary>
		[RowTest]
		[Row(11, 10, 2, 1)]
        //[Row(11, 5, 3, 1)]
        //[Row(12, 5, 3, 2)]
        //[Row(10, 5, 2, 5)]
        //[Row(10, 20, 1, 10)]
		[RollBack]
		public void GetPagedFeedbackHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			Config.CreateBlog("", "username", "password", this.hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, "blog");
			AssertPagedCollection(new FeedbackCollectionTester(), expectedPageCount, itemsCountOnLastPage, pageSize, total);
		}

		[RowTest]
		[Row(11, 10, 2, 1)]
		[Row(11, 5, 3, 1)]
		[Row(12, 5, 3, 2)]
		[Row(10, 5, 2, 5)]
		[Row(10, 20, 1, 10)]
		[RollBack]
		public void GetPagedLinksHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			Config.CreateBlog("", "username", "password", this.hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, "blog");
			IPagedCollectionTester tester = new LinkCollectionTester();
			AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
		}

		[RowTest]
		[Row(11, 10, 2, 1)]
		[Row(11, 5, 3, 1)]
		[Row(12, 5, 3, 2)]
		[Row(10, 5, 2, 5)]
		[Row(10, 20, 1, 10)]
		[RollBack]
		public void GetPagedLogEntriesHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			Config.CreateBlog("", "username", "password", this.hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, "blog");
			IPagedCollectionTester tester = new LogEntryCollectionTester();
			AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
		}

        [RowTest]
        [Row(11, 10, 2, 1)]
        [Row(11, 5, 3, 1)]
        [Row(12, 5, 3, 2)]
        [Row(10, 5, 2, 5)]
        [Row(10, 20, 1, 10)]
        [RollBack]
        public void GetPagedMetaTagsHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
        {
            Config.CreateBlog("", "username", "password", this.hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, "blog");
            IPagedCollectionTester tester = new MetaTagCollectionTester();
            AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
        }

		[RowTest]
		[Row(11, 10, 2, 1)]
		[Row(11, 5, 3, 1)]
		[Row(12, 5, 3, 2)]
		[Row(10, 5, 2, 5)]
		[Row(10, 20, 1, 10)]
		[RollBack]
		public void GetPagedKeywordsHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			Config.CreateBlog("", "username", "password", this.hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(hostName, "blog");
			IPagedCollectionTester tester = new KeyWordCollectionTester();
			AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
		}

		[RowTest]
		[Row(11, 10, 2, 1)]
		[Row(11, 5, 3, 1)]
		[Row(12, 5, 3, 2)]
		[Row(10, 5, 2, 5)]
		[Row(10, 20, 1, 10)]
		[RollBack]
		public void GetPagedBlogsHandlesPagingProperly(int total, int pageSize, int expectedPageCount, int itemsCountOnLastPage)
		{
			IPagedCollectionTester tester = new BlogCollectionTester();
			AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
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
			this.hostName = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(this.hostName, "blog");
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
			UnitTestHelper.Create(UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Title" + index, "Who rocks the party that rocks the party?"));
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

	internal class PagedEntryByCategoryCollectionTester : IPagedCollectionTester
	{
		int categoryId;
		
		public PagedEntryByCategoryCollectionTester()
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
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Title" + index, "Who rocks the party that rocks the party?");
			entry.Categories.Add("Foobar");
			UnitTestHelper.Create(entry);
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			return Entries.GetPagedEntries(PostType.BlogPost, this.categoryId, pageIndex, pageSize);
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
			FeedbackItem feedbackItem = new FeedbackItem(FeedbackType.Comment);
			feedbackItem.Author = "Phil";
			feedbackItem.Title = "Title" + index;
			feedbackItem.Body = "Who rocks the party that rocks the party? " + index;

			feedbackItem.SourceUrl = new Uri("http://blah/");
			FeedbackItem.Create(feedbackItem, null);
			FeedbackItem.Approve(feedbackItem, null);
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			return FeedbackItem.GetPagedFeedback(pageIndex, pageSize, FeedbackStatusFlag.Approved, FeedbackType.None);
		}

		public int GetCount(IPagedCollection collection)
		{
			return ((IPagedCollection<FeedbackItem>)collection).Count;
		}	
	}

	internal class LogEntryCollectionTester : IPagedCollectionTester
	{
		public void Create(int index)
		{
			SqlParameter[] parameters = {
			                            	new SqlParameter("@BlogId", Config.CurrentBlog.Id)
											, new SqlParameter("@Date", DateTime.Now)
											, new SqlParameter("@Thread", "SomeThread")
											, new SqlParameter("@Context", "SomeContext")
											, new SqlParameter("@Level", "unit test")
											, new SqlParameter("@Logger", "UnitTestLogger")
											, new SqlParameter("@Message", "This test was brought to you by the letter 'Q'.")
											, new SqlParameter("@Exception", "")
											, new SqlParameter("@Url", "http://localhost/")
			                            };
			SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.StoredProcedure, "subtext_AddLogEntry", parameters);
			
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			return LoggingProvider.Instance().GetPagedLogEntries(pageIndex, pageSize);
		}

		public int GetCount(IPagedCollection collection)
		{
			return ((IPagedCollection<LogEntry>)collection).Count;
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
			
			//Create a couple links that should be ignored because postId is not null.
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "title", "in great shape");
			int entryId = UnitTestHelper.Create(entry);
			UnitTestHelper.CreateLinkInDb(this.categoryId, "A Forgettable Link", entryId, String.Empty);
            UnitTestHelper.CreateLinkInDb(this.categoryId, "Another Forgettable Link", entryId, String.Empty);
            UnitTestHelper.CreateLinkInDb(this.categoryId, "Another Forgettable Link", entryId, String.Empty);
		}

		public void Create(int index)
		{
			UnitTestHelper.CreateLinkInDb(this.categoryId, "A Link To Remember Part " + index, null, String.Empty);
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			return Links.GetPagedLinks(categoryId, pageIndex, pageSize, true);
		}

		public int GetCount(IPagedCollection collection)
		{
			return ((IPagedCollection<Link>)collection).Count;
		}
	}

	internal class KeyWordCollectionTester : IPagedCollectionTester
	{
		public KeyWordCollectionTester()
		{
		}

		public void Create(int index)
		{
			KeyWord keyword = new KeyWord();
			keyword.BlogId = Config.CurrentBlog.Id;
			keyword.Text = "The Keyword" + index;
			keyword.Title = "Blah";
			keyword.Word = "The Word " + index;
			keyword.Rel = "Rel" + index;
			keyword.Url = "http://localhost/";
			KeyWords.CreateKeyWord(keyword);
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			return KeyWords.GetPagedKeyWords(pageIndex, pageSize);
		}

		public int GetCount(IPagedCollection collection)
		{
			return ((IPagedCollection<KeyWord>)collection).Count;
		}
	}

	internal class BlogCollectionTester : IPagedCollectionTester
	{
		string host = UnitTestHelper.GenerateUniqueString();
		
		public BlogCollectionTester()
		{
		}

		public void Create(int index)
		{
			Config.CreateBlog("title " + index, "phil", "password", host, "Subfolder" + index);
		}

		public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
		{
			return Blog.GetBlogsByHost(this.host, pageIndex, pageSize, ConfigurationFlags.IsActive);
		}

		public int GetCount(IPagedCollection collection)
		{
			return ((IPagedCollection<Blog>)collection).Count;
		}
	}

    internal class MetaTagCollectionTester : IPagedCollectionTester
    {
        public MetaTagCollectionTester()
        {
        }

        public void Create(int index)
        {
            MetaTag tag = new MetaTag("test" + index);
            tag.DateCreated = DateTime.Now;
            tag.Name = "foo";
            tag.BlogId = Config.CurrentBlog.Id;
            MetaTags.Create(tag);
        }

        public IPagedCollection GetPagedItems(int pageIndex, int pageSize)
        {
            return MetaTags.GetMetaTagsForBlog(Config.CurrentBlog, pageIndex, pageSize);
        }

        public int GetCount(IPagedCollection collection)
        {
            return ((IPagedCollection<MetaTag>)collection).Count;
        }
    }
}
