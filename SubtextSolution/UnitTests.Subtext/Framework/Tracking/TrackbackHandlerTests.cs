using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Tracking;
using Moq;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using System.Collections.Specialized;

namespace UnitTests.Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackbackHandler.
	/// </summary>
	[TestFixture]
	public class TrackbackHandlerTests
	{
		[Test]
		[RollBack]
        public void ProcessRequest_WithTrackbacksDisabled_ReturnEmptyResponse()
		{
            //arrange
            UnitTestHelper.SetupBlog();
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			int id = UnitTestHelper.Create(entry);

            Blog blog = Config.CurrentBlog;
			blog.TrackbacksEnabled = false;
            var handler = new TrackBackHandler();

            var subtextContext = new Mock<ISubtextContext>();
            var writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", id.ToString());

            //act
            handler.ProcessRequest(subtextContext.Object);

            //assert
            Assert.AreEqual(string.Empty, writer.ToString());
		}
		
		/// <summary>
		/// Sends an RSS Snippet for requests made using the "GET" http verb.
		/// </summary>
		[Test]
		[RollBack]
        public void ProcessRequest_WithGetRequest_SendsRssResponse()
		{
            //arrange
            UnitTestHelper.SetupBlog();
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "this is the title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			int id = UnitTestHelper.Create(entry);

            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = true;
            var handler = new TrackBackHandler();

            var subtextContext = new Mock<ISubtextContext>();
            var writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", id.ToString());
            var urlHelper = Mock.Get<UrlHelper>(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.TrackbacksUrl(It.IsAny<int>())).Returns("/whatever/trackback");

			//act
            handler.ProcessRequest(subtextContext.Object);

            //assert
            Assert.IsTrue(writer.ToString().Contains("this is the title"));
		}
		
		/// <summary>
		/// Sends an error message if the id in the url does not match an existing entry.
		/// </summary>
		[Test]
		[RollBack]
		public void ProcessRequest_WithInvalidEntryId_SendsErrorResponse()
		{
            //arrange
            UnitTestHelper.SetupBlog();
            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = true;
            var handler = new TrackBackHandler();

            var subtextContext = new Mock<ISubtextContext>();
            var writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", int.MaxValue.ToString());
            var urlHelper = Mock.Get<UrlHelper>(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.TrackbacksUrl(It.IsAny<int>())).Returns("/whatever/trackback");

            //act
            handler.ProcessRequest(subtextContext.Object);

            //assert
            Assert.IsTrue(writer.ToString().Contains("EntryID is invalid or missing"));
		}
		
		/// <summary>
		/// Checks the error message returned when the trackback URL does not have an entry id.
		/// </summary>
		[Test]
		[RollBack]
		public void ProcessRequest_WithoutEntryIdInRouteData_SendsErrorResponse()
		{
            //arrange
            UnitTestHelper.SetupBlog();
            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = true;
            var handler = new TrackBackHandler();

            var subtextContext = new Mock<ISubtextContext>();
            var writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            var urlHelper = Mock.Get<UrlHelper>(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.TrackbacksUrl(It.IsAny<int>())).Returns("/whatever/trackback");

            //act
            handler.ProcessRequest(subtextContext.Object);

            //assert
			Assert.IsTrue(writer.ToString().Contains("EntryID is invalid or missing"));
		}
		
        /// <summary>
		/// Makes sure the HTTP handler used to handle trackbacks handles a proper trackback request 
		/// by creating a trackback record in the local system.
		/// </summary>
        [Test]
        [RollBack]
        public void ProcessRequest_WithValidTrackback_CreatesTracbackRecordInDatabase()
        {
            //arrange
            UnitTestHelper.SetupBlog();
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "this is the title", "body");
            entry.DateCreated = entry.DateSyndicated = entry.DateModified = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            int id = UnitTestHelper.Create(entry);

            Blog blog = Config.CurrentBlog;
            blog.TrackbacksEnabled = true;
            var handler = new TrackBackHandler();
            handler.SourceVerification += delegate(object sender, SourceVerificationEventArgs e) {
                e.Verified = true;
            };

            var subtextContext = new Mock<ISubtextContext>();
            var writer = subtextContext.FakeSubtextContextRequest(blog, "/trackbackhandler", "/", string.Empty);
            subtextContext.Setup(c => c.Repository).Returns(ObjectProvider.Instance());
            subtextContext.Object.RequestContext.RouteData.Values.Add("id", id.ToString());
            var urlHelper = Mock.Get<UrlHelper>(subtextContext.Object.UrlHelper);
            urlHelper.Setup(u => u.EntryUrl(It.IsAny<Entry>())).Returns("/whatever/entry");
            urlHelper.Setup(u => u.TrackbacksUrl(It.IsAny<int>())).Returns("/whatever/trackback");
            var httpContext = Mock.Get<HttpContextBase>(subtextContext.Object.RequestContext.HttpContext);
            httpContext.Setup(c => c.Request.HttpMethod).Returns("POST");

            var form = new NameValueCollection();
            form["title"] = entry.Title;
            form["excert"] = entry.Body;
            form["url"] = "http://myblog.example.com/";
            form["blog_name"] = "Random Blog";
            
            httpContext.Setup(c => c.Request.Form).Returns(form);

            //act
            handler.ProcessRequest(subtextContext.Object);

            //assert
            ICollection<FeedbackItem> trackbacks = Entries.GetFeedBack(entry);
            Assert.AreEqual(1, trackbacks.Count, "We expect to see the one feedback we just created.");
            Assert.AreEqual("this is the title", trackbacks.First().Title);
        }
	}
}
