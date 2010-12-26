using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Tracking;

namespace UnitTests.Subtext.Framework.Tracking
{
    /// <summary>
    /// Summary description for TrackbackHandler.
    /// </summary>
    [TestFixture]
    public class TrackbackHandlerTests
    {
        [Test]
        [RollBack2]
        public void ProcessRequest_WithTrackbacksDisabled_ReturnEmptyResponse()
        {
            //arrange
            UnitTestHelper.SetupBlog();
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
            entry.DateCreated =
                entry.DateSyndicated =
                entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int id = UnitTestHelper.Create(entry);
            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = false;
            var subtextContext = new Mock<ISubtextContext>();
            StringWriter writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", id.ToString());
            var handler = new TrackBackHandler(subtextContext.Object);

            //act
            handler.ProcessRequest();

            //assert
            Assert.AreEqual(string.Empty, writer.ToString());
        }

        /// <summary>
        /// Sends an RSS Snippet for requests made using the "GET" http verb.
        /// </summary>
        [Test]
        [RollBack2]
        public void ProcessRequest_WithGetRequest_SendsRssResponse()
        {
            //arrange
            UnitTestHelper.SetupBlog();
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "this is the title", "body");
            entry.DateCreated =
                entry.DateSyndicated =
                entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int id = UnitTestHelper.Create(entry);

            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = true;

            var subtextContext = new Mock<ISubtextContext>();
            StringWriter writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", id.ToString());
            Mock<UrlHelper> urlHelper = Mock.Get(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.TrackbacksUrl(It.IsAny<int>())).Returns("/whatever/trackback");
            subtextContext.SetupBlog(blog);
            var handler = new TrackBackHandler(subtextContext.Object);

            //act
            handler.ProcessRequest();

            //assert
            Assert.IsTrue(writer.ToString().Contains("this is the title"));
        }

        /// <summary>
        /// Sends an error message if the id in the url does not match an existing entry.
        /// </summary>
        [Test]
        [RollBack2]
        public void ProcessRequest_WithInvalidEntryId_SendsErrorResponse()
        {
            //arrange
            UnitTestHelper.SetupBlog();
            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = true;
            var subtextContext = new Mock<ISubtextContext>();
            StringWriter writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", int.MaxValue.ToString());
            Mock<UrlHelper> urlHelper = Mock.Get(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.TrackbacksUrl(It.IsAny<int>())).Returns("/whatever/trackback");
            subtextContext.SetupBlog(blog);
            var handler = new TrackBackHandler(subtextContext.Object);

            //act
            handler.ProcessRequest();

            //assert
            Assert.IsTrue(writer.ToString().Contains("EntryId is invalid or missing"));
        }

        /// <summary>
        /// Checks the error message returned when the trackback URL does not have an entry id.
        /// </summary>
        [Test]
        [RollBack2]
        public void ProcessRequest_WithoutEntryIdInRouteData_SendsErrorResponse()
        {
            //arrange
            UnitTestHelper.SetupBlog();
            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = true;
            var subtextContext = new Mock<ISubtextContext>();
            StringWriter writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            Mock<UrlHelper> urlHelper = Mock.Get(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.TrackbacksUrl(It.IsAny<int>())).Returns("/whatever/trackback");
            subtextContext.SetupBlog(blog);
            var handler = new TrackBackHandler(subtextContext.Object);

            //act
            handler.ProcessRequest();

            //assert
            Assert.IsTrue(writer.ToString().Contains("EntryId is invalid or missing"));
        }

        /// <summary>
        /// Makes sure the HTTP handler used to handle trackbacks handles a proper trackback request 
        /// by creating a trackback record in the local system.
        /// </summary>
        [Test]
        [RollBack2]
        public void ProcessRequest_WithValidTrackback_CreatesTracbackRecordInDatabase()
        {
            //arrange
            UnitTestHelper.SetupBlog();
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "this is the title", "body");
            entry.DateCreated =
                entry.DateSyndicated =
                entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int id = UnitTestHelper.Create(entry);
            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = true;
            var subtextContext = new Mock<ISubtextContext>();
            StringWriter writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", id.ToString());
            subtextContext.SetupBlog(blog);
            var handler = new TrackBackHandler(subtextContext.Object);
            handler.SourceVerification += (sender, e) => e.Verified = true;
            Mock<UrlHelper> urlHelper = Mock.Get(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/whatever/entry");
            urlHelper.Setup(u => u.TrackbacksUrl(It.IsAny<int>())).Returns("/whatever/trackback");
            Mock<HttpContextBase> httpContext = Mock.Get(subtextContext.Object.RequestContext.HttpContext);
            httpContext.Setup(c => c.Request.HttpMethod).Returns("POST");

            var form = new NameValueCollection();
            form["title"] = entry.Title;
            form["excert"] = entry.Body;
            form["url"] = "http://myblog.example.com/";
            form["blog_name"] = "Random Blog";

            httpContext.Setup(c => c.Request.Form).Returns(form);

            //act
            handler.ProcessRequest();

            //assert
            ICollection<FeedbackItem> trackbacks = ObjectProvider.Instance().GetFeedbackForEntry(entry);
            Assert.AreEqual(1, trackbacks.Count, "We expect to see the one feedback we just created.");
            Assert.AreEqual("this is the title", trackbacks.First().Title);
        }
    }
}