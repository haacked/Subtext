using System;
using System.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Syndication;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Syndication
{
	/// <summary>
	/// Tests the CommentRSS HttpHandler.
	/// </summary>
	[TestFixture]
	public class CommentRssHandlerTests
	{
		/// <summary>
		/// <para>
		/// Makes sure that the CommentRssHandler produces a valid RSS feed even if 
		/// the entry has no feed items.
		/// This test is in response to case [1446934] 
		/// </para>
		/// <para>
		/// https://sourceforge.net/tracker/index.php?func=detail&amp;aid=1446934&amp;group_id=137896&amp;atid=739979"
		/// </para>
		/// </summary>
		[Test]
		[RollBack]
		public void CommentRssHandlerProducesValidEmptyFeed()
		{
			string hostName = UnitTestHelper.GenerateUniqueHostname();
            //BlogInfo blog = new BlogInfo {
            //    Host = hostName,
            //    Email = "Subtext@example.com",
            //    RFC3229DeltaEncodingEnabled = false,
            //};
            int blogId = Config.CreateBlog("Test", "username", "password", hostName, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(hostName, string.Empty);
            Blog blog = Config.CurrentBlog;
            blog.Host = hostName;
            blog.Email = "Subtext@example.com";
            blog.RFC3229DeltaEncodingEnabled = false;

			DateTime dateCreated = DateTime.Now;
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication(blog, "Author", "Best post EVER", "testbody", null, dateCreated);
			int id = Entries.Create(entry); //persist to db.

			RssCommentHandler handler = new RssCommentHandler();
            string rssOutput = null;

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.FakeSyndicationContext(blog, "/" + id + ".aspx", s => rssOutput = s);
            var urlHelper = Mock.Get<UrlHelper>(subtextContext.Object.UrlHelper);
            urlHelper.Expect(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/whatever/entry");

			handler.ProcessRequest(subtextContext.Object);

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(rssOutput);
			
			XmlNodeList titleNodes = doc.SelectNodes("/rss/channel/title");
			Assert.IsNotNull(titleNodes, "The title node should not be null.");
			Assert.AreEqual("Best post EVER", titleNodes[0].InnerText, "Did not get the expected value of the title node.");
		}
	}
}
