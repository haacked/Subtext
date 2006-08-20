using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using MbUnit.Framework;
using Subtext.BlogMl;
using Subtext.BlogMl.Interfaces;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Import;

namespace UnitTests.Subtext.Framework.Import
{
	/// <summary>
	/// Unit tests of the BlogImportExport functionality.
	/// </summary>
    [TestFixture]
    public class BlogMlImportTests
    {
		string connectionString = ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString;
		
		[Test]
		[ExpectedException(typeof(BlogDoesNotExistException))]
    	public void WritingInvalidBlogIdThrowsException()
    	{
			SubtextBlogMLWriter writer = new SubtextBlogMLWriter(this.connectionString, int.MaxValue, false);
			writer.Write(XmlWriter.Create(new StringBuilder()));
    	}
		
		public class BlogMlTester : BlogML.BlogMLWriterBase
		{
			protected override void InternalWriteBlog()
			{
				WriteStartBlog("Title", "Subtitle", "RootUrl");
				WriteAuthor("Me", "test@example.com");
				WriteEndElement();
			}
		}

		[Test, Ignore("This test exposes a bug with BlogML!")]
		public void TestBlogML()
		{
			BlogMlTester writer = new BlogMlTester();
			StringBuilder builder = new StringBuilder();
			
			//Going to write xml to a string.
			XmlWriter xml = XmlWriter.Create(builder);
			writer.Write(xml);
			Console.WriteLine(builder.ToString());
		}
		   	
        [Test]
        [RollBack]
        public void ReadBlogCreatesEntriesAndAttachments()
        {
            //Create blog.
			CreateBlogAndSetupContext();
        	
            //Test BlogML reader.
            SubtextBlogMLReader reader = new SubtextBlogMLReader();
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SimpleBlogMl.xml");
            reader.ReadBlog(stream, BlogMlReaderOption.None);

            IList<Entry> entries = Entries.GetRecentPosts(20, PostType.BlogPost, PostConfig.None, true);
            Assert.AreEqual(18, entries.Count, "Did not get the expected number of entries.");

            string[] attachments = Directory.GetFiles(Config.CurrentBlog.ImageDirectory, "*.png");
            Assert.AreEqual(3, attachments.Length, "There should be two file attachments created.");
        }

        [Test]
        [RollBack]
        [Ignore("BlogMl makes web requests. We need to deal with that later.")]
        public void RoundTripBlogMlTest()
        {
            //Create blog.
			CreateBlogAndSetupContext();

            Config.CurrentBlog.ImageDirectory = Path.Combine(Environment.CurrentDirectory, "images");
            Config.CurrentBlog.ImagePath = "/image/";

            //Test BlogML reader.
            SubtextBlogMLReader reader = new SubtextBlogMLReader();
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SimpleBlogMl.xml");
            reader.ReadBlog(stream, BlogMlReaderOption.None);

            IList<Entry> entries = Entries.GetRecentPosts(20, PostType.BlogPost, PostConfig.None, true);

			IBlogMLProvider provider = BlogMLProvider.Instance();
			BlogMlWriter writer = BlogMlWriter.Create(provider);
			writer.EmbedAttachments = true;
            MemoryStream memoryStream = new MemoryStream();
			
        	using (XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
            {
                writer.Write(xmlWriter);
                reader = new SubtextBlogMLReader();
                
                //Create yet another new blog.
                Assert.IsTrue(Config.CreateBlog("BlogML Import Unit Test Blog", "test", "test", Config.CurrentBlog.Host + "1", ""), "Could not create the blog for this test");
                UnitTestHelper.SetHttpContextWithBlogRequest(Config.CurrentBlog.Host + "1", "");
                reader.ReadBlog(memoryStream, BlogMlReaderOption.None);
            }

            IList<Entry> newEntries = Entries.GetRecentPosts(20, PostType.BlogPost, PostConfig.None, true);
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
		
		private void CreateBlogAndSetupContext()
		{
			string hostName = UnitTestHelper.GenerateRandomString();
            Assert.IsTrue(Config.CreateBlog("BlogML Import Unit Test Blog", "test", "test", hostName, ""), "Could not create the blog for this test");
            UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "");
            Assert.IsNotNull(Config.CurrentBlog, "Current Blog is null.");

            Config.CurrentBlog.ImageDirectory = Path.Combine(Environment.CurrentDirectory, "images");
            Config.CurrentBlog.ImagePath = "/image/";
		}
    }
}
