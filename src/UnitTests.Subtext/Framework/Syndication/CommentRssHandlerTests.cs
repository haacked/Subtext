using System;
using System.Xml;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Syndication;
using Subtext.Framework.Web.HttpModules;

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
        /// </para>
        /// </summary>
        [Test]
        [RollBack2]
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
            BlogRequest.Current.Blog = Config.GetBlog(hostName, string.Empty);
            Blog blog = Config.CurrentBlog;
            blog.Host = hostName;
            blog.Email = "Subtext@example.com";
            blog.RFC3229DeltaEncodingEnabled = false;


            DateTime dateCreated = DateTime.Now;
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication(blog, "Author", "Best post EVER", "testbody",
                                                                           null, dateCreated);
            var repository = new Mock<ObjectProvider>();
            repository.Setup(r => r.GetEntry(It.IsAny<int>(), true, true)).Returns(entry);

            int id = UnitTestHelper.Create(entry); //persist to db.

            string rssOutput = null;

            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.FakeSyndicationContext(blog, "/" + id + ".aspx", s => rssOutput = s);
            subtextContext.Setup(c => c.Repository).Returns(repository.Object);
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", id.ToString());
            Mock<UrlHelper> urlHelper = Mock.Get(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/whatever/entry");

            var handler = new RssCommentHandler(subtextContext.Object);
            handler.ProcessRequest();

            var doc = new XmlDocument();
            doc.LoadXml(rssOutput);

            XmlNodeList titleNodes = doc.SelectNodes("/rss/channel/title");
            Assert.IsNotNull(titleNodes, "The title node should not be null.");
            Assert.AreEqual("Best post EVER", titleNodes[0].InnerText,
                            "Did not get the expected value of the title node.");
        }
    }
}