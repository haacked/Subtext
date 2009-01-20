using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using MbUnit.Framework;
using Subtext.BlogML;
using Subtext.BlogML.Interfaces;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.ImportExport;
using Moq;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.HttpModules;

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
            BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object));
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.FieldsTooLong.xml");
            reader.ReadBlog(stream);

            ICollection<Entry> entries = Entries.GetRecentPosts(10, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(1, entries.Count, "Expected only one post.");
            Assert.AreEqual(255, entries.First().Title.Length, "Expected the title to be the max length");
            Assert.AreEqual(150, entries.First().Categories.First().Length, "Expected the category name to be the max length");
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
            BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object));
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.PostWithAuthor.xml");
            reader.ReadBlog(stream);

            ICollection<Entry> entries = Entries.GetRecentPosts(10, PostType.BlogPost, PostConfig.None, false);
            Assert.AreEqual(1, entries.Count, "Expected only one post.");
            Assert.AreEqual("The Author", entries.First().Author, "Expected the title to be the max length");
        }

        [Test]
        [RollBack2]
        public void ReadBlogCreatesEntriesAndAttachments()
        {
            //Create blog.
			UnitTestHelper.CreateBlogAndSetupContext();
        	
            //Test BlogML reader.
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object));
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SimpleBlogMl.xml");
            reader.ReadBlog(stream);

            ICollection<Entry> entries = Entries.GetRecentPosts(20, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(18, entries.Count, "Did not get the expected number of entries.");

            string[] attachments = Directory.GetFiles(Config.CurrentBlog.ImageDirectory, "*.png");
            Assert.AreEqual(3, attachments.Length, "There should be two file attachments created.");
        }

		[Test]
		[RollBack2]
		public void CanReadAndCreateCategories()
		{
			UnitTestHelper.CreateBlogAndSetupContext();

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
			BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object));
			Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.TwoCategories.xml");
			reader.ReadBlog(stream);

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
			BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object));
			Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SinglePostWithCategory.xml");
			reader.ReadBlog(stream);

			ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
			Assert.AreEqual(2, categories.Count, "Expected two total categories to be created");

			ICollection<Entry> entries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
			Assert.AreEqual(1, entries.Count, "Expected a single entry.");
			Assert.AreEqual("Category002", entries.First().Categories.First(), "Expected the catgory to be 'Category002'");
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

			BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object));
			Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SinglePostWithBadCategoryRef.xml");
			reader.ReadBlog(stream);

			ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
			Assert.AreEqual(2, categories.Count, "Expected two total categories to be created");

			ICollection<Entry> entries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
			Assert.AreEqual(1, entries.Count, "Expected a single entry.");
			Assert.AreEqual(0, entries.First().Categories.Count, "Expected this post not to have any categories.");
		}

        [Test]
        [RollBack2]
        [Ignore("Need to rewrite")]
        public void RoundTripBlogMlTest()
        {
			UnitTestHelper.SetupBlog();
            string host = Config.CurrentBlog.Host;

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/whatever");
            // Import /Resources/BlogMl/SimpleBlogMl.xml into the current blog
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.Setup(c => c.Blog).Returns(Config.CurrentBlog);
            subtextContext.Setup(c => c.UrlHelper).Returns(urlHelper.Object);

            BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object));
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SimpleBlogMl.xml");
            reader.ReadBlog(stream);

        	// Confirm the entries
            ICollection<Entry> entries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
        	Assert.AreEqual(18, entries.Count);

            Config.CurrentBlog.ImageDirectory = null;
            
        	// Export this blog.
			IBlogMLProvider provider = new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object);
			BlogMLWriter writer = BlogMLWriter.Create(provider);
            MemoryStream memoryStream = new MemoryStream();
			
        	using (XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
        	{
        		writer.Write(xmlWriter);
				reader = BlogMLReader.Create(new SubtextBlogMLProvider(Config.ConnectionString, subtextContext.Object));
                
                // Now read it back in.
                string newHost = host + "1";
                Config.CreateBlog("BlogML Import Unit Test Blog", "test", "test", newHost, "");
                UnitTestHelper.SetHttpContextWithBlogRequest(newHost, "");
                BlogRequest.Current.Blog = Config.GetBlog(newHost, string.Empty);
        		Assert.IsTrue(Config.CurrentBlog.Host.EndsWith("1"), "Looks like we've cached our old blog.");
				memoryStream.Position = 0;
                Config.CurrentBlog.ImageDirectory = null;
                Config.CurrentBlog.ImagePath = null;
                reader.ReadBlog(memoryStream);
            }

            ICollection<Entry> newEntries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(newEntries.Count, entries.Count, "Round trip failed to create the same number of entries.");
        }

        [SetUp]
        public void Setup()
        {
            //Make sure no files are left over from last time.
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "images")))
            {
                Directory.Delete(Path.Combine(Environment.CurrentDirectory, "images"), true);
            }
        }
        
        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "images")))
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
