using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using MbUnit.Framework;
using BlogML.Xml;
using Subtext.BlogML;
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
