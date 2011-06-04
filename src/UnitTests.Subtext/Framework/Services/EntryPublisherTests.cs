using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using MbUnit.Framework;
using Moq;
using Subtext.Configuration;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Services.SearchEngine;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class EntryPublisherTests
    {
        [Test]
        public void Ctor_WithNullContext_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(
                () =>
                new EntryPublisher(null, EmptyTextTransformation.Instance,
                                   new SlugGenerator(FriendlyUrlSettings.Settings), null));
        }

        [Test]
        public void Publish_WithTransformations_RunsTransformationAgainstEntryBody()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var transform = new Mock<ITextTransformation>();
            var searchengine = new Mock<IIndexingService>();
            transform.Setup(t => t.Transform(It.IsAny<string>())).Returns<string>(s => s + "t1");
            var publisher = new EntryPublisher(context.Object, transform.Object, null, searchengine.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "Test", Body = "test" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual("testt1", entry.Body);
        }

        [Test]
        public void Publish_WithEntryTitleButNoSlug_CreatesSlug()
        {
            //arrange
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };
            var slugGenerator = new Mock<ISlugGenerator>();
            slugGenerator.Setup(g => g.GetSlugFromTitle(entry)).Returns("this-is-a-test");
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            var searchengine = new Mock<IIndexingService>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null, slugGenerator.Object, searchengine.Object);

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual("this-is-a-test", entry.EntryName);
        }

        [Test]
        public void Publish_WithEntryTitleAndSlug_DoesNotOverideSlug()
        {
            //arrange
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", EntryName = "testing" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };
            var slugGenerator = new Mock<ISlugGenerator>();
            slugGenerator.Setup(g => g.GetSlugFromTitle(entry)).Returns("this-is-a-test");
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchengine = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, slugGenerator.Object, searchengine.Object);

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual("testing", entry.EntryName);
        }

        [Test]
        public void Publish_WithEntry_SavesInRepository()
        {
            //arrange
            Entry savedEntry = null;
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null)).Callback<Entry, IEnumerable<int>>(
                (e, i) => savedEntry = e);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchengine = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchengine.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual(entry, savedEntry);
        }

        [Test]
        public void Publish_WithEntry_SetsDateCreatedToUtc()
        {
            //arrange
            DateTime currentTime = DateTime.UtcNow;
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchengine = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchengine.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", IsActive = false };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.IsTrue(entry.DateCreatedUtc >= currentTime);
            Assert.IsTrue(entry.DateCreatedUtc <= DateTime.UtcNow);
            //cheating by shoving these extra asserts here. MUAHAHAHA!!! ;)
            Assert.IsTrue(entry.DatePublishedUtc.IsNull());
            Assert.IsTrue(entry.DateSyndicated.IsNull());
        }

        [Test]
        public void Publish_WithActiveEntryAndIncludeInSyndication_SetsDatePublishedUtcToUtc()
        {
            //arrange
            var currentTime = DateTime.UtcNow;
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchengine = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchengine.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", IsActive = true, IncludeInMainSyndication = true };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.IsTrue(entry.DatePublishedUtc >= currentTime);
            Assert.IsTrue(entry.DatePublishedUtc <= DateTime.UtcNow);
        }

        [Test]
        public void Publish_WithEntryHavingCategories_CreatesEntryWithAssociatedCategoryIds()
        {
            //arrange
            DateTime currentTime = DateTime.UtcNow;
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetLinkCategory("category1", true)).Returns(new LinkCategory(11, "category1"));
            repository.Setup(r => r.GetLinkCategory("category2", true)).Returns(new LinkCategory(22, "category2"));
            repository.Setup(r => r.GetLinkCategory("category3", true)).Returns(new LinkCategory(33, "category3"));
            IEnumerable<int> categoryIds = null;
            repository.Setup(r => r.Create(It.IsAny<Entry>(), It.IsAny<IEnumerable<int>>())).Callback
                <Entry, IEnumerable<int>>((e, ids) => categoryIds = ids);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchengine = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchengine.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };
            entry.Categories.Add("category1");
            entry.Categories.Add("category2");
            entry.Categories.Add("category3");

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual(11, categoryIds.First());
            Assert.AreEqual(22, categoryIds.ElementAt(1));
            Assert.AreEqual(33, categoryIds.ElementAt(2));
        }

        [Test]
        public void Publish_WithEntryBodyHavingTags_SetsEntryTags()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            IEnumerable<string> tagNames = null;
            repository.Setup(r => r.SetEntryTagList(It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Callback<int, IEnumerable<string>>((i, t) => tagNames = t);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchengine = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchengine.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", Body = "" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.IsNotNull(tagNames);
        }

        [Test]
        public void Publish_WithEntry_AddsToSearchEngine()
        {
            //arrange
            var searchEngineService = new Mock<IIndexingService>();
            Entry searchEngineEntry = null;
            searchEngineService.Setup(s => s.AddPost(It.IsAny<Entry>(), It.IsAny<IList<string>>()))
                .Callback<Entry, IList<string>>((e, l) => searchEngineEntry = e);
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", Body = "", IsActive = true };
            entry.Blog = new Blog() { Title = "MyTestBlog" };


            //act
            publisher.Publish(entry);

            //assert
            Assert.IsNotNull(searchEngineEntry);
        }


        [Test]
        public void Publish_WithScriptTagsAllowed_AllowsScriptTagInBody()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", Body = "Some <script></script> Body" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };
            Config.Settings.AllowScriptsInPosts = true;

            //act
            publisher.Publish(entry);

            //assert
            //no exception thrown.
        }

        [Test]
        public void Publish_WithNullEntry_ThrowsArgumentNullException()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);

            //act, assert
            UnitTestHelper.AssertThrowsArgumentNullException(() => publisher.Publish(null));
        }

        [Test]
        public void Publish_WithEntryHavingPostTypeNone_ThrowsArgumentException()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);

            //act, assert
            UnitTestHelper.AssertThrows<ArgumentException>(() => publisher.Publish(new Entry(PostType.None)));
        }

        [Test]
        public void Publish_WithDuplicateEntryName_ThrowsException()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            var exception = new Mock<DbException>();
            exception.Setup(e => e.Message).Returns("pick a unique EntryName");
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null)).Throws(exception.Object);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", EntryName = "test" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act, assert
            UnitTestHelper.AssertThrows<DuplicateEntryException>(() =>
                                                                 publisher.Publish(entry)
                );
        }

        [Test]
        public void Publish_WithRepositoryThrowingException_PropagatesException()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            var exception = new Mock<DbException>();
            exception.Setup(e => e.Message).Returns("unknown");
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null)).Throws(exception.Object);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", EntryName = "test" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act, assert
            UnitTestHelper.AssertThrows<DbException>(() =>
                                                     publisher.Publish(entry)
                );
        }

        [Test]
        public void Publish_WithScriptTagInBody_ThrowsException()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", Body = "Some <script></script> Body" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };
            Config.Settings.AllowScriptsInPosts = false;

            //act, assert
            UnitTestHelper.AssertThrows<IllegalPostCharactersException>(() =>
                                                                        publisher.Publish(entry)
                );
        }

        [Test]
        public void Publish_WithScriptTagInTitle_ThrowsException()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test<script></script>", Body = "Some Body" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };
            Config.Settings.AllowScriptsInPosts = false;

            //act, assert
            UnitTestHelper.AssertThrows<IllegalPostCharactersException>(() =>
                                                                        publisher.Publish(entry)
                );
        }

        [Test]
        public void Publish_WithScriptTagInSlug_ThrowsException()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "stuff", EntryName = "<script></script>", Body = "Some Body" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };
            Config.Settings.AllowScriptsInPosts = false;

            //act, assert
            UnitTestHelper.AssertThrows<IllegalPostCharactersException>(() =>
                                                                        publisher.Publish(entry)
                );
        }

        [Test]
        public void Publish_WithScriptTagInDescription_ThrowsException()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, null, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", Body = "Whatever", Description = "Some <script></script> Body" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };
            Config.Settings.AllowScriptsInPosts = false;

            //act, assert
            UnitTestHelper.AssertThrows<IllegalPostCharactersException>(() =>
                                                                        publisher.Publish(entry)
                );
        }

        [Test]
        public void Publish_WithEntryHavingValidEntryName_DoesNotChangeEntryName()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var transform = new Mock<ITextTransformation>();
            transform.Setup(t => t.Transform(It.IsAny<string>())).Returns<string>(s => s);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, transform.Object, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "Test", Body = "test", EntryName = "original-entry-name" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual("original-entry-name", entry.EntryName);
        }

        [Test]
        public void Publish_WithEntryHavingNumericIntegerEntryName_PrependsNUnderscore()
        {
            //arrange
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Repository).Returns(repository.Object);
            var transform = new Mock<ITextTransformation>();
            transform.Setup(t => t.Transform(It.IsAny<string>())).Returns<string>(s => s);
            var searchEngineService = new Mock<IIndexingService>();
            var publisher = new EntryPublisher(context.Object, transform.Object, null, searchEngineService.Object);
            var entry = new Entry(PostType.BlogPost) { Title = "Test", Body = "test", EntryName = "4321" };
            entry.Blog = new Blog() { Title = "MyTestBlog" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual("n_4321", entry.EntryName);
        }
    }
}