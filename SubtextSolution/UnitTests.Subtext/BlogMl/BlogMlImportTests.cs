using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.ImportExport;

namespace UnitTests.Subtext.Framework.Import
{
    /// <summary>
    /// Unit tests of the BlogImportExport functionality.
    /// </summary>
    [TestFixture]
    public class BlogMLImportTests
    {
        [Test]
        [RollBack2]
        public void CanImportAndTruncateTooLongFields()
        {
            //Create blog.
            UnitTestHelper.CreateBlogAndSetupContext();

            //Test BlogML reader.
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            IEntryPublisher entryPublisher = UnitTestHelper.CreateEntryPublisher(subtextContext.Object);
            var commentService = new CommentService(subtextContext.Object, null);
            var importService = new SubtextBlogMlImportService(subtextContext.Object, commentService, entryPublisher);
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.FieldsTooLong.xml");

            // act
            importService.ImportBlog(new BlogMLReader(), stream);

            ICollection<Entry> entries = ObjectProvider.Instance().GetEntries(10, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(1, entries.Count, "Expected only one post.");
            Assert.AreEqual(255, entries.First().Title.Length, "Expected the title to be the max length");
            Assert.AreEqual(150, entries.First().Categories.First().Length,
                            "Expected the category name to be the max length");
            Assert.AreEqual(50, entries.First().Author.Length, "Expected the author name to be the max length");
        }

        [Test]
        [RollBack2]
        public void CanImportPostWithAuthor()
        {
            //Create blog.
            UnitTestHelper.CreateBlogAndSetupContext();

            //Test BlogML reader.
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            IEntryPublisher entryPublisher = UnitTestHelper.CreateEntryPublisher(subtextContext.Object);
            var commentService = new CommentService(subtextContext.Object, null);
            var importService = new SubtextBlogMlImportService(subtextContext.Object, commentService, entryPublisher);
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.PostWithAuthor.xml");
            
            // act
            importService.ImportBlog(new BlogMLReader(), stream);

            // assert
            ICollection<Entry> entries = ObjectProvider.Instance().GetEntries(10, PostType.BlogPost, PostConfig.None, false);
            Assert.AreEqual(1, entries.Count, "Expected only one post.");
            Assert.AreEqual("The Author", entries.First().Author, "Expected the title to be the max length");
        }

        [Test]
        [RollBack2]
        [Ignore("Need to rewrite")]
        public void ReadBlogCreatesEntriesAndAttachments()
        {
            //Create blog.
            UnitTestHelper.CreateBlogAndSetupContext();

            //Test BlogML reader.
            var subtextContext = new Mock<ISubtextContext>();
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            IEntryPublisher entryPublisher = UnitTestHelper.CreateEntryPublisher(subtextContext.Object);
            var commentService = new CommentService(subtextContext.Object, null);
            var importService = new SubtextBlogMlImportService(subtextContext.Object, commentService, entryPublisher);
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SimpleBlogMl.xml");

            // act
            importService.ImportBlog(new BlogMLReader(), stream);

            ICollection<Entry> entries = ObjectProvider.Instance().GetEntries(20, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(18, entries.Count, "Did not get the expected number of entries.");

            //string[] attachments = Directory.GetFiles(Config.CurrentBlog.ImageDirectory, "*.png");
            //Assert.AreEqual(3, attachments.Length, "There should be two file attachments created.");
        }

        [Test]
        [RollBack2]
        public void CanReadAndCreateCategories()
        {
            UnitTestHelper.CreateBlogAndSetupContext();

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            IEntryPublisher entryPublisher = UnitTestHelper.CreateEntryPublisher(subtextContext.Object);
            var commentService = new CommentService(subtextContext.Object, null);
            var importService = new SubtextBlogMlImportService(subtextContext.Object, commentService, entryPublisher);
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.TwoCategories.xml");

            // act
            importService.ImportBlog(new BlogMLReader(), stream);

            ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
            Assert.AreEqual(2, categories.Count, "Expected two categories to be created");
        }

        [Test]
        [RollBack2]
        [Ignore("Need to rewrite this test")]
        public void CanPostAndReferenceCategoryAppropriately()
        {
            UnitTestHelper.CreateBlogAndSetupContext();

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            IEntryPublisher entryPublisher = UnitTestHelper.CreateEntryPublisher(subtextContext.Object);
            var commentService = new CommentService(subtextContext.Object, null);
            var importService = new SubtextBlogMlImportService(subtextContext.Object, commentService, entryPublisher);
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SinglePostWithCategory.xml");

            importService.ImportBlog(new BlogMLReader(), stream);

            ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
            Assert.AreEqual(2, categories.Count, "Expected two total categories to be created");

            ICollection<Entry> entries = ObjectProvider.Instance().GetEntries(100, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(1, entries.Count, "Expected a single entry.");
            Assert.AreEqual("Category002", entries.First().Categories.First(),
                            "Expected the catgory to be 'Category002'");
        }

        /// <summary>
        /// When importing some blogml. If the post references a category that 
        /// doesn't exist, then we just don't add that category.
        /// </summary>
        [Test]
        [RollBack2]
        public void ImportOfPostWithBadCategoryRefHandlesGracefully()
        {
            UnitTestHelper.CreateBlogAndSetupContext();
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            //TODO: FIX!!!
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            IEntryPublisher entryPublisher = UnitTestHelper.CreateEntryPublisher(subtextContext.Object);
            var commentService = new CommentService(subtextContext.Object, null);

            var importService = new SubtextBlogMlImportService(subtextContext.Object, commentService, entryPublisher);
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SinglePostWithBadCategoryRef.xml");

            importService.ImportBlog(new BlogMLReader(), stream);

            ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
            Assert.AreEqual(2, categories.Count, "Expected two total categories to be created");

            ICollection<Entry> entries = ObjectProvider.Instance().GetEntries(100, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(1, entries.Count, "Expected a single entry.");
            Assert.AreEqual(0, entries.First().Categories.Count, "Expected this post not to have any categories.");
        }

        [SetUp]
        public void Setup()
        {
            //Make sure no files are left over from last time.
            if(Directory.Exists(Path.Combine(Environment.CurrentDirectory, "images")))
            {
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "images"), true);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if(Directory.Exists(Path.Combine(Environment.CurrentDirectory, "images")))
            {
                try
                {
                    Directory.Delete(Path.Combine(Environment.CurrentDirectory, "images"), true);
                    Console.WriteLine("Deleted " + Path.Combine(Environment.CurrentDirectory, "images"));
                }
                catch(Exception)
                {
                    Console.WriteLine("Could not delete " + Path.Combine(Environment.CurrentDirectory, "images"));
                }
            }
        }
    }
}