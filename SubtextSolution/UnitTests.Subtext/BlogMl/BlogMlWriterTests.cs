using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using MbUnit.Framework;
using BlogML.Xml;
using Rhino.Mocks;
using Subtext.BlogML;
using Subtext.BlogML.Conversion;
using Subtext.BlogML.Interfaces;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;
using Subtext.ImportExport;
using Subtext.ImportExport.Conversion;

namespace UnitTests.Subtext.BlogML
{
	[TestFixture]
	public class BlogMlWriterTests
	{
		[Test]
		[ExpectedArgumentNullException]
		public void CreateRequiresProvider()
		{
			BlogMLWriter.Create(null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CreateRequiresContext()
		{
			MockRepository mocks = new MockRepository();
			IBlogMLProvider provider = (IBlogMLProvider)mocks.DynamicMock(typeof(IBlogMLProvider));
			mocks.ReplayAll();
			BlogMLWriter.Create(provider);
			mocks.VerifyAll();
		}
		
		[Test]
		public void CreateWithProperContextReturnsWriter()
		{
			BlogMLContext context = new BlogMLContext("8675309", false);
			
			MockRepository mocks = new MockRepository();
			IBlogMLProvider provider = (IBlogMLProvider)mocks.CreateMock(typeof(IBlogMLProvider));
			Expect.Call(provider.GetBlogMlContext()).Return(context);
			Expect.Call(provider.IdConversion).Return(IdConversionStrategy.Empty);
			mocks.ReplayAll();
			BlogMLWriter writer = BlogMLWriter.Create(provider);
			Assert.IsNotNull(writer);
			mocks.VerifyAll();
		}
		
		[Test]
		public void WritesProperBlogMl()
		{
			//Set up ids
			string blogId = "8675309";
			string catOneId = "1";
			string catTwoId = "2";
			string postOneId = "100";
			string postTwoId = "200";
			string postThreeId = "300";
			string commentOneId = "91";
			string trackBackOneId = "92";
			
			int pageSize = 2;

			#region Create a full instance of a blog in an object hierarchy
			DateTime dateCreated = DateTime.Now;
			BlogMLBlog blog = ObjectHydrator.CreateBlogInstance("the title", "the subtitle", "http://blog.example.com/", "phil", "test@example.com", dateCreated);
			BlogMLContext context = new BlogMLContext(blogId, false);
			ICollection<BlogMLCategory> categories = new Collection<BlogMLCategory>();
			categories.Add(ObjectHydrator.CreateCategoryInstance(catOneId, "category1", "Category 1 is the first", true, null, dateCreated, dateCreated));
			categories.Add(ObjectHydrator.CreateCategoryInstance(catTwoId, "category2", "Category 2 is the second", true, null, dateCreated, dateCreated));

			IPagedCollection<BlogMLPost> firstPage = new PagedCollection<BlogMLPost>();
			firstPage.Add(ObjectHydrator.CreatePostInstance(postOneId, "post title 1", "http://blog.example.com/post100/", true, "Nothing important 1", dateCreated, dateCreated));
			firstPage[0].Categories.Add(catOneId);
			firstPage[0].Categories.Add(catTwoId);
			firstPage[0].Comments.Add(ObjectHydrator.CreateCommentInstance(commentOneId, "re: post", "http://blog.example.com/post100/#91", "You rock!", "test@example.com", "haacked", true, dateCreated, dateCreated));
			firstPage[0].Trackbacks.Add(ObjectHydrator.CreateTrackBackInstance(trackBackOneId, "re: title", "http://another.example.com/", true, dateCreated, dateCreated));
			firstPage.Add(ObjectHydrator.CreatePostInstance(postTwoId, "post title 2", "http://blog.example.com/post200/", true, "Nothing important 2", dateCreated, dateCreated));
			firstPage.MaxItems = 3;

			IPagedCollection<BlogMLPost> secondPage = new PagedCollection<BlogMLPost>();
			secondPage.Add(ObjectHydrator.CreatePostInstance(postThreeId, "post title 3", "http://blog.example.com/post300/", true, "Nothing important 3", dateCreated, dateCreated));
			secondPage.MaxItems = 3;
			#endregion

			//Now setup expectations.
			MockRepository mocks = new MockRepository();
			IBlogMLProvider provider = (IBlogMLProvider)mocks.CreateMock(typeof(IBlogMLProvider));
			Expect.Call(provider.GetBlogMlContext()).Return(context);
			Expect.Call(provider.IdConversion).Return(IdConversionStrategy.Empty);
			Expect.Call(provider.GetBlog(blogId)).Return(blog);
			Expect.Call(provider.GetAllCategories(blogId)).Return(categories);
			Expect.Call(provider.PageSize).Return(pageSize);
			Expect.Call(provider.GetBlogPosts(blogId, 0, pageSize)).Return(firstPage);
			Expect.Call(provider.GetBlogPosts(blogId, 1, pageSize)).Return(secondPage);
			mocks.ReplayAll();

			//TODO: simplify when BlogML bug is fixed.
			StringBuilder builder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(builder);
			XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

			BlogMLWriter writer = BlogMLWriter.Create(provider);
			writer.Write(xmlWriter);

			mocks.VerifyAll();
		
			//Verify blog.
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(builder.ToString());
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/09/BlogML");

			XmlNode blogNode = doc.SelectSingleNode("bml:blog", nsmgr);
			Assert.AreEqual("http://blog.example.com/", blogNode.Attributes["root-url"].InnerText, "The root url doesn't match.");

			XmlNodeList categoryNodes = doc.SelectNodes("bml:blog/bml:categories/bml:category", nsmgr);
			Assert.AreEqual(2, categoryNodes.Count, "Expected 2 categories");

			XmlNodeList postNodes = doc.SelectNodes("bml:blog/bml:posts/bml:post", nsmgr);
			Assert.AreEqual(3, postNodes.Count, "Expected 3 posts");

			XmlNodeList firstPostCategoryNodes = doc.SelectNodes("bml:blog/bml:posts/bml:post[@id='" + postOneId + "']/bml:categories/bml:category", nsmgr);
			Assert.AreEqual(2, firstPostCategoryNodes.Count, "Expected first post to have 2 categories");

			XmlNodeList firstPostCommentsNodes = doc.SelectNodes("bml:blog/bml:posts/bml:post[@id='" + postOneId + "']/bml:comments/bml:comment", nsmgr);
			Assert.AreEqual(1, firstPostCommentsNodes.Count, "Expected first post to have 1 comment");

			XmlNodeList firstPostTrackbackNodes = doc.SelectNodes("bml:blog/bml:posts/bml:post[@id='" + postOneId + "']/bml:trackbacks/bml:trackback", nsmgr);
			Assert.AreEqual(1, firstPostTrackbackNodes.Count, "Expected first post to have 1 trackback");
		}

		[RowTest]
		[Row("first-post-id", "first-cat-id", "first-comment-id", "first-trackback-id", "second-post-id", "null")]
		[Row("1", "1", "1", "1", "2", "int")]
		[Row("1", "1", "2", "3", "4", "subtext")]
		public void WritesProperBlogMlWithIdConversion(string firstPostConvertedId, string firstCategoryConvertedId, string firstCommentConvertedId, string firstTrackbackConvertedId, string secondPostConvertedId, string conversionType)
		{
			//Set up ids
			string blogId = "blah-abc-000";
			string catOneId = "first-cat-id";
			string catTwoId = "blah-acd";
			string firstPostId = "first-post-id";
			string secondPostId = "second-post-id";
			string thirdPostId = "blah-blah";
			string commentOneId = "first-comment-id";
			string trackBackOneId = "first-trackback-id";

			IdConversionStrategy conversion = IdConversionStrategy.Empty;
			switch(conversionType)
			{
				case "int":
					conversion = new IntConversionStrategy();
					break;
				
				case "subtext":
					conversion = new SubtextConversionStrategy();
					break;
			}
			
			
			int pageSize = 2;

			#region Create a full instance of a blog in an object hierarchy
			DateTime dateCreated = DateTime.Now;
			BlogMLBlog blog = ObjectHydrator.CreateBlogInstance("the title", "the subtitle", "http://blog.example.com/", "phil", "test@example.com", dateCreated);
			BlogMLContext context = new BlogMLContext(blogId, false);
			ICollection<BlogMLCategory> categories = new Collection<BlogMLCategory>();
			categories.Add(ObjectHydrator.CreateCategoryInstance(catOneId, "category1", "Category 1 is the first", true, null, dateCreated, dateCreated));
			categories.Add(ObjectHydrator.CreateCategoryInstance(catTwoId, "category2", "Category 2 is the second", true, null, dateCreated, dateCreated));

			IPagedCollection<BlogMLPost> firstPage = new PagedCollection<BlogMLPost>();
			firstPage.Add(ObjectHydrator.CreatePostInstance(firstPostId, "post title 1", "http://blog.example.com/post100/", true, "Nothing important 1", dateCreated, dateCreated));
			firstPage[0].Categories.Add(catOneId);
			firstPage[0].Categories.Add(catTwoId);
			firstPage[0].Comments.Add(ObjectHydrator.CreateCommentInstance(commentOneId, "re: post", "http://blog.example.com/post100/#91", "You rock!", "test@example.com", "haacked", true, dateCreated, dateCreated));
			firstPage[0].Trackbacks.Add(ObjectHydrator.CreateTrackBackInstance(trackBackOneId, "re: title", "http://another.example.com/", true, dateCreated, dateCreated));
			firstPage.Add(ObjectHydrator.CreatePostInstance(secondPostId, "post title 2", "http://blog.example.com/post200/", true, "Nothing important 2", dateCreated, dateCreated));
			firstPage.MaxItems = 3;

			IPagedCollection<BlogMLPost> secondPage = new PagedCollection<BlogMLPost>();
			secondPage.Add(ObjectHydrator.CreatePostInstance(thirdPostId, "post title 3", "http://blog.example.com/post300/", true, "Nothing important 3", dateCreated, dateCreated));
			secondPage.MaxItems = 3;
			#endregion

			//Now setup expectations.
			MockRepository mocks = new MockRepository();
			IBlogMLProvider provider = (IBlogMLProvider)mocks.CreateMock(typeof(IBlogMLProvider));
			Expect.Call(provider.GetBlogMlContext()).Return(context);
			Expect.Call(provider.IdConversion).Return(conversion);
			Expect.Call(provider.GetBlog(blogId)).Return(blog);
			Expect.Call(provider.GetAllCategories(blogId)).Return(categories);
			Expect.Call(provider.PageSize).Return(pageSize);
			Expect.Call(provider.GetBlogPosts(blogId, 0, pageSize)).Return(firstPage);
			Expect.Call(provider.GetBlogPosts(blogId, 1, pageSize)).Return(secondPage);
			mocks.ReplayAll();

			//TODO: simplify when BlogML bug is fixed.
			StringBuilder builder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(builder);
			XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);

			BlogMLWriter writer = BlogMLWriter.Create(provider);
			writer.Write(xmlWriter);

			mocks.VerifyAll();

			//Verify blog.
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(builder.ToString());
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/09/BlogML");

			XmlNode firstPostNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='" + firstPostConvertedId + "']", nsmgr);
			Assert.IsNotNull(firstPostNode, "Could not find post node with expected converted id" + firstPostConvertedId);

			XmlNode secondPostNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='" + secondPostConvertedId + "']", nsmgr);
			Assert.IsNotNull(secondPostNode, "Could not find post node with expected converted id" + secondPostConvertedId);
			
			XmlNode firstCategoryNode = doc.SelectSingleNode("bml:blog/bml:categories/bml:category[@id='" + firstCategoryConvertedId + "']", nsmgr);
			Assert.IsNotNull(firstCategoryNode, "Could not find first category node with id " + firstCategoryConvertedId);
			
			XmlNode firstPostFirstCategoryNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='" + firstPostConvertedId + "']/bml:categories/bml:category[@ref='" + firstCategoryConvertedId + "']", nsmgr);
			Assert.IsNotNull(firstPostFirstCategoryNode, "Could not find first post category reference to first category.");

			XmlNode firstPostCommentsNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='" + firstPostConvertedId + "']/bml:comments/bml:comment[@id='" + firstCommentConvertedId + "']", nsmgr);
			Assert.IsNotNull(firstPostCommentsNode, "Could not find first comment of the first post. Expected comment to have id: " + firstCommentConvertedId);

			XmlNode firstPostTrackbackNode = doc.SelectSingleNode("bml:blog/bml:posts/bml:post[@id='" + firstPostConvertedId + "']/bml:trackbacks/bml:trackback[@id='" + firstTrackbackConvertedId + "']", nsmgr);
			Assert.IsNotNull(firstPostTrackbackNode, "Could not find trackback with id " + firstTrackbackConvertedId);
		}

		/// <summary>
		/// We expect this test to convert ids to ints.
		/// </summary>
		[Test]
		public void WritesProperBlogMlUsingIntStrategy()
		{

		}
	}
}
