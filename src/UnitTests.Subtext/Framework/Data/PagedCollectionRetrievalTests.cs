using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using MbUnit.Framework;
using Microsoft.ApplicationBlocks.Data;
using Moq;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Util;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Data
{
    [TestFixture]
    public class PagedCollectionRetrievalTests
    {
        string _hostName;

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
        public void GetPagedEntriesHandlesPagingProperly(int total, int pageSize, int expectedPageCount,
                                                         int itemsCountOnLastPage)
        {
            Config.CreateBlog("", "username", "password", _hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "blog");
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
        public void GetPagedEntriesByCategoryHandlesPagingProperly(int total, int pageSize, int expectedPageCount,
                                                                   int itemsCountOnLastPage)
        {
            Config.CreateBlog("", "username", "password", _hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "blog");
            AssertPagedCollection(new PagedEntryByCategoryCollectionTester(), expectedPageCount, itemsCountOnLastPage,
                                  pageSize, total);
        }

        /// <summary>
        /// Creates some entries and makes sure that the proper 
        /// number of pages and entries per page are created 
        /// for various page sizes.
        /// </summary>
        [RowTest]
        [Ignore("TODO")]
        [Row(11, 10, 2, 1)]
        //[Row(11, 5, 3, 1)]
        //[Row(12, 5, 3, 2)]
        //[Row(10, 5, 2, 5)]
        //[Row(10, 20, 1, 10)]
        [RollBack]
        public void GetPagedFeedbackHandlesPagingProperly(int total, int pageSize, int expectedPageCount,
                                                          int itemsCountOnLastPage)
        {
            Config.CreateBlog("", "username", "password", _hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "blog");
            AssertPagedCollection(new FeedbackCollectionTester(), expectedPageCount, itemsCountOnLastPage, pageSize,
                                  total);
        }

        [RowTest]
        [Row(11, 10, 2, 1)]
        [Row(11, 5, 3, 1)]
        [Row(12, 5, 3, 2)]
        [Row(10, 5, 2, 5)]
        [Row(10, 20, 1, 10)]
        [RollBack]
        public void GetPagedLinksHandlesPagingProperly(int total, int pageSize, int expectedPageCount,
                                                       int itemsCountOnLastPage)
        {
            Config.CreateBlog("", "username", "password", _hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "blog");
            var tester = new LinkCollectionTester();
            AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
        }

        [RowTest]
        [Row(11, 10, 2, 1)]
        [Row(11, 5, 3, 1)]
        [Row(12, 5, 3, 2)]
        [Row(10, 5, 2, 5)]
        [Row(10, 20, 1, 10)]
        [RollBack]
        public void GetPagedLogEntriesHandlesPagingProperly(int total, int pageSize, int expectedPageCount,
                                                            int itemsCountOnLastPage)
        {
            Config.CreateBlog("", "username", "password", _hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "blog");
            var tester = new LogEntryCollectionTester();
            AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
        }

        [RowTest]
        [Row(11, 10, 2, 1)]
        [Row(11, 5, 3, 1)]
        [Row(12, 5, 3, 2)]
        [Row(10, 5, 2, 5)]
        [Row(10, 20, 1, 10)]
        [RollBack]
        public void GetPagedMetaTagsHandlesPagingProperly(int total, int pageSize, int expectedPageCount,
                                                          int itemsCountOnLastPage)
        {
            Config.CreateBlog("", "username", "password", _hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "blog");
            var tester = new MetaTagCollectionTester();
            AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
        }

        [RowTest]
        [Row(11, 10, 2, 1)]
        [Row(11, 5, 3, 1)]
        [Row(12, 5, 3, 2)]
        [Row(10, 5, 2, 5)]
        [Row(10, 20, 1, 10)]
        [RollBack]
        public void GetPagedKeywordsHandlesPagingProperly(int total, int pageSize, int expectedPageCount,
                                                          int itemsCountOnLastPage)
        {
            Config.CreateBlog("", "username", "password", _hostName, "blog");
            BlogRequest.Current.Blog = Config.GetBlog(_hostName, "blog");
            var tester = new KeyWordCollectionTester();
            AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
        }

        [RowTest]
        [Row(11, 10, 2, 1)]
        [Row(11, 5, 3, 1)]
        [Row(12, 5, 3, 2)]
        [Row(10, 5, 2, 5)]
        [Row(10, 20, 1, 10)]
        [RollBack]
        public void GetPagedBlogsHandlesPagingProperly(int total, int pageSize, int expectedPageCount,
                                                       int itemsCountOnLastPage)
        {
            var tester = new BlogCollectionTester();
            AssertPagedCollection(tester, expectedPageCount, itemsCountOnLastPage, pageSize, total);
        }

        private static void AssertPagedCollection<TItem>(IPagedCollectionTester<TItem> pagedCollectionTester, int expectedPageCount,
                                                  int itemsCountOnLastPage, int pageSize, int total)
        {
            //Create entries
            for(int i = 0; i < total; i++)
            {
                pagedCollectionTester.Create(i);
            }

            int pageCount = 0;
            int totalSeen = 0;
            for(int pageIndex = 0; pageIndex < expectedPageCount; pageIndex++)
            {
                var items = pagedCollectionTester.GetPagedItems(pageIndex, pageSize);
                Assert.AreEqual(total, items.MaxItems, "The paged collection got the max items wrong)");

                if(pageIndex < expectedPageCount - 1)
                {
                    //Expect to see pageSize number of entries.
                    Assert.AreEqual(pageSize, pagedCollectionTester.GetCount(items),
                                    "The page at index " + pageIndex + "Did not have the correct number of records.");
                }
                else
                {
                    Assert.AreEqual(itemsCountOnLastPage, pagedCollectionTester.GetCount(items),
                                    "The last page did not have the correct number of records.");
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
            _hostName = UnitTestHelper.GenerateUniqueString();
            UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, "blog");
        }

        [TearDown]
        public void TearDown()
        {
        }
    }

    internal interface IPagedCollectionTester<TItem>
    {
        void Create(int index);
        IPagedCollection<TItem> GetPagedItems(int pageIndex, int pageSize);
        int GetCount(IPagedCollection<TItem> collection);
    }

    internal class PagedEntryCollectionTester : IPagedCollectionTester<EntryStatsView>
    {
        public void Create(int index)
        {
            UnitTestHelper.Create(UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Title" + index,
                                                                                   "Who rocks the party that rocks the party?"));
        }

        public IPagedCollection<EntryStatsView> GetPagedItems(int pageIndex, int pageSize)
        {
            return ObjectProvider.Instance().GetEntries(PostType.BlogPost, null, pageIndex, pageSize);
        }

        public int GetCount(IPagedCollection<EntryStatsView> collection)
        {
            return collection.Count;
        }
    }

    internal class PagedEntryByCategoryCollectionTester : IPagedCollectionTester<EntryStatsView>
    {
        readonly int _categoryId;

        public PagedEntryByCategoryCollectionTester()
        {
            var category = new LinkCategory {BlogId = Config.CurrentBlog.Id, IsActive = true, Title = "Foobar", Description = "Unit Test"};
            _categoryId = Links.CreateLinkCategory(category);
        }

        public void Create(int index)
        {
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "Title" + index,
                                                                           "Who rocks the party that rocks the party?");
            entry.Categories.Add("Foobar");
            UnitTestHelper.Create(entry);
        }

        public IPagedCollection<EntryStatsView> GetPagedItems(int pageIndex, int pageSize)
        {
            return ObjectProvider.Instance().GetEntries(PostType.BlogPost, _categoryId, pageIndex, pageSize);
        }

        public int GetCount(IPagedCollection<EntryStatsView> collection)
        {
            return collection.Count;
        }
    }

    internal class FeedbackCollectionTester : IPagedCollectionTester<FeedbackItem>
    {
        public void Create(int index)
        {
            var feedbackItem = new FeedbackItem(FeedbackType.Comment)
            {
                Author = "Phil",
                Title = "Title" + index,
                Body = "Who rocks the party that rocks the party? " + index,
                SourceUrl = new Uri("http://blah/")
            };

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Cache).Returns(new TestCache());
            subtextContext.SetupBlog(BlogRequest.Current.Blog);
            subtextContext.SetupRepository(ObjectProvider.Instance());
            subtextContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());
            var commentService = new CommentService(subtextContext.Object, null);
            commentService.Create(feedbackItem, true /*runFilters*/);
            FeedbackItem.Approve(feedbackItem, null);
        }

        public IPagedCollection<FeedbackItem> GetPagedItems(int pageIndex, int pageSize)
        {
            return ObjectProvider.Instance().GetPagedFeedback(pageIndex, pageSize, FeedbackStatusFlag.Approved,
                                                              FeedbackStatusFlag.None, FeedbackType.None);
        }

        public int GetCount(IPagedCollection<FeedbackItem> collection)
        {
            return collection.Count;
        }
    }

    internal class LogEntryCollectionTester : IPagedCollectionTester<LogEntry>
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
                                            ,
                                            new SqlParameter("@Message",
                                                             "This test was brought to you by the letter 'Q'.")
                                            , new SqlParameter("@Exception", "")
                                            , new SqlParameter("@Url", "http://localhost/")
                                        };
            SqlHelper.ExecuteNonQuery(Config.ConnectionString, CommandType.StoredProcedure, "subtext_AddLogEntry",
                                      parameters);
        }

        public IPagedCollection<LogEntry> GetPagedItems(int pageIndex, int pageSize)
        {
            return LoggingProvider.Instance().GetPagedLogEntries(pageIndex, pageSize);
        }

        public int GetCount(IPagedCollection<LogEntry> collection)
        {
            return collection.Count;
        }
    }

    internal class LinkCollectionTester : IPagedCollectionTester<Link>
    {
        readonly int _categoryId;

        public LinkCollectionTester()
        {
            var category = new LinkCategory {BlogId = Config.CurrentBlog.Id, IsActive = true, Title = "Foobar", Description = "Unit Test"};
            _categoryId = Links.CreateLinkCategory(category);

            //Create a couple links that should be ignored because postId is not null.
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("Phil", "title", "in great shape");
            int entryId = UnitTestHelper.Create(entry);
            UnitTestHelper.CreateLinkInDb(_categoryId, "A Forgettable Link", entryId, String.Empty);
            UnitTestHelper.CreateLinkInDb(_categoryId, "Another Forgettable Link", entryId, String.Empty);
            UnitTestHelper.CreateLinkInDb(_categoryId, "Another Forgettable Link", entryId, String.Empty);
        }

        public void Create(int index)
        {
            UnitTestHelper.CreateLinkInDb(_categoryId, "A Link To Remember Part " + index, null, String.Empty);
        }

        public IPagedCollection<Link> GetPagedItems(int pageIndex, int pageSize)
        {
            return ObjectProvider.Instance().GetPagedLinks(_categoryId, pageIndex, pageSize, true);
        }

        public int GetCount(IPagedCollection<Link> collection)
        {
            return collection.Count;
        }
    }

    internal class KeyWordCollectionTester : IPagedCollectionTester<KeyWord>
    {
        public void Create(int index)
        {
            var keyword = new KeyWord
            {
                BlogId = Config.CurrentBlog.Id,
                Text = "The Keyword" + index,
                Title = "Blah",
                Word = "The Word " + index,
                Rel = "Rel" + index,
                Url = "http://localhost/"
            };
            ObjectProvider.Instance().InsertKeyWord(keyword);
        }

        public IPagedCollection<KeyWord> GetPagedItems(int pageIndex, int pageSize)
        {
            return ObjectProvider.Instance().GetPagedKeyWords(pageIndex, pageSize);
        }

        public int GetCount(IPagedCollection<KeyWord> collection)
        {
            return collection.Count;
        }
    }

    internal class BlogCollectionTester : IPagedCollectionTester<Blog>
    {
        readonly string _host = UnitTestHelper.GenerateUniqueString();

        public void Create(int index)
        {
            Config.CreateBlog("title " + index, "phil", "password", _host, "Subfolder" + index);
        }

        public IPagedCollection<Blog> GetPagedItems(int pageIndex, int pageSize)
        {
            return Blog.GetBlogsByHost(_host, pageIndex, pageSize, ConfigurationFlags.IsActive);
        }

        public int GetCount(IPagedCollection<Blog> collection)
        {
            return collection.Count;
        }
    }

    internal class MetaTagCollectionTester : IPagedCollectionTester<MetaTag>
    {
       public void Create(int index)
        {
            var tag = new MetaTag("test" + index) {DateCreated = DateTime.Now, Name = "foo", BlogId = Config.CurrentBlog.Id};
           MetaTags.Create(tag);
        }

        public IPagedCollection<MetaTag> GetPagedItems(int pageIndex, int pageSize)
        {
            return MetaTags.GetMetaTagsForBlog(Config.CurrentBlog, pageIndex, pageSize);
        }

        public int GetCount(IPagedCollection<MetaTag> collection)
        {
            return collection.Count;
        }
    }
}