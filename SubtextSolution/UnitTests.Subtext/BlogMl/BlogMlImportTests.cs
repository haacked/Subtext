using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Xml;
using MbUnit.Framework;
using Subtext.BlogML;
using Subtext.BlogML.Interfaces;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
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
		[ExtractResource("UnitTests.Subtext.Resources.BlogMl.TwoCategories.xml", typeof(BlogMLImportTests))]
		[RollBack2]
		public void CanReadAndCreateCategories()
		{
			UnitTestHelper.SetupBlog();

			BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider());
			reader.ReadBlog(ExtractResourceAttribute.Stream);

			ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
			Assert.AreEqual(2, categories.Count, "Expected two categories to be created");
		}

		[Test]
		[ExtractResource("UnitTests.Subtext.Resources.BlogMl.SimpleBlogMl.xml", typeof(BlogMLImportTests))]
		[RollBack2]
		public void ReadBlogCreatesEntriesAndAttachments()
		{
            //Create blog.
			UnitTestHelper.SetupBlog();
        	
            //Test BlogML reader.
            BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider());
            Stream stream = ExtractResourceAttribute.Stream;
            reader.ReadBlog(stream);

            IList<Entry> entries = Entries.GetRecentPosts(20, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(18, entries.Count, "Did not get the expected number of entries.");

            string[] attachments = Directory.GetFiles(Config.CurrentBlog.ImageDirectory, "*.png");
            Assert.AreEqual(3, attachments.Length, "There should be two file attachments created.");
        }

		[Test]
		[RollBack2]
		public void CanPostAndReferenceCategoryAppropriately()
		{
			UnitTestHelper.SetupBlog();

			BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider());
			Stream stream = ExtractResourceAttribute.Stream;
			reader.ReadBlog(stream);

			ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
			Assert.AreEqual(2, categories.Count, "Expected two total categories to be created");

			IList<Entry> entries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
			Assert.AreEqual(1, entries.Count, "Expected a single entry.");
			Assert.AreEqual("Category002", entries[0].Categories[0], "Expected the catgory to be 'Category002'");
		}

		/// <summary>
		/// When importing some blogml. If the post references a category that 
		/// doesn't exist, then we just don't add that category.
		/// </summary>
		[Test]
		[ExtractResource("UnitTests.Subtext.Resources.BlogMl.SinglePostWithBadCategoryRef.xml", typeof(BlogMLImportTests))]
		[RollBack2]
		public void ImportOfPostWithBadCategoryRefHandlesGracefully()
		{
			UnitTestHelper.SetupBlog();

			BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider());
			Stream stream = ExtractResourceAttribute.Stream;
			reader.ReadBlog(stream);

			ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
			Assert.AreEqual(2, categories.Count, "Expected two total categories to be created");

			IList<Entry> entries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
			Assert.AreEqual(1, entries.Count, "Expected a single entry.");
			Assert.AreEqual(0, entries[0].Categories.Count, "Expected this post not to have any categories.");
		}

        [Test]
		[ExtractResource("UnitTests.Subtext.Resources.BlogMl.SimpleBlogMl.xml", typeof(BlogMLImportTests))]
        [RollBack2]
        public void RoundTripBlogMlTest()
        {
			UnitTestHelper.CreateBlogAndSetupContext();

            // Import /Resources/BlogMl/SimpleBlogMl.xml into the current blog
			BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider());
			Stream stream = ExtractResourceAttribute.Stream;
            reader.ReadBlog(stream);

        	// Confirm the entries
            IList<Entry> entries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
        	Assert.AreEqual(18, entries.Count);

        	// Export this blog.
			IBlogMLProvider provider = BlogMLProvider.Instance();
        	provider.ConnectionString = Config.ConnectionString;
			BlogMLWriter writer = BlogMLWriter.Create(provider);
            MemoryStream memoryStream = new MemoryStream();

        	IDisposable blogRequest;
        	using (XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
        	{
        		writer.Write(xmlWriter);
				reader = BlogMLReader.Create(new SubtextBlogMLProvider());
                
                // Now read it back in.
				MembershipUser owner = Membership.CreateUser(UnitTestHelper.MembershipTestUsername, "test", UnitTestHelper.MembershipTestEmail);
                BlogInfo blog = Config.CreateBlog("BlogML Import Unit Test Blog", Config.CurrentBlog.Host + "1", "", owner);
				blogRequest = BlogRequestSimulator.SimulateRequest(blog, blog.Host, "", "");
				Assert.IsTrue(Config.CurrentBlog.Host.EndsWith("1"), "Looks like we've cached our old blog.");
				memoryStream.Position = 0;
				reader.ReadBlog(memoryStream);
        	}

            IList<Entry> newEntries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(newEntries.Count, entries.Count, "Round trip failed to create the same number of entries.");
			if (blogRequest != null)
				blogRequest.Dispose();
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
