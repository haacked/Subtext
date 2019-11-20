using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Providers
{
    [TestClass]
    public class SearchEngineTests
    {
        [DatabaseIntegrationTestMethod]
        public void Search_WithMultipleMatchingEntries_FindsThoseEntries()
        {
            //arrange
            UnitTestHelper.SetupBlog();
            UnitTestHelper.Create(UnitTestHelper.CreateEntryInstanceForSyndication("author", "whatever 1", "body"));
            UnitTestHelper.Create(UnitTestHelper.CreateEntryInstanceForSyndication("author", "whatever 2",
                                                                                   "the body has some words"));
            var urlHelper = new Mock<BlogUrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/whatever");
            var search = new SearchEngine(Config.CurrentBlog, urlHelper.Object, Config.ConnectionString);

            //act
            ICollection<SearchResult> results = search.Search(Config.CurrentBlog.Id, "words");

            //assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("/whatever", results.First().Url.ToString());
            Assert.AreEqual("whatever 2", results.First().Title);
        }
    }
}