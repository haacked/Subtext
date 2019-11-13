using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Store;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Services.SearchEngine;

namespace UnitTests.Subtext.Framework.Services.SearchEngine
{
    [TestFixture]
    public class SearchEngineServiceTest
    {
        private SearchEngineService _service;
        private string[] stopWords;

        [SetUp]
        public void CreateSearchEngine()
        {
            stopWords = new string[StopAnalyzer.ENGLISH_STOP_WORDS_SET.Values.Count + 1];
            stopWords[0] = "into";
            int i = 1;
            foreach (string value in StopAnalyzer.ENGLISH_STOP_WORDS_SET.Values)
            {
                stopWords[i++] = value;
            }
            _service = new SearchEngineService(new RAMDirectory(), new SnowballAnalyzer("English", stopWords), new FullTextSearchEngineSettings());
        }

        [TearDown]
        public void DestroySearchEngine()
        {
            _service.Dispose();
        }

        [Test]
        public void SearchEngineService_WithEntry_AddsToIndex()
        {
            _service.AddPost(new SearchEngineEntry
            {
                EntryId = 1,
                Body = "This is a sample post",
                Title = "This is the title",
                Tags = "Title",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
                );

            _service.AddPost(new SearchEngineEntry
            {
                EntryId = 2,
                Body = "This is another sample post",
                Title = "This is another title",
                Tags = "Title another",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
            );

            var result = _service.Search("sample", 100, 0) as List<SearchEngineResult>;
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void SearchEngineService_WithEntryNameNull_RetrievesEntryNameNull()
        {
            _service.AddPost(new SearchEngineEntry()
                    {
                        EntryId = 1,
                        Body = "This is a sample post",
                        Title = "This is the title",
                        Tags = "Title",
                        BlogName = "MyTestBlog",
                        IsPublished = true,
                        PublishDate = DateTime.UtcNow,
                        EntryName = null
                    }
                );

            var result = _service.Search("sample", 100, 0) as List<SearchEngineResult>;
            Assert.IsNull(result[0].EntryName);
        }

        [Test]
        public void SearchEngineService_ConvertsToSearchResult()
        {
            _service.AddPost(new SearchEngineEntry()
                {
                    EntryId = 1,
                    Body = "This is a sample post",
                    Title = "This is the title",
                    Tags = "Title",
                    BlogName = "MyTestBlog",
                    IsPublished = true,
                    PublishDate = DateTime.UtcNow,
                    EntryName = "this-is-the-title"
                }
            );

            var result = _service.Search("sample", 100, 0) as List<SearchEngineResult>;

            Assert.AreEqual("This is the title", result[0].Title);
            Assert.AreEqual("MyTestBlog", result[0].BlogName);
            Assert.AreEqual(1, result[0].EntryId);
        }

        [Test]
        public void SearchEngineService_WhenAddingToItemWithSamePostId_UpdatesOriginalEntry()
        {
            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 1,
                Body = "This is a sample post",
                Title = "This is the title",
                Tags = "Title",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
            );

            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 1,
                Body = "This is a post",
                Title = "This is the title",
                Tags = "Title",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
            );

            var result = _service.Search("sample", 100, 0) as List<SearchEngineResult>;
            Assert.AreEqual(0, result.Count);

            result = _service.Search("post", 100, 0) as List<SearchEngineResult>;
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].EntryId);
        }

        [Test]
        public void SearchEngineService_DeletesEntry()
        {
            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 1,
                Body = "This is a sample post",
                Title = "This is the title",
                Tags = "Title",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
            );

            _service.RemovePost(1);

            var result = _service.Search("sample", 100, 0) as List<SearchEngineResult>;
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void SearchEngineService_ReturnsCorrectTotalNumber()
        {
            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 1,
                BlogId = 1,
                Body = "This is a sample post",
                Title = "This is the title",
                Tags = "Title",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
                );

            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 2,
                BlogId = 2,
                Body = "This is another sample post",
                Title = "This is another title",
                Tags = "Title another",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
            );

            int totNumber = _service.GetTotalIndexedEntryCount();
            Assert.AreEqual(2, totNumber);
        }

        [Test]
        public void SearchEngineService_ReturnsCorrectNumberOfPostsByBlog()
        {
            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 1,
                BlogId = 1,
                Body = "This is a sample post",
                Title = "This is the title",
                Tags = "Title",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
                );

            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 2,
                BlogId = 2,
                Body = "This is another sample post",
                Title = "This is another title",
                Tags = "Title another",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
            );

            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 3,
                BlogId = 2,
                Body = "This is another sample post",
                Title = "This is another title",
                Tags = "Title another",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            }
            );

            int postCountBlog1 = _service.GetIndexedEntryCount(1);
            int postCountBlog2 = _service.GetIndexedEntryCount(2);
            Assert.AreEqual(1, postCountBlog1);
            Assert.AreEqual(2, postCountBlog2);
        }

        [Test]
        public void SearchEngineService_PerformsMoreLikeThisSearch()
        {
            for (int i = 1; i <= 10; i++)
            {
                _service.AddPost(new SearchEngineEntry()
                                {
                                    EntryId = i,
                                    Body = "This is a sample post",
                                    Title = "This is the title of the post",
                                    Tags = ".net, mvc, post",
                                    BlogName = "MyTestBlog",
                                    IsPublished = true,
                                    PublishDate = DateTime.UtcNow,
                                    EntryName = "this-is-the-title"
                                }
                );
            }


            var result = _service.RelatedContents(1, 100, 0) as List<SearchEngineResult>;
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public void SearchEngineService_MoreLikeThisSearch_FiltersOriginalDocOut()
        {
            for (int i = 1; i <= 10; i++)
            {
                _service.AddPost(new SearchEngineEntry()
                {
                    EntryId = i,
                    Body = "This is a sample post",
                    Title = "This is the title of the post",
                    Tags = ".net, mvc, post",
                    BlogName = "MyTestBlog",
                    IsPublished = true,
                    PublishDate = DateTime.UtcNow,
                    EntryName = "this-is-the-title"
                }
                );
            }

            var result = _service.RelatedContents(1, 100, 0) as List<SearchEngineResult>;
            Assert.AreEqual(0, result.Count(r => r.EntryId == 1));
        }

        [Test]
        public void SearchEngineService_MoreLikeThisSearch_WithMinDocumentSet_ReturnsEmptySet()
        {
            _service.Dispose();
            _service = new SearchEngineService(new RAMDirectory(), new SnowballAnalyzer("English", stopWords), new FullTextSearchEngineSettings() { Parameters = new TuningParameters() { MinimumDocumentFrequency = 20 } });

            for (int i = 1; i <= 10; i++)
            {
                _service.AddPost(new SearchEngineEntry()
                {
                    EntryId = i,
                    Body = "This is a sample post",
                    Title = "This is the title of the post",
                    Tags = ".net, mvc, post",
                    BlogName = "MyTestBlog",
                    IsPublished = true,
                    PublishDate = DateTime.UtcNow,
                    EntryName = "this-is-the-title"
                }
                );
            }


            var result = _service.RelatedContents(1, 100, 0) as List<SearchEngineResult>;
            Assert.AreEqual(0, result.Count());

        }

        [Test]
        public void SearchEngineService_Search_DoesntIncludeNotActiveEntries()
        {
            for (int i = 1; i <= 10; i++)
            {
                _service.AddPost(new SearchEngineEntry()
                {
                    EntryId = i,
                    Body = "This is a sample post",
                    Title = "This is the title of the post",
                    Tags = ".net, mvc, post",
                    BlogName = "MyTestBlog",
                    IsPublished = true,
                    PublishDate = DateTime.UtcNow,
                    EntryName = "this-is-the-title"
                }
                );
            }

            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 20,
                Body = "This is a sample post",
                Title = "This is the title of the post",
                Tags = ".net, mvc, post",
                BlogName = "MyTestBlog",
                IsPublished = false,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            });


            var result = _service.RelatedContents(1, 100, 0) as List<SearchEngineResult>;
            Assert.AreEqual(0, result.Count(r => r.EntryId == 20));
        }

        [Test]
        public void SearchEngineService_Search_DoesntIncludeFuturePosts()
        {
            for (int i = 1; i <= 10; i++)
            {
                _service.AddPost(new SearchEngineEntry()
                {
                    EntryId = i,
                    Body = "This is a sample post",
                    Title = "This is the title of the post",
                    Tags = ".net, mvc, post",
                    BlogName = "MyTestBlog",
                    IsPublished = true,
                    PublishDate = DateTime.UtcNow,
                    EntryName = "this-is-the-title"
                }
                );
            }

            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 20,
                Body = "This is a sample post",
                Title = "This is the title of the post",
                Tags = ".net, mvc, post",
                BlogName = "MyTestBlog",
                IsPublished = true,
                PublishDate = DateTime.UtcNow.AddDays(1),
                EntryName = "this-is-the-title"
            });


            var result = _service.RelatedContents(1, 100, 0) as List<SearchEngineResult>;
            Assert.AreEqual(0, result.Count(r => r.EntryId == 20));
        }

        [Test]
        public void SearchEngineService_Search_DoesntIncludePostsFromOtherBlogs()
        {
            for (int i = 1; i <= 10; i++)
            {
                _service.AddPost(new SearchEngineEntry()
                {
                    EntryId = i,
                    Body = "This is a sample post",
                    Title = "This is the title of the post",
                    Tags = ".net, mvc, post",
                    BlogName = "MyTestBlog",
                    BlogId = 1,
                    IsPublished = true,
                    PublishDate = DateTime.UtcNow,
                    EntryName = "this-is-the-title"
                }
                );
            }

            _service.AddPost(new SearchEngineEntry()
            {
                EntryId = 20,
                Body = "This is a sample post",
                Title = "This is the title of the post",
                Tags = ".net, mvc, post",
                BlogName = "MyTestBlog",
                BlogId = 2,
                IsPublished = true,
                PublishDate = DateTime.UtcNow,
                EntryName = "this-is-the-title"
            });


            var result = _service.RelatedContents(1, 100, 1) as List<SearchEngineResult>;
            Assert.AreEqual(0, result.Count(r => r.EntryId == 20));
        }

        [Test]
        public void SearchEngineService_Search_WhenAllTheSame_ReturnsCorrectNumberOfHits()
        {
            for (int i = 1; i <= 10; i++)
            {
                _service.AddPost(new SearchEngineEntry()
                {
                    EntryId = i,
                    Body = "This is a sample post",
                    Title = "This is the title of the post",
                    Tags = ".net, mvc, post",
                    BlogName = "MyTestBlog",
                    BlogId = 1,
                    IsPublished = true,
                    PublishDate = DateTime.UtcNow,
                    EntryName = "this-is-the-title"
                }
                );
            }

            var result = _service.RelatedContents(1, 10, 1).ToList<SearchEngineResult>();
            Assert.AreEqual(9, result.Count);
        }

        [Test]
        public void SearchEngineService_Search_WithSpecialSearchCharactersReturnsEmptyList()
        {
            // arrange

            // act
            var results = _service.Search(")", 100, 1).ToList<SearchEngineResult>();

            // assert
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void SearchEngineService_Search_WithBackSlashSearchCharactersReturnsEmptyList()
        {
            // arrange

            // act
            var results = _service.Search(@"\", 100, 1).ToList<SearchEngineResult>();

            // assert
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void SearchEngineService_Search_WithTwoBackSlashSearchCharactersReturnsEmptyList()
        {
            // arrange

            // act
            var results = _service.Search(@"\\", 100, 1).ToList<SearchEngineResult>();

            // assert
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void SearchEngineService_Search_WithStopWordOnlyReturnsEmptyList()
        {
            // arrange

            // act
            var results = _service.Search("into", 100, 1).ToList<SearchEngineResult>();

            // assert
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void SearchEngineService_Search_WithStopWordsAndWhiteSpaceOnlyReturnsEmptyList()
        {
            // arrange

            // act
            var results = _service.Search("into into", 100, 1).ToList<SearchEngineResult>();

            // assert
            Assert.AreEqual(0, results.Count);
        }

    }
}
