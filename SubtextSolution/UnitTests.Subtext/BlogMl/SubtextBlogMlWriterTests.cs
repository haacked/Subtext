using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using MbUnit.Framework;
using Subtext.BlogML;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogML
{
	/// <summary>
	/// Unit tests of the BlogImportExport functionality.
	/// </summary>
	[TestFixture]
	public class SubtextBlogMlWriterTests
	{
		string connectionString = ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString;
		
		/// <summary>
		/// Make sure that when we export a post with a category, that we retain 
		/// the mapping between the post and category.
		/// </summary>
		[Test]
		[RollBack]
		public void CanWritePostWithCategoryAndImportTheOutput()
		{
			CreateBlogAndSetupContext();

			// Shortcut to creating a blog post with a category.
			BlogMLReader reader = BlogMLReader.Create(new SubtextBlogMLProvider());
			Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SinglePostWithCategory.xml");
			reader.ReadBlog(stream);

			BlogMLWriter writer = BlogMLWriter.Create(new SubtextBlogMLProvider());
			writer.EmbedAttachments = false;
            MemoryStream memoryStream = new MemoryStream();

			using (XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
			{
				writer.Write(xmlWriter);

				// Now read it back in to a new blog.
				Assert.IsTrue(Config.CreateBlog("BlogML Import Unit Test Blog", "test", "test", Config.CurrentBlog.Host + "1", ""), "Could not create the blog for this test");
				UnitTestHelper.SetHttpContextWithBlogRequest(Config.CurrentBlog.Host + "2", "");
				Assert.IsTrue(Config.CurrentBlog.Host.EndsWith("2"), "Looks like we've cached our old blog.");
				memoryStream.Position = 0;
				reader.ReadBlog(memoryStream);
			}

			IList<Entry> newEntries = Entries.GetRecentPosts(100, PostType.BlogPost, PostConfig.None, true);
			Assert.AreEqual(1, newEntries.Count, "Round trip failed to create the same number of entries.");
			Assert.AreEqual(1, newEntries[0].Categories.Count, "Expected one category for this entry.");
			Assert.AreEqual("Category002", newEntries[0].Categories[0], "Expected the catgory to be 'Category002'");
		}
		
		[Test]
		[RollBack]
		public void WritingBlogMLWithEntriesContainingNoCategoriesWorks()
		{
			CreateBlogAndSetupContext();

			//Add a few entries.
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "blah blah", "full bodied goodness");
			Entries.Create(entry);

			// Not using BlogMlProvider.Instance() because we need to reset the state.
			SubtextBlogMLProvider provider = new SubtextBlogMLProvider();
			provider.ConnectionString = connectionString;
			
			BlogMLWriter writer = BlogMLWriter.Create(provider);
			writer.EmbedAttachments = false;

			//Note, once the next version of BlogML is released, we can cleanup some of this.
			StringBuilder builder = new StringBuilder();
			StringWriter textWriter = new StringWriter(builder);
			XmlTextWriter xml = new XmlTextWriter(textWriter);
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "  ";
			XmlWriter xmlWriter = XmlWriter.Create(xml);
			writer.Write(xmlWriter);

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(builder.ToString());
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/01/BlogML");

			XmlNodeList postNodes = doc.SelectNodes("//bml:post", nsmgr);
			Assert.AreEqual(1, postNodes.Count);
		}

		[Test]
		[RollBack]
		public void WritingBlogMLWithEverythingWorks()
		{
			CreateBlogAndSetupContext();

			LinkCategory category = new LinkCategory();
			category.Title = "CategoryA";
			category.BlogId = Config.CurrentBlog.Id;
			category.CategoryType = CategoryType.PostCollection;
			category.IsActive = true;
			Links.CreateLinkCategory(category);

			//Add a few entries.
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "blah blah", "full bodied goodness");
			entry.Categories.Add("CategoryA");
			Entries.Create(entry);

			//Add a comment.
			Entry comment = UnitTestHelper.CreateCommentInstance(entry.Id, "joe", "re: blah", UnitTestHelper.GenerateRandomString(), DateTime.Now);
			Entries.CreateComment(comment);

			//Add a trackback.
			Trackback trackback = new Trackback(entry.Id, "blah", "http://example.com/", "you", "your post is great" + UnitTestHelper.GenerateRandomString());
			Entries.Create(trackback);

			//setup provider
			// Not using BlogMlProvider.Instance() because we need to reset the state.
			SubtextBlogMLProvider provider = new SubtextBlogMLProvider();
			provider.ConnectionString = connectionString;
			BlogMLWriter writer = BlogMLWriter.Create(provider);
			writer.EmbedAttachments = false;

			//Note, once the next version of BlogML is released, we can cleanup some of this.
			StringBuilder builder = new StringBuilder();
			StringWriter textWriter = new StringWriter(builder);
			XmlTextWriter xml = new XmlTextWriter(textWriter);
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "  ";
			XmlWriter xmlWriter = XmlWriter.Create(xml);
			writer.Write(xmlWriter);

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(builder.ToString());
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/01/BlogML");

			Console.WriteLine(doc.InnerXml);
			XmlNode postNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='1']", nsmgr);
			Assert.IsNotNull(postNode, "The post node is null");

			XmlNode firstPostCategoryNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='1']/bml:categories/bml:category", nsmgr);
			Assert.IsNotNull(firstPostCategoryNode, "Expected a category for the first post");

			XmlNode firstPostCommentNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='1']/bml:comments/bml:comment", nsmgr);
			Assert.IsNotNull(firstPostCommentNode, "Expected a comment for the first post");

			XmlNode firstPostTrackbackNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='1']/bml:trackbacks/bml:trackback", nsmgr);
			Assert.IsNotNull(firstPostTrackbackNode, "Expected a trackback for the first post");
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
