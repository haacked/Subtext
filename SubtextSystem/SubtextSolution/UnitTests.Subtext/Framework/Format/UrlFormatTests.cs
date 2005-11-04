using System;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

namespace UnitTests.Subtext.Framework.Format
{
	/// <summary>
	/// Summary description for UrlFormatTests.
	/// </summary>
	[TestFixture]
	public class UrlFormatTests
	{
		string _hostName;

		/// <summary>
		/// Makes sure the method GetEditLink distringuishes between a post and article.
		/// </summary>
		[Test]
		public void GetEditLinkDistringuishesBetweenPostAndArticle()
		{
			Assert.IsTrue(Config.CreateBlog("", "username", "password", _hostName, string.Empty));

			Entry postEntry = new Entry(PostType.BlogPost);
			postEntry.EntryID = 123;

			string editPostUrl = UrlFormats.GetEditLink(postEntry);
			Assert.AreEqual("~/Admin/EditPosts.aspx?PostID=123", editPostUrl, "Expected blog post to go to EditPosts.aspx");

			Entry articleEntry = new Entry(PostType.Story);
			articleEntry.EntryID = 456;
			string editArticleUrl = UrlFormats.GetEditLink(articleEntry);
			Assert.AreEqual("~/Admin/EditArticles.aspx?PostID=456", editArticleUrl, "Expected blog post to go to EditPosts.aspx");


		}

		[SetUp]
		public void SetUp()
		{
			_hostName = System.Guid.NewGuid().ToString().Replace("-", "") + ".com";
			UnitTestHelper.SetHttpContextWithBlogRequest(_hostName, string.Empty);
		}

		[TearDown]
		public void TearDown()
		{
		}
	}
}
