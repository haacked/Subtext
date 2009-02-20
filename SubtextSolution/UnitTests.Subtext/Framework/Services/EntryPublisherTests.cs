using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Framework.Services
{
    [TestFixture]
    public class EntryPublisherTests
    {
        [Test]
        public void Ctor_WithContexts_InitializesNecessaryTransformations() {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            
            //act
            var publisher = new EntryPublisher(context.Object);

            //assert
            var transformations = publisher.Transformations;
            Assert.AreEqual(3, transformations.Count());
        }

        [Test]
        public void Publish_WithTransformations_RunsEachTransformationAgainstEntryBody() {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var transform1 = new Mock<ITextTransformation>();
            transform1.Setup(t => t.Transform(It.IsAny<string>())).Returns<string>(s => s + "t1");
            var transform2 = new Mock<ITextTransformation>();
            transform2.Setup(t => t.Transform(It.IsAny<string>())).Returns<string>(s => s + "t2");
            var publisher = new EntryPublisher(context.Object, new[] { transform1.Object, transform2.Object });
            var entry = new Entry(PostType.BlogPost) { Title="Test", Body = "test" };

            //act
            publisher.Publish(entry);
            
            //assert
            Assert.AreEqual("testt1t2", entry.Body);
        }

        [Test]
        public void Publish_WithEntryTitleButNoSlug_CreatesSlug() {
            //arrange
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };
            var slugGenerator = new Mock<ISlugGenerator>();
            slugGenerator.Setup(g => g.GetSlugFromTitle(entry)).Returns("this-is-a-test");
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null, slugGenerator.Object);

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
            var slugGenerator = new Mock<ISlugGenerator>();
            slugGenerator.Setup(g => g.GetSlugFromTitle(entry)).Returns("this-is-a-test");
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null, slugGenerator.Object);

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
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null)).Callback<Entry, IEnumerable<int>>((e, i) => savedEntry = e);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual(entry, savedEntry);
        }

        [Test]
        public void Publish_WithEntry_SetsDateCreatedToBlogCurrentTimeZoneTime()
        {
            //arrange
            DateTime currentTime = DateTime.Now;
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(currentTime);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual(currentTime, entry.DateCreated);
            //cheating by shoving this extra assert here. MUAHAHAHA!!! ;)
            Assert.IsTrue(NullValue.IsNull(entry.DateSyndicated));
        }

        [Test]
        public void Publish_WithActiveEntry_SetsDateSyndicatedToBlogCurrentTimeZoneTime()
        {
            //arrange
            DateTime currentTime = DateTime.Now;
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(currentTime);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", IsActive = true };

            //act
            publisher.Publish(entry);

            //assert
            Assert.AreEqual(currentTime, entry.DateSyndicated);
        }

        [Test]
        public void Publish_WithEntryHavingCategories_CreatesEntryWithAssociatedCategoryIds()
        {
            //arrange
            DateTime currentTime = DateTime.Now;
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetLinkCategory("category1", true)).Returns(new LinkCategory(11, "category1"));
            repository.Setup(r => r.GetLinkCategory("category2", true)).Returns(new LinkCategory(22, "category2"));
            repository.Setup(r => r.GetLinkCategory("category3", true)).Returns(new LinkCategory(33, "category3"));
            IEnumerable<int> categoryIds = null;
            repository.Setup(r => r.Create(It.IsAny<Entry>(), It.IsAny<IEnumerable<int>>())).Callback<Entry, IEnumerable<int>>((e, ids) => categoryIds = ids);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(currentTime);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test" };
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
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            IEnumerable<string> tagNames = null;
            repository.Setup(r => r.SetEntryTagList(It.IsAny<int>(), It.IsAny<IEnumerable<string>>()))
                .Callback<int, IEnumerable<string>>((i, t) => tagNames = t);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", Body = "" };

            //act
            publisher.Publish(entry);

            //assert
            Assert.IsNotNull(tagNames);
        }

        [Test]
        public void Publish_WithScriptTagsAllowed_AllowsScriptTagInBody()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", Body = "Some <script></script> Body" };
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
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, (IEnumerable<ITextTransformation>)null);

            //act, assert
            UnitTestHelper.AssertThrows<ArgumentNullException>(() => publisher.Publish(null));
        }

        [Test]
        public void Publish_WithEntryHavingPostTypeNone_ThrowsArgumentException()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, (IEnumerable<ITextTransformation>)null);

            //act, assert
            UnitTestHelper.AssertThrows<ArgumentException>(() => publisher.Publish(new Entry(PostType.None)));
        }

        [Test]
        public void Publish_WithDuplicateEntryName_ThrowsException()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            var exception = new Mock<DbException>();
            exception.Setup(e => e.Message).Returns("pick a unique EntryName");
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null)).Throws(exception.Object);
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", EntryName = "test" };

            //act, assert
            UnitTestHelper.AssertThrows<DuplicateEntryException>( () => 
                publisher.Publish(entry)
            );
        }

        [Test]
        public void Publish_WithScriptTag_ThrowsException()
        {
            //arrange
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.Create(It.IsAny<Entry>(), null));
            var context = new Mock<ISubtextContext>();
            context.Setup(c => c.Blog.TimeZone.Now).Returns(DateTime.Now);
            context.Setup(c => c.Repository).Returns(repository.Object);
            var publisher = new EntryPublisher(context.Object, null);
            var entry = new Entry(PostType.BlogPost) { Title = "this is a test", Body = "Some <script></script> Body" };
            Config.Settings.AllowScriptsInPosts = false;

            //act, assert
            UnitTestHelper.AssertThrows<IllegalPostCharactersException>(() =>
                publisher.Publish(entry)
            );
        }
    }
}
