using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using Lucene.Net.Util;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Services.SearchEngine;

namespace UnitTests.Subtext.Framework.Services.SearchEngine
{
    [TestFixture]
    public class IndexingServiceTests
    {
        private SearchEngineService _service;
        
        [SetUp]
        public void CreateSearchEngine()
        {
            _service = new SearchEngineService(new RAMDirectory(), new StandardAnalyzer(Version.LUCENE_29), new FullTextSearchEngineSettings());
        }
        
        [TearDown]
        public void DestroySearchEngine()
        {
            _service.Dispose();
        }

        [Test]
        public void RebuildIndex_LoadEntriesFromRepository()
        {
            var context = new Mock<ISubtextContext>();
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntries(PostType.BlogPost, null, It.IsAny<int>(), It.IsAny<int>())).Returns(
                new PagedCollection<EntryStatsView>());
            context.Setup(c => c.Repository).Returns(repository.Object);

            var indexService = new IndexingService(context.Object, _service);

            indexService.RebuildIndex();

            repository.Verify(rep => rep.GetEntries(PostType.BlogPost, null, It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public void RebuildIndex_AddsDataToIndex()
        {
            var context = new Mock<ISubtextContext>();
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntries(PostType.BlogPost, null, It.IsAny<int>(), It.IsAny<int>())).Returns(
                BuildFakeCollection());
            context.Setup(c => c.Repository).Returns(repository.Object);

            var indexService = new IndexingService(context.Object, _service);

            indexService.RebuildIndex();

            Assert.AreEqual(1,_service.GetTotalIndexedEntryCount());
        }

        [Test]
        public void RebuildIndex_WithEntryNotPublished_DoesntAddsDataToIndex()
        {
            var context = new Mock<ISubtextContext>();
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntries(PostType.BlogPost, null, It.IsAny<int>(), It.IsAny<int>())).Returns(
                BuildFakeCollectionNotPublished());
            context.Setup(c => c.Repository).Returns(repository.Object);

            var indexService = new IndexingService(context.Object, _service);

            indexService.RebuildIndex();

            Assert.AreEqual(0, _service.GetTotalIndexedEntryCount());
        }


        [Test]
        public void IndexService_WithPublishedPost_AddsPostToIndex()
        {
            var context = new Mock<ISubtextContext>();
            var searchEngine = new Mock<ISearchEngineService>();
            SearchEngineEntry entry = null;
            searchEngine.Setup(s => s.AddPost(It.IsAny<SearchEngineEntry>())).Callback<SearchEngineEntry>(e => entry = e);

            var indexService = new IndexingService(context.Object, searchEngine.Object);

            var blogEntry = new Entry(PostType.BlogPost)
                            {
                                Title ="Sample Post",
                                Blog = new Blog() { Title = "My Blog" },
                                IsActive=true,
                            };

            indexService.AddPost(blogEntry);
            Assert.IsNotNull(entry);
        }

        [Test]
        public void IndexService_WithNotPublishedPost_DoesntAddsPostToIndex()
        {
            var context = new Mock<ISubtextContext>();
            var searchEngine = new Mock<ISearchEngineService>();
            searchEngine.Setup(s => s.AddPost(It.IsAny<SearchEngineEntry>())).Never();

            var indexService = new IndexingService(context.Object, searchEngine.Object);

            var entry = new Entry(PostType.BlogPost)
            {
                Title = "Sample Post",
                Blog = new Blog() { Title = "My Blog" },
                IsActive = false,
            };

            indexService.AddPost(entry);
        }

        [Test]
        public void IndexService_WithNotPublishedPost_RemovesPostFromIndex()
        {
            var context = new Mock<ISubtextContext>();
            var searchEngine = new Mock<ISearchEngineService>();
            bool deleted = false;
            searchEngine.Setup(s => s.RemovePost(It.IsAny<int>())).Callback(() => deleted = true);

            var indexService = new IndexingService(context.Object, searchEngine.Object);

            var entry = new Entry(PostType.BlogPost)
            {
                Title = "Sample Post",
                Blog = new Blog() { Title = "My Blog" },
                IsActive = false,
            };

            indexService.AddPost(entry);
            Assert.IsTrue(deleted);
        }

        private PagedCollection<EntryStatsView> BuildFakeCollection()
        {
            var coll = new PagedCollection<EntryStatsView>();
            coll.Add(new EntryStatsView()
                         {
                             Title = "My Post Title",
                             EntryName = "this-is-the-title",
                             IsActive = true,
                             Blog = new Blog(){Title = "My Blog"}
                         });
            return coll;
        }

        private PagedCollection<EntryStatsView> BuildFakeCollectionNotPublished()
        {
            var coll = new PagedCollection<EntryStatsView>();
            coll.Add(new EntryStatsView()
            {
                Title = "My Post Title",
                EntryName = "this-is-the-title",
                IsActive = false,
                Blog = new Blog() { Title = "My Blog" }
            });
            return coll;
        }
    }
}
