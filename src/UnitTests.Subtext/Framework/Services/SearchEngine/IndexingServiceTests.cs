using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
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
            _service = new SearchEngineService(new RAMDirectory(), new StandardAnalyzer(), new FullTextSearchEngineSettings());
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
            var repository = new Mock<ObjectProvider>();
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
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntries(PostType.BlogPost, null, It.IsAny<int>(), It.IsAny<int>())).Returns(
                BuildPreCookedCollection());
            context.Setup(c => c.Repository).Returns(repository.Object);

            var indexService = new IndexingService(context.Object, _service);

            indexService.RebuildIndex();

            Assert.AreEqual(1,_service.GetTotalIndexedEntryCount());
        }

        private PagedCollection<EntryStatsView> BuildPreCookedCollection()
        {
            var coll = new PagedCollection<EntryStatsView>();
            coll.Add(new EntryStatsView()
                         {
                             Title = "My Post Title",
                             EntryName = "this-is-the-title",
                             Blog = new Blog(){Title = "My Blog"}
                         });
            return coll;
        }
    }
}
