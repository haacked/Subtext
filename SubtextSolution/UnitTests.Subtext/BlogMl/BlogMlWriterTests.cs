using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using BlogML;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.BlogMl;
using Subtext.BlogMl.Conversion;
using Subtext.BlogMl.Interfaces;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.BlogMl
{
	[TestFixture]
	public class BlogMlWriterTests
	{
		[Test]
		[ExpectedArgumentNullException]
		public void CreateRequiresProvider()
		{
			BlogMlWriter.Create(null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CreateRequiresContext()
		{
			MockRepository mocks = new MockRepository();
			IBlogMlProvider provider = (IBlogMlProvider)mocks.DynamicMock(typeof(IBlogMlProvider));
			mocks.ReplayAll();
			BlogMlWriter.Create(provider);
			mocks.VerifyAll();
		}
		
		[Test]
		public void CreateWithProperContextReturnsWriter()
		{
			BlogMlContext context = new BlogMlContext("8675309", false);
			
			MockRepository mocks = new MockRepository();
			IBlogMlProvider provider = (IBlogMlProvider)mocks.CreateMock(typeof(IBlogMlProvider));
			Expect.Call(provider.GetBlogMlContext()).Return(context);
			Expect.Call(provider.IdConversion).Return(IdConversionStrategy.Empty);
			mocks.ReplayAll();
			BlogMlWriter writer = BlogMlWriter.Create(provider);
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
			BlogMlBlog blog = new BlogMlBlog("the title", ContentTypes.Base64, "the subtitle", ContentTypes.Html, "http://blog.example.com/", "phil", "test@example.com", dateCreated);
			BlogMlContext context = new BlogMlContext(blogId, false);
			ICollection<IBlogMlCategory> categories = new Collection<IBlogMlCategory>();
			categories.Add(new BlogMlCategory(catOneId, "category1", ContentTypes.Html, "Category 1 is the first", true, null, dateCreated, dateCreated));
			categories.Add(new BlogMlCategory(catTwoId, "category2", ContentTypes.Html, "Category 2 is the second", true, null, dateCreated, dateCreated));

			IPagedCollection<IBlogMlPost> firstPage = new PagedCollection<IBlogMlPost>();
			firstPage.Add(new BlogMlPost(postOneId, "post title 1", ContentTypes.Xhtml, "http://blog.example.com/post100/", true, "Nothing important 1", dateCreated, dateCreated));
			firstPage[0].CategoryIds.Add(catOneId);
			firstPage[0].CategoryIds.Add(catTwoId);
			firstPage[0].Comments.Add(new BlogMlComment(commentOneId, "re: post", ContentTypes.Xhtml, "http://blog.example.com/post100/#91", "You rock!", ContentTypes.Xhtml, "test@example.com", "haacked", true, dateCreated, dateCreated));
			firstPage[0].Trackbacks.Add(new BlogMlTrackback(trackBackOneId, "re: title", ContentTypes.Text, "http://another.example.com/", true, dateCreated, dateCreated));
			firstPage.Add(new BlogMlPost(postTwoId, "post title 2", ContentTypes.Xhtml, "http://blog.example.com/post200/", true, "Nothing important 2", dateCreated, dateCreated));
			firstPage.MaxItems = 3;

			IPagedCollection<IBlogMlPost> secondPage = new PagedCollection<IBlogMlPost>();
			secondPage.Add(new BlogMlPost(postThreeId, "post title 3", ContentTypes.Xhtml, "http://blog.example.com/post300/", true, "Nothing important 3", dateCreated, dateCreated));
			secondPage.MaxItems = 3;
			#endregion

			//Now setup expectations.
			MockRepository mocks = new MockRepository();
			IBlogMlProvider provider = (IBlogMlProvider)mocks.CreateMock(typeof(IBlogMlProvider));
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

			BlogMlWriter writer = BlogMlWriter.Create(provider);
			writer.Write(xmlWriter);

			mocks.VerifyAll();
		
			//Verify blog.
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(builder.ToString());
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/01/BlogML");

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
			BlogMlBlog blog = new BlogMlBlog("the title", ContentTypes.Base64, "the subtitle", ContentTypes.Html, "http://blog.example.com/", "phil", "test@example.com", dateCreated);
			BlogMlContext context = new BlogMlContext(blogId, false);
			ICollection<IBlogMlCategory> categories = new Collection<IBlogMlCategory>();
			categories.Add(new BlogMlCategory(catOneId, "category1", ContentTypes.Html, "Category 1 is the first", true, null, dateCreated, dateCreated));
			categories.Add(new BlogMlCategory(catTwoId, "category2", ContentTypes.Html, "Category 2 is the second", true, null, dateCreated, dateCreated));

			IPagedCollection<IBlogMlPost> firstPage = new PagedCollection<IBlogMlPost>();
			firstPage.Add(new BlogMlPost(firstPostId, "post title 1", ContentTypes.Xhtml, "http://blog.example.com/post100/", true, "Nothing important 1", dateCreated, dateCreated));
			firstPage[0].CategoryIds.Add(catOneId);
			firstPage[0].CategoryIds.Add(catTwoId);
			firstPage[0].Comments.Add(new BlogMlComment(commentOneId, "re: post", ContentTypes.Xhtml, "http://blog.example.com/post100/#91", "You rock!", ContentTypes.Xhtml, "test@example.com", "haacked", true, dateCreated, dateCreated));
			firstPage[0].Trackbacks.Add(new BlogMlTrackback(trackBackOneId, "re: title", ContentTypes.Text, "http://another.example.com/", true, dateCreated, dateCreated));
			firstPage.Add(new BlogMlPost(secondPostId, "post title 2", ContentTypes.Xhtml, "http://blog.example.com/post200/", true, "Nothing important 2", dateCreated, dateCreated));
			firstPage.MaxItems = 3;

			IPagedCollection<IBlogMlPost> secondPage = new PagedCollection<IBlogMlPost>();
			secondPage.Add(new BlogMlPost(thirdPostId, "post title 3", ContentTypes.Xhtml, "http://blog.example.com/post300/", true, "Nothing important 3", dateCreated, dateCreated));
			secondPage.MaxItems = 3;
			#endregion

			//Now setup expectations.
			MockRepository mocks = new MockRepository();
			IBlogMlProvider provider = (IBlogMlProvider)mocks.CreateMock(typeof(IBlogMlProvider));
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

			BlogMlWriter writer = BlogMlWriter.Create(provider);
			writer.Write(xmlWriter);

			mocks.VerifyAll();

			//Verify blog.
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(builder.ToString());
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
			nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/01/BlogML");

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
